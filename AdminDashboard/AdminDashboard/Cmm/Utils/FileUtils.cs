using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DSELN.Cmm.Utils
{
    public static class FileUtils
    {
        public static long GetFileKB(long fileLength)
        {
            double length = fileLength / 1024;
            long rtn = (long)Math.Ceiling(length);
            return (rtn == 0 ? 1 : rtn);
        }


        public static string GetBaseDir(string section)
        {
            return GetBaseDir(section, "");
        }

        public static string GetBaseDir(string section, string subDir)
        {
            //"ExpResultDir": { // 실험결과 파일 경로 (dev 부분을 개발자 환경에 맞춰서 변경하십시오.)
            //    "dev.ExpResultDir": "D:\\FILES\\ELN_LAS\\",
            //    "test.ExpResultDir": "C:\\ELN_LAS\\",
            //    "live.ExpResultDir": "C:\\ELN_LAS\\"
            //  },

            string baseDir = "";
            string separator = Path.DirectorySeparatorChar.ToString();

            if (subDir.Equals(string.IsNullOrEmpty))
            {
                baseDir = ConfigUtil.getSectionValue(section + ":" + section) + separator;
            }
            else
            {
                baseDir = ConfigUtil.getSectionValue(section + ":" + section) + separator + subDir + separator;
            }

            return baseDir.Replace(separator + separator, separator);
        }



        static List<FileInfo> GetFiles(string path, bool recursive = false)
        {
            List<FileInfo> files = new List<FileInfo>();

            DirectoryInfo di = new DirectoryInfo(path);

            files.AddRange(di.GetFiles());
            if (recursive)
            {
                var directories = di.GetDirectories();

                if (directories.Length > 0)

                    foreach (var dir in directories)
                    {
                        files.AddRange(GetFiles(dir.FullName, true));
                    }
            }

            return files;
        }

        static List<string> OrderByCreation(List<FileInfo> files, bool descend = false)
        {
            IOrderedEnumerable<FileInfo> orderedfiles = null;
            orderedfiles = descend
                ? files.OrderByDescending(item => item.CreationTime)
                : files.OrderBy(item => item.CreationTime);
            return orderedfiles.Select(item => item.FullName).ToList();
        }
        public static string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }
}

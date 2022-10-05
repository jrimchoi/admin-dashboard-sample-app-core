using System;
using System.IO;
using System.Net;

namespace DSELN.Cmm.Utils
{
    public class FTPUtil
    {
        public static void Upload(string path, string fileName)
        {
            // Get the object used to communicate with the server.
            string ftpBaseUrl = ConfigUtil.getSectionValue("AnalysisTemplate:FTP_BASE_Url");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpBaseUrl + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("elnuser", "Qwer1234");

            // Copy the contents of the file to the request stream.
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(requestStream);
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        Console.WriteLine($"Upload File Complete, status Code : {response.StatusCode}");
                    }
                }
            }
        }

        public static void FtpUpload(string ftpurl, string path, string filename)
        {
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftpurl + filename);
            ftpClient.Credentials = new System.Net.NetworkCredential("elnuser", "Qwer1234");
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            ftpClient.ContentLength = fi.Length;
            byte[] buffer = new byte[4097];
            int bytes = 0;
            int total_bytes = (int)fi.Length;
            System.IO.FileStream fs = fi.OpenRead();
            System.IO.Stream rs = ftpClient.GetRequestStream();
            while (total_bytes > 0)
            {
                bytes = fs.Read(buffer, 0, buffer.Length);
                rs.Write(buffer, 0, bytes);
                total_bytes = total_bytes - bytes;
            }
            //fs.Flush();
            fs.Close();
            rs.Close();
            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            string value = uploadResponse.StatusDescription;
            uploadResponse.Close();
        }


    }
}

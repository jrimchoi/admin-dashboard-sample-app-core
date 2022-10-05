using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Serilog;
using Spire.Xls;

namespace DSELN.Cmm.Utils
{
    public static class Utils
    {

        public static string PrefixTabs(int cnt)
        {
            string prefix = "";
            for (int i = 0; i < cnt; i++)
            {
                prefix += "\t";
            }
            return prefix;
        }

        public static string GetAppMode()
        {
            return ConfigUtil.getSectionValue("Mode");
        }

        public static string GetAppSettiong(string section, bool isMode)
        {
            //string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var builder = new ConfigurationBuilder()
            //                    .SetBasePath(Directory.GetCurrentDirectory())
            //                    .AddJsonFile($"appsettings.{environment}.json");
            //var config = builder.Build();
            //var mode = config.GetSection("Mode").Value;

            //var value = config.GetValue<string>(section + ":" + (isMode ? mode + "." : "") + section);   // table 의 서버 기준 path 를 mode 기준으로 변경 

            return ConfigUtil.getSectionValue(section);
        }


        // null 대체값 리턴  
        public static T IsNull<T>(this T v1, T defaultValue)
        {
            return v1 == null ? defaultValue : v1;
        }

        // sha512 hashkey  <-- important !!!
        public static string GetHashKey(string value)
        {
            value = value + "hello";

            string hashkey = "";

            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                hashkey = hashedInputStringBuilder.ToString();
            }

            Console.WriteLine("GetHashKey : " + value + " : " + hashkey);

            return hashkey;
        }

        // 문자열 치환 WholeWord 
        public static string ReplaceWholeWord(string original, string pattern, string replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            return Regex.Replace(original, @"(?:(?<=^|\s)(?=\S|$)|(?<=^|\S)(?=\s|$))" + pattern + @"(?:(?<=^|\s)(?=\S|$)|(?<=^|\S)(?=\s|$))", replacement, RegexOptions.IgnoreCase);
        }


        // 날짜 문자열이 YYYYMMDD 형식 && 유효한 날짜 인지 검사한다.
        public static bool IsDate(string date)
        {
            bool result = false;

            // 날짜 포맷 체크 : YYYYMMDD, YYYY-MM-DD, YYYY.MM.DD 
            result = Regex.IsMatch(date, @"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$");
            if (!result) result = Regex.IsMatch(date, @"^(19|20)\d{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1])$");
            if (!result) result = Regex.IsMatch(date, @"^(19|20)\d{2}.(0[1-9]|1[012]).(0[1-9]|[12][0-9]|3[0-1])$");

            //Console.WriteLine("isDate 1 : " + date + " / " + result);  

            // 유효한 날짜 체크 
            DateTime tempDate;
            string formatDate = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);

            //Console.WriteLine("isDate 2 : " + result + " / " + formatDate );

            result = DateTime.TryParse(formatDate, out tempDate);

            //Console.WriteLine("isDate 3 : " + result + " / " + formatDate + " / " + tempDate);

            return result;
        }

        public static string GetDashDate(string date)
        {
            if ("".Equals(IsNull(date, "")))
            {
                return "";
            }

            return String.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
        }

        // Dashboard 조회기간 
        public static string GetDashBoardPeriod(string svid, int period)
        {
            if ("MAX_FR_DATE".Equals(svid))
            {
                period = 12;
                svid = svid.Replace("MAX_", "");
            }

            if ("MAX_TO_DATE".Equals(svid))
            {
                period = 12;
                svid = svid.Replace("MAX_", "");
            }

            DateTime now = DateTime.Now;
            var FR_DATE = now.AddMonths((-1) * (period - 1)).ToString("yyyyMM") + "01";  // ex) 3개월 : today (M), M-1, M-2 
            var TO_DATE = now.ToString("yyyyMMdd");

            if ("FR_DATE".Equals(svid))
            {
                return FR_DATE;
            }
            else if ("TO_DATE".Equals(svid))
            {
                return TO_DATE;
            }

            return "";
        }
        public static string[] GetExcelSheetImages(string excelFilePath)
        {
            // Workbook 인스턴스 생성
            Workbook workbook = new Workbook();
            // Stream으로 파일을 읽어온다.
            using (FileStream stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                // Workbook 인스턴스에 파일 Stream을 읽는다.
                workbook.LoadFromStream(stream);
            }
            string[] images = new string[workbook.Worksheets.Count];
            string dirPath = Path.GetDirectoryName(excelFilePath);
            int fileName = DateTime.Now.Millisecond;
            // Workbook의 worksheet별로 반복
            int i = 0;
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                string imageName = worksheet.Name;
                // worksheet를 pdf로 출력
                string imagePath = @$"{dirPath}\image-{fileName}.png";
                string croppedImagePath = @$"{dirPath}\image-{fileName}_cropped.png";
                worksheet.SaveToImage(imagePath);
                try
                {
                    TrimImage(imagePath, croppedImagePath);
                    images[i++] = GetBase64Image(croppedImagePath);

                }
                catch (Exception ex)
                {
                    Log.Error(ex.StackTrace);
                    images[i++] = GetBase64Image(imagePath);
                }
            }
            return images;
        }
        public static void TrimImage(string pngFilePath, string targetPngFilePath)
        {
            int threshhold = 250;


            int topOffset = 0;
            int bottomOffset = 0;
            int leftOffset = 0;
            int rightOffset = 0;
            Bitmap img = new Bitmap(pngFilePath);


            bool foundColor = false;
            // Get left bounds to crop
            for (int x = 1; x < img.Width && foundColor == false; x++)
            {
                for (int y = 1; y < img.Height && foundColor == false; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                leftOffset += 1;
            }


            foundColor = false;
            // Get top bounds to crop
            for (int y = 1; y < img.Height && foundColor == false; y++)
            {
                for (int x = 1; x < img.Width && foundColor == false; x++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                topOffset += 1;
            }


            foundColor = false;
            // Get right bounds to crop
            for (int x = img.Width - 1; x >= 1 && foundColor == false; x--)
            {
                for (int y = 1; y < img.Height && foundColor == false; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                rightOffset += 1;
            }


            foundColor = false;
            // Get bottom bounds to crop
            for (int y = img.Height - 1; y >= 1 && foundColor == false; y--)
            {
                for (int x = 1; x < img.Width && foundColor == false; x++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                bottomOffset += 1;
            }


            // Create a new image set to the size of the original minus the white space
            //Bitmap newImg = new Bitmap(img.Width - leftOffset - rightOffset, img.Height - topOffset - bottomOffset);

            Bitmap croppedBitmap = new Bitmap(img);
            croppedBitmap = croppedBitmap.Clone(
                            new Rectangle(leftOffset - 3, topOffset - 3, img.Width - leftOffset - rightOffset + 6, img.Height - topOffset - bottomOffset + 6),
                            System.Drawing.Imaging.PixelFormat.DontCare);


            // Get a graphics object for the new bitmap, and draw the original bitmap onto it, offsetting it do remove the whitespace
            //Graphics g = Graphics.FromImage(croppedBitmap);
            //g.DrawImage(img, 1 - leftOffset, 1 - rightOffset);
            croppedBitmap.Save(targetPngFilePath, ImageFormat.Png);
        }
        public static string GetBase64Image(string path)
        {
            byte[] b = System.IO.File.ReadAllBytes(path);
            return Convert.ToBase64String(b);
        }
    }
}

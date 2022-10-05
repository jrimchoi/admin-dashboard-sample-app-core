using DSELN.Models.Common;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DSELN.Cmm.Utils
{
    public class HttpUtil
    {
        /// <summary>
        /// Pipeline Pilot에 URL을 요청하여 파일을 다운받아서 byte[]를 리턴함
        /// 복호화한 파일을 받는 용도로 사용
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetHttpFileBytes(string url)
        {
            HttpClient _client = new HttpClient();
            byte[] buffer = null;
            try
            {
                _client.DefaultRequestHeaders.Add($"Authorization", "Basic c2NpdGVnaWNhZG1pbjpzY2l0ZWdpYw==");
                HttpResponseMessage task = await _client.GetAsync(url);
                Stream task2 = await task.Content.ReadAsStreamAsync();
                using (MemoryStream ms = new MemoryStream())
                {
                    await task2.CopyToAsync(ms);
                    buffer = ms.ToArray();
                }
                //File.WriteAllBytes("C:/**PATH_TO_SAVE**", buffer);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
            return buffer;
        }
        private static string GetLasAuthentication(string userId)
        {
            string authStr = $"{userId}@ELN";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(authStr);
            string authBase64String = "";
            try
            {
                authBase64String = System.Convert.ToBase64String(bytes);
                Console.WriteLine("authBase64String : " + authBase64String);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return authBase64String;
        }
        public static ResponseData GetDecryptedFile(string filePath, string folderName, string userId)
        {
            string baseUrl = ConfigUtil.getSectionValue("Las:Base_Url");
            string decryptUrl = ConfigUtil.getSectionValue("Las:Decrpyt_Url");
            var client = new RestClient(baseUrl);
            var request = new RestRequest(decryptUrl);
            request.Method = Method.Post;
            request.AddHeader("authentication", GetLasAuthentication(userId));
            request.AddHeader("equipId", userId);
            request.AddFile("FileToUpload", filePath);
            request.AddParameter("UserId", userId);
            request.AddParameter("EquipId", userId);
            request.AddParameter("FolderName", folderName);
            request.AddParameter("Title", "Decrypt");

            Console.WriteLine("Requesting Post url : " + baseUrl + decryptUrl);
            RestResponse response = client.Execute(request);
            string responseData = response.Content;
            if (responseData == null)
            {
                throw new Exception("response data is null");
            }
            ResponseData data = JsonConvert.DeserializeObject<ResponseData>(responseData);
            return data;
        }
        /// <summary>
        /// 특정경로에 있는 엑셀파일을 연구노트에 엑셀섹션으로 추가함.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> InsertExcelToExperiment(string url)
        {
            HttpClient _client = new HttpClient();
            string msg = null;
            try
            {
                _client.DefaultRequestHeaders.Add($"Authorization", "Basic c2NpdGVnaWNhZG1pbjpzY2l0ZWdpYw==");
                HttpResponseMessage task = await _client.GetAsync(url);
                msg = await task.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                msg = ex.Message;
            }
            return msg;
        }
    }
}
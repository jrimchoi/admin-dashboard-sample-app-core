using System.Text;

namespace DSELN.Models.Common
{
    public class ResponseData
    {
        public string result { get; set; }
        public string msg { get; set; }
        public string data { get; set; }

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"result : {result}").Append("\n");
            sb.Append($"message : {msg}").Append("\n");
            sb.Append($"data : {data}");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}

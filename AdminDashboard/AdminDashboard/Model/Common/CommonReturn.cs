namespace DSELN.Models.Common
{
    public class CommonReturn
    {
        public CommonReturn(string result,string msg)
        {
            this.RESULT = result;
            this.MESSAGE = msg;
        }
        public string RESULT { get; set; }
        public string MESSAGE { get; set; }

    }

    public enum Result
    {
        Success, Fail
    }
}

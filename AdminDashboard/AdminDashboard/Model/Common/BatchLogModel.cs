using DSELN.Cmm.Filters;

namespace DSELN.Models.Common
{
    public class BatchLogModel
    {
        public int LOG_ID { get; set; }
        [ValidateInputTextAttribute]
        public string PGM_NAME { get; set; }
        [ValidateInputTextAttribute]
        public string LOG_PATH { get; set; }
        [ValidateInputTextAttribute]
        public string RESULT { get; set; }

        public string MSG { get; set; }
        [ValidateInputTextAttribute]
        public string TARGET { get; set; }
        [ValidateInputTextAttribute]
        public string CREATED { get; set; }
        [ValidateInputTextAttribute]
        public string MODIFIED { get; set; }
    }
}

using DSELN.Cmm.Filters;

namespace DSELN.Models.Analysis
{
    public class ExperimentAnalysisAttributeModel
    {
        //SAMPLE_ID
        //    SAMPLE_ORDER
        //ATTR_ID
        //    ATTR_ORDER
        //ATTR_NAME
        //    TITLE
        //ATTR_TYPE
        //    ATTR_VALUE
        //FORMULA
        //    WIDTH
        //CREATED
        //    MODIFIED
        public int EAA_ID { get; set; }
        public int ANAL_ID { get; set; }
        public int SAMPLE_ID { get; set; }
        public int SAMPLE_ORDER { get; set; }
        public int ROW_NO { get; set; }
        public int COLUMN_NO { get; set; }
        //public int ATTR_ORDER { get; set; }

        [ValidateInputTextAttribute]
        public string ATTR_NAME { get; set; }

        [ValidateInputTextAttribute]
        public string TITLE { get; set; }

        [ValidateInputTextAttribute]
        public string ATTR_TYPE { get; set; }

        [ValidateInputTextAttribute]
        public string ATTR_VALUE { get; set; }

        [ValidateInputTextAttribute]
        public string FORMULA { get; set; }

        [ValidateInputTextAttribute]
        public string WIDTH { get; set; }

        [ValidateInputTextAttribute]
        public string CREATED { get; set; }

        [ValidateInputTextAttribute]
        public string MODIFIED { get; set; }
    }
}
using DSELN.Cmm.Filters;

namespace DSELN.Models.Analysis
{
    public class ExperimentAnalysisModel : BaseModel
    {
        public int ANAL_ID { get; set; }
        public int? EXP_ID { get; set; }
        public int? TMPL_ID { get; set; }

        [ValidateInputTextAttribute]
        public string? TITLE { get; set; }

        [ValidateInputTextAttribute]
        public string? EXP_NO { get; set; }

        [ValidateInputTextAttribute]
        public string? USE_DEPT { get; set; }

        [ValidateInputTextAttribute]
        public string? ANAL_ITEMS { get; set; }

        [ValidateInputTextAttribute]
        public string? TMPL_TYPE { get; set; }

        [ValidateInputTextAttribute]
        public string? PARSING_TYPE { get; set; }

        public string? PARSING_RESULT { get; set; }

        [ValidateInputTextAttribute]
        public string? STATE { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }

        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }

        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATOR_ID { get; set; }  // 권한체크용 
    }
}

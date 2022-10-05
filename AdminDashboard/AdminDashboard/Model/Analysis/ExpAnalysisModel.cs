using DSELN.Cmm.Filters;
using DSELN.Cmm.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Analysis
{
    public partial class ExpAnalysisSearch : BaseSearchModel // 조회조건용 모델
    {
        public int? EXP_ID { get; set; }

        [ValidateSearchTextAttribute]
        public string? TITLE { get; set; }
        [ValidateSearchTextAttribute]
        public string? USE_DEPT { get; set; }
        [ValidateSearchTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateSearchTextAttribute]
        public string? TMPL_TYPE { get; set; }
        [ValidateSearchTextAttribute]
        public string? ANAL_ITEMS { get; set; }
        [ValidateSearchTextAttribute]
        public string? ANAL_ITEMS_NAME { get; set; }
        [ValidateSearchTextAttribute]
        public string? ANAL_ID { get; set; }

        [DateVaild("FR_DATE")]  // 사용자정의 
        public string? FR_DATE { get; set; }
        [DateVaild("TO_DATE")]
        public string? TO_DATE { get; set; }
    }

    public partial class ExpAnalysisModel : BaseModel // 입력용 모델
    {
        public int? EXP_ID { get; set; }

        [ValidateInputTextAttribute]
        public string? ANAL_ID { get; set; }

        [ValidateInputTextAttribute]
        public string? TITLE { get; set; }

        [ValidateInputTextAttribute]
        public string? DESCRIPTION { get; set; }

        [ValidateInputTextAttribute]
        public string? USE_DEPT { get; set; }

        [ValidateInputTextAttribute]
        public string? PARSING_TYPE { get; set; }

        [ValidateInputTextAttribute]
        public string? TMPL_TYPE { get; set; }

        [ValidateInputTextAttribute]
        public string? STATE { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATOR_ID { get; set; }

        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }

        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }

        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }

        [ValidateInputTextAttribute]
        public string? EXP_NO { get; set; } = "";

        [ValidateInputTextAttribute]
        public string? ANAL_ITEMS { get; set; }

        [ValidateInputTextAttribute]
        public string? ANAL_ITEMS_NAME { get; set; }
    }
}

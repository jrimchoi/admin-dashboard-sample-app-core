using DSELN.Cmm.Filters;
using DSELN.Cmm.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Analysis
{
    public partial class ExpFragranceSearch : BaseSearchModel // 조회조건용 모델
    {
        [ValidateSearchTextAttribute]
        public int? SAMPLE_ID { get; set; }
        [ValidateSearchTextAttribute]
        public string? SAMPLE_NAME { get; set; }
        [ValidateSearchTextAttribute]
        public string? FR_RT { get; set; }
        [ValidateSearchTextAttribute]
        public string? TO_RT { get; set; }
        [ValidateSearchTextAttribute]
        public float? RT { get; set; }
        [ValidateSearchTextAttribute]
        public float? AREA_PCT { get; set; }
        [ValidateSearchTextAttribute]
        public string? FR_AREA_PCT { get; set; }
        [ValidateSearchTextAttribute]
        public string? TO_AREA_PCT { get; set; }
        [ValidateSearchTextAttribute]
        public string? LIBRARY_ID { get; set; }
        [ValidateSearchTextAttribute]
        public string? REF { get; set; }
        [ValidateSearchTextAttribute]
        public string? CAS { get; set; }
        [ValidateSearchTextAttribute]
        public float? QUAL { get; set; }
        [ValidateSearchTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateSearchTextAttribute]
        public string? MODIFIER { get; set; }
        [ValidateSearchTextAttribute]
        public int? ANAL_ID { get; set; }

        public string? CREATED { get; set; }

        [DateVaild("FR_DATE")]  // 사용자정의 
        public string? FR_DATE { get; set; }
        [DateVaild("TO_DATE")]
        public string? TO_DATE { get; set; }
    }

    public partial class ExpFragranceModel : BaseModel // 입력용 모델
    {
        public int? SAMPLE_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? SAMPLE_NAME { get; set; }
        [ValidateInputTextAttribute]
        public string? PK { get; set; }
        public float? RT { get; set; }
        public float? AREA_PCT { get; set; }
        [ValidateInputTextAttribute]
        public string? LIBRARY_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? REF { get; set; }
        [ValidateInputTextAttribute]
        public string? CAS { get; set; }
        public float? QUAL { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }
        public int? ANAL_ID { get; set; }
    }
}

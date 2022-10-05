using DSELN.Cmm.Filters;
using DSELN.Cmm.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Sample
{
    public class SampleModel : BaseModel
    {
        public int SAMPLE_ID { get; set; }
        public int ANAL_ID { get; set; }
        public int? TMPL_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? SAMPLE_NAME { get; set; }
        [ValidateInputTextAttribute]
        public string? FILE_PATH { get; set; }
        [ValidateInputTextAttribute]
        public string? USE_DEPT { get; set; }
        [ValidateInputTextAttribute]
        public string? TMPL_TYPE { get; set; }

        public int? SEQ_LINE { get; set; }
        public int VIAL { get; set; }
        [ValidateInputTextAttribute]
        public string? ACQ_METH { get; set; }
        public string? PEAKS { get; set; }
        [ValidateInputTextAttribute]
        public string? STATE { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }
        public Double? DILUTION { get; set; } = 0;
        [ValidateInputTextAttribute]
        public string? SAMPLE_TYPE { get; set; }

        public int ANALYSIS_REM_CNT { get; set; }  //  남은 건수 

        public string? VISCOSITY { get; set; }  // BRA 챠트용 
        public string? TEMPERATURE { get; set; } // BRA 챠트용 
        public string? TIME_MS { get; set; } // BRA 챠트용 
        public float? BOG { get; set; } // BRA 챠트용 
        public float? PEAK { get; set; } // BRA 챠트용 
        public float? FINAL { get; set; } // BRA 챠트용 
        public float? SB { get; set; } // BRA 챠트용 
        public float? BD { get; set; } // BRA 챠트용

        public int? EXP_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? FILE_FULL_PATH { get; set; } // 파일 조회용
        // LC_COMMON PDF Viewer 용
    }
    public class SampleSearch : BaseSearchModel
    {
        [ValidateSearchTextAttribute]
        public int SAMPLE_ID { get; set; }
        [ValidateSearchTextAttribute]
        public int ANAL_ID { get; set; }
        [ValidateSearchTextAttribute]
        public string? SAMPLE_NAME { get; set; } = "";

        [DateVaild("FR_DATE")]  // 사용자정의 
        public string? FR_DATE { get; set; } = "";
        [DateVaild("TO_DATE")]
        public string? TO_DATE { get; set; } = "";
    }

}
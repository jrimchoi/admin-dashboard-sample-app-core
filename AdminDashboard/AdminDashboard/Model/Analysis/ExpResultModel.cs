using DSELN.Cmm.Filters;
using DSELN.Cmm.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Analysis
{
    public partial class ExpResultSearch : BaseSearchModel // 조회조건용 모델
    {
        [ValidateSearchTextAttribute]
        public string?	EXP_ID	{ get; set; }
        [ValidateSearchTextAttribute]
        public string?	EQUIP_ID	{ get; set; }
        [ValidateSearchTextAttribute]
        public string?  USE_DEPT { get; set; }
        [ValidateSearchTextAttribute]
        public string?  STATE { get; set; }
        [ValidateSearchTextAttribute]
        public string?  CREATOR { get; set; }
        [ValidateSearchTextAttribute]
        public string?	TITLE	{ get; set; }
        [ValidateSearchTextAttribute]
        public string?	FILE_PATH	{ get; set; }
        [ValidateSearchTextAttribute]
        public string? FOLDER_NAME { get; set; }
        [ValidateSearchTextAttribute]
        public string?  TMPL_TYPE { get; set; }
        [ValidateSearchTextAttribute]
        public string?	PARSING_RESULT	{ get; set; }

        [DateVaild("FR_DATE")]  // 사용자정의 
        public string? FR_DATE { get; set; }
        [DateVaild("TO_DATE")]
        public string? TO_DATE { get; set; }
    }

    public partial class ExpResultModel : BaseModel // 입력용 모델
    {
        //[Required(ErrorMessage = "실험 ID는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? EXP_ID { get; set; }

        public string? EXP_ID_KEY { get; set; } // update / delete key

        [ValidateInputTextAttribute]
        public string? EQUIP_ID { get; set; }

        [ValidateInputTextAttribute]
        public string? TITLE { get; set; }
        [ValidateInputTextAttribute]
        public string? FILE_PATH { get; set; }
        [ValidateInputTextAttribute]
        public string? FOLDER_NAME { get; set; }
        public string? PARSING_LOG { get; set; }

        public DateTime? EXP_DATE { get; set; }

        //[Required(ErrorMessage = "사용부서는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? USE_DEPT { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; } = "";

        //[Required(ErrorMessage = "장비유형은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? PARSING_TYPE { get; set; }
        [ValidateInputTextAttribute]
        public string? STATE { get; set; }
        [ValidateInputTextAttribute]
        public string? TMPL_TYPE { get; set; }

        //[Required(ErrorMessage = "파싱결과는 필수입력사항입니다.")]
        //public string? PARSING_RESULT { get; set; } // 최적화를 위해 Peak Data 분리하여 SAMPLES 열로 변경
        public string? SAMPLES { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }
        [ValidateInputTextAttribute]
        public string? EQUIP { get; set; }

        //[Required(ErrorMessage = "템플릿 ID는 필수입력사항입니다.")]
        public int TMPL_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }

        [ValidateInputTextAttribute]
        public string? EXP_NO { get; set; }
        public int ANAL_ID { get; set; }

        // TB_EXPERIMENT_ANALYSIS 입력 모델 추가
        [ValidateInputTextAttribute]
        public string? ANAL_ITEMS { get; set; }

        public int? ID { get; set; } // 파싱 로그 모델용
        [ValidateInputTextAttribute]
        public string? TYPE { get; set; } // 파싱 로그 모델용
        [ValidateInputTextAttribute]
        public string? RESULT { get; set; } // 파싱 로그 모델용
        [ValidateInputTextAttribute]
        public string? FOLDER { get; set; } // 파싱 로그 모델용
        public string? MSG { get; set; } // 파싱 로그 모델용
    }
}
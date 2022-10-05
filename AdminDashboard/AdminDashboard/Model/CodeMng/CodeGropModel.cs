using DSELN.Cmm.Filters;
using DSELN.Cmm.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.CodeMng
{
    // 코드유형 
    public class CodeType : BaseModel
    {
        [Required(ErrorMessage = "코드유형코드는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? CD_TYP { get; set; }

        public string? CD_TYP_KEY { get; set; }

        [Required(ErrorMessage = "코드유형 한글명은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? CD_TYP_NM_KO { get; set; }

        [ValidateInputTextAttribute]
        public string? USE_YN { get; set; }

        [ValidateInputTextAttribute]
        public string? REM { get; set; }

        [ValidateInputTextAttribute]
        public string? CD_TYP_NM_EN { get; set; }

        [ValidateInputTextAttribute]
        public string? CD_TYP_NM_ZH { get; set; }

        [ValidateInputTextAttribute]
        public string? STS { get; set; }

        [ValidateInputTextAttribute]
        public string? SYS_ID { get; set; }

        // #### ZyHashKey 는 프로퍼티중 맨 마지막에 추가할것. mapping 순서 == property 순서  ###
        ////[ZyHashKeyValid("CD_TYP_KEY")]  not used...
        [ValidateInputTextAttribute]
        public string? ZyHashKey { get; set; }
    }

    public class CodeGroupMD : BaseModel
    {
        [ValidateInputTextAttribute]
        public string? GRP_CD { get; set; }
        public string? GRP_CD_KEY { get; set; }
        public CodeGroup? master { get; set; }
        public List<CodeDetail>? detail { get; set; }

    }

    public class CodeGroup : BaseModel
    {
        [Required(ErrorMessage = "코드그룹은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? GRP_CD { get; set; }
        public string? GRP_CD_KEY { get; set; }

        [Required(ErrorMessage = "코드유형은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? CD_TYP { get; set; }

        [Required(ErrorMessage = "코드그룹 한글명은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? GRP_CD_NM_KO { get; set; }

        [ValidateInputTextAttribute]
        public string? USE_YN { get; set; } = "N";

        [ValidateInputTextAttribute]
        public string? REM { get; set; }

        [ValidateInputTextAttribute]
        public string? GRP_CD_NM_EN { get; set; }

        [ValidateInputTextAttribute]
        public string? GRP_CD_NM_ZH { get; set; }

        [ValidateInputTextAttribute]
        public string? STS { get; set; }

    }

    public class CodeDetail : BaseModel
    {
        [Required(ErrorMessage = "코드그룹은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? GRP_CD { get; set; }
        public string? GRP_CD_KEY { get; set; }

        [Required(ErrorMessage = "상세코드는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? DTL_CD { get; set; }
        public string? DTL_CD_KEY { get; set; }

        [ValidateInputTextAttribute]
        public string? SYS_ID { get; set; }

        [Required(ErrorMessage = "상세코드 한글명은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? DTL_CD_NM_KO { get; set; }

        [ValidateInputTextAttribute]
        public string? USE_YN { get; set; }

        [ValidateInputTextAttribute]
        public string? REM { get; set; }

        [ValidateInputTextAttribute]
        public string? DTL_CD_NM_EN { get; set; }

        [ValidateInputTextAttribute]
        public string? DTL_CD_NM_CN { get; set; }

        [ValidateInputTextAttribute]
        public string? STS { get; set; }
        [ValidateInputTextAttribute]
        public string? SORT_ORD { get; set; }
        [ValidateInputTextAttribute]
        public string? CHILD_GRP_CD { get; set; }
    }

    public class CodeGroupSearch : BaseSearchModel  // 조회조건 
    {
        [ValidateSearchTextAttribute]
        public string? LANG_CD { get; set; }
        [ValidateSearchTextAttribute]
        public string? CD_TYP { get; set; }
        [ValidateSearchTextAttribute]
        public string? GRP_CD { get; set; }
    }



}

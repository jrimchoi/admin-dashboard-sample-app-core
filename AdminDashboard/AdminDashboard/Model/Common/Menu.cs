using DSELN.Cmm.Validations;
using System.ComponentModel.DataAnnotations;
using DSELN.Cmm.Filters;

namespace DSELN.Models.Common
{
    // 메뉴  
    public class Menu : BaseModel
    {
        [Required(ErrorMessage = "메뉴코드는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? MENU_CD { get; set; }

        [ValidateInputTextAttribute]
        public string? MENU_CD_KEY { get; set; }  // 업데이트용 key 

        [ValidateInputTextAttribute]
        public string? SYS_ID { get; set; }

        [Required(ErrorMessage = "상위메뉴코드는 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? UP_MENU_CD { get; set; }

        [ValidateInputTextAttribute]
        public string? REM { get; set; }
        [ValidateInputTextAttribute]
        public string? STS { get; set; }
        [ValidateInputTextAttribute]
        public string? LINK_URL { get; set; }
        [ValidateInputTextAttribute]
        public string? SORT_ORD { get; set; }
        [ValidateInputTextAttribute]
        public string? MENU_TYP { get; set; }
        [ValidateInputTextAttribute]
        public string? USE_YN { get; set; }
        [ValidateInputTextAttribute]
        public string? USR_CLS { get; set; }
        [ValidateInputTextAttribute]
        public string? MD_CLS { get; set; }
        [ValidateInputTextAttribute]
        public string? DISPLAY_YN { get; set; }
        [ValidateInputTextAttribute]
        public string? MENU_NM_EN { get; set; }

        [Required(ErrorMessage = "메뉴코드 한글명은 필수입력사항입니다.")]
        [ValidateInputTextAttribute]
        public string? MENU_NM_KO { get; set; }
        [ValidateInputTextAttribute]
        public string? MENU_NM_ZH { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATION_USER_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATION_DATE { get; set; }
        [ValidateInputTextAttribute]
        public string? LAST_UPDATE_USER_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? LAST_UPDATE_DATE { get; set; }
        [ValidateInputTextAttribute]
        public string? ROLE_CD { get; set; } = "";  //  role or auth  
    }

    // 메뉴  
    public class MenuSearch : BaseSearchModel
    {
        [ValidateSearchTextAttribute]
        public string? LANG_CD { get; set; }
        [ValidateSearchTextAttribute]
        public string? USE_YN { get; set; }
    }

}

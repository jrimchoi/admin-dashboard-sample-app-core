using DSELN.Cmm.Validations;
using DSELN.Cmm.Filters;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.DashBoard
{
     
    public class DashBoardSearch : BaseSearchModel
    {
        [Required(ErrorMessage = "기간은 필수입력사항입니다.")]
        [ValidateSearchTextAttribute]
        public string? PERIOD { get; set; }

        //[Required(ErrorMessage = "BU는 필수입력사항입니다.")]
        [ValidateSearchTextAttribute]
        public string? BU_CODE { get; set; }
        [ValidateSearchTextAttribute]
        public string? TEAM_CODE { get; set; }

        [ValidateSearchTextAttribute]
        public string? USER_NAME { get; set; }

        //// [Required(ErrorMessage = "연구원은 필수입력사항입니다.")]   // 대시보드(연구원) 에서 필수 controller 에서 체크하겠다.
        ///[ValidateSearchTextAttribute]
        public string? USER_ID { get; set; }

        [DateVaild("FR_DATE")]  // 사용자정의 
        public string? FR_DATE { get; set; } = "";

        [DateVaild("TO_DATE")]
        public string? TO_DATE { get; set; } = "";

        [ValidateSearchTextAttribute]
        public string? SEARCH_GBN { get; set; } = "LIST";

    }

}
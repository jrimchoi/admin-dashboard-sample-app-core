using DSELN.Cmm.Filters;
using DSELN.Models.Login;
using DSELN.Cmm.Helper;
using Serilog;
using DSELN.Cmm.Utils;

namespace DSELN.Models
{
    public class BaseSearchModel
    {

        public BaseSearchModel()
        {
            SessionInfo = SessionHelper.GetSessionModel(AppHttpContext.Current.Session);
            if (SessionInfo != null)
            {
                SessionInfo.PrintModelValues("Mapped....BaseSearchModel");
                SYS_ID = SessionInfo.SYS_ID;
            }
            else
            {
                if ("dev".Equals(Utils.GetAppMode())) // 개발환경 session filter 제거한 경우
                {
                    SessionInfo = new SessionModel();
                    SessionInfo.USER_ID = "10140220";
                    SessionInfo.USER_NM = "김용수";
                    SessionInfo.USE_DEPT = "BIO";
                }
            }
        }

        public SessionModel SessionInfo = new SessionModel();

        [ValidateSearchTextAttribute]
        public string? SYS_ID { get; set; }

        [ValidateSearchTextAttribute]
        public string? _PROC_TYPE { get; set; }
        [ValidateSearchTextAttribute]
        public string? _ROW_TYPE { get; set; }
        [ValidateSearchTextAttribute]
        public string? _POPUP_YN { get; set; } = "N"; // Popup 화면여부 
        [ValidateSearchTextAttribute]
        public string? _VIEW_MODE { get; set; } = ""; //  view mode or reg, edit mode 


        // 페이징 관련 항목 
        [ValidateSearchTextAttribute]
        public bool _PAGE_ABLE { get; set; }  = true;
        [ValidateSearchTextAttribute]
        public long _PAGE_INDEX { get; set; } = 1;
        [ValidateSearchTextAttribute]
        public long _PAGE_SIZE { get; set; } = 10; // 각 페이지당 행 개수  10행씩 

        [ValidateSearchTextAttribute]
        public long _FIRST_INDEX { get; set; } = 0;
        [ValidateSearchTextAttribute]
        public long _LAST_INDEX { get; set; } = 0;

        [ValidateSearchTextAttribute]
        public long _START_ROW { get; set; } = 0;
        [ValidateSearchTextAttribute]
        public long PAGE_TOT_ROWS { get; set; } = 0;

        [ValidateSearchTextAttribute]
        public string? Dummy1 { get; set; }
        [ValidateSearchTextAttribute]
        public string? Dummy2 { get; set; }
        [ValidateSearchTextAttribute]
        public string? Dummy3 { get; set; }

        public void PrintModelValues()  
        {
            PrintModelValues("");
        }
        public void PrintModelValues(string prefixStr)
        {
            string str = "";
            foreach (var item in this.GetType().GetProperties())
            {
                str += item.Name + "={" + item.GetValue(this) + "}, ";
            }

            if (SessionInfo != null)
            {
                // 세션정보 
                str += "SessionInfo={";
                foreach (var item in this.SessionInfo.GetType().GetProperties())
                {
                    str += item.Name + "={" + item.GetValue(this.SessionInfo) + "}, ";
                }
                str += "},";
            }
            else
            {
                str += "},";
            }


            Log.Debug(prefixStr + " : " + str);
        }
    }

}

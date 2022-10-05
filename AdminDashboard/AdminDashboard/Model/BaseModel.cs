using DSELN.Models.Login;
using DSELN.Cmm.Helper;
using Serilog;
using DSELN.Cmm.Utils;

namespace DSELN.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            SessionInfo = SessionHelper.GetSessionModel(AppHttpContext.Current.Session);
            if(SessionInfo != null)
            {
                //SessionInfo.PrintModelValues("Mapped....BaseModel");
                SYS_ID = SessionInfo.SYS_ID;
            }
            else
            {
                if("dev".Equals(Utils.GetAppMode())) // 개발환경 session filter 제거한 경우
                {
                    SessionInfo = new SessionModel();
                    SessionInfo.USER_ID = "10140220";
                    SessionInfo.USER_NM = "김용수";
                    SessionInfo.USE_DEPT = "BIO"; 
                }
            }

        }

        public SessionModel SessionInfo = new SessionModel();

        public string? SYS_ID { get; set; }  

        public string? _PROC_TYPE { get; set; }

        public string? _ROW_TYPE { get; set; }
        public string? PAGE_TOT_ROWS { get; set; }  // 조회 결과 전체행수 

        // Row Type 설정 I/U/D 
        public void SetRowType(int? keyValue)
        {
            string key = keyValue.ToString();   
            this.SetRowType(key);
        }

        // Row Type 설정 I/U/D 
        public void SetRowType(string keyValue)
        {
            if (string.IsNullOrEmpty(this._PROC_TYPE)) this._PROC_TYPE = "SAVE";  // default SAVE

            string rowType = this._ROW_TYPE;

            if (this._PROC_TYPE.Equals("DELETE"))  // 공통함수로..뺄것.... 
            {
                rowType = "D";
            }
            else if (this._PROC_TYPE.Equals("SAVE") && string.IsNullOrEmpty(keyValue))
            {
                rowType = "I";
            }
            else if (this._PROC_TYPE.Equals("SAVE") && !string.IsNullOrEmpty(keyValue))
            {
                rowType = "U";
            }
            else if (string.IsNullOrEmpty(keyValue))
            {
                TransactionHelper.SetRollbackOnly("_ROW_TYPE not defined.....");
            }

            this._ROW_TYPE = rowType;
        }

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


            //Log.Debug(prefixStr + " : " + str);
        }
    }

}

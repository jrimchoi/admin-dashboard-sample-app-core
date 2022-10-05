using System;
using System.ComponentModel.DataAnnotations;
using DSELN.Cmm.Helper;
using DSELN.Cmm.Validations;
using DSELN.Models.Common;

namespace DSELN.Models.Login
{
    // Student 저장/처리용 Model 
    public class SessionModel ////: BaseModel
    {

        public string? SYS_ID { get; set; }
        public string? USER_ID { get; set; }
        public string? USER_NM { get; set; }
        public string? LOGIN_DT { get; set; }
        public string? USER_ROLE { get; set; }

        public string? USER_BU_CD { get; set; }  // bu code   
        public string? USE_DEPT { get; set; }  //  bu code 와 동일한 개념 &&   USE_DEPT   != USER_DEPT_CD
        public string? USER_DEPT_CD { get; set; } // 부서 
        public string? USER_POSITION_CD { get; set; }  // 포지션  
        public string? BU_NAME { get; set; }  // BU

        public string? DEPT_NM { get; set; }
        public string? CLIENT_IP_ADDR { get; set; }

        public string? LANG_CD { get; set; } = "KO";  // 사용자 언어구분 KO, EN, ZE 

        public void PrintModelValues(string prefixStr)
        {
            string str = "";
            foreach (var item in this.GetType().GetProperties())
            {
                str += item.Name + "={" + item.GetValue(this) + "}, ";
            }

            Console.WriteLine(prefixStr + " : " + str);
        }
    }
}

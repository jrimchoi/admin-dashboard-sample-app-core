using DSELN.Models.Login;
using System.ComponentModel;

namespace DSELN.Cmm.Utils
{
    public static class Const
    {
        public const string SESSION_SYS_ID = "SESSION_SYS_ID";
        public const string SESSION_USER_ID = "SESSION_USER_ID";
        public const string SESSION_USER_NM = "SESSION_USER_NM";
        public const string SESSION_CLIENT_IP_ADDR = "SESSION_CLIENT_IP_ADDR";
        public const string SESSION_SESSION_MODEL = "SESSION_MODEL";
        public const string SESSION_USER_ROLE = "SESSION_USER_ROLE";
        public const string SESSION_USER_MENU = "SESSION_USER_MENU";

        public const string SESSION_USE_DEPT = "SESSION_USE_DEPT";           // bu code 와 동일한 개념. 
        public const string SESSION_USER_BU_CD = "SESSION_USER_BU_CD";   // bu code
        public const string SESSION_USER_DEPT_CD = "SESSION_USER_DEPT_CD";  // 사용자 부서 
        public const string SESSION_USER_POSITION_CD = "SESSION_USER_POSITION_CD";  // 사용자 직위 
        public const string SESSION_BU_NAME = "SESSION_BU_NAME";  // 사용자 BU 



        public const string Status        =  "_status";
        public const string Msg           = "_msg";
        public const string ErrorList     = "_errorList";

        public const string SUCC         = "1";  // 성공 
        public const string FAIL           = "0";  // 실패 

        public const string SERVER_ROLE_WEB = "LAS Web Server";
        public const string SERVER_ROLE_COLLECTION = "LAS Collection Server";
    }

    /*********************************************
    // Position 코드 
    *********************************************/
    public static class Position
    {
        public const string DIRECTOR = "소장";        // login dapper >> positioncode1 참조 
        public const string TEAM_LEADER = "팀장";  // login dapper >> positioncode1 참조 
    }

    /*********************************************
    // Role 코드 
    *********************************************/
    public static class Role
    {
        public const string ADMIN = "ADMIN";
        public const string USER = "USER";
    }

}

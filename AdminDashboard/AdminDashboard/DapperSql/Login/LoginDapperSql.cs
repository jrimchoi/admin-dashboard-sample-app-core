using Dapper;
using Dapper.Oracle;
using DSELN.Cmm.Helper;
using DSELN.Cmm.DataBase;
using DSELN.Models.Login;
using Quickwire.Attributes;
using System.Collections;
using System.Data;
using DSELN.Cmm.Utils;
using System;

namespace DSELN.DapperSql.Login
{

    public static class LoginDapperSql
    {

        public static SqlBuilder.Template GetLoginInfo(LoginModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
            //if (model.USER_ID.Length <= 6) model.USER_ID = "10" + model.USER_ID;  // ??? what ???
            string roleName = Role.USER;
            string strAdminUsers = ConfigUtil.getSectionValue("Las:Admin_User");
            string[] adimUserList = strAdminUsers.Split(",", StringSplitOptions.TrimEntries);
            if (adimUserList.AsList<string>().Contains(model.USER_ID))
            {
                roleName = Role.ADMIN;
            }
            var sql = builder.AddTemplate(@"
                WITH UM AS (
                        SELECT A.IMUSERID AS USER_ID 
                                  , A.NAME AS USER_NM 
                                  , 'a1234' AS PWD 
                                  , A.USE_DEPT AS USE_DEPT
                                  , A.BU_NAME AS BU_NAME
                                  , A.BU_CODE AS USER_BU_CD
                                  , A.DEPTCODE1 AS USER_DEPT_CD
                                  , A.POSITION1 AS USER_POSITION 
                                  , A.POSITIONCODE1 AS USER_POSITION_CD 
                                  , 'SYS10' AS SYS_ID  
                        FROM ELN_IF.VW_USER A 
                        WHERE 1=1
                            AND A.IMUSERID = :USER_ID    
                            AND (1=1 OR 'a1234' = :PWD) 
                ) 
                , RM AS (  /**** not used ****/ 
                    SELECT A.ID 
                              , A.ROLE 
                              , A.MEMBER_ID
                    FROM ELN_IF.TB_PLM_ROLE A 
                    WHERE 1 = 1 
                       AND A.MEMBER_ID LIKE '%' || :USER_ID || '%'
                )
                , RM2 AS (  /**** not used ****/ 
                    SELECT A.ID 
                              , A.ROLE 
                              , REPLACE(REPLACE(REPLACE(REPLACE(REGEXP_SUBSTR(A.MEMBER_ID, '[^,]+', 1, LEVEL), '" + "\"" + @"', ''), '[', ''), ']', ''), ' ', '') AS  USER_ID 
                    FROM RM A
                    CONNECT BY LEVEL <= LENGTH(REGEXP_REPLACE(A.MEMBER_ID, '[^,]+', '')) + 1
                )
                SELECT A.*
                          ,   '" + roleName + @"'  AS USER_ROLE   /*** Las:Admin_User로 관리 ***/ 
                          , TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') AS LOGIN_DT
                FROM UM A
                        , RM2 B
                WHERE A.USER_ID = B.USER_ID(+)

             ");

            // 동적 파라미터 적용 
            //builder.Where("A.USER_ID = :USER_ID  ");
            //builder.Where("A.PWD = :PWD  ");
            //builder.Where("A.USER_ID =  :BB" , new { BB = model.USER_ID});  // model 에 없는 파라미터 추가 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        public static SqlBuilder.Template GetHubLoginInfo(LoginModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
            if (model.USER_ID.Length <= 6) model.USER_ID = "10" + model.USER_ID;  // ??? what ???
            string roleName = Role.USER;
            string strAdminUsers = ConfigUtil.getSectionValue("Las:Admin_User");
            string[] adimUserList = strAdminUsers.Split(",", StringSplitOptions.TrimEntries);
            if (adimUserList.AsList<string>().Contains(model.USER_ID))
            {
                roleName = Role.ADMIN;
            }
            var sql = builder.AddTemplate(@"
                WITH UM AS (
                        SELECT A.IMUSERID AS USER_ID 
                                  , A.NAME AS USER_NM 
                                  , 'a1234' AS PWD 
                                  , A.USE_DEPT AS USE_DEPT 
                                  , A.BU_CODE AS USER_BU_CD
                                  , A.DEPTCODE1 AS USER_DEPT_CD
                                  , A.POSITION1 AS USER_POSITION 
                                  , A.POSITIONCODE1 AS USER_POSITION_CD 
                                  , 'SYS10' AS SYS_ID  
                        FROM ELN_IF.VW_USER A 
                        WHERE 1=1
                            AND A.IMUSERID = :USER_ID    
                            AND (1=1 OR 'a1234' = :PWD) 
                ) 
                , RM AS (  /**** not used ****/ 
                    SELECT A.ID 
                              , A.ROLE 
                              , A.MEMBER_ID
                    FROM ELN_IF.TB_PLM_ROLE A 
                    WHERE 1 = 1 
                       AND A.MEMBER_ID LIKE '%' || :USER_ID || '%'
                )
                , RM2 AS (  /**** not used ****/ 
                    SELECT A.ID 
                              , A.ROLE 
                              , REPLACE(REPLACE(REPLACE(REPLACE(REGEXP_SUBSTR(A.MEMBER_ID, '[^,]+', 1, LEVEL), '" + "\"" + @"', ''), '[', ''), ']', ''), ' ', '') AS  USER_ID 
                    FROM RM A
                    CONNECT BY LEVEL <= LENGTH(REGEXP_REPLACE(A.MEMBER_ID, '[^,]+', '')) + 1
                )
                SELECT A.*
                          ,   '" + roleName + @"'  AS USER_ROLE   /*** Las:Admin_User로 관리 ***/ 
                          , TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') AS LOGIN_DT
                FROM UM A
                        , RM2 B
                WHERE A.USER_ID = B.USER_ID(+)

             ");

            // 동적 파라미터 적용 
            //builder.Where("A.USER_ID = :USER_ID  ");
            //builder.Where("A.PWD = :PWD  ");
            //builder.Where("A.USER_ID =  :BB" , new { BB = model.USER_ID});  // model 에 없는 파라미터 추가 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

    }
}

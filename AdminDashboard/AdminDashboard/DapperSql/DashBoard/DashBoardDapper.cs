using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Cmm.Utils;
using DSELN.Models.DashBoard;

namespace DSELN.DapperSql.DashBoard
{
    public class DashBoardDapper
    {
        public static SqlBuilder.Template GetBelongCount(DashBoardSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            // 입력파라미터의 작성자ID, 팀이 현재 로그인 사용자의 팀(하위팀)의 사용자/부서인지를 count 
            var sql = builder.AddTemplate(@"
                SELECT COUNT(1) AS CNT 
                FROM ELN_IF.VW_USER A 
                /**where**/
            ");

            if (Role.ADMIN.Equals(model.SessionInfo.USER_ROLE))
            {
                builder.Where("1 = 1");
            }
            else
            {
                builder.Where(@"A.DEPTCODE1 IN (
                                                                    SELECT DEPTCODE
                                                                    FROM ELN_IF.TB_DEPARTMENT
                                                                    CONNECT BY PRIOR DEPTCODE = PARENTCODE
                                                                    START WITH DEPTCODE = :SessionInfo_USER_DEPT_CD
                                                                )");
            }

            if (!string.IsNullOrEmpty(model.USER_ID))
            {
                builder.Where("A.IMUSERID = :USER_ID ");
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }


        // DashBoard (관리자, 연구자)  > 연구노트 리스트 조회 
        public static SqlBuilder.Template GetExpNoteList(DashBoardSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
            var sql = builder.AddTemplate(@"
                WITH DASH AS ( 
                    SELECT A.EXPERIMENTNUMBER
                            , A.PROJECT_NAME
                            , A.TITLE
                            , A.BU_CODE
                            , A.DEPTCODE 
                            , B.SHORTNAME AS DEPTNAME
                            , A.USER_NAME
                            , A.USER_ID
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS CREATED
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS DATE4CHART
                            , A.SIGNED
                            , CASE WHEN A.SIGNED IS NULL THEN 'N' ELSE 'Y' END SIGNED_YN  
                            , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    FROM ELN_IF.V_NOTEBOOK_DASHBOARD A 
                           , ELN_IF.TB_DEPARTMENT B 
                    /**where**/
                ) 
                SELECT A.*     
                          , B.SHORTNAME AS BU_NAME 
                FROM DASH A 
                        , ELN_IF.TB_DEPARTMENT B 
                WHERE A.BU_CODE = B.DEPTCODE 
                ORDER BY A.EXPERIMENTNUMBER
            ");

            builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = B.DEPTCODE");

            // -------- Role, Position 에 따른 조회 --------------------------------------------------------
            // Role : Admin , Position Cd : 소장, 팀장 외 사용자는 자신의 데이터만 조회한다.  
            if (!(Role.ADMIN.Equals(model.SessionInfo.USER_ROLE) || Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD)))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :SessionInfo_USER_ID  /** 자기 데이터만... session user id **/ ");
            }

            // 소장: 하위 팀의 데이터 조회가능, 팀장: 팀의 데이터 조회가능 
            if (Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + model.SessionInfo.USER_DEPT_CD + "')  /** 소장 or 팀장 **/");
            }

            // ------- 이하 조회 조건 ----------------------
            if (!string.IsNullOrEmpty(model.BU_CODE))
            {
                //builder.Where(SqlHelper.PrefixWhere(6) + "A.BU_CODE = :BU_CODE");   // A.BU_CODE 가 안맞는듯...
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE  START WITH DEPTCODE = :BU_CODE)");
            }

            if (!string.IsNullOrEmpty(model.TEAM_CODE))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = :TEAM_CODE");
            }

            if (!string.IsNullOrEmpty(model.USER_ID))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :USER_ID");
            }

            if (model.FR_DATE != null)  // 작성일 fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 작서일 to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // DashBoard (관리자, 연구자)  > 템플릿 리스트 조회 
        public static SqlBuilder.Template GetTemplateList(DashBoardSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
            var sql = builder.AddTemplate(@"
                WITH DASH AS ( 
                    SELECT A.ANAL_NAME
                            , A.ANAL_ID
                            , A.EXPERIMENTNUMBER
                            , A.TITLE
                            , A.BU_CODE
                            , A.DEPTCODE 
                            , B.SHORTNAME AS DEPTNAME
                            , A.USER_NAME
                            , A.USER_ID
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS CREATED
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS DATE4CHART
                            , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    FROM ELN_IF.V_ANALYSIS_DASHBOARD A 
                           , ELN_IF.TB_DEPARTMENT B 
                    /**where**/
                ) 
                SELECT A.*     
                          , B.SHORTNAME AS BU_NAME 
                FROM DASH A 
                        , ELN_IF.TB_DEPARTMENT B 
                WHERE A.BU_CODE = B.DEPTCODE 
                ORDER BY A.ANAL_NAME
            ");

            builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = B.DEPTCODE");

            // -------- Role, Position 에 따른 조회 --------------------------------------------------------
            // Role : Admin , Position Cd : 소장, 팀장 외 사용자는 자신의 데이터만 조회한다.  
            if (!(Role.ADMIN.Equals(model.SessionInfo.USER_ROLE) || Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD)))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :SessionInfo_USER_ID  /** 자기 데이터만... session user id **/ ");
            }

            // 소장: 하위 팀의 데이터 조회가능, 팀장: 팀의 데이터 조회가능 
            if (Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + model.SessionInfo.USER_DEPT_CD + "')  /** 소장 or 팀장 **/");
            }

            // ------- 이하 조회 조건 ----------------------
            if (!string.IsNullOrEmpty(model.BU_CODE))
            {
                //builder.Where(SqlHelper.PrefixWhere(6) + "A.BU_CODE = :BU_CODE");   // A.BU_CODE 가 안맞는듯...
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE  START WITH DEPTCODE = :BU_CODE)");
            }

            if (!string.IsNullOrEmpty(model.TEAM_CODE))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = :TEAM_CODE");
            }

            if (!string.IsNullOrEmpty(model.USER_ID))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :USER_ID");
            }

            if (model.FR_DATE != null)  // 작성일 fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 작서일 to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        // DashBoard (관리자, 연구자)  > 실험결과 리스트 조회 
        public static SqlBuilder.Template GetExpResultList(DashBoardSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
            var sql = builder.AddTemplate(@"
                WITH DASH AS ( 
                    SELECT A.EXP_NAME
                            , A.EXP_ID
                            , A.EQUIP_ID
                            , A.BU_CODE
                            , A.DEPTCODE 
                            , B.SHORTNAME AS DEPTNAME
                            , A.USER_NAME
                            , A.USER_ID
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS CREATED
                            , TO_CHAR(A.CREATED, 'YYYY-MM-DD') AS DATE4CHART
                            , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    FROM ELN_IF.V_EXPRESULT_DASHBOARD A 
                           , ELN_IF.TB_DEPARTMENT B 
                    /**where**/
                ) 
                SELECT A.*     
                          , B.SHORTNAME AS BU_NAME 
                FROM DASH A 
                        , ELN_IF.TB_DEPARTMENT B 
                WHERE A.BU_CODE = B.DEPTCODE 
                ORDER BY A.EXP_NAME
            ");

            builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = B.DEPTCODE");

            // -------- Role, Position 에 따른 조회 --------------------------------------------------------
            // Role : Admin , Position Cd : 소장, 팀장 외 사용자는 자신의 데이터만 조회한다.  
            if (!(Role.ADMIN.Equals(model.SessionInfo.USER_ROLE) || Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD)))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :SessionInfo_USER_ID  /** 자기 데이터만... session user id **/ ");
            }

            // 소장: 하위 팀의 데이터 조회가능, 팀장: 팀의 데이터 조회가능 
            if (Position.DIRECTOR.Equals(model.SessionInfo.USER_POSITION_CD) || Position.TEAM_LEADER.Equals(model.SessionInfo.USER_POSITION_CD))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + model.SessionInfo.USER_DEPT_CD + "')  /** 소장 or 팀장 **/");
            }

            // ------- 이하 조회 조건 ----------------------
            if (!string.IsNullOrEmpty(model.BU_CODE))
            {
                //builder.Where(SqlHelper.PrefixWhere(6) + "A.BU_CODE = :BU_CODE");   // A.BU_CODE 가 안맞는듯...
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE IN (SELECT DEPTCODE FROM TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE  START WITH DEPTCODE = :BU_CODE)");
            }

            if (!string.IsNullOrEmpty(model.TEAM_CODE))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE = :TEAM_CODE");
            }

            if (!string.IsNullOrEmpty(model.USER_ID))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USER_ID = :USER_ID");
            }

            if (model.FR_DATE != null)  // 작성일 fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 작서일 to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

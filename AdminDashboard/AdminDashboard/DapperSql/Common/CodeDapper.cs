using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;
using DSELN.Cmm.Utils;

namespace DSELN.DapperSql.Common
{
    public class CodeDapper
    {
        // 각종 코드 가져오기   
        public static SqlBuilder.Template GetCodeList(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT A.*
                    FROM (
                        SELECT 'CD_TYP' AS GRP_CD
                                  , CD_TYP AS VALUE
                                  , CD_TYP_NM_KO AS TEXT 
                                  , '' AS ATT1, '' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                  , ROWNUM AS SORT_ORD  
                        FROM  ELN_IF.TB_ESA_CDTP  /* 코드유형 */
                        
                        UNION

                        SELECT A.GRP_CD, B.DTL_CD AS VALUE, B.DTL_CD_NM_KO AS TEXT 
                                   , B.REM AS ATT1, '' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                   , B.SORT_ORD  
                        FROM ELN_IF.TB_ESA_CDGP A 
                               , ELN_IF.TB_ESA_CDDT B /* 공통코드 */ 
                        WHERE A.GRP_CD = B.GRP_CD 
                          AND A.GRP_CD = :ID 
                          AND B.USE_YN='Y'
                    ) A 
                    WHERE A.GRP_CD = :ID 
                    ORDER BY A.SORT_ORD 
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // Role 코드 
        public static SqlBuilder.Template GetRoleList(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder
 
            var sql = builder.AddTemplate(@"  
                    SELECT A.*
                    FROM (
                        SELECT 'ROLE_CD' AS GRP_CD   /**** 필요시 코드등록 또는 롤코드관리(추가개발)를 사용할것. ****/
                                  , '" + Role.ADMIN + @"' AS VALUE
                                  , '관리자' AS TEXT  
                        FROM DUAL 

                        UNION

                        SELECT 'ROLE_CD' AS GRP_CD
                                  , '" + Role.USER + @"' AS VALUE
                                  , '사용자' AS TEXT  
                        FROM DUAL 

                    ) A 
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // BU 코드 
        public static SqlBuilder.Template GetBUList(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"  
                SELECT 'BU_CODE' AS GRP_CD
                          , CASE WHEN B.DTL_CD = 'BIO' THEN '15000504'
                                     WHEN B.DTL_CD = 'GLOBAL' THEN '15000423'
                                     WHEN B.DTL_CD = 'STARCH' THEN '15000503'
                                     WHEN B.DTL_CD = 'FOOD' THEN '10410000'
                                     ELSE 'NOT_DEFINED' 
                            END AS VALUE
                          , B.DTL_CD_NM_KO AS TEXT 
                          , '' AS ATT1, '' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                          , B.SORT_ORD  
                FROM ELN_IF.TB_ESA_CDGP A 
                        , ELN_IF.TB_ESA_CDDT B /* 공통코드 */ 
                /**where**/
             ");

            builder.Where(SqlHelper.PrefixWhere(6) + "A.GRP_CD = B.GRP_CD");   
            builder.Where(SqlHelper.PrefixWhere(6) + "A.GRP_CD = 'USE_DEPT' ");

            // User Role  (CodeCondition 모델에는 session 정보가 없다.) 
            BaseSearchModel model2 = new BaseSearchModel();
            if (Role.USER.Equals(model2.SessionInfo.USER_ROLE)) // 롤이 사용자일 경우 소속 BU만 나오도록 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "B.DTL_CD = '" + model2.SessionInfo.USE_DEPT + "'");  //  사용자의 USE_DEPT 
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // TEAM 코드 (Dashboard)
        public static SqlBuilder.Template GetDashBoardTeamList(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            string FR_DATE = Utils.GetDashBoardPeriod("MAX_FR_DATE", 0);
            string TO_DATE = Utils.GetDashBoardPeriod("MAX_TO_DATE", 0);

            var sql = builder.AddTemplate(@"  
                WITH TEAM AS (
                        SELECT 'TEAM_CODE' AS GRP_CD, DEPTCODE AS VALUE, SHORTNAME AS TEXT 
                                  , '15000503' AS ATT1, '소재_전분당BU' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                  , 1 AS SORT_ORD  
                        FROM ELN_IF.TB_DEPARTMENT 
                        CONNECT BY PRIOR DEPTCODE = PARENTCODE
                        START WITH DEPTCODE = '15000503'

                        UNION ALL 

                        SELECT 'TEAM_CODE' AS GRP_CD, DEPTCODE AS VALUE, SHORTNAME AS TEXT 
                                  , '15000504' AS ATT1, '소재_바이오BU' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                  , 1 AS SORT_ORD  
                        FROM ELN_IF.TB_DEPARTMENT
                        CONNECT BY PRIOR DEPTCODE = PARENTCODE
                        START WITH DEPTCODE = '15000504'

                        UNION ALL 

                        SELECT 'TEAM_CODE' AS GRP_CD, DEPTCODE AS VALUE, SHORTNAME AS TEXT 
                                  , '15000423' AS ATT1, '식품Global사업총괄' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                  , 1 AS SORT_ORD  
                        FROM ELN_IF.TB_DEPARTMENT
                        CONNECT BY PRIOR DEPTCODE = PARENTCODE
                        START WITH DEPTCODE = '15000423'

                        UNION ALL 

                        SELECT 'TEAM_CODE' AS GRP_CD, DEPTCODE AS VALUE, SHORTNAME AS TEXT 
                                  , '10410000' AS ATT1, '식품사업총괄' AS ATT2, '' AS ATT3, '' AS ATT4, '' AS ATT5
                                  , 1 AS SORT_ORD  
                        FROM ELN_IF.TB_DEPARTMENT
                        CONNECT BY PRIOR DEPTCODE = PARENTCODE
                        START WITH DEPTCODE = '10410000'
                ) 
                SELECT A.* 
                FROM TEAM A 
                /**where**/
                ORDER BY A.TEXT 
             ");

            // 기간내 데이터에 존재하는 부서만 
            builder.Where(SqlHelper.PrefixWhere(6) + @"A.VALUE IN (
                                                SELECT DISTINCT DEPTCODE 
                                                FROM ELN_IF.V_NOTEBOOK_DASHBOARD 
                                                WHERE 1 = 1
                                                  AND CREATED >= " + SqlHelper.FrDateWhere(FR_DATE) + @"
                                                  AND CREATED < " + SqlHelper.ToDateWhere(TO_DATE)  + @"

                                                UNION ALL

                                                SELECT DISTINCT DEPTCODE 
                                                FROM ELN_IF.V_ANALYSIS_DASHBOARD 
                                                WHERE 1 = 1
                                                  AND CREATED >= " + SqlHelper.FrDateWhere(FR_DATE) + @"
                                                  AND CREATED < " + SqlHelper.ToDateWhere(TO_DATE) + @"
                                            ) ");


            // User Role  (CodeCondition 모델에는 session 정보가 없다.) 
            BaseSearchModel user = new BaseSearchModel();
            if (Role.ADMIN.Equals(user.SessionInfo.USER_ROLE)) // 관리자 
            {
                // dummy 
            }
            else
            {
                if ("DASHBOARD_USER".Equals(model.SUB_ID))  // 대시보드(사용자) 화면에서 호출시, 로그인 사용자의 하위 팀만 조회 
                {
                    builder.Where(SqlHelper.PrefixWhere(6) + "A.VALUE IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + user.SessionInfo.USER_DEPT_CD + "')");
                }
            }
 
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 장비 
        public static SqlBuilder.Template GetEquip4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT 'EQUIP' AS GRP_CD
                            , A.EQUIP_ID AS VALUE  
                            , A.EQUIP_NAME AS TEXT 
                            , A.PARSING_TYPE AS ATT1
                            , A.USE_DEPT AS ATT2
                            , '' AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5
                            , '' AS ATT1_NM
                            , '' AS ATT2_NM
                            , '' AS ATT3_NM
                            , '' AS ATT4_NM
                            , '' AS ATT5_NM
                    FROM ELN_IF.TB_EQUIPMENT A 
                    /**where**/
                    ORDER BY A.EQUIP_NAME
             ");

            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 분석항목 
        public static SqlBuilder.Template GetAnalysisItem4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT 'ANAL_ITEM' AS GRP_CD
                            , A.ITEM_ID AS VALUE  
                            , A.DESCRIPTION AS TEXT /*** 설명을 콤보text,   ITEM_NAME 필요할 경우 알려주세요 ***/

                            , A.USE_DEPT AS ATT1
                            , A.RET_TIME AS ATT2
                            , A.ITEM_NAME AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5

                            , B.DTL_CD_NM_KO AS ATT1_NM
                            , '' AS ATT2_NM
                            , '' AS ATT3_NM
                            , '' AS ATT4_NM
                            , '' AS ATT5_NM

                    FROM ELN_IF.TB_ANALYSIS_ITEM A 
                           , ELN_IF.TB_ESA_CDDT B 
                    /**where**/
                    ORDER BY A.ITEM_ID
             ");

            // 동적 파라미터 적용 
            builder.Where(SqlHelper.PrefixWhere(6) + " 'USE_DEPT' = B.GRP_CD AND A.USE_DEPT = B.DTL_CD ");

            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 분석 템플릿  ???? 
        public static SqlBuilder.Template GetAnalysisTemplate4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT 'TMPL_ID' AS GRP_CD
                            , A.TMPL_ID AS VALUE  
                            , A.TMPL_NAME AS TEXT 

                            , A.PARSING_TYPE AS ATT1
                            , A.TMPL_TYPE AS ATT2
                            , A.USE_DEPT AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5

                            , '' AS ATT1_NM
                            , '' AS ATT2_NM
                            , '' AS ATT3_NM
                            , '' AS ATT4_NM
                            , '' AS ATT5_NM
                    FROM ELN_IF.TB_ANALYSIS_TEMPLATE A
                    /**where**/
                    ORDER BY A.TMPL_ID
             ");

            // 동적 파라미터 적용 
            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 템플릿속성  
        public static SqlBuilder.Template GetTemplateAttr4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT ''' || :ID || ''' AS GRP_CD
                            , A.ATTR_ID AS VALUE  
                            , A.TITLE AS TEXT  /*** title을 콤보text,   ATTR_NAME 필요할 경우 알려주세요 ***/

                            , A.USE_DEPT AS ATT1
                            , A.ATTR_TYPE AS ATT2
                            , A.FIELD_TYPE AS ATT3
                            , A.ATTR_NAME AS ATT4
                            , '' AS ATT5

                            , B.DTL_CD_NM_KO AS ATT1_NM
                            , '' AS ATT2_NM
                            , '' AS ATT3_NM
                            , '' AS ATT4_NM
                            , '' AS ATT5_NM
                    FROM ELN_IF.TB_TEMPLATE_ATTRIBUTE A 
                           , ELN_IF.TB_ESA_CDDT B 
                    /**where**/
                    ORDER BY A.ATTR_ID
             ");

            // 동적 파라미터 적용 
            builder.Where(SqlHelper.PrefixWhere(6) + " 'USE_DEPT' = B.GRP_CD AND A.USE_DEPT = B.DTL_CD ");

            if (!string.IsNullOrEmpty(model.SUB_ID))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.ATTR_TYPE = :SUB_ID");
            }

            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }
            return DynamicParameterHelper.RefineSql(sql, model);
        }


        // 사용자 가져오기 
        public static SqlBuilder.Template GetUserList(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT A.IMUSERID AS USER_ID  
                            , A.NAME AS USER_NM 

                            , A.POSITION1 AS POSITION
                            , NVL(A.COMPANYPHONE, '') AS TEL
                            , NVL(B.SHORTNAME, '') AS DEPTNAME
                            , NVL(A.DEPTCODE1, '') AS DEPTCODE
 
                    FROM ELN_IF.VW_USER A 
                           , ELN_IF.TB_DEPARTMENT B 
                    /**where**/
              ");

            builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE1 = B.DEPTCODE ");

            // 동적 파라미터 적용 
            if (!string.IsNullOrEmpty(model.KEYWORD))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.NAME LIKE '%' || :KEYWORD || '%' ");
            }

            // User Role  (CodeCondition 모델에는 session 정보가 없다.) 
            BaseSearchModel user = new BaseSearchModel();

            // 대시보드(사용자) 화면에서 호출시 
            if ("DASHBOARD_USER".Equals(model.SUB_ID))
            {
                string FR_DATE = Utils.GetDashBoardPeriod("MAX_FR_DATE", 0);
                string TO_DATE = Utils.GetDashBoardPeriod("MAX_TO_DATE", 0);

                // 연구노트, 템플릿 작성 연구원만 나오도록 
                // 기간내 데이터에 존재하는 부서만 
                builder.Where(SqlHelper.PrefixWhere(6) + @"A.DEPTCODE1 IN (
                                                SELECT DISTINCT DEPTCODE 
                                                FROM ELN_IF.V_NOTEBOOK_DASHBOARD 
                                                WHERE 1 = 1
                                                  AND CREATED >= " + SqlHelper.FrDateWhere(FR_DATE) + @"
                                                  AND CREATED < " + SqlHelper.ToDateWhere(TO_DATE) + @"

                                                UNION ALL

                                                SELECT DISTINCT DEPTCODE 
                                                FROM ELN_IF.V_ANALYSIS_DASHBOARD 
                                                WHERE 1 = 1
                                                  AND CREATED >= " + SqlHelper.FrDateWhere(FR_DATE) + @"
                                                  AND CREATED < " + SqlHelper.ToDateWhere(TO_DATE) + @"
                                            ) ");

                // 로그인 사용자가 속한 팀(하위팀)내의 연구원만 
                if (!Role.ADMIN.Equals(user.SessionInfo.USER_ROLE)) // 관리자 아니면.. 
                {
                    builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE1 IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + user.SessionInfo.USER_DEPT_CD + "')");
                }

                // 팀내 사용자만 
                if(!string.IsNullOrEmpty(model.PARAM1))
                {
                    builder.Where(SqlHelper.PrefixWhere(6) + "A.DEPTCODE1 IN (SELECT DEPTCODE FROM ELN_IF.TB_DEPARTMENT CONNECT BY PRIOR DEPTCODE = PARENTCODE START WITH DEPTCODE = '" + model.PARAM1 + "')");
                }

            }
 
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /// <summary>
        /// Dropdown List를 위해 속성 단위 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public static SqlBuilder.Template GetUnitData4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT ''' || :ID || ''' AS GRP_CD
                            , A.UNIT_ID AS VALUE  
                            , A.UNIT_DATA AS TEXT 
                            , B.MEASURE_ID AS ATT1
                            , '' AS ATT2
                            , '' AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5
                    FROM ELN_IF.TB_UNIT A
                        , ELN_IF.TB_MEASURE_UNIT B
                    /**where**/
                    
                    ORDER BY A.UNIT_ID
             ");

            builder.Where(SqlHelper.PrefixWhere(6) + " A.UNIT_ID = B.UNIT_ID");

            // 동적 파라미터 적용 
            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /// <summary>
        /// QR 장비 관리 화면의 장비 담당자 Dropdown List를 위해 장비 담당자 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public static SqlBuilder.Template GetEquipCharger4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT ''' || :ID || ''' AS GRP_CD
                            , A.CHARGER AS VALUE  
                            , B.NAME AS TEXT 
                            , '' AS ATT1
                            , A.USE_DEPT AS ATT2
                            , '' AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5
                    FROM ELN_IF.TB_EQUIPMENT A, ELN_IF.TB_USER B
                    /**where**/
                    ORDER BY A.EQUIP_ID
             ");

            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE AND A.CHARGER = B.IMUSERID");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /// <summary>
        /// QR 장비 관리 화면의 장비 위치 Dropdown List를 위해 장비 위치 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns>조회 결과 리턴</returns>
        public static SqlBuilder.Template GetEquipLoc4Code(CodeCondition model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                    SELECT ''' || :ID || ''' AS GRP_CD
                            , A.EQUIP_ID AS VALUE  
                            , A.EQUIP_LOCATION AS TEXT 
                            , '' AS ATT1
                            , A.USE_DEPT AS ATT2
                            , '' AS ATT3
                            , '' AS ATT4
                            , '' AS ATT5
                    FROM ELN_IF.TB_EQUIPMENT A
                    /**where**/
                    ORDER BY A.EQUIP_ID
             ");

            if (!string.IsNullOrEmpty(model.PARAM1))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM1 + " = :PARAM1_VALUE");  // 조건1 
            }

            if (!string.IsNullOrEmpty(model.PARAM2))
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A." + model.PARAM2 + " = :PARAM2_VALUE");  // 조건2
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

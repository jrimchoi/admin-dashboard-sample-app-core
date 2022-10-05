using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;

namespace DSELN.DapperSql.Analysis
{
    public class AnalysisDapper
    {
        /**************************************************************************
        // 분석템플릿-작성/조회
        **************************************************************************/
        // 분석템플릿-작성/조회
        public static SqlBuilder.Template GetAnalysisList(ExpAnalysisSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
	                SELECT COUNT(1) OVER() AS PAGE_TOT_ROWS
		                , A.ANAL_ID
		                , ROW_NUMBER() OVER(ORDER BY A.CREATED DESC) AS RN 
	                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS A
	                /**where**/
                )
                SELECT A.PAGE_TOT_ROWS AS PAGE_TOT_ROWS
		                , B.ANAL_ID
		                , B.ANAL_ID AS ANAL_ID_KEY
                        , B.EXP_ID
		                , B.TITLE
		                , B.DESCRIPTION
		                , B.USE_DEPT
		                , B.PARSING_TYPE
		                , B.TMPL_TYPE
		                , B.STATE
		                , B.EXP_NO
		                , B.ANAL_ITEMS
                        , B.CREATOR AS CREATOR_ID
		                , D.NAME  AS CREATOR
		                , B.MODIFIER
		                , TO_CHAR( B.CREATED, 'YYYY-MM-DD HH24:MI:SS') AS CREATED
		                , TO_CHAR( B.MODIFIED, 'YYYY-MM-DD HH24:MI:SS') AS MODIFIED
		                , B.TMPL_ID
		                , C.DESCRIPTION AS ANAL_ITEMS_NAME
                FROM GRP A
	                , ELN_IF.TB_EXPERIMENT_ANALYSIS B
	                , ELN_IF.VW_ANALYSIS_TEMPLATE_ITEMS C
                    , ELN_IF.TB_USER D
                where A.ANAL_ID = B.ANAL_ID
                and B.TMPL_ID = C.TMPL_ID
                and B.CREATOR = D.IMUSERID
                ORDER BY A.RN
                "
                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");

            // 동적 파라미터 적용
            if (model.TITLE != null) // 실험명 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.TITLE LIKE '%' || :TITLE || '%' ");
            }

            if (model.USE_DEPT != null) // 부서
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USE_DEPT = :USE_DEPT  ");
            }

            if (model.CREATOR != null) // 작성자
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATOR IN (SELECT X.IMUSERID FROM ELN_IF.TB_USER X WHERE NAME LIKE '%' || :CREATOR || '%') ");
            }

            if (model.TMPL_TYPE != null) // 템플리 유형 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.TMPL_TYPE = :TMPL_TYPE  ");
            }

            if (model.ANAL_ITEMS != null) // 분석 항목 이름
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.TMPL_ID IN (SELECT X.TMPL_ID FROM ELN_IF.VW_ANALYSIS_TEMPLATE_ITEMS_ROW X WHERE X.ITEM_NAME_QRY LIKE '%' || UPPER(:ANAL_ITEMS) || '%'  ) ");
            }

            if (model.FR_DATE != null)  // 작성일 fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 작성일 to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // DB 템플릿 삭제 - TB_EXPERIMENT_ANALYSIS
        public static SqlBuilder.Template Del_TB_EXPERIMENT_ANALYSIS(ExpAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_EXPERIMENT_ANALYSIS
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // DB 템플릿 삭제 - TB_SAMPLE
        public static SqlBuilder.Template Del_TB_SAMPLE(ExpAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_SAMPLE
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // DB 템플릿 삭제 - TB_ITEM
        public static SqlBuilder.Template Del_TB_ITEM(ExpAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_ITEM
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // DB 템플릿 삭제 - TB_EXPERIMENT_ANALYSIS_ATTR
        public static SqlBuilder.Template Del_TB_EXPERIMENT_ANALYSIS_ATTR(ExpAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /**************************************************************************
        // 실험결과-분석
        **************************************************************************/
        // 실험결과-분석 조회
        public static SqlBuilder.Template GetExpResultList(ExpResultSearch model) {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
	                SELECT A.EXP_ID
		                , ROW_NUMBER() OVER(ORDER BY A.MODIFIED DESC) AS RN
	                FROM ELN_IF.TB_EXPERIMENT_RESULT A
	                /**where**/
                ), EXP_TR AS (
                    SELECT B.EXP_ID
                        , B.EXP_ID AS EXP_ID_KEY
                        , B.EQUIP_ID
                        , B.TITLE
                        , B.FILE_PATH
                        , B.FOLDER_NAME
                        , B.EXP_DATE
                        , B.USE_DEPT
                        , B.EXP_OWNER
                        , B.PARSING_TYPE
                        , B.STATE
                        , B.SAMPLES
                        , B.PARSING_LOG
                        , C.NAME AS CREATOR
                        , B.MODIFIER
                        , TO_CHAR( B.CREATED, 'YYYY-MM-DD HH24:MI:SS') AS CREATED
                        , TO_CHAR( B.MODIFIED, 'YYYY-MM-DD HH24:MI:SS') AS MODIFIED
                    FROM ELN_IF.TB_EXPERIMENT_RESULT B
                        , ELN_IF.TB_USER C
                    WHERE B.PARSING_TYPE is not null
                    and B.EXP_OWNER = C.IMUSERID
                    UNION ALL
                    SELECT B.EXP_ID
                        , B.EXP_ID AS EXP_ID_KEY
                        , B.EQUIP_ID
                        , B.TITLE
                        , B.FILE_PATH
                        , B.FOLDER_NAME
                        , B.EXP_DATE
                        , B.USE_DEPT
                        , B.EXP_OWNER
                        , B.PARSING_TYPE
                        , B.STATE
                        , B.SAMPLES
                        , B.PARSING_LOG
                        , null as CREATOR
                        , B.MODIFIER
                        , TO_CHAR( B.CREATED, 'YYYY-MM-DD HH24:MI:SS') AS CREATED
                        , TO_CHAR( B.MODIFIED, 'YYYY-MM-DD HH24:MI:SS') AS MODIFIED
                    FROM ELN_IF.TB_EXPERIMENT_RESULT B
                    WHERE B.PARSING_TYPE is not null
                    and EXP_OWNER is null
                )
                SELECT COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    , B.*
                FROM GRP A
                    , EXP_TR B
                WHERE A.EXP_ID = B.EXP_ID
                ORDER BY A.RN
                "
                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");

            // 동적 파라미터 적용
            if (model.EXP_ID != null) // 실험ID 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.EXP_ID = :EXP_ID  ");
            }

            if (model.EQUIP_ID != null) // 장비명(값은 ID)
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.EQUIP_ID = :EQUIP_ID  ");
            }

            if (model.USE_DEPT != null) // 부서 = 사용부서?
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.USE_DEPT = :USE_DEPT  ");
            }

            if (model.STATE != null) // 상태
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.STATE = :STATE  ");
            }

            if (model.CREATOR != null) // 작성자
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.EXP_OWNER IN (SELECT X.IMUSERID FROM ELN_IF.TB_USER X WHERE NAME LIKE '%' || :CREATOR || '%') ");
            }

            if (model.TITLE != null) // 실험명 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.TITLE LIKE '%' || :TITLE || '%' ");
            }

            if (model.FILE_PATH != null) // 폴더명 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.FILE_PATH LIKE '%' || :FILE_PATH || '%' ");
            }
            if (model.FOLDER_NAME != null) // 폴더명 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.FOLDER_NAME LIKE '%' || :FOLDER_NAME || '%' ");
            }

            if (model.FR_DATE != null)  // 작성일 fr (수정일시 비교)
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.MODIFIED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 작성일 to (수정일시 비교) 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.MODIFIED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과-분석 Update
        public static SqlBuilder.Template ExpResultUpdate(ExpResultModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_EXPERIMENT_RESULT
		        SET TITLE = :TITLE
		            , EXP_OWNER = :SessionInfo.USER_ID
		            , MODIFIER = :SessionInfo.USER_ID
		            , MODIFIED = SYSDATE
		        WHERE EXP_ID = :EXP_ID_KEY
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과-분석 Insert (TB_EXPERIMENT_ANALYSIS)
        public static SqlBuilder.Template ExpAnalysisInsert(ExpResultModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        INSERT INTO ELN_IF.TB_EXPERIMENT_ANALYSIS (
		              ANAL_ID     /***  분석결과 ID    ***/
		            , TITLE    /***   실험명   ***/
		            , USE_DEPT    /***   사용부서   ***/
		            , PARSING_TYPE   /***    장비유형   ***/
		            , TMPL_TYPE   /***    템플릿유형 (공통코드)   ***/
		            , PARSING_RESULT    /***   파싱결과   ***/
		            , STATE     /***  상태   ***/
		            , CREATOR     /***  생성자   ***/
		            , CREATED     /***  생성일시   ***/
		            , ANAL_ITEMS     /***  분석항목리스트   ***/
		            , TMPL_ID     /***  템플릿 ID   ***/
		            , EXP_ID     /***  실험 ID(TB_EXPERIMENT_RESULT EXP_ID) ***/
		        ) 
		        VALUES ( 
		              :ANAL_ID
		            , :TITLE
		            , :USE_DEPT
		            , :PARSING_TYPE  
		            , (SELECT ELN_IF.TB_ANALYSIS_TEMPLATE.TMPL_TYPE 
                    FROM ELN_IF.TB_ANALYSIS_TEMPLATE
                    WHERE ELN_IF.TB_ANALYSIS_TEMPLATE.TMPL_ID = :TMPL_ID)
		            , (SELECT ELN_IF.TB_EXPERIMENT_RESULT.PARSING_RESULT 
                    FROM ELN_IF.TB_EXPERIMENT_RESULT
                    WHERE ELN_IF.TB_EXPERIMENT_RESULT.EXP_ID = :EXP_ID)
		            , 'ANALYZING'  
		            , :SessionInfo.USER_ID
		            , SYSDATE
		            , (SELECT ELN_IF.TB_ANALYSIS_TEMPLATE.ANAL_ITEMS
                    FROM ELN_IF.TB_ANALYSIS_TEMPLATE
                    WHERE ELN_IF.TB_ANALYSIS_TEMPLATE.TMPL_ID = :TMPL_ID)
		            , :TMPL_ID
                    , :EXP_ID
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과-분석 Update
        public static SqlBuilder.Template GetParsingLog(ExpResultSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        SELECT COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    , A.EXP_ID
                    , A.ID
                    , A.TYPE
                    , A.RESULT
                    , A.FOLDER
                    , A.MSG
                FROM V_PARSING_LOG A
                /**where**/
             "
            // paging 
            + DynamicParameterHelper.SetPaginCondition(model)

            + @"");

            builder.Where("A.EXP_ID = :EXP_ID");
            builder.Where("A.ID != 0");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

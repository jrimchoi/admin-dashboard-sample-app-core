using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;

namespace DSELN.DapperSql.Analysis
{
    public class ItemDapper
    {
        public static SqlBuilder.Template GetItem(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_ITEM A
                /**where**/
            ");

            builder.Where("A.ITEM_ID = :ITEM_ID");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template ItemInsert(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_ITEM (
                     ITEM_ID
                    , SAMPLE_ID
                    , ANAL_ID
                    , ITEM_NAME
                    , USE_DEPT
                    , RET_TIME
                    , AREA
                    , CREATOR
                    , MODIFIER
                    , CREATED
                    , MODIFIED
                    , PCT_AREA
                    , DATA_JSON
		        ) 
		        VALUES ( 
                      :ITEM_ID
                    , :SAMPLE_ID
                    , :ANAL_ID
                    , :ITEM_NAME
                    , :USE_DEPT
                    , :RET_TIME
                    , :AREA
                    , :SessionInfo.USER_ID   
                    , :SessionInfo.USER_ID   
                    , SYSDATE
                    , SYSDATE
                    , :PCT_AREA
                    , :DATA_JSON
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template ItemUpdate(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ITEM
		        SET 
                      SAMPLE_ID     = :SAMPLE_ID
                    , ANAL_ID        = :ANAL_ID  
                    , ITEM_NAME   = :ITEM_NAME
                    , USE_DEPT     = :USE_DEPT
                    , RET_TIME      = :RET_TIME 
                    , AREA             = :AREA 
                    , MODIFIER       = :SessionInfo.USER_ID   
                    , MODIFIED       = SYSDATE  
                    , PCT_AREA     = :PCT_AREA  
                    , DATA_JSON   = :DATA_JSON  
		        WHERE ITEM_ID = :ITEM_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template GetItemList(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT A.ITEM_ID
                          , A.SAMPLE_ID
                          , A.ANAL_ID
                          , A.USE_DEPT
                          , A.ITEM_NAME
                          , A.RET_TIME
                          , A.AREA
                          , A.MEAS_RET_TIME
                          , CASE WHEN A.TITLE IS NULL THEN NVL(B.DESCRIPTION, A.ITEM_NAME) ELSE A.TITLE END TITLE  /*** title 현재 누락됨 ***/
                          , A.PCT_AREA    
                          , A.DATA_JSON   
                FROM ELN_IF.TB_ITEM A 
                       , ELN_IF.TB_ANALYSIS_ITEM B 
                WHERE A.ITEM_NAME = B.ITEM_NAME (+)  AND A.USE_DEPT = B.USE_DEPT(+)  /*** key 확인 필요 ***/
                  AND A.SAMPLE_ID = :SAMPLE_ID
                ORDER BY A.ITEM_ID ASC
            ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        public static SqlBuilder.Template GetItemListByAnalysis(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_ITEM A
                WHERE A.ANAL_ID = :ANAL_ID
                ORDER BY ITEM_ID ASC
            ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 분석항목 저장 
        public static SqlBuilder.Template ItemUpdate4Analysis(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ITEM        
		        SET MEAS_RET_TIME  = :MEAS_RET_TIME 
                     , AREA              = :AREA 
                     , TITLE             =  :TITLE 
                     , PCT_AREA      =  :PCT_AREA 
                     , DATA_JSON    =  :DATA_JSON 
                     , MODIFIER       =  :SessionInfo.USER_ID      
                     , MODIFIED       =   SYSDATE     
		        WHERE ITEM_ID = :ITEM_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플삭제 > 분석항목 삭제 
        public static SqlBuilder.Template ItemDelete4Analysis(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_ITEM         
		        WHERE SAMPLE_ID = :SAMPLE_ID   /**** sample id 로 삭제 ****/
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > Ret Time 지정 조회 
        public static SqlBuilder.Template GetRetTimeList(ItemSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH RET AS (
                    SELECT DISTINCT 
                                A.ITEM_NAME 
                              , NVL(A.TITLE, C.DESCRIPTION) AS TITLE   
                              , A.RET_TIME 
                              , A.ANAL_ID  
                              , B.TMPL_ID 
                    FROM ELN_IF.TB_ITEM A 
                           , ELN_IF.TB_SAMPLE B 
                           , ELN_IF.TB_ANALYSIS_ITEM C 
                    WHERE A.ANAL_ID = B.ANAL_ID AND A.SAMPLE_ID = B.SAMPLE_ID 
                      AND A.ITEM_NAME = C.ITEM_NAME(+)  AND A.USE_DEPT = C.USE_DEPT(+)  
                      AND A.ANAL_ID = :ANAL_ID  
                ) 
                , ANAL_ITEM_ORD AS (  /*** 분석템플릿에 등록된 분석항목의 순서를 최종 조회 순서로 설정하기 위해 ***/
                    SELECT B.ITEM_NAME 
                              , A.SORT_ORD  
                    FROM (
                                SELECT TRIM(REGEXP_SUBSTR(B.ANAL_ITEMS, '[^,]+', 1, Level)) AS ANAL_ITEM      
                                          , ROWNUM AS SORT_ORD  
                                FROM (SELECT DISTINCT TMPL_ID FROM RET) A 
                                        , ELN_IF.TB_ANALYSIS_TEMPLATE B 
                                WHERE  A.TMPL_ID = B.TMPL_ID 
                                  AND  B.ANAL_ITEMS IS NOT NULL     
                                  CONNECT BY LEVEL <= REGEXP_COUNT(B.ANAL_ITEMS, '[^,]+')    
                                  AND PRIOR A.TMPL_ID  = A.TMPL_ID    
                                  AND PRIOR DBMS_RANDOM.VALUE IS NOT NULL
                               ) A 
                               , ELN_IF.TB_ANALYSIS_ITEM B 
                      WHERE A.ANAL_ITEM = B.ITEM_ID  
                )

                SELECT DISTINCT   /*** 최종 distinct ****/ 
                           A.ITEM_NAME
                         , A.TITLE 
                         , A.RET_TIME 
                         , A.ANAL_ID 
                         , (SELECT MIN(X.SORT_ORD)  FROM ANAL_ITEM_ORD X WHERE A.ITEM_NAME = X.ITEM_NAME) AS SORT_ORD  /*** tmpl_id 여러개, 동일 item 여러개 인 경우 있을지도... min 으로  ***/
                FROM RET A 
                ORDER BY SORT_ORD 
            ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > Ret Time 저장  
        public static SqlBuilder.Template ItemRetTimeUpdate(ItemModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ITEM        
		        SET RET_TIME       = :RET_TIME 
                     , MODIFIER       =  :SessionInfo.USER_ID      
                     , MODIFIED       =   SYSDATE     
		        WHERE ANAL_ID     = :ANAL_ID
                    AND ITEM_NAME = :ITEM_NAME  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 분석결과&샘플&분석항목 조회 (권한체크용)   
        public static SqlBuilder.Template GetExpAnalysisItem(ItemSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.SAMPLE_ID  
                          , A.ANAL_ID 
                          , A.ITEM_ID 
                          , B.STATE 
                          , C.CREATOR AS CREATOR_ID  /*** 권한 체크용 ***/
                          , C.EXP_NO 
                          , C.STATE AS EXP_ANAL_STATE
                FROM ELN_IF.TB_ITEM A 
                       , ELN_IF.TB_SAMPLE B
                       , ELN_IF.TB_EXPERIMENT_ANALYSIS C 
                /**where**/     
            ");

            builder.Where("A.SAMPLE_ID = B.SAMPLE_ID ");
            builder.Where("B.ANAL_ID = C.ANAL_ID ");

            builder.Where("A.ITEM_ID     = :ITEM_ID");
            builder.Where("A.SAMPLE_ID = :SAMPLE_ID ");
            builder.Where("C.ANAL_ID     = :ANAL_ID");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /****************************************************************
        // 분석결과 > 샘플리스트 >  Frangrance 
        ****************************************************************/
        // 실험결과분석 > 샘플 리스트 > Frangrance 조회  
        public static SqlBuilder.Template GetSampleFrangranceList(FragranceSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.SAMPLE_ID 
                        , A.SAMPLE_NAME	 
                        , A.PK 
                        , A.RT	 
                        , A.AREA_PCT 
                        , A.LIBRARY_ID 
                        , A.REF 
                        , A.CAS 
                        , A.QUAL 
                        , A.CREATOR 
                        , A.MODIFIER	 
                        , A.CREATED	 
                        , A.MODIFIED 
                        , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                FROM ELN_IF.TB_FRAGRANCE A 
                /**where**/
                ORDER BY A.PK  
            ");

            builder.Where("A.SAMPLE_ID = :SAMPLE_ID ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 >  Frangrance Insert  
        public static SqlBuilder.Template FrangranceInsert4Analysis(FragranceModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        INSERT INTO ELN_IF.TB_FRAGRANCE (
                          SAMPLE_ID 
                        , SAMPLE_NAME	 
                        , PK 
                        , RT	 
                        , AREA_PCT 
                        , LIBRARY_ID 
                        , REF 
                        , CAS 
                        , QUAL 
                        , CREATOR 
                        , CREATED	 
                        , ANAL_ID
                ) VALUES (
                          :SAMPLE_ID 
                        , :SAMPLE_NAME	 
                        , :PK 
                        , :RT	 
                        , :AREA_PCT 
                        , :LIBRARY_ID 
                        , :REF 
                        , :CAS 
                        , :QUAL 
                        , :SessionInfo.USER_ID    
                        , SYSDATE	 
                        , :ANAL_ID
                ) 
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > Frangrance 삭제 
        public static SqlBuilder.Template FrangranceDelete4Analysis(FragranceModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_FRAGRANCE         
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플 리스트 > Frangrance 샘플명 중목 체크   
        public static SqlBuilder.Template GetFrangranceSampleNameCount(FragranceModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT COUNT(1) AS CNT 
                FROM ELN_IF.TB_FRAGRANCE A 
                /**where**/
            ");

            builder.Where("A.SAMPLE_ID != :SAMPLE_ID ");  // 현재 샘플 제외하고 

            builder.Where("A.SAMPLE_NAME = :SAMPLE_NAME "); // 샘플명 일치 

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

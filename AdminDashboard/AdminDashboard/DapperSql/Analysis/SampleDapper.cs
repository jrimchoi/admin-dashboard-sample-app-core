using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;
using DSELN.Models.Sample;

namespace DSELN.DapperSql.Analysis
{
    public class SampleDapper
    {
        public static SqlBuilder.Template GetSample(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_ITEM A
                /**where**/
            ");

            builder.Where("A.SAMPLE_ID = :SAMPLE_ID"); 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        public static SqlBuilder.Template SampleInsert(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_SAMPLE (
		            SAMPLE_ID
                    ,ANAL_ID     
                    ,TMPL_ID     
                    ,USE_DEPT     
                    ,TMPL_TYPE     
                    ,SAMPLE_NAME
                    ,FILE_PATH
                    ,SEQ_LINE
                    ,VIAL
                    ,PEAKS
                    ,ACQ_METH
                    ,STATE
                    ,CREATOR
                    ,MODIFIER
                    ,CREATED
                    ,MODIFIED
                    , DILUTION 
                    , SAMPLE_TYPE  
		        ) 
		        VALUES ( 
		            :SAMPLE_ID                    
                    ,:ANAL_ID     
                    ,:TMPL_ID     
                    ,:USE_DEPT     
                    ,:TMPL_TYPE     
                    ,:SAMPLE_NAME
                    ,:FILE_PATH
                    ,:SEQ_LINE
                    ,:VIAL
                    ,:PEAKS_CLOB
                    ,:ACQ_METH
                    ,:STATE
                    ,:SessionInfo.USER_ID   
                    ,:SessionInfo.USER_ID 
                    ,SYSDATE    
                    ,SYSDATE    
                    , :DILUTION 
                    , :SAMPLE_TYPE  
		        )
             ");
            var parameter = new DynamicParameters();
            parameter.Add("PEAKS_CLOB", model.PEAKS);
            builder.AddParameters(parameter);

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        public static SqlBuilder.Template SampleUpdate(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_SAMPLE       
		        SET 
                      ANAL_ID      =  :ANAL_ID
                    , TMPL_ID    =  :TMPL_ID
                    , USE_DEPT    =  :USE_DEPT
                    , TMPL_TYPE    =  :TMPL_TYPE
                    , SAMPLE_NAME    =  :SAMPLE_NAME
                    , SEQ_LINE    =  :SEQ_LINE
                    , VIAL    =  :VIAL
                    , ACQ_METH    =  :ACQ_METH
                    , PEAKS    =  :PEAKS
                    , STATE    =  :STATE
                    , FILE_PATH    =  :FILE_PATH
                    , MODIFIER           =  :SessionInfo.USER_ID      
                    , MODIFIED           =  SYSDATE     

                    , DILUTION           =  :DILUTION   
                    , SAMPLE_TYPE      =  :SAMPLE_TYPE 
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플 리스트 조회 
        public static SqlBuilder.Template GetSampleList(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH SMPL AS ( 
                    SELECT A.SAMPLE_ID
                              , A.ANAL_ID
                              , A.TMPL_ID
                              , A.USE_DEPT
                              , A.TMPL_TYPE
                              , A.SAMPLE_NAME
                              , A.SEQ_LINE
                              , A.VIAL
                              , A.ACQ_METH
                              , PEAKS
                              , A.STATE
                              , A.CREATOR
                              , A.FILE_PATH

                              , A.DILUTION  
                              , A.SAMPLE_TYPE 

                              , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/

                    FROM ELN_IF.TB_SAMPLE A 
                    /**where**/
                ) 

              , BRV AS ( /*** BRA 챠트용 ***/ 
                    SELECT FLOOR(B.TIME_MS) AS TIME_MS
                              , B.SAMPLE_ID AS SAMPLE_ID
                              , B.SAMPLE_NAME AS SAMPLE_NAME                              
                              , B.VISCOSITY AS VISCOSITY
                              , B.TEMPERATURE 
                    FROM SMPL A
                            , ELN_IF.VW_BRA_PEAK B 
                    WHERE A.SAMPLE_ID = B.SAMPLE_ID 
                ) 
                , RE AS (
                    SELECT A.SAMPLE_ID 
                              , A.SAMPLE_NAME 
                              , A.TIME_MS
                              , CASE WHEN A.TIME_MS = 0 THEN MIN(A.VISCOSITY) ELSE  MAX(A.VISCOSITY) END AS VISCOSITY
                              , CASE WHEN A.TIME_MS = 0 THEN MIN(A.TEMPERATURE) ELSE  MAX(A.TEMPERATURE) END AS TEMPERATURE
                    FROM BRV A 
                    GROUP BY A.SAMPLE_ID
                                  , A.SAMPLE_NAME
                                  , A.TIME_MS
                    ORDER BY A.TIME_MS 
                )
                , AGG AS ( 
                    SELECT A.SAMPLE_ID 
                              , A.SAMPLE_NAME 
                              , LISTAGG(A.VISCOSITY, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS VISCOSITY
                              , LISTAGG(A.TEMPERATURE, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS TEMPERATURE
                              , LISTAGG(A.TIME_MS, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS TIME_MS
                    FROM RE A 
                    GROUP BY A.SAMPLE_ID
                                  , A.SAMPLE_NAME 
                    ORDER BY A.SAMPLE_NAME 
                )
                , SMPL_FOLDER_PATH AS (
                    SELECT CONCAT(CONCAT(CONCAT(CONCAT(A.EQUIP_ID, '/'), A.FOLDER_NAME), '/'), A.FILE_PATH) AS FOLDER_PATH
                    FROM TB_EXPERIMENT_RESULT A
                    WHERE A.EXP_ID = :EXP_ID
                )
                
                SELECT A.* 
                          , B.VISCOSITY
                          , B.TEMPERATURE
                          , B.TIME_MS
                          , CONCAT(CONCAT(C.FOLDER_PATH,'/'), A.FILE_PATH) AS FILE_FULL_PATH
                FROM SMPL A 
                        , AGG B 
                        , SMPL_FOLDER_PATH C
                WHERE A.SAMPLE_ID = B.SAMPLE_ID (+) 
                ORDER BY A.SAMPLE_ID
            ");

            builder.Where("A.ANAL_ID = :ANAL_ID ");

            if (model.SAMPLE_ID != null && model.SAMPLE_ID != 0)   
            {
                builder.Where("A.SAMPLE_ID = :SAMPLE_ID");
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }
 
        // 실험결과분석 > 샘플 업데이트 
        public static SqlBuilder.Template SampleUpdate4Analysis(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_SAMPLE        
		        SET SAMPLE_NAME    =  :SAMPLE_NAME   
                    , DILUTION     =  :DILUTION   
                    , SAMPLE_TYPE    =  :SAMPLE_TYPE   

                     , MODIFIER           =  :SessionInfo.USER_ID      
                     , MODIFIED           =   SYSDATE     
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과 분석 > 분석완료 변경 및 저장
        public static SqlBuilder.Template ChangeSampleState(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_SAMPLE        
		        SET SAMPLE_NAME    =  :SAMPLE_NAME   
                    , DILUTION     =  :DILUTION   
                    , SAMPLE_TYPE    =  :SAMPLE_TYPE
                    , STATE = 'ANALYZED'
                     , MODIFIER           =  :SessionInfo.USER_ID      
                     , MODIFIED           =   SYSDATE     
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플삭제 
        public static SqlBuilder.Template SampleDelete4Analysis(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_SAMPLE         
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
 
        // 실험결과분석 > 샘플 분석상태 업데이트  
        public static SqlBuilder.Template StateUpdate4Analysis(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_SAMPLE        
		        SET STATE                =  :STATE   
                     , MODIFIER           =  :SessionInfo.USER_ID      
                     , MODIFIED           =   SYSDATE     
		        WHERE SAMPLE_ID = :SAMPLE_ID  
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 분석결과&샘플 조회 (권한체크용)   
        public static SqlBuilder.Template GetExpAnalysisSample(SampleSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.SAMPLE_ID  
                          , A.ANAL_ID 
                          , A.STATE 
                          , B.CREATOR AS CREATOR_ID  /*** 권한 체크용 ***/
                          , B.EXP_NO 
                          , B.STATE AS EXP_ANAL_STATE
                FROM ELN_IF.TB_SAMPLE A 
                       , ELN_IF.TB_EXPERIMENT_ANALYSIS B 
                /**where**/     
            ");

            builder.Where("A.ANAL_ID = B.ANAL_ID "); 

            builder.Where("A.ANAL_ID     = :ANAL_ID ");
            builder.Where("A.SAMPLE_ID = :SAMPLE_ID ");
 
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 분석결과 조회 (권한체크용)   
        public static SqlBuilder.Template GetExpAnalysis(SampleSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.ANAL_ID 
                          , A.CREATOR AS CREATOR_ID  /*** 권한 체크용 ***/
                          , A.EXP_NO 
                          , A.STATE AS EXP_ANAL_STATE
                          , A.TMPL_TYPE AS TMPL_TYPE
                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS A 
                /**where**/     
            ");

            builder.Where("A.ANAL_ID     = :ANAL_ID ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플 ANALYZED & 잔여 샘플수  
        public static SqlBuilder.Template GetSampleAnalyzedRemain(SampleSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH TOT AS (
                  SELECT COUNT(1) AS CNT 
                  FROM ELN_IF.TB_SAMPLE A
                  WHERE A.ANAL_ID = :ANAL_ID
                )
                SELECT COUNT(1) AS CNT 
                         ,  A.STATE 
                         , B.CNT AS TOT_CNT 
                         , B.CNT - COUNT(1) AS REM_CNT
                FROM ELN_IF.TB_SAMPLE A 
                       , TOT B 
                WHERE 1=1 
                  AND A.ANAL_ID = :ANAL_ID
                  AND A.STATE   = 'ANALYZED'
                GROUP BY  A.STATE
                                , B.CNT
            ");
 
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /****************************************************************
        // 분석결과 삭제 
        ****************************************************************/
        // 실험결과분석 > 분석항목삭제 (anal id) 
        public static SqlBuilder.Template ItemDeleteByAnalId(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_ITEM 
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 샘플삭제 (anal id) 
        public static SqlBuilder.Template SampleDeleteByAnalId(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_SAMPLE         
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 실험결과분석 > 실험결과분석 삭제 (anal id) 
        public static SqlBuilder.Template ExpAnalysisDeleteByAnalId(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_EXPERIMENT_ANALYSIS         
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // VISCO_BRA - Brabender Graph 저장 Search
        public static SqlBuilder.Template SearchBrabenderGraphData(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.SAMPLE_ID, A.SAMPLE_NAME
                FROM ELN_IF.TB_BRA_SAMPLE A
                /**where**/     
             ");
            builder.Where("A.SAMPLE_ID     != :SAMPLE_ID ");
            builder.Where("A.SAMPLE_NAME     = :SAMPLE_NAME ");
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // VISCO_BRA - Brabender Graph 저장 Insert (TB_BRA_SAMPLE)
        public static SqlBuilder.Template InsertBrabenderGraphData_TB_BRA_SAMPLE(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                -- TB_BRA_SAMPLE
                INSERT INTO ELN_IF.TB_BRA_SAMPLE
                SELECT A.SAMPLE_ID
                , A.SAMPLE_NAME
                , A.BOG
                , A.PEAK
                , A.FINAL
                , A.SB
                , A.BD
                , A.CREATOR
                , A.MODIFIER
                , A.CREATED
                , A.MODIFIED
                FROM V_TB_BRA_SAMPLE A
                /**where**/   
             ");
            builder.Where("A.ANAL_ID     = :ANAL_ID ");
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // VISCO_BRA - Brabender Graph 저장 Insert (TB_BRA_DATA)
        public static SqlBuilder.Template InsertBrabenderGraphData_TB_BRA_DATA(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                -- TB_BRA_DATA
                INSERT INTO ELN_IF.TB_BRA_DATA
                SELECT A.SAMPLE_ID
                , A.SAMPLE_NAME
                , A.MINUTE
                , A.VISCOSITY
                , A.TEMPERATURE
                , A.CREATOR
                , A.MODIFIER
                , A.CREATED
                , A.MODIFIED
                FROM V_TB_BRA_DATA A
                /**where**/   
             ");
            builder.Where("A.ANAL_ID     = :ANAL_ID ");
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // VISCO_BRA - Brabender Graph 저장 Delete (TB_BRA_SAMPLE)
        public static SqlBuilder.Template DeleteBrabenderGraphData_TB_BRA_SAMPLE(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                -- TB_BRA_SAMPLE
                DELETE FROM ELN_IF.TB_BRA_SAMPLE
                /**where**/
             ");
            builder.Where("SAMPLE_ID     = :SAMPLE_ID ");
            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // VISCO_BRA - Brabender Graph 저장 Delete (TB_BRA_DATA)
        public static SqlBuilder.Template DeleteBrabenderGraphData_TB_BRA_DATA(SampleModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                -- TB_BRA_DATA
		        DELETE FROM ELN_IF.TB_BRA_DATA         
		        /**where**/
             ");
            builder.Where("SAMPLE_ID     = :SAMPLE_ID ");
            return DynamicParameterHelper.RefineSql(sql, model);
        }


        /****************************************************************
        // Brabender 그래프 조회
        ****************************************************************/
        // Brabender 그래프 > 샘플별 챠트 데이터 
        public static SqlBuilder.Template GetSeriesData(SampleSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH BRA AS (
                    SELECT FLOOR(A.MINUTE) AS TIME_MS
                              , A.SAMPLE_NAME AS SAMPLE_NAME
                              --, A.SAMPLE_ID AS SAMPLE_ID
                              , A.VISCOSITY AS VISCOSITY
                              , A.TEMPERATURE 
                    FROM ELN_IF.TB_BRA_DATA A 
                    /**where**/     
                ) 
                , RE AS (
                    SELECT A.SAMPLE_NAME AS SAMPLE_NAME
                              --, A.SAMPLE_ID AS SAMPLE_ID
                              , A.TIME_MS
                              , CASE WHEN A.TIME_MS = 0 THEN MIN(A.VISCOSITY) ELSE  MAX(A.VISCOSITY) END AS VISCOSITY
                              , CASE WHEN A.TIME_MS = 0 THEN MIN(A.TEMPERATURE) ELSE  MAX(A.TEMPERATURE) END AS TEMPERATURE
                              , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                    FROM BRA A 
                    GROUP BY A.SAMPLE_NAME
                                  --, A.SAMPLE_ID 
                                  , A.TIME_MS
                    ORDER BY A.SAMPLE_NAME
                                  , A.TIME_MS 
                )
                , BRA_DATA AS ( SELECT A.SAMPLE_NAME 
                          --, A.SAMPLE_ID 
                          , LISTAGG(A.VISCOSITY, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS VISCOSITY
                          , LISTAGG(A.TEMPERATURE, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS TEMPERATURE      
                          , LISTAGG(A.TIME_MS, ',') WITHIN GROUP (ORDER BY A.TIME_MS ASC) AS TIME_MS      
                          , A.PAGE_TOT_ROWS
                FROM RE A 
                GROUP BY A.SAMPLE_NAME 
                             --, A.SAMPLE_ID 
                             , A.PAGE_TOT_ROWS
                ORDER BY A.SAMPLE_NAME
                )
                SELECT A.PAGE_TOT_ROWS
                    , A.SAMPLE_NAME
                    , B.SAMPLE_ID
                    , B.BOG
                    , B.PEAK
                    , B.FINAL
                    , B.SB
                    , B.BD
                    , A.VISCOSITY
                    , A.TEMPERATURE
                    , A.TIME_MS
                FROM BRA_DATA A
                    , TB_BRA_SAMPLE B
                WHERE A.SAMPLE_NAME = B.SAMPLE_NAME
                
                " + DynamicParameterHelper.SetPaginCondition(model) // paging 

                + @"");

            if (!string.IsNullOrEmpty(model.SAMPLE_NAME))  
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.SAMPLE_NAME LIKE '%' || :SAMPLE_NAME || '%'  ");
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

using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;

namespace DSELN.DapperSql.Analysis
{
    public class ExperimentAnalysisAttributeDapper
    {
        public static SqlBuilder.Template GetExperimentAnalysisAttribute(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR A");
            builder.Where("A.EAA_ID=:EAA_ID");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template ExperimentAnalysisAttributeInsert(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR (
                    EAA_ID
                    ,ANAL_ID
                    ,SAMPLE_ID
                    ,SAMPLE_ORDER
                    ,ROW_NO
                    ,COLUMN_NO
                    ,ATTR_NAME
                    ,TITLE
                    ,ATTR_TYPE
                    ,ATTR_VALUE
                    ,FORMULA
                    ,WIDTH
                    ,CREATED
                    ,MODIFIED
		        ) 
		        VALUES ( 
                    :EAA_ID
                    ,:ANAL_ID
                    ,:SAMPLE_ID
                    ,:SAMPLE_ORDER
                    ,:ROW_NO
                    ,:COLUMN_NO
                    ,:ATTR_NAME
                    ,:TITLE
                    ,:ATTR_TYPE
                    ,:ATTR_VALUE
                    ,:FORMULA
                    ,:WIDTH
                    ,SYSDATE
                    ,SYSDATE
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template ExperimentAnalysisAttributeUpdate(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR
		        SET 
                    ATTR_VALUE      = :ATTR_VALUE
                   ,MODIFIED       = SYSDATE  
		        WHERE 1=1
                AND EAA_ID = :EAA_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template ExperimentAnalysisAttributeUpdateFormula(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR
		        SET 
                    FORMULA      = :FORMULA
                   ,MODIFIED       = SYSDATE  
		        WHERE 1=1
                AND SAMPLE_ID = :SAMPLE_ID  
                AND ROW_NO = :ROW_NO  
                AND COLUMN_NO = :COLUMN_NO  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template GetListForSample(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR A
                WHERE A.SAMPLE_ID=:SAMPLE_ID
                ORDER BY EAA_ID            ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template GetListForExperimentAnalysis(ExperimentAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                
                SELECT *
                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR A
                WHERE A.ANAL_ID=:ANAL_ID
                ORDER BY SAMPLE_ID,EAA_ID
            ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template DeleteExperimentAnalyaisAttribute(ExperimentAnalysisAttributeModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        DELETE FROM ELN_IF.TB_EXPERIMENT_ANALYSIS_ATTR         
		        WHERE ANAL_ID = :ANAL_ID  
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

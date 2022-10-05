using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;

namespace DSELN.DapperSql.Analysis
{
    public class ExperimentAnalysisDapper
    {
        public static SqlBuilder.Template GetExperimentAnalysis(ExperimentAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.*    
                          , A.CREATOR AS CREATOR_ID  /*** 권한 체크용 ***/
                FROM ELN_IF.TB_EXPERIMENT_ANALYSIS A
                WHERE A.ANAL_ID = :ANAL_ID");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template UpdateExperimentAnalysisState(ExperimentAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                UPDATE ELN_IF.TB_EXPERIMENT_ANALYSIS A
                SET STATE = :STATE
                WHERE A.ANAL_ID = :ANAL_ID");

            return DynamicParameterHelper.RefineSql(sql, model);
        }
        public static SqlBuilder.Template UpdateExperimentAnalysisExpNo(ExperimentAnalysisModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                UPDATE ELN_IF.TB_EXPERIMENT_ANALYSIS A
                SET EXP_NO = :EXP_NO
                WHERE A.ANAL_ID = :ANAL_ID");



            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}
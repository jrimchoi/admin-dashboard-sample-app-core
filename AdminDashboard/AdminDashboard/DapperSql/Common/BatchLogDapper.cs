using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Common;

namespace DSELN.DapperSql.Common
{
    public class BatchLogDapper
    {
        public static SqlBuilder.Template BatchLogInsert(BatchLogModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                INSERT INTO ELN_IF.TB_BATCH_LOG (
                    LOG_ID
                    ,PGM_NAME
                    ,LOG_PATH
                    ,RESULT
                    ,MSG
                    ,TARGET
                    ,CREATED
                    ,MODIFIED
		        ) 
		        VALUES ( 
		             LOG_SEQ.NEXTVAL
                    ,:PGM_NAME
                    ,:LOG_PATH
                    ,:RESULT
                    ,:MSG
                    ,:TARGET
		            , SYSDATE     
                    , SYSDATE
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

	}
}

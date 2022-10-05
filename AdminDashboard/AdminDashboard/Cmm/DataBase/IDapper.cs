using Dapper;
using System.Collections.Generic;
using static Dapper.SqlMapper;

namespace DSELN.Cmm.DataBase
{
    public interface IDapper
    {
        public T QuerySingle<T>(SqlBuilder.Template sql);

        public List<T> Query<T>(SqlBuilder.Template sql);

        // list 조회 : Model 불필요 
        public List<Dictionary<string, string>> Query4NoModel(SqlBuilder.Template sql);

        // 조회(프로시져) : Model 필요   sql, parameters
        public List<T> Query4SP<T>(string sql, object parameters);

        // 조회(프로시져) : Model 불필요   
        public List<Dictionary<string, string>> Query4SP4NoModel(string sql, object parameters);

        // count  
        public long Count(SqlBuilder.Template sql);

        // Multi Result Query 
        public GridReader QueryMultiple(string sql, object parameters);

        // 처리 insert/update/delete 
        public int Execute(SqlBuilder.Template sql);
    }
}

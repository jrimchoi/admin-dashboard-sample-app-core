using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static Dapper.SqlMapper;

namespace DSELN.Cmm.DataBase
{
    public class OracleDapper : IDapper, IDisposable   // IDisposable 명시적 메모리관리를 위해 
    {
        private readonly string _connectionString;

        OracleConnection _cnn = null;
        OracleTransaction _tran = null;

        public OracleDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");  // dev.DBConnection / test.DBConnection / live.DBConnection
            _cnn = new OracleConnection(this._connectionString);
            // Log.Debug("OracleDapperHelper calling...");
        }

        public T QuerySingle<T>(SqlBuilder.Template sql)  // tuple.Item1 = sql, tuple.Item2 = model object
        {
            var result = SqlMapper.Query<T>(this._cnn, sql.RawSql, sql.Parameters, this._tran).AsList<T>();
            if (result.Count == 0)
            {
                return default(T);
            }
            else
            {
                return result[0];
            }
            // SqlMapper.Query<T>(this._cnn, sql.RawSql, sql.Parameters, this._tran).AsList<T>()[0]; 
        }

        // list 조회 : Model 필요  
        public List<T> Query<T>(SqlBuilder.Template sql)
        {
            return SqlMapper.Query<T>(this._cnn, sql.RawSql, sql.Parameters, this._tran).ToList();
        }

        // list 조회 : Model 불필요 
        public List<Dictionary<string, string>> Query4NoModel(SqlBuilder.Template sql)
        {
            DataTable dt = new DataTable("MyTable");
            dt.Load(SqlMapper.ExecuteReader(this._cnn, sql.RawSql, sql.Parameters, this._tran));

            var result = new List<Dictionary<string, string>>(); //or var result = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    dictRow.Add(col.ColumnName, (row[col] == null ? "" : row[col].ToString()));  //or dictRow.Add(col.ColumnName, row[col].ToString());
                    //Console.WriteLine("Dictionary => " +  col.ColumnName + ":" + row[col] as String);
                }

                result.Add(dictRow);
            }
            return result;
        }

        // list 조회 : Model 불필요 
        public List<Dictionary<string, object>> Query4NoModel2(SqlBuilder.Template sql)
        {
            DataTable dt = new DataTable("MyTable");
            dt.Load(SqlMapper.ExecuteReader(this._cnn, sql.RawSql, sql.Parameters, this._tran));

            var result = new List<Dictionary<string, object>>(); //or var result = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dictRow.Add(col.ColumnName, (row[col] == null ? "" : row[col].ToString()));  //or dictRow.Add(col.ColumnName, row[col].ToString());
                    //Console.WriteLine("Dictionary => " +  col.ColumnName + ":" + row[col] as String);
                }

                result.Add(dictRow);
            }
            return result;
        }
        // 조회(프로시져) : Model 필요   sql, parameters
        public List<T> Query4SP<T>(string sql, object parameters)
        {
            return SqlMapper.Query<T>(this._cnn, sql, parameters, this._tran,
                                                      commandType: CommandType.StoredProcedure).ToList();
        }

        // 조회(프로시져) : Model 불필요   
        public List<Dictionary<string, string>> Query4SP4NoModel(string sql, object parameters)
        {
            DataTable dt = new DataTable("MyTable");
            dt.Load(SqlMapper.ExecuteReader(this._cnn, sql, parameters, this._tran,
                                                                commandType: CommandType.StoredProcedure));

            var result = new List<Dictionary<string, string>>(); //or var result = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    dictRow.Add(col.ColumnName, (row[col] == null ? "" : row[col].ToString()));  //or dictRow.Add(col.ColumnName, row[col].ToString());
                    //Console.WriteLine("Dictionary => " +  col.ColumnName + ":" + row[col] as String);
                }

                result.Add(dictRow);
            }

            return result;
        }

        // count  
        public long Count(SqlBuilder.Template sql)
        {
            return SqlMapper.Query<long>(this._cnn, sql.RawSql, sql.Parameters, this._tran).Single();  // 실제 쿼리에서 count 넘겨줄것 
        }

        // sequence  
        public int Sequence(SqlBuilder.Template sql)
        {
            return SqlMapper.Query<int>(this._cnn, sql.RawSql, sql.Parameters, this._tran).Single();
        }

        // Multi Result Query 
        public GridReader QueryMultiple(string sql, object parameters)
        {
            return SqlMapper.QueryMultiple(this._cnn, sql, parameters, this._tran);
        }

        // 처리 insert/update/delete 
        public int Execute(SqlBuilder.Template sql)
        {
            return SqlMapper.Execute(this._cnn, sql.RawSql, sql.Parameters, this._tran);
        }

        // 처리(프로시져)  
        public int SPExec(string sql, object parameters)
        {
            return SqlMapper.Execute(this._cnn, sql, parameters, this._tran, commandType: CommandType.StoredProcedure);
        }

        public void BeginTransaction()
        {
            this._cnn.Open();
            this._tran = this._cnn.BeginTransaction();
        }

        private void Commit() // not used.....
        {
            this._tran.Commit();
            this._cnn.Close();
            this._tran = null;
        }


        private void Rollback() // not used.....
        {
            this._tran.Rollback();
            this._cnn.Close();
            this._tran = null;
        }

        #region 명시적 메모리관리 
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    if (this._tran != null)
                    {
                        this._tran.Rollback();
                        this._tran.Dispose();
                    }

                    this._cnn.Dispose();
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~OracleDapperHelper()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }

}

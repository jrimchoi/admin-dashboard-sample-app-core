using Dapper;
using Dapper.Oracle;
using DSELN.Models;
using DSELN.Models.Login;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DSELN.Cmm.Helper
{
    public static class DynamicParameterHelper
    {
        public static string SetPaginCondition(BaseSearchModel model)
        {
            if (!model._PAGE_ABLE || model._PAGE_SIZE == 0)
            {
                return string.Empty;
            }
            model._START_ROW = (model._PAGE_SIZE * model._PAGE_INDEX) - model._PAGE_SIZE;

            return " OFFSET " + model._START_ROW.ToString() + " ROWS FETCH FIRST " + model._PAGE_SIZE.ToString() + " ROWS ONLY ";
        }

        public static SqlBuilder.Template RefineSql(SqlBuilder.Template sql, object model)
        {
            string sqlStr = sql.RawSql.Replace(":SessionInfo.", ":SessionInfo_", StringComparison.OrdinalIgnoreCase);

            var builderX = new SqlBuilder();

            builderX.AddParameters(model);
            builderX.AddParameters(sql.Parameters);

            // 현재 세션의 세션정보 
            SessionModel sessionInfo = SessionHelper.GetSessionModel(AppHttpContext.Current.Session);
            if (sessionInfo == null)
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                //var builder = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("appsettings.json");
                //var _config = builder.Build();


                // 개발환경에서 cshtml 변경시 다시 빌드해야하므로 세션아웃후 재로그인해야됨. 생산성이 떨어짐. 
                if ("Development".ToUpper().Equals(environment.ToUpper()))
                {
                    var session = new Dictionary<string, object>();
                    session.Add("SessionInfo_" + "SYS_ID", "SYS10");
                    session.Add("SessionInfo_" + "USER_ID", "10140265");
                    session.Add("SessionInfo_" + "USER_NM", "no session");
                    session.Add("SessionInfo_" + "CLIENT_IP_ADDR", ":1:1:1");
                    session.Add("SessionInfo_" + "LANG_CD", "KO");
                    session.Add("SessionInfo_" + "USER_ROLE", "USER");

                    builderX.AddParameters(session);
                }

            }
            else
            {
                var session = new Dictionary<string, object>();
                foreach (var item in sessionInfo.GetType().GetProperties())
                {
                    session.Add("SessionInfo_" + item.Name, item.GetValue(sessionInfo));
                }

                builderX.AddParameters(session);
            }

            var sqlX = builderX.AddTemplate(sqlStr);

            DynamicParameterHelper.GetMappedSql(sqlX);

            return sqlX;
        }

        // dapper 쿼리 스크립트
        public static void GetMappedSql(SqlBuilder.Template sql)
        {
            var sql_parameters = new Dapper.DynamicParameters(sql.Parameters);
            var parameters = DynamicParameterHelper.ToParametersDictionary(sql_parameters);

            string sqlLog = sql.RawSql.ToString(); // 실제 dapper 에 매핑되는 쿼리 스크립트 
            string paramStr = "";

            foreach (var item in parameters)
            {
                //Log.Debug(item.Key + " : " + item.Value);
                string key = item.Key.ToString();
                var value = item.Value;

                if (key.IndexOf("Model") >= 0)
                {
                    //Log.Debug("skip....... : " + item.Key + " : " + item.Value);
                    continue;
                }

                string type = "";
                if (value != null)
                {
                    type = (value.GetType().Name as String ?? String.Empty).ToLower();
                }

                paramStr += item.Key + "={" + (value as String ?? String.Empty) + "}, ";

                if (type.IndexOf("int") >= 0 || type.IndexOf("long") >= 0 || type.IndexOf("double") >= 0)
                {
                    if (value == null)
                    {
                        sqlLog = sqlLog.Replace(":" + key, "", StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        sqlLog = Utils.Utils.ReplaceWholeWord(sqlLog, ":" + key, value.ToString(), RegexOptions.IgnoreCase);
                    }
                }
                else
                {
                    sqlLog = Utils.Utils.ReplaceWholeWord(sqlLog, ":" + key, "'" + (value as String ?? String.Empty) + "'", RegexOptions.IgnoreCase);
                }
            }

            Log.Debug("DapperSql parameters [참고용] : " + paramStr.Replace("SessionInfo_", "SessionInfo.", StringComparison.OrdinalIgnoreCase));  // 입력 파라미터 로그 
            Log.Debug("DapperSql sql [참고용]: [실제 매핑되는 쿼리와 다를수 있습니다.]" + "\r" + sqlLog); // 쿼리 
        }

        private static Dictionary<string, object> ToParametersDictionary(this DynamicParameters dynamicParams)
        {
            var argsDictionary = new Dictionary<String, Object>();
            var iLookup = (SqlMapper.IParameterLookup)dynamicParams;

            foreach (var paramName in dynamicParams.ParameterNames)
            {
                var value = iLookup[paramName];
                argsDictionary.Add(paramName, value);
            }

            var templates = dynamicParams.GetType().GetField("templates", BindingFlags.NonPublic | BindingFlags.Instance);
            if (templates != null)
            {
                var list = templates.GetValue(dynamicParams) as List<Object>;
                if (list != null)
                {
                    // add properties of each dynamic parameters section
                    foreach (var objProps in list.Select(obj => obj.GetPropertyValuePairs().ToList()))
                    {
                        objProps.ForEach(p => argsDictionary.Add(p.Key, p.Value));
                    }
                }
            }

            return argsDictionary;
        }

        private static Dictionary<string, object> GetPropertyValuePairs(this object obj, String[] hidden = null)
        {
            var type = obj.GetType();
            var pairs = hidden == null
                ? type.GetProperties()
                    .DistinctBy(propertyInfo => propertyInfo.Name)
                    .ToDictionary(
                        propertyInfo => propertyInfo.Name,
                        propertyInfo => propertyInfo.GetValue(obj, null))
                : type.GetProperties()
                    .Where(it => !hidden.Contains(it.Name))
                    .DistinctBy(propertyInfo => propertyInfo.Name)
                    .ToDictionary(
                        propertyInfo => propertyInfo.Name,
                        propertyInfo => propertyInfo.GetValue(obj, null));
            return pairs;
        }

        private static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        private static void SetFor<T>(string prefix = null)   // 컬럼 재매핑 USER_ID --> USERID
        {
            //Console.WriteLine("SetFor name : " + typeof(T).Name);
            if (prefix == null) prefix = typeof(T).Name + ".";
            var typeMap = new CustomPropertyTypeMap(typeof(T), (type, name) =>
            {
                Console.WriteLine("SetFor name : " + name);
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    name = name.Substring(prefix.Length);
                return type.GetProperty(name);
            });
            SqlMapper.SetTypeMap(typeof(T), typeMap);
        }
    }
}

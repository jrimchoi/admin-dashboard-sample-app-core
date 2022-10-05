
using DSELN.Cmm.Utils;
using System;

namespace DSELN.Cmm.Helper
{
    public static class SqlHelper
    {
        // dapper query /r /t 
        public static string PrefixWhere(int cnt)
        {
            string prefix = "\r";
            for (int i = 0; i < cnt; i++)
            {
                prefix += "\t";
            }
            return prefix;
        }

        // muliselector combo    'A, B' --> 'A','B' 
        public static string MultiConditonWhere(string multiValues)
        {
            string rtn = "";

            if (!string.IsNullOrEmpty(multiValues))
            {
                int index = 0;
                string[] words = multiValues.Split(',');
                foreach (var word in words)
                {
                    string value = word.ToString().Replace("'", "").Replace("[", "").Replace("]", "");
                    rtn += (index == 0 ? "" : " , ") + "  '" + value.ToString() + "' ";
                    Console.WriteLine($"<{value}>");
                    index++;
                }
            }
            return rtn;
        }

        // from Date where : date 형식의 컬럼을 조회조건으로 비교할때 
        // AND TO_CHAR(ORD_DATE, 'YYYYMMDD') >= '20210101'  (X)  --> 조회조건의 왼쪽 컬럼은 가공하지 않는다. 
        public static string FrDateWhere(string fromDate)
        {
            return "TO_DATE(TO_CHAR(TO_DATE(REPLACE(REPLACE(" + "'" + (string.IsNullOrEmpty(fromDate) ? "20000101" : fromDate) + "'" + ", '.', ''), '-', ''), 'YYYYMMDD') + 0, 'YYYYMMDD') || ' 00:00:00', 'YYYYMMDD HH24:MI:SS')";
        }

        // to Date where 
        // AND TO_CHAR(ORD_DATE, 'YYYYMMDD') <= '20210131'  (X)  --> 조회조건의 왼쪽 컬럼은 가공하지 않는다. 
        public static string ToDateWhere(string toDate)
        {
            return "TO_DATE(TO_CHAR(TO_DATE(REPLACE(REPLACE(" + "'" + (string.IsNullOrEmpty(toDate) ? "29991231" : toDate) + "'" + ", '.', ''), '-', ''), 'YYYYMMDD') + 1, 'YYYYMMDD') || ' 00:00:00', 'YYYYMMDD HH24:MI:SS')";
        }

        // file dir 
        public static string FilePathReplace(string col, string baseDir)
        {
            var baseDir2 = ConfigUtil.getSectionValue("ExpResultDir");
            return "REPLACE(" + col + ", '" + baseDir + "', '" + baseDir2 + "')";
        }

    }


}

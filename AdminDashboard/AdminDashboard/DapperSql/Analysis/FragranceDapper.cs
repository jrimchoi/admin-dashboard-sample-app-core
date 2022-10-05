using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models.Analysis;

namespace DSELN.DapperSql.Analysis
{
    public class FragranceDapper
    {
        /**************************************************************************
        // 분석템플릿-작성/조회
        **************************************************************************/
        // 분석템플릿-작성/조회
        public static SqlBuilder.Template GetFragranceList(ExpFragranceSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
	                SELECT COUNT(1) OVER() AS PAGE_TOT_ROWS
		                , A.PK
                        , A.ANAL_ID
		                , ROW_NUMBER() OVER(ORDER BY A.CREATED DESC) AS RN 
	                FROM ELN_IF.TB_FRAGRANCE A
	                /**where**/
                )
                SELECT A.PAGE_TOT_ROWS AS PAGE_TOT_ROWS
                    , B.SAMPLE_ID
                    , B.SAMPLE_NAME
                    , B.PK
                    , B.RT
                    , B.AREA_PCT
                    , B.LIBRARY_ID
                    , B.REF
                    , B.CAS
                    , B.QUAL
                    , D.NAME  AS CREATOR
		            , TO_CHAR( B.CREATED, 'YYYY-MM-DD HH24:MI:SS') AS CREATED
		            , TO_CHAR( B.MODIFIED, 'YYYY-MM-DD HH24:MI:SS') AS MODIFIED
                    , B.ANAL_ID
                FROM GRP A
	                , ELN_IF.TB_FRAGRANCE B
                    , ELN_IF.TB_USER D
                where A.PK = B.PK
                and A.ANAL_ID = B.ANAL_ID
                and B.CREATOR = D.IMUSERID
                ORDER BY A.ANAL_ID, A.PK
                "
                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");

            // ANAL_ID 조회
            if (model.ANAL_ID != null) // ANAL_ID 가 있을 경우, VALUE : <ANAL_ID>,<ANAL_ID>,... 형식
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.ANAL_ID in ( SELECT REGEXP_SUBSTR(A.TEXT, '[^,]+', 1, LEVEL) AS ANAL_ID_ITEM FROM (SELECT :ANAL_ID AS TEXT FROM dual) a CONNECT BY LEVEL <= LENGTH(REGEXP_REPLACE(A.TEXT, '[^,]+','')) + 1 )");
            }
            // 동적 파라미터 적용
            if (model.SAMPLE_NAME != null) // 샘플명
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.SAMPLE_NAME LIKE '%' || :SAMPLE_NAME || '%' ");
            }

            if (model.FR_RT != null)  // RT fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.RT >= :FR_RT ");
            }

            if (model.TO_RT != null) // RT to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.RT <= :TO_RT ");
            }

            if (model.FR_AREA_PCT != null)  // AREA_PCT fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.AREA_PCT >= :FR_AREA_PCT ");
            }

            if (model.TO_AREA_PCT != null) // AREA_PCT to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.AREA_PCT <= :TO_AREA_PCT ");
            }

            if (model.LIBRARY_ID != null) // Library_ID
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.LIBRARY_ID LIKE '%' || :LIBRARY_ID || '%' ");
            }

            if (model.CREATOR != null) // 작성자
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.EXP_OWNER IN (SELECT X.IMUSERID FROM ELN_IF.TB_USER X WHERE NAME LIKE '%' || :CREATOR || '%') ");
            }

            if (model.FR_DATE != null)  // 생성일시 fr 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED >= " + SqlHelper.FrDateWhere(model.FR_DATE));
            }

            if (model.TO_DATE != null) // 생성일시 to 
            {
                builder.Where(SqlHelper.PrefixWhere(6) + "A.CREATED < " + SqlHelper.ToDateWhere(model.TO_DATE));
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }
    }
}

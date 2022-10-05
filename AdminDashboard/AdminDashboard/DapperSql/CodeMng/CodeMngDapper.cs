using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;

namespace DSELN.DapperSql.CodeMng
{
    public class CodeMngDapper
    {
 
        /**************************************************************************
        // 코드유형  
        **************************************************************************/
        // 코드유형 조회 
        public static SqlBuilder.Template GetCodeTypeList(BaseSearchModel model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
                  SELECT A.CD_TYP 
                           , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                  FROM ELN_IF.TB_ESA_CDTP A  
                  /**where**/
                  ORDER BY A.CD_TYP    
                ) 
                SELECT A.PAGE_TOT_ROWS AS PAGE_TOT_ROWS
                          , B.* 
                          , B.CD_TYP AS CD_TYP_KEY
                          , CONVERT(STANDARD_HASH (B.CD_TYP || 'hello' , 'SHA512'), 'UTF8') AS ZyHashKey  
                FROM GRP A
                       , ELN_IF.TB_ESA_CDTP B 
                WHERE A.CD_TYP = B.CD_TYP "

                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");
 
            return DynamicParameterHelper.RefineSql(sql, model);
        }


        // Insert 
        public static SqlBuilder.Template CodeTypeInsert(CodeType model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_ESA_CDTP (
		              SYS_ID     /***  시스템아이디    ***/
		            , CD_TYP    /***   코드유형코드   ***/
		            , CD_TYP_NM_KO   /***    코드유형 한글명   ***/
		            , CD_TYP_NM_EN   /***    코드유형 영문명   ***/
		            , CD_TYP_NM_ZH    /***   코드유형 중문명   ***/
		            , USE_YN     /***  사용여부   ***/
		            , REM     /***  비고   ***/
		            , STS     /***  상태   ***/
		            , CREATION_USER_ID    /***   등록자아이디   ***/
		            , CREATION_DATE     /***  최초등록일시   ***/ 
		        ) 
		        VALUES ( 
		              :SessionInfo.SYS_ID  
		            , :CD_TYP     
		            , :CD_TYP_NM_KO   
		            , :CD_TYP_NM_EN  
		            , :CD_TYP_NM_ZH   
		            , :USE_YN  
		            , :REM  
		            , :STS   
		            , :SessionInfo.USER_ID   
		            , SYSDATE     
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // Update 
        public static SqlBuilder.Template CodeTypeUpdate(CodeType model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ESA_CDTP
		        SET 
		              SYS_ID =  :SYS_ID 
		            , CD_TYP =  :CD_TYP    
		            , CD_TYP_NM_KO =  :CD_TYP_NM_KO  
		            , CD_TYP_NM_EN =  :CD_TYP_NM_EN   
		            , CD_TYP_NM_ZH =  :CD_TYP_NM_ZH    
		            , USE_YN =  :USE_YN      
		            , REM =  :REM  
		            , STS =  :STS       
		            , LAST_UPDATE_USER_ID = :SessionInfo.USER_ID
		            , LAST_UPDATE_DATE = SYSDATE     
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND CD_TYP = :CD_TYP_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // Delete 
        public static SqlBuilder.Template CodeTypeDelete(CodeType model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                DELETE ELN_IF.TB_ESA_CDTP A 
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND CD_TYP = :CD_TYP_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Where("A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /**************************************************************************
        // 코드그룹  
        **************************************************************************/
        // 코드그룹 리스트 조회 
        public static SqlBuilder.Template GetCodeGroupList(CodeGroupSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
                  SELECT A.GRP_CD 
                           , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                  FROM ELN_IF.TB_ESA_CDGP A  
                  /**where**/
                  ORDER BY A.GRP_CD    
                ) 
                SELECT A.PAGE_TOT_ROWS AS PAGE_TOT_ROWS
                          , B.* 
                          , B.GRP_CD AS GRP_CD_KEY
                FROM GRP A
                       , ELN_IF.TB_ESA_CDGP B
                WHERE A.GRP_CD = B.GRP_CD "

                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");


            // 동적 파라미터 적용 
            if (model.CD_TYP != null)
            {
                builder.Where("A.CD_TYP = :CD_TYP  ");
            }

            if (model.LANG_CD != null)
            {
                builder.Where("A.LANG_CD = :LANG_CD  ");
            }

            if (model.GRP_CD != null)
            {
                builder.Where("A.GRP_CD = :GRP_CD  ");
            }
 
            //builder.Where("A.USER_ID =  :BB" , new { BB = model.USER_ID});  // model 에 없는 파라미터 추가 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

 
        // 코드그룹 Insert 
        public static SqlBuilder.Template CodeGroupInsert(CodeGroup model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_ESA_CDGP(
		              GRP_CD      /***  그룹코드    ***/
		            , SYS_ID     /***  시스템 ID    ***/
		            , REM     /*** 비고    ***/
		            , GRP_CD_NM_EN     /***  그룹 코드 영문명    ***/
		            , GRP_CD_NM_KO     /***  그룹 코드 한글명   ***/
		            , GRP_CD_NM_ZH     /***  그룹 코드 중문명   ***/
		            , USE_YN     /***  사용유무   ***/
		            , CD_TYP     /***  코드유형   ***/
		            , STS     /***  상태   ***/
		            , CREATION_USER_ID     
		            , CREATION_DATE     
		        ) 
		        VALUES ( 
		             :GRP_CD 
		            , :SessionInfo.SYS_ID  
		            , :REM 
		            , :GRP_CD_NM_EN 
		            , :GRP_CD_NM_KO 
		            , :GRP_CD_NM_ZH 
		            , :USE_YN 
		            , :CD_TYP 
		            , :STS 
		            , :SessionInfo.USER_ID
		            , SYSDATE 
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드그룹 Update 
        public static SqlBuilder.Template CodeGroupUpdate(CodeGroup model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ESA_CDGP
		        SET 
		             GRP_CD = :GRP_CD 
		            , REM = :REM 
		            , GRP_CD_NM_EN = :GRP_CD_NM_EN 
		            , GRP_CD_NM_KO = :GRP_CD_NM_KO 
		            , GRP_CD_NM_ZH = :GRP_CD_NM_ZH 
		            , USE_YN = :USE_YN 
		            , CD_TYP = :CD_TYP 
		            , STS = :STS 
		            , LAST_UPDATE_USER_ID = :SessionInfo.USER_ID
		            , LAST_UPDATE_DATE = SYSDATE     
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND GRP_CD = :GRP_CD_KEY   
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드그룹 Delete 
        public static SqlBuilder.Template CodeGroupDelete(CodeGroup model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                DELETE ELN_IF.TB_ESA_CDGP A 
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND GRP_CD = :GRP_CD_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Where("A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        /**************************************************************************
        // 코드 관리  
        **************************************************************************/

        // 코드그룹 마스터 조회 
        public static SqlBuilder.Template GetCodeGroup(CodeGroupSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH GRP AS  ( 
                  SELECT A.GRP_CD 
                           , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                  FROM ELN_IF.TB_ESA_CDGP A  
                  /**where**/
                  ORDER BY A.GRP_CD    
                ) 
                SELECT A.PAGE_TOT_ROWS AS PAGE_TOT_ROWS
                          , B.* 
                          , B.GRP_CD AS GRP_CD_KEY
                FROM GRP A
                       , ELN_IF.TB_ESA_CDGP B
                WHERE A.GRP_CD = B.GRP_CD    "

                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");


            // 동적 파라미터 적용 
            builder.Where("A.GRP_CD = :GRP_CD  ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드그룹 디테일  조회 
        public static SqlBuilder.Template GetCodeDetail(CodeGroupSearch model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                SELECT A.GRP_CD AS GRP_CD_KEY
                          , B.DTL_CD AS DTL_CD_KEY
                          , B.* 
                          , COUNT(1) OVER()  AS PAGE_TOT_ROWS  /*** paging required ***/
                FROM ELN_IF.TB_ESA_CDGP A
                       , ELN_IF.TB_ESA_CDDT B
                /**where**/
                ORDER BY B.SORT_ORD "

                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");

            // 동적 파라미터 적용 
            builder.Where("A.GRP_CD = B.GRP_CD ");
            builder.Where("A.GRP_CD = :GRP_CD  ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드디테일 Insert 
        public static SqlBuilder.Template CodeDetailInsert(CodeDetail model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"

		        INSERT INTO ELN_IF.TB_ESA_CDDT(
			          GRP_CD
			        , SYS_ID
			        , DTL_CD
			        , DTL_CD_NM_EN
			        , DTL_CD_NM_KO
			        , DTL_CD_NM_CN
			        , REM
			        , STS
			        , USE_YN
			        , SORT_ORD
			        , CHILD_GRP_CD
			        , CREATION_USER_ID
			        , CREATION_DATE 
		        ) 
		        VALUES ( 
			          :GRP_CD 
			        , :SessionInfo.SYS_ID 
			        , :DTL_CD 
			        , :DTL_CD_NM_EN 
			        , :DTL_CD_NM_KO 
			        , :DTL_CD_NM_CN 
			        , :REM 
			        , :STS 
			        , :USE_YN 
			        , :SORT_ORD 
			        , :CHILD_GRP_CD 
		            , :SessionInfo.USER_ID
		            , SYSDATE 
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드디테일 Update 
        public static SqlBuilder.Template CodeDetailUpdate(CodeDetail model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ESA_CDDT
		        SET GRP_CD = :GRP_CD 
		            , DTL_CD = :DTL_CD 
			        , DTL_CD_NM_EN = :DTL_CD_NM_EN 
			        , DTL_CD_NM_KO = :DTL_CD_NM_KO 
			        , DTL_CD_NM_CN = :DTL_CD_NM_CN 
			        , REM = :REM 
			        , STS = :STS 
			        , USE_YN = :USE_YN 
			        , SORT_ORD = :SORT_ORD  
			        , CHILD_GRP_CD = :CHILD_GRP_CD 
		            , LAST_UPDATE_USER_ID = :SessionInfo.USER_ID
		            , LAST_UPDATE_DATE = SYSDATE     
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND GRP_CD = :GRP_CD_KEY
			        AND DTL_CD = :DTL_CD_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 코드디테일 Delete 
        public static SqlBuilder.Template CodeDetailDelete(CodeDetail model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                DELETE ELN_IF.TB_ESA_CDDT A 
		        WHERE SYS_ID = :SessionInfo.SYS_ID  
			        AND GRP_CD = :GRP_CD_KEY
			        AND DTL_CD = :DTL_CD_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Where("A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

    }
}

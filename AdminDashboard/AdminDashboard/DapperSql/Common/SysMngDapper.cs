using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;

namespace DSELN.DapperSql.SysMng
{
    public class SysMngDapper
    {

        /**************************************************************************
        // 메뉴등록 
        **************************************************************************/
        // 메뉴등록 조회 
        public static SqlBuilder.Template GetMenuList(MenuSearch model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                WITH MENU_TREE (
                      MENU_CD	             /**	메뉴코드   a  **/ 
        			, SYS_ID	            /**  시스템아이디   **/ 
        			, UP_MENU_CD			/**  상위메뉴코드   **/ 
        			, REM	        		/**  비고   **/ 
        			, STS	        		/**  상태   **/ 
        			, LINK_URL			/**  링크URL   **/ 
        			, SORT_ORD			/**  정렬순서   **/ 
        			, MENU_TYP			/**  메뉴유형   **/ 
        			, USE_YN	    		/**  사용여부   **/ 
        			, USR_CLS	    		/**  사용자분류   **/ 
        			, MD_CLS	    		/**  모듈분류   **/ 
        			, DISPLAY_YN			/**  표시여부   **/ 
        			, MENU_NM_EN			/**  메뉴코드 영문명   **/ 
        			, MENU_NM_KO			/**  메뉴코드 한글명   **/ 
        			, MENU_NM_ZH			/**  메뉴코드 중문명   **/ 
        			, MENU_CD_KEY  
        			, LVL 
                    , ROLE_CD   
                )  AS
               (
            	  SELECT 'M' AS MENU_CD	                            /**	메뉴코드   **/ 
            			, :SessionInfo.SYS_ID AS SYS_ID	            /**  시스템아이디   **/ 
            			, 'ROOT' AS UP_MENU_CD			/**  상위메뉴코드   **/ 
            			, '' AS REM	        		/**  비고   **/ 
            			, '' AS STS	        		/**  상태   **/ 
            			, '' AS LINK_URL			/**  링크URL   **/ 
            			, 0 AS SORT_ORD			/**  정렬순서   **/ 
            			, '' AS MENU_TYP			/**  메뉴유형   **/ 
            			, 'N' AS USE_YN	    		/**  사용여부   **/ 
            			, '' AS USR_CLS	    		/**  사용자분류   **/ 
            			, '' AS MD_CLS	    		/**  모듈분류   **/ 
            			, 'N' AS DISPLAY_YN			/**  표시여부   **/ 
            			, '' AS MENU_NM_EN			/**  메뉴코드 영문명   **/ 
            			, 'DSELN LAS' AS MENU_NM_KO			/**  메뉴코드 한글명   **/ 
            			, '' AS MENU_NM_ZH			/**  메뉴코드 중문명   **/ 
            			, '0' AS MENU_CD_KEY 
            			, 0 AS LVL   
            			, '' AS ROLE_CD   
            	FROM DUAL  

                UNION ALL
               
        	    SELECT  A.MENU_CD	             /**	메뉴코드   **/ 
        			, A.SYS_ID	            /**  시스템아이디   **/ 
        			, A.UP_MENU_CD			/**  상위메뉴코드   **/ 
        			, A.REM	        		/**  비고   **/ 
        			, A.STS	        		/**  상태   **/ 
        			, A.LINK_URL			/**  링크URL   **/ 
        			, A.SORT_ORD			/**  정렬순서   **/ 
        			, A.MENU_TYP			/**  메뉴유형   **/ 
        			, A.USE_YN	    		/**  사용여부   **/ 
        			, A.USR_CLS	    		/**  사용자분류   **/ 
        			, A.MD_CLS	    		/**  모듈분류   **/ 
        			, A.DISPLAY_YN			/**  표시여부   **/ 
        			, A.MENU_NM_EN			/**  메뉴코드 영문명   **/ 
        			, A.MENU_NM_KO			/**  메뉴코드 한글명   **/ 
        			, A.MENU_NM_ZH			/**  메뉴코드 중문명   **/ 
        			, A.MENU_CD AS MENU_CD_KEY
        			, B.LVL + 1 AS LVL    
            		, A.ROLE_CD   
        		FROM ELN_IF.TB_ESA_AUMM A ,  MENU_TREE B
                /**where**/
            )
           SELECT A.MENU_CD	             /**	메뉴코드   **/ 
        			, A.SYS_ID	            /**  시스템아이디   **/ 
        			, A.UP_MENU_CD			/**  상위메뉴코드   **/ 
        			, A.REM	        		/**  비고   **/ 
        			, A.STS	        		/**  상태   **/ 
        			, A.LINK_URL			/**  링크URL   **/ 
        			, A.SORT_ORD			/**  정렬순서   **/ 
        			, A.MENU_TYP			/**  메뉴유형   **/ 
        			, A.USE_YN	    		/**  사용여부   **/ 
        			, A.USR_CLS	    		/**  사용자분류   **/ 
        			, A.MD_CLS	    		/**  모듈분류   **/ 
        			, A.DISPLAY_YN			/**  표시여부   **/ 
        			, A.MENU_NM_EN			/**  메뉴코드 영문명   **/ 
        			, A.MENU_NM_KO			/**  메뉴코드 한글명   **/ 
        			, A.MENU_NM_ZH			/**  메뉴코드 중문명   **/ 
        			, A.MENU_CD_KEY 
        			, A.LVL 
        			, A.ROLE_CD 
           FROM MENU_TREE A 
           ORDER BY A.UP_MENU_CD  
                         , A.SORT_ORD "

                // paging 
                + DynamicParameterHelper.SetPaginCondition(model)

                + @"");

            // 동적 파라미터 적용 
            builder.Where("A.UP_MENU_CD = B.MENU_CD ");

            if (!string.IsNullOrEmpty(model.USE_YN))
            {
                builder.Where("A.USE_YN = :USE_YN  ");
            }

            return DynamicParameterHelper.RefineSql(sql, model);
        }


        // 메뉴 Insert 
        public static SqlBuilder.Template MenuInsert(Menu model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                INSERT INTO ELN_IF.TB_ESA_AUMM (
		              MENU_CD     
		            , SYS_ID    
		            , UP_MENU_CD   
		            , REM     
		            , STS  
		            , LINK_URL    
		            , SORT_ORD    
		            , MENU_TYP    
		            , USE_YN    
		            , USR_CLS 
		            , MD_CLS    
		            , DISPLAY_YN  
		            , MENU_NM_EN   
		            , MENU_NM_KO  
		            , MENU_NM_ZH    
		            , CREATION_USER_ID    
		            , CREATION_DATE    
                    , ROLE_CD
		        ) 
		        VALUES ( 
		             :MENU_CD     
		            , :SessionInfo.SYS_ID  
		            , :UP_MENU_CD   
		            , :REM     
		            , :STS  
		            , :LINK_URL    
		            , :SORT_ORD    
		            , :MENU_TYP    
		            , :USE_YN    
		            , :USR_CLS 
		            , :MD_CLS    
		            , :DISPLAY_YN  
		            , :MENU_NM_EN   
		            , :MENU_NM_KO  
		            , :MENU_NM_ZH   
		            , :SessionInfo.USER_ID   
		            , SYSDATE     
                    , :ROLE_CD
		        )
             ");

            // 동적 파라미터 적용 

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 메뉴 Update 
        public static SqlBuilder.Template MenuUpdate(Menu model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
		        UPDATE ELN_IF.TB_ESA_AUMM
		        SET 
		              MENU_CD   =  :MENU_CD  
		            , SYS_ID      =  :SessionInfo.SYS_ID        
		            , UP_MENU_CD   =  :UP_MENU_CD     
		            , REM   =  :REM       
		            , STS   =  :STS    
		            , LINK_URL   =  :LINK_URL      
		            , SORT_ORD   =  :SORT_ORD      
		            , MENU_TYP   =  :MENU_TYP      
		            , USE_YN   =  :USE_YN      
		            , USR_CLS   =  :USR_CLS   
		            , MD_CLS    =  :MD_CLS     
		            , DISPLAY_YN    =  :DISPLAY_YN   
		            , MENU_NM_EN    =  :MENU_NM_EN    
		            , MENU_NM_KO    =  :MENU_NM_KO   
		            , MENU_NM_ZH    =  :MENU_NM_ZH     
		            , LAST_UPDATE_USER_ID = :SessionInfo.USER_ID
		            , LAST_UPDATE_DATE = SYSDATE    
                    , ROLE_CD = :ROLE_CD
		        WHERE MENU_CD = :MENU_CD_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Set("AND A.ID = :Id ");


            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 메뉴 Delete 
        public static SqlBuilder.Template MenuDelete(Menu model){
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            var sql = builder.AddTemplate(@"
                DELETE ELN_IF.TB_ESA_AUMM  
		        WHERE MENU_CD = :MENU_CD_KEY
             ");

            // 동적 파라미터 적용 
            //builder.Where("A.ID = :Id ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

    }
}

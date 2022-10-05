using Dapper;
using DSELN.Cmm.Helper;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Models.CodeMng;
using DSELN.Cmm.Utils;
using Microsoft.AspNetCore.Http;

namespace DSELN.DapperSql.Common
{
    public class CommonDapper
    {
        // 사용자별 메뉴가져오기 
        public static SqlBuilder.Template GetUserMenu(BaseSearchModel model)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder


            string userRoles = "";
            var roleArr = AppHttpContext.Current.Session.GetString(Const.SESSION_USER_ROLE).Split(",");
            //var roleArr = model.SessionInfo.USER_ROLE.Split(",");  // session 정보를 담기 전임.... 

            int cnt = 0;
            foreach (var role in roleArr)
            {
                if (!string.IsNullOrEmpty(role.Trim()))
                {
                    if (cnt > 0)
                    {
                        userRoles += ", '" + role.Trim() + "'";
                    }
                    else
                    {
                        userRoles += "'" + role.Trim() + "'";
                    }

                }
            }

            var sql = builder.AddTemplate(@"
                WITH MENU_TREE (
                      MENU_CD	     
        			, UP_MENU_CD		
         			, MENU_NM 
        			, LINK_URL		
        			, SORT_ORD		
        			, USE_YN	    	
        			, DISPLAY_YN	
        			, LVL 
                    , ROLE_CD 
                    , DIR_YN 
                )  AS
               (
            	  SELECT 'M' AS MENU_CD	                     
            			, 'ROOT' AS UP_MENU_CD	
        			    , 'DSELN LAS' AS MENU_NM 
            			, '' AS LINK_URL		
            			, 0 AS SORT_ORD	
            			, 'N' AS USE_YN	    	
            			, 'N' AS DISPLAY_YN	
            			, 0 AS LVL   
                        , '' AS ROLE_CD 
                        , 'Y' AS DIR_YN 
            	    FROM DUAL  

                    UNION ALL
               
        	        SELECT A.MENU_CD	         
        			    , A.UP_MENU_CD		 
        			    , CASE WHEN :SessionInfo_LANG_CD = 'EN' THEN  NVL(A.MENU_NM_EN, A.MENU_NM_KO) 
                                   WHEN :SessionInfo_LANG_CD = 'ZE' THEN  NVL(A.MENU_NM_ZH, A.MENU_NM_KO)
                                   ELSE A.MENU_NM_KO
                          END AS MENU_NM
        			    , A.LINK_URL			 
        			    , A.SORT_ORD	 
        			    , A.USE_YN	     
        			    , A.DISPLAY_YN		 
        			    , B.LVL + 1 AS LVL  
                        , A.ROLE_CD  
                        , CASE WHEN A.LINK_URL IS NULL THEN 'Y' ELSE 'N' END AS DIR_YN
        		    FROM ELN_IF.TB_ESA_AUMM A ,  MENU_TREE B
                    /**where**/
                )

                , MENU_ROLE AS (
                              SELECT T.MENU_CD
                                        , REGEXP_SUBSTR(T.ROLE_CD_EACH, '[^,]+', 1, Level) AS ROLE_CD_EACH
                                        , ROWNUM AS RN
                              FROM (
                                  SELECT A.MENU_CD 
                                            , A.ROLE_CD AS ROLE_CD_EACH
                                  FROM MENU_TREE  A
                                  WHERE A.LINK_URL IS NOT NULL 
                              )T
                              WHERE T.ROLE_CD_EACH IS NOT NULL 
                              CONNECT BY LEVEL <= REGEXP_COUNT(T.ROLE_CD_EACH, '[^,]+')
                                  AND PRIOR T.MENU_CD  = T.MENU_CD
                                  AND PRIOR DBMS_RANDOM.VALUE IS NOT NULL
                 )
               , USER_MENU AS (
                       SELECT A.MENU_CD        
                              , A.UP_MENU_CD  
                              , A.MENU_NM 
                              , A.LINK_URL     
                              , A.SORT_ORD     
                              , A.USE_YN     
                              , A.DISPLAY_YN     
                              , A.LVL 
                              , A.ROLE_CD 
                              , A.DIR_YN 
                       FROM MENU_TREE A 
                       WHERE A.DIR_YN = 'Y' 
                          AND EXISTS (SELECT 1 FROM MENU_TREE X WHERE A.MENU_CD = X.UP_MENU_CD ) /*** dir 하위 메뉴가 존재하는것만 ***/
              
                       UNION ALL
           
                       SELECT A.MENU_CD        
                              , A.UP_MENU_CD  
                              , A.MENU_NM 
                              , A.LINK_URL     
                              , A.SORT_ORD     
                              , A.USE_YN     
                              , A.DISPLAY_YN     
                              , A.LVL 
                              , A.ROLE_CD 
                              , A.DIR_YN 
                       FROM MENU_TREE A 
                       WHERE A.DIR_YN = 'N' 
                          AND A.ROLE_CD IS NULL  /*** page && role_cd is null 인것.  ***/ 

                       UNION ALL

                       SELECT DISTINCT 
                                A.MENU_CD        
                              , A.UP_MENU_CD  
                              , A.MENU_NM 
                              , A.LINK_URL     
                              , A.SORT_ORD     
                              , A.USE_YN     
                              , A.DISPLAY_YN     
                              , A.LVL 
                              , A.ROLE_CD  
                              , A.DIR_YN 
                       FROM MENU_TREE A 
                              , MENU_ROLE B 
                       WHERE A.MENU_CD = B.MENU_CD  /*** inner join ***/
                           AND (A.DIR_YN = 'N'  AND A.ROLE_CD IS NOT NULL)   /*** page & role_cd not null  ***/ 
                           AND B.ROLE_CD_EACH  IN (" + userRoles + @") 
                  )
              
                  SELECT A.* 
                  FROM USER_MENU A 
                  WHERE (A.DIR_YN = 'N' 
                                 OR EXISTS (SELECT 1 FROM USER_MENU X WHERE A.MENU_CD = X.UP_MENU_CD )  /*** 디렉토리중 하위 아무것도 없으면 제외 ***/
                              )
                   ORDER BY A.UP_MENU_CD  
                                , A.SORT_ORD  "
                    + @"");

            // 동적 파라미터 적용 
            builder.Where("A.UP_MENU_CD = B.MENU_CD ");
            builder.Where("A.USE_YN = 'Y'  ");
            builder.Where("A.DISPLAY_YN = 'Y'  ");

            //if (!model.SessionInfo.USER_ID.Equals("scitegicadmin"))  // role 관리 전까지... 임시로 
            //{
            //    builder.Where("NVL(A.LINK_URL, '0') NOT IN ( '/Equip/QRMng' ) ");
            //}

            return DynamicParameterHelper.RefineSql(sql, model);
        }


        // 데이터 존재여부   
        public static SqlBuilder.Template DataExistCheck(object model, string tblNm, string keyType, string keyId, string compareId, string keyId2, string compareId2)
        {
            // keyType == _KEY ... 
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            BaseModel baseModel = model as BaseModel;

            var sql = builder.AddTemplate(@"
                SELECT COUNT(1) AS CNT 
                FROM " + tblNm + @" A 
                WHERE 1=1  
                    AND A." + compareId + " = :" + compareId + keyType + " /* 키 컬럼 */  " +
                    (!compareId2.Equals("") ? "AND A." + compareId2 + " != :" + compareId2 + keyType + " /* 키 컬럼2 */  " : "") + @"

             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 중복체크  
        public static SqlBuilder.Template DupicateCheck(object model, string tblNm, string keyType, string keyId, string compareId, string keyId2, string compareId2)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            BaseModel baseModel = model as BaseModel;

            // keyType : 코드등록 등 화면에서 키 컬럼값을 입력할 경우 사용, sequence 채번할 경우는 공백으로 넘기면됨. 
            // keyId2 : 데이터의 Key 가 두개 컬럼으로 구성될때 사용. 1개 컬럼이면 공백으로 넘김. 

            var sql = builder.AddTemplate(@"
                SELECT COUNT(1) AS CNT 
                FROM " + tblNm + @" A 
                WHERE 1=1  
                    AND A." + compareId + " = :" + compareId + " /* 중복체크할 컬럼 */  " +

                    (!compareId2.Equals("") ? "AND A." + compareId2 + " = :" + compareId2 + " /* 중복체크할 컬럼2 */  " : "") +

                    (baseModel._ROW_TYPE.Equals("U") ? "AND A." + keyId + " != :" + keyId + keyType + " /* 업데이트시 변경전 keyId로 자기 자신을 제외하고 비교  */ " : "") +

                    (baseModel._ROW_TYPE.Equals("U") && !keyId2.Equals("") ? "AND A." + keyId2 + " != :" + keyId2 + keyType + " /* 업데이트시 변경전 keyId2로 자기 자신을 제외하고 비교   */ " : "") + @"

             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

        // 키값 채번   
        public static SqlBuilder.Template GetSeqence(object model, string seqId)
        {
            var builder = new SqlBuilder();   // Dapper.SqlBuilder

            BaseModel baseModel = model as BaseModel;

            var sql = builder.AddTemplate(@"
                SELECT " + seqId + @".NEXTVAL AS NEXT_SEQ FROM DUAL
             ");

            return DynamicParameterHelper.RefineSql(sql, model);
        }

    }
}

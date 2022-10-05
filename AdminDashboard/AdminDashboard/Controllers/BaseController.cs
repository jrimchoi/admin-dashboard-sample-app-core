using DSELN.Cmm.Filters;
using Microsoft.AspNetCore.Mvc;
using DSELN.Cmm.Utils;
using DSELN.Models;
using DSELN.Models.Common;
using DSELN.Service.Common;
using Newtonsoft.Json;
using DSELN.Models.Login;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using DSELN.Cmm.Helper;
using DSELN.Repository.Common;
using DSELN.Service.Login;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace DSELN.Controllers
{
    public class BaseController : Controller
    {
        // 화면별 버튼 권한 담기 
        protected void setPermit2ViewData(Dictionary<string, string> permit)
        {
            if (permit != null)
            {
                foreach (KeyValuePair<string, string> items in permit)
                {
                    //var key = items.Key;
                    ViewData[items.Key] = items.Value;
                }
            }
        }

        // get common code list ver.1
        public void getComCodeList(string[] constArr, ICommonService _cmmService)
        {
            //ViewData["USE_DEPT"] = "BIO";  // ViewData 공통으로 필요시 여기에 추가할것. 

            if (!"Development".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                this.setSession(_cmmService);
            }

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["EXP_NO"])) // 연구노트 번호 가져오기
            {
                string expNo = HttpContext.Request.Query["EXP_NO"];
                ViewBag.ExpNo = expNo;
            }

            var constProp = new List<CodeCondition>();  // new version 
            foreach (var str in constArr)
            {
                constProp.Add(new CodeCondition() { ID = str });
            }
            Dictionary<string, List<CodeModel>> codeDic = _cmmService.GetCodeList(constProp);

            //Dictionary<string, List<CodeModel>> codeDic = _cmmService.GetCodeList(constArr);

            ViewBag.CodeList = JsonConvert.SerializeObject(codeDic);
        }

        // get common code list ver.2
        public void getComCodeList(List<CodeCondition> constProp, ICommonService _cmmService)
        {
            //ViewData["USE_DEPT"] = "BIO";  // ViewData 공통으로 필요시 여기에 추가할것. 
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["EXP_NO"])) // 연구노트 번호 가져오기
            {
                string expNo = HttpContext.Request.Query["EXP_NO"];
                ViewBag.ExpNo = expNo;
            }

            Dictionary<string, List<CodeModel>> codeDic = _cmmService.GetCodeList(constProp);
            ViewBag.CodeList = JsonConvert.SerializeObject(codeDic);
        }

        // 조회 결과 리턴 
        public object returnSearchOK(object result0, object condition)
        {
            ViewBag.Condition = condition;  // 조회조건 유지 
            return new { _status = Const.SUCC, _msg = "Success", result0 = result0 };  // succ = 1, fail = 0 or != 1  
        }

        public object returnSearchOK(object result0, object result1, object condition)
        {
            ViewBag.Condition = condition;  // 조회조건 유지 
            return new { _status = Const.SUCC, _msg = "Success", result0 = result0, result1 = result1 };
        }

        public object returnSearchOK(object result0, object result1, object result2, object condition)
        {
            ViewBag.Condition = condition;  // 조회조건 유지 
            return new { _status = Const.SUCC, _msg = "Success", result0 = result0, result1 = result1, result2 = result2 };
        }

        public object returnSearchOK(object result0, object result1, object result2, object result3, object condition)
        {
            ViewBag.Condition = condition;  // 조회조건 유지 
            return new { _status = Const.SUCC, _msg = "Success", result0 = result0, result1 = result1, result2 = result2, result3 = result3 };
        }

        // 저장 및 처리 결과 
        public object returnOK(object model0)
        {
            return new { _status = Const.SUCC, _msg = "Success", result0 = model0 };  // succ = 1, fail = 0 or != 1 
        }

        public object returnOK(object model0, object model1)
        {
            //string proctype = ((BaseModel) model0)._PROC_TYPE as string ?? string.Empty;
            return new { _status = Const.SUCC, _msg = "Success", result0 = model0, result1 = model1 };
        }

        public object returnOK(object model0, object model1, object model2)
        {
            //string proctype = ((BaseModel) model0)._PROC_TYPE as string ?? string.Empty;
            return new { _status = Const.SUCC, _msg = "Success", result0 = model0, result1 = model1, model2 = model2 };
        }

        // 저장 및 처리 실패 
        public object returnFail(object model0)
        {
            return new { _status = Const.FAIL, _msg = "Fail", model0 = model0 };  // succ = 1, fail = 0 or != 1 
        }

        public void setSession(ICommonService _cmmService)
        {
            SessionModel sessionModel = new SessionModel();
            LoginModel model = new LoginModel();

            string? userId = HttpContext.Session.GetString(Const.SESSION_USER_ID);
            string? userNm = HttpContext.Session.GetString(Const.SESSION_USER_NM);
            if (!string.IsNullOrEmpty(userId))
            {
                model.USER_ID = userId?.ToLower();
                model.PWD = userId;

                sessionModel = _cmmService.GetLoginInfo(model);

                if (sessionModel == null || string.IsNullOrEmpty(sessionModel.USER_ID))
                {
                    Response.Redirect(GetRedirectLoginUrl());
                }
                else
                {
                    // 10. 세션정보 담기 
                    this.setSessionInfo(sessionModel);

                    // 20. 세션정보로 사용자별 메뉴 담기 
                    List<Dictionary<string, string>> list = _cmmService.GetUserMenu(new BaseSearchModel() { });
                    HttpContext.Session.SetString(Const.SESSION_USER_MENU, JsonConvert.SerializeObject(list));
                }
            }
        }

        protected void setSessionInfo(SessionModel sessionModel)
        {
            // 10. 세션정보 저장 
            sessionModel.CLIENT_IP_ADDR = HttpContext.Connection.RemoteIpAddress.ToString();
            HttpContext.Session.SetString(Const.SESSION_USER_ID, sessionModel.USER_ID);
            HttpContext.Session.SetString(Const.SESSION_USER_NM, sessionModel.USER_NM);
            HttpContext.Session.SetString(Const.SESSION_SYS_ID, sessionModel.SYS_ID);
            HttpContext.Session.SetString(Const.SESSION_CLIENT_IP_ADDR, sessionModel.CLIENT_IP_ADDR);
            HttpContext.Session.SetString(Const.SESSION_USER_ROLE, sessionModel.USER_ROLE);  // 현재 ADMIN / USER 
            HttpContext.Session.SetString(Const.SESSION_SESSION_MODEL, JsonConvert.SerializeObject(sessionModel));

            HttpContext.Session.SetString(Const.SESSION_USER_DEPT_CD, sessionModel.USER_DEPT_CD ?? "");     // 사용자 테이블의 deptcode1 
            HttpContext.Session.SetString(Const.SESSION_USER_POSITION_CD, sessionModel.USER_POSITION_CD ?? "");
            HttpContext.Session.SetString(Const.SESSION_USER_BU_CD, sessionModel.USER_BU_CD ?? "");   // 사용자 테이블의 bu_code 
            HttpContext.Session.SetString(Const.SESSION_USE_DEPT, sessionModel.USE_DEPT ?? "");
            HttpContext.Session.SetString(Const.SESSION_BU_NAME, sessionModel.BU_NAME ?? ""); // 사용자 테이블의 bu_name

            // 세션에 저장된 로그인사용자 정보 가져오기 (샘플코드) 
            SessionHelper.GetSessionModel(HttpContext.Session).PrintModelValues("login session info");
        }

        private string GetRedirectLoginUrl(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = this.CreateDefaultReturnUrl();

            string loginURL = ConfigUtil.getSectionValue("Las:Eln_Base_Url") + "/?returnUrl=" + returnUrl;
            Log.Debug($"BaseController GetRedirectLoginUrl loginURL  : {loginURL}");

            return loginURL;
        }

        private string CreateDefaultReturnUrl()
        {
            string retval = this.HttpContext.Request.GetDisplayUrl();
            Log.Debug($"BaseController CreateDefaultReturnUrl  : {retval}");

            return retval;
        }
    }
}

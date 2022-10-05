using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using ElnApplication.Controllers.Apis;
using Microsoft.AspNetCore.Mvc;
using DSELN.Cmm.Exceptions;
using DSELN.Cmm.Utils;
using DSELN.Cmm.Filters.FilterTask;
using DSELN.Cmm.Helper;
using DSELN.Models.Login;
using DSELN.Service.Common;
using DSELN.Service.Login;
using Newtonsoft.Json;
using Quickwire.Attributes;
using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Serilog;

namespace DSELN.Cmm.Filters
{
    public class SessionCheckActionFilterAttribute : ActionFilterAttribute
    {

        public LoginService _loginService { get; private set; }

        public enum AuthorizationState
        {
            /// <summary>
            /// Indicates "undefined"
            /// </summary>
            None = 0,

            /// <summary>
            /// Authenicated and authorized to use the application
            /// </summary>
            OK = 1,

            /// <summary>
            /// Authenicated but not authorized to use the application
            /// </summary>
            Unauthorized = 2,

            /// <summary>
            /// Not authenicated
            /// </summary>
            Invalid = 3,

            /// <summary>
            /// No credentials provided
            /// </summary>
            Anonymous = 4,

            /// <summary>
            /// An exception occurred while determining the authorization state
            /// </summary>
            Exception = 5,

            /// <summary>
            /// The session is about to be terminated
            /// </summary>
            Signout = 6
        }

        private const string ELNAPI_ISAUTHORIZED = "/api/v1/me/isauthorized";
        private const string AUTHORIZATIONCOOKIENAME = "AuthorizationEln";
        protected const string SCISIDCOOKIENAME = "SCISID9944";
        private static string path = "";
        private FilterContext _context;
        public SessionCheckActionFilterAttribute(LoginService loginService)
        {
            this._loginService = loginService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            this._context = context;
            //if (!string.IsNullOrEmpty(_context.HttpContext.Session.GetString(Const.SESSION_USER_ID)))
            //    return;
            if ("Development".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                string? userId = context.HttpContext.Session.GetString(Const.SESSION_USER_ID);  // Const.SESSION_USER_ID, "acc1");

                if (userId == null)
                {
                    // 세션체크 예외 인지여부 
                    var attributes = context.ActionDescriptor.EndpointMetadata.OfType<SkipSessionCheckTaskAttribute>();
                    var IsSkip = false;

                    foreach (var item in attributes)
                    {
                        Console.WriteLine("item.TypeId : " + item.TypeId);
                        if (item.TypeId.ToString().IndexOf("SkipSessionCheckTaskAttribute") >= 0) IsSkip = true;
                    }

                    if (IsSkip)
                    {
                        Console.WriteLine("SessionCheck Skipped..... : " + attributes.ToString());
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Session is Expired...... : " + attributes.ToString());
                        //if (!context.ModelState.IsValid)
                        {
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
                            base.OnActionExecuting(context);

                            //throw new ArgumentException("SessionExpired");  // 사용자 정의 exception 으로... 변경할것.. 
                            throw new SessionExpiredException("SessionExpired");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Session is Valid........  : " + userId);
                    return;
                }
            }
            else
            {
                // Authorize the request
                path = ConfigUtil.getConfigValue("Las", "Eln_Base_Url");
                AuthorizationState state = this.CheckAuthorization(path + ELNAPI_ISAUTHORIZED);

                if (state.Equals(AuthorizationState.OK))
                {
                    string eln = GetAuthorizationToken();
                    string user;
                    if (eln.Contains("ADMIN_PLATFORM"))
                    {
                        user = eln?.Split(".")[1].Split("_0")[0].ToLower();
                    }
                    else
                    {
                        user = eln?.Split(".")[1].Split("_")[0];
                    }
                    string? userId = context.HttpContext.Session.GetString(Const.SESSION_USER_ID);  // Const.SESSION_USER_ID, "acc1");
                    if (userId == null)
                    {
                        LoginModel model = new LoginModel();
                        model.USER_ID = user;
                        SessionModel sessionModel = _loginService.GetLoginInfo(model);
                        setSessionInfo(sessionModel);

                        // 20. 세션정보로 사용자별 메뉴 담기 
                        List<Dictionary<string, string>> list = _loginService.GetUserMenu();
                        this._context.HttpContext.Session.SetString(Const.SESSION_USER_MENU, JsonConvert.SerializeObject(list));
                    }
                    return;
                }
                else // SSO 로그인 처리
                {
                    // 세션체크 예외 인지여부 
                    var attributes = context.ActionDescriptor.EndpointMetadata.OfType<SkipSessionCheckTaskAttribute>();
                    var IsSkip = false;

                    foreach (var item in attributes)
                    {
                        Console.WriteLine("item.TypeId : " + item.TypeId);
                        if (item.TypeId.ToString().IndexOf("SkipSessionCheckTaskAttribute") >= 0) IsSkip = true;
                    }

                    if (IsSkip)
                    {
                        Console.WriteLine("SessionCheck Skipped..... : " + attributes.ToString());
                        return;
                    }
                    else
                    {
                        // We are not authorized, redirect to login page
                        string loginUrl = GetRedirectLoginUrl(state);
                        Log.Debug($"SessionCheckActionFilterAttribute GetRedirectLoginUrl  : {loginUrl}");
                        context.Result = new RedirectResult(loginUrl);
                    }

                }
            }
        }

        private AuthorizationState CheckAuthorization(string pathAndQuery, string overrideToken = null)
        {
            try
            {
                string token = overrideToken ?? this.GetAuthorizationToken();

                HttpWebResponse apiResponse = RequestUtil.ExecuteWebRequest(pathAndQuery, token, "GET", "application/json");
                int retval = int.Parse(apiResponse.GetMessage());

                return (AuthorizationState)retval;
            }
            catch (Exception ex)
            {
                return AuthorizationState.Exception;
            }
        }

        private string GetAuthorizationToken()
        {
            return _context.HttpContext.Request.Cookies[AUTHORIZATIONCOOKIENAME];
        }

        private string GetSciSidToken()
        {
            return _context.HttpContext.Request.Cookies[SCISIDCOOKIENAME];
        }

        public string GetRedirectLoginUrl(AuthorizationState state, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = this.CreateDefaultReturnUrl();

            string loginURL = ConfigUtil.getSectionValue("Las:Eln_Base_Url") + "/?returnUrl=" + returnUrl;
            Log.Debug($"SessionCheckActionFilterAttribute GetRedirectLoginUrl loginURL : {loginURL}");

            return loginURL;
        }

        public string CreateDefaultReturnUrl()
        {
            string retval = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";
            Log.Debug($"SessionCheckActionFilterAttribute GetDisplayUrl  : {retval}");

            return retval;
        }
        protected void setSessionInfo(SessionModel sessionModel)
        {
            // 10. 세션정보 저장 
            sessionModel.CLIENT_IP_ADDR = this._context.HttpContext.Connection.RemoteIpAddress.ToString();
            this._context.HttpContext.Session.SetString(Const.SESSION_USER_ID, sessionModel.USER_ID);
            this._context.HttpContext.Session.SetString(Const.SESSION_USER_NM, sessionModel.USER_NM);
            this._context.HttpContext.Session.SetString(Const.SESSION_SYS_ID, sessionModel.SYS_ID);
            this._context.HttpContext.Session.SetString(Const.SESSION_CLIENT_IP_ADDR, sessionModel.CLIENT_IP_ADDR);
            this._context.HttpContext.Session.SetString(Const.SESSION_USER_ROLE, sessionModel.USER_ROLE);  // 현재 ADMIN / USER 
            this._context.HttpContext.Session.SetString(Const.SESSION_SESSION_MODEL, JsonConvert.SerializeObject(sessionModel));

            // 세션에 저장된 로그인사용자 정보 가져오기 (샘플코드) 
            SessionHelper.GetSessionModel(this._context.HttpContext.Session).PrintModelValues("login session info");
        }
    }

}

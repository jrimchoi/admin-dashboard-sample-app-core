using DSELN.Cmm.Exceptions;
using DSELN.Cmm.Helper;
using DSELN.Cmm.Utils;
using DSELN.Models.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.ComponentModel;
using System.Linq;

namespace DSELN.Cmm.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
        public AuthorizeFilterAttribute(params string[] roles)
        {
            _roles = roles ?? new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            bool isAuth = false, isSession = false;
            string? userId = AppHttpContext.Current.Session.GetString(Const.SESSION_USER_ID);

            if (AppHttpContext.Current.Session == null || string.IsNullOrEmpty(userId))
            {
                isAuth = false;
            }
            else
            {
                //var roles = model.USER_ROLE ?? String.Empty;  // "A", "B"
                var roleArr = AppHttpContext.Current.Session.GetString(Const.SESSION_USER_ROLE).Split(",", StringSplitOptions.TrimEntries);
                isSession = true;

                for (int i = 0; i < _roles.Length; i++)
                {
                    var authRole = _roles[i].ToString();
                    //RoleDescript myLocal = 0;  // myLocal.ToDescriptionString()

                    //Console.WriteLine("user role : " + authRole + " / " + roleArr.Contains(authRole));

                    if (roleArr.Contains(authRole))
                    {
                        isAuth = true;
                    }
                    else
                    {
                        isAuth = false;
                    }
                }
            }

            if (_roles.Any() && !isAuth)
            {
                // not logged in or role not authorized
                //context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

                // html or json 
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                string redirectUrl = "Views/Shared/Error.cshtml";
                if (!isSession) redirectUrl = "Views/Shared/SessionExpired.cshtml";

                var result = new ViewResult { ViewName = redirectUrl };
                var modelMetadata = new EmptyModelMetadataProvider();

                result.ViewData = new ViewDataDictionary(modelMetadata, context.ModelState);

                result.ViewData.Add("_status", "0");  // 실패   

                result.ViewData.Add("_msg", "사용 권한이 없습니다." + (isSession ? "" : " [세션만료]"));

                result.ViewData.Add("_isLogin", false);

                context.Result = result;
            }

        }

    }

}

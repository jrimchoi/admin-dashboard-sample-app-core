using Microsoft.AspNetCore.Mvc;
using DSELN.Cmm.Filters;
using DSELN.Cmm.Filters.FilterTask;
using DSELN.Cmm.Helper;
using DSELN.Cmm.Utils;
using DSELN.Models.Login;
using DSELN.Service.Login;
using Newtonsoft.Json;
using Quickwire.Attributes;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DSELN.Controllers.Login
{
    //public interface ILoginService{}

    [RegisterService(ServiceLifetime.Scoped)]
    public class LoginController : BaseController
    {
        // QuickWire DI like as Spriing 
        [InjectService]
        public LoginService? _loginService { get; private set; }

        [SkipSessionCheckTask]    // session check skip 
        public IActionResult Login()
        {
            //return RedirectToAction("RedirectHub", "Login");
            //return RedirectToAction("EquipList", "Equip");
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.SESSION_USER_ID)))
            {
                if ("Development".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("RedirectHub", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("Login/Login")]
        [SkipSessionCheckTask]     // session check skip 
        //[ValidateAntiForgeryToken] // xss 
        [ValidateModel]                  // validation interceptor 
        public IActionResult Login(LoginModel model)
        {
            SessionModel sessionModel = _loginService.GetLoginInfo(model);

            if (sessionModel == null || string.IsNullOrEmpty(sessionModel.USER_ID))
            {
                ViewBag.error = "Invalid Account";
                return View("../Login/Login");  // ViewBag.error 을 전달하려면 View() 사용 
                //return RedirectToAction("Login", "Login");
            }
            else
            {
                // 10. 세션정보 담기 
                base.setSessionInfo(sessionModel);

                // 20. 세션정보로 사용자별 메뉴 담기 
                List<Dictionary<string, string>> list = _loginService.GetUserMenu();
                HttpContext.Session.SetString(Const.SESSION_USER_MENU, JsonConvert.SerializeObject(list));

                return RedirectToAction("Dashboard4Admin", "Dashboard");
            }
        }

        [SkipSessionCheckTask]
        [Route("Login/Logout")]
        [HttpGet]
        public void Logout()
        {
            HttpContext.Session.Remove(Const.SESSION_USER_ID);
            string returnUrl = $"{this.Request.Scheme}://{this.Request.Host}";
            if ("Development".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
                Response.Redirect(returnUrl);
            else
            {
                //Response.Cookies.Delete("AuthorizationEln");
                SetCookie("AuthorizationEln", "", -1);
                SetCookie("ElnSessionKeyNew", "", -1);
                //https://elndev.daesang.com/logout?returnurl=https://elndev.daesang.com/notebook.aspx
                string url = ConfigUtil.getSectionValue("Las:Hub_Base_Url") + "/foundation/hub/security/logout?target_uri=" + ConfigUtil.getSectionValue("Las:Base_Url");
                Console.WriteLine(url);
                //Response.Redirect(ConfigUtil.getSectionValue("Las:Hub_Base_Url") + "/foundation/hub/security/logout?target_uri=" + ConfigUtil.getSectionValue("Las:Eln_Base_Url") + "?returnUrl=" + $"{this.Request.Scheme}://{this.Request.Host}");

                Response.Redirect(url);
            }

            //return RedirectToAction("Login", "Login");
        }
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        [SkipSessionCheckTask]
        [Route("Login/RedirectHub")]
        [HttpGet]
        public void RedirectHub()
        {
            string url = ConfigUtil.getSectionValue("Las:Hub_Base_Url") + "/foundation/hub/security/auth?target_uri=" + ConfigUtil.getSectionValue("Las:Eln_Base_Url") + "?returnUrl=" + ConfigUtil.getSectionValue("Las:Base_Url");
            Console.WriteLine(url);
            Response.Redirect(url);
        }

    }
}

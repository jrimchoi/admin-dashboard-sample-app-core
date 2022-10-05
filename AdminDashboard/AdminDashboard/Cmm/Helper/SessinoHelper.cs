using DSELN.Cmm.Utils;
using DSELN.Models.Login;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using Quickwire.Attributes;
using System.Collections.Generic;

namespace DSELN.Cmm.Helper
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static SessionModel GetSessionModel(this ISession session)
        {
            SessionModel sessionModel = SessionHelper.GetObjectFromJson<SessionModel>(session, Const.SESSION_SESSION_MODEL);
            return sessionModel;
        }

        public static List<Dictionary<string, string>> GetSessionUserMenu(this ISession session)
        {
            List<Dictionary<string, string>> UserMenu = SessionHelper.GetObjectFromJson<List<Dictionary<string, string>>>(session, Const.SESSION_USER_MENU);
            return UserMenu;
        }

    }

    public static class AppHttpContext
    {
        static IServiceProvider services = null;


        /// <summary>
        /// Provides static access to the framework's services provider
        /// </summary>
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        /// <summary>
        /// Provides static access to the current HttpContext
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

    }

}

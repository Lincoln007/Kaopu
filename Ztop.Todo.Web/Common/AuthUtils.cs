using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web
{
    public static class AuthUtils
    {
        private const string _cookieName = ".user";

        public static void SaveAuth(this HttpContextBase context, User user)
        {
            var ticket = new FormsAuthenticationTicket(user.ID.ToString(), true, 60);
            var cookieValue = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(_cookieName, cookieValue);
            context.Response.Cookies.Remove(_cookieName);
            context.Response.Cookies.Add(cookie);
        }

        public static int GetUserID(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(_cookieName);
            if (cookie != null)
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                    {
                        var userId = 0;
                        int.TryParse(ticket.Name, out userId);
                        return userId;
                    }
                }
                catch
                {
                }
            }
            return 0;
        }

        public static void ClearAuth(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(_cookieName);
            if (cookie == null) return;
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddHours(2);
            context.Response.SetCookie(cookie);
        }
    }
}
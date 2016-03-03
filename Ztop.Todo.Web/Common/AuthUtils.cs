using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web
{
    public static class AuthUtils
    {
        private const string _cookieName = ".user";

        public static void SaveAuth(this HttpContextBase context, User user)
        {
            var ticket = new FormsAuthenticationTicket(user.ID.ToString()+"|"+user.Username+"|"+user.Type.ToString(), true, 60);
            var cookieValue = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(_cookieName, cookieValue);
            context.Response.Cookies.Remove(_cookieName);
            context.Response.Cookies.Add(cookie);
        }
        public static UserIdentity GetCurrentUser(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(_cookieName);
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(cookie.Value))
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                    {
                        var values = ticket.Name.Split('|');
                        if (values.Length == 3)
                        {
                            var type = GroupType.Guest;
                            return new UserIdentity
                            {
                                UserName = values[1],
                                GroupType = Enum.TryParse(values[2], out type) ? type : GroupType.Guest
                            };
                        }
                    }
                }
            }
            return UserIdentity.Guest;
        }

        public static int GetUserID(this HttpContextBase context)
        {
            var token = context.Request.Headers["token"];
            if (string.IsNullOrEmpty(token))
            {
                var cookie = context.Request.Cookies.Get(_cookieName);
                if (cookie != null)
                {
                    token = cookie.Value;
                }
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(token);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                    {
                        var values = ticket.Name.Split('|');
                        if (values.Length == 2)
                        {
                            var userId = 0;
                            int.TryParse(values[0], out userId);
                            return userId;
                        }
                        
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
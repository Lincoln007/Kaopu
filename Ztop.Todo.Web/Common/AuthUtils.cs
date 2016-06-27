using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web
{
    public static class AuthUtils
    {
        private const string _cookieName = ".user";
        private static List<string> _directors = XmlHelper.GetDirectors();

        public static string GenerateToken(this HttpContextBase context, User user)
        {
            var ticket = new FormsAuthenticationTicket(1, user.ID + "|" + user.DisplayName + "|" + user.Type+"|"+user.Username, DateTime.Now, DateTime.MaxValue, true, "user_token");
            var token = FormsAuthentication.Encrypt(ticket);
            return token;
        }

        public static void SaveAuth(this HttpContextBase context, User user)
        {
            user.AccessToken = GenerateToken(context, user);
            var cookie = new HttpCookie(_cookieName, user.AccessToken);
            context.Response.Cookies.Remove(_cookieName);
            context.Response.Cookies.Add(cookie);
        }

        private static string GetToken(HttpContextBase context)
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
            return token;
        }

        public static UserIdentity GetCurrentUser(this HttpContextBase context)
        {
            var token = GetToken(context);
            if (!string.IsNullOrEmpty(token))
            {
                var ticket = FormsAuthentication.Decrypt(token);
                if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                {
                    var values = ticket.Name.Split('|');
                    if (values.Length == 4)
                    {
                        var type = GroupType.Guest;
                        return new UserIdentity
                        {
                            UserID = int.Parse(values[0]),
                            Name = values[1],
                            GroupType = Enum.TryParse(values[2], out type) ? type : GroupType.Guest,
                            sAMAccountName=values[3],
                            Director=_directors.Contains(values[1])?true:false
                        };
                    }
                }
            }
            return UserIdentity.Guest;
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
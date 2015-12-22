using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ztop.Todo.Web
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class UserAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.User.Identity.Name);
        }
    }
}
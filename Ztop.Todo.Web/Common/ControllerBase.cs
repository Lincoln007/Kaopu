using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Manager;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web.Controllers
{
    
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = ManagerCore.Instance;

        protected User CurrentUser { get; private set; }
        protected UserIdentity Identity
        {
            get
            {
                return (UserIdentity)HttpContext.User.Identity;
            }
        }

        protected ActionResult SuccessJsonResult(object data = null)
        {
            return new ContentResult { Content = new { result = 1, content = "操作成功", data }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        protected ActionResult ErrorJsonResult(string message)
        {
            return new ContentResult { Content = new { result = 0, content = message }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        protected ActionResult ErrorJsonResult(Exception ex)
        {
            return new ContentResult { Content = new { result = 0, content = ex.Message, data = ex }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        private User GetCurrentUser()
        {
            var userId = HttpContext.GetUserID();
            if (userId > 0)
            {
                CurrentUser = Core.UserManager.GetUser(userId);
            }
            else
            {
                if (string.IsNullOrEmpty(Identity.UserName))
                {
                    return null;
                }
                var user = Core.UserManager.GetUser(Identity.UserName);
                if (user == null)
                {
                    CurrentUser = new User { Username = Identity.UserName };
                    Core.UserManager.Save(CurrentUser);
                }
                else
                {
                    CurrentUser = user;
                }
                HttpContext.SaveAuth(CurrentUser);
                Core.UserManager.UpdateLogin(CurrentUser);
            }
            return CurrentUser;
        }

        protected bool ADLogin(string Name,string Password)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password))
            {
                if (Ztop.Todo.Common.ADController.Login(Name, Password))
                {
                    var user = Core.UserManager.GetUser(Name);
                    if (user == null)
                    {
                        user = new User { Username = Name };
                        Core.UserManager.Save(user);
                    }
                    CurrentUser = user;
                    ViewBag.CurrentUser = CurrentUser;
                    HttpContext.SaveAuth(user);
                    return true;
                }
            }
            return false;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Controller = filterContext.RequestContext.RouteData.Values["controller"];
            ViewBag.Action = filterContext.RequestContext.RouteData.Values["action"];
            ViewBag.CurrentUser = GetCurrentUser();
            base.OnActionExecuting(filterContext);
        }

        private int GetStatusCode(Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            if (ex is HttpException)
            {
                statusCode = (ex as HttpException).GetHttpCode();
            }
            else if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
            }
            return statusCode;
        }

        private Exception GetException(Exception ex)
        {
            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                return GetException(innerEx);
            }
            return ex;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            if (filterContext.HttpContext.Response.StatusCode == 200)
            {
                filterContext.HttpContext.Response.StatusCode = GetStatusCode(filterContext.Exception);
            }
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            var ex = GetException(filterContext.Exception);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = ErrorJsonResult(ex);
            }
            else
            {
                ViewBag.Exception = ex;
                filterContext.Result = View("Error");
            }
        }
    }
}
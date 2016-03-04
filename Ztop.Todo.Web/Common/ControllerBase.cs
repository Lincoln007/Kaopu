using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Manager;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;
using System.IO;
using System.Collections.Generic;

namespace Ztop.Todo.Web.Controllers
{

    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = ManagerCore.Instance;

        protected User CurrentUser { get; private set; }
        //private AUser _auser { get; set; }
        //protected AUser AUser
        //{
        //    get { return _auser == null ? _auser = Core.UserManager.GetZTOPAccount(Identity.Name) : _auser; }
        //}
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
            ViewBag.GroupType = CurrentUser == null ? GroupType.Guest : CurrentUser.Type;
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
            using (var fs=new FileStream("error.txt", FileMode.Append, FileAccess.Write))
            {
                using (var sw=new StreamWriter(fs))
                {
                    sw.WriteLine(string.Format("时间：{0} 用户名：{1} 错误信息：{2}",DateTime.Now,CurrentUser!=null?CurrentUser.DisplayName:"未登陆",ex.ToString()));
                }
            }
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

        protected ActionResult HtmlResult(List<string> html)
        {
            var values = html.ListToTable();
            var str = string.Empty;
            foreach (var item in values)
            {
                var st = string.Empty;
                st += "<tr>";
                foreach (var entry in item)
                {
                    if (string.IsNullOrEmpty(entry))
                    {
                        continue;
                    }
                    st += "<td><label class='checkbox-inline'><input type='checkbox' name='Group' value='" + entry + "' />" + entry + "</label></td>";
                }
                st += "</tr>";
                str += st;
            }
            return Content(str);
        }
    }
}
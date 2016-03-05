using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Manager;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;
using System.IO;
using System.Text;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize]
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = ManagerCore.Instance;

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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Controller = filterContext.RequestContext.RouteData.Values["controller"];
            ViewBag.Action = filterContext.RequestContext.RouteData.Values["action"];
            ViewBag.GroupType = Identity != null ? Identity.GroupType : GroupType.Guest;
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
            WriteExceptionLog(filterContext.HttpContext, ex);
        }

        private void WriteExceptionLog(HttpContextBase context, Exception ex)
        {
            if (context.Response.StatusCode < 500)
            {
                return;
            }
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                var content = new StringBuilder();
                content.AppendLine(context.Request.Url.AbsoluteUri);
                content.AppendLine(ex.Message);
                content.AppendLine(ex.StackTrace);
                content.AppendLine(ex.Source);
                System.IO.File.WriteAllText(Path.Combine(logPath, ex.GetType().Name + DateTime.Now.Ticks.ToString() + ".txt"), content.ToString());
            }
            catch
            {

            }
        }
    }
}
﻿using Newtonsoft.Json;
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

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize]
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = ManagerCore.Instance;

        protected User CurrentUser { get; private set; }

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
                var user = Core.UserManager.GetUser(Thread.CurrentPrincipal.Identity.Name);
                if (user == null)
                {
                    CurrentUser = new User { Username = Thread.CurrentPrincipal.Identity.Name };
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
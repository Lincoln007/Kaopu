using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (!Identity.IsAuthenticated)
            {
                return Redirect("/User/Login");
            }

            if (string.IsNullOrEmpty(Identity.Name))
            {
                return RedirectToAction("Register", "User");
            }

            ViewBag.NewList = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                Order = Model.UserTaskOrder.CreateTime,
                CreatorID = Identity.UserID,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            ViewBag.CompleteList = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                Order = Model.UserTaskOrder.ScheduleTime,
                CreatorID = Identity.UserID,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            return View();
        }
    }
}
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
            if (string.IsNullOrEmpty(CurrentUser.RealName))
            {
                return RedirectToAction("Register", "User");
            }

            ViewBag.NewList = Core.TaskManager.GetUserTasks(new Model.UserTaskQueryParameter
            {
                Order = Model.UserTaskOrder.CreateTime,
                UserID = CurrentUser.ID,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            ViewBag.CompleteList = Core.TaskManager.GetUserTasks(new Model.UserTaskQueryParameter
            {
                Order = Model.UserTaskOrder.ScheduleTime,
                UserID = CurrentUser.ID,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            return View();
        }
    }
}
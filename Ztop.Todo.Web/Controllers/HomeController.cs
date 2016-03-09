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

            ViewBag.ReceiveNewTasks = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                ReceiverID = Identity.UserID,
                GetCreator = true,
                Order = Model.UserTaskOrder.CreateTime,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            ViewBag.ReceiveUndoneTasks = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                Order = Model.UserTaskOrder.ScheduleTime,
                ReceiverID = Identity.UserID,
                GetCreator = true,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            ViewBag.CreateUndoneTasks = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                CreatorID = Identity.UserID,
                GetReceiver = true,
                IsCompleted = false,
                Page = new Model.PageParameter(1, 10),
            });

            ViewBag.CreateDoneTasks = Core.TaskManager.GetUserTasks(new Model.TaskQueryParameter
            {
                CreatorID = Identity.UserID,
                GetReceiver = true,
                IsCompleted = true,
                Page = new Model.PageParameter(1, 10),
            });

            return View();
        }
    }
}
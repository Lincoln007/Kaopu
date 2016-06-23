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

            #region  报销系统
            //return Redirect("/Report/Index");
            #endregion

            #region 任务系统
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
            #endregion
        }

        public ActionResult Import()
        {
            ViewBag.Data = Ztop.Todo.ActiveDirectory.ADController.GetUserDict(true, null);
            return View();
        }

        [HttpPost]
        public ActionResult Import(string[] users)
        {
            foreach (var str in users)
            {
                var arr = str.Split('_');
                var groupName = arr[0];
                var account = arr[1];
                var name = arr[2];

                var group = Core.UserManager.GetOrSetUserGroup(groupName);
                var user = Core.UserManager.GetUser(account) ?? new Model.User
                {
                    Username = account
                };
                user.GroupID = group.ID;
                user.RealName = name;

                Core.UserManager.Save(user);
            }
            return Content("导入成功");
        }

        public ActionResult Resources()
        {
            return View();
        }

        public ActionResult Daily()
        {
            return View();
        }
    }
}
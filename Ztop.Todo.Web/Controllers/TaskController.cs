using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class TaskController : ControllerBase
    {
        public ActionResult Index(int page = 1, int rows = 20)
        {
            var parameter = new UserTaskQueryParameter
            {
                UserID = CurrentUser.ID,
                Page = new PageParameter(page, rows),
            };

            //最近新添加的
            ViewBag.List = Core.TaskManager.GetUserTasks(parameter);
            ViewBag.Page = parameter.Page;
            return View();
        }

        public ActionResult Edit(int id = 0)
        {

            var model = Core.TaskManager.GetModel(id) ?? new Task();
            ViewBag.Model = model;
            if (model != null && model.ID > 0)
            {
                ViewBag.Users = Core.TaskManager.GetUsers(id);
                ViewBag.Attachments = Core.AttachmentManager.GetList(model.ID);
            }
            ViewBag.AllUsers = Core.UserManager.GetAllUsers();
            return View();
        }

        public ActionResult Save(Task data, int[] userIds)
        {
            var model = Core.TaskManager.GetModel(data.ID) ?? new Task
            {
                OwnerID = CurrentUser.ID,
            };
            model.ScheduledTime = data.ScheduledTime;
            model.Title = data.Title;
            model.Content = data.Content;
            if (userIds == null || userIds.Length == 0)
            {
                throw new ArgumentException("没有选择相关人员");
            }
            //相关人员
            foreach (var userId in userIds)
            {
                var user = Core.UserManager.GetUser(userId);
                if (user != null)
                {
                    model.Users.Add(user);
                }
            }
            if (model.Users.Count == 0)
            {
                throw new ArgumentException("没有选择相关人员");
            }
            Core.TaskManager.Save(model);
            //相关附件
            foreach(HttpPostedFileBase file in Request.Files)
            {
                Core.AttachmentManager.Upload(file, model.ID);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var model = Core.TaskManager.GetModel(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }
            ViewBag.Model = model;
            var hasRight = Core.TaskManager.HasRight(model.ID, CurrentUser.ID);
            if (hasRight)
            {
                ViewBag.Comments = Core.CommentManager.GetList(model.ID);
                ViewBag.Users = Core.TaskManager.GetUsers(model.ID);
                ViewBag.Attachments = Core.AttachmentManager.GetList(model.ID);
            }
            else
            {
                throw new HttpException(401, "你没有权限删除该任务");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var model = Core.TaskManager.GetModel(id);
            if (model != null)
            {
                if (model.OwnerID != CurrentUser.ID)
                {
                    throw new HttpException(401, "你没有权限删除该任务");
                }
                Core.TaskManager.Delete(id);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没找到该任务");
        }

        public ActionResult DeleteAttachment(int id)
        {
            var model = Core.AttachmentManager.GetModel(id);
            if (model != null)
            {
                if (!Core.TaskManager.HasRight(model.TaskID, CurrentUser.ID))
                {
                    throw new ArgumentException("权限不足");
                }
            }
            Core.AttachmentManager.Delete(id);
            return SuccessJsonResult();
        }

        public ActionResult DeleteComment(int id)
        {
            var model = Core.CommentManager.GetModel(id);
            if (model != null)
            {
                if (model.UserID != CurrentUser.ID)
                {
                    throw new ArgumentException("你没有权限删除该评论");
                }
                Core.CommentManager.Delete(model.ID);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没找到该评论");
        }
    }
}
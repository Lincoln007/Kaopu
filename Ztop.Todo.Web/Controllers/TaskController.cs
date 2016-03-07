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
        public ActionResult Index(string keyword, int? completed, int page = 1, int rows = 20)
        {
            var parameter = new TaskQueryParameter
            {
                SearchKey = keyword,
                IsCompleted = completed == null ? default(bool?) : (completed.Value == 1),
                CreatorID = Identity.UserID,
                Page = new PageParameter(page, rows),
            };

            //最近新添加的
            ViewBag.List = Core.TaskManager.GetUserTasks(parameter);
            ViewBag.Page = parameter.Page;
            return View();
        }

        public ActionResult Edit(int id = 0, string file = null)
        {
            var model = Core.TaskManager.GetModel(id) ?? new Task();
            ViewBag.Model = model;
            if (model != null && model.ID > 0)
            {
                ViewBag.Attachments = Core.AttachmentManager.GetList(model.ID);
            }
            ViewBag.File = file;
            return View();
        }

        public ActionResult SelectUser(int taskId = 0)
        {
            if (taskId > 0)
            {
                ViewBag.Users = Core.TaskManager.GetUsers(taskId);
            }
            ViewBag.Groups = Core.UserManager.GetUserGroups();
            ViewBag.AllUsers = Core.UserManager.GetAllUsers();
            return View();
        }

        public ActionResult Save(Task data, int[] userIds, string AddedFile)
        {
            var model = Core.TaskManager.GetModel(data.ID) ?? new Task
            {
                CreatorID = Identity.UserID,
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
            Core.TaskManager.Save(model);
            //相关附件
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                Core.AttachmentManager.Upload(file, model.ID);
            }
            if (!string.IsNullOrEmpty(AddedFile))
            {
                Core.AttachmentManager.Upload(AddedFile, model.ID);
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
            var userTasks = Core.TaskManager.GetUserTasks(model.ID);
            var userTask = userTasks.FirstOrDefault(e => e.UserID == Identity.UserID);
            if (userTask != null)
            {
                ViewBag.UserTasks = userTasks;
                ViewBag.UserTask = userTask;
                ViewBag.Comments = Core.CommentManager.GetList(model.ID);
                ViewBag.Attachments = Core.AttachmentManager.GetList(model.ID);
                //标记已读
                Core.TaskManager.ReadTask(model.ID, Identity.UserID);
            }
            else
            {
                throw new HttpException(401, "你没有权限查看该任务");
            }
            return View();
        }

        public ActionResult Copy(int id)
        {
            var model = Core.TaskManager.GetModel(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }
            ViewBag.Model = model;
            var selectedUsers = Core.TaskManager.GetUsers(model.ID);
            var allUsers = Core.UserManager.GetAllUsers();
            //转发会排除掉原任务已有的参与人员
            ViewBag.Users = allUsers.Where(e => !selectedUsers.Any(e1 => e1.ID == e.ID)).ToList();
            return View();
        }

        //public ActionResult SaveCopy(int id, int[] userIds)
        //{
        //    Task copy = null;
        //    var data = Core.TaskManager.GetModel(id);
        //    if (data != null)
        //    {
        //        copy = new Task
        //        {
        //            Title = data.Title,
        //            Content = data.Content,
        //            ScheduledTime = data.ScheduledTime,
        //            CreatorID = Identity.UserID,
        //            ParentID = data.ID,
        //        };
        //    }
        //    else
        //    {
        //        throw new ArgumentException("参数错误");
        //    }

        //    if (userIds == null || userIds.Length == 0)
        //    {
        //        throw new ArgumentException("没有选择相关人员");
        //    }
        //    //相关人员
        //    foreach (var userId in userIds)
        //    {
        //        var user = Core.UserManager.GetUser(userId);
        //        if (user != null)
        //        {
        //            copy.Users.Add(user);
        //        }
        //    }
        //    copy.Users.Add(CurrentUser);
        //    Core.TaskManager.Save(copy);
        //    Core.AttachmentManager.Copy(data.ID, copy.ID);
        //    return RedirectToAction("Index");
        //}

        public ActionResult Complete(int id)
        {
            if (!Core.TaskManager.HasRight(id, Identity.UserID))
            {
                throw new HttpException(401, "你没有权限完成该任务");
            }
            Core.TaskManager.CompleteTask(id, Identity.UserID);
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            var model = Core.TaskManager.GetModel(id);
            if (model != null)
            {
                if (model.CreatorID != Identity.UserID)
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
                if (!Core.TaskManager.HasRight(model.TaskID, Identity.UserID))
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
                if (model.UserID != Identity.UserID)
                {
                    throw new ArgumentException("你没有权限删除该评论");
                }
                Core.CommentManager.Delete(model.ID);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没找到该评论");
        }

        public ActionResult GetNewTask(DateTime lastGetTime)
        {
            var task = Core.TaskManager.GetNewTask(Identity.UserID, lastGetTime);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(task);
            return Content(json);
        }
    }
}
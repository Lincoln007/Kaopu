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
        public ActionResult Index(string keyword, bool isCreator = true, int days = 0, UserTaskOrder order = UserTaskOrder.CreateTime, bool? isCompleted = null, int queryId = 0, int page = 1, int rows = 20)
        {
            TaskQueryParameter parameter = null;
            //说明是立即查询
            if (queryId == -1)
            {
                parameter = new TaskQueryParameter
                {
                    SearchKey = keyword,
                    IsCompleted = isCompleted,
                    Order = order,
                    IsCreator = isCreator,
                    Page = new PageParameter(page, rows),
                    GetReceiver = true,
                    GetCreator = true,
                    Days = days,
                };
            }
            else if (queryId > 0)
            {
                var query = Core.QueryManager.GetModel(queryId);
                if (query != null)
                {
                    ViewBag.Query = query;
                    parameter = query.ConvertToTaskQueryParameter();
                }
            }

            ViewBag.Queries = Core.QueryManager.GetList(Identity.UserID);

            if (parameter != null)
            {
                parameter.GetCreator = true;
                parameter.GetReceiver = true;
                parameter.Page = new PageParameter(page, rows);
                //强制为当前用户的ID
                if (parameter.IsCreator)
                {
                    parameter.CreatorID = Identity.UserID;
                }
                else
                {
                    parameter.ReceiverID = Identity.UserID;
                }
                //最近新添加的
                ViewBag.Parameter = parameter;
                ViewBag.List = Core.TaskManager.GetUserTasks(parameter);
                ViewBag.Page = parameter.Page;
            }
            return View();
        }

        public ActionResult Edit(int id = 0, int parentId = 0, string file = null)
        {
            var isCopy = parentId > 0;
            var model = Core.TaskManager.GetTask(id == 0 ? parentId : id) ?? new Task();
            if (isCopy)
            {
                model.ID = 0;
                model.Users.Clear();
            }
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
                ViewBag.Users = Core.TaskManager.GetTaskUsers(taskId);
            }
            ViewBag.Groups = Core.UserManager.GetUserGroups();
            ViewBag.AllUsers = Core.UserManager.GetAllUsers();
            return View();
        }

        public ActionResult Save(Task data, int[] userIds, string AddedFile)
        {
            var model = Core.TaskManager.GetTask(data.ID) ?? new Task
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
            var model = Core.TaskManager.GetUserTask(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }

            ViewBag.Model = model;
            var hasRight = model.Task.CreatorID == Identity.UserID || model.UserID == Identity.UserID;
            if (!hasRight)
            {
                throw new HttpException(401, "你没有权限查看该任务");
            }
            ViewBag.Comments = Core.CommentManager.GetList(model.ID);
            ViewBag.Attachments = Core.AttachmentManager.GetList(model.TaskID);

            //标记已读
            if (model.UserID == Identity.UserID)
            {
                Core.TaskManager.FlagUserTaskRead(model.ID, Identity.UserID);
            }
            return View();
        }

        public ActionResult Copy(int id)
        {
            var model = Core.TaskManager.GetTask(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }
            ViewBag.Model = model;
            var selectedUsers = Core.TaskManager.GetTaskUsers(model.ID);
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

        public ActionResult Complete(int taskId)
        {
            if (!Core.TaskManager.HasRight(taskId, Identity.UserID))
            {
                throw new HttpException(401, "你没有权限完成该任务");
            }
            Core.TaskManager.CompleteTask(taskId, Identity.UserID);
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            var model = Core.TaskManager.GetTask(id);
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
            var data = Core.TaskManager.GetNewTask(Identity.UserID, lastGetTime);
            if (data == null)
            {
                return null;
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                data.ID,
                data.TaskID,
                data.UserID,
                data.Task.Title,
                data.Task.Content,
                data.Task.CreatorID,
                data.Task.CreatorName,
                data.Task.CreateTime,
                data.Task.ScheduledTime,
            });
            return Content(json);
        }
    }
}
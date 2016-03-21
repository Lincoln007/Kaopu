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
        public ActionResult Index(string keyword, bool isCreator = true, int days = 0, UserTaskOrder order = UserTaskOrder.CreateTime, bool? isCompleted = null, int queryId = -1, int page = 1, int rows = 20)
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
            ViewBag.Model = model;
            var attachments = Core.AttachmentManager.GetList(model.ID);
            if (isCopy)
            {
                model.ID = 0;
                ViewBag.Copy = true;
            }
            ViewBag.Attachments = attachments;
            ViewBag.ClientFile = file;
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

        public ActionResult Save(Task data, int[] userIds, int[] addedFiles = null, string clientFile = null)
        {
            if (string.IsNullOrEmpty(data.Title))
            {
                throw new ArgumentException("内容标题没有填写");
            }
            if (string.IsNullOrEmpty(data.Content))
            {
                throw new ArgumentException("任务内容没有填写");
            }

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
            var receivers = new List<User>();
            foreach (var userId in userIds)
            {
                var user = Core.UserManager.GetUser(userId);
                if (user != null)
                {
                    receivers.Add(user);
                }
            }
            Core.TaskManager.Save(model, receivers);
            //先处理编辑或拷贝时的旧附件，有的附件可能已被移除
            Core.AttachmentManager.UpdateFiles(model.ID, addedFiles);
            //再上传新的附件
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                Core.AttachmentManager.Upload(file, model.ID);
            }
            //上传客户端发送的附件
            if (!string.IsNullOrEmpty(clientFile))
            {
                Core.AttachmentManager.Upload(clientFile, model.ID);
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

        public ActionResult Complete(int id)
        {
            Core.TaskManager.CompleteTask(id, Identity.UserID);
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            Core.TaskManager.Delete(id, Identity.UserID);
            return SuccessJsonResult();
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
                CreatorName = data.Task.Creator.DisplayName,
                data.Task.CreateTime,
                data.Task.ScheduledTime,
            });
            return Content(json);
        }
    }
}
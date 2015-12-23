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
                ViewBag.Attachments = Core.AttachmentManager.GetList(model.ID);
                ViewBag.Comments = Core.CommentManager.GetList(model.ID);
                ViewBag.Users = Core.TaskManager.GetUsers(model.ID);
            }

            return View();
        }

        public ActionResult Detail(int id)
        {
            var model = Core.TaskManager.GetModel(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }
            ViewBag.Model = model;
            ViewBag.HasRight = Core.TaskManager.HasRight(model.ID, CurrentUser.ID);
            ViewBag.Comments = Core.CommentManager.GetList(model.ID);
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
            return ErrorJsonResult("参数错误，没找到该任务");
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
            return ErrorJsonResult("参数错误，没找到该评论");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class CommentController : ControllerBase
    {
        public ActionResult Save(int taskId, string content)
        {
            if (taskId == 0)
            {
                throw new ArgumentException("参数错误");
            }
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("评论内容没有填写");
            }
            Core.CommentManager.Save(new Model.Comment
            {
                Content = content,
                TaskID = taskId,
                UserID = CurrentUser.ID
            });
            return SuccessJsonResult();
        }
    }
}
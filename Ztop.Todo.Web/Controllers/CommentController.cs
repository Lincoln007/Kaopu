﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class CommentController : ControllerBase
    {
        public ActionResult Save(int userTaskId, string content)
        {
            if (userTaskId == 0)
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
                UserTaskID = userTaskId,
                UserID = Identity.UserID
            });
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            var model = Core.CommentManager.GetModel(id);
            if(model == null)
            {
                throw new ArgumentException("参数错误");
            }
            if(model.UserID != Identity.UserID)
            {
                throw new HttpException(401,"你没有权限删除该评论");
            }
            Core.CommentManager.Delete(id);
            return SuccessJsonResult();
        }
    }
}
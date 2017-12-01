using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project.Controllers
{
    public class UserController : ProjectControllerBase
    {
        public ActionResult AddUser(int projectId, string ignoreString = null, char ch = ';')
        {
            var project = Core.ProjectManager.Get(projectId);
            ViewBag.Project = project;
            if (!string.IsNullOrEmpty(ignoreString))
            {
                var array = ignoreString.Split(ch);
                ViewBag.Array = array;
            }
            var dict = Core.UserManager.GetDict();
            ViewBag.Dict = dict;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int projectId,int[] userId)
        {
            if (userId == null)
            {
                return ErrorJsonResult("请选择添加人员！");
            }
            var list = userId.Select(e => new ProjectUser { ProjectId = projectId, UserId = e }).ToList();
            Core.ProjectUserManager.Add(list);
            return SuccessJsonResult();
        }

        public ActionResult Select(int projectId)
        {
            var project = Core.ProjectManager.Get(projectId);
            ViewBag.Project = project;
            return View();
        }
    }
}
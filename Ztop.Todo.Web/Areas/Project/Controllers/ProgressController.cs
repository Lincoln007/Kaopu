using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project.Controllers
{
    public class ProgressController : ProjectControllerBase
    {
        // GET: Project/Progress
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int projectID)
        {
            var project = Core.ProjectManager.Get(projectID);
            var lastProgress = Core.Project_ProgressManager.GetLast(projectID);
            ViewBag.Project = project;
            ViewBag.Last = lastProgress;
            return View();
        }

        [HttpPost]
        public ActionResult Save(ProjectProgress progress,double Min)
        {
            if (progress.Percent <= Min)
            {
                return ErrorJsonResult("录入的项目进度百分比不能低于之前的百分比，请核对！");
            }
            var id = Core.Project_ProgressManager.Save(progress);
            if (id > 0)
            {
                if (Math.Abs(progress.Percent - 100) < 0.01)//一旦录入项目进度为100%；
                {
                    if (Core.ProjectManager.Done(progress.ProjectID,true))
                    {
                        return SuccessJsonResult();
                    }
                    else
                    {
                        return ErrorJsonResult("成功录入项目进度，但是更新项目完成状态失败!");
                    }
                }
                return SuccessJsonResult();
            }
            return ErrorJsonResult("录入工作进度失败!");
        }
    }
}
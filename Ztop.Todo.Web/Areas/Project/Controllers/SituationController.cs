using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project.Controllers
{
    public class SituationController : ProjectControllerBase
    {
        public ActionResult Create(int id=0,int projectId=0)
        {
            var project = Core.ProjectManager.Get(projectId);
            ViewBag.Project = project;
            var situation = Core.ProjectSituationManager.Get(id);
            ViewBag.Situation = situation;
            return View();
        }

        [HttpPost]
        public ActionResult Save(ProjectSituation situation)
        {
            if (string.IsNullOrEmpty(situation.Content))
            {
                return ErrorJsonResult("工作情况不能为空！");
            }
            if (situation.ID>0)
            {
                if (!Core.ProjectSituationManager.Edit(situation))
                {
                    return ErrorJsonResult("编辑工作情况失败！");
                }
                Core.ProjectRecordManager.Save(new ProjectRecord
                {
                    Content = string.Format("{0}编辑工作情况", Identity.Name),
                    ProjectId = situation.ProjectId,
                    UserId=situation.UserId
                });
            }
            else
            {
                var id = Core.ProjectSituationManager.Add(situation);
                if (id <= 0)
                {
                    return ErrorJsonResult("录入工作情况失败!");
                }
                var recordId = Core.ProjectRecordManager.Save(new ProjectRecord
                {
                    Content = string.Format("{0}录入工作情况", Identity.Name),
                    ProjectId = situation.ProjectId,
                    UserId=situation.UserId
                });
            }
            return SuccessJsonResult();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project.Controllers
{
    public class WorkloadController : ProjectControllerBase
    {

        public ActionResult Create(int projectId)
        {
            var project = Core.ProjectManager.Get(projectId);
            ViewBag.Project = project;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int projectId,int[] userId,double[] percent)
        {
            if (userId == null || percent == null || userId.Length != percent.Length)
            {
                return ErrorJsonResult("未获取工作量信息！");
            }
            if (Math.Abs(percent.Sum() - 100) > 0.01)
            {
                return ErrorJsonResult("工作量未达到100%");
            }
            var list = new List<WorkLoad>();
            for(var i = 0; i < userId.Length; i++)
            {
                list.Add(new WorkLoad { ProjectId = projectId, UserId = userId[i], Percent = percent[i] });
            }
            Core.WorkloadManager.Update(list);
            var recordId = Core.ProjectRecordManager.Save(new ProjectRecord { ProjectId = projectId, Content = string.Format("{0}登记项目工作量", Identity.Name) });
            return SuccessJsonResult();
        }
    }
}
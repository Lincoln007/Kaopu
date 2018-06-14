using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
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
        public ActionResult Save(int projectId, int[] year,double[] percent,string[] content,int[] userId,double[] ppercent,string[] pContent)
        {
            if (year == null || year.Length == 0 || percent == null || content == null || year.Length != percent.Length || percent.Length != content.Length)
            {
                return ErrorJsonResult("获取参数失败！请创新重试");
            }
            var project = Core.ProjectManager.Get(projectId);
            if (project == null)
            {
                return ErrorJsonResult("项目信息获取失败！");
            }
            var currentUsers = project.ProjectUser.OrderBy(e => e.UserId).Select(e => e.UserId).ToArray();
            if (currentUsers == null)
            {
                return ErrorJsonResult("读取参数失败!");
            }
            var queue = userId.Tranlate();
            var list = ProgressTable.Generate(projectId,currentUsers, year, percent, content,queue,ppercent,pContent);
            if (list == null)
            {
                return ErrorJsonResult("生成工作量表格失败！");
            }
            if (Math.Abs(list.Sum(e => e.Percent) - project.RecentProgress.Percent) > 0.01)
            {
                return ErrorJsonResult("分析参数错误（项目阶段百分比合计不等于工作进度百分比），请刷新重试！");
            }
            Core.ProgressTableManager.Update(list);
            return SuccessJsonResult();
        }

        public ActionResult Detail(int projectId)
        {
            var project = Core.ProjectManager.Get(projectId);
            ViewBag.Project = project;
            return View();
        }
    }
}
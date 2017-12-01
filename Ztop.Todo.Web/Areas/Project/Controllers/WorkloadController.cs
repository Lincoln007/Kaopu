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


        public ActionResult Create(int progressId)
        {
            var project_progress = Core.Project_ProgressManager.GetEntry(progressId);
            ViewBag.Progress = project_progress;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int progressId, int[] year,double[] percent,string[] content,int[] userId,double[] ppercent)
        {
            if (year == null || year.Length == 0 || percent == null || content == null || year.Length != percent.Length || percent.Length != content.Length)
            {
                return ErrorJsonResult("获取参数失败！请创新重试");
            }
            var progress = Core.Project_ProgressManager.GetEntry(progressId);
            if (progress == null)
            {
                return ErrorJsonResult("工作进度信息获取失败！");
            }
            var currentUsers = progress.Project.ProjectUser.OrderBy(e => e.UserId).Select(e => e.UserId).ToArray();
            if (currentUsers == null)
            {
                return ErrorJsonResult("读取参数失败!");
            }
            var queue = userId.Tranlate();
            var list = ProgressTable.Generate(progressId,currentUsers, year, percent, content,queue,ppercent);
            if (list == null)
            {
                return ErrorJsonResult("生成工作量表格失败！");
            }
            if (Math.Abs(list.Sum(e => e.Percent) - progress.Percent) > 0.01)
            {
                return ErrorJsonResult("分析参数错误（项目阶段百分比合计不等于工作进度百分比），请刷新重试！");
            }
            Core.ProgressTableManager.Update(list);
            return SuccessJsonResult();
        }

        public ActionResult Detail(int progressId)
        {
            var progress = Core.Project_ProgressManager.GetEntry(progressId);
            ViewBag.Progress = progress;
            return View();
        }
    }
}
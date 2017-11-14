using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
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
        public ActionResult Save(ProjectProgress progress,double Min,string projectName)
        {
            if (progress.Percent <= Min)
            {
                throw new ArgumentException("录入的项目进度百分比不能低于之前的百分比，请核对！");
               // return ErrorJsonResult("录入的项目进度百分比不能低于之前的百分比，请核对！");
            }
            var id = Core.Project_ProgressManager.Save(progress);
            if (id > 0)
            {
                if (Math.Abs(progress.Percent - 100) < 0.01)//一旦录入项目进度为100%；
                {
                    var replyPath = string.Empty;
                    if (Request.Files.Count > 0)
                    {
                        var file = HttpContext.Request.Files[0];
                        var fileName = file.FileName;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var ext = Path.GetExtension(fileName);
                            if (ext == ".pdf")
                            {
                                replyPath = FileManager.UploadProject(file, projectName);
                            }
                        }
                    }
                    if (Core.ProjectManager.Done(progress.ProjectID,true,replyPath))
                    {
                        var recordId = Core.ProjectRecordManager.Save(new ProjectRecord { ProjectId = progress.ProjectID, Content = string.Format("{0}标记项目完成100%{1}", Identity.Name, string.IsNullOrEmpty(replyPath) ? "" : ";并上传项目批复文件") });
                        return Redirect("/Project/Home/Detail?id=" + progress.ProjectID);
                        //return SuccessJsonResult();
                    }
                    else
                    {
                        throw new ArgumentException("成功录入项目进度，但是更新项目完成状态失败!");
                        //return ErrorJsonResult("成功录入项目进度，但是更新项目完成状态失败!");
                    }
                }
                return Redirect("/Project/Home/Detail?id=" + progress.ProjectID);
               // return SuccessJsonResult();
            }
            throw new ArgumentException("录入公告进度失败！");
            //return ErrorJsonResult("录入工作进度失败!");
        }
    }
}
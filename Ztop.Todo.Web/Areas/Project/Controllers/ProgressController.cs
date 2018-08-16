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
        public ActionResult Save(ProjectProgress progress,double Min,string projectName,string replyRemark,int[] userId)
        {
            //应袁总要求，工作进度可以随便填写，不需要验证 20180702 修改注释
            //if (progress.Percent <= Min)
            //{
            //    throw new ArgumentException("录入的项目进度百分比不能低于之前的百分比，请核对！");
            //}
            var replyPath = string.Empty;
            var flag = false;
            if (Math.Abs(progress.Percent - 100) < 0.01)//输入100%的时候,进行校验
            {
                #region 核对项目参与人员
                if (userId == null)
                {
                    throw new ArgumentException("请确认项目参与人员！");
                }
                else
                {
                    var project = Core.ProjectManager.Get(progress.ProjectID);
                    if (project == null||project.ProjectUser==null||project.ProjectUser.Count==0)
                    {
                        throw new ArgumentException("未获取项目信息,无法校验项目参与人员！");
                    }
                    else
                    {
                        var lack = false;
                        foreach(var item in project.ProjectUser)
                        {
                            if (!userId.Contains(item.UserId))
                            {
                                lack = true;
                                break;
                            }
                        }
                        if (lack)
                        {
                            throw new ArgumentException("请确认勾选每个项目参与人员！");
                        }
                    }
                }
                #endregion

                #region 核对批复文件
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

                #endregion

                flag = true;
            }

            var id = Core.Project_ProgressManager.Save(progress);
            if (id > 0)
            {
                Core.ProjectRecordManager.Save(new ProjectRecord
                {
                    Content = string.Format("{0}录入工作进度", Identity.Name),
                    ProjectId = progress.ProjectID,
                    UserId=progress.UserID
                });
                if (flag)//一旦录入项目进度为100%；并通过校验
                {
                    if (Core.ProjectManager.Done(progress.ProjectID,true,replyPath,replyRemark))
                    {
                        var recordId = Core.ProjectRecordManager.Save(new ProjectRecord
                        {
                            ProjectId = progress.ProjectID,
                            Content = string.Format("{0}标记项目完成100%{1}", Identity.Name, string.IsNullOrEmpty(replyPath) ? "" : ";并上传项目批复文件"),
                            UserId=progress.UserID
                        });
                        return Redirect("/Project/Home/Detail?id=" + progress.ProjectID);
                    }
                    else
                    {
                        throw new ArgumentException("成功录入项目进度，但是更新项目完成状态失败!");
                    }
                }
                return Redirect("/Project/Home/Detail?id=" + progress.ProjectID);
            }
            throw new ArgumentException("录入公告进度失败！");
        }


        public ActionResult Edit(int id)
        {
            var progress = Core.Project_ProgressManager.Get(id);
            ViewBag.Progress = progress;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProjectProgress progress)
        {
            return SuccessJsonResult();
        }
    }
}
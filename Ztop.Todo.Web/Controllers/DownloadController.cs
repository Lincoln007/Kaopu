using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Manager;
using Ztop.Todo.Manager.Excels;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class DownloadController : ControllerBase
    {
        // GET: Download
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Sheet(DownloadEnum download)
        {
            var stringkey = string.Format("{0}{1}", Identity.UserID, ParameterManager.ShentuKey);
            var list = SessionHelper.GetSession(stringkey) as List<Sheet>;
            //var list = RedisManager.Get<List<Sheet>>(string.Format("{0}{1}", Identity.UserID, ParameterManager.ShentuKey), RedisManager.Client);
            if (list == null)
            {
                return Content("未读取到报销单信息");
            }
            IWorkbook workbook = ExcelManager.WriteSheets(list, download);
            if (workbook == null)
            {
                return Content("创建表格失败");
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, ParameterManager.ExcelContentType, string.Format("{0} {1}.xls", download.GetDescription(), DateTime.Now.ToString(ParameterManager.TimeContentType)));
        }

        public ActionResult Verify(DownloadEnum download)
        {
            var stringKey = Identity.UserID + ParameterManager.ParameterKey;
            var parameter = SessionHelper.GetSession(stringKey) as SheetVerifyParameter;
            //var parameter = RedisManager.Get<SheetVerifyParameter>(, RedisManager.Client);
            if (parameter == null)
            {
                return Content("未获取查询条件！");
            }
            var list = Core.VerifyViewManager.Search(parameter, true);
            if (list == null)
            {
                return Content("未读取到报销单信息");
            }
            IWorkbook workbook = ExcelManager.WriteVerify(list, download);
            if (workbook == null)
            {
                return Content("创建表格失败!");
            }

            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, ParameterManager.ExcelContentType, string.Format("{0} {1}.xls", download.GetDescription(), DateTime.Now.ToString(ParameterManager.TimeContentType)));
        }


        public ActionResult Projects()
        {
            var stringKey = Identity.UserID + ParameterManager.ParameterKey;
            var parameter = SessionHelper.GetSession(stringKey) as ProjectParameter;
            if (parameter == null)
            {
                return Content("未获取当前查询条件！");
            }
            parameter.Page = null;
            var list = Core.ProjectManager.Search(parameter);
            if (list == null)
            {
                return Content("查询项目信息失败！");
            }
            IWorkbook workbook = ExcelManager.WriteCoProjects(list);
            if (workbook == null)
            {
                return Content("生成表格失败！");
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, ParameterManager.ExcelContentType, string.Format("项目清单-{0}.xls", DateTime.Now.ToString(ParameterManager.TimeContentType)));
        }
        public ActionResult ADProjects(int year)
        {
            var stringKey = Identity.UserID + ParameterManager.ParameterKey;
            var parameter = SessionHelper.GetSession(stringKey) as ProjectParameter;
            if (parameter == null)
            {
                return Content("未获取当前查询条件！");
            }
            parameter.Page = null;
            var list = Core.ProjectManager.Search(parameter);
            if (list == null)
            {
                return Content("查询项目信息失败！");
            }
            var tables = Core.ProgressTableManager.Search(year);
            if (tables == null)
            {
                return Content("未获取年度工作量信息!");
            }
            var users = Core.UserManager.GetAllUsers().Where(e => e.Delete == false&&e.GroupID!=7).OrderBy(e => e.ID).ToList();
            IWorkbook workbook = ExcelManager.WriteAdProjects(list, year, tables, users);
            if (workbook == null)
            {
                return Content("生成表格失败！");
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, ParameterManager.ExcelContentType, string.Format("工作量-{0}.xlsx", DateTime.Now.ToString(ParameterManager.TimeContentType)));
        }
    }
}
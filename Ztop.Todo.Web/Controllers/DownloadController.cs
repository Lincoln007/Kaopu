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
            var list = RedisManager.Get<List<Sheet>>(string.Format("{0}{1}", Identity.UserID, ParameterManager.ShentuKey), RedisManager.Client);
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
            var parameter = RedisManager.Get<SheetVerifyParameter>(Identity.UserID + ParameterManager.ParameterKey, RedisManager.Client);
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
    }
}
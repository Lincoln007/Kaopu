using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize]
    public class ProjectTypeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Select()
        {
            var list = Core.Project_TypeManager.GetByPPID(0);
            ViewBag.List = list;
            return View();
        }

        public ActionResult Get(int ppid)
        {
            var list = Core.Project_TypeManager.GetByPPID(ppid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
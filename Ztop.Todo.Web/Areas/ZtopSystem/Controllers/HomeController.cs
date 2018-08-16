using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Areas.ZtopSystem.Controllers
{
    public class HomeController : ZtopSystemControllerBase
    {
        // GET: ZtopSystem/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
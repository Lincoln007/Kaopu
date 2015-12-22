using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if(!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
    }
}
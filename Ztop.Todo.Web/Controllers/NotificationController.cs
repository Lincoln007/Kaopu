using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class NotificationController : ControllerBase
    {
        public ActionResult GetNewest()
        {
            return View();
        }
    }
}
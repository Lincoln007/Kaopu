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
            var model = Core.NotificationManager.GetNewest(Identity.UserID);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }
    }
}
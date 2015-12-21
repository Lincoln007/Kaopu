using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize(Enabled = false)]
    public class UserController : ControllerBase
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}
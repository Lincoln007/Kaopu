using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class UserController : ControllerBase
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(string realname)
        {
            if(string.IsNullOrEmpty(realname))
            {
                throw new ArgumentException("请填写真实姓名");
            }
            CurrentUser.RealName = realname;
            Core.UserManager.Save(CurrentUser);
            return Redirect("/");
        }
    }
}
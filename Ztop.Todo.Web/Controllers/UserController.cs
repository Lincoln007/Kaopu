using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class UserController : ControllerBase
    {
        public bool ADController { get; private set; }

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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Username,string Password)
        {
            if (ADLogin(Username, Password))
            {
                return Redirect("/Home/Index");
            }
            else
            {
                throw new ArgumentException("用户名或者密码错误");
            }
            return View();
        }
        public ActionResult LoginOut()
        {
            HttpContext.ClearAuth();
            return RedirectToAction("Login");
        }
    }
}
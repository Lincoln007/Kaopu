using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize(false)]
    public class UserController : ControllerBase
    {
        public ActionResult Login()
        {
            return View();
        }

        private User LoginActiveDirectory(string username, string password)
        {
            if (ADController.TryLogin(username, password))
            {
                var user = Core.UserManager.GetUser(username);
                if (user == null)
                {
                    user = new User { Username = username };
                    Core.UserManager.Save(user);
                }
                return user;
            }
            return null;
        }

        [HttpPost]
        public ActionResult LoginResult(string username, string password)
        {
            var user = LoginActiveDirectory(username, password);
            if (user == null)
            {
                throw new HttpException(401, "用户名或密码错误");
            }
            else
            {
                HttpContext.SaveAuth(user);
                return SuccessJsonResult(user);  
            }
        }
        public ActionResult LoginOut()
        {
            HttpContext.ClearAuth();
            return RedirectToAction("Login");
        }
    }
}
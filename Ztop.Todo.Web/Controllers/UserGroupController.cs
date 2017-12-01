using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize]
    public class UserGroupController : ControllerBase
    {
        // GET: UserGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Select(string selectString=null,char ch=';')
        {
            if (!string.IsNullOrEmpty(selectString))
            {
                var array = selectString.Split(ch);
                ViewBag.Array = array;
            }
            var dict = Core.UserManager.GetDict();
            ViewBag.Dict = dict;
            return View();
        }



        public ActionResult ShowGroup()
        {

            return View();
        }

        public ActionResult SelectGroup()
        {
            ViewBag.Groups = Core.UserGroupManager.Get();
            return View();
        }

        public ActionResult GetUser(int groupId)
        {
            var list = Core.UserManager.GetUsers(groupId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
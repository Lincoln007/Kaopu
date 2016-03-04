using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web.Areas.Jurisdiction.Controllers
{
    public class AdminController : JurisdictionControllerBase
    {
        // GET: Jurisdiction/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserList(bool? IsActive=null,string key = null)
        {
            ViewBag.Users = Ztop.Todo.Common.ADController.GetUserDict(IsActive, key);
            ViewBag.Organization = System.Configuration.ConfigurationManager.AppSettings["PEOPLE"].GetOrganizations();
            return View();
        }
        public ActionResult Group()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(string Name,string sAMAccountName,string Organization,string FirstPassword)
        {
            if (string.IsNullOrEmpty(sAMAccountName) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Organization) || string.IsNullOrEmpty(FirstPassword))
            {
                throw new ArgumentException("姓名、账户名或者初始密码不能为空");
            }
            try
            {
                ADController.CreateUser(Name, sAMAccountName, Organization, FirstPassword);
            }catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }

        public ActionResult DisableUser(string sAMAccountName)
        {
            try
            {
                sAMAccountName.DisableAccount();
            }catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }
        public ActionResult ActiveUser(string sAMAccountName)
        {
            try
            {
                sAMAccountName.ActiveAccount();
            }catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }
        [HttpPost]
        public ActionResult Move(string sAMAccountName,string NewOrganization)
        {
            try
            {
                if (!sAMAccountName.MoveUserToGroup(NewOrganization))
                {
                    throw new ArgumentException("移动用户到新租失败！");
                }
            }catch(Exception ex)
            {
                throw new ArgumentException("移动用户到新组失败，错误信息：" + ex.Message);
            }
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }

        public ActionResult GJson()
        {
            var treeObject = ADController.GetTreeObject();
            return Json(treeObject, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Impower()
        {
            ViewBag.List = Core.AuthorizeManager.GetList();
            ViewBag.Groups = ADController.GetGroupDict().Sort().DictToTable();
            return View();
        }
        [HttpPost]
        public ActionResult Impower(Authorize authorize)
        {
            Core.AuthorizeManager.Add(Core.AuthorizeManager.Get(HttpContext));
            ViewBag.List = Core.AuthorizeManager.GetList();
            ViewBag.Groups = ADController.GetGroupDict().Sort().DictToTable();
            return View();
        }

        [HttpPost]
        public ActionResult ImpowerEdit(int ID)
        {
            Core.AuthorizeManager.Edit(Core.AuthorizeManager.Get(HttpContext, ID));
            return Redirect("/Jurisdiction/Admin/Impower");
        }

        public ActionResult Gain()
        {
            var list = ADController.GetUserList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult History(bool?Label,CheckStatus status=CheckStatus.All,string Checker=null,string Name=null,string GroupName=null,int page=1)
        {
            var filter = new DataBookFilter
            {
                Status = status,
                Checker = Checker,
                Name = Name,
                GroupName = GroupName,
                Label = Label,
                Page = new PageParameter(page,20)
            };
            ViewBag.List = Core.DataBookManager.Get(filter);
            ViewBag.Page = filter.Page;
            var list = Core.DataBookManager.GetList();
            ViewBag.NList = list.GroupBy(e => e.Name).Select(e => e.Key).ToList();
            ViewBag.GList = list.GroupBy(e => e.GroupName).Select(e => e.Key).ToList();
            ViewBag.CList = list.GroupBy(e => e.Checker).Select(e => e.Key).ToList();
            return View();
        }
    }
}
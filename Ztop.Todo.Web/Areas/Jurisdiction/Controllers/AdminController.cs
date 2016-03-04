using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

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
            //ViewBag.DICT = ADController.Gain();
            //var list = ADController.GetAllOrganization();
            //list.Remove(System.Configuration.ConfigurationManager.AppSettings["PEOPLE"]);
            //ViewBag.List = list;
            //ViewBag.Tree = ADController.GetTree();
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
            return RedirectToAction("UserList", new { IsActive = true });
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
            return RedirectToAction("UserList", new { IsActive = true });
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
            return RedirectToAction("UserList", new { IsActive = true });
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
            return RedirectToAction("UserList", new { IsActive = true });
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

        public ActionResult Gain()
        {
            var list = ADController.GetUserList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Manager()
        {
            var groups = ADController.GetGroupList();
            ViewBag.Wait = Core.DataBookManager.Get(groups, Model.CheckStatus.Wait);
            return View();
        }

        [HttpPost]
        public ActionResult Manager(int ID,string Reason,int? Day,bool? Check,CheckStatus status = CheckStatus.Wait)
        {
            var book= Core.DataBookManager.Check(ID, Reason, Identity.Name, Day, Check, status);
            Core.MessageManager.Add(new Message
            {
                Sender = Identity.Name,
                Info = string.Format("申请{0}的权限已经确认！", book.GroupName),
                Receiver = book.Name
            });
            var groups = ADController.GetGroupList();
            ViewBag.Wait = Core.DataBookManager.Get(groups, CheckStatus.Wait);
            ViewBag.DGroups = ADController.GetUserDict(groups);
            return View();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web.Controllers
{
    public class AuthorityController : ControllerBase
    {

        [UserRole(groupType = GroupType.Member)]
        /// <summary>
        /// 授权审批
        /// </summary>
        /// <returns></returns>
        public ActionResult Manager()
        {
            var groups = ADController.GetGroupList();
            ViewBag.Wait = Core.DataBookManager.Get(groups, Model.CheckStatus.Wait);
            if (Identity.GroupType == GroupType.Manager)
            {
                ViewBag.DGroups = ADController.GetUserDict(groups);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Manager(int ID, string Reason, int? Day, bool? Check, CheckStatus status = CheckStatus.Wait)
        {
            var book = Core.DataBookManager.Check(ID, Reason, Identity.Name, Day, Check, status);
            Core.MessageManager.Add(new Message
            {
                Sender = Identity.Name,
                Info = string.Format("申请{0}的权限已经确认！", book.GroupName),
                Receiver = book.Name
            });
            var groups = ADController.GetGroupList();
            ViewBag.Wait = Core.DataBookManager.Get(groups, CheckStatus.Wait);
            if (Identity.GroupType == GroupType.Manager)
            {
                ViewBag.DGroups = ADController.GetUserDict(groups);
            }

            return View();
        }


        [UserRole(groupType = GroupType.Administrator)]
        /// <summary>
        /// 申请权限  管理者和普通用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Apply()
        {
            ViewBag.ManagerList = Core.AuthorizeManager.GetAllManager();
            ViewBag.User = new AUser
            {
                Name = Identity.Name,
                MGroup = Identity.Name.GetGroupList()
            };
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Boss)
        {
            var groups = HttpContext.GetValue("Group");
            List<string> None;
            List<string> Have;
            List<int> Indexs;
            Core.AuthorizeManager.Screen(groups, Identity.sAMAccountName, out None, out Have);
            try
            {
                Indexs = Core.DataBookManager.Add(None, Identity.Name,Identity.sAMAccountName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            ViewBag.Have = Have;
            ViewBag.Book = Core.DataBookManager.Get(Indexs);
            return View("ASuccess");
        }

        /// <summary>
        /// 获取管理者所管理的组列表
        /// </summary>
        /// <param name="Boss"></param>
        /// <returns></returns>
        public ActionResult GetGroupList(string Boss)
        {
            var html = Core.AuthorizeManager.GetList(Boss);
            return HtmlResult(html);
        }

        protected ActionResult HtmlResult(List<string> html)
        {
            var values = html.ListToTable();
            var str = string.Empty;
            foreach (var item in values)
            {
                var st = string.Empty;
                st += "<tr>";
                foreach (var entry in item)
                {
                    if (string.IsNullOrEmpty(entry))
                    {
                        continue;
                    }
                    st += "<td><label class='checkbox-inline'><input type='checkbox' name='Group' value='" + entry + "' />" + entry + "</label></td>";
                }
                st += "</tr>";
                str += st;
            }
            return Content(str);
        }

        [UserRole(groupType = GroupType.Administrator)]
        /// <summary>
        /// 申请权限历史 管理者和普通用户
        /// </summary>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult History(CheckStatus status = CheckStatus.All, int page = 1)
        {
            var filter = new DataBookFilter
            {
                Name = Identity.Name,
                Status = status,
                Page = new PageParameter(page, 20)
            };
            ViewBag.List = Core.DataBookManager.Get(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        [UserRole(groupType = GroupType.Member)]
        /// <summary>
        /// 授权审批历史  管理者和Administrator
        /// </summary>
        /// <param name="Label"></param>
        /// <param name="status"></param>
        /// <param name="Checker"></param>
        /// <param name="Name"></param>
        /// <param name="GroupName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult CHistory(bool? Label, CheckStatus status = CheckStatus.All, string Checker = null, string Name = null, string GroupName = null, int page = 1)
        {

            var filter = new DataBookFilter
            {
                Status = status,
                Checker = Identity.GroupType == GroupType.Manager ? Identity.Name : Checker,
                Name = Name,
                GroupName = GroupName,
                Label = Label,
                Page = new PageParameter(page, 20)
            };
            ViewBag.List = Core.DataBookManager.Get(filter);
            ViewBag.Page = filter.Page;
            var list = Core.DataBookManager.GetList(Identity.GroupType == GroupType.Manager ? Identity.Name : null);
            ViewBag.NList = list.GroupBy(e => e.Name).Select(e => e.Key).ToList();
            ViewBag.GList = list.GroupBy(e => e.GroupName).Select(e => e.Key).ToList();
            ViewBag.CList = list.GroupBy(e => e.Checker).Select(e => e.Key).ToList();
            return View();
        }
    }
}
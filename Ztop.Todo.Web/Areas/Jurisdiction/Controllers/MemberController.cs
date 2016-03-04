using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;
using Ztop.Todo.Common;

namespace Ztop.Todo.Web.Areas.Jurisdiction.Controllers
{
    public class MemberController : JurisdictionControllerBase
    {
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

        public ActionResult GetGroupList(string Boss)
        {
            var html = Core.AuthorizeManager.GetList(Boss);
            return HtmlResult(html);
        }
        [HttpPost]
        public ActionResult Apply(string Boss)
        {
            var groups = HttpContext.GetValue("Group");
            List<string> None;
            List<string> Have;
            List<int> Indexs;
            Core.AuthorizeManager.Screen(groups, Identity.Name, out None, out Have);
            try
            {
                Indexs = Core.DataBookManager.Add(None, Identity.Name);

            }catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            ViewBag.Have = Have;
            ViewBag.Book = Core.DataBookManager.Get(Indexs);
            return View("ASuccess");
        }

        public ActionResult History(CheckStatus status=CheckStatus.All,int page = 1)
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
    }
}
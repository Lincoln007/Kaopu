﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Common;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web.Areas.Jurisdiction.Controllers
{
    [UserRole(groupType =GroupType.Administrator,Mode =true)]
    public class AdminController : JurisdictionControllerBase
    {
        // GET: Jurisdiction/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserList(bool? IsActive = null, string key = null)
        {
            ViewBag.Users = ADController.GetUserDict(IsActive, key);
            ViewBag.Organization = ADController.GetOrganizations(System.Configuration.ConfigurationManager.AppSettings["PEOPLE"]);
            return View();
        }
        public ActionResult Group()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(string Name, string sAMAccountName, string Organization, string FirstPassword)
        {
            if (string.IsNullOrEmpty(sAMAccountName) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Organization) || string.IsNullOrEmpty(FirstPassword))
            {
                throw new ArgumentException("姓名、账户名或者初始密码不能为空");
            }
            try
            {
                ADController.CreateUser(Name, sAMAccountName, Organization, FirstPassword);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }

        public ActionResult DisableUser(string sAMAccountName)
        {
            ADController.DisableAccount(sAMAccountName);
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }
        public ActionResult ActiveUser(string sAMAccountName)
        {
            ADController.ActiveAccount(sAMAccountName);
            return Redirect("/Jurisdiction/Admin/UserList?IsActive=true");
        }
        [HttpPost]
        public ActionResult Move(string sAMAccountName, string NewOrganization)
        {
            try
            {
                if (!ADController.MoveUserToGroup(sAMAccountName, NewOrganization))
                {
                    throw new ArgumentException("移动用户到新租失败！");
                }
            }
            catch (Exception ex)
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
        /// <summary>
        /// 作用：权限系统管理界面
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日12:10:43
        /// </summary>
        /// <returns></returns>
        public ActionResult Impower()
        {
            Core.AuthorizeManager.Add(Core.AuthorizeManager.Get(HttpContext));
            ViewBag.ADGroup = Core.AD_groupManager.Get();
            return View();
        }
        /// <summary>
        /// 作用：查看授权管理用户列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日12:10:10
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAuthorizes()
        {
            ViewBag.Dict = Core.AuthorizeManager.GetAuthorizeFasts();
            return View();
        }
        /// <summary>
        /// 作用：编辑权限管理列表
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日10:46:45
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditImpower(string name)
        {
            var FGUV = Core.AuthorizeManager.GetFGUV(name);
            var groups = FGUV.Select(e=>e.Name).ToList();
            ViewBag.MGroups = groups;
            ViewBag.Name = name;
            ViewBag.UID = FGUV.FirstOrDefault().UID;
            ViewBag.ADGroup = Core.AD_groupManager.Get();
            return View();
        }

        public ActionResult DeleteImpower(string name)
        {
            Core.AuthorizeManager.Delete(name);
            return RedirectToAction("Impower");
        }

        public ActionResult Gain()
        {
            var list = ADController.GetUserList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作用：权限管理编辑
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日10:45:44
        /// </summary>
        /// <param name="ID">管理者ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImpowerEdit(int ID)
        {
            var groups = HttpContext.Request.Form["GroupName"].ToString();
            Core.AuthorizeManager.Edit(groups, ID);
           // Core.AuthorizeManager.Edit(Core.AuthorizeManager.Get(HttpContext, ID),ID);
            return Redirect("/Jurisdiction/Admin/Impower");
        }


        /// <summary>
        /// 作用：获取最高级选择菜单
        /// 作者：汪建龙
        /// 编写时间：2016-10-31
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBig()
        {
            var list = XmlHelper.GetDitrict();
            return HtmlResult(list, "/Admin/GetFirst?name=");
        }

        /// <summary>
        /// 作用：获取一级选择地区菜单
        /// 作者：汪建龙
        /// 编写时间：2016-10-31
        /// </summary>
        /// <param name="name">上级名称（最高级）</param>
        /// <returns></returns>
        public ActionResult GetFirst(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if(Regex.IsMatch(name, @"^[0-9]{6}[\u4e00-\u9fa5]{0,}$"))
                {
                    var key = name.Substring(0, 4);
                    if (!string.IsNullOrEmpty(key))
                    {
                        var children = Core.AD_groupManager.GetOrganication().Where(e => e.Name.StartsWith(key)).Select(e => e.Name).ToList();
                        return HtmlResult(children,"/Admin/GetSecond?name=");
                    }
                  
                }
            }
            return Content("<tr><td>参数错误，请重新加载网页！</td></tr>");
        }
        public ActionResult GetSecond(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var children = Core.AD_groupManager.GetGroupByOrganication(name).Select(e => e.Name).ToList();
                return HtmlResult(children, "");
            }
            return Content("<tr><td>参数错误，请重新加载网页！</td></tr>");
        }
        
        protected ActionResult HtmlResult(List<string> list,string hrefhead)
        {
            var values = list.ListToTable();
            var str = string.Empty;
            foreach(var item in values)
            {
                var st = string.Empty;
                st += "<tr>";
                foreach(var it in item)
                {
                    if (!string.IsNullOrEmpty(st))
                    {
                        st += string.Format("<td><a href='{0}{1}'>{1}</a></td>", hrefhead, it);
                    }
                }
                st += "</tr>";
                str += st;
            }

            return Content(str);
        } 
    }
}
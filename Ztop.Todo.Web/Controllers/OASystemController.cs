using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class OASystemController : ControllerBase
    {
        // GET: OASystem
        public ActionResult Index()
        {
            var list = Core.OASystemManager.Get();
            ViewBag.List = list;
            return View();
        }

        public ActionResult CreateSystem(int id=0)
        {
            var system = Core.OASystemManager.Get(id);
            ViewBag.Model = system;
            return View();
        }

        [HttpPost]
        public ActionResult SaveSystem(OASystem sysetm)
        {
            var id = Core.OASystemManager.Save(sysetm);
            if (id > 0)
            {
                return SuccessJsonResult();
            }
            return ErrorJsonResult("保存失败");
        }


        public ActionResult CreatePower(int systemId)
        {
            ViewBag.SystemID = systemId;
            ViewBag.Dict = Core.UserManager.GetDict();
            return View();
        }

        [HttpPost]
        public ActionResult SavePower(Power power, int[] userId)
        {
            if (power != null && userId != null)
            {
                power.Items = userId.Select(e => new PowerItem { UserId = e }).ToList();
            }
            var id = Core.PowerManager.Add(power);
            if (id > 0)
            {
                return SuccessJsonResult();
            }

            return ErrorJsonResult("添加权限功能失败");
        }


        public ActionResult EditPower(int id)
        {
            var power = Core.PowerManager.Get(id);
            ViewBag.Dict = Core.UserManager.GetDict();
            return View(power);
        }

        [HttpPost]
        public ActionResult EditPower(Power power,int[] userId)
        {
            if (power != null && userId != null)
            {
                power.Items= userId.Select(e => new PowerItem { UserId = e }).ToList();
            }
            if (!Core.PowerManager.Edit(power))
            {
                return ErrorJsonResult("编辑失败,未找到相关编辑信息！");
            }
            return SuccessJsonResult();
        }

        public ActionResult DeletePower(int id)
        {
            if (!Core.PowerManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，未找到相关权限功能信息!");
            }

            return SuccessJsonResult();
        }

        public ActionResult ViewUser(int powerId)
        {
            var power = Core.PowerManager.Get(powerId);
            ViewBag.User = power.Items.Select(e => e.User).ToList();
            return View();
        }
    }

    
}
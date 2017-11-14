using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class PowerController : ControllerBase
    {
        // GET: Power
        public ActionResult Index()
        {
            return View();
        }

        private Power GetPower(int id)
        {
            var power = Core.PowerManager.Get(id);
            if (power != null)
            {
                var power_Item = power.Items.FirstOrDefault(e => e.UserId == Identity.UserID);
                if (power_Item != null)
                {
                    return power;
                    //ViewBag.Model = power;
                }
            }
            return null;
        }
     
        public ActionResult Get(int id,string Parameter,PowerType type=PowerType.Address,string name=null)
        {
            ViewBag.Model= GetPower(id);
            ViewBag.Type = type;
            ViewBag.Name = name;
            ViewBag.Parameter = Parameter;
            return View();
        }

        public ActionResult GetPartialView(int id,string viewName,object Parameters)
        {
            var power= GetPower(id);
            if (power == null)
            {
                return null;
            }
            return PartialView(viewName,Parameters);
        }
    }
}
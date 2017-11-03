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

     
        public ActionResult Get(int id,string Parameter,PowerType type=PowerType.Address,string name=null)
        {
            var power = Core.PowerManager.Get(id);
            if (power != null)
            {
                var power_Item = power.Items.FirstOrDefault(e => e.UserId == Identity.UserID);
                if (power_Item != null)
                {
                    ViewBag.Model = power;
                }
            }
            ViewBag.Type = type;
            ViewBag.Name = name;
            ViewBag.Parameter = Parameter;
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    [UserAuthorize]
    public class CityController : ControllerBase
    {
        // GET: City
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Select()
        {
            var provinces = Core.CityManager.Search(new CityParameter { Rank = Rank.Province });
            ViewBag.Provinces = provinces;
            return View();
        }

        public ActionResult Get(Rank rank,int CityID)
        {
            var citys = Core.CityManager.Search(new CityParameter { Rank = rank, PCID = CityID });
            return Json(citys, JsonRequestBehavior.AllowGet);
        }
        
    }
}
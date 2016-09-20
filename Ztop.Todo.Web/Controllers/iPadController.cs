using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class iPadController : ControllerBase
    {
        // GET: iPad
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(string title)
        {
            return SuccessJsonResult();
        }
    }
}
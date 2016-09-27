using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class iPadController : ControllerBase
    {
        // GET: iPad
        public ActionResult Index()
        {
            ViewBag.List = Core.iPadManager.Get();
            return View();
        }

        public ActionResult Create(int id=0,bool edit=false)
        {
            ViewBag.Edit = edit;
            ViewBag.iPad = Core.iPadManager.Get(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(iPad ipad,bool edit)
        {
            try
            {
                var id = Core.iPadManager.Add(ipad,edit);
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Core.iPadManager.Delete(id);
            }catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }


        public ActionResult CreateInvoice()
        {
            ViewBag.List = Core.iPadManager.Get().Where(e => !e.IID.HasValue).ToList();
            return View();
        }
        

        public ActionResult CreateContract()
        {
            return View();
        }


    }
}
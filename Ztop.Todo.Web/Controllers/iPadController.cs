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
            ViewBag.Registers = Core.iPad_registerManager.Get();
            
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
            ViewBag.List = Core.iPadManager.Get().Where(e => e.Statue == iPadStatue.Vacant).ToList();
            return View();
        }

        public ActionResult CreateRegister(int id=0)
        {
            ViewBag.Register = Core.iPad_registerManager.Get(id);
            ViewBag.List = Core.iPadManager.Get().Where(e => e.Statue == iPadStatue.Vacant).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegister(iPadRegister register,int[] ipads)
        {
            if (!Core.iPadManager.CheckUse(ipads))
            {
                return ErrorJsonResult("未读取到平板信息或者平板不可借用，请重试");
            }
            try
            {
                var rid=Core.iPad_registerManager.Save(register);
                if (!Core.iPadManager.Update(ipads, iPadStatue.Borrow))
                {
                    return ErrorJsonResult("更改平板状态失败，请检查iPad使用状态");
                }
                Core.Register_iPadManager.Add(ipads, rid);
            }catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
            return SuccessJsonResult();
        }


    }
}
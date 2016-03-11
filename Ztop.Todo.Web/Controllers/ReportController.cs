using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class ReportController : ControllerBase
    {
        public ActionResult Index()
        {
            SheetQueryParameter parameter = new SheetQueryParameter
            {
                Name = Identity.Name
            };
            var list = Core.SheetManager.GetSheets(parameter);
            ViewBag.OutList = list.Where(e => e.Status == Status.OutLine).ToList();
            ViewBag.ExaminingList = list.Where(e => e.Status == Status.Examining).ToList();
            ViewBag.ExaminList = list.Where(e => e.Status == Status.Examine).ToList();
            return View();
        }
        public ActionResult Create(int id=0)
        {
            var model = Core.SheetManager.GetModel(id) ?? new Model.Sheet()
            {
                Name = Identity.Name
            };
            if (model.NumberExt == 0)
            {
                model.NumberExt = Core.SheetManager.GetNumberExt(model.Number);
            }
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Save(Sheet sheet)
        {
            sheet.Substances = Core.SubstanceManager.Get(HttpContext);
            Core.SheetManager.Save(sheet);
            //try
            //{
               
            //}
            //catch(Exception ex)
            //{
            //    throw new HttpException("保存报销单发生错误：" + ex.Message);
            //}
            
            return SuccessJsonResult();
        }

        public ActionResult List()
        {
            return View();
        }

    }
}
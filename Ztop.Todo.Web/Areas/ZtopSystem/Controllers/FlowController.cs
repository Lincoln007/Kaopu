using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.ZtopSystem.Controllers
{
    public class FlowController : ZtopSystemControllerBase
    {

        public ActionResult Index()
        {
            var list = Core.FlowwManager.GetList();
            ViewBag.List = list;
            return View();
        }


        public ActionResult Create(int id = 0)
        {
            var flow = Core.FlowwManager.Get(id);
            ViewBag.Flow = flow;
            return View();
        }

        [HttpPost]
        public ActionResult Save(Floww flow)
        {
            if (flow.ID > 0)
            {
                if (!Core.FlowwManager.Edit(flow))
                {
                    return ErrorJsonResult("编辑流程失败！");
                }
            }
            else
            {
                var id = Core.FlowwManager.Add(flow);
                if (id <= 0)
                {
                    return ErrorJsonResult("创建流程失败！");
                }
            }
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            if (!Core.FlowwManager.Delete(id))
            {
                return ErrorJsonResult("删除流程失败！");
            }
            return SuccessJsonResult();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.ZtopSystem.Controllers
{
    public class FlowNodeController : ZtopSystemControllerBase
    {
        // GET: ZtopSystem/FlowNode
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id=0,int flowId = 0)
        {
            var flowNode = Core.FlowwNodeManager.Get(id);
            ViewBag.FlowNode = flowNode;
            if (flowNode != null)
            {
                flowId = flowNode.FlowwId;
            }
            var flow = Core.FlowwManager.Get(flowId);
            ViewBag.Flow = flow;
            ViewBag.Users = Core.UserManager.GetAllUser2();
            return View();
        }

        [HttpPost]
        public ActionResult Save(FlowwNode node)
        {
            if (node.ID > 0)
            {
                if (!Core.FlowwNodeManager.Edit(node))
                {
                    return ErrorJsonResult("编辑流程节点失败，未找到相关节点信息");
                }
            }
            else
            {
                var id = Core.FlowwNodeManager.Add(node);
                if (id <= 0)
                {
                    return ErrorJsonResult("添加流程节点信息失败！");
                }
            }
            return SuccessJsonResult();
        }


        public ActionResult Prev(int id)
        {
            if (!Core.FlowwNodeManager.Prev(id))
            {
                return ErrorJsonResult("上移失败！");
            }
            return SuccessJsonResult();
        }

        public ActionResult Later(int id)
        {
            if (!Core.FlowwNodeManager.Later(id))
            {
                return ErrorJsonResult("下移失败！");
            }
            return SuccessJsonResult();
        }

        public ActionResult Delete(int id)
        {
            if (!Core.FlowwNodeManager.Delete(id))
            {
                return ErrorJsonResult("删除流程节点信息!");
            } 
            return SuccessJsonResult();
        }
    }
}
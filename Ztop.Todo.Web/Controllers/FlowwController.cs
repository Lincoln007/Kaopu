using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class FlowwController : ControllerBase
    {
        // GET: Floww
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 第一次提交
        /// </summary>
        /// <param name="flowDataId"></param>
        /// <returns></returns>
        public ActionResult Post(int flowDataId)
        {
            var flowwData = Core.FlowwDataManager.Get(flowDataId);
            if (flowwData == null)
            {
                return ErrorJsonResult("未获取相关审核信息！");
            }
            var first = flowwData.Floww.GetFirstNode();
            if (first == null)
            {
                return ErrorJsonResult("未配置相关审核节点信息");
            }
            var flowwNodeData = new FlowwNodeData
            {
                FlowwDataId = flowDataId,
                FlowwNodeId = first.ID,
                PostUserId=Identity.UserID
            };
            var id = Core.FlowwNodeDataManager.Save(flowwNodeData);
            if (id <= 0)
            {
                return ErrorJsonResult("保存审核节点信息失败！");
            }
            if (!Core.FlowwDataManager.ChangeState(flowDataId, FlowwDataState.Checking))
            {
                return ErrorJsonResult("更改审核信息状态失败！");
            }
            return SuccessJsonResult();
        }

        public ActionResult RollBack(int flowwNodeDataId)
        {
            var flowwNodeData = Core.FlowwNodeDataManager.Get(flowwNodeDataId);

            return SuccessJsonResult();
        }
        
    }
}
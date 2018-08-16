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



        public ActionResult Detail(int id,int ownerId,string url)
        {
            var flowData = Core.FlowwDataManager.Get(id);
            ViewBag.FlowData = flowData;
            ViewBag.OwnerId = ownerId;
            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// 第一次提交审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Commit(int id)
        {
            var flowData = Core.FlowwDataManager.Get(id);
            if (flowData == null)
            {
                return ErrorJsonResult("未获取相关审核流程信息！");
            }
            var current = flowData.GetCurrentNode();
            if (current != null&&current.State==FlowwNodeState.Checking)
            {
                return ErrorJsonResult("当前流程正在审核，请勿重复提交！");
            }
            var first = flowData.Floww.GetFirstNode();
            if (first == null)
            {
                return ErrorJsonResult("当前审核流程未配置，请勿提交审核！");
            }
            var flowNodeData = new FlowwNodeData
            {
                FlowwDataId = id,
                FlowwNodeId = first.ID,
                PostUserId = Identity.UserID
            };
            var nodeId = Core.FlowwNodeDataManager.Save(flowNodeData);
            if (nodeId <= 0)
            {
                return ErrorJsonResult("保存审核流程信息失败！");
            }
            if (!Core.FlowwDataManager.ChangeState(id, FlowwDataState.Checking))
            {
                return ErrorJsonResult("更改审核流程信息状态失败！");
            }
            return SuccessJsonResult();
        }









        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="flowwNodeDataId"></param>
        /// <returns></returns>
        public ActionResult RollBack(int flowwNodeDataId)
        {
            var flowwNodeData = Core.FlowwNodeDataManager.Get(flowwNodeDataId);
            if (flowwNodeData == null)
            {
                return ErrorJsonResult("撤回参数信息错误，请刷新！");
            }
            if (flowwNodeData.FlowwData.Floww.CanBack == false)
            {
                return ErrorJsonResult("当前系统管理员已经设置为不能撤回操作，请刷新！");
            }
            if (flowwNodeData.PostUserId != Identity.UserID)
            {
                return ErrorJsonResult("当前您无权进行撤回操作！");
            }
            if (flowwNodeData.State != FlowwNodeState.Checking)
            {
                return ErrorJsonResult("当前流程已经审核完成，无法进行撤回操作！");
            }
            flowwNodeData.CheckTime = DateTime.Now;
            flowwNodeData.State = FlowwNodeState.Roll;
            flowwNodeData.UserId = Identity.UserID;
            flowwNodeData.Content = "提交人撤回";
            if (!Core.FlowwNodeDataManager.Edit(flowwNodeData))
            {
                return ErrorJsonResult("变更信息失败！");
            }
            if (!Core.FlowwDataManager.ChangeState(flowwNodeData.FlowwDataId, FlowwDataState.None))
            {
                return ErrorJsonResult("更改审核流程状态失败!");
            }
            return SuccessJsonResult();
        }

        /// <summary>
        /// 用户审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(int id,FlowwNodeState state,string content)
        {
            var flowNodeData = Core.FlowwNodeDataManager.Get(id);
            if (flowNodeData == null)
            {
                return ErrorJsonResult("未找到审核流程节点信息！");
            }
            if (flowNodeData.State != FlowwNodeState.Checking)
            {
                return ErrorJsonResult("当前审核节点已经完成审核，请勿重复操作！");
            }
            if (flowNodeData.FlowwNode.UserIds.Any(e => e == Identity.UserID)==false)
            {
                return ErrorJsonResult("您当前无权审核,如有异议，请咨询管理员");
            }
            if (flowNodeData.FlowwNode.IsCheckGroup)
            {
                if (Identity.GroupId != flowNodeData.PostUser.GroupID)
                {
                    return ErrorJsonResult("您当前审核的是其他组的信息,请告知系统管理员！");
                }
            }
            flowNodeData.CheckTime = DateTime.Now;
            flowNodeData.State = state;
            flowNodeData.UserId = Identity.UserID;
            flowNodeData.Content = content;
            if (!Core.FlowwNodeDataManager.Edit(flowNodeData))
            {
                return ErrorJsonResult("录入审核信息失败！");
            }
            var next = state == FlowwNodeState.Success ?
                Core.FlowwNodeManager.GetNext(flowNodeData.FlowwNodeId) :
                Core.FlowwNodeManager.GetPrev(flowNodeData.FlowwNodeId);
            if (next == null)
            {
                if (!Core.FlowwDataManager.ChangeState(flowNodeData.FlowwDataId, state == FlowwNodeState.Success ? FlowwDataState.Done : FlowwDataState.None))
                {
                    return ErrorJsonResult("设置审批流程信息失败！");
                }
            }
            else
            {
                var newId = Core.FlowwNodeDataManager.Save(new FlowwNodeData { FlowwDataId = flowNodeData.FlowwDataId, FlowwNodeId = next.ID, PostUserId = Identity.UserID });
                if (newId <= 0)
                {
                    return ErrorJsonResult("创建下一个审核节点失败！");
                }
            }



            return SuccessJsonResult();
        }
        
    }
}
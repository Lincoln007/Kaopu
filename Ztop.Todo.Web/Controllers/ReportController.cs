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
                Name = Identity.Name,
                Deleted=false
            };
            var list = Core.SheetManager.GetSheets(parameter);
            ViewBag.OutList = list.Where(e => e.Status == Status.OutLine).ToList();//草稿
            ViewBag.ExaminingList = list.Where(e => e.Status == Status.Examining||e.Status==Status.Post).ToList();
            ViewBag.ExaminList = list.Where(e => e.Status == Status.Examined).ToList();
            
            return View();
        }

        /// <summary>
        /// 填写新的报销单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.SerialNumber = Core.SerialNumberManager.GetNewModel();//获取唯一单据编号
            return View();
        }

        private void Save(Sheet sheet,string DirectorVal)
        {
            if (!string.IsNullOrEmpty(DirectorVal))
            {
                sheet.Status = Status.Post;
            }
            Core.SheetManager.Save(sheet);
            var verify = new Verify//创建审核步骤记录表
            {
                Step = Step.Create,
                SID = sheet.ID,
                Name = Identity.Name,
                Time=DateTime.Now,
                IsCheck = sheet.Status == Status.Post ? true : false//当报销单处于审核状态，那么确认用户提交了
            };
            Core.VerifyManager.Update(verify);
            if (!string.IsNullOrEmpty(DirectorVal))//做了 提交申请  需要等主管确认
            {
                Core.VerifyManager.Save(new Verify
                {
                    Step = Step.Confirm,
                    SID = sheet.ID,
                    Name = DirectorVal
                });
            }
        }

        /// <summary>
        /// 填写报销单初期（保存报销单 但是不提交上级）
        /// </summary>
        /// <param name="serialNumber">单据编号 信息</param>
        /// <param name="sheet">报销单内容</param>
        /// <param name="snid">单据编号ID</param>
        /// <param name="DirectorVal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(SerialNumber serialNumber,Sheet sheet,int snid,string DirectorVal)
        {
            if (snid == 0)
            {
                throw new ArgumentException("参数错误，未找到单据编号信息");
            }
            sheet.Substances = Core.SubstanceManager.Get(HttpContext);
            Save(sheet, DirectorVal);

            serialNumber.ID = snid;
            serialNumber.SID = sheet.ID;
            Core.SerialNumberManager.Update(serialNumber);
            return SuccessJsonResult();
        }

        [HttpPost]
        public ActionResult PostSheet(int id, string DirectorVal)
        {
            var sheet = Core.SheetManager.GetModel(id);
            if (sheet != null)
            {
                Save(sheet, DirectorVal);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没有找到报销单");
        }

        /// <summary>
        /// 查看报销单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.Sheet = Core.SheetManager.GetAllModel(id);
            return View();
        }

        /// <summary>
        /// 删除报销单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var model = Core.SheetManager.GetModel(id);
            if (model != null)
            {
                if (model.Name != Identity.Name)
                {
                    throw new HttpException(401, "您没有权限删除该报销单");
                }
                Core.SheetManager.Delete(id);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没有找到该报销单");
            
        }

        /// <summary>
        /// 主管审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Check1(int id)
        {
            var model = Core.SheetManager.GetModel(id);
            if (model != null)
            {
                model.Status = Status.Examining;//将报销单切换到正在审核中
                Core.SheetManager.Save(model);
                //var verify = new Verify
                //{
                //    Name = Identity.Name,
                //    SID = model.ID,
                //    Step=Step.Examine,
                //    IsCheck=true
                //};
                //Core.VerifyManager.Save(verify);
                return SuccessJsonResult();
            }
            throw new ArgumentException("参数错误，没有找打该报销单");
        }

        public ActionResult List()
        {

            return View();
        }


    }
}
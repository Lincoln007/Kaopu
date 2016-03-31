﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class ReportController : ControllerBase
    {
        protected static List<string> Directors = XmlHelper.GetDirectors();
        public ActionResult Index()
        {
            SheetQueryParameter parameter = new SheetQueryParameter
            {
                Name = Identity.Name,
                Deleted=false
            };
            var list = Core.SheetManager.GetSheets(parameter);
            ViewBag.OutList = list.Where(e => e.Status == Status.OutLine).ToList();//草稿
            //未完成审核    提交 主管审核  申屠审核  财务审核  退回
            ViewBag.ExaminingList = list.Where(e => e.Status == Status.ExaminingDirector || e.Status == Status.ExaminingFinance || e.Status == Status.ExaminingManager||e.Status==Status.RollBack).ToList();
            //  我提交的报销单  同时也
            ViewBag.ExaminList = list.Where(e => e.Status == Status.Examined).ToList();
            ViewBag.RollBackList = list.Where(e => e.Status == Status.RollBack).ToList();
            if (Directors.Contains(Identity.Name))
            {
                ViewBag.WaitForMe = Core.SheetManager.GetSheets(new SheetQueryParameter { Deleted = false, Controler = Identity.Name }).Where(e => e.Status != Status.Examined && e.Status != Status.OutLine).ToList();
            }
            
            return View();
        }

        /// <summary>
        /// 填写新的报销单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int id=0)
        {
            ViewBag.Sheet = Core.SheetManager.GetSerialNumberModel(id);
            ViewBag.SerialNumber = Core.SerialNumberManager.GetNewModel();//获取唯一单据编号
            return View();
        }

        private void Save(Sheet sheet,string DirectorVal)
        {
            if (!string.IsNullOrEmpty(DirectorVal))
            {
                sheet.Status = Status.ExaminingDirector;
                sheet.Controler = DirectorVal;
            }
            else
            {
                sheet.Status = Status.OutLine;
                sheet.Controler = Identity.Name;
            }
            Core.SheetManager.Save(sheet);
            var verify = new Verify//创建审核步骤记录表
            {
                Step = Step.Create,
                SID = sheet.ID,
                Name = Identity.Name,
                Time=DateTime.Now,
                Position=sheet.Status==Status.ExaminingDirector?Position.Check:Position.Wait
                //IsCheck = sheet.Status == Status.ExaminingDirector ? true : false//当报销单处于审核状态，那么确认用户提交了
            };
            Core.VerifyManager.Update(verify);
            if (!string.IsNullOrEmpty(DirectorVal))//做了 提交申请  需要等主管确认
            {
                Core.VerifyManager.Save(new Verify
                {
                    Step = Step.Examine,
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
            sheet.Substances = Core.SubstanceManager.Get(HttpContext);//获取详细清单
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
        private void Reversion(Sheet sheet,string reason)
        {
            if (sheet.Controler != Identity.Name)
            {
                throw new ArgumentException("当前您无法进行退出操作");
            }
            var sverify = new Verify
            {
                Name = Identity.Name,
                SID = sheet.ID,
                Time = DateTime.Now,
                Position = Position.RollBack,
                Reason=reason
            };
            switch (sheet.Status)
            {
                case Status.ExaminingDirector:
                    sverify.Step = Step.Examine;
                    break;
                case Status.ExaminingManager:
                    sverify.Step = Step.Confirm;
                    break;
                case Status.ExaminingFinance:
                    sverify.Step = Step.Approved;
                    break;
                default:break;
            }
            sheet.Controler = sheet.Name;
            sheet.Status = Status.RollBack;
            //sheet.Status = Status.OutLine;
            Core.SheetManager.Save(sheet);
            Core.VerifyManager.Update(sverify);
        }

        private void Check(Sheet sheet)
        {
            if (sheet.Controler != Identity.Name)
            {
                throw new ArgumentException("当前您无法进行审核操作");
            }
            var sverify = new Verify
            {
                Name = Identity.Name,
                SID = sheet.ID,
                Time = DateTime.Now,
                Position=Position.Check
                //IsCheck = true
            };
            var nVerify = new Verify
            {
                SID = sheet.ID
            };
            switch (sheet.Status)
            {
                case Status.ExaminingDirector://主管审核通过
                    sheet.Status = Status.ExaminingManager;
                    sheet.Controler = XmlHelper.GetManager();
                    sverify.Step = Step.Examine;
                    nVerify.Name = sheet.Controler;
                    nVerify.Step = Step.Confirm;
                    break;
                case Status.ExaminingManager://申屠审核通过
                    sheet.Status = Status.ExaminingFinance;
                    sheet.Controler = XmlHelper.GetFinance();
                    sverify.Step = Step.Confirm;
                    nVerify.Name = sheet.Controler;
                    nVerify.Step = Step.Approved;
                    break;
                case Status.ExaminingFinance://财务审核通过
                    sheet.Status = Status.Examined;
                    sheet.Controler = Identity.Name;
                    sverify.Step = Step.Approved;

                    break;
                default:
                    break;
            }
            Core.VerifyManager.Update(sverify);
            if (sheet.Status != Status.Examined)
            {
                Core.VerifyManager.Save(nVerify);
            }
            Core.SheetManager.Save(sheet);
        }
        public ActionResult Check(int id)
        {
            var sheet = Core.SheetManager.GetModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数错误，未找到相关报销单");
            }
            Check(sheet);
            return SuccessJsonResult();
        }
        [HttpPost]
        public ActionResult Reversion(int id,string Reason)
        {
            var sheet = Core.SheetManager.GetModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数不正确，未找到相关报销单信息");
            }
            Reversion(sheet, Reason);
            return SuccessJsonResult();
        } 

        public ActionResult List()
        {

            return View();
        }


    }
}
using System;
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
        //protected static List<string> Directors = XmlHelper.GetDirectors();
        public ActionResult Index()
        {
            SheetQueryParameter parameter = new SheetQueryParameter
            {
                Name = Identity.Name,
                Deleted=false
            };
            var list = Core.SheetManager.GetSheets(parameter).OrderByDescending(e=>e.Time);
            ViewBag.OutList = list.Where(e => e.Status == Status.OutLine).Take(10).ToList();//草稿
            //未完成审核    提交 主管审核  申屠审核  财务审核  退回
            ViewBag.ExaminingList = list.Where(e => e.Status == Status.ExaminingDirector || e.Status == Status.ExaminingFinance || e.Status == Status.ExaminingManager||e.Status==Status.RollBack||e.Status==Status.Filing).Take(10).ToList();
            //  我提交的报销单  同时也
            ViewBag.ExaminList = list.Where(e => e.Status == Status.Examined).Take(10).ToList();
            ViewBag.RollBackList = list.Where(e => e.Status == Status.RollBack).Take(10).ToList();
            if(Identity.Director||Identity.Name== "靳小阳")
            {
                ViewBag.WaitForMe = Core.SheetManager.GetSheets(new SheetQueryParameter { Deleted = false, Controler = Identity.Name }).Where(e => e.Status != Status.Examined && e.Status != Status.OutLine).ToList();
                ViewBag.Checks = Core.VerifyManager.GetSheetByVerify(Identity.Name);
            }
            return View();
        }
        /// <summary>
        /// 填写新的报销单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(SheetType type=SheetType.Daily,int id=0)
        {
            ViewBag.Sheet = Core.SheetManager.GetSheet(id,type,Identity.Name);
            ViewBag.Groups = Core.UserManager.GetUserGroups();
            ViewBag.Users = Core.UserManager.GetAllUsers();
            return View();
        }
        private void Save(Sheet sheet,string DirectorVal)
        {
            sheet.Checkers = DirectorVal;
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
            if (sheet.Status == Status.ExaminingDirector)
            {
                var verify = new Verify//创建审核步骤记录表
                {
                    Step = Step.Create,
                    SID = sheet.ID,
                    Name = Identity.Name,
                    Time = DateTime.Now,
                    Position = Position.Check
                };
                Core.VerifyManager.Save(verify);
            }
            if (!string.IsNullOrEmpty(DirectorVal))
            {
                Core.NotificationManager.Add(sheet);
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
        public ActionResult Save(Sheet sheet,string DirectorVal,Evection evection,string[] busType,string lines,double CarPetty=.0,Driver driver=Driver.无)
        {
            double sum = .0;
            if (sheet.Type == SheetType.Daily)//日常报销
            {
                sheet.Substances = Core.SubstanceManager.GetSubstances(HttpContext);//获取详细清单
                sum = sheet.Substances.Sum(e => e.Price);
            }
            else//出差报销
            {
                if (string.IsNullOrEmpty(evection.Persons))
                {
                    throw new ArgumentException("出差报销，出差人员不能为空！");
                }
                if (string.IsNullOrEmpty(evection.Way))
                {
                    throw new ArgumentException("出差交通方式未选择！");
                }
                evection.Errands = Core.SubstanceManager.GetErrands(HttpContext,lines);
                evection.TCosts = Core.SubstanceManager.GetTraffic(HttpContext, busType,driver,CarPetty);
                sheet.Evection = evection;
                sum = evection.Traffic + evection.SubSidy + evection.Hotel + evection.Other;
            }
            if (Math.Abs(sheet.Money - sum) > 0.001)
            {
                throw new ArgumentException("报销金额合计与清单合计不符，请核对金额！");
            }

            Save(sheet, DirectorVal);

            //serialNumber.ID = snid;
            //serialNumber.SID = sheet.ID;
            //Core.SerialNumberManager.Update(serialNumber);
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
            ViewBag.Detail = true;
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
        public ActionResult Revoke(int id)
        {
            var sheet = Core.SheetManager.GetAllModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数不正确，无法获取报销单信息");
            }
            if (sheet.Controler != sheet.Checkers)//
            {
                throw new ArgumentException("当前报销单主管已经审核，无法执行撤回操作！");
            }
            var sverify = new Verify
            {
                Name = Identity.Name,
                SID = sheet.ID,
                Time = DateTime.Now,
                Position = Position.RollBack,
                Step=Step.Roll
            };
            sheet.Controler = Identity.Name;
            sheet.Status = Status.RollBack;
            Core.SheetManager.Save(sheet);
            Core.VerifyManager.Save(sverify);
            return SuccessJsonResult();
        }
        /// <summary>
        /// 审核人  点击退回
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="reason"></param>
        private void Reversion(Sheet sheet,string reason)
        {
            if (sheet.Controler != Identity.Name)
            {
                throw new ArgumentException("当前您无法进行退回操作");
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
            Core.SheetManager.Save(sheet);
            Core.VerifyManager.Save(sverify);
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
            };
            switch (sheet.Status)
            {
                case Status.ExaminingDirector://主管审核通过
                    sheet.Status = Status.ExaminingManager;
                    sheet.Controler = XmlHelper.GetManager();
                    sverify.Step = Step.Examine;
                    break;
                case Status.ExaminingManager://申屠审核通过
                    sheet.Status = Status.ExaminingFinance;
                    sheet.Controler = XmlHelper.GetFinance();
                    sverify.Step = Step.Confirm;
                    break;
                case Status.ExaminingFinance://财务审核通过  财务审核通过之后  需要生成归档编号并且生成条形码
                    sheet.Status = Status.Filing;
                    sheet.Controler = XmlHelper.GetManager();
                    sheet.CheckTime = DateTime.Now;
                    sverify.Step = Step.Approved;
                    break;
                case Status.Filing:
                    sheet.Status = Status.Examined;
                    sheet.Controler = sheet.Name;
                    sverify.Step = Step.Filing;
                    break;
                default:
                    break;
            }
            Core.VerifyManager.Save(sverify);
            Core.SheetManager.Save(sheet);
        }
        public ActionResult Check(int id)
        {
            var sheet = Core.SheetManager.GetAllModel(id);
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
        public ActionResult List(string Creater="我", string Custom = null, string Position="不限", string Checker = "我", string Checker2 = null, string CurrentTime="不限",Order order=Order.Time,int page=1,int rows=10)
        {
            var queryParameter = new QueryParameter
            {
                Creater = (Operator)Enum.Parse(typeof(Operator), Creater, true),
                Custom = Custom,
                Status = (StatusPosition)Enum.Parse(typeof(StatusPosition), Position, true),
                Checker = (Operator)Enum.Parse(typeof(Operator), Checker, true),
                Checker2 = Checker2,
                Time = (Time)Enum.Parse(typeof(Time), CurrentTime, true),
                Order = order,
                Page = new PageParameter(page, rows)
            };
            ViewBag.List = Core.SheetManager.GetSheets(queryParameter, Identity.Name);
            ViewBag.Parameter = queryParameter;
            ViewBag.Page = queryParameter.Page;
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Statistic()
        {
            ViewBag.Users = Core.UserManager.GetAllUsers().Select(e => e.RealName).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Statistic(string name,DateTime startTime,DateTime endTime)
        {
            var list = Core.SheetManager.Statistic(name, startTime, endTime);
            if (list.Count == 0)
            {
                throw new ArgumentException("未找到相关报销数据");
            }
            return Json(list);
        }
        [HttpPost]
        public ActionResult Statistic2(string name,Category category,DateTime startTime,DateTime endTime)
        {
            var dict = Core.SheetManager.Statistic(name, category, startTime, endTime);
            if (dict.Count == 0)
            {
                throw new ArgumentException("未找到相关报销信息");
            }
            return Json(dict);
        }


        /// <summary>
        /// 等待我审核的报销单
        /// </summary>
        /// <returns></returns>
        public ActionResult Sheeting(StatusPosition position)
        {
            if (position == StatusPosition.已审核)
            {
                ViewBag.List = Core.SheetManager.GetSheets(Identity.Name);
            }else if (position == StatusPosition.未审核)
            {
                ViewBag.List = Core.SheetManager.GetSheets(new SheetQueryParameter { Deleted = false, Controler = Identity.Name }).Where(e => e.Status != Status.Examined && e.Status != Status.OutLine).ToList();
            }
            else
            {
                throw new ArgumentException("参数有误！");
            }
            return View();
        }

        public ActionResult Filing()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Filing(int id)
        {
            var sheet = Core.SheetManager.GetAllModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数错误，未找到相关报销单");
            }
            Check(sheet);
            return SuccessJsonResult();
        }

        [HttpGet]
        public ActionResult Search(string key)
        {
            var List = Core.SheetManager.GetSheetsByKey(key);
            return Json(List,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Collect(int year,int month)
        {
            ViewBag.Sheets = Core.SheetManager.GetSheets(year, month);
            return View();
        }

        public ActionResult Review(string Coding=null,string Time=null,double? MinMoney=null,double? MaxMoney=null,string Creater=null,Order order=Order.Time,int page=1)
        {
            var parameter = new SheetVerifyParameter()
            {
                Page = new PageParameter(page, 20),
                Coding = Coding,
                Time = Time,
                MinMoney = MinMoney,
                MaxMoney = MaxMoney,
                Creater = Creater,
                Order = order,
                Checker = Identity.Name
            };
            ViewBag.List = Core.VerifyManager.GetSheetByVerify(parameter);
            ViewBag.Parameter = parameter;
            return View();
        }

    }
}
﻿using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Common;
using Ztop.Todo.Manager;
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
            var list = Core.SheetManager.GetSheets(parameter).OrderByDescending(e=>e.Time).ToList();
            ViewBag.List = list;
            ViewBag.RollBackList = list.Where(e => e.Status == Status.RollBack).Take(10).ToList();
            return View();
        }

        public ActionResult Director()
        {
            if (Identity.Director || Identity.Name == "靳小阳")
            {
                return View();
            }
            throw new ArgumentException("您没有权限查看当前页面");
        }

        public ActionResult Wait()
        {
            ViewBag.WaitForMe = Core.SheetManager.GetSheets(new SheetQueryParameter { Deleted = false, Controler = Identity.Name }).Where(e => e.Status != Status.Examined && e.Status != Status.OutLine && e.Status != Status.RollBack && e.Status != Status.Cash&&e.Status!=Status.Repeal).ToList();
            return View();
        }

        public ActionResult CheckList()
        {
            ViewBag.List = Core.VerifyViewManager.Search(new SheetVerifyParameter() { Checker = Identity.Name, Order = Order.Time, Page = new PageParameter(1, 20) });
            return View();
        }
        public ActionResult WaitForCheck()
        {
            var list = Core.SheetManager.GetSheets(new SheetQueryParameter { Name = Identity.Name, Controler = Identity.Name, Deleted = false, Status = Status.Auditing });
            ViewBag.List = list;
            return View();
        }

        public ActionResult CheckTime(int id)
        {
            var model = Core.SheetManager.GetModel(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult CheckTime(int id,DateTime time,Cost cost)
        {
            if (!Core.SheetManager.CheckTranfer(id, time,cost))
            {
                return ErrorJsonResult("确认到账时间失败，未找到转账单或者当前报销单非转账单");
            }
            return SuccessJsonResult();
        }


        public ActionResult UnCheck(int id)
        {
            var model = Core.SheetManager.GetModel(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult UnCheck(int id,string reason)
        {
            if (!Core.SheetManager.UnCheck(id, reason))
            {
                return ErrorJsonResult("作废失败，未找到转账单或者当前报销单为非转账单");
            }
            return SuccessJsonResult();
        }


        public ActionResult Expenses()
        {
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
            var types = Core.Report_TypeManager.Get();
            types = Core.Report_TypeManager.GetChildren(types);
            ViewBag.Types = types;
            return View();
        }
        private void Save(Sheet sheet,string DirectorVal)
        {
            sheet.Checkers = DirectorVal;
            if (!string.IsNullOrEmpty(DirectorVal))
            {
                sheet.Status = Status.ExaminingDirector;
                sheet.Controler = sheet.Type==SheetType.Transfer?XmlHelper.GetManager() :DirectorVal;
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
        public ActionResult Save(
            Sheet sheet,string DirectorVal,
            Evection evection,string[] busType,string lines,double[] CarPetty,Driver[] driver,
            int[] rid, int[] srid,string[] detail,double[] price,bool[] payWay,
            Reception reception,string[] content,double[] Coin,PayWay[] Way,double[] Average,string[] Memo
            )
        {
            double sum = .0;
            if (sheet.Type == SheetType.Errand)//出差报销
            {
                if (string.IsNullOrEmpty(evection.Persons))
                {
                    return ErrorJsonResult("出差报销，出差人员不能为空！");
                }
                evection.Errands = Core.SubstanceManager.GetErrands(HttpContext, lines);
                evection.TCosts = Core.SubstanceManager.GetTraffic(HttpContext, busType, driver, CarPetty);
                sheet.Evection = evection;
                sum = evection.Traffic + evection.SubSidy + evection.Hotel + evection.Other;
            }else if (sheet.Type == SheetType.Reception)//日常招待
            {
                var items = Core.ReceptionManager.GainItems(content, Coin, Way, Memo);
                if (reception == null||items==null||items.Count==0)
                {
                    return ErrorJsonResult("未获取相关日常招待基础信息");
                }
                reception.Items = items;
                sum= items.Where(e => e.Way == PayWay.Cash).Sum(e => e.Coin);
                sheet.Money = sum;
                sheet.Reception = reception;
            }
            else//日常报销  转账支出
            {
                sheet.Substances = Core.SubstanceManager.GetSubstances(rid, srid,detail,price,payWay);//获取详细清单
                sum = sheet.Substances.Where(e=>e.EnterprisePay==false).Sum(e => e.Price);
                sheet.Money = sum;
            }
            if (Math.Abs(sheet.Money - sum) > 0.001)
            {
                return ErrorJsonResult("报销金额合计与清单合计不符，请核对金额！");
            }

            Save(sheet, DirectorVal);
            return SuccessJsonResult();
        }
        [HttpPost]
        public ActionResult PostSheet(int id, string DirectorVal)
        {
            var sheet = Core.SheetManager.GetModel(id);
            if (sheet != null)
            {
                //sheet.Reception = null;
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
        public ActionResult Detail(int id,InfoType? infoType=null,int ?verifyid=null)
        {
            var model = Core.SheetManager.GetFull(id);
  
            if (model.Type != SheetType.Transfer)
            {
                if (model.Status == Status.Filing || model.Status == Status.Examined)
                {
                    if (!model.CheckExt.HasValue && model.CheckTime.HasValue)
                    {
                        model.CheckExt = Core.SheetManager.GetCheckExt(model.CheckTime.Value);
                        Core.SheetManager.SaveSheet(model);
                    }
                }
            }
          
            ViewBag.Sheet = model;
            ViewBag.Detail = true;
            if (infoType.HasValue)
            {
                if (infoType == InfoType.Sheet)
                {
                    Core.NotificationManager.FlagSheetRead(model.ID, Identity.UserID);
                }else if (infoType == InfoType.Verify&&verifyid.HasValue)
                {
                    Core.NotificationManager.FlagVerifyRead(verifyid.Value, Identity.UserID);
                }
            }
            return View();
        }
        /// <summary>
        /// 作用：查看类似金额报销单
        /// 作者：汪建龙
        /// 编写时间：2017年1月13日15:35:05
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailSimilar(int id)
        {
            if (id > 0)
            {
                ViewBag.Similar = Core.SheetManager.GetSimilars(id);
            }
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
                if (model.Name == Identity.Name||(model.Checkers==Identity.Name&&model.Type==SheetType.Transfer))
                {
                    Core.SheetManager.Delete(id);
                    return SuccessJsonResult();
                }
                throw new HttpException(401, "您没有权限删除该报销单");
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
        private void Reversion(Sheet sheet,string reason,string person,Step? step=null)
        {
            var sverify = new Verify
            {
                Name = Identity.Name,
                SID = sheet.ID,
                Time = DateTime.Now,
                Position = Position.RollBack,
                Reason=reason,
            };
            if (step.HasValue)
            {
                sverify.Step = step.Value;
            }
            else
            {
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
                    default: break;
                }
            }
           
            sheet.Controler = person;
            if (person.ToUpper() == sheet.Name.ToUpper())
            {
                sheet.Status = Status.RollBack;
            }
            else
            {
                sheet.Status = sheet.Status - 1;
            }
            Core.SheetManager.Save(sheet);
            Core.VerifyManager.Save(sverify);
            Core.NotificationManager.Add(sverify);
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
                    sheet.Status = sheet.Type==SheetType.Transfer?Status.Auditing: Status.ExaminingManager;
                    sheet.Controler = sheet.Type==SheetType.Transfer?sheet.Name: XmlHelper.GetManager();
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
                    sheet.CheckExt = Core.SheetManager.GetCheckExt(sheet.CheckTime.Value);
                    sverify.Step = Step.Approved;
                    break;
                case Status.Filing://报销归档之后，进入现金核算
                    sheet.Status = Status.Cash;
                    sheet.Controler = XmlHelper.GetFinance();
                    sverify.Step = Step.Filing;
                    break;
                default:
                    break;
            }
            Core.SheetManager.Save(sheet);
            Core.VerifyManager.Save(sverify);
            if (sheet.Status != Status.Filing&&sheet.Status!=Status.Examined)
            {
                Core.NotificationManager.Add(sverify);
            }
           
          
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
        public ActionResult Reversion(int id,string Reason,string person)
        {
            var sheet = Core.SheetManager.GetModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数不正确，未找到相关报销单信息");
            }
            if (sheet.Controler != Identity.Name)
            {
                throw new ArgumentException("当前您无法进行退回操作");
            }
            Reversion(sheet, Reason,person);
            return SuccessJsonResult();
        } 


        public ActionResult CancelCheck(int id)
        {
            var model = Core.SheetManager.GetAllModel(id);
            if (model == null)
            {
                throw new ArgumentException("未找到相关报销单，或已经删除");
            }
            if (model.Status == Status.ExaminingFinance)
            {
                Reversion(model, "主动撤销通过", Identity.Name,Step.Confirm);
                return RedirectToAction("Detail", new { id = id });
            }
            else
            {
                throw new ArgumentException("该报销单已经被上级审核通过！无法进行取消通过操作");
            }
            
        }
        public ActionResult List(string Creater="我", string Custom = null, string Position="不限", string Checker = "我", string Checker2 = null, string CurrentTime="不限",Order order=Order.Time,int page=1,int rows=20,double? minMoney=null,double? maxMoney=null)
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
                MinMoney=minMoney,
                MaxMoney=maxMoney,
                Page = new PageParameter(page, rows)
            };
            ViewBag.List = Core.SheetManager.GetSheets(queryParameter, Identity.Name);
            ViewBag.Parameter = queryParameter;
            ViewBag.Page = queryParameter.Page;
            return View();
        }

        public ActionResult AdvanceSearch(
            string name=null,DateTime? startTime=null,DateTime? endTime=null,
            double? minMoney=null,double?maxMoney=null,SheetType? type=null,
            int?RID=null,string content=null,Order order=Order.Time,
            Cost? Cost=null,int page=1,int rows=20
            )
        {
            var parameter = new SheetParameter
            {
                Name = name,
                StartTime = startTime,
                EndTime = endTime,
                minMoney = minMoney,
                maxMoney = maxMoney,
                Type = type,
                Order = order,
                Deleted = false,
                Page = new PageParameter(page, rows)
            };
            if (parameter.Type.HasValue)
            {
                switch (parameter.Type.Value)
                {
                    case SheetType.Daily:
                        parameter.RID = RID;
                        break;
                    case SheetType.Errand:
                        parameter.Content = content;
                        break;
                    case SheetType.Transfer:
                        parameter.Cost = Cost;
                        break;
                }
            }
            var list = Core.SheetViewManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            var users = Core.UserManager.GetAllUsers();
            var reportType = Core.Report_TypeManager.Get();
            ViewBag.Users = users;
            ViewBag.ReportType = reportType;
            return View();
        }
        public ActionResult DownloadSheets(string name = null, DateTime? startTime = null, DateTime? endTime = null,
        double? minMoney = null, double? maxMoney = null, SheetType? type = null,
        int? RID = null, string content = null, Order order = Order.Time,
        Cost? Cost = null)
        {
            var parameter = new SheetParameter
            {
                Name = name,
                StartTime = startTime,
                EndTime = endTime,
                minMoney = minMoney,
                maxMoney = maxMoney,
                Type = type,
                Order = order,
                Deleted = false
            };
            if (parameter.Type.HasValue)
            {
                switch (parameter.Type.Value)
                {
                    case SheetType.Daily:
                        parameter.RID = RID;
                        break;
                    case SheetType.Errand:
                        parameter.Content = content;
                        break;
                    case SheetType.Transfer:
                        parameter.Cost = Cost;
                        break;
                }
            }
            var dict = Core.SheetViewManager.Download(parameter);

            return View();
        }
        public ActionResult Statistic()
        {
            ViewBag.Users = Core.UserManager.GetAllUsers().Select(e => e.RealName).ToList();
            return View();
        }



        public ActionResult Daily()
        {
            var complete = Core.SheetManager.GetCompletes().Where(e=>e.Type==SheetType.Daily).ToList();
            var substances = Core.SubstanceManager.GetSubstances(complete);
            ViewBag.Dict = substances.GroupBy(e => e.Category).ToDictionary(e => e.Key, e => e.Sum(k => k.Price));
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

        /// <summary>
        /// 作用：查看某年某月的报销情况
        /// 作者：汪建龙
        /// 备注时间：2016年11月19日10:09:40
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult Collect(int year,int month)
        {
            ViewBag.Sheets = Core.SheetManager.GetSheets(year, month);
            return View();
        }

        /// <summary>
        /// 作用：只查看除了转账支出的报销单
        /// </summary>
        /// <param name="Coding"></param>
        /// <param name="CheckKey"></param>
        /// <param name="Time"></param>
        /// <param name="MinMoney"></param>
        /// <param name="MaxMoney"></param>
        /// <param name="Creater"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="sheetType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ActionResult Review(
            string Coding=null,string CheckKey=null,
            string Time=null,
            double? MinMoney=null,double? MaxMoney=null,
            string Creater=null,Order order=Order.Time,int page=1,string sheetType=null,string content=null)
        {
            var parameter = new SheetVerifyParameter()
            {
                Page = new PageParameter(page, 20),
                Coding = Coding,
                CheckKey = CheckKey,
                Time = Time,
                MinMoney = MinMoney,
                MaxMoney = MaxMoney,
                Creater = Creater,
                Order = order,
                Checker = Identity.Name,
                Content = content
            };
            if (!string.IsNullOrEmpty(sheetType))
            {
                parameter.SheetType = EnumHelper.GetEnum<SheetType>(sheetType);
            }
            ViewBag.List = Core.VerifyViewManager.Search(parameter);
            ViewBag.Parameter = parameter;
            return View();
        }

        /// <summary>
        /// 作用：显示转账支出的报销单
        /// </summary>
        /// <param name="coding"></param>
        /// <param name="checkkey"></param>
        /// <param name="time"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="creater"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult Bank(
            string coding=null,string checkkey=null,
            string time=null,double? minMoney=null,double? maxMoney=null,
            string creater=null,Order order=Order.Time,int ?year=null,int? month=null,
            int page=1,int rows=20)
        {
            var parameter = new SheetVerifyParameter
            {
                Coding = coding,
                CheckKey = checkkey,
                Time = time,
                Year=year,
                Month=month,
                MinMoney = minMoney,
                MaxMoney = maxMoney,
                Creater = creater,
                Order = order,
                Checker = Identity.Name,
                //Page = new PageParameter(page, rows)
            };
            var stringKey = Identity.UserID + ParameterManager.ParameterKey;
            SessionHelper.SetSession(stringKey, parameter);
            //RedisManager.Set(, parameter, RedisManager.Client);
            parameter.Page = new PageParameter(page, rows);
            var list= Core.VerifyViewManager.Search(parameter, true);

            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }


        public ActionResult DownloadExcel(
            string name=null,string coding=null,
            string checkKey=null,string time=null,
            double? minMoney=null,double? maxmoney=null,
            string creater=null,Order order = Order.Time,
            string sheetType=null,string content=null,string month=null)
        {
            var parameter = new SheetVerifyParameter()
            {
                Coding = coding,
                Time = time,
                MinMoney = minMoney,
                MaxMoney = maxmoney,
                Creater = creater,
                Order = order,
                Checker = name,
                Content = content,
                CheckKey=checkKey
            };
            if (!string.IsNullOrEmpty(content))
            {
                var reporttype = Core.Report_TypeManager.Get(content);
                if (reporttype != null)
                {
                    parameter.RID = reporttype.ID;
                }
               
            }
            if (!string.IsNullOrEmpty(month))
            {
                var array = month.Replace("年", ";").Replace("月", ";").Split(';');
                var starttime= Convert.ToDateTime(string.Format("{0}-{1}-01 00:00:00", array[0], array[1]));
                parameter.Year = int.Parse(array[0]);
                parameter.Month = int.Parse(array[1]);
                //parameter.StartTime = starttime;
                //parameter.EndTime = starttime.AddMonths(1);
            }
           
            if (!string.IsNullOrEmpty(sheetType))
            {
                parameter.SheetType = EnumHelper.GetEnum<SheetType>(sheetType);
            }
            //var list = Core.VerifyViewManager.Search(parameter);
            var list = Core.VerifyManager.GetSheetByVerify(parameter);
            IWorkbook workbook = Core.VerifyManager.GetWorkbook(list);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", DateTime.Now.ToLongDateString()+".xls");
        }

        public ActionResult CashCheck(string name=null)
        {
            var parameter = new SheetQueryParameter
            {
                Status = Status.Cash
            };
            var list= Core.SheetManager.GetSheets(parameter);
            var users = list.GroupBy(e => e.Name).Select(e => e.Key).ToList();
            ViewBag.Users = users;
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(e => e.Name.ToLower() == name.ToLower()).ToList();
            }
            else
            {
                list = list.OrderByDescending(e => e.Name).ToList();
            }
            ViewBag.Name = name;
            ViewBag.List = list;
   
            return View();
        }

        [HttpPost]
        public ActionResult PayCash(int[] sid)
        {
            if (sid == null)
            {
                return ErrorJsonResult("未获取勾选的报销单信息");
            }
            var sum = Core.SheetManager.Pay(sid);
            return sum > 0 ? SuccessJsonResult() : ErrorJsonResult("支付失败！请告知");
        }

        public ActionResult Transfer(int id = 0)
        {
            var sheet = Core.SheetManager.GetModel(id);
            ViewBag.Sheet = sheet;
            return View();
        }

        /// <summary>
        /// 作用：报销系统的
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {

            return View();
        }

    }
}
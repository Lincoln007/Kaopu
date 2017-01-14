using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class BankController : ControllerBase
    {
        /// <summary>
        /// 作用：查看银行对账清单列表以及报销统计情况
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:35:42
        /// </summary>
        /// <returns></returns>
        // GET: Bank
        public ActionResult Index()
        {
            var heads = Core.Bill_OneManager.GetAllHeads();
            ViewBag.Evaluations = heads.Where(e => e.Company == Company.Evaluation).ToList();
            ViewBag.Projections = heads.Where(e => e.Company == Company.Projection).ToList();
            ViewBag.Projection2s = heads.Where(e => e.Company == Company.Projection2).ToList();
            ViewBag.Sheets = Core.SheetManager.Collect();
            return View();
        }

        public ActionResult Create(Company company)
        {
            ViewBag.Company = company;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month-1;
            if (month == 1)
            {
                year--;
                month = 12;
            }
            else
            {
                month--;
            }
            var PreBank = Core.BankManager.GetBank(year, month, company);
            ViewBag.PreBank = PreBank;
            return View();
        }

        [HttpPost]
        public ActionResult Save(int year,int month,Company company,DateTime[] Time,double[] Income,double[] Pay,double[] Balance, string[] Account,string[] Summary,Cost[] Cost,Category[] Category,bool Edit=false)
        {
            if (Core.BillManager.Exist(year, month, company)&&!Edit)
            {
                return ErrorJsonResult(string.Format("系统中已经存在{0}年{1}月{2}公司的银行对账清单，对有需要更改请前往编辑！", year, month, company.GetDescription()));
            }
            var bank = Core.BillManager.GetBank(year, month, company);
            var bills = Core.BillManager.GetBills(bank.ID, Time, Income, Pay, Balance, Account, Summary, Cost, Category);
            //var bills = Core.BillManager.GetBills(bank.ID, Coding, Time, Budget, Money, Account, Summary,Cost,Category);
            try
            {
                Core.BillManager.UpDateBills(bills, bank.ID);
            }
            catch(Exception ex)
            {
                return ErrorJsonResult(ex.ToString());
            }
           
            return SuccessJsonResult(bank);
        }

        public ActionResult SaveOne(int year,int month,Company company,DateTime time,double income,double pay,double balance,string account,string summary,Cost cost,Category category)
        {
            var bank = Core.BillManager.GetBank(year, month, company);
            var id = Core.BillManager.Save(new Bill()
            {
                Time = time,
                Money = income > 0 ? income : pay,
                Account = account,
                Balance = balance,
                //Budget = income > 0 ? Budget.Income : Budget.Expense,
                Summary = summary,
                Remark = summary,
                //Cost = cost,
                //Category = category,
                BID = bank.ID
            });
            return SuccessJsonResult();
        }

        public ActionResult Detail(int id,bool edit=false)
        {
            ViewBag.Bank = Core.BillManager.GetBank(id);
            ViewBag.EditFlag = edit;
            return View();
        }

        public ActionResult Collect(int year,int month)
        {
            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Evaluation = Core.BillManager.GetAllModelBank(year, month, Company.Evaluation);
            ViewBag.Projection = Core.BillManager.GetAllModelBank(year, month, Company.Projection);
            ViewBag.Cash = Core.SheetManager.Sum(year, month);
            return View();
        }

        public ActionResult Download(int year,int month,Company company)
        {
            IWorkbook workbook = Core.BillManager.GetWorkbook(year, month, company);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", string.Format("杭州智拓{0}咨询有限公司{1}年{2}月银行对账单.xls", company == Company.Evaluation ? "房地产土地评估" : "土地规划设计", year, month));
        }

        [HttpPost]
        public ActionResult Upload(int year,int month,Company company)
        {
            if (Core.BillManager.Exist(year, month, company))
            {
                throw new ArgumentException(string.Format("系统中已经存在{0}年{1}月{2}公司的银行对账清单，对有需要更改请前往编辑！", year, month, company.GetDescription()));
            }
            var bank = Core.BillManager.GetBank(year, month, company);
            var file = HttpContext.Request.Files[0];
            var saveFullFilePath = Core.BillManager.Upload(file);
            var errors = new List<string>();
            var bills = BillClass.Analyze(saveFullFilePath, bank.ID,ref errors);
            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join("\r\n", errors.ToArray()));
            }
            try
            {
                Core.BillManager.UpDateBills(bills, bank.ID);
            }catch(Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
            return RedirectToAction("Detail", new { id = bank.ID });
        }

        public ActionResult Gain(int year,int month,Company company)
        {
            if (month == 1)
            {
                year--;
                month = 12;
            }
            else
            {
                month--;
            }
            var bank = Core.BankManager.GetBank(year, month, company);
            return Json(bank, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetBalance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetBalance(int year,int month,double balance,Company company)
        {
            var currrent = Core.BankManager.GetBank(year, month, company);
            if (currrent != null)
            {
                if (currrent.Balance > 0)
                {
                    return ErrorJsonResult(string.Format("当前系统中查询到{0}的{1}年{2}月的账户余额不为零，无法进行设置", company.GetDescription(), year, month));
                }
            }
            Core.BankManager.Save(new Bank { Year = year, Month = month, Company = company, Balance = balance });
            return SuccessJsonResult();
        }

        #region
        public ActionResult InputFile()
        {
            return View();
        }
        /// <summary>
        /// 作用：导入规划银行对账单
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日10:39:31
        /// </summary>
        /// <returns></returns>
        public ActionResult InputProjection(Company company=Company.Projection)
        {
            ViewBag.Company = company;
            return View();
        }

        /// <summary>
        /// 作用：导入评估银行对账单
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日10:39:54
        /// </summary>
        /// <returns></returns>
        public ActionResult InputEvaluation()
        {
            return View();
        }
        public ActionResult Check(int year,int month,Company company)
        {
            var bank = Core.BillManager.Get(year, month, company)??new Bank();
            return Json(bank, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作用：用户上传评估银行明细Excel文件，并读取分析Excel文件中的记录
        /// 作者：汪建龙
        /// 编写时间：2016年11月13日14:56:083
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(int year, int month, Company company)
        {
            var file = HttpContext.Request.Files[0];
            if (file == null)
            {
                throw new ArgumentException("请上传Excel文件");
            }

            var saveFullFilePath = Core.BillManager.Upload(file);
            var errors = new List<string>();
            var bills = BillClass.AnalyzeExcel(saveFullFilePath, ref errors);
            Session["Read"] = bills;
            ViewBag.Values = bills.ToJson();
            ViewBag.Bank = new Bank { Year = year, Month = month, Company = company };
            ViewBag.Bills = bills;
            ViewBag.Errors = errors;
            return View();
        }
        /// <summary>
        /// 作用：用户规划公司银行对账Excel文件，并读取分析Excel文件中的数据  返回读取的数据界面
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日13:17:51
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input2(int year,int month,Company company)
        {
            var file = HttpContext.Request.Files[0];
            if (file == null)
            {
                throw new ArgumentException("请上传Excel文件！");
            }
            var saveFullFilePath = Core.BillManager.Upload(file);
            var errors = new List<string>();
            var bills = BillClass.AnalyzeExcel2(saveFullFilePath, ref errors);
            Session["Read"] = bills;
            //ViewBag.Values = bills.ToJson();
            ViewBag.Bank = new Bank { Year = year, Month = month, Company = company };
            ViewBag.Bills = bills;
            ViewBag.Errors = errors;
            return View();
        }

        [HttpPost]
        public ActionResult SaveInput2(int year,int month,Company company)
        {
            var bills = Session["Read"] as List<BillTwo>;
            if (bills == null || bills.Count == 0)
            {
                return ErrorJsonResult("未获取银行对账信息！原因：读取的银行对账信息失效，请重新上传文件！");
            }
            var head = Core.Bill_OneManager.GetBillHead(year, month, company);

            var hid = head != null ? head.ID : Core.Bill_OneManager.Add(new Bill_Head { Year = year, Month = month, Company = company });
            var errors = Core.Bill_OneManager.Input2(hid, bills, year, month,company);
            if (errors.Count > 0)
            {
                return ErrorJsonResult(string.Join("<br />", errors));
            }
            return SuccessJsonResult(hid);
        }

        /// <summary>
        /// 作用：用户在上传Excel文件之后，对读取到的账单进行保存到数据库中
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:28:07 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="company"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInput(int year,int month,Company company,bool edit=false)
        {
            var bills = Session["Read"] as List<BillOne>;

            if (bills == null || bills.Count == 0)
            {
                return ErrorJsonResult("未读取银行对账信息！原因：读取的银行对账信息失效，请重新上传文件！");
            }
            var head = Core.Bill_OneManager.GetBillHead(year, month, company);
            
            var hid = head != null ? head.ID : Core.Bill_OneManager.Add(new Bill_Head { Year = year, Month = month, Company = company });

            var errors = Core.Bill_OneManager.Input(hid, bills,year,month,company);
            if (errors.Count > 0)
            {
                return ErrorJsonResult(string.Join("-", errors));
            }
            return SuccessJsonResult(hid);

        }
        /// <summary>
        /// 作用：查看某年某月的银行对账单 -------------- 作废
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:28:43
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailBill(int id,Company company=Company.Evaluation)
        {
            var bill_heads = Core.Bill_OneManager.GetHead(id);
            ViewBag.Heads = bill_heads;
            ViewBag.Company = company;
            return View();
        }
        /// <summary>
        /// 作用：查看评估银行对账情况
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日16:50:36
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewEvaluation(int id,bool depend=true)
        {
            var head = Core.Bill_OneManager.GetHead(id);
            if (head != null)
            {
                var list = Core.Bill_Records_ViewManager.GetByHID(head.ID);
                //var list = Core.Bill_RecordManager.GetByHID(head.ID);
                ViewBag.List = list;
                ViewBag.PN = Core.Bill_OneManager.GetNearBy(head.Year, head.Month,head.Company);
            }
            ViewBag.Head = head;
           
            ViewBag.Depend = depend;
            return View();
        }

        /// <summary>
        /// 作用：查看规划银行对账情况
        /// 作者：汪建龙
        /// 编写时间：2016年12月11日16:50:25
        /// </summary>
        /// <param name="id"></param>
        /// <param name="depend">是否独立页面</param>
        /// <returns></returns>
        public ActionResult ViewProjection(int id,bool depend = true)
        {
            var head = Core.Bill_OneManager.GetHead(id);
            ViewBag.Head = head;
            if (head != null)
            {
                var list = Core.Bill_Records_ViewManager.GetByHID(head.ID);
                //var list = Core.Bill_RecordManager.GetByHID(head.ID);
                ViewBag.List = list;
                ViewBag.PN = Core.Bill_OneManager.GetNearBy(head.Year, head.Month,head.Company);
            }
            ViewBag.Depend = depend;
            return View();
        }

        public ActionResult CollectBills(List<BillRecordView> list)
        {
            var dict = Core.Bill_RecordManager.Collect(list);
            ViewBag.Dict = dict;
            return View();
        }
        /// <summary>
        /// 作用：公司备注
        /// 作者：汪建龙
        /// 编写时间：2017年1月3日13:49:09
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remark(int id,Company company)
        {
            var billRecord = Core.Bill_RecordManager.Get(id);
            ViewBag.Record = billRecord;
            ViewBag.Company = company;
            return View();
        }

        [HttpPost]
        public ActionResult Remark(int id,string remark)
        {
            var entry = Core.Bill_RecordManager.Remark(id, remark);
            if (entry == null)
            {
                return ErrorJsonResult("备注失败！");
            }
            return SuccessJsonResult();
        }

        /// <summary>
        /// 作用：对某一笔账单进行归类
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:29:14
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Classify(int id,Company company)
        {
            var billRecord = Core.Bill_RecordManager.Get(id);
            var list = Core.Report_TypeManager.Get();
            ViewBag.ReportType = list;
            ViewBag.Record = billRecord;
            ViewBag.Company = company;
            return View();
        }
        /// <summary>
        /// 作用：在对账单归类之后，进行POST保存
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:29:46 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cost"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Classify(int id,Cost cost,int ?rid)
        {
            if (cost != Cost.RealPay)
            {
                rid = null;
            }
            var entry = Core.Bill_RecordManager.Classify(id, cost, rid);
            if (entry == null)
            {
                return ErrorJsonResult("归类失败");
            }
            return SuccessJsonResult(entry.HID);
        }

        /// <summary>
        /// 作用：查看某年某月的报销情况
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日10:30:53
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult CollectSheet(int year,int month)
        {
            var sheets = Core.SheetManager.GetSheets(year, month);
            ViewBag.Sheets = sheets;
            //Session["Sheets"] = sheets;
            var users = Core.UserManager.GetAllUsers();
            ViewBag.Users = users.Select(e => e.RealName).ToList();
            int[] pre = month == 1 ? new int[] { year - 1, 12 } : new int[] { year, month - 1 };
            int[] next = month == 12 ? new int[] { year + 1, 1 } : new int[] { year, month + 1 };
            int[][] PN = null;
            if (year == 2016 && month == 8)
            {
                PN = new int[][] { null, next };

            }else if (year == DateTime.Now.Year && month == DateTime.Now.Month)
            {
                PN = new int[][] { pre, null };
            }
            else
            {
                PN = new int[][] { pre, next };
            }
            ViewBag.PN = PN;
            return View();
        }

        /// <summary>
        /// 作用：统计各个类别报销情况
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日11:23:27
        /// </summary>
        /// <param name="sheets"></param>
        /// <returns></returns>
        public ActionResult CollectCategory(List<Sheet> sheets)
        {
            var dailys = sheets.Where(e => e.Type == SheetType.Daily).ToList();
            var SV = new List<SubstancsView>();
            foreach (var item in dailys)
            {
                if (item.Substancs_Views != null)
                {
                    SV.AddRange(item.Substancs_Views);
                }
               
            }
            var dict = new Dictionary<string, double>();
            var categorys = SV.GroupBy(e => e.Name).Select(e => e.Key).ToList();
            foreach(var name in categorys)
            {
                var sum = SV.Where(e => e.Name.ToLower() == name).Sum(e => e.Price);
                if (!dict.ContainsKey(name))
                {
                    dict.Add(name, sum);
                }
            }
            var errands= sheets.Where(e => e.Type == SheetType.Errand).ToList();
            dict.Add("差旅费", errands.Sum(e => e.Money));
            ViewBag.Dict = dict;
            ViewBag.Sheets = sheets;
           
            return View();
        }

        /// <summary>
        /// 作用：
        /// 作者：汪建龙
        /// 编写时间
        /// </summary>
        /// <param name="sheets"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult CategoryCollect(string category)
        {
            var sheets = Session["Sheets"] as List<Sheet>;
            ViewBag.Category = category;
            if (category == "出差报销")
            {
                sheets = sheets.Where(e => e.Type == SheetType.Errand).ToList();
            }
            else
            {
                try
                {
                    Category cate = EnumHelper.GetEnum<Category>(category);
                    sheets = sheets.Where(e => e.Type == SheetType.Daily&&e.Substances.Select(k=>k.Category).Contains(cate)).ToList();
                    ViewBag.Cate = cate;
                }
                catch(Exception ex)
                {
                    throw new ArgumentException(ex.ToString());
                }
             
            }
            sheets = sheets.OrderBy(e => e.Name).ThenByDescending(e => e.Money).ToList();
            ViewBag.Sheets = sheets;
            return View();
        }
        /// <summary>
        /// 作用：统计用户的报销金额
        /// 作者：汪建龙
        /// 编写时间：2016年11月19日11:37:24
        /// </summary>
        /// <param name="sheets"></param>
        /// <returns></returns>
        public ActionResult CollectUser(List<Sheet> sheets)
        {
            var dict = sheets.GroupBy(e => e.Name).ToDictionary(e => e.Key, e => e.Sum(k => k.Money)).OrderByDescending(e=>e.Value).ToDictionary(e=>e.Key,e=>e.Value);
            ViewBag.Dict = dict;
            return View();
        }
        /// <summary>
        /// 作用：查看某一个人的报销单
        /// 作者：汪建龙
        /// 编写时间：2016年11月22日09:21:33
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult UserCollect(string name)
        {
            List<Sheet> sheets = Session["Sheets"] as List<Sheet>;
            if (sheets != null)
            {
                sheets = sheets.Where(e => e.Name.ToUpper() == name.ToUpper()).ToList();
            }
            ViewBag.Name = name;
            ViewBag.Sheets = sheets;
            return View();
        }

        public ActionResult AdvanceCollect(DateTime ?startTime,DateTime? endTime,string cate,string name)
        {
            var parameter = new SheetQueryParameter2
            {
                StartTime = startTime,
                EndTime = endTime,
                Category = cate,
                Name = name
            };
            var sheets = Core.SheetManager.GetSheets(parameter);
            ViewBag.Sheets = sheets;
            ViewBag.Parameter = parameter;
            var users = Core.UserManager.GetAllUsers();
            ViewBag.Users = users.Select(e => e.RealName).ToList();
            return View();
        }

        /// <summary>
        /// 作用：
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public ActionResult CollectChart(Dictionary<object,object> Dict)
        {
            ViewBag.Dict = Dict;
            return View();
        }
        #endregion
    }
}
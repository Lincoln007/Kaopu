using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class VerifyManager:ManagerBase
    {
      
        /// <summary>
        /// 更新 当前数据库中存在 那就更新 不存在添加
        /// </summary>
        /// <param name="verify"></param>
        public void Update(Verify verify)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Verifys.OrderByDescending(e=>e.ID).FirstOrDefault(e => e.Name == verify.Name && e.SID == verify.SID && e.Step == verify.Step);
                if (entry != null)
                {
                    verify.ID = entry.ID;
                    db.Entry(entry).CurrentValues.SetValues(verify);
                }
                else
                {
                    db.Verifys.Add(verify);
                }
                db.SaveChanges();
            }
        }


        public void Save(Verify verify)
        {
            using (var db = GetDbContext())
            {
                db.Verifys.Add(verify);
                db.SaveChanges();
            }
        }
        public List<int> GetSheetID(string name)
        {
            using (var db = GetDbContext())
            {
                return db.Verifys.Where(e => e.Name == name).GroupBy(e => e.SID).Select(g => g.Key).ToList();
            }
        }

       
        public List<Sheet> GetSheetByVerify(string name,DateTime? startTime=null,DateTime? endTime=null,int?year=null,int?month=null)
        {
            using (var db = GetDbContext())
            {
                var list = new List<Sheet>();
                var query = db.Verifys.Where(e => e.Position == Position.Check && e.Name == name&&e.Step!=Step.Create).AsQueryable();
                if (query.Count() > 0)
                {
                    var result = query.GroupBy(e => e.SID).Select(e => e.Key).ToList();
                    var sheets = (from t in db.Sheets
                    where result.Contains(t.ID)
                    select t).ToList();

                    var query2 = sheets.AsQueryable();
                    if (startTime.HasValue)
                    {
                        query2 = query2.Where(e => e.Time >= startTime.Value);
                    }

                    if (endTime.HasValue)
                    {
                        query2 = query2.Where(e => e.Time <= endTime.Value);
                    }
                    if (year.HasValue)
                    {
                        query2 = query2.Where(e => e.Time.Year == year.Value);
                    }
                    if (month.HasValue)
                    {
                        query2 = query2.Where(e => e.Time.Month == month.Value);
                    }
                    sheets = query2.ToList();
                    foreach(var sheet in sheets)
                    {
                        if (sheet != null)
                        {
                            if (sheet.Type == SheetType.Daily)
                            {
                                sheet.Substances = db.Substances.Where(e => e.SID == sheet.ID).OrderBy(e => e.ID).ToList();
                                sheet.Substancs_Views = db.Substancs_View.Where(e => e.SID == sheet.ID).OrderBy(e => e.ID).ToList();
                            }
                            else
                            {
                                sheet.Evection = db.Evections.FirstOrDefault(e => e.SID == sheet.ID);
                                if (sheet.Evection != null)
                                {
                                    sheet.Evection.Errands = db.Errands.Where(e => e.EID == sheet.Evection.ID).ToList();
                                    sheet.Evection.TCosts = db.Traffics.Where(e => e.EID == sheet.Evection.ID).ToList();
                                }
                            }
                            list.Add(sheet);
                           
                        }
                    }
                }
                return list;
            }
        }

        public List<Sheet> GetSheetByVerify(SheetVerifyParameter parameter)
        {
            var checks = GetSheetByVerify(parameter.Checker,parameter.StartTime,parameter.EndTime,parameter.Year,parameter.Month);
            var query = checks.AsQueryable();
           
            if (!string.IsNullOrEmpty(parameter.Coding))
            {
                query = query.Where(e => e.PrintNumber.Contains(parameter.Coding));
            }

            if (!string.IsNullOrEmpty(parameter.CheckKey))
            {
                query = query.Where(e => e.CheckNumber.Contains(parameter.CheckKey));
            }

            if (parameter.MinMoney.HasValue&& parameter.MinMoney.Value > 0)
            {
                query = query.Where(e => e.Money >= parameter.MinMoney.Value);
            }
            if (parameter.MaxMoney.HasValue&& parameter.MaxMoney.Value > 0)
            {
                query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Creater))
            {
                query = query.Where(e => e.Name.Contains(parameter.Creater));
            }
            if (!string.IsNullOrEmpty(parameter.Time))
            {
                var currentTime = DateTime.Now;
                switch (parameter.Time)
                {
                    case "一周内":
                        currentTime = currentTime.AddDays(7);
                        break;
                    case "一个月内":
                        currentTime = currentTime.AddMonths(1);
                        break;
                    case "半年内":
                        currentTime = currentTime.AddMonths(6);
                        break;
                    case "一年内":
                        currentTime = currentTime.AddYears(1);
                        break;
                }
                query = query.Where(e => e.Time <= currentTime);
            }
           
            if (parameter.SheetType.HasValue)
            {
                query = query.Where(e => e.Type == parameter.SheetType.Value);
                if (!string.IsNullOrEmpty(parameter.Content))
                {
                    if (parameter.SheetType.Value == SheetType.Daily)
                    {
                        if (parameter.RID.HasValue)
                        {
                            query = query.Where(e => e.Substances.Any(k => k.RID == parameter.RID.Value));
                        }
                        //if (parameter.SRID.HasValue)
                        //{
                        //    query = query.Where(e => e.Substances.Any(k => k.SRID == parameter.SRID.Value));
                        //}

                        //try
                        //{
                        //    var category = EnumHelper.GetEnum<Category>(parameter.Content);
                        //    query = query.Where(e => e.Substances.Any(k => k.Category == category));
                        //}
                        //catch
                        //{

                        //}
                    }
                    else if (parameter.SheetType.Value == SheetType.Errand)
                    {
                        query = query.Where(e => (!string.IsNullOrEmpty(e.Remarks)&&e.Remarks.Contains(parameter.Content)) || (!string.IsNullOrEmpty(e.Evection.Place)&& e.Evection.Place.Contains(parameter.Content)) || (!string.IsNullOrEmpty(e.Evection.Reason)&&e.Evection.Reason.Contains(parameter.Content)));
                    }
                }
            }
            switch (parameter.Order)
            {
                case Order.Time:
                    query = query.OrderByDescending(e => e.Time);
                    break;
                case Order.Money:
                    query = query.OrderByDescending(e => e.Money);
                    break;
                case Order.PrintNumber:
                    query = query.OrderByDescending(e => e.PrintNumber);
                    break;
                case Order.CheckNumber:
                    query = query.OrderByDescending(e => e.CheckNumber);
                    break;
            }
            query = query.SetPage(parameter.Page);
            return query.ToList();
        }
        private void WriteDialy(Sheet item,SubstancsView entry,IRow modelRow,int line,int serial,ref ISheet sheet)
        {
            var row = sheet.GetRow(line);
            if (row == null)
            {
                row = sheet.CreateRow(line);
            }
            var cell = ExcelClass.GetCell(row, 0, modelRow);
            cell.SetCellValue(serial);
            ExcelClass.GetCell(row, 1, modelRow).SetCellValue(item.PrintNumber);
            ExcelClass.GetCell(row, 2, modelRow).SetCellValue(item.CheckNumber);
            ExcelClass.GetCell(row, 3, modelRow).SetCellValue(item.Name);
            ExcelClass.GetCell(row, 4, modelRow).SetCellValue(item.Time.ToLongDateString());
            ExcelClass.GetCell(row, 5, modelRow).SetCellValue(item.Count);
            ExcelClass.GetCell(row, 6, modelRow).SetCellValue(item.Money);
            ExcelClass.GetCell(row, 7, modelRow).SetCellValue(item.Remarks);
            ExcelClass.GetCell(row, 8, modelRow).SetCellValue(item.Type.GetDescription());
            ExcelClass.GetCell(row, 9, modelRow).SetCellValue(item.Status.GetDescription());
            ExcelClass.GetCell(row, 10, modelRow).SetCellValue(item.Checkers);
            ExcelClass.GetCell(row, 11, modelRow).SetCellValue(item.CheckTime.HasValue ? item.CheckTime.Value.ToLongDateString() : "未审核");
            ExcelClass.GetCell(row, 12, modelRow).SetCellValue(string.Format("类别：{0}|内容：{1}|金额：{2}元", entry.Name, entry.Details, Math.Round(entry.Price, 2)));
        }
  
        private void WriteErrand(Errand item,IRow modelRow,int line,ref ISheet sheet)
        {
            var row = sheet.GetRow(line);
            if (row == null)
            {
                row = sheet.CreateRow(line);
            }
            var cell = ExcelClass.GetCell(row, 19, modelRow);
            cell.SetCellValue(item.Users);
            ExcelClass.GetCell(row, 20, modelRow).SetCellValue(string.Format("{0}-{1}", item.StartTime.ToShortDateString(), item.EndTime.ToShortDateString()));
            ExcelClass.GetCell(row, 21, modelRow).SetCellValue(item.Days);
            ExcelClass.GetCell(row, 22, modelRow).SetCellValue(item.Days * item.Peoples * 40);
        }
        private void WrirteTraffic(Traffic first,IRow modelRow,int line,ref ISheet sheet)
        {
            var row = sheet.GetRow(line);
            if (row == null)
            {
                row = sheet.CreateRow(line);
            }
            var cell = ExcelClass.GetCell(row, 23, modelRow);
            cell.SetCellValue(first.Type.GetDescription());
            ExcelClass.GetCell(row, 24, modelRow).SetCellValue(first.Cost);
            ExcelClass.GetCell(row, 25, modelRow).SetCellValue(first.KiloMeters);
            ExcelClass.GetCell(row, 26, modelRow).SetCellValue(first.Plate);
            ExcelClass.GetCell(row, 27, modelRow).SetCellValue(first.Toll);
            ExcelClass.GetCell(row, 28, modelRow).SetCellValue(first.Petrol);
            ExcelClass.GetCell(row, 29, modelRow).SetCellValue(first.Driver.GetDescription());
            ExcelClass.GetCell(row, 30, modelRow).SetCellValue(first.CarPetty);
        }
        private void WriteEvection(Sheet item,IRow modelRow, int serial,ref int line,ref ISheet sheet)
        {
            var row = sheet.GetRow(line);
            if (row == null)
            {
                row = sheet.CreateRow(line);
            }
            var eline = line;
            var tline = line;
            var cell = ExcelClass.GetCell(row, 0, modelRow);
            cell.SetCellValue(serial);
            ExcelClass.GetCell(row, 1, modelRow).SetCellValue(item.PrintNumber);
            ExcelClass.GetCell(row, 2, modelRow).SetCellValue(item.CheckNumber);
            ExcelClass.GetCell(row, 3, modelRow).SetCellValue(item.Name);
            ExcelClass.GetCell(row, 4, modelRow).SetCellValue(item.Time.ToLongDateString());
            ExcelClass.GetCell(row, 5, modelRow).SetCellValue(item.Count);
            ExcelClass.GetCell(row, 6, modelRow).SetCellValue(item.Money);
            ExcelClass.GetCell(row, 7, modelRow).SetCellValue(item.Remarks);
            ExcelClass.GetCell(row, 8, modelRow).SetCellValue(item.Type.GetDescription());
            ExcelClass.GetCell(row, 9, modelRow).SetCellValue(item.Status.GetDescription());
            ExcelClass.GetCell(row, 10, modelRow).SetCellValue(item.Checkers);
            ExcelClass.GetCell(row, 11, modelRow).SetCellValue(item.CheckTime.HasValue ? item.CheckTime.Value.ToLongDateString() : "未审核");
            ExcelClass.GetCell(row, 12, modelRow).SetCellValue("/");
            if (item.Evection != null)
            {
                ExcelClass.GetCell(row, 13, modelRow).SetCellValue(item.Evection.Place);
                ExcelClass.GetCell(row, 14, modelRow).SetCellValue(item.Evection.Reason);
                ExcelClass.GetCell(row, 15, modelRow).SetCellValue(item.Evection.Persons);
                ExcelClass.GetCell(row, 16, modelRow).SetCellValue(item.Evection.Hotel);
                ExcelClass.GetCell(row, 17, modelRow).SetCellValue(item.Evection.Mark);
                ExcelClass.GetCell(row, 18, modelRow).SetCellValue(item.Evection.Other);
                if (item.Evection.Errands != null && item.Evection.Errands.Count > 0)
                {
                    foreach (var errand in item.Evection.Errands)
                    {
                        WriteErrand(errand, modelRow, eline, ref sheet);
                    }
 
                }
                if (item.Evection.TCosts != null && item.Evection.TCosts.Count > 0)
                {   
                    foreach(var traffic in item.Evection.TCosts)
                    {
                        WrirteTraffic(traffic, modelRow, tline++, ref sheet);
                    }
                }
            }
            if (eline > tline)//出差人员多行
            {

            }
            else//交通多行
            {

            }
            line = eline > tline ? eline : tline;
         
        }

        public IWorkbook GetWorkbook(List<Sheet> list)
        {
            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["REPORT"].ToString()).OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    IRow row = sheet.GetRow(3);
                    if (row != null)
                    {
                        var modelRow = row;
                        var serial = 1;
                        var rowNumber = 3;
                        foreach(var item in list)
                        {
                            if (item.Type == SheetType.Daily && item.Substances != null)
                            {
                                foreach(var entry in item.Substancs_Views)
                                {
                                    WriteDialy(item, entry, modelRow, rowNumber++, serial++, ref sheet);
                                }
                                //foreach (var entry in item.Substances)
                                //{
                                //    WriteDialy(item, entry, modelRow, rowNumber++, serial++, ref sheet);
                                //}

                            }
                            else
                            {
                                var startrow = rowNumber;
                                WriteEvection(item, modelRow, serial++,ref rowNumber, ref sheet);
                                if (startrow != rowNumber)
                                {
                                    for (var i = 0; i < 19; i++)
                                    {
                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startrow, rowNumber - 1, i, i));
                                }
                                }
                            
                            }
                        }
                    }
                }
            }
            return workbook;
        }
        public Verify Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Verifys.FirstOrDefault(e => e.ID == id);
            }
        }
        
    }
}

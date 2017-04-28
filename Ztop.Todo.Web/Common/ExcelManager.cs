using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Common
{
    public static class ExcelManager
    {
        /// <summary>
        /// 作用：下载每个月汇总表格
        /// 作者：汪建龙
        /// 编写时间：2017年1月17日17:03:56
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="reportTypes"></param>
        /// <param name="sheets"></param>
        /// <param name="billList1"></param>
        /// <param name="billList2"></param>
        /// <returns></returns>
        public static IWorkbook DownloadCollect(int year,int month, List<ReportType> reportTypes, List<Sheet> sheets, List<BillRecordView> billList1,List<BillRecordView> billList2)
        {
            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["Collect"].ToString()).OpenExcel();
            if (workbook == null)
            {
                return null;
            }
            ISheet sheet1 = workbook.GetSheetAt(0);
            ISheet sheet2 = workbook.GetSheetAt(1);
            ISheet sheet3 = workbook.GetSheetAt(2);
            WriteCompany(string.Format("杭州智拓{0}年{1}月收支汇总（一）", year, month), reportTypes, billList1, ref sheet1);
            WriteCompany(string.Format("杭州智拓{0}年{1}月收支汇总（二）", year, month), reportTypes, billList2, ref sheet2);
            WriteReport(string.Format("杭州智拓{0}年{1}月收支汇总（三）", year, month), reportTypes, sheets, ref sheet3);
            return workbook;
        }
       
        /// <summary>
        /// 作用：返回项目洽谈信息表
        /// 作者：汪建龙
        /// 编写时间：2017年1月16日19:44:45
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IWorkbook DownloadArticleWorkbook(List<ArticleView> list)
        {
            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["Article"].ToString()).OpenExcel();
            if (workbook == null)
            {
                return null;
            }
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                return null;
            }

            var line = 1;
            IRow modelrow = sheet.GetRow(line);
            foreach(var item in list)
            {
                var row = sheet.GetRow(line);
                if (row == null)
                {
                    row = sheet.CreateRow(line);
                }

                line++;
                var cell = ExcelClass.GetCell(row, 0, modelrow);
                cell.SetCellValue(item.Name);
                ExcelClass.GetCell(row, 1, modelrow).SetCellValue(item.Number);
                ExcelClass.GetCell(row, 2, modelrow).SetCellValue(item.OtherSide);
                ExcelClass.GetCell(row, 3, modelrow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 4, modelrow).SetCellValue(item.CName);
                ExcelClass.GetCell(row, 5, modelrow).SetCellValue(item.PName);
            }
            return workbook;

        }

        /// <summary>
        /// 作用：下载合同统计表
        /// 作者：汪建龙
        /// 编写时间：2017年1月16日19:33:54
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IWorkbook DownContractWorkbook(List<Contract> list)
        {
            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["Contract"].ToString()).OpenExcel();
            if (workbook == null)
            {
                return null;
            }
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                return null;
            }
            var line = 1;
            IRow modelrow = sheet.GetRow(1);
            foreach(var item in list)
            {
                var row = sheet.GetRow(line);
                if (row == null)
                {
                    row = sheet.CreateRow(line);
                }
                line++;
                var cell = ExcelClass.GetCell(row, 0, modelrow);
                cell.SetCellValue(item.ID);
                ExcelClass.GetCell(row, 1, modelrow).SetCellValue(item.Number);
                ExcelClass.GetCell(row, 2, modelrow).SetCellValue(item.Name);
                ExcelClass.GetCell(row, 3, modelrow).SetCellValue(item.Company);
                ExcelClass.GetCell(row, 4, modelrow).SetCellValue(item.ZtopCompany.GetDescription());
                ExcelClass.GetCell(row, 5, modelrow).SetCellValue(item.StartTime.ToLongDateString());
                ExcelClass.GetCell(row, 6, modelrow).SetCellValue(item.EndTime.HasValue ? item.EndTime.Value.ToLongDateString() : "/");
                ExcelClass.GetCell(row, 7, modelrow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 8, modelrow).SetCellValue(item.PerformanceBond);
                ExcelClass.GetCell(row, 9, modelrow).SetCellValue(item.Invoices != null ? item.Invoices.Sum(e => e.Money):.0);
                ExcelClass.GetCell(row, 10, modelrow).SetCellValue(item.BillContracts != null ? item.BillContracts.Sum(e => e.Price) : .0);
            }
            return workbook;
        }
        public static byte[] Translate(IWorkbook workbook)
        {
            if (workbook == null)
            {
                return null;
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            return ms.ToArray();
        }

  

        /// <summary>
        /// 作用：写入日常报销汇总表
        /// 作者：汪建龙
        /// 编写时间：2017年1月17日16:56:19
        /// </summary>
        /// <param name="head"></param>
        /// <param name="reportTypes"></param>
        /// <param name="list"></param>
        /// <param name="sheet"></param>
        public static void WriteReport(string head,List<ReportType> reportTypes, List<Sheet> list,ref ISheet sheet)
        {
            if (list == null || reportTypes == null)
            {
                return;
            }
            var line = 0;
            IRow row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            ICell cell = row.GetCell(0) ?? row.CreateCell(0);
            cell.SetCellValue(head);
            line = 3;
            var modelrow = sheet.GetRow(line);
            var dailys = list.Where(e => e.Type == SheetType.Daily).ToList();
            var errands = list.Where(e => e.Type == SheetType.Errand).ToList();
            var substance_views = new List<SubstancsView>();
            foreach(var item in dailys)
            {
                if (item.Substancs_Views != null && item.Substancs_Views.Count > 0)
                {
                    substance_views.AddRange(item.Substancs_Views);
                }
            }
            List<SubstancsView> temp;
            foreach(var item in reportTypes)
            {
                temp = substance_views.Where(e => e.Name.ToLower() == item.Name.ToLower()).ToList();
                row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                line++;
                cell = ExcelClass.GetCell(row, 0, modelrow);
                cell.SetCellValue(item.Name);
                ExcelClass.GetCell(row, 1, modelrow).SetCellValue(temp.Sum(e => e.Price));
                ExcelClass.GetCell(row, 2, modelrow).SetCellValue(string.Join(";", temp.Select(e => e.Details).ToArray()));
            }
            row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            cell = ExcelClass.GetCell(row, 0, modelrow);
            cell.SetCellValue("差旅费");
            ExcelClass.GetCell(row, 1, modelrow).SetCellValue(errands.Sum(e => e.Money));

        }

        /// <summary>
        /// 作用：生成评估公司汇总表
        /// 作者：汪建龙
        /// 编写时间：2017年1月17日14:29:22
        /// </summary>
        /// <param name="head">表格头信息</param>
        /// <param name="ReportTypes"></param>
        /// <param name="list"></param>
        /// <param name="sheet"></param>
        public static void WriteCompany(string head,List<ReportType> ReportTypes, List<BillRecordView> list,ref ISheet sheet)
        {
            if (list == null || sheet == null)
            {
                return;
            }
            var line = 0;
            IRow row = sheet.GetRow(line);
            if (row == null)
            {
                return;
            }
            ICell cell = row.GetCell(0);
            if (cell == null)
            {
                cell = row.CreateCell(0);
            }

            cell.SetCellValue(head);
            line = 4;
            IRow modelRow = sheet.GetRow(line);
            var inComes = list.Where(e => e.Budget == Budget.Income).OrderBy(e=>e.SerialNumber).ToList();
            foreach(var item in inComes)
            {
                row = sheet.GetRow(line);
                if (row == null)
                {
                    row = sheet.CreateRow(line);
                }

                line++;
                cell = ExcelClass.GetCell(row, 0, modelRow);
                cell.SetCellValue(item.Summary);
                ExcelClass.GetCell(row, 1, modelRow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 2, modelRow).SetCellValue(item.Time.HasValue ? item.Time.Value.ToString("yyyy-MM-dd") : "未获取时间信息");
                ExcelClass.GetCell(row, 3, modelRow).SetCellValue(string.Format("{0}({1})", item.Remark, item.Remark2));
            }
            line = 4;
            var expenses = list.Where(e => e.Budget == Budget.Expense).OrderBy(e => e.SerialNumber).ToList();
            List<BillRecordView> temp = null;
            foreach(var item in ReportTypes)
            {
                row = sheet.GetRow(line);
                if (row == null)
                {
                    row = sheet.CreateRow(line);
                }
                line++;
                cell = ExcelClass.GetCell(row, 4, modelRow);
                cell.SetCellValue(item.Name);
                temp = expenses.Where(e => !string.IsNullOrEmpty(e.TName)&&e.TName.ToLower() == item.Name.ToLower()).ToList();
                ExcelClass.GetCell(row, 5, modelRow).SetCellValue(temp.Sum(e => e.Money));
                ExcelClass.GetCell(row, 6, modelRow).SetCellValue(string.Join(";", temp.Select(e => string.Format("{0}({1})", e.Remark, e.Remark2)).ToArray()));
            }

        }



        public static IWorkbook SaveSheets(Dictionary<string,List<SheetView>> dict)
        {
            IWorkbook workbook = ExcelClass.GetAbsolutePath(System.Configuration.ConfigurationManager.AppSettings["REPORT"].ToString()).OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var row = sheet.GetRow(3);
                    if (row != null)
                    {
                        var modelrow = row;
                        var serial = 1;
                        var rowNumber = 3;
                        foreach(var entry in dict)
                        {
                            if (entry.Value.First().Type == SheetType.Errand)
                            {

                            }
                        }
                    }
                }
            }
            return workbook;
        }

    }
}
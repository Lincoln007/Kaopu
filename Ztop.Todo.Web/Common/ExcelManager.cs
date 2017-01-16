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
                cell.SetCellValue(item.Coding);
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
    }
}
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager.Excels
{
    public static class ExcelManager
    {
        public static IWorkbook WriteSheets(List<Sheet> list,DownloadEnum download)
        {
            switch (download)
            {
                case DownloadEnum.Sheet:
                    return WriteAllSheet(list);
                case DownloadEnum.Daily_Sheet:
                    return WriteDaily(list.Where(e=>e.Type==SheetType.Daily).ToList());
                case DownloadEnum.Errand_Sheet:
                    return WriteErrand(list.Where(e=>e.Type==SheetType.Errand).ToList());
                case DownloadEnum.Transfer_Sheet:
                    return WriteTransfer(list.Where(e => e.Type == SheetType.Transfer).ToList());
                case DownloadEnum.Reception_Sheet:
                    return WriteReception(list.Where(e => e.Type == SheetType.Reception).ToList());
            }

            return null;
        }
        public static IWorkbook WriteVerify(List<VerifyView> list,DownloadEnum download)
        {
            switch (download)
            {
                case DownloadEnum.Transfer_Sheet:
                    return WriteTransfer(list);
                //default:
                //    return null;
            }
            return null;
        }

        private static IWorkbook WriteAllSheet(List<Sheet> list)
        {
            IWorkbook workbook = ParameterManager.ShentuFilePath.OpenExcel();
            if (workbook != null)
            {
                var sheet1 = workbook.GetSheetAt(0);
                if (sheet1 != null)
                {
                    WriteDaily(list.Where(e => e.Type == SheetType.Daily).ToList(), sheet1);
                }
                var sheet2 = workbook.GetSheetAt(1);
                if (sheet2 != null)
                {
                    WriteErrand(list.Where(e => e.Type == SheetType.Errand).ToList(), sheet2);
                }
                var sheet3 = workbook.GetSheetAt(2);
                if (sheet3 != null)
                {
                    WriteReception(list.Where(e => e.Type == SheetType.Reception).ToList(), sheet3);
                }
                var sheet4 = workbook.GetSheetAt(3);
                if (sheet4 != null)
                {
                    WriteDaily(list.Where(e => e.Type == SheetType.Transfer).ToList(), sheet4);
                }
            }
            return workbook;
        }

        private static IWorkbook WriteErrand(List<Sheet> list)
        {
            IWorkbook workbook = ParameterManager.SHentuErrand.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    WriteErrand(list, sheet);
                }
            }

            return workbook;
        }
        private static IWorkbook WriteDaily(List<Sheet> list)
        {
            IWorkbook workbook = ParameterManager.ShentuDaily.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    WriteDaily(list, sheet);
                }
            }

            return workbook;
        }
        private static IWorkbook WriteTransfer(List<Sheet> list)
        {
            IWorkbook workbook = ParameterManager.ShentuTransfer.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    WriteDaily(list, sheet);
                }
            }
            return workbook;
        }
        private static IWorkbook WriteTransfer(List<VerifyView> list)
        {
            IWorkbook workbook = ParameterManager.ShentuTransfer.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    WriteTransfer(list, sheet);
                }
            }

            return workbook;
        }
        private static IWorkbook WriteReception(List<Sheet> list)
        {
            IWorkbook workbook = ParameterManager.ShentuReception.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    WriteReception(list, sheet);
                }
            }
            return workbook;
        }

        private static void WriteReception(List<Sheet> list,ISheet sheet)
        {
            var serial = 1;
            var line = 1;
            var modelrow = sheet.GetRow(serial);
            foreach(var item in list)
            {
                var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                line++;
                row.Height = modelrow.Height;
                var cell = ExcelClass.GetCell(row, 0, modelrow);
                cell.SetCellValue(serial++);
                ExcelClass.GetCell(row, 1, modelrow).SetCellValue(item.CheckNumber);
                ExcelClass.GetCell(row, 2, modelrow).SetCellValue(item.Name);
                ExcelClass.GetCell(row, 3, modelrow).SetCellValue(item.Time.ToLongDateString());
                ExcelClass.GetCell(row, 4, modelrow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 5, modelrow).SetCellValue(item.Count);
                ExcelClass.GetCell(row, 6, modelrow).SetCellValue(item.Remarks);
                if (item.Reception != null)
                {
                    ExcelClass.GetCell(row, 7, modelrow).SetCellValue(item.Reception.RTime.ToLongDateString());
                    ExcelClass.GetCell(row, 8, modelrow).SetCellValue(item.Reception.CityName);
                    ExcelClass.GetCell(row, 9, modelrow).SetCellValue(item.Reception.Amount);
                    ExcelClass.GetCell(row, 10, modelrow).SetCellValue(item.Reception.Persons);
                    if (item.Reception.Items != null && item.Reception.Items.Count > 0)
                    {
                        var rline = line - 1;
                        foreach(var entry in item.Reception.Items)
                        {
                            row = sheet.GetRow(rline) ?? sheet.CreateRow(rline);
                            rline++;
                            ExcelClass.GetCell(row, 11, modelrow).SetCellValue(entry.Content);
                            ExcelClass.GetCell(row, 12, modelrow).SetCellValue(entry.Coin);
                            ExcelClass.GetCell(row, 13, modelrow).SetCellValue(entry.Way.GetDescription());
                           // ExcelClass.GetCell(row, 14, modelrow).SetCellValue(entry.Average);
                            ExcelClass.GetCell(row, 15, modelrow).SetCellValue(entry.Memo);
                        }
                        if (rline > line)
                        {
                            row = sheet.GetRow(rline - 1);
                            for(var i = 0; i < 11; i++)
                            {
                                ExcelClass.GetCell(row, i, modelrow);
                                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(line - 1, rline - 1, i, i));
                            }
                        }
                        line = rline;
                    }
                }
            }
        }

        /// <summary>
        /// 作用：编写出差报销单文件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sheet"></param>
        private static void WriteErrand(List<Sheet> list, ISheet sheet)
        {
            var serial = 1;
            var line = 2;
            var modelrow = sheet.GetRow(line);
            foreach(var item in list)
            {
                var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                row.Height = modelrow.Height;
                var cell = ExcelClass.GetCell(row, 0, modelrow);
                cell.SetCellValue(serial++);
                ExcelClass.GetCell(row, 1, modelrow).SetCellValue(item.CheckNumber);
                ExcelClass.GetCell(row, 2, modelrow).SetCellValue(item.Name);
                ExcelClass.GetCell(row, 3, modelrow).SetCellValue(item.Time.ToLongDateString());
                ExcelClass.GetCell(row, 4, modelrow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 5, modelrow).SetCellValue(item.Count);
                ExcelClass.GetCell(row, 6, modelrow).SetCellValue(item.Remarks);
                if (item.Evection != null)
                {
                    ExcelClass.GetCell(row, 7, modelrow).SetCellValue(item.Evection.Place);
                    ExcelClass.GetCell(row, 8, modelrow).SetCellValue(item.Evection.Reason);
                    ExcelClass.GetCell(row, 9, modelrow).SetCellValue(item.Evection.Hotel);
                    ExcelClass.GetCell(row, 10, modelrow).SetCellValue(item.Evection.Mark);
                    ExcelClass.GetCell(row, 11, modelrow).SetCellValue(item.Evection.Other);
                    ExcelClass.GetCell(row, 12, modelrow).SetCellValue(item.Evection.Persons);
                    ExcelClass.GetCell(row, 13, modelrow).SetCellValue(item.Evection.SubSidy);
                    ExcelClass.GetCell(row, 14, modelrow).SetCellValue(item.Evection.Traffic);
                    var eLine = line;
                    var tLine = line;
                    if (item.Evection.Errands != null && item.Evection.Errands.Count > 0)
                    {
                        foreach(var errand in item.Evection.Errands)
                        {
                            row = sheet.GetRow(eLine) ?? sheet.CreateRow(eLine);
                            eLine++;
                            ExcelClass.GetCell(row, 15, modelrow).SetCellValue(errand.Users);
                            ExcelClass.GetCell(row, 16, modelrow).SetCellValue(string.Format("{0} 至 {1}", errand.StartTime.ToString("yyyy-MM-dd"), errand.EndTime.ToString("yyyy-MM-dd")));
                            ExcelClass.GetCell(row, 17, modelrow).SetCellValue(errand.Peoples);
                            ExcelClass.GetCell(row, 18, modelrow).SetCellValue(errand.Days);
                        }
                    }
                    if (item.Evection.TCosts != null && item.Evection.TCosts.Count > 0)
                    {
                        foreach(var cost in item.Evection.TCosts)
                        {
                            row = sheet.GetRow(tLine) ?? sheet.CreateRow(tLine);
                            tLine++;
                            ExcelClass.GetCell(row, 19, modelrow).SetCellValue(cost.Type.GetDescription());
                            ExcelClass.GetCell(row, 20, modelrow).SetCellValue(cost.Cost);
                            ExcelClass.GetCell(row, 21, modelrow).SetCellValue(cost.Type==BusType.Company||cost.Type==BusType.Personal? cost.KiloMeters.ToString():"/");
                            ExcelClass.GetCell(row, 22, modelrow).SetCellValue(cost.Type == BusType.Company || cost.Type == BusType.Personal ? cost.Plate : "/");
                            ExcelClass.GetCell(row, 23, modelrow).SetCellValue(cost.Type == BusType.Company || cost.Type == BusType.Personal ? cost.Toll.ToString() : "/");
                            ExcelClass.GetCell(row, 24, modelrow).SetCellValue(cost.Type == BusType.Company ? cost.Petrol.ToString() : "/");
                            ExcelClass.GetCell(row, 25, modelrow).SetCellValue(cost.Type == BusType.Company || cost.Type == BusType.Personal ? cost.Driver.GetDescription() : "/");
                            ExcelClass.GetCell(row, 26, modelrow).SetCellValue(cost.Type == BusType.Company || cost.Type == BusType.Personal ? cost.CarPetty.ToString() : "/");
                        }
                    }
                    var currentline = eLine > tLine ? eLine : tLine;
                    if (currentline - 1 > line)
                    {
                        row = sheet.GetRow(currentline - 1);
                        
                        for (var i = 0; i < 15; i++)
                        {
                            ExcelClass.GetCell(row, i, modelrow);
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(line, currentline - 1, i, i));
                        }
                    }
                    line = currentline;
                }
                else
                {
                    line++;
                }
            }
        }
        private static void WriteTransfer(List<VerifyView> list,ISheet sheet)
        {
            var serial = 1;
            var line = 1;
            var modelRow = sheet.GetRow(serial);
            ExcelClass.GetCell(sheet.GetRow(0), 1).SetCellValue("流水编号");
            foreach(var item in list)
            {
                var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                line++;
                row.Height = modelRow.Height;
                var cell = ExcelClass.GetCell(row, 0, modelRow);
                cell.SetCellValue(serial++);
                ExcelClass.GetCell(row, 1, modelRow).SetCellValue(item.PrintNumber);
                ExcelClass.GetCell(row, 2, modelRow).SetCellValue(item.SheetName);
                ExcelClass.GetCell(row, 3, modelRow).SetCellValue(item.SheetTime.ToLongDateString());
                ExcelClass.GetCell(row, 4, modelRow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 5, modelRow).SetCellValue(item.Count);
                ExcelClass.GetCell(row, 6, modelRow).SetCellValue(item.Remarks);
                ExcelClass.GetCell(row, 7, modelRow).SetCellValue(item.subName + "-" + item.sName);
                ExcelClass.GetCell(row, 8, modelRow).SetCellValue(item.Details);
                ExcelClass.GetCell(row, 9, modelRow).SetCellValue(item.Price.HasValue?item.Price.Value:.0);
            }
        }

        /// <summary>
        /// 作用：日常报销填写
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sheet"></param>
        private static void WriteDaily(List<Sheet> list,ISheet sheet)
        {
            var serial = 1;//当前报销单的序号数
            var line = 1;//当前写的行数
            var modelRow = sheet.GetRow(serial);
            foreach(var item in list)
            {
                var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                line++;
                row.Height = modelRow.Height;
                var cell = ExcelClass.GetCell(row, 0, modelRow);
                cell.SetCellValue(serial++);
                ExcelClass.GetCell(row, 1, modelRow).SetCellValue(item.CheckNumber);
                ExcelClass.GetCell(row, 2, modelRow).SetCellValue(item.Name);
                ExcelClass.GetCell(row, 3, modelRow).SetCellValue(item.Time.ToLongDateString());
                ExcelClass.GetCell(row, 4, modelRow).SetCellValue(item.Money);
                ExcelClass.GetCell(row, 5, modelRow).SetCellValue(item.Count);
                ExcelClass.GetCell(row, 6, modelRow).SetCellValue(item.Remarks);
                if (item.Substancs_Views != null&&item.Substancs_Views.Count>0)
                {
                    var sline = line - 1;
                    foreach(var subView in item.Substancs_Views)
                    {
                        row = sheet.GetRow(sline) ?? sheet.CreateRow(sline);
                        sline++;
                        ExcelClass.GetCell(row, 7, modelRow).SetCellValue(subView.Title);
                        ExcelClass.GetCell(row, 8, modelRow).SetCellValue(subView.Details);
                        ExcelClass.GetCell(row, 9, modelRow).SetCellValue(subView.Price);
                        
                    }
                    if (sline > line)
                    {
                        row = sheet.GetRow(sline - 1);
                        for (var i = 0; i < 7; i++)
                        {
                            ExcelClass.GetCell(row, i, modelRow);
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(line-1, sline-1, i, i));
                        }
                    }
                    line = sline;
                }
            }
        }
       
    }
}

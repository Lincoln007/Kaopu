using NPOI.SS.UserModel;
using System;
using System.IO;

namespace Ztop.Todo.Common
{
    public static class ExcelClass
    {
        private static string _folder = "Excels";
        public static string GetAbsolutePath(string fileName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _folder, fileName);
        }

        public   static IWorkbook OpenExcel(this string filePath)
        {
            IWorkbook workbook = null;
            using (var fs=new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
            }

            return workbook;
        }


        public static ICell GetCell(IRow row,int line,IRow modelRow = null)
        {
            ICell cell = row.GetCell(line);
            if (cell == null)
            {
                if (modelRow != null)
                {
                    cell = row.CreateCell(line, modelRow.GetCell(line).CellType);
                    cell.CellStyle = modelRow.GetCell(line).CellStyle;
                }else
                {
                    cell = row.CreateCell(line);
                }
            }
            return cell;
        }


        public static int WriteBase<T>(T data, IRow row, int startLine, IRow moldRow)
        {
            System.Reflection.PropertyInfo[] propList = typeof(T).GetProperties();
            var a = .0;
            var b = 0;
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    if (double.TryParse(item.GetValue(data, null).ToString(), out a))
                    {
                        GetCell(row, startLine, moldRow).SetCellValue(a);
                    }
                    startLine++;
                }
                else if (item.PropertyType.Equals(typeof(int)))
                {
                    if (int.TryParse(item.GetValue(data, null).ToString(), out b))
                    {
                        GetCell(row, startLine, moldRow).SetCellValue(b);
                    }
                    startLine++;
                }
            }
            return startLine;
        }



    }
}

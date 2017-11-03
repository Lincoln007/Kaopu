using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("sheet_view")]
    public class SheetView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int Count { get; set; }
        public double Money { get; set; }
        public Status Status { get; set; }
        public string Controler { get; set; }
        public string Remarks { get; set; }
        public bool Deleted { get; set; }
        public SheetType Type { get; set; }
        public string Checkers { get; set; }
        public DateTime? CheckTime { get; set; }
        public string Coding { get; set; }
        public string Number { get; set; }
        public int NumberExt { get; set; }
        public int? CheckExt { get; set; }
        public Cost? Cost { get; set; }
        public int GroupID { get; set; }
        public string UserName { get; set; }
        public string Details { get; set; }
        public double? Price { get; set; }
        public string SubName { get; set; }
        public string SubSName { get; set; }
        public string Place { get; set; }
        public string Reason { get; set; }
        public string Mark { get; set; }
        public string Persons { get; set; }


        [NotMapped]
        public string PrintNumber
        {
            get
            {
                return string.Format("{0}{1}", Number, NumberExt.ToString("0000"));
            }
        }
        [NotMapped]
        public string CheckNumber
        {
            get
            {
                if (Status == Status.Filing || Status == Status.Examined || Status == Status.Cash)
                {
                    if (CheckTime.HasValue && CheckExt.HasValue)
                    {
                        return string.Format("{0}{1}{2}", CheckTime.Value.Year, CheckTime.Value.Month.ToString("00"), CheckExt.Value.ToString("0000"));
                    }
                }
                return "未生成单据编号";
            }
        }

    }
}

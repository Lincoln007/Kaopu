using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("verify_view")]
    public class VerifyView
    {
        public int ID { get; set; }
        /// <summary>
        /// 步骤
        /// </summary>
        public Step Step { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Name { get; set; }
        public string Reason { get; set; }
        public Position Position { get; set; }
        /// <summary>
        /// 报销单ID
        /// </summary>
        public int SID { get; set; }
        /// <summary>
        /// 报销人名字
        /// </summary>
        public string SheetName { get; set; }
        public string Coding { get; set; }
        public int Count { get; set; }
        public double Money { get; set; }
        public Status Status { get; set; }
        public string Controler { get; set; }
        public string Number { get; set; }
        public int NumberExt { get; set; }
        public string PrintNumber { get; set; }

        //[NotMapped]
        //public string PrintNumber
        //{
        //    get
        //    {
        //        return string.Format("{0}{1}", Number, NumberExt.ToString("0000"));
        //    }
        //}
        public DateTime? CheckTime { get; set; }
        public int ? CheckExt { get; set; }
        public string CheckNumber { get; set; }

        //[NotMapped]
        //public string CheckNumber
        //{
        //    get
        //    {
        //        if (Status == Status.Filing || Status == Status.Examined || Status == Status.Cash)
        //        {
        //            if (CheckTime.HasValue && CheckExt.HasValue)
        //            {
        //                return string.Format("{0}{1}{2}", CheckTime.Value.Year, CheckTime.Value.Month.ToString("00"), CheckExt.Value.ToString("0000"));
        //            }
        //        }
        //        return "未生成单据编号";
        //    }
        //}
        public bool Deleted { get; set; }
        public DateTime SheetTime { get; set; }
        public SheetType Type { get; set; }
        public int? SubID { get; set; }
        public int? RID { get; set; }
        public int? SRID { get; set; }
        public string sName { get; set; }
        public string Details { get; set; }
        public string subName { get; set; }
        public double? Price { get; set; }
        public string Remarks { get; set; }
        public string Place { get; set; }
    }
}

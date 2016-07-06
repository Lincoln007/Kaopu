using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("billaccount")]
    public class BillAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Coding { get; set; }
        public DateTime Time { get; set; }
        public double Money { get; set; }
        /// <summary>
        /// 可关联金额
        /// </summary>
        public double Leave { get; set; }
        public string Account { get; set; }
        [MaxLength(1023)]
        public string Remark { get; set; }
        public Association Association { get; set; }
        [NotMapped]
        public List<Invoice> Invoices { get; set; }
    }

    public enum Association
    {
        [Description("未关联")]
        None,
        [Description("部分关联")]
        Part,
        [Description("全部关联")]
        Full
    }
}

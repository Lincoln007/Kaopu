using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("bills")]
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>
        //public string Coding { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? Time { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 对方 单位 /户名
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 收支
        /// </summary>
        //[Column(TypeName = "int")]
        //public Budget? Budget { get; set; }
        //[NotMapped]
        //[Column(TypeName = "int")]
        //public Cost? Cost { get; set; }
        ///// <summary>
        ///// 支出 的时候 选择实际支出 选择分类
        ///// </summary>
        //[Column(TypeName = "int")]
        //public Category? Category { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public double Balance { get; set; }
        public int BID { get; set; }
        public double Leave { get; set; }
        public Association Association { get; set; }
        [NotMapped]
        public List<BillContract> BillContracts { get; set; }
        public int HID { get; set; }
        public int BBID { get; set; }

        [NotMapped]
        public BillOne One { get; set; }

        [NotMapped]
        public BillTwoView TwoView { get; set; }
    }

 
}

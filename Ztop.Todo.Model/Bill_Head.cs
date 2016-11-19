using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("bill_heads")]
    public class Bill_Head
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 公司类别
        /// </summary>
        [Column(TypeName ="int")]
        public Company Company { get; set; }
        [NotMapped]
        public List<BillOne> Ones { get; set; }
        [NotMapped]
        public string Head
        {
            get
            {
                return string.Format("{0}年{1}月", Year, Month);
            }
        }
    }

    /// <summary>
    /// 2016-11-12  重新设计银行对账Model
    /// 
    /// </summary>
    [Table("bill_ones")]
    public class BillOne
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? Time { get; set; }
        /// <summary>
        /// 凭证号
        /// </summary>
        public string Voucher { get; set; }
        /// <summary>
        /// 收入 还是支出
        /// </summary>
        [Column(TypeName = "int")]
        public Budget? Budget { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string CounterPart { get; set; }
        /// <summary>
        /// 对方户名
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [Column(TypeName = "int")]
        public Cost? Cost { get; set; }
        /// <summary>
        /// 二级类别  收入的时候 为null
        /// </summary>
        [Column(TypeName = "int")]
        public Category? Category { get; set; }
        public int HID { get; set; }

    }
}

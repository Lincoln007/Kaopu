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
        /// <summary>
        /// 评估公司
        /// </summary>
        [NotMapped]
        public List<BillOne> Ones { get; set; }
        /// <summary>
        /// 规划公司
        /// </summary>
        [NotMapped]
        public List<BillTwo> Twos { get; set; }
        [NotMapped]
        public string Head
        {
            get
            {
                return string.Format("{0}年{1}月", Year, Month);
            }
        }
    }

    public class BillBase
    {
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
        /// 年月ID
        /// </summary>
        public int HID { get; set; }
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
    }
    /// <summary>
    /// 2016-11-12  重新设计银行对账Model----评估公司  
    /// 
    /// </summary>
    [Table("bill_ones")]
    public class BillOne:BillBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }  
        /// <summary>
        /// 凭证号
        /// </summary>
        public string Voucher { get; set; } 
        /// <summary>
        /// 对方账号
        /// </summary>
        public string CounterPart { get; set; } 
    }
    /// <summary>
    /// 
    /// </summary>
    [Table("bill_twos")]
    public class BillTwo:BillBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 手续费总额
        /// </summary>
        public double CommissionCharge { get; set; }
        /// <summary>
        /// 交易方式
        /// </summary>
        public string Way { get; set; }
        /// <summary>
        /// 交易行名
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 交易类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 对方省市
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 对方户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 交易附言
        /// </summary>
        public string PostScript { get; set; }

    }
}

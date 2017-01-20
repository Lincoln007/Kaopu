using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("bill_records")]
    public class BillRecord
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
        /// 收入还是支出
        /// </summary>
        [Column(TypeName="int")]
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
        /// 银行备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 自我备注
        /// </summary>
        public string Remark2 { get; set; }
        /// <summary>
        /// Bill_Head  ID 
        /// </summary>
        public int HID { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public Cost? Cost { get; set; }
        /// <summary>
        /// 二级类
        /// </summary>
        //public Category? Category { get; set; }
        /// <summary>
        /// 二级类 RID
        /// </summary>
        public int? RID { get; set; }
        public bool Sync { get; set; }
        /*以上为两家银行共同*/

        #region  评估
        /// <summary>
        /// 凭证号
        /// </summary>

        public string Voucher { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string CounterPart { get; set; }

        #endregion

        #region  规划
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 手续费总额
        /// </summary>
        public double? CommissionCharge { get; set; }
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
        /// 对方名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 交易附言
        /// </summary>
        public string PostScript { get; set; }


        #endregion


    }

    [Table("bill_records_view")]
    public class BillRecordView : BillRecord
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string TName { get; set; }
        public bool? Delete { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 发票信息
    /// </summary>
    [Table("invoices")]
    public class Invoice
    {
        public Invoice()
        {
            Time = DateTime.Now;
            Coding = Time.Ticks.ToString();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 开票编号
        /// </summary>
        public string Coding { get; set; }
        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 开票申请人
        /// </summary>
        public string Applicant { get; set; }
        /// <summary>
        /// 开票单位
        /// </summary>
        [Column(TypeName ="int")]
        public ZtopCompany ZtopCompany { get; set; }
        /// <summary>
        /// 对方单位
        /// </summary>
        public string OtherSideCompany { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 发票内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发票开出时间
        /// </summary>
        public DateTime? FillTime { get; set; }
        /// <summary>
        /// 发票编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 发票状态
        /// </summary>
        [Column(TypeName ="int")]
        public InvoiceState? State { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int CID { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public Contract Contract { get; set; }
        /// <summary>
        /// 到账信息
        /// </summary>
        [NotMapped]
        public List<InvoiceBill> InvoiceBills { get; set; }

    }

    public enum InvoiceState
    {
        [Description("已开票")]
        Have,
        [Description("退票")]
        Back,
        [Description("红票")]
        Red
    }

    
}

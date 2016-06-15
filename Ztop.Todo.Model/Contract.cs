using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 合同
    /// </summary>
    [Table("contracts")]
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        [MaxLength(255)]
        public string Coding { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        [MaxLength(1023)]
        public string Name { get; set; }
        /// <summary>
        /// 对方单位
        /// </summary>
        [MaxLength(1023)]
        public string Company { get; set; }
        /// <summary>
        /// 创建人员
        /// </summary>
        [MaxLength(255)]
        public string Creator { get; set; }
        [Column(TypeName ="int")]
        public ZtopCompany ZtopCompany { get; set; }

        [NotMapped]
        public List<Invoice> Invoices { get; set; }
    }

    public enum ZtopCompany
    {
        [Description("杭州智拓土地规划设计咨询有限公司")]
        Evaluation,
        [Description("杭州智拓房地产土地评估咨询有限公司")]
        Projection
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 报销单审核信息表
    /// </summary>
    [Table("verifys")]
    public class Verify
    {
        public Verify()
        {
            //Time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 步骤
        /// </summary>
        [Column(TypeName ="int")]
        public Step Step { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注  （原因）
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        //public bool IsCheck { get; set; }
        public Position Position { get; set; }
        /// <summary>
        /// 关联的报销单ID
        /// </summary>
        public int SID { get; set; }
        [NotMapped]
        public Sheet Sheet { get; set; }

    }

    public enum Step
    {
        [Description("报销填单")]//报销人
        Create,
        [Description("主管核实")]//主管
        Examine,
        [Description("财务负责人核准")]//申屠
        Confirm,
        [Description("报销确认")]//财务
        Approved,
        [Description("归档")]//申屠报销归档
        Filing,
        [Description("撤回")]
        Roll,
        [Description("现金核算")]
        Cash,
        [Description("确认到账")]
        Affirm,
        [Description("转账作废")]
        Repeal
    }

    public enum Position
    {
        [Description("审核通过")]
        Check,
        [Description("退回")]
        RollBack
    }
}

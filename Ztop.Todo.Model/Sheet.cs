using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 报销单
    /// </summary>
    [Table("sheets")]
    public class Sheet
    {
        public Sheet()
        {
            Time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 单据编号  实例值
        /// </summary>
        [NotMapped]
        public SerialNumber SerialNumber { get; set; }
        /// <summary>
        /// 报销人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报销时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 报销单状态
        /// </summary>
        [Column(TypeName ="int")]
        public Status Status { get; set; }
        /// <summary>
        /// 当前需要审核人
        /// </summary>
        public string Controler { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// 内容分项清单
        /// </summary>
        [NotMapped]
        public List<Substancs> Substances { get; set; }
        /// <summary>
        /// 审核信息表
        /// </summary>
        [NotMapped]
        public List<Verify> Verifys { get; set; }

    }
    public enum Status
    {
        [Description("草稿")]//只是填写了报销单  但是没有提交
        OutLine,
        //[Description("提交成功未审核")]//报销单上交上级  但是上级没有查看审核过
        //Post,
        [Description("审核中-主管")]//主管开始审核 
        ExaminingDirector,
        [Description("审核中-申屠")]//申屠经理审核
        ExaminingManager,
        [Description("审核中-财务")]//财务审核
        ExaminingFinance,
        [Description("已完成")]//审核完成
        Examined,
    }
    
}

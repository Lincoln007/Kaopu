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
        public string Number { get; set; }
        public int NumberExt { get; set; }

        [NotMapped]
        public string PrintNumber
        {
            get
            {
                return string.Format("{0}{1}", Number, NumberExt.ToString("0000"));
            }
        }
        public int? CheckExt { get; set; }

        [NotMapped]
        public string CheckNumber
        {
            get
            {
                if (Status == Status.Filing || Status == Status.Examined)
                {
                    if (CheckTime.HasValue&&CheckExt.HasValue)
                    {
                        return string.Format("{0}{1}{2}", CheckTime.Value.Year, CheckTime.Value.Month.ToString("00"), CheckExt.Value.ToString("0000"));
                    }
                }
                return "/";
            }
        }

        public string BarCode { get; set; }

        /// <summary>
        /// 单据编号  实例值  流水单据编号
        /// </summary>
        //[NotMapped]
       // public SerialNumber SerialNumber { get; set; }
        /// <summary>
        /// 流水单据编号
        /// </summary>
        public string Coding { get; set; }

        /// <summary>
        /// 报销人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报销时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 发票张数
        /// </summary>
        public int Count { get; set; }
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
        /// 报销单类型
        /// </summary>
        public SheetType Type { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// 日常报销—— 内容分项清单
        /// </summary>
        [NotMapped]
        public List<Substancs> Substances { get; set; }
        /// <summary>
        /// 出差报销——分类清单
        /// </summary>
        [NotMapped]
        public Evection Evection { get; set; }
        /// <summary>
        /// 审核信息表
        /// </summary>
        [NotMapped]
        public List<Verify> Verifys { get; set; }
        /// <summary>
        /// 审核人  直接上级领导  点击提交的第一人领导
        /// </summary>
        public string Checkers { get; set; }
        /// <summary>
        /// 财务审核通过时间
        /// </summary>
        public DateTime? CheckTime { get; set; }

    }
    public enum SheetType
    {
        [Description("日常报销")]
        Daily,
        [Description("出差报销")]
        Errand
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
        [Description("报销归档")]
        Filing,
        [Description("已完成")]//审核完成
        Examined,
        [Description("审核不通过")]
        RollBack
    }
    
}

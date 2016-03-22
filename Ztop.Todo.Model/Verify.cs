﻿using System;
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
        public bool IsCheck { get; set; }
        /// <summary>
        /// 关联的报销单ID
        /// </summary>
        public int SID { get; set; }

    }

    public enum Step
    {
        [Description("报销填单")]
        Create,
        [Description("主管审核")]
        Examine,
        [Description("报销确认")]
        Confirm,
        [Description("财务负责人核准")]
        Approved
    }
}
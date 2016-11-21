using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class SheetQueryParameter
    {
        /// <summary>
        /// 报销人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 页数信息
        /// </summary>
        public PageParameter Page { get; set; }
        /// <summary>
        /// 是否删除  全部  删除  未删除
        /// </summary>
        public bool? Deleted { get; set; }
        /// <summary>
        /// 当前 需要审核的人
        /// </summary>
        public string Controler { get; set; }
        /// <summary>
        /// 报销单状态
        /// </summary>
        public Status? Status { get; set; }
    }

    public class QueryParameter
    {
        /// <summary>
        /// 表单创建人
        /// </summary>
        public Operator Creater { get; set; }
        /// <summary>
        /// 自定义创建人
        /// </summary>
        public string Custom { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusPosition Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public Operator Checker { get; set; }
        /// <summary>
        /// 自定义审核人
        /// </summary>
        public string Checker2 { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// 报销类型   null 全部  日常报销  出差报销
        /// </summary>
        public SheetType? Type { get; set; }
        public double? MinMoney { get; set; }
        public double? MaxMoney { get; set; }
        public PageParameter Page { get; set; }
    }

    public class SheetQueryParameter2
    {
        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Category { get; set; }

    }

    public enum Operator
    {
        我,
        全部,
        自定义
    }

    public enum StatusPosition
    {
        不限,
        草稿,
        未审核,
        已审核
    }
    public enum Time
    {
        不限,
        一周内,
        一月内,
        半年内,
        一年内
    }
    public enum Order
    {
        [Description("时间排序")]
        Time,
        [Description("金额排序")]
        Money,
        [Description("流水编号排序")]
        PrintNumber,
        [Description("单据编号排序")]
        CheckNumber
    }


}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class SheetQueryParameter
    {
        public string Name { get; set; }
        public PageParameter Page { get; set; }
        public bool? Deleted { get; set; }
        public string Controler { get; set; }
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
        Money
    }


}

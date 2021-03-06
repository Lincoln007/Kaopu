﻿using System;

namespace Ztop.Todo.Model
{
    public class SheetVerifyParameter
    {
        /// <summary>
        /// 报销单流水编号
        /// </summary>
        public string Coding { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string CheckKey { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// 金额范围最小值
        /// </summary>
        public double? MinMoney { get; set; }
        /// <summary>
        /// 金额范围最大值
        /// </summary>
        public double? MaxMoney { get; set; }
        /// <summary>
        /// 报销人关键字
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 报销内容关键字
        /// </summary>
        public string Content { get; set; }
        public int? RID { get; set; }

        public int? SRID { get; set; }
        public SheetType? SheetType { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
        public PageParameter Page { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

        public Position? Position { get; set; }

    }

    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class InvoiceParameter:ParameterBase
    {
        /// <summary>
        /// 申请人所在部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 发票状态
        /// </summary>
        public InvoiceState? Status { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 到账情况
        /// </summary>
        public Recevied? Recevied { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 最小值金额
        /// </summary>
        public double? MinMoney { get; set; }
        /// <summary>
        /// 最大值金额
        /// </summary>
        public double? MaxMoney { get; set; }
       
        public bool Instance { get; set; }
        public string Key { get; set; }

    }

    public enum Recevied
    {
        [Description("未到账")]
        None,
        [Description("部分到账")]
        Part,
        [Description("已到账")]
        ALL
    }



}

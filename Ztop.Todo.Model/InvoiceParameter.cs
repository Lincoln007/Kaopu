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
        /// 金额
        /// </summary>
        public double Money { get; set; }


        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 发票状态
        /// </summary>
        public InvoiceState? Status { get; set; }


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

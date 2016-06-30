using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class ParameterBase
    {
        /// <summary>
        /// 开票单位
        /// </summary>
        public ZtopCompany? ZtopCompany { get; set; }
        /// <summary>
        /// 对方单位
        /// </summary>
        public string OtherSide { get; set; }
   
        /// <summary>
        /// 到账情况
        /// </summary>
        public Recevied? Recevied { get; set; }
        /// <summary>
        /// 最小值金额
        /// </summary>
        public double? MinMoney { get; set; }
        /// <summary>
        /// 最大值金额
        /// </summary>
        public double? MaxMoney { get; set; }
        /// <summary>
        /// 申请人所在部门
        /// </summary>
        public string Department { get; set; }
        public PageParameter Page { get; set; }
    }
}

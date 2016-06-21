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
        public PageParameter Page { get; set; }
    }
}

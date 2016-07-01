using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class ContractParameter:ParameterBase
    {
        public string Name { get; set; }
        public bool? Archived { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ContractState? Status { get; set; }
        
    }

    public enum ContractState
    {
        [Description("未开发票")]
        None,
        [Description("部分开票")]
        Part,
        [Description("全部开票")]
        ALL
    }
}

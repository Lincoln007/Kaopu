using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class ContractParameter:ParameterBase
    {
        public string Name { get; set; }
        public bool? Archived { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class ProjectRecordParameter
    {
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public PageParameter Page { get; set; }
    }
}

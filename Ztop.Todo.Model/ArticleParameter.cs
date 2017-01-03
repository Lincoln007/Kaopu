using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class ArticleParameter
    {
        public string Name { get; set; }
        public string OtherSide { get; set; }
        public string Remark { get; set; }
        public double? MinMoney { get; set; }
        public double? MaxMoney { get; set; }
        public bool? Deleted { get; set; }
        public string Number { get; set; }
        public PageParameter Page { get; set; }
    }
}

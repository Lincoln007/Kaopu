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
        public ArticleState? State { get; set; }
        public string Year { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string ProjectType { get; set; }
        public string PayCompany { get; set; }
        public PageParameter Page { get; set; }
    }
}

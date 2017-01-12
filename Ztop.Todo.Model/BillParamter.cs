using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class BillParamter
    {
        public Company? Company { get; set; }
        public double? MinMoney { get; set; }
        public double? MaxMoney { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string OtherSide { get; set; }
        public Association? Association { get; set; }
        public string Remark { get; set; }
        public PageParameter Page { get; set; }
        public Cost? Cost { get; set; }
    }
}

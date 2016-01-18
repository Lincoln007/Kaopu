using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class  PageParameter
    {
        public PageParameter() : this(1, 20) { }

        public PageParameter(int page, int limit)
        {
            PageIndex = page < 1 ? 1 : page;
            PageSize = limit < 10 ? 10 : limit;
        }

        public int RecordCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                return RecordCount / PageSize + (RecordCount % PageSize) > 0 ? 1 : 0;
            }
        }
    }
}

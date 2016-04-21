using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class StringHelper
    {
        public static string AToString(this List<string> list)
        {
            var result = string.Empty;
            foreach(var item in list)
            {
                result = string.IsNullOrEmpty(result) ? item : result + '*' + item;
            }
            return result;
        }
    }
}

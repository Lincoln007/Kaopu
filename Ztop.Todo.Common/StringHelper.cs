using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class StringHelper
    {
        public static string Extract(this string str, int Index = 0, string Filter = "CN=")
        {
            var key = str.Split(',');
            return key[Index].Replace(Filter, "");
        }
    }
}

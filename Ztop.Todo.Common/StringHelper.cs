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


        public static int[] ToIntArray(this string str, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            return str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(val => int.Parse(val)).ToArray();
        }

        /// <param name="str">08:30</param>
        public static TimeSpan ToTimeSpan(this string str)
        {
            var arr = str.ToIntArray(':');
            switch (arr.Length)
            {
                case 3:
                    return new TimeSpan(arr[0], arr[1], arr[2]);
                case 2:
                    return new TimeSpan(arr[0], arr[1], 0);
                default:
                    throw new Exception("格式不正确");
            }
        }
    }
}

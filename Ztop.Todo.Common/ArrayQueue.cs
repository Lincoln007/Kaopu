using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class ArrayQueue
    {
        public static Queue<T> Tranlate<T>(this T[] array)
        {
            var queue = new Queue<T>();
            foreach(var item in array)
            {
                queue.Enqueue(item);
            }
            return queue;
        }
    }
}

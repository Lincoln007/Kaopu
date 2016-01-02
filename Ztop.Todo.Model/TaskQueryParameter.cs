using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public enum UserTaskOrder
    {
        CreateTime,
        ScheduleTime,
    }

    public class UserTaskQueryParameter
    {
        public int UserID { get; set; }

        public string SearchKey { get; set; }

        public bool IsCompleted { get; set; }

        public UserTaskOrder Order { get; set; }

        public PageParameter Page { get; set; }
    }
}

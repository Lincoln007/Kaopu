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

    public class TaskQueryParameter
    {
        public int CreatorID { get; set; }

        public int UserID { get; set; }

        public string SearchKey { get; set; }

        public bool? IsCompleted { get; set; }

        public UserTaskOrder Order { get; set; }

        public PageParameter Page { get; set; }

        public bool? HasRead { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool GetCreator { get; set; }

        public bool GetReceiver { get; set; }

        public void SetTimeRange(int days)
        {
            EndTime = DateTime.Today.AddDays(1);
            BeginTime = DateTime.Today.AddDays(-days);
        }
    }
}

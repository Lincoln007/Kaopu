using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ztop.Todo.WindowsClient
{
    public class Task
    {
        public int ID { get; set; }

        public int TaskID { get; set; }

        public int UserID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string CreatorName { get; set; }

        public DateTime? ScheduleTime { get; set; }

        public DateTime CreateTime { get; set; }
    }
}

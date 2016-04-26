using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ztop.Todo.WindowsClient
{
    public class Notification
    {
        public int ID { get; set; }

        public int InfoID { get; set; }

        public int InfoType { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public string Path { get; set; }
    }
}

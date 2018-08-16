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
        public bool IsCreator { get; set; }

        public int CreatorID { get; set; }
        public int? CreatorId2 { get; set; }

        public int ReceiverID { get; set; }
        public int? ReceiverId2 { get; set; }
        public string SearchKey { get; set; }
        /// <summary>
        /// 标题关键字
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// 内容关键词
        /// </summary>
        public string ContentKey { get; set; }

        public bool? IsCompleted { get; set; }

        public UserTaskOrder Order { get; set; }

        public PageParameter Page { get; set; }

        public bool? HasRead { get; set; }

        public int Days { get; set; }

        public bool GetCreator { get; set; }

        public bool GetReceiver { get; set; }
    }
}

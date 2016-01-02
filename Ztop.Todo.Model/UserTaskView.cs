using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 用户任务视图
    /// </summary>
    [Table("user_task_view")]
    public class UserTaskView
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int TaskID { get; set; }

        [NotMapped]
        public bool Completed { get { return CompletedTime.HasValue; } }

        public DateTime? CompletedTime { get; set; }

        public string Title { get; set; }

        public DateTime? ScheduledTime { get; set; }

        public DateTime CreateTime { get; set; }

        public int OwnerID { get; set; }

        [NotMapped]
        public User Owner { get; set; }
    }
}

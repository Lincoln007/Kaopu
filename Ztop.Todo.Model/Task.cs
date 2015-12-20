using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? ScheduledTime { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public int OwnerID { get; set; }

        public bool IsCompleted { get; set; }

        public int ParentID { get; set; }


        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
    }
}

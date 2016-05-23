using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("comment")]
    public class Comment
    {
        public Comment()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>

        public string Content { get; set; }

        /// <summary>
        /// 评论人ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>

        [Column("taskid")]
        public int UserTaskID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateTime { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}

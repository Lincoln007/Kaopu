using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("notification")]
    public class Notification
    {
        public Notification()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ReceiverID { get; set; }

        public int SenderID { get; set; }

        public int InfoID { get; set; }

        public InfoType InfoType { get; set; }

        public bool HasRead { get; set; }

        public DateTime CreateTime { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public string Path { get; set; }
    }

    public enum InfoType
    {
        [Description("任务")]
        Task = 1,

        [Description("评论")]
        Comment = 2,

    }
}

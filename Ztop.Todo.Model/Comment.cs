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

        public string Content { get; set; }

        public int UserID { get; set; }

        [Column("taskid")]
        public int UserTaskID { get; set; }

        public DateTime CreateTime { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}

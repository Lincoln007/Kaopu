using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("user_task")]
    public class UserTask
    {
        public UserTask()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int TaskID { get; set; }

        public int UserID { get; set; }
        [NotMapped]
        public User User { get; set; }

        [NotMapped]
        public Task Task { get; set; }

        [NotMapped]
        public bool IsCompleted { get { return CompletedTime.HasValue; } }

        public DateTime CreateTime { get; set; }

        public DateTime? CompletedTime { get; set; }

        public bool HasRead { get; set; }

        public bool Deleted { get; set; }
    }
}

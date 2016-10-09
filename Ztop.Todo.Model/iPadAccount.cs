using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ipad_accounts")]
    public class iPadAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        public string EPassword { get; set; }
        public string Enter { get; set; }
        public DateTime Time { get; set; }


    }
}

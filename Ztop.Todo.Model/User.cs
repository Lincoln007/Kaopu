using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]    
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }
}

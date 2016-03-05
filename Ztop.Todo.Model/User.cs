using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Model
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int GroupID { get; set; }

        public string Username { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }

        [NotMapped]
        public GroupType Type { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(RealName) ? (Username.Contains('\\') ? Username.Split('\\')[1] : Username) : RealName;
            }
        }

        public string RealName { get; set; }

        public int LoginTimes { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }

    [Serializable]
    public class UserInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("fast_ad_groups_view")]
    public class FastADGroupView
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public int OID { get; set; }

        public int GID { get; set; }

        public int UID { get; set; }
    }

    [Table("fasts_group_user_view")]
    public class FastGroupUserView
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int OID { get; set; }
        /// <summary>
        /// 上级组
        /// </summary>
        [NotMapped]
        public ADGroup Parent { get; set; }

        public int GID { get; set; }
        [NotMapped]
        public ADGroup ADGroup { get; set; }

        public int UID { get; set; }
        [NotMapped]
        public User User { get; set; }

        public string Username { get; set; }
        public string RealName { get; set; }
    }
}

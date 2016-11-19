using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("authorizes")]
    public class Authorize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ParentGroup { get; set; }
        [NotMapped]
        public string GroupName {
            get;
            set;
        }
        public string Manager { get; set; }
        public string GID { get; set; }
        [NotMapped]
        public List<ADGroup> Groups { get; set; }

    }
    [Table("authorize_fasts")]
    public class Authorize_Fast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// AD_Group组ID
        /// </summary>
        public int GID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UID { get; set; }
    }

    public enum AuthFilter
    {
        All,
        Wait,
        Agree,
        Disagree
    }
}

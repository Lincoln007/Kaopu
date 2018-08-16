using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model.Base
{
    /// <summary>
    /// 每个组 部门的管理人员
    /// </summary>
    [Table("user_group_manager")]
    public class UserGroupManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}

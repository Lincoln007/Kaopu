using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("user_group_view")]
    public class UserGroupView
    {
        [Key]
        public int ID { get; set; }
        public string RealName { get; set; }
        public string Name { get; set; }
    }
}

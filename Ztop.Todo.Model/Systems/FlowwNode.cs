using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ztop.Todo.Common;

namespace Ztop.Todo.Model
{
    [Table("floww_node")]
    public class FlowwNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        [Column("UserIds")]
        public string UserIdValues { get; set; }
        [NotMapped]
        public int[] UserIds
        {
            get
            {
                if (string.IsNullOrEmpty(UserIdValues)) return null;
                return UserIdValues.ToIntArray();
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    UserIdValues = null;
                }
                else
                {
                    UserIdValues = string.Join(",", value);
                }
            }
        }
        [NotMapped]
        public List<User> Users { get; set; }
        /// <summary>
        /// 是否检验 项目一部  项目二部
        /// </summary>
        public bool IsCheckGroup { get; set; }
        public string Remark { get; set; }
        public int FlowwId { get; set; }
        public virtual Floww Floww { get; set; }
        public int PrevId { get; set; }
        [NotMapped]
        public FlowwNode Next { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("register_ipads")]
    public class Register_iPad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RID { get; set; }
        public int IID { get; set; }
        /// <summary>
        /// 归还标志
        /// </summary>
        public iPadRevert Revert { get; set; }
        public string Restorer { get; set; }
        public DateTime? Time { get; set; }
        public Relation Relation { get; set; }
        [NotMapped]
        public iPad iPad { get; set; }
    }

    public enum Relation
    {
        Register_iPad,
        Contract_iPad,
        Invoice_iPad
    }
    public enum iPadRevert
    {
        [Description("使用中")]
        Using,
        [Description("归还")]
        Back,
        [Description("项目")]
        Projection
    }
}

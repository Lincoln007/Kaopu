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
    /// <summary>
    /// 报销单
    /// </summary>
    [Table("sheets")]
    public class Sheet
    {
        public Sheet()
        {
            Number = string.Format("");
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Time { get; set; }
        public double Money { get; set; }
        [Column(TypeName ="int")]
        public Status Status { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public List<Substancs> Substances { get; set; }
        [NotMapped]
        public List<Verify> Verifys { get; set; }

    }
    public enum Status
    {
        [Description("草稿")]
        OutLine,
        [Description("未审核")]
        Examining,
        [Description("已审核")]
        Examine,
    }
    
}

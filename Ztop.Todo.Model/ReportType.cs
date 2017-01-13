using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 报销类型
    /// </summary>
    [Table("reportTypes")]
    public class ReportType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int RID { get; set; }
        public bool Delete { get; set; }
        [NotMapped]
        public List<ReportType> Children { get; set; }
    }
}

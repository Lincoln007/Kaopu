using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("Project_Progress")]
    public class ProjectProgress
    {
        public ProjectProgress()
        {
            Time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        public double Percent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
        public bool Delete { get; set; }
        public virtual List<ProgressTable> Tables { get; set; }
    }
}

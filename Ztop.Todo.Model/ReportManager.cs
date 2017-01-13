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
    [Table("report_manager")]
    public class ReportManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UID { get; set; }
        public bool Delete { get; set; }
    }
    [Table("report_manager_view")]
    public class ReportManagerView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Delete { get; set; }
    }
    [Table("flow")]
    public class Flow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int UID { get; set; }
        public bool Delete { get; set; }
    }

    [Table("flow_view")]
    public class FlowView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string UserName { get; set; }
        public bool Delete { get; set; }
    }

  

    public enum ReportEnum
    {
        [Description("报销流程")]
        Flow,
        [Description("报销审核人员")]
        Manager
    }
}

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
    /// 报销单审核信息表
    /// </summary>
    [Table("verifys")]
    public class Verify
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public int SID { get; set; }

    }
    public enum Step
    {
        [Description("报销填单")]
        Create,
        [Description("主管审核")]
        Examine,
        [Description("报销确认")]
        Confirm,
        [Description("财务负责人核准")]
        Approved
    }
}

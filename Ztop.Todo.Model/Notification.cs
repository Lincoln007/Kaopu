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
    [Table("notification")]
    public class Notification
    {
        public Notification()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 接收者ID
        /// </summary>
        public int ReceiverID { get; set; }
        /// <summary>
        /// 发送者ID
        /// </summary>

        public int SenderID { get; set; }
        /// <summary>
        /// 信息ID(任务ID)
        /// </summary>

        public int InfoID { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>

        public InfoType InfoType { get; set; }
        /// <summary>
        /// 是否阅读
        /// </summary>

        public bool HasRead { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateTime { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public string Path { get; set; }
    }

    public enum InfoType
    {
        [Description("任务")]
        Task = 1,

        [Description("评论")]
        Comment = 2,

        [Description("报销")]
        Sheet=3,

        [Description("审核")]
        Verify=4,

        [Description("开票")]
        Invoice=5
    }
}

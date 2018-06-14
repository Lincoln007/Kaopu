using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("floww_node_data")]
    public class FlowwNodeData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? CheckTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public FlowwNodeState State { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        /// <summary>
        /// 节点创建人Id
        /// </summary>
        public int PostUserId { get; set; }
        public virtual User PostUser { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审核节点信息
        /// </summary>
        public int FlowwNodeId { get; set; }
        public virtual FlowwNode FlowwNode { get; set; }
        public int FlowwDataId { get; set; }
        public virtual FlowwData FlowwData { get; set; }

    }

    public enum FlowwNodeState
    {
        [Description("审核中")]
        Checking,
        [Description("通过")]
        Success,
        [Description("失败")]
        Fail,
        [Description("撤回")]
        Roll
    }
}

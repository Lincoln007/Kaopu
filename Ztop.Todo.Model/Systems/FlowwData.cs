using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ztop.Todo.Model
{
    [Table("floww_data")]
    public class FlowwData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int FlowwId { get; set; }
        [ForeignKey("FlowwId")]
        public virtual Floww Floww { get; set; }
        public FlowwDataState FlowwDataState { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public virtual List<FlowwNodeData> FlowwNodeDatas { get; set; }
        public FlowwNodeData GetCurrentNode()
        {
            if (FlowwNodeDatas == null)
            {
                return null;
            }
            var nodedata = FlowwNodeDatas.OrderByDescending(e => e.ID).FirstOrDefault();
            return nodedata;
        }
        public bool CanCheck(int UserId)
        {
            var currentNode = GetCurrentNode();
            if (currentNode == null)
            {
                return false;
            }
            if (currentNode.FlowwNode == null||currentNode.FlowwNode.UserIds==null)
            {
                return false;
            }

            return currentNode.FlowwNode.UserIds.Contains(UserId);
        }

    }

    public enum FlowwDataState
    {
        [Description("初始化")]
        None,
        [Description("审核中")]
        Checking,
        [Description("完成")]
        Done
    }
}

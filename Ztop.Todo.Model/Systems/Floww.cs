using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ztop.Todo.Model
{
    [Table("floww")]
    public class Floww
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public virtual List<FlowwNode> Nodes { get; set; }
        public FlowwNode GetFirstNode()
        {
            if (Nodes == null)
            {
                return null;
            }
            var node=Nodes.FirstOrDefault(e => e.PrevId == 0);
            if (node != null)
            {
                node.Next = GetNextNode(node.ID);
            }

            return node;
        }

        public FlowwNode GetNextNode(int id)
        {
            var node = Nodes.FirstOrDefault(e => e.PrevId == id);
            if (node != null)
            {
                node.Next = GetNextNode(node.ID);
            }

            return node;
        }
    }
}

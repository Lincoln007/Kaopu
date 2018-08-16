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
        public bool Delete { get; set; }
        public bool CanBack { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public virtual List<FlowwNode> Nodes { get; set; }
        [NotMapped]
        public List<FlowwNode> Nodes2 { get; set; }
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

        public FlowwNode GetPrevNode(int id)
        {
            var node = Nodes.FirstOrDefault(e => e.ID == id);
            if (node == null)
            {
                return null;
            }
            return Nodes.FirstOrDefault(e => e.ID == node.PrevId);
        }

        public FlowwNode GetLastNode()
        {
            if (Nodes == null)
            {
                return null;
            }
            foreach(var node in Nodes)
            {
                if (!Nodes.Any(e => e.PrevId == node.ID))
                {
                    return node;
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class FlowwNodeManager:ManagerBase
    {
        public FlowwNode Get(int id)
        {
            var model = Get2(id);
            if (model != null)
            {
                if (model.UserIds != null)
                {
                    model.Users = Core.UserManager.GetUsers(model.UserIds);
                }
            }

            return model;
        }

        public FlowwNode Get2(int id)
        {
            return DB.Floww_Nodes.Find(id);
        }

        public List<FlowwNode> Getlist(int flowId)
        {
            var result = DB.Floww_Nodes.Where(e => e.FlowwId == flowId).ToList();
            foreach(var item in result)
            {
                if (item.UserIds != null && item.UserIds.Length > 0)
                {
                    item.Users = Core.UserManager.GetUsers(item.UserIds);
                }
            }
            return result;
        }

        public bool Edit(FlowwNode node)
        {
            var model = DB.Floww_Nodes.Find(node.ID);
            if (model == null)
            {
                return false;
            }
            DB.Entry(model).CurrentValues.SetValues(node);
            DB.SaveChanges();
            return true;
        }

        public int Add(FlowwNode node)
        {
            DB.Floww_Nodes.Add(node);
            DB.SaveChanges();
            return node.ID;
        }

        public bool Delete(int id)
        {
            var current = DB.Floww_Nodes.Find(id);
            if (current == null)
            {
                return false;
            }

            var next = DB.Floww_Nodes.FirstOrDefault(e => e.PrevId == id);
            if (next != null)
            {
                next.PrevId = current.PrevId;
            }
            DB.Floww_Nodes.Remove(current);
            DB.SaveChanges();
            return true;
        }

        public bool Prev(int id)
        {
            var current = DB.Floww_Nodes.Find(id);
            if (current == null)
            {
                return false;
            }

            var prev = DB.Floww_Nodes.Find(current.PrevId);
            if (prev == null)
            {
                return false;
            }

            var next = DB.Floww_Nodes.FirstOrDefault(e => e.PrevId == id);
            current.PrevId = prev.PrevId;
            prev.PrevId = current.ID;
            if (next != null)
            {
                next.PrevId = prev.ID;
            }
            DB.SaveChanges();
            return true;
        }

        public bool Later(int id)
        {
            var current = DB.Floww_Nodes.Find(id);
            if (current == null)
            {
                return false;
            }
            var next = DB.Floww_Nodes.FirstOrDefault(e => e.PrevId == id);
            if (next == null)
            {
                return false;
            }
            return Prev(next.ID);
        }

        public FlowwNode GetNext(int currentId)
        {
            return DB.Floww_Nodes.FirstOrDefault(e => e.PrevId == currentId);
        }


        public FlowwNode GetPrev(int currentId)
        {
            var current = Get2(currentId);
            if (current != null)
            {
                return Get2(current.PrevId);
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Client_MessageManager:ManagerBase
    {
        public List<ClientMessage> GetList()
        {
            return DB.Client_Messages.OrderBy(e=>e.ID).ToList();
        }


        public int Add(ClientMessage client)
        {
            DB.Client_Messages.Add(client);
            DB.SaveChanges();
            return client.ID;
        }

        public bool Edit(ClientMessage client)
        {
            var entry = DB.Client_Messages.Find(client.ID);
            if (entry != null)
            {
                DB.Entry(entry).CurrentValues.SetValues(client);
                DB.SaveChanges();
                return true;
            }

            return false;
        }
    }
}

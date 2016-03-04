using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class MessageManager:ManagerBase
    {
        public void Add(Message message)
        {
            using (var db = GetDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }
    }
}

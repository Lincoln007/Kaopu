using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class CommentManager : ManagerBase
    {
        public void Save(Comment model)
        {
            using (var db = GetDbContext())
            {
                var isAdd = model.ID == 0;
                if (model.ID > 0)
                {
                    var entity = db.Comments.FirstOrDefault(e => e.ID == model.ID);
                    db.Entry(entity).CurrentValues.SetValues(model);
                }
                else
                {
                    db.Comments.Add(model);
                }
                db.SaveChanges();
                if (isAdd)
                {
                    Core.NotificationManager.Add(model);
                }
            }
        }

        public List<Comment> GetList(int taskId)
        {
            using (var db = GetDbContext())
            {
                var list = db.Comments.Where(e => e.TaskID == taskId).ToList();
                foreach (var item in list)
                {
                    item.User = Core.UserManager.GetUser(item.UserID);
                }

                return list;
            }
        }

        public Comment GetModel(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Comments.FirstOrDefault(e => e.ID == id);
            }
        }

        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Comments.FirstOrDefault(e => e.ID == id);
                db.Comments.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}

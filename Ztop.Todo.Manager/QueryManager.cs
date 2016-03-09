using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class QueryManager : ManagerBase
    {
        public List<TaskQuery> GetList(int userId)
        {
            using (var db = GetDbContext())
            {
                return db.TaskQueries.Where(e => e.UserID == userId || e.UserID == 0).ToList();
            }
        }

        public TaskQuery GetModel(int queryId)
        {
            if (queryId == 0) return null;
            using (var db = GetDbContext())
            {
                return db.TaskQueries.FirstOrDefault(e => e.ID == queryId);
            }
        }

        public void Delete(int queryId, int userId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.TaskQueries.FirstOrDefault(e => e.ID == queryId);
                if (entity != null)
                {
                    if (entity.UserID != userId)
                    {
                        throw new HttpException(401, "权限不足");
                    }
                    db.TaskQueries.Remove(entity);
                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("参数错误");
                }
            }
        }

        public void Save(TaskQuery model)
        {
            using (var db = GetDbContext())
            {
                if (model.ID > 0)
                {
                    var entity = db.TaskQueries.FirstOrDefault(e => e.ID == model.ID);
                    if (entity != null)
                    {
                        entity.Name = model.Name;
                        entity.Content = model.Content;
                    }
                }
                else
                {
                    db.TaskQueries.Add(model);
                }
                db.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{

    public class TaskManager : ManagerBase
    {
        public List<UserTaskView> GetUserTasks(UserTaskQueryParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.UserTaskViews.Where(e => e.UserID == parameter.UserID && e.IsCompleted == parameter.IsCompleted);
                if (parameter.Order == UserTaskOrder.ScheduleTime)
                {
                    query = query.Where(e => e.ScheduledTime.HasValue);
                    return query.OrderBy(e => e.ScheduledTime).SetPage(parameter.Page).ToList();
                }
                else
                {
                    return query.OrderByDescending(e => e.ID).SetPage(parameter.Page).ToList();
                }
            }
        }

        public bool HasRight(int taskId,int userId)
        {
            using (var db = GetDbContext())
            {
                return db.UserTasks.Any(e => e.TaskID == taskId && e.UserID == userId);
            }
        }

        public List<User> GetUsers(int id)
        {
            using (var db = GetDbContext())
            {
                var list = db.UserTasks.Where(e => e.TaskID == id);
                var users = new List<User>();
                foreach(var item in list)
                {
                    users.Add(Core.UserManager.GetUser(item.UserID));
                }
                return users;
            }
        }

        public Task GetModel(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Tasks.FirstOrDefault(e => e.ID == id);
            }
        }

        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Tasks.FirstOrDefault(e => e.ID == id);
                if(entity !=null)
                {
                    entity.Deleted = true;
                    db.SaveChanges();
                }
            }
        }
    }
}

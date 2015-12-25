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

        public bool HasRight(int taskId, int userId)
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
                foreach (var item in list)
                {
                    users.Add(Core.UserManager.GetUser(item.UserID));
                }
                return users;
            }
        }

        public Task GetModel(int id)
        {
            if (id == 0) return null;
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
                if (entity != null)
                {
                    entity.Deleted = true;
                    db.SaveChanges();
                }
            }
        }

        public void Save(Task model)
        {
            using (var db = GetDbContext())
            {
                if (model.ID > 0)
                {
                    var entity = db.Tasks.FirstOrDefault(e => e.ID == model.ID);
                    if (entity != null)
                    {
                        db.Entry(entity).CurrentValues.SetValues(model);
                    }
                    //如果重新编辑了任务，则删除之前分配的项
                    var oldUsers = db.UserTasks.Where(e => e.TaskID == model.ID);
                    db.UserTasks.RemoveRange(oldUsers);
                    //拷贝的任务一并更新
                    var copies = db.Tasks.Where(e => e.ParentID == model.ID);
                    foreach(var item in copies)
                    {
                        item.Title = model.Title;
                        item.Content = model.Content;
                        item.ScheduledTime = model.ScheduledTime;
                    }
                }
                else
                {
                    db.Tasks.Add(model);
                }

                db.SaveChanges();
                
                db.UserTasks.AddRange(model.Users.Select(e => new UserTask
                {
                    TaskID = model.ID,
                    UserID = e.ID,
                    CompletedTime = model.ScheduledTime,
                }));

                db.SaveChanges();
            }

        }
    }
}

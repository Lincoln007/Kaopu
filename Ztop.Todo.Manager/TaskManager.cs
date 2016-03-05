using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{

    public class TaskManager : ManagerBase
    {
        public List<UserTaskView> GetUserTasks(TaskQueryParameter parameter)
        {
            List<UserTaskView> list = null;
            using (var db = GetDbContext())
            {
                var query = db.UserTaskViews.AsQueryable();
                if (parameter.CreatorID > 0)
                {
                    query = query.Where(e => e.CreatorID == parameter.CreatorID);
                }
                if (parameter.UserID > 0)
                {
                    query = query.Where(e => e.UserID == parameter.UserID);
                }
                if (!string.IsNullOrEmpty(parameter.SearchKey))
                {
                    query = query.Where(e => e.Title.Contains(parameter.SearchKey.Trim()));
                }
                if (parameter.IsCompleted.HasValue)
                {
                    if (parameter.IsCompleted.Value)
                    {
                        query = query.Where(e => e.CompletedTime.HasValue);
                    }
                    else
                    {
                        query = query.Where(e => e.CompletedTime == null);
                    }
                }
                if (parameter.BeginTime.HasValue)
                {
                    query = query.Where(e => e.CreateTime > parameter.BeginTime.Value);
                }
                if (parameter.EndTime.HasValue)
                {
                    query = query.Where(e => e.CreateTime < parameter.EndTime.Value);
                }
                if (parameter.HasRead.HasValue)
                {
                    query = query.Where(e => e.HasRead == parameter.HasRead.Value);
                }
                if (parameter.Order == UserTaskOrder.ScheduleTime)
                {
                    query = query.Where(e => e.ScheduledTime.HasValue);
                    list = query.OrderBy(e => e.ScheduledTime).SetPage(parameter.Page).ToList();
                }
                else
                {
                    list = query.OrderByDescending(e => e.ID).SetPage(parameter.Page).ToList();
                }
                foreach (var item in list)
                {
                    item.CreatorName = Core.UserManager.GetUser(item.CreatorID).DisplayName;
                }
            }
            return list;
        }

        public Task GetNewTask(int userId, DateTime minTime)
        {
            using (var db = GetDbContext())
            {
                var userTask = db.UserTasks.Where(e => e.UserID == userId
                && e.CompletedTime == null
                && e.HasRead == false
                && e.CreateTime > minTime)
                .OrderByDescending(e => e.ID).FirstOrDefault();
                if (userTask != null)
                {
                    var task = db.Tasks.FirstOrDefault(e => e.ID == userTask.TaskID);
                    task.CreatorName = Core.UserManager.GetUser(task.CreatorID).DisplayName;
                    return task;
                }
            }
            return null;
        }

        public bool HasRight(int taskId, int userId)
        {
            using (var db = GetDbContext())
            {
                return db.UserTasks.Any(e => e.TaskID == taskId && e.UserID == userId);
            }
        }

        public List<UserTask> GetUserTasks(int id)
        {
            using (var db = GetDbContext())
            {
                var list = db.UserTasks.Where(e => e.TaskID == id).ToList();
                var users = new List<User>();
                foreach (var item in list)
                {
                    item.User = Core.UserManager.GetUser(item.UserID);
                }
                return list;
            }
        }

        public List<User> GetUsers(int taskId)
        {
            using (var db = GetDbContext())
            {
                return db.UserTasks
                    .Where(e => e.TaskID == taskId).Select(e => e.UserID)
                    .ToArray()
                    .Select(id => Core.UserManager.GetUser(id)).ToList();
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


        public void ReadTask(int taskId, int userId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.UserTasks.FirstOrDefault(e => e.TaskID == taskId && e.UserID == userId);
                if (entity != null)
                {
                    if (entity.HasRead) return;
                    entity.HasRead = true;
                    db.SaveChanges();
                }
                else
                {
                    throw new HttpException("参数错误或权限不足");
                }
            }
        }

        public void Delete(int userTaskId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.UserTasks.FirstOrDefault(e => e.ID == userTaskId);
                if (entity != null)
                {
                    if (entity.IsCompleted)
                    {
                        throw new Exception("任务已完成，无法删除");
                    }
                    entity.Deleted = true;
                    if (db.UserTasks.Any(e => e.TaskID == entity.TaskID && e.Deleted == false))
                    {
                        var task = db.Tasks.FirstOrDefault(e => e.ID == entity.TaskID);
                        task.Deleted = true;
                    }

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
                    foreach (var item in copies)
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

                //保存参与人员，移除重复数据
                db.UserTasks.AddRange(model.Users.GroupBy(e => e.ID).ToDictionary(g => g.Key, g => g.First()).Select(e => new UserTask
                {
                    TaskID = model.ID,
                    UserID = e.Value.ID,
                }));

                db.SaveChanges();
            }

        }

        public void CompleteTask(int taskId, int userId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.UserTasks.FirstOrDefault(e => e.TaskID == taskId && e.UserID == userId);
                
                if (entity != null && !entity.CompletedTime.HasValue)
                {
                    entity.CompletedTime = DateTime.Now;
                }
                db.SaveChanges();
            }
        }
    }
}

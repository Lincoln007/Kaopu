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
                if (!string.IsNullOrEmpty(parameter.SearchKey))
                {
                   
                    var user = db.Users.FirstOrDefault(e => e.Username.Contains(parameter.SearchKey) || e.RealName.Contains(parameter.SearchKey));
                    if (user != null)
                    {
                        if (parameter.IsCreator)
                        {
                            parameter.CreatorID = user.ID;
                        }
                        else
                        {
                            parameter.ReceiverID = user.ID;
                        }
                    }
                    else
                    {
                        query = query.Where(e => e.Title.Contains(parameter.SearchKey.Trim()));
                    }
                }
                if (!string.IsNullOrEmpty(parameter.TitleKey))
                {
                    query = query.Where(e => e.Title.Contains(parameter.TitleKey.Trim()));
                }

                if (!string.IsNullOrEmpty(parameter.ContentKey))
                {
                    query = query.Where(e => e.Content.Contains(parameter.ContentKey.Trim()));
                }
                if (parameter.CreatorId2.HasValue)
                {
                    query = query.Where(e => e.CreatorID == parameter.CreatorId2.Value);
                }
                if (parameter.ReceiverId2.HasValue)
                {
                    query = query.Where(e => e.UserID == parameter.ReceiverId2.Value);
                }

                if (parameter.CreatorID > 0)
                {
                    query = query.Where(e => e.CreatorID == parameter.CreatorID);
                }
                if (parameter.ReceiverID > 0)
                {
                    query = query.Where(e => e.UserID == parameter.ReceiverID);
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
                if (parameter.Days > 0)
                {
                    var beginTime = DateTime.Today.AddDays(-parameter.Days);
                    var endTime = DateTime.Now;
                    query = query.Where(e => e.CreateTime > beginTime && e.CreateTime < endTime);
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
                    if (parameter.GetCreator)
                    {
                        item.Creator = Core.UserManager.GetUser(item.CreatorID);
                    }
                    if (parameter.GetReceiver)
                    {
                        item.Receiver = Core.UserManager.GetUser(item.UserID);
                    }
                }
            }
            return list;
        }

        public UserTask GetNewTask(int userId, DateTime minTime)
        {
            using (var db = GetDbContext())
            {
                var model = db.UserTasks.Where(e => e.UserID == userId
                && e.CompletedTime == null
                && e.HasRead == false
                && e.CreateTime > minTime)
                .OrderByDescending(e => e.ID).FirstOrDefault();
                if (model != null)
                {
                    model.Task = GetTask(model.TaskID);
                }
                return model;
            }
        }

        public bool HasRight(int taskId, int userId)
        {
            using (var db = GetDbContext())
            {
                return db.UserTaskViews.Any(e => e.TaskID == taskId && (e.UserID == userId || e.CreatorID == userId));
            }
        }

        public List<UserTask> GetUserTasks(int taskId, bool getUserInfo = true)
        {
            using (var db = GetDbContext())
            {
                var list = db.UserTasks.Where(e => e.TaskID == taskId).ToList();
                if (getUserInfo)
                {
                    var users = new List<User>();
                    foreach (var item in list)
                    {
                        item.User = Core.UserManager.GetUser(item.UserID);
                    }
                }
                return list;
            }
        }

        public UserTask GetUserTask(int id)
        {
            using (var db = GetDbContext())
            {
                var model = db.UserTasks.FirstOrDefault(e => e.ID == id);
                model.Task = GetTask(model.TaskID);
                model.User = Core.UserManager.GetUser(model.UserID);
                return model;
            }
        }

        public List<User> GetTaskUsers(int taskId)
        {
            using (var db = GetDbContext())
            {
                return db.UserTasks
                    .Where(e => e.TaskID == taskId).Select(e => e.UserID)
                    .ToArray()
                    .Select(id => Core.UserManager.GetUser(id)).ToList();
            }
        }

        public Task GetTask(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                var model = db.Tasks.FirstOrDefault(e => e.ID == id);
                model.Creator = Core.UserManager.GetUser(model.CreatorID);
                return model;
            }
        }


        public void FlagUserTaskRead(int userTaskId, int userId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.UserTasks.FirstOrDefault(e => e.ID == userTaskId && e.UserID == userId);
                if (entity != null)
                {
                    if (entity.HasRead) return;
                    entity.HasRead = true;
                    db.SaveChanges();
                }
            }
        }

        public void Delete(int userTaskId, int creatorId)
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
                    var task = db.Tasks.FirstOrDefault(e => e.ID == entity.TaskID);
                    if (task != null && task.CreatorID == creatorId)
                    {
                        entity.Deleted = true;
                        if (db.UserTasks.Any(e => e.TaskID == entity.TaskID && e.Deleted == false))
                        {
                            task.Deleted = true;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new HttpException(401, "你没有权限删除该任务");
                    }
                }
                else
                {
                    throw new ArgumentException("参数错误");
                }
            }
        }

        public void Save(Task model, List<User> receivers)
        {
            using (var db = GetDbContext())
            {
                var isAdd = model.ID == 0;
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

                var userIds = receivers.GroupBy(e => e.ID).Select(g => g.Key).ToArray();
                //保存参与人员，移除重复数据
                db.UserTasks.AddRange(userIds.Select(userId => new UserTask
                {
                    TaskID = model.ID,
                    UserID = userId,
                }));

                db.SaveChanges();

                if (isAdd)
                {
                    //添加通知记录
                    Core.NotificationManager.Add(model);
                }
            }

        }

        public void CompleteTask(int userTaskId, int userId)
        {
            using (var db = GetDbContext())
            {
                var entity = db.UserTasks.FirstOrDefault(e => e.ID == userTaskId);
                if (entity == null)
                {
                    throw new ArgumentException("参数错误");
                }
                if (entity.IsCompleted)
                {
                    return;
                }
                if (entity.UserID != userId)
                {
                    var task = db.Tasks.FirstOrDefault(e => e.ID == entity.TaskID);
                    if (task != null && task.CreatorID == userId)
                    {
                        entity.CompletedTime = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new HttpException(401, "你没有权限完成该任务");
                    }
                }
                else
                {
                    entity.CompletedTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }
}

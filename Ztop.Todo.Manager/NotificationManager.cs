using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class NotificationManager : ManagerBase
    {
        public Notification GetNewest(int userId)
        {
            using (var db = GetDbContext())
            {
                var model = db.Notifications.Where(e => e.ReceiverID == userId && !e.HasRead).OrderByDescending(e => e.ID).FirstOrDefault();
                if (model != null)
                {
                    switch (model.InfoType)
                    {
                        case InfoType.Task:
                            {
                                var task = Core.TaskManager.GetTask(model.InfoID);
                                model.Description = task.Creator.DisplayName + " 下达了 " + task.Title;
                                model.Path = "/Task/Detail/?id=" + task.ID;
                            }
                            break;
                        case InfoType.Comment:
                            {
                                var comment = Core.CommentManager.GetModel(model.InfoID);
                                var task = Core.TaskManager.GetTask(comment.TaskID);
                                model.Description = comment.User.DisplayName + " 评论了 " + task.Title;
                                model.Path = "/Task/Detail/?id=" + task.ID + "#comment-" + model.ID;
                            }
                            break;
                    }
                }
                return model;
            }
        }

        private void AddNotification(Notification model)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Notifications.FirstOrDefault(e => e.ReceiverID == model.ReceiverID && e.InfoID == model.InfoID);
                if (entity == null)
                {
                    db.Notifications.Add(model);
                    db.SaveChanges();
                }
            }
        }

        private void AddCommentNotification(Comment comment)
        {
            var task = Core.TaskManager.GetTask(comment.TaskID);
            AddNotification(new Notification
            {
                InfoID = comment.ID,
                InfoType = InfoType.Comment,
                ReceiverID = task.CreatorID,
                SenderID = comment.UserID,
            });
        }

        private void AddTaskNotification(Model.Task task)
        {
            var users = Core.TaskManager.GetTaskUsers(task.ID);
            foreach (var user in users)
            {
                AddNotification(new Notification
                {
                    InfoID = task.ID,
                    InfoType = InfoType.Comment,
                    ReceiverID = user.ID,
                    SenderID = task.CreatorID,
                });
            }
        }

        public void Add(object info)
        {
            var infoType = (InfoType)Enum.Parse(typeof(InfoType), info.GetType().Name);

            switch (infoType)
            {
                case InfoType.Comment:
                    AddCommentNotification((Comment)info);
                    break;
                case InfoType.Task:
                    AddTaskNotification((Model.Task)info);
                    break;
            }
        }

        public void FlagRead(int infoId, InfoType infoType)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Notifications.FirstOrDefault(e => e.InfoID == infoId && e.InfoType == infoType);
                if (entity != null)
                {
                    entity.HasRead = true;
                    db.SaveChanges();
                }
            }
        }
    }
}

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
                                var userTask = Core.TaskManager.GetUserTask(model.InfoID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = sender.DisplayName + " 下达了 " + userTask.Task.Title;
                                model.Path = "/Task/Detail/?id=" + userTask.ID;
                            }
                            break;
                        case InfoType.Comment:
                            {
                                var userTask = Core.TaskManager.GetUserTask(model.InfoID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = sender.DisplayName + " 评论了 " + userTask.Task.Title;
                                model.Path = "/Task/Detail/?id=" + userTask.ID + "#comment-" + model.ID;
                            }
                            break;
                        case InfoType.Sheet:
                            {
                                var sheet = Core.SheetManager.GetModel(model.InfoID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = sender.DisplayName + "提交了报销";
                                model.Path = "/Report/Detail/?id=" + sheet.ID;
                            }
                            break;
                        case InfoType.Review:
                            {

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
                db.Notifications.Add(model);
                db.SaveChanges();
                //var entity = db.Notifications.FirstOrDefault(e => e.ReceiverID == model.ReceiverID && e.InfoID == model.InfoID);
                //if (entity == null)
                //{
                //    db.Notifications.Add(model);
                //    db.SaveChanges();
                //}
            }
        }

        private void AddCommentNotification(Comment comment)
        {
            var userTask = Core.TaskManager.GetUserTask(comment.UserTaskID);
            var userIDs = Core.TaskManager.GetUserTasks(userTask.TaskID).Where(e => e.UserID != comment.UserID).Select(e => e.UserID).ToList();
            if (!userIDs.Contains(userTask.Task.CreatorID))
            {
                userIDs.Add(userTask.Task.CreatorID);
            }
            foreach(var userid in userIDs)
            {
                AddNotification(new Notification
                {
                    InfoID = userTask.ID,
                    InfoType = InfoType.Comment,
                    ReceiverID = userid,
                    SenderID = comment.UserID
                });
            }
            //AddNotification(new Notification
            //{
            //    InfoID = userTask.ID,
            //    InfoType = InfoType.Comment,
            //    ReceiverID = userTask.Task.CreatorID,
            //    SenderID = comment.UserID,
            //});
        }

        private void AddTaskNotification(Model.Task task)
        {
            var userTasks = Core.TaskManager.GetUserTasks(task.ID, false);
            foreach (var ut in userTasks)
            {
                AddNotification(new Notification
                {
                    InfoID = ut.ID,
                    InfoType = InfoType.Task,
                    ReceiverID = ut.UserID,
                    SenderID = task.CreatorID
                });
            }
        }
        private void AddSheetNotification(Model.Sheet sheet)
        {
            var receiver = Core.UserManager.GetUser(sheet.Controler);
            var sender = Core.UserManager.GetUser(sheet.Name);
            AddNotification(new Notification
            {
                InfoID = sheet.ID,
                InfoType = InfoType.Sheet,
                ReceiverID = receiver.ID,
                SenderID = sender.ID
            });
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
                case InfoType.Sheet:
                    AddSheetNotification((Model.Sheet)info);
                    break;
            }
        }

        public void FlagRead(int infoId, int receiverID)
        {
            using (var db = GetDbContext())
            {
                var entitys = db.Notifications.Where(e => e.InfoID == infoId&&e.ReceiverID==receiverID&&!e.HasRead);
                foreach(var entity in entitys)
                {
                    entity.HasRead = true;
                }

                db.SaveChanges();

                //var entity = db.Notifications.FirstOrDefault(e => e.InfoID == infoId && e.InfoType == infoType);
                //if (entity != null)
                //{
                //    entity.HasRead = true;
                //    db.SaveChanges();
                //}
            }
        }
    }
}

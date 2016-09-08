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
                                model.Path = "/Report/Detail/?id=" + sheet.ID+"&&infoType="+InfoType.Sheet;
                            }
                            break;
                        case InfoType.Verify:
                            {
                                var verify = Core.VerifyManager.Get(model.InfoID);
                                var sheet = Core.SheetManager.GetModel(verify.SID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = sender.DisplayName + (verify.Position==Position.Check?"审核通过了":"审核退回") + sheet.PrintNumber;
                                model.Path = "/Report/Detail/?id=" + sheet.ID+"&&infoType="+InfoType.Verify+"&&verifyid="+verify.ID;
                            }
                            break;
                        case InfoType.Invoice:
                            {
                                var invoice = Core.InvoiceManager.Get(model.InfoID);
                                var contract = Core.ContractManager.Get(invoice.CID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = string.Format("{0}申请给{1}开具{2}的发票", sender.DisplayName, invoice.OtherSideCompany, invoice.Content);
                                model.Path = "/Finance/Detail/?id=" + contract.ID;
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
            var receiver = Core.UserManager.UserGet(sheet.Controler);
            var sender = Core.UserManager.UserGet(sheet.Name);
            AddNotification(new Notification
            {
                InfoID = sheet.ID,
                InfoType = InfoType.Sheet,
                ReceiverID = receiver.ID,
                SenderID = sender.ID
            });
        }

        private void AddVerifyNotification(Model.Verify verify)
        {
            var sender = Core.UserManager.UserGet(verify.Name);
            var sheet = Core.SheetManager.GetModel(verify.SID);
            var receiver = Core.UserManager.UserGet(sheet.Controler);
            AddNotification(new Notification()
            {
                InfoID = verify.ID,
                InfoType = InfoType.Verify,
                ReceiverID = receiver.ID,
                SenderID = sender.ID
            });
        }

        private void AddInvoiceNotification(Invoice invoice)
        {
            var sender = Core.UserManager.UserGet(invoice.Applicant);
            var recevier = Core.UserManager.UserGet(System.Configuration.ConfigurationManager.AppSettings["FINANCE"]);
            AddNotification(new Notification()
            {
                InfoID = invoice.ID,
                InfoType = InfoType.Invoice,
                ReceiverID = recevier.ID,
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
                    AddSheetNotification((Sheet)info);
                    break;
                case InfoType.Verify:
                    AddVerifyNotification((Verify)info);
                    break;
                case InfoType.Invoice:
                    AddInvoiceNotification((Invoice)info);
                    break;
            }
        }

        public void FlagRead(int infoId, int receiverID)
        {
            FlagReadBase(infoId, receiverID, InfoType.Comment);
            FlagReadBase(infoId, receiverID, InfoType.Task);
            //using (var db = GetDbContext())
            //{
            //    var entitys = db.Notifications.Where(e => e.InfoID == infoId&&e.ReceiverID==receiverID&&!e.HasRead);
            //    foreach(var entity in entitys)
            //    {
            //        entity.HasRead = true;
            //    }

            //    db.SaveChanges();
            //}
        }

        private void FlagReadBase(int infoId,int receiverID,InfoType infoType)
        {
            using (var db = GetDbContext())
            {
                var entrys = db.Notifications.Where(e => e.InfoID == infoId && e.ReceiverID == receiverID && e.InfoType == infoType && !e.HasRead);
                foreach (var entry in entrys)
                {
                    entry.HasRead = true;
                }
                db.SaveChanges();
            }
        }

        public void FlagSheetRead(int infoId,int receiverID)
        {
            FlagReadBase(infoId, receiverID, InfoType.Sheet);
        }

        public void FlagVerifyRead(int infoId,int receiverID)
        {
            FlagReadBase(infoId, receiverID, InfoType.Verify);
        }



    }
}

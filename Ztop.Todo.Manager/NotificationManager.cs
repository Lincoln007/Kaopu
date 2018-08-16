using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class NotificationManager : ManagerBase
    {

        public int HasAllRead(int userId)
        {
            var count = 0;
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (MySqlConnection connection=new MySqlConnection(connectionString))
            {
                var sqlText = string.Format("UPDATE notification SET HasRead=1 where ReceiverID = {0} AND HasRead = 0", userId);
                connection.Open();
                using(MySqlCommand command=new MySqlCommand(sqlText, connection))
                {
                    count = command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return count;
        }
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
                                model.Path = "/Finance/InvoiceDetail?id=" + invoice.ID;
                            }
                            break;
                        case InfoType.ProjectRegister:
                            {
                                var project = Core.ProjectManager.Get(model.InfoID);
                                var sender = Core.UserManager.GetUser(model.SenderID);
                                model.Description = string.Format("{0}录入项目：{1}，请为该项目登记", sender.DisplayName, project.Name);
                                model.Path = "/Project/Home/Detail?id=" + project.ID;
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
        private void AddProjectNotification(Project project)
        {
            var charge = project.Charge;
            if (charge != null)
            {
                var sender = charge.User;
                if (sender != null)
                {
                    var recevier = Core.UserManager.UserGet(ProjectHelper.Register);
                    AddNotification(new Notification()
                    {
                        InfoID = project.ID,
                        InfoType = InfoType.ProjectRegister,
                        ReceiverID = recevier.ID,
                        SenderID = sender.ID
                    });
                }
            }
        }

        public void Add(object info)
        {
            var name = info.GetType().Name;
            var index = name.IndexOf("_");
            if (index > 0)
            {
                name = name.Substring(0, index);
            }
            var infoType = (InfoType)Enum.Parse(typeof(InfoType), name);

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
                case InfoType.ProjectRegister:
                    AddProjectNotification((Project)info);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="receiverID"></param>
        public void FlagVerifyRead(int infoId,int receiverID)
        {
            FlagReadBase(infoId, receiverID, InfoType.Verify);
        }

        /// <summary>
        /// 作用：将某一张发票信息的提示 设置为已读
        /// 作者：汪建龙
        /// 编写时间：2016年11月20日14:07:56
        /// </summary>
        /// <param name="infoId">发票ID</param>
        public void FlagInvoiceRead(int infoId)
        {
            var receiver = Core.UserManager.UserGet(System.Configuration.ConfigurationManager.AppSettings["FINANCE"]);
            if (receiver != null)
            {
                FlagReadBase(infoId, receiver.ID, InfoType.Invoice);
            }
        }


        public void FlagRead(int notid)
        {
            if (notid == 0) return;
            using (var db = GetDbContext())
            {
                var entry = db.Notifications.Find(notid);
                if (entry != null)
                {
                    entry.HasRead = true;
                }
                db.SaveChanges();
            }
        }

        



    }
}

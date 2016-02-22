using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class AttachmentManager : ManagerBase
    {
        private static string _uploadDir;

        static AttachmentManager()
        {
            _uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["upload_floder"] ?? "upload_files");
        }

        public List<Attachment> GetList(int taskId)
        {
            using (var db = GetDbContext())
            {
                return db.Attachments.Where(e => e.TaskID == taskId).ToList();
            }
        }

        public void Upload(HttpPostedFileBase file, int taskId)
        {
            if (!Directory.Exists(_uploadDir))
            {
                Directory.CreateDirectory(_uploadDir);
            }

            var newFileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);

            file.SaveAs(Path.Combine(_uploadDir, newFileName));

            using (var db = GetDbContext())
            {
                db.Attachments.Add(new Attachment
                {
                    FileName = file.FileName,
                    FileSize = file.ContentLength,
                    SavePath = newFileName,
                    TaskID = taskId
                });
                db.SaveChanges();
            }
        }
        public void Upload(string AddedFile,int taskId)
        {
            System.IO.FileInfo file = new FileInfo(AddedFile);
            if (file.Exists)
            {
                using (var db = GetDbContext())
                {
                    db.Attachments.Add(new Attachment
                    {
                        FileName = System.IO.Path.GetFileName(AddedFile),
                        FileSize = (int)file.Length,
                        SavePath = AddedFile,
                        TaskID = taskId
                    });
            }
            }
            
        }

        public byte[] GetFileData(Attachment model)
        {
            var filePath = Path.Combine(_uploadDir, model.SavePath);
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// TODO 没做权限验证
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Attachments.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    try
                    {
                        //因为有任务拷贝，所以不物理删除附件
                        //File.Delete(Path.Combine(_uploadDir, entity.SavePath));
                        db.Attachments.Remove(entity);
                        db.SaveChanges();
                    }
                    catch { }
                }
            }
        }

        public Attachment GetModel(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Attachments.FirstOrDefault(e => e.ID == id);
            }
        }

        /// <summary>
        /// 复制附件
        /// </summary>
        public void Copy(int orignalId, int targetId)
        {
            using (var db = GetDbContext())
            {
                var list = db.Attachments.Where(e => e.TaskID == orignalId);
                foreach (var item in list)
                {
                    db.Attachments.Add(new Attachment
                    {
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        SavePath = item.SavePath,
                        TaskID = targetId
                    });
                }
                db.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
        private static string _uploadDir = System.Configuration.ConfigurationManager.AppSettings["upload_floder"] ?? "upload_files";

        public List<Attachment> GetList(int taskId)
        {
            using (var db = GetDbContext())
            {
                return db.Attachments.Where(e => e.TaskID == taskId).ToList();
            }
        }

        public void Upload(HttpPostedFileBase file, int taskId)
        {
            if (Directory.Exists(_uploadDir))
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
            }
        }

        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entity = db.Attachments.FirstOrDefault(e => e.ID == id);
                if(entity != null)
                {
                    try
                    {
                        File.Delete(Path.Combine(_uploadDir, entity.SavePath));
                        db.Attachments.Remove(entity);
                        db.SaveChanges();
                    }
                    catch { }
                }
            }
        }
    }
}

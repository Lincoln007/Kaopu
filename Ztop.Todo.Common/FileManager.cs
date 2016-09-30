using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ztop.Todo.Common
{
    public static class FileManager
    {
        private static string _uploadDir { get; set; }
        private static string _folder { get; set; }
        static FileManager()
        {
            _folder = ConfigurationManager.AppSettings["upload_floder"] ?? "upload_files";
            _uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _folder);
        }

        public static  string Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0) return string.Empty;
            if (!Directory.Exists(_uploadDir))
            {
                Directory.CreateDirectory(_uploadDir);
            }
            var newFileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
            var saveFileFullPath = Path.Combine(_uploadDir, newFileName);
            file.SaveAs(saveFileFullPath);
            return Path.Combine(_folder, newFileName);

        }
    }
}

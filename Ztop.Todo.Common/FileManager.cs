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
        private static string _iPadFolder { get; set; }
        private static string _iPadDir { get; set; }
        private static string _contractFolder { get; set; }
        private static string _contractDir { get; set; }
        static FileManager()
        {
            _folder = ConfigurationManager.AppSettings["upload_floder"] ?? "upload_files";
            _iPadFolder = ConfigurationManager.AppSettings["iPad_folder"] ?? "iPad_files";
            _contractFolder = ConfigurationManager.AppSettings["Contract_folder"] ?? "Contract_files";

            _uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _folder);
            _iPadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _iPadFolder);
            _contractDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _contractFolder);
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

        public static string Upload2(HttpPostedFileBase file)
        {
            return UploadBase(file, _iPadFolder, _iPadDir);
            //if (file.ContentLength == 0) return string.Empty;
            //if (!Directory.Exists(_iPadDir))
            //{
            //    Directory.CreateDirectory(_iPadDir);
            //}
            //var saveFileFullPath = Path.Combine(_iPadDir, file.FileName);
            //if (File.Exists(saveFileFullPath))
            //{
            //    var newfile = Path.GetFileNameWithoutExtension(saveFileFullPath) + "-" + DateTime.Now.Ticks.ToString();
            //    if (newfile.Length > 256)
            //    {
            //        newfile = newfile.Substring(255);
            //    }
            //    newfile = newfile + Path.GetExtension(saveFileFullPath);
            //    newfile = Path.Combine(_iPadDir, newfile);
            //    File.Copy(saveFileFullPath, newfile);
            //    File.Delete(saveFileFullPath);

            //}
            //file.SaveAs(saveFileFullPath);
            //return Path.Combine(_iPadFolder, file.FileName);

        }
        public static string UploadContract(HttpPostedFileBase file,string addFolder)
        {
            return UploadBase(file, Path.Combine(_contractFolder,addFolder), Path.Combine(_contractDir,addFolder));
        }

        public static string UploadiPadContract(HttpPostedFileBase file,string addFolder)
        {
            return UploadBase(file, Path.Combine(_iPadFolder, addFolder), Path.Combine(_iPadDir, addFolder));
        }

        private static string UploadBase(HttpPostedFileBase file,string folder,string dir)
        {
            if (file.ContentLength == 0) return string.Empty;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var saveFileFullPath = Path.Combine(dir, file.FileName);
            if (File.Exists(saveFileFullPath))
            {
                var newfile = Path.GetFileNameWithoutExtension(saveFileFullPath) + "-" + DateTime.Now.Ticks.ToString();
                if (newfile.Length > 256)
                {
                    newfile = newfile.Substring(255);
                }
                newfile = newfile + Path.GetExtension(saveFileFullPath);
                newfile = Path.Combine(dir, newfile);
                File.Copy(saveFileFullPath, newfile);
                File.Delete(saveFileFullPath);

            }
            file.SaveAs(saveFileFullPath);
            return Path.Combine(folder, file.FileName);
        }
    }
}

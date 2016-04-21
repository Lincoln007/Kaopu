using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Ztop.Todo.Common
{
    public static class FTPHelper
    {
        private static string FTPIP { get; set; }
        private static string UserName { get; set; }
        private static string Password { get; set; }
        private static string FTPDirectory { get; set; }
        public static string WebRequestMethods { get; private set; }

        static FTPHelper()
        {
            FTPIP = System.Configuration.ConfigurationManager.AppSettings["FTPIP"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["FTPName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["FTPPassword"];
            FTPDirectory = System.Configuration.ConfigurationManager.AppSettings["FTPDIR"];
        }
        private static System.Net.FtpWebRequest GetRequest(string URL)
        {
            return GetRequest(URL, UserName, Password);
        }
        private static System.Net.FtpWebRequest GetRequest(string URL,string UserName,string Password)
        {
            System.Net.FtpWebRequest result = System.Net.FtpWebRequest.Create(URL) as System.Net.FtpWebRequest;
            result.Credentials = new System.Net.NetworkCredential(UserName, Password);
            result.KeepAlive = false;
            return result;
        }
        public static Dictionary<string,string> GetUniqueDict(this List<string> files)
        {
            var dict = new Dictionary<string, string>();
            foreach(var item in files)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, string.Format("{0}-{1}{2}", System.IO.Path.GetFileNameWithoutExtension(item), DateTime.Now.Ticks.ToString(), System.IO.Path.GetExtension(item)));
                }
            }
            return dict;
        }
        public static string GetFTPFullPath(string fileName)
        {
            return System.IO.Path.Combine(FTPDirectory, fileName);
        }
        public static void UploadFile(string FilePath,string target)
        {
            FileInfo fi = new FileInfo(FilePath);
            if (!fi.Exists)
            {
                return;
            }
            //string Target = System.IO.Path.GetFileNameWithoutExtension(FilePath) +Guid.NewGuid().ToString() + System.IO.Path.GetExtension(FilePath);
            //string Target = System.IO.Path.GetFileNameWithoutExtension(FilePath) +"-"+ DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(FilePath);
            string URL = "FTP://" + FTPIP + "/" + target;
            var ftp = GetRequest(URL);
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = true;
            ftp.ContentLength = fi.Length;

            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize];
            int dataRead;
            using (var fs = fi.OpenRead())
            {
                using (Stream rs = ftp.GetRequestStream())
                {
                    do
                    {
                        dataRead = fs.Read(content, 0, BufferSize);
                        rs.Write(content, 0, dataRead);
                    } while (!(dataRead < BufferSize));
                    rs.Close();
                }
            }
            ftp = null;
            //return System.IO.Path.Combine(FTPDirectory, Target);

        }

        private static void MakeDirectory(string DirName,string HostName,string UserName,string Password)
        {
            try
            {


            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

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
        public static string UploadFile(string FilePath)
        {
            FileInfo fi = new FileInfo(FilePath);
            if (!fi.Exists)
            {
                return null;
            }
            //string Target = System.IO.Path.GetFileNameWithoutExtension(FilePath) +Guid.NewGuid().ToString() + System.IO.Path.GetExtension(FilePath);
            string Target = System.IO.Path.GetFileNameWithoutExtension(FilePath) + DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(FilePath);
            string URL = "FTP://" + FTPIP + "/" + Target;
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
                //try
                //{
                   
                //}catch(Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //}
                //finally
                //{
                //    fs.Close();
                //}
            }
            ftp = null;
            return System.IO.Path.Combine(FTPDirectory, Target);
            //Console.WriteLine("成功上传文件");

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

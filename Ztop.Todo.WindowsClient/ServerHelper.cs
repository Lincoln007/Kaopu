using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ztop.Todo.WindowsClient
{
    public class ServerHelper
    {

        public static string GetServerUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Server"];
        }

        private static DateTime _lastGetTime = DateTime.Today;

        public static string GetTaskUrl(Task task)
        {
            return GetServerUrl() + "/Task/Detail?id=" + task.ID;
        }

        public static Task GetNewTask(string UserName,string Password)
        {
            string url = GetServerUrl() + "/Task/GetNewTask?lastGetTime=" + _lastGetTime.GetDateTimeFormats('s')[0].ToString() + "&&UserName=" + UserName + "&&Password=" + Password;
            var request = WebRequest.Create(url);
            using (var fs=new FileStream("url.txt", FileMode.Append, FileAccess.Write))
            {
                using (var sw=new StreamWriter(fs))
                {
                    sw.WriteLine(string.Format("时间：{0}  URL：{1}", DateTime.Now, url));
                }
            }

            request.Headers.Add("token", MainForm.AccessToken);
            var cred = new CredentialCache();
            cred.Add(new Uri(GetServerUrl()), "NTLM", CredentialCache.DefaultNetworkCredentials);
            request.Credentials = cred;

            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var json = sr.ReadToEnd();
                    _lastGetTime = DateTime.Now;
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Task>(json);
                }
            }
        }
    }
}

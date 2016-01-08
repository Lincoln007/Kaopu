using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ztop.Todo.WindowsClient
{
    public class ServerHelper
    {

        public static string GetServerUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Server"];
        }

        private static DateTime _lastGetTime = DateTime.Now;

        public static string GetTaskUrl(Task task)
        {
            return GetServerUrl() + "/Task/Detail?id=" + task.ID;
        }

        public static Task GetNewTask()
        {
            var request = System.Net.WebRequest.Create(GetServerUrl() + "/Task/GetNewTask?lastGetTime=" + _lastGetTime.ToString());
            request.Headers.Add("token", MainForm.AccessToken);
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

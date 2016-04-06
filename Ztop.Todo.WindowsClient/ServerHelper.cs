using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Ztop.Todo.Common;

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


        public static Task GetNewTask()
        {
            var url = GetServerUrl() + "/Task/GetNewTask?lastGetTime=" + _lastGetTime.ToString("yyyy-MM-ddTHH:mm:ss");

            var request = WebRequest.Create(url);

            request.Headers.Add("token", LoginHelper.GetToken());

            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var json = sr.ReadToEnd();
                    _lastGetTime = DateTime.Now;
                    try
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Task>(json);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(ex);
                        return null;
                    }
                }
            }
        }
    }
}

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

        public static string GetNotificationUrl(Notification notice)
        {
            return GetServerUrl() + "/" + notice.Path;
        }

        public static Notification GetNotification()
        {
            var url = GetServerUrl() + "/Notification/GetNewest?_=" + DateTime.Now.Ticks;

            var request = WebRequest.Create(url);

            request.Headers.Add("token", LoginHelper.GetOAToken());

            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var json = sr.ReadToEnd();
                    try
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Notification>(json);
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

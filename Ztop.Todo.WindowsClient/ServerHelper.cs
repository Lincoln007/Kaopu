using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.WindowsClient
{
    public class ServerHelper
    {

        public static string GetServerUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Server"];
        }
        public static string GetReportServerUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SServer"];
        }

        public static string GetNotificationUrl(Notification notice)
        {
            var server = GetServerUrl();
            switch (notice.InfoType)
            {
                case 1:
                case 2:
                    server = GetServerUrl();
                    break;
                case 3:
                case 4:
                case 5:
                    server = GetReportServerUrl();
                    break;
            }
            return server + notice.Path;
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

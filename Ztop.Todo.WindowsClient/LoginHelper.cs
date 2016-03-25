using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.WindowsClient
{
    public class LoginHelper
    {
        private static readonly string WebServer = System.Configuration.ConfigurationManager.AppSettings["Server"];

        private static string _accessToken;

        public static string Login(string username, string password)
        {
            var url = WebServer + "/User/LoginResult?&username=" + username + "&password=" + password;
            using (var client = new WebClient())
            {
                client.Headers.Add("X-Requested-With", "XMLHttpRequest");
                client.Encoding = Encoding.UTF8;
                try
                {

                    var responseText = client.UploadString(url, "POST", "");
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, JToken>>(responseText);

                    if (data.ContainsKey("data"))
                    {
                        var user = data["data"].ToObject<Dictionary<string, object>>();
                        if (user.ContainsKey("AccessToken"))
                        {
                            return _accessToken = user["AccessToken"].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        private static readonly string TokenFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ZtopTodo";

        private static readonly string TokenPath = TokenFolder + "\\token.txt";
        static LoginHelper()
        {
            if (!Directory.Exists(TokenFolder))
            {
                Directory.CreateDirectory(TokenFolder);
            }
        }


        public static void Remeber(string token)
        {
            File.WriteAllText(TokenPath, token);
        }

        public static string GetToken()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                if (File.Exists(TokenPath))
                {
                    _accessToken = File.ReadAllText(TokenPath);
                }
            }
            return _accessToken;
        }

        public static void Logout()
        {
            _accessToken = null;
            try
            {
                File.Delete(TokenFolder);
            }
            catch { }
        }
    }
}

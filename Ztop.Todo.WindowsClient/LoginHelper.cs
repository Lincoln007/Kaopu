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
        private static readonly string SheetServer = System.Configuration.ConfigurationManager.AppSettings["SServer"];

        /// <summary>
        /// 任务系统
        /// </summary>
        private static string _accessToken;

        /// <summary>
        /// 报销系统
        /// </summary>
        private static string _saccessToken;

        private static string Login(string username,string password,string server)
        {
            var url = server + "/User/LoginResult?&username=" + username + "&password=" + password;
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

        /// <summary>
        /// 任务系统登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string OALogin(string username, string password)
        {
            return Login(username, password, WebServer);
        }

        /// <summary>
        /// 报销系统登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ReimburseLogin(string username,string password)
        {
            return Login(username, password, SheetServer);
        }

        private static readonly string TokenFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ZtopTodo\\";
        /// <summary>
        /// 任务系统登录状态记录文件
        /// </summary>
        private static readonly string TokenPath = TokenFolder + "token.txt";
        /// <summary>
        /// 报销系统登录状态记录文件路径
        /// </summary>
        private static readonly string STokenPath = TokenFolder + "stoken.txt";

        static LoginHelper()
        {
            if (!Directory.Exists(TokenFolder))
            {
                Directory.CreateDirectory(TokenFolder);
            }
        }


        /// <summary>
        /// 记住密码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="stoken"></param>
        public static void Remeber(string token,string stoken)
        {
            Logout();
            File.WriteAllText(TokenPath, token);
            File.WriteAllText(STokenPath, stoken);
        }

        /// <summary>
        /// 读取任务系统
        /// </summary>
        /// <returns></returns>
        public  static string GetOAToken()
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

        /// <summary>
        /// 读取报销系统
        /// </summary>
        /// <returns></returns>
        public static string GetReToken()
        {
            if (string.IsNullOrEmpty(_saccessToken))
            {
                if (File.Exists(STokenPath))
                {
                    _saccessToken = File.ReadAllText(STokenPath);
                }
            }
            return _saccessToken;
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
                File.Delete(TokenPath);
                File.Delete(STokenPath);
            }
            catch { }
        }
    }
}

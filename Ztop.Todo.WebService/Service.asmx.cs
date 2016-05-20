using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services;

namespace Ztop.Todo.WebService
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Service : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetVersion()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["version"].ConnectionString;
        }

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetUrl()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString + System.Configuration.ConfigurationManager.ConnectionStrings["directory"].ConnectionString + "/";
        }

        /// <summary>
        /// 获取下载zip压缩包
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string[] GetZips()
        {
            string folder = HttpRuntime.AppDomainAppPath + System.Configuration.ConfigurationManager.ConnectionStrings["directory"].ConnectionString;
            string[] zips = Directory.GetFileSystemEntries(folder);
            for (var i = 0; i < zips.Length; i++)
            {
                zips[i] = Path.GetFileName(zips[i]);
            }
            return zips;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ztop.Todo.Common
{
    public static class ProjectHelper
    {
        private static XmlDocument _configXml { get; set; }
        static ProjectHelper()
        {
            _configXml = new XmlDocument();
            _configXml.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["PROJECT"]));
        }

        private static List<string> Get(string selectString)
        {
            var list = new List<string>();
            var nodes = _configXml.SelectNodes(selectString);
            if (nodes != null)
            {
                for(var i = 0; i < nodes.Count; i++)
                {
                    list.Add(nodes[i].Attributes["Name"].Value);
                }
            }
            return list;
        }

        private static string GetSingle(string selectString)
        {
            var node = _configXml.SelectSingleNode(selectString);
            if (node != null)
            {
                return node.Attributes["Name"].Value;
            }
            return string.Empty;
        }

        public static List<string> GetProjects()
        {
            return Get("/Managers/Projects/Person");
        }

        public static List<string> GetFinances()
        {
            return Get("/Managers/Finances/Person");
        }

        public static List<string> GetAdmins()
        {
            return Get("/Managers/Admins/Person");
        }
        public static List<string> GetMarkerts()
        {
            return Get("/Managers/Markets/Person");
        }

        public static List<string> GetLoowoo()
        {
            return Get("/Managers/Loowoo/Person");
        }

        private static string _register { get; set; }
        /// <summary>
        /// 项目编号登记人员名称
        /// </summary>
        public static string Register { get { return string.IsNullOrEmpty(_register) ? _register = GetRegister() : _register; } }

        public static string GetRegister()
        {
            return GetSingle("/Managers/Register/Person");
        }

        private static string _master { get; set; }
        /// <summary>
        /// 项目管理管理员  大主宰
        /// </summary>
        public static string Master { get { return string.IsNullOrEmpty(_master) ? _master = GetSingle("/Managers/Master/Person") : _master; } }
    }
}

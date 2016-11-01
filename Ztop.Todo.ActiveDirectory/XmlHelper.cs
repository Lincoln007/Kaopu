using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ztop.Todo.ActiveDirectory
{
    public static class XmlHelper
    {
        private static XmlDocument _configXml { get; set; }
        static XmlHelper()
        {
            _configXml = new XmlDocument();
            _configXml.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["IGNORE"]));
        }

        public static List<string> InitConfig(string SelectString)
        {
            var list = new List<string>();
            var nodes = _configXml.SelectNodes(SelectString);
            if (nodes != null)
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    list.Add(nodes[i].Attributes["Name"].Value);
                }
            }
            return list;
        }
        private static string Get(string selectString)
        {
            var node = _configXml.SelectSingleNode(selectString);
            if (node != null)
            {
                return node.Attributes["Name"].Value;
            }
            return string.Empty;
        }

        public static List<string> GetDirectors()
        {
            return InitConfig("/Composes/Directors/Director");
        }
        public static string GetManager()
        {
            return Get("/Composes/Directors/Manager");
        }
        public static string GetFinance()
        {
            return Get("/Composes/Directors/Finance");
        }

        public static List<string> GetBanks()
        {
            return InitConfig("/Composes/Directors/Bank");
        }

        public static List<string> GetDitrict()
        {
            var list = new List<string>();
            var nodes = _configXml.SelectNodes("/Composes/Citys/City");
            if (nodes != null)
            {
                for(var i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    list.Add(string.Format("{0}{1}", node.Attributes["Code"].Value, node.Attributes["Name"].Value));
                }
            }

            return list;
        }
    }
}

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

        public static List<string> GetDirectors()
        {
            return InitConfig("/Composes/Directors/Director");
        }
    }
}

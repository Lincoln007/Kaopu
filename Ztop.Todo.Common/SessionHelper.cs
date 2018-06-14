using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ztop.Todo.Common
{
    public class SessionHelper
    {
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public static void SetSession(string name,object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static void RemoveSession(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        public static void RemoveAllSession()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}

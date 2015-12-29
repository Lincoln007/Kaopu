using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class HtmlUtils
    {
        public static string ToHtml(string content)
        {
            if(string.IsNullOrEmpty(content))
            {
                return null;
            }
            return content.Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }
    }
}

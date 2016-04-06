using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ztop.Todo.Common
{
    public class LogHelper
    {
        public static Exception GetInnerException(Exception ex)
        {
            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                return GetInnerException(innerEx);
            }
            return ex;
        }

        private static string GetExceptionLogContent(Exception ex)
        {
            var content = new StringBuilder();
            content.AppendLine(ex.Message);
            content.AppendLine(ex.StackTrace);
            content.AppendLine(ex.Source);
            if (ex.InnerException != null)
            {
                content.AppendLine(GetExceptionLogContent(ex.InnerException));
            }
            return content.ToString();
        }

        public static void WriteLog(Exception ex)
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                var content = GetExceptionLogContent(ex);
                if (HttpContext.Current != null)
                {
                    var request = HttpContext.Current.Request;
                    content = request.Url.AbsoluteUri + "\r\n" + content;
                }
                File.WriteAllText(Path.Combine(logPath, ex.GetType().Name + DateTime.Now.Ticks.ToString() + ".txt"), content);
            }
            catch
            {

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ztop.Todo.Manager
{
    public static class HttpDbContextContainer
    {
        private static readonly string ContextName = "";
        public static T GetDbContext<T>() where T : DbContext
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                return (T)context.Items[ContextName];
            }
            return default(T);
        }
        public static void OnBeginRequest(this HttpContext context, object dbContext)
        {
            context.Items.Add(ContextName, dbContext);
        }

        public static void OnEndRequest(this HttpContext context)
        {
            context.Items.Remove(ContextName);
        }
    }
}

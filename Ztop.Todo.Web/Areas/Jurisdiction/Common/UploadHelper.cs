using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web
{
    public static class UploadHelper
    {
        public static string[] GetValue(this HttpContextBase context,string PropertyName)
        {
            return context.Request.Form[PropertyName].Split(',');
        }
    }
}
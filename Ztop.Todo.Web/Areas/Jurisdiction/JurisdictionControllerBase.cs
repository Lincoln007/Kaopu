using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
namespace Ztop.Todo.Web.Areas.Jurisdiction
{
    [UserAuthorize]
    public class JurisdictionControllerBase : Ztop.Todo.Web.Controllers.ControllerBase
    {
        protected ActionResult HtmlResult(List<string> html)
        {
            var values = html.ListToTable();
            var str = string.Empty;
            foreach(var item in values)
            {
                var st = string.Empty;
                st += "<tr>";
                foreach(var entry in item)
                {
                    if (string.IsNullOrEmpty(entry))
                    {
                        continue;
                    }
                    st += "<td><label class='checkbox-inline'><input type='checkbox' name='Group' value='" + entry + "' />" + entry + "</label></td>";
                }
                st += "</tr>";
                str += st;
            }
            return Content(str);
        }
    }
}
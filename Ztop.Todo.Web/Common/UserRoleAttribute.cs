using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Common
{
    public class UserRoleAttribute:System.Web.Mvc.ActionFilterAttribute
    {
        public UserRoleAttribute()
        {
            groupType = GroupType.Guest;
        }

        public GroupType groupType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (groupType == GroupType.Guest)
            {
                return;
            }

            var currentUser = (UserIdentity)Thread.CurrentPrincipal.Identity;
            if (currentUser.GroupType == groupType)
            {
                throw new HttpException(401, "您没有权限查看此页面");
            }
            return;
            
        }
    }
}
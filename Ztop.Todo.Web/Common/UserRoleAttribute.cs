﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web
{
    public class UserRoleAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public UserRoleAttribute()
        {
            Role = Role.Everyone;
        }

        public Role Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (Role == Role.Everyone)
            {
                return;
            }

            var currentUser = (UserIdentity)Thread.CurrentPrincipal.Identity;

            //if (currentUser == null)
            //{
            //    filterContext.HttpContext.Response.Redirect("/", true);
            //    return;
            //}

            if (currentUser.Role != Role)
            {
                throw new HttpException(401, "你没有权限查看此页面");
            }

            return;
        }
    }
}
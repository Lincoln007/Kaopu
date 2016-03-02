﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
namespace Ztop.Todo.Web.Common
{
    public class UserPrincipal:System.Security.Principal.IPrincipal
    {
        public UserPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
    public class UserIdentity : System.Security.Principal.IIdentity
    {
        public static UserIdentity Guest = new UserIdentity();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationType { get { return "Web.Session"; } }
        public string Name { get { return UserName; } }
        public bool IsAuthenticated
        {
            get { return string.IsNullOrEmpty(UserName) ? false: true; }
        }
    }
}
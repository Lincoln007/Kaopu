﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Ztop.Todo.Model;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Web.Common
{
    public class UserPrincipal : System.Security.Principal.IPrincipal
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
        public static readonly UserIdentity Guest = new UserIdentity() { GroupType = GroupType.Guest };
        public int UserID { get; set; }
        public GroupType GroupType { get; set; }
        public int GroupId { get; set; }
        public string AuthenticationType { get { return "Web.Session"; } }
        public string Name { get; set; }
        //是否为审核人
        public bool Director { get; set; }
        public bool Project { get; set; }
        public bool Finance { get; set; }
        public bool Admin { get; set; }
        public bool Market { get; set; }
        public bool iPad { get; set; }
        public string sAMAccountName { get; set; }
        public string[] Groups { get; set; }
        public bool IsAuthenticated
        {
            get { return UserID > 0; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Manager;
using Ztop.Todo.Model;
using Ztop.Todo.Web.Common;

namespace Ztop.Todo.Web
{
    public class BaseView<TModel> : WebViewPage<TModel>
    {
        public UserIdentity Identity { get; private set; }
        public List<OASystemBase> Systems { get; private set; }
        public Dictionary<string, List<PowerBase>> Items { get; set; }

        public BaseView()
        {
            Identity = Thread.CurrentPrincipal.Identity as UserIdentity;
            if (!string.IsNullOrEmpty(Identity.sAMAccountName))
            {
                Items = RedisManager.Get<Dictionary<string, List<PowerBase>>>(Identity.sAMAccountName, RedisManager.Client);

                Systems = RedisManager.Get<List<OASystemBase>>(Identity.sAMAccountName + "System", RedisManager.Client);
            }

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
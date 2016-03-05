using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;
using Ztop.Todo.Common;
using System.Web;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Manager
{
    public class AuthorizeManager:ManagerBase
    {
        public Authorize Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Authorizes.Find(id);
            }
        }
        public Authorize Get(HttpContextBase context,int id=0)
        {
            if (id == 0)
            {
                return new Authorize
                {
                    GroupName = context.Request.Form["GroupName"],
                    Manager = context.Request.Form["Manager"]
                };
            }
            var authorize = Get(id);
            if (authorize == null)
            {
                throw new ArgumentException("未找到权限表");
            }
            authorize.GroupName = context.Request.Form["GroupName"];
            return authorize;
        }
        public void Add(Authorize authorize)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Where(e => e.Manager == authorize.Manager).FirstOrDefault();
                if (entry != null)
                {
                    throw new ArgumentException(string.Format("已经授权{0}为管理者，如要增加该管理者的权限，请在权限表中增加{0}的权限",authorize.Manager));
                }
                db.Authorizes.Add(authorize);
                db.SaveChanges();
            }
        }
        public void Edit(Authorize authorize)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Authorizes.Find(authorize.ID);
                if (entry != null)
                {
                    db.Entry(entry).CurrentValues.SetValues(authorize);
                    db.SaveChanges();
                }
            }
        }
        public List<Authorize> GetList()
        {
            using (var db = GetDbContext())
            {
                return db.Authorizes.ToList();
            }
        }
        public List<string> GetList(string Name)
        {
            var authorize = GetList().FirstOrDefault(e => e.Manager == Name);
            if (authorize == null || string.IsNullOrEmpty(authorize.GroupName))
            {
                return new List<string>();
            }
            var array = authorize.GroupName.Split(',');
            var list = new List<string>();
            foreach (var item in array)
            {
                list.Add(item);
            }
            return list.OrderBy(e => e).ToList();
        }
        public List<string> GetAllManager()
        {
            return GetList().Select(e => e.Manager).ToList();
        }
        public void Screen(string[] Origin,string sAMAccountName,out List<string> None,out List<string> Have)
        {
            None = new List<string>();
            Have = new List<string>();
            foreach(var item in Origin)
            {
                if (ADController.IsMember(item, sAMAccountName))
                {
                    Have.Add(item);
                }
                else
                {
                    None.Add(item);
                }
            }
        }
    }
}

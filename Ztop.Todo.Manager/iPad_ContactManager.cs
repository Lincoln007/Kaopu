using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    /// <summary>
    /// 平板联系人
    /// </summary>
    public class iPad_ContactManager:ManagerBase
    {

        /// <summary>
        /// 作用：验证系统中是否已存在相同记录
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:14:10
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool Exist(string name,string address,string city)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Contacts.FirstOrDefault(e => e.Name == name && e.Address == address && e.City == city&&e.Delete==false);
                return entry != null;
            }
        }

        /// <summary>
        /// 作用：添加联系人到系统中  调用前需要验证是否已存在
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:17:58
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public int Add(iPadContact contact)
        {
            using (var db = GetDbContext())
            {
                contact.CreateTime = DateTime.Now;
                db.iPad_Contacts.Add(contact);
                db.SaveChanges();
                return contact.ID;
            }
        }

        /// <summary>
        /// 作用：获取联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:21:11
        /// </summary>
        /// <returns></returns>
        public List<iPadContact> Get()
        {
            using (var db = GetDbContext())
            {
                return db.iPad_Contacts.Where(e=>e.Delete==false).OrderBy(e => e.ID).ToList();
            }
        }

        /// <summary>
        /// 作用：通过ID获取联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:30:48
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public iPadContact Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.iPad_Contacts.Find(id);
            }
        }

        /// <summary>
        /// 作用：编辑联系人
        /// 作者：汪建龙
        /// 编写时间：2016年12月8日10:42:22
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool Edit(iPadContact contact)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Contacts.Find(contact.ID);
                if (entry == null)
                {
                    return false;
                }
                contact.CreateTime = entry.CreateTime;
                db.Entry(entry).CurrentValues.SetValues(contact);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 作用：删除联系人
        /// 作者:汪建龙
        /// 编写时间：2016年12月8日10:47:40
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.iPad_Contacts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Delete = true;
                db.SaveChanges();
                db.SaveChanges();
                return true;
            }
        }
    }
}

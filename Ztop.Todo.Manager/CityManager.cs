using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class CityManager:ManagerBase
    {
        /// <summary>
        /// 作用：验证数据库中是否已存在
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日13:40:27
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Exist(string name,string code)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Citys.FirstOrDefault(e => e.Name == name && e.Code == code);
                return entry != null;
            }
        }

        /// <summary>
        /// 作用：添加记录  调用之前需要验证是否数据库已存在
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日13:42:09
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public int Add(City city)
        {
            if (city == null)
            {
                return 0;
            }
            using (var db = GetDbContext())
            {
                db.Citys.Add(city);
                db.SaveChanges();
                return city.ID;
            }
        }

        /// <summary>
        /// 作用：获取所有城市信息
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日14:40:38
        /// </summary>
        /// <returns></returns>
        public List<City> GetList()
        {
            using (var db = GetDbContext())
            {
                return db.Citys.OrderBy(e => e.ID).ToList();
            }
        }
        /// <summary>
        /// 作用：通过名称获取City
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日14:09:15
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public City Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Citys.FirstOrDefault(e => e.Name == name);
            }
        }
        /// <summary>
        /// 作用：通过ID获取City
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日15:13:19
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public City Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Citys.Find(id);
            }
        }

        /// <summary>
        /// 作用：编辑城市
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日15:38:52
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool Edit(City city)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Citys.Find(city.ID);
                if (entry != null)
                {
                    entry.Name = city.Name;
                    entry.Code = city.Code;
                    entry.Remark = city.Remark;
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 作用：删除城市记录
        /// 作者：汪建龙
        /// 编写时间：2016年12月5日15:55:46
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Citys.Find(id);
                if (entry == null)
                {
                    return false;
                }
                var children = db.Citys.Where(e => e.PCID == entry.ID).ToList();
                if (children.Count >0)
                {
                    return false;
                }
                db.Citys.Remove(entry);
                db.SaveChanges();
                return true;
            }
        }
    }
}

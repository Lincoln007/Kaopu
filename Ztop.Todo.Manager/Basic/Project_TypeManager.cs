using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Project_TypeManager:ManagerBase
    {
        /// <summary>
        /// 作用：通过名称以及级别获取
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日10:58:49
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProjectType Get(string name,Degree degree,string chars)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Project_Types.FirstOrDefault(e => e.Name == name&&e.Degree==degree&&e.Chars==chars);
            }
        }
        /// <summary>
        /// 作用：通过名称和级别判断数据库中是否已存在
        /// 作者：汪建龙
        /// 编写时间:2016年12月6日11:00:28
        /// </summary>
        /// <param name="name"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public bool Exist(string name,Degree degree,string chars)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Project_Types.FirstOrDefault(e => e.Degree == degree && e.Name == name&&e.Chars==chars);
                return entry != null;
            }
        }
        /// <summary>
        /// 作用：获取所有项目类型
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日12:58:15
        /// </summary>
        /// <returns></returns>
        public List<ProjectType> Get()
        {
            return DB.Project_Types.ToList();
        }

        public List<ProjectType> GetByPPID(int ppid)
        {
            return DB.Project_Types.Where(e => e.PPID == ppid).OrderBy(e => e.Chars).ToList();
        }


        /// <summary>
        /// 作用：通过ID获取
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日13:15:06
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectType Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Project_Types.Find(id);
            }
        }

        /// <summary>
        /// 作用:保存到数据库  调用之前需要验证是否已存在
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日13:17:42
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int Add(ProjectType type)
        {
            using (var db = GetDbContext())
            {
                db.Project_Types.Add(type);
                db.SaveChanges();
                return type.ID;
            }
        }

        /// <summary>
        /// 作用：编辑项目类型
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日13:21:42
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Edit(ProjectType type)
        {
            using(var db = GetDbContext())
            {
                var entry = db.Project_Types.Find(type.ID);
                if (entry == null)
                {
                    return false;
                }
                entry.Chars = type.Chars;
                entry.Name = type.Name;
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 作用：删除项目类型
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日13:26:23
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using(var db = GetDbContext())
            {
                var entry = db.Project_Types.Find(id);
                if (entry == null)
                {
                    return false;
                }
                if (entry.Degree == Degree.First)
                {
                    var list = db.Project_Types.Where(e => e.PPID == entry.ID).ToList();
                    if (list.Count > 0)
                    {
                        return false;
                    }
                }
                db.Project_Types.Remove(entry);
                db.SaveChanges();
            }
            return true;
        }
    }
}

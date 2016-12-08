using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ArticleManager:ManagerBase
    {
        public bool Exist(Article article)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Articles.FirstOrDefault(e => e.Name == article.Name && e.OtherSide == article.OtherSide);
                return entry != null;
            }
        }
        /// <summary>
        /// 作用：添加或更新 项目洽谈
        /// 作者：汪建龙
        /// 编写时间：2016年12月6日20:35:31
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public int Save(Article article)
        {
            if (Exist(article) && article.ID > 0)
            {
                Edit(article);
                return article.ID;
            }
            if (!CheckNumber(article.Number))
            {
                throw new ArgumentException("登记编号不唯一");
            }
            using (var db = GetDbContext())
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return article.ID;
            }
        }

        /// <summary>
        /// 作用：验证登记编号 唯一性  唯一为true 不唯一：false
        /// 作者：汪建龙
        /// 编写时间：2016年12月7日09:17:47
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool CheckNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Articles.FirstOrDefault(e => e.Number == number);
                return entry == null;
            }
        }
        public int Edit(Article article)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Articles.Find(article.ID);
                if (entry != null)
                {
                    db.Entry(entry).CurrentValues.SetValues(article);
                    db.SaveChanges();
                }
                return article.ID;
            }
        }
        public Article Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Articles.FirstOrDefault(e => e.ID == id);
            }
        }
        public List<Article> Search(ArticleParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Articles.AsQueryable();
                if (parameter.Deleted.HasValue)
                {
                    query = query.Where(e => e.Deleted == parameter.Deleted.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name.Contains(parameter.Name));
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.OtherSide.Contains(parameter.OtherSide));
                }
                if (!string.IsNullOrEmpty(parameter.Remark))
                {
                    query = query.Where(e => e.Remark.Contains(parameter.Remark));
                }
                if (parameter.MinMoney.HasValue)
                {
                    query = query.Where(e => e.Money >= parameter.MinMoney.Value);
                }
                if (parameter.MaxMoney.HasValue)
                {
                    query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
                }
                query = query.OrderBy(e => e.ID).SetPage(parameter.Page);
                return query.ToList();
            }
        }
        public List<Article> GetByIDList(List<int> idlist)
        {
            var list = new List<Article>();
            foreach(var id in idlist)
            {
                var entry = Get(id);
                if (entry != null)
                {
                    list.Add(entry);
                }
            }
            return list;
        }
        public bool Deleted(int id)
        {
            using(var db = GetDbContext())
            {
                var article = db.Articles.FirstOrDefault(e => e.ID == id);
                if (article == null)
                {
                    return false;
                }
                article.Deleted = true;
                db.SaveChanges();
            }
            return true;
        }
        /// <summary>
        /// 作用：通过登记编号获得项目洽谈
        /// 作者：汪建龙
        /// 编写时间：2016年12月7日09:28:19
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Article Get(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return null;
            }
            using (var db = GetDbContext())
            {
                return db.Articles.FirstOrDefault(e => e.Number == number);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ContractArticleManager:ManagerBase
    {
        public void UpdateContract(int contractid,int[] articleid)
        {
            using (var db = GetDbContext())
            {
                var old = db.ContractArticles.Where(e => e.ContractID == contractid).ToList();
                if (old != null && old.Count > 0)
                {
                    db.ContractArticles.RemoveRange(old);
                    db.SaveChanges();
                }
                foreach(var id in articleid.Distinct())
                {
                    db.ContractArticles.Add(new ContractArticle
                    {
                        ContractID = contractid,
                        ArticleID = id
                    });
                    db.SaveChanges();
                }
            }
        }
        public void UpdateArticle(int articleid,int[] contractid)
        {
            using (var db = GetDbContext())
            {
                var old = db.ContractArticles.Where(e => e.ArticleID == articleid).ToList();
                if (old != null && old.Count > 0)
                {
                    db.ContractArticles.RemoveRange(old);
                    db.SaveChanges();
                }
                foreach(var id in contractid.Distinct())
                {
                    db.ContractArticles.Add(new ContractArticle
                    {
                        ArticleID = articleid,
                        ContractID = id
                    });
                    db.SaveChanges();
                }
            }
        }

        public List<ContractArticle> GetByContractID(int contractID)
        {
            using (var db = GetDbContext())
            {
                return db.ContractArticles.Where(e => e.ContractID == contractID).ToList();
            }
        }

        public List<ContractArticle> GetByArticleID(int articleID)
        {
            using (var db = GetDbContext())
            {
                return db.ContractArticles.Where(e => e.ArticleID == articleID).ToList();
            }
        }
    }
}

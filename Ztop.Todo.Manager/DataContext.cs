using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class DataContext : DbContext
    {
        public DataContext():base("name=DbConnection")
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

     

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        #region 任务系统

        public DbSet<Model.Task> Tasks { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }

        public DbSet<UserTaskView> UserTaskViews { get; set; }

        public DbSet<TaskQuery> TaskQueries { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        #endregion

        #region  权限系统

        public DbSet<Authorize> Authorizes { get; set; }

        public DbSet<Authorize_Fast> Authorize_Fasts { get; set; }

        public DbSet<ADGroup> AD_Groups { get; set; }

        public DbSet<FastGroupUserView> FastGroupUserViews { get; set; }

      //  public DbSet<FastADGroupView> FastADGroupViews { get; set; }

        #endregion

        #region 报销系统

        public DbSet<Sheet> Sheets { get; set; }

        public DbSet<Substancs> Substances { get; set; }

        public DbSet<Verify> Verifys { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Evection> Evections { get; set; }

        public DbSet<Errand> Errands { get; set; }

        public DbSet<Traffic> Traffics { get; set; }

        #endregion

        #region 项目管理系统

        public DbSet<ContractArticle> ContractArticles { get; set; }
        #endregion

        #region 平板管理系统

        public DbSet<iPad> iPads { get; set; }

        public DbSet<iPadInvoice> iPad_Invoices { get; set; }

        public DbSet<iPadRegister> iPad_Registers { get; set; }

        public DbSet<Register_iPad> Register_iPads { get; set; }

        public DbSet<iPadContract> iPad_Contracts { get; set; }

        public DbSet<iPadAccount> iPad_Accounts { get; set; }

        #endregion

        #region  银行对账系统
        public DbSet<BillOne> BillOnes { get; set; }

        public DbSet<Bill_Head> Bill_Heads { get; set; }

        #endregion
     

        public DbSet<DataBook> DataBooks { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<SerialNumber> SerialNumbers { get; set; }
     
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UserGroupView> UserGroupViews { get; set; }
        public DbSet<ContractFile> ContractFiles { get; set; }
        public DbSet<BillAccount> BillAccounts { get; set; }
        public DbSet<BillContract> BillContracts { get; set; }
        public DbSet<Article> Articles { get; set; }
     
       
      
      
     
    }
}

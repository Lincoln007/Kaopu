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

        public DbSet<Model.Task> Tasks { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }

        public DbSet<UserTaskView> UserTaskViews { get; set; }

        public DbSet<TaskQuery> TaskQueries { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Authorize> Authorizes { get; set; }
        public DbSet<DataBook> DataBooks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Substancs> Substances { get; set; }
        public DbSet<Verify> Verifys { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Evection> Evections { get; set; }
        public DbSet<Errand> Errands { get; set; }
        public DbSet<Traffic> Traffics { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UserGroupView> UserGroupViews { get; set; }
        public DbSet<ContractFile> ContractFiles { get; set; }
        public DbSet<BillAccount> BillAccounts { get; set; }
    }
}

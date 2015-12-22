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

        public DbSet<Model.Task> Tasks { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }
    }
}

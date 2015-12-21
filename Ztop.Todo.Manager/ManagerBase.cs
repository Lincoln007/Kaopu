using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;

namespace Ztop.Todo.Manager
{
    public class ManagerBase
    {
        protected static readonly LocalCacheService Cache = new LocalCacheService();

        protected DataContext GetDbContext()
        {
            return new DataContext();
        }
    }
}

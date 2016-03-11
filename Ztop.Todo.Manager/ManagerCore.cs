using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Manager
{
    public class ManagerCore
    {
        public static readonly ManagerCore Instance = new ManagerCore();

        private ManagerCore()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                if (p.PropertyType == this.GetType())
                {
                    continue;
                }
                var val = p.GetValue(this);
                if (val == null)
                {
                    p.SetValue(this, Activator.CreateInstance(p.PropertyType));
                }
            }
        }

        public UserManager UserManager { get; private set; }

        public TaskManager TaskManager { get; private set; }

        public CommentManager CommentManager { get; private set; }

        public AttachmentManager AttachmentManager { get; private set; }

        public AuthorizeManager AuthorizeManager { get; private set; }

        public DataBookManager DataBookManager { get; private set; }

        public MessageManager MessageManager { get; private set; }

        public QueryManager QueryManager { get; private set; }
        public SheetManager SheetManager { get; private set; }
        public SubstancsManager SubstanceManager { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;
using Ztop.Todo.Common;
using Ztop.Todo.ActiveDirectory;

namespace Ztop.Todo.Manager
{
    public class DataBookManager : ManagerBase
    {
        public DataBook Get(int ID)
        {
            using (var db = GetDbContext())
            {
                return db.DataBooks.Find(ID);
            }
        }

        public void Edit(DataBook Book)
        {
            using (var db = GetDbContext())
            {
                var entry = db.DataBooks.Find(Book.ID);
                if (entry != null)
                {
                    Book.ID = entry.ID;
                    db.Entry(entry).CurrentValues.SetValues(Book);
                    db.SaveChanges();
                }
            }
        }
        public List<DataBook> GetListByGroupName(string GroupName)
        {
            using (var db = GetDbContext())
            {
                return db.DataBooks.Where(e => e.GroupName == GroupName).ToList();
            }
        }
        public List<DataBook> Get(List<string> GroupNames, CheckStatus status)
        {
            var list = new List<DataBook>();
            foreach (var item in GroupNames)
            {
                var glist = GetListByGroupName(item).Where(e => e.Status == status).ToList();
                if (glist != null)
                {
                    foreach (var entry in glist)
                    {
                        list.Add(entry);
                    }
                }
            }
            return list;
        }

        public DataBook Check(int ID, string Reason, string Checker, int? Day, bool? Check, CheckStatus status)
        {
            if (string.IsNullOrEmpty(Checker))
            {
                return null;
            }
            var book = Get(ID);
            if (book == null)
            {
                throw new ArgumentException("内部服务器错误！");
            }
            if (status == CheckStatus.Agree)
            {
                ADController.AddUserToGroup(book.Name, book.GroupName);
            }

            if (status != CheckStatus.Wait)
            {
                book.Checker = Checker;
                book.Reason = Reason;
                book.CheckTime = DateTime.Now;
                book.Status = status;
                var time = book.CheckTime;
                if (Day.HasValue)
                {
                    time = time.AddDays(Day.Value);
                }
                var span = time.Subtract(book.CheckTime);
                if (span.Days == 0 && span.Minutes == 0 && span.Hours == 0 && span.Seconds == 0)
                {
                    time = new DateTime(9999, 12, 31, 12, 00, 00);
                }
                if (Check.HasValue && Check.Value)
                {
                    time = new DateTime(9999, 12, 31, 12, 00, 00);
                }
                book.MaturityTime = time;
                Edit(book);
            }
            return book;

        }
    }
}

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

        public List<DataBook> Get(DataBookFilter Filter)
        {
            using (var db = GetDbContext())
            {
                var query = db.DataBooks.AsQueryable();
                if (Filter.Status != CheckStatus.All)
                {
                    query = query.Where(e => e.Status == Filter.Status);
                }
                if (!string.IsNullOrEmpty(Filter.Name))
                {
                    query = query.Where(e => e.Name == Filter.Name);
                }
                if (!string.IsNullOrEmpty(Filter.Checker))
                {
                    query = query.Where(e => e.Checker == Filter.Checker);
                }
                if (!string.IsNullOrEmpty(Filter.GroupName))
                {
                    query = query.Where(e => e.GroupName == Filter.GroupName);
                }
                if (Filter.Label.HasValue)
                {
                    query = query.Where(e => e.Label == Filter.Label.Value);
                }

                if (Filter.Page != null)
                {
                    Filter.Page.RecordCount = query.Count();
                    query = query.OrderBy(e => e.ID).Skip(Filter.Page.PageSize * (Filter.Page.PageIndex - 1)).Take(Filter.Page.PageSize);
                }
                return query.ToList();
               
            }
        }
        public List<DataBook> Get(List<int> Indexs)
        {
            var list = new List<DataBook>();
            foreach(var item in Indexs)
            {
                list.Add(Get(item));
            }
            return list;
        }
        public int Add(DataBook Book)
        {
            using (var db = GetDbContext())
            {
                db.DataBooks.Add(Book);
                db.SaveChanges();
                return Book.ID;
            }
        }
        public List<int> Add(List<string> Groups,string Name,string sAMAccountName)
        {
            var list = new List<int>();
            foreach(var group in Groups)
            {
                list.Add(Add(new DataBook { Name = Name, GroupName = group,sAMAccountName=sAMAccountName }));
            }
            return list;
        }

        public List<DataBook> GetList(string Name = null)
        {
            using (var db = GetDbContext())
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return db.DataBooks.Where(e => e.Checker == Name).ToList();
                }
                else
                {
                    return db.DataBooks.ToList();
                }
            }
        }

        public List<DataBook> GetPastDue()
        {
            var list = GetList().Where(e=>e.Label==false).ToList();
            var thelist = new List<DataBook>();
            foreach(var item in list)
            {
                if (item.CheckTime > DateTime.MinValue && item.MaturityTime > DateTime.MinValue)
                {
                    if (item.Span.Days < 0 || item.Span.Hours < 0 || item.Span.Minutes < 0 || item.Span.Seconds < 0)
                    {
                        thelist.Add(item);
                    }
                }
            }
            return thelist;
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
            using(var db = GetDbContext())
            {
                var result = db.DataBooks.Where(e => e.Status == status).ToList();
                if (result == null || result.Count == 0)
                {
                    return list;
                }
                foreach(var item in result)
                {
                    if (GroupNames.Contains(item.GroupName))
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
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
                ADController.AddUserToGroup(book.sAMAccountName, book.GroupName);
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

        public bool Examine(DataBook dataBook,out string error)
        {
            error = string.Empty;
            if (dataBook.Span.Days > 0 || dataBook.Span.Hours > 0 || dataBook.Span.Minutes > 0 || dataBook.Span.Seconds > 0)
            {
                return false;
            }
            string str = "";
            if(ADController.DeleteUserFromGroup(dataBook.sAMAccountName,dataBook.GroupName,out str))
            {
                dataBook.Label = true;
                try
                {
                    Edit(dataBook);
                }catch(Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
            else
            {
                error = str;
                return false;
            }
            return true;
        }
        public void Records(Record record)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Records.FirstOrDefault(e => e.DID == record.DID);
                if (entry != null)
                {
                    record.ID = entry.ID;
                    db.Entry(entry).CurrentValues.SetValues(record);
                }
                else
                {
                    db.Records.Add(record);
                }
                db.SaveChanges();
            }
        }
    }
}

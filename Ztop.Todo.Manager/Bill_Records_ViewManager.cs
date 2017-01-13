using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class Bill_Records_ViewManager:ManagerBase
    {
        /// <summary>
        /// 作用：通过HID 获取列表
        /// 作者：汪建龙
        /// 编写时间：2017年1月13日10:18:47
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<BillRecordView> GetByHID(int hid)
        {
            using (var db = GetDbContext())
            {
                return db.Bill_Records_View.Where(e => e.HID == hid).OrderBy(e => e.SerialNumber).ToList();
            }
        }
    }
}

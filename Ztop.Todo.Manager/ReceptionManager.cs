using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ReceptionManager:ManagerBase
    {
        public List<ReceptionItem> GainItems(string[] content,double[] coin,PayWay[] way,double[] average,string[] memo)
        {
            if (content == null || coin == null || way == null || average == null || memo == null
                ||content.Length!=coin.Length||coin.Length!=way.Length||way.Length!=average.Length||average.Length!=memo.Length
                )
            {
                return null;
            }

            var list = new List<ReceptionItem>();
            for(var i = 0; i < content.Length; i++)
            {
                list.Add(new ReceptionItem
                {
                    Content = content[i],
                    Coin = coin[i],
                    Way = way[i],
                    Average = average[i],
                    Memo = memo[i]
                });
            }
            return list;
        }
    }
}

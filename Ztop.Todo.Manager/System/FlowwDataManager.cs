using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class FlowwDataManager:ManagerBase
    {
        public FlowwData Get(int id)
        {
            return DB.Floww_Datas.Find(id);
        }

        public int Save(FlowwData data)
        {
            DB.Floww_Datas.Add(data);
            DB.SaveChanges();
            return data.ID;
        }
        //public FlowwData Get(int infoId,int flowwId)
        //{
        //    var model= DB.Floww_Datas.FirstOrDefault(e => e.InfoId == infoId && e.FlowwId == flowwId);
        //    if (model == null)
        //    {
        //        model = new FlowwData
        //        {
        //            InfoId = infoId,
        //            FlowwId = flowwId
        //        };
        //        var id = Save(model);
        //    }
        //    return model; 
        //}
        public bool ChangeState(int id,FlowwDataState state)
        {
            var model = DB.Floww_Datas.Find(id);
            if (model == null)
            {
                return false;
            }
            model.FlowwDataState = state;
            DB.SaveChanges();
            return true;
        }

        public FlowwData CreateData(int flowId,string content=null)
        {
            var entry = new FlowwData
            {
                FlowwId = flowId,
                Content = content
            };
            DB.Floww_Datas.Add(entry);
            return entry;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ProjectManager:ManagerBase
    {
        public Project Get(int id)
        {
            return DB.Projects.Find(id);
        }

        public int Add(Project project)
        {
            //DB.Entry(project).State = System.Data.Entity.EntityState.Added;
            DB.Projects.Add(project);
            DB.SaveChanges();
            return project.ID;
        }

        public bool Edit(Project project,int serialNumber)
        {
            var entry = DB.Projects.Find(project.ID);
            if (entry == null)
            {
                return false;
            }

            project.Number = entry.Number;
            if (project.Year != entry.Year)
            {
                project.SerialNumber = serialNumber;
            }
            else
            {
                project.SerialNumber = entry.SerialNumber;
            }
           
            DB.Entry(entry).CurrentValues.SetValues(project);
            DB.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var model = DB.Projects.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Deleted = true;
            DB.SaveChanges();
            return true;
        }

       /// <summary>
       /// 更改项目是否完成状态
       /// </summary>
       /// <param name="id"></param>
       /// <param name="isDone"></param>
       /// <returns></returns>
        public bool Done(int id,bool isDone,string replyPath,string remark)
        {
            var model = DB.Projects.Find(id);
            if (model != null)
            {
                model.IsDone = isDone;
                model.ReplyFile = replyPath;
                model.ReplyRemark = remark;
                DB.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeFile(int projectId,string filePath)
        {
            var model = DB.Projects.Find(projectId);
            if (model == null)
            {
                return false;
            }
            model.ReplyFile = filePath;
            DB.SaveChanges();

            return true;
        }


        public List<ProjectView> Search(ProjectParameter parameter)
        {
            var query = DB.Project_Views.Where(e=>e.Deleted==false).AsQueryable();
            if (parameter.FlowDataState.HasValue)
            {
                query = query.Where(e => e.FlowwDataState == parameter.FlowDataState.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name.Contains(parameter.Name));
            }
            if (parameter.NumberIsNull.HasValue)
            {
                query = query.Where(e => parameter.NumberIsNull.Value ^ !string.IsNullOrEmpty(e.Number));
            }
            if (!string.IsNullOrEmpty(parameter.Town))
            {
                query = query.Where(e => e.Town.Contains(parameter.Town));
            }
            if (!string.IsNullOrEmpty(parameter.Number))
            {
                query = query.Where(e => e.Number.Contains(parameter.Number.Trim()));
            }
            if (parameter.Year.HasValue)
            {
                query = query.Where(e => e.Year == parameter.Year.Value);
            }
            if (parameter.CityId.HasValue)
            {
                query = query.Where(e => e.CityId == parameter.CityId.Value);
            }
            if (!string.IsNullOrEmpty(parameter.CityName))
            {
                query = query.Where(e => e.CityName.Contains(parameter.CityName));
            }
            if (parameter.GroupId.HasValue)
            {
                query = query.Where(e => e.GroupId == parameter.GroupId.Value);
            }
            if (!string.IsNullOrEmpty(parameter.GroupName))
            {
                query = query.Where(e => e.GroupName == parameter.GroupName);
            }
            if (parameter.FID.HasValue)
            {
                query = query.Where(e => e.FID == parameter.FID.Value);
            }

            if (parameter.SEID.HasValue)
            {
                query = query.Where(e => e.ProjectTypeId == parameter.SEID.Value);
            }

            if (parameter.ChargeID.HasValue)
            {
                query = query.Where(e => e.UserId == parameter.ChargeID.Value);
            }
            if (parameter.IsRecord.HasValue)
            {

                query = query.Where(e => (string.IsNullOrEmpty(e.Number) ^ parameter.IsRecord.Value));
            }
            if (!string.IsNullOrEmpty(parameter.Participation))
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.Participation));

                query = query.Where(e => e.Participation.ToLower().Contains(parameter.Participation.ToLower()));
            }

            switch (parameter.Order)
            {
                case ProjectOrder.ID:
                    query = query.OrderBy(e => e.ID);
                    break;
                case ProjectOrder.IDDescending:
                    query = query.OrderByDescending(e => e.ID);
                    break;
                case ProjectOrder.Number:
                    query = query.OrderBy(e => e.Number);
                    break;
                case ProjectOrder.NumberDescending:
                    query = query.OrderByDescending(e => e.Number);
                    break;
                case ProjectOrder.Serial:
                    query = query.OrderBy(e => e.Year).ThenBy(e=>e.SerialNumber);
                    break;
                case ProjectOrder.SerialDescending:
                    query = query.OrderByDescending(e => e.Year).ThenByDescending(e => e.SerialNumber);
                    break;
            }
            query = query.SetPage(parameter.Page);
            return query.ToList();
        }

        /// <summary>
        /// 作用：获取系统中的存在的年份记录信息
        /// </summary>
        /// <returns></returns>
        public List<int> GetYears()
        {
            return DB.Projects.GroupBy(e => e.Year).Select(e=>e.Key).ToList();
        }

        public Project GetLast(int year)
        {
            return DB.Projects.Where(e => e.Year == year).OrderByDescending(e => e.SerialNumber).FirstOrDefault();
        }

        public bool Number(int id,string number)
        {
            var entry = DB.Projects.Find(id);
            if (entry == null)
            {
                return false;
            }
            entry.Number = number;
            DB.SaveChanges();
            return true;
        }
    }
}

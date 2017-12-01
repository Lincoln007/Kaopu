using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ProjectUserManager:ManagerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">当前最新的关系表</param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool Edit(List<ProjectUser> list,int projectId)
        {
            var currents = DB.Project_Users.Where(e => e.ProjectId == projectId).ToList();//目前数据库中存在的用户关系表
            if (currents.Count == 0)
            {
                DB.Project_Users.AddRange(list);
                DB.SaveChanges();
                return true;
            }
            foreach(var item in currents)
            {
                var entry = list.FirstOrDefault(e => e.UserId == item.UserId && e.Relation == item.Relation);
                if (entry != null)//存在
                {
                    list.Remove(entry);
                }
                else//不存在
                {
                    DB.Project_Users.Remove(item);
                }
            }
            if (list.Count > 0)
            {
                DB.Project_Users.AddRange(list);
            }
            DB.SaveChanges();
            return true;
        }

        /// <summary>
        /// 作用：修改项目负责人
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <param name="userId">修改后负责人的ID</param>
        /// <returns></returns>
        public bool ChangeCharge(int id,int userId)
        {
            var current = DB.Project_Users.FirstOrDefault(e => e.ProjectId == id && e.Relation == ProjectRelation.InCharge);
            if (current == null||current.UserId==userId)
            {
                return false;
            }
            current.Relation = ProjectRelation.Participation;//更改项目负责人，原始项目负责人成为项目参与人员
            //DB.Project_Users.Remove(current);
            DB.Project_Users.Add(new ProjectUser { UserId = userId, ProjectId = id, Relation = ProjectRelation.InCharge });
            DB.SaveChanges();
            return true;
        }

        public List<User> GetChargeList(string groupName)
        {
            var list= DB.Project_Users.Where(e => e.Relation == ProjectRelation.InCharge).GroupBy(e => e.User).Select(e => e.Key).ToList();
            if (!string.IsNullOrEmpty(groupName))
            {
                var group = DB.UserGroups.FirstOrDefault(e => e.Name == groupName);
                if (group != null)
                {
                    list = list.Where(e => e.GroupID == group.ID).ToList();
                }
            }
            return list;
        }

        public void Add(List<ProjectUser> list)
        {
            DB.Project_Users.AddRange(list);
            DB.SaveChanges();
        }

    }
}

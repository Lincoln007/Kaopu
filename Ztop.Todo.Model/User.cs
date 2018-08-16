using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Common;

namespace Ztop.Todo.Model
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 项目一部  项目二部
        /// </summary>
        public int GroupID { get; set; }

        public string Username { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }

        [NotMapped]
        public GroupType Type { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(RealName) ? (Username.Contains('\\') ? Username.Split('\\')[1] : Username) : RealName;
            }
        }

        public string RealName { get; set; }

        public int LoginTimes { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public int Order { get; set; }
        public bool Delete { get; set; }
        
        /// <summary>
        /// 系统权限 1,2,3,4,5
        /// </summary>
        [Column("GroupIds")]
        public string GroupIdValues { get; set; }
        /// <summary>
        /// 系统权限  数组
        /// </summary>
        public int[] GroupIds
        {
            get
            {
                if (string.IsNullOrEmpty(GroupIdValues)) return null;
                return GroupIdValues.ToIntArray();
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    GroupIdValues = null;
                }
                else
                {
                    GroupIdValues = string.Join(",", value);
                }
            }
        }
        [NotMapped]
        public List<PowerGroup> Groups { get; set; }
        /// <summary>
        /// 系统权限  助理，部门经理，
        /// </summary>
        [NotMapped]
        public string GroupNames
        {
            get
            {
                if (Groups == null) return null;
                return string.Join(",", Groups.Select(e => e.Name).ToArray());
            }
        }


    }

    [Serializable]
    public class UserInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

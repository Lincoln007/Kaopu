using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("projects")]
    public class Project
    {
        public Project()
        {
            Time = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        //public string GroupName { get; set; }
        //public int GroupId { get; set; }
        
        //public virtual UserGroup Group { get; set; }

        public int SerialNumber { get; set; }
        [NotMapped]
        public string Serial
        {
            get { return string.Format("{0}{1}", Year, SerialNumber.ToString("0000")); }
        }
        /// <summary>
        ///登记编号
        /// </summary>
        [MaxLength(7)]
        public string Number { get; set; }
        /// <summary>
        /// 城市（县级）
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 乡镇主体
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public int ProjectTypeId { get; set; }
        public virtual ProjectType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Deleted { get; set; }
        
        public bool IsDone { get; set; }
        /// <summary>
        /// 批复文件路径
        /// </summary>
        public string ReplyFile { get; set; }

        public virtual List<ProjectUser> ProjectUser { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [NotMapped]
        public ProjectUser Charge
        {
            get { return ProjectUser.FirstOrDefault(e => e.Relation == ProjectRelation.InCharge); }
        }

        /// <summary>
        /// 项目参与人员
        /// </summary>
        [NotMapped]
        public List<ProjectUser> Participations
        {
            get { return ProjectUser.Where(e => e.Relation == ProjectRelation.Participation).ToList(); }
        }

        public virtual List<WorkLoad> Workloads { get; set; }

        //public virtual List<ProjectRecord> Records { get; set; }
        //public virtual List<ProjectProgress> Progress { get; set; } 


    }
    [Table("project_user")]
    public class ProjectUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ProjectId { get; set; }
        public ProjectRelation Relation { get; set; }
    }

    public enum ProjectRelation
    {

        [Description("参与")]
        Participation,
        [Description("负责")]
        InCharge
    }
    public enum ProjectOrder
    {
        [Description("ID升序")]
        ID,
        [Description("ID降序")]
        IDDescending,
        [Description("项目编号升序")]
        Serial,
        [Description("项目编号降序")]
        SerialDescending,
        [Description("登记编号升序")]
        Number,
        [Description("登记编号降序")]
        NumberDescending
    }
}

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
    /// <summary>
    /// 作用：项目类型表
    /// 作者：汪建龙
    /// 编写时间：2016年12月5日19:11:49
    /// </summary>
    [Table("project_types")]
    public class ProjectType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 字母（代号）
        /// </summary>
        public string Chars { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public Degree Degree { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int PPID { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}{1}", Chars, Name);
            }
        }
        
    }


    public enum Degree
    {
        [Description("一级类")]
        First,
        [Description("二级类")]
        Second
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 作用：平板联系人表
    /// 作者：汪建龙
    /// 编写时间：2016年12月8日09:39:35
    /// </summary>
    [Table("ipad_contacts")]
    public class iPadContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///单位  部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 所属城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }  
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string Section_Office { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public bool Delete { get; set; }
        /// <summary>
        /// 信息录入人
        /// </summary>
        public string Creator { get; set; }
          
    }



}

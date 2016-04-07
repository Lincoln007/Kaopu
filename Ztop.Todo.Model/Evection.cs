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
    /// 出差报销  分类项目
    /// </summary>
    [Table("evections")]
    public class Evection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 公里数
        /// </summary>
        public double KiloMeters { get; set; }
        /// <summary>
        /// 交通费
        /// </summary>
        public double Traffic { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int Peoples { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 出差补贴
        /// </summary>
        public int SubSidy { get; set; }
        /// <summary>
        /// 出差住宿
        /// </summary>
        public double Hotel { get; set; }
        /// <summary>
        /// 其他内容
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 其他金额
        /// </summary>
        public double Other { get; set; }
        /// <summary>
        /// 过路费
        /// </summary>
        public double Toll { get; set; }
        /// <summary>
        /// 出差人  名字
        /// </summary>
        public string Names { get; set; }
        public int SID { get; set; }
    }
}

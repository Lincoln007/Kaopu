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
    /// 作用：城市表
    /// 作者：汪建龙
    /// 时间：2016年12月5日10:41:34
    /// </summary>
    [Table("citys")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市级别
        /// </summary>
        public Rank Rank { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int PCID { get; set; }
        [NotMapped]
        public City PreCity { get; set; }
        
    }

    public enum Rank
    {
        [Description("省级")]
        Province,
        [Description("市级")]
        City,
        [Description("区/县级")]
        District,
        [Description("乡镇")]
        Town
    }

    public enum SystemCategory
    {
        City,
        ProjectType,
        ProjectSystem
    }

}

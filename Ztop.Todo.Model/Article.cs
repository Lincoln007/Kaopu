using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 项目洽谈表
    /// 作者：汪建龙
    /// 备注时间：2016年12月5日18:26:54
    /// </summary>
    [Table("article")]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登记编号  需要验证唯一性
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 对方单位
        /// </summary>
        public string OtherSide { get; set; }
        /// <summary>
        /// 意向金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1023)]
        public string Remark { get; set; }
        /// <summary>
        /// 项目洽谈状态
        /// </summary>
        public ArticleState State { get; set; }
        /// <summary>
        /// 所属城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public List<Contract> Contracts { get; set; } 
    }

    public enum ArticleState
    {
        [Description("洽谈中")]
        Talking,
        [Description("启动")]
        Start,
        [Description("完成")]
        Finish,
        [Description("未完成")]
        UnFinish
    }
}

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
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public City CEntry { get; set; }
        /// <summary>
        /// 所属城市ID
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [NotMapped]
        public string ProjectType { get; set; }
        [NotMapped]
        public ProjectType PEntry { get; set; }
        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int PID { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public List<Contract> Contracts { get; set; } 
        /// <summary>
        /// 所属年份 必填
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 乡镇（主体） 必填
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 支出单位
        /// </summary>
        public string PayCompany { get; set; }
        /// <summary>
        /// 需支出部分金额【万元】
        /// </summary>
        public double? PayMoney { get; set; }
    }
    [Table("article_view")]
    public class ArticleView
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string OtherSide { get; set; }
        public double Money { get; set; }
        public string Remark { get; set; }
        public ArticleState State { get; set; }
        public string Code { get; set; }
        public string CName { get; set; }
        public string PName { get; set; }
        public string Chars { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}{1}", Chars, PName);
            }
        }
        public bool Deleted { get; set; }
        public string Year { get; set; }
        public string Town { get; set; }
        public string PayCompany { get; set; }
        public double? PayMoney { get; set;}
        [NotMapped]
        public List<Contract> Contracts { get; set; }
    }
    public enum ArticleState
    {
        [Description("待定")]
        None,
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

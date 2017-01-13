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
    /// 分项清单
    /// </summary>
    [Table("substancs")]
    public class Substancs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 一级分类
        /// </summary>
        [Column(TypeName ="int")]
        public Category Category { get; set; }
        /// <summary>
        /// 二级分类
        /// </summary>
        [Column(TypeName ="int")]
        public SecondCategory? SecondCategory { get; set; }
        /// <summary>
        /// 一级类ID
        /// </summary>
        public int RID { get; set; }
        /// <summary>
        /// 二级类ID
        /// </summary>
        public int? SRID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Price { get; set; }
        public int SID { get; set; }
    }

    [Table("substancs_view")]
    public class SubstancsView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Details { get; set; }
        public double Price { get; set; }
        public int SID { get; set; }
        public string Name { get; set; }
        public string SName { get; set; }
    }

    public enum Category
    {
        [Description("办公用品费")]
        OfficialBussiness,
        [Description("固定资产费")]
        FixedAsssets,
        [Description("耗材费")]
        Equipment,
        [Description("交通费")]
        Traffic,
        [Description("维修维护费")]
        Maintenance,
        [Description("快递邮寄费")]
        Express,
        [Description("印刷装订费")]
        Print,
        [Description("日常招待费")]
        Reception,
        [Description("福利相关费")]
        Welfare,
        [Description("评审会务费")]
        Evaluate,
        [Description("招标相关费")]
        Bidding,
        [Description("财务费")]
        Financial,
        [Description("其他")]
        Other,
        [Description("薪资")]
        Emolument,
        [Description("增值税")]
        ValueAddTax,
        [Description("所得税")]
        InComeTax,
        [Description("五险")]
        FiveInsurance,
        [Description("公积金")]
        AccumulationFound,
        [Description("水电物管费")]
        Property,
        [Description("项目合作费")]
        Projection,
        [Description("年检培训费")]
        Trainning
    }

    public enum SecondCategory
    {
        [Description("设备")]
        Equipment,
        [Description("家具")]
        Furniture,
        [Description("纸")]
        Paper,
        [Description("墨盒")]
        Cartridge,
        [Description("硒鼓")]
        Toner,
        [Description("市内交通")]
        Traffic,
        [Description("车辆相关")]
        Car,
        [Description("邮寄")]
        Post,
        //[Description("通讯")]
        //Communication,
        [Description("印刷")]
        Printing,
        [Description("复印")]
        Copy,
        [Description("装订")]
        Binding,
        [Description("节日福利")]
        Festival,
        [Description("日常福利")]
        Daily,
        [Description("代理费")]
        Proxy,
        [Description("报名费")]
        Register,
        [Description("标书费")]
        Petty,
        [Description("其他")]
        Other
    }


}

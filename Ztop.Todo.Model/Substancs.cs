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
        public SecondCategory SecondCategory { get; set; }
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

    public enum Category
    {
        [Description("日常办公")]
        OfficialBussiness,
        [Description("固定资产")]
        FixedAsssets,
        [Description("耗材")]
        Equipment,
        [Description("交通费")]
        Traffic,
        [Description("维修维护")]
        Maintenance,
        [Description("邮电费")]
        Express,
        [Description("印刷装订")]
        Print,
        [Description("招待费")]
        Reception,
        [Description("福利费")]
        Welfare,
        [Description("评审费")]
        Evaluate,
        [Description("招投标费")]
        Bidding,
        [Description("财务费")]
        Financial,
        [Description("其他")]
        Other
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
        [Description("通讯")]
        Communication,
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

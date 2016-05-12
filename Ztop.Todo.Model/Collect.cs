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
    [Table("collects")]
    public class Collect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Summary { get; set; }
        public double Expenses { get; set; }
        public double Income { get; set; }
        [Column(TypeName ="int")]
        public Company Company { get; set; }
        public int AID { get; set; }
    }

    public enum Company
    {
        [Description("评估")]
        Evaluation,
        [Description("规划")]
        Projection,
        [Description("现金")]
        Cash,
        [Description("合计")]
        Sum
    }
    public enum Budget
    {
        [Description("收入")]
        Income,
        [Description("支出")]
        Expense
    }
    [Table("gathers")]
    public class Gather
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName ="int")]
        public Company Company { get; set; }
        public double Income { get; set; }
        public double MarginIncome { get; set; }
        public double Pay { get; set; }
        public double MarginPay { get; set; }
        public double Transfer { get; set; }
        public double Petty { get; set; }
        public int AID { get; set; }
    }

    [Table("aggregations")]
    public class Aggregation
    {
        public Aggregation()
        {
            Time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 房地产土地评估支出合计
        /// </summary>
        public double SubstalEE { get; set; }
        /// <summary>
        /// 房地产土地评估收入合计
        /// </summary>
        public double SubstalIE { get; set; }
        /// <summary>
        /// 土地规划设计支出合计
        /// </summary>
        public double SubstalEP { get; set; }
        /// <summary>
        /// 推动规划设计收入合计
        /// </summary>
        public double SubstalIP { get; set; }
        [NotMapped]
        public List<Gather> Gathers { get; set; }
        [NotMapped]
        public List<Collect> EvaluationCollects { get; set; }
        [NotMapped]
        public List<Collect> ProjectionCollects { get; set; }

    }


    /// <summary>
    /// 银行对账单
    /// </summary>
    [Table("banks")]
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Column(TypeName ="int")]
        public Company Company { get; set; }
        [NotMapped]
        public List<Bill> Bills { get; set; }
    }


    [Table("bills")]
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 到账编号
        /// </summary>
        public string Coding { get; set; }
        /// <summary>
        /// 到账时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 对方银行账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 收支
        /// </summary>
        [Column(TypeName ="int")]
        public Budget Budget { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        public int BID { get; set; }
    }
}

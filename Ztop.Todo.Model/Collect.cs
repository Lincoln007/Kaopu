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
    public class Collect
    {
        /// <summary>
        /// 收入
        /// </summary>
        public double Income { get; set; }
        /// <summary>
        /// 保证金收入
        /// </summary>
        public double MarginIncome { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public double Pay { get; set; }
        /// <summary>
        /// 保证金支出
        /// </summary>
        public double MarginPay { get; set; }
        /// <summary>
        /// 转账
        /// </summary>
        public double Transfer { get; set; }
        /// <summary>
        /// 备用金
        /// </summary>
        public double Petty { get; set; }

        public static Collect operator +(Collect c1,Collect c2)
        {
            return new Collect()
            {
                Income = c1.Income + c2.Income,
                MarginIncome = c1.MarginIncome + c2.MarginIncome,
                Pay = c1.Pay + c2.Pay,
                MarginPay = c1.MarginPay + c2.MarginPay,
                Transfer = c1.Transfer + c2.Transfer,
                Petty = c1.Petty + c2.Petty
            };
        }
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
        /// <summary>
        /// 本月余额
        /// </summary>
        public double Balance { get; set; }
        [NotMapped]
        public List<Bill> Bills { get; set; }
        [NotMapped]
        public string YearMonth
        {
            get
            {
                return string.Format("{0}年{1}月", Year, Month);
            }
        }
    }
 
    //public enum Cost
    //{
    //    [Description("工资")]
    //    Salary,
    //    [Description("杂费")]
    //    Extras,
    //    [Description("税")]
    //    Tax,
    //    [Description("电话费")]
    //    Phone,
    //    [Description("备用金")]
    //    Petty,
    //    [Description("保证金")]
    //    Margin,
    //    [Description("转账")]
    //    Transfer,
    //    [Description("其他")]
    //    Other,
    //    [Description("实际收入")]
    //    RealIncome,
    //    [Description("还款")]
    //    Repayment,
    //    [Description("借款")]
    //    Load
    //}

    public enum Cost
    {
        [Description("实际收入")]
        RealIncome,
        [Description("还款")]
        Repayment,
        [Description("保证金退款")]
        MarginIncome,
        [Description("过账")]
        Posting,
        [Description("借款")]
        Load,
        [Description("保证金支出")]
        MarginPay,
        [Description("实际支出")]
        RealPay,
        [Description("备用金")]
        Petty
    }
}

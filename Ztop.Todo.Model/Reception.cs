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
    /// 招待
    /// </summary>
    [Table("receptions")]
    public class Reception
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 招待时间
        /// </summary>
        public DateTime RTime { get; set; }
        /// <summary>
        /// 招待城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 招待人数
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 招待人员
        /// </summary>
        public string Persons { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public virtual List<ReceptionItem> Items { get; set; }

    }

    [Table("reception_Items")]
    public class ReceptionItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Content { get; set; }
        public double Coin { get; set; }
        //[NotMapped]
        //public double Average { get { return } }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        public PayWay Way { get; set; }
        public int ReceptionId { get; set; }
        public virtual Reception Reception { get; set; }
    }

    public enum PayWay
    {
       [Description("现金支付")]
        Cash,
       [Description("储值卡支付")]
        Card,
       [Description("挂账")]
        Bill,
       [Description("自备")]
       Self
    }
}

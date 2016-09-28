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
    [Table("ipads")]
    public class iPad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string SerialNumber { get; set; }
        [Column(TypeName ="int")]
        public iPadType Type { get; set; }
        [Column(TypeName ="int")]
        public Space Space { get; set; }
        public string Buyer { get; set; }
        public string Account { get; set; }
        [Column(TypeName ="int")]
        public InternetWay Way { get; set; }
        public int? IID { get; set; }
        [Column(TypeName ="int")]
        public iPadStatue Statue { get; set; }
        public string Enter { get; set; }

    }


    public enum iPadType
    {
        [Description("iPad Mini2")]
        Mini2,
        [Description("iPad Mini4")]
        Mini4,
        [Description("iPad Air")]
        Air,
        [Description("iPad Air2")]
        Air2,
        [Description("iPad Pro")]
        Pro
    }

    public enum Space
    {
        [Description("16G")]
        Sixteen,
        [Description("32G")]
        ThirtyTwo,
        [Description("64G")]
        SixtyFour,
        [Description("128G")]
        OneHundredAndTwentyEight,
        [Description("256G")]
        TwoHundredAndFivetySix
    }
    public enum InternetWay
    {
        WLAN,
        WLAN_Cellular
    }

    public enum iPadStatue
    {
        [Description("闲置")]
        Vacant,
        [Description("外借")]
        Borrow,
        [Description("项目使用")]
        Deliver
    }
}

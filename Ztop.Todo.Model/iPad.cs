using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public class iPad
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string SerialNumber { get; set; }
        public iPadType Type { get; set; }
        public Space Space { get; set; }

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
}

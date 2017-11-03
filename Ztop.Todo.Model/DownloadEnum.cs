using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    public enum DownloadEnum
    {
        [Description("报销单")]
        Sheet,
        [Description("日常报销")]
        Daily_Sheet,
        [Description("出差报销")]
        Errand_Sheet,
        [Description("转账支出")]
        Transfer_Sheet,
        [Description("日常招待")]
        Reception_Sheet,
    }
}

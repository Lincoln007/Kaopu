using System;
using System.Runtime.InteropServices;

namespace Ztop.Todo.Model
{
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string IpData;
    }
}

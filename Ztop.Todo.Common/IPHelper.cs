using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class IPHelper
    {
        public static IPAddress GetIPAddress()
        {
            // IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            var ipHostEntry = Dns.GetHostEntry(Dns.GetHostName()); 
            return ipHostEntry.AddressList[0];
        }
    }
}

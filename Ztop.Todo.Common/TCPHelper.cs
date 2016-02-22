using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class TCPHelper
    {
        public static void TCPSend(string Message)
        {
            TcpClient tcpClient = new TcpClient(IPHelper.GetIPAddress().ToString(), Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"]));
            NetworkStream ns = tcpClient.GetStream();
            byte[] buffer = System.Text.Encoding.Unicode.GetBytes(Message);
            try
            {
                lock (ns)
                {
                    ns.Write(buffer, 0, buffer.Length);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string TCPRecevier(TcpListener tcpListener)
        {
            string result = string.Empty;
            using (TcpClient tcpClient = tcpListener.AcceptTcpClient())
            {
                NetworkStream ns = tcpClient.GetStream();
                byte[] buffer = new byte[tcpClient.Available];
                ns.Read(buffer, 0, buffer.Length);
                result = System.Text.Encoding.Unicode.GetString(buffer).Trim();
            } 
            return result;
        }
    }
}

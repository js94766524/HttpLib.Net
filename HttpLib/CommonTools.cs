using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpLib
{
    public static class Tools
    {
        public static string GetNetAddress()
        {
            try
            {
                string HostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                foreach(var addr in IpEntry.AddressList)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork) return addr.ToString();
                }
                return "localhost";
            }
            catch
            {
                return "localhost";
            }
        }
    }
}

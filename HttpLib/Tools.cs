using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

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

        public static NameValueCollection ParseNameValues( string kvString )
        {
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(kvString);
            var nvc = new NameValueCollection();

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
            return nvc;
        }

        public static byte[] ReadInputStream( this HttpListenerRequest request )
        {
            Stream input = request.InputStream;

            BufferedStream br = new BufferedStream(input);

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[4096];

                int len = 0;

                while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, len);
                }

                return ms.ToArray();
            }
        }
    }

    
}

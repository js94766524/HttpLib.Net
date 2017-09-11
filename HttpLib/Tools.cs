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
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 获取与本机关联的IPv4地址
        /// </summary>
        /// <returns>IPv4地址</returns>
        public static string GetNetAddress()
        {

            string HostName = Dns.GetHostName();
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);

            foreach (var addr in IpEntry.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork) return addr.ToString();
            }
            
            return "NO_INTERNET_WORK";
        }
        
        /// <summary>
        /// 将URL中的参数串解析为键值对集合
        /// </summary>
        /// <param name="kvString">参数字符串</param>
        /// <returns>参数键值对集合</returns>
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

        /// <summary>
        /// 从请求的输入流中读取byte数组
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>byte数组</returns>
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

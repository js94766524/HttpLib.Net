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

        /// <summary>
        /// 将文件转换为Base64字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="bufferSize">缓冲区大小，默认200（200 * 12 = 4800）</param>
        /// <returns>Base64字符串</returns>
        public static string FileToBase64String( string filePath, int bufferSize = 200 )
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[bufferSize * 12];
                int offset = 0;
                int count = 0;
                StringBuilder sb = new StringBuilder();
                while ((count = fs.Read(buffer, offset, bufferSize * 12)) > 0)
                {
                    sb.Append(Convert.ToBase64String(buffer, 0, count));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 将Base64字符串转化为文件
        /// </summary>
        /// <param name="base64">Base64字符串</param>
        /// <param name="filePath">保存文件的路径</param>
        public static void FileFromBase64String( string base64, string filePath )
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] source = Convert.FromBase64String(base64);
                fs.Write(source, 0, source.Length);
            }
        }
    }


}

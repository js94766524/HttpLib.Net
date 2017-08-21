using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace HttpLib
{
    public static class HttpMethods
    {
        /// <summary>
        /// Http Request Get
        /// </summary>
        public static string Get(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream myResponseStream = response.GetResponseStream())
            using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
            {
                return myStreamReader.ReadToEnd();
            }

        }

        /// <summary>
        /// Http Request Post
        /// </summary>
        public static string Post(string url, string dataStr)
        {
            byte[] data = Encoding.UTF8.GetBytes(dataStr);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.LongLength;

            using (Stream myRequestStream = request.GetRequestStream())
            {
                myRequestStream.Write(data, 0, data.Length);
                myRequestStream.Flush();

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream myResponseStream = response.GetResponseStream())
                using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                {
                    return myStreamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 片段下载，用于断点续传功能
        /// </summary>
        public static void Download(this string url, IDownloadHandler handler, long from = 0, long to = -1)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (to < from)
            {
                request.AddRange(from);
            }
            else
            {
                request.AddRange(from, to);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    throw new Exception("服务器不支持断点续传");
                }
                Debug.WriteLine("contentLength = " + response.ContentLength);
                handler.setFileLength(response.ContentLength);
                byte[] buffer = new byte[1024 * 5];
                using (Stream stream = response.GetResponseStream())
                {
                    int count;
                    //读取时注意实际接收数据大小  
                    while ((count = stream.Read(buffer, 0, buffer.Length)) != 0 && handler.Downloading)
                    {
                        handler.HandleBytes(buffer, count);
                        Thread.Sleep(5);
                    }
                }
            }
        }

        /// <summary>
        /// 获取请求内容的长度
        /// </summary>
        public static long GetContentLength(this string url)
        {
            long length = 0;
            try
            {
                var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 5000;
                var res = (HttpWebResponse)req.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    length = res.ContentLength;
                }
                res.Close();
                return length;
            }
            catch (WebException)
            {
                return 0;
            }
        }
    }

    public interface IDownloadHandler
    {
        void setFileLength(long remineLength);
        bool Downloading { get; }
        void HandleBytes(byte[] data, int count);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace HttpLib.HttpClient
{
    public static class HttpMethods
    {
        /// <summary>
        /// Http Request Get
        /// </summary>
        public static string Get( string url )
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
        public static string Post( string url, string dataStr )
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
        public static void Download( this string url, IDownloadHandler handler, long from = 0, long to = -1 )
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
                        //Thread.Sleep(5);
                    }
                }
            }
        }

        /// <summary>
        /// 获取请求内容的长度
        /// </summary>
        public static long GetContentLength( this string url )
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

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formItems">Post表单内容</param>
        /// <param name="cookieContainer"></param>
        /// <param name="timeOut">默认20秒</param>
        /// <param name="encoding">响应内容的编码类型（默认utf-8）</param>
        /// <returns></returns>
        public static string PostForm( string url, List<FormItem> formItems, CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 20000 )
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            #region 初始化请求对象
            if (encoding == null) encoding = Encoding.UTF8;
            request.Method = "POST";
            request.Timeout = timeOut;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(refererUrl))
                request.Referer = refererUrl;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流
            var postStream = new MemoryStream();

            #region 处理Form表单请求内容
            //是否用Form上传文件
            var formUploadFile = formItems != null && formItems.Count > 0;
            if (formUploadFile)
            {
                //文件数据模板
                string fileFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                    "\r\nContent-Type: application/octet-stream" +
                    "\r\n\r\n";
                //文本数据模板
                string dataFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                    "\r\n\r\n{1}";
                foreach (var item in formItems)
                {
                    string formdata = null;
                    if (item.IsFile)
                    {
                        //上传文件
                        formdata = string.Format(
                            fileFormdataTemplate,
                            item.Key, //表单键
                            item.FileName);
                    }
                    else
                    {
                        //上传文本
                        formdata = string.Format(
                            dataFormdataTemplate,
                            item.Key,
                            item.Value);
                    }

                    //统一处理
                    byte[] formdataBytes = null;
                    //第一行不需要换行
                    if (postStream.Length == 0)
                        formdataBytes = encoding.GetBytes(formdata.Substring(2, formdata.Length - 2));
                    else
                        formdataBytes = encoding.GetBytes(formdata);
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);

                    //写入文件内容
                    if (item.FileContent != null && item.FileContent.Length > 0)
                    {
                        using (var stream = item.FileContent)
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                //结尾
                var footer = encoding.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                ////debug
                //postStream.Seek(0, SeekOrigin.Begin);
                //StreamReader sr = new StreamReader(postStream);
                //var postStr = sr.ReadToEnd();
                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {

                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }
    }

    public interface IDownloadHandler
    {
        void setFileLength( long remineLength );
        bool Downloading { get; }
        void HandleBytes( byte[] data, int count );
    }


}

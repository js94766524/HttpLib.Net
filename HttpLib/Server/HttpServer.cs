using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using HttpLib.CustomEventArgs;
using HttpLib.Server.Interfaces;

namespace HttpLib.Server
{
    /// <summary>
    /// 一个简单易用的Http服务器类
    /// </summary>
    public class HttpServer
    {
        /// <summary>
        /// 本机的IPv4地址
        /// </summary>
        private string IP { get; set; }
        /// <summary>
        /// <see cref="HttpServer"/> 监听的端口号集合
        /// </summary>
        private uint[] PORT { get; set; }
        /// <summary>
        /// 获取由此 <see cref="HttpServer"/> 对象处理的统一资源标识符 (URI) 前缀。
        /// </summary>
        public HttpListenerPrefixCollection Prefixes { get { return httpListener.Prefixes; } }
        /// <summary>
        /// 请求处理器工厂对象
        /// </summary>
        public IRequestHandlerFactory HandlerFactory { get; set; }
        /// <summary>
        /// 获取一个值来判断 <see cref="HttpServer"/> 是否正在运行
        /// </summary>
        public bool Running { get { return httpListener != null && httpListener.IsListening; } }
        /// <summary>
        /// 当发生请求处理业务级别的异常时，会触发 <see cref="OnError"/> 事件
        /// </summary>
        public event EventHandler<ExceptionEventArgs> OnError;
        /// <summary>
        /// Http协议侦听器对象
        /// </summary>
        private HttpListener httpListener = new HttpListener();
        /// <summary>
        /// 请求响应线程对象
        /// </summary>
        private Thread responseThread;

        /// <summary>
        /// 初始化 <see cref="HttpServer"/> 类的新实例
        /// </summary>
        /// <param name="factory">请求处理器工厂</param>
        /// <param name="port">需要监听的端口号</param>
        public HttpServer( IRequestHandlerFactory factory, params uint[] port )
        {
            IP = Tools.GetNetAddress();
            SetPort(port);

            if (factory != null) HandlerFactory = factory;
            else HandlerFactory = new DefaultRequestHandlerFactory();

            responseThread = new Thread(ResponseThreadLoop) { IsBackground = true };
        }

        /// <summary>
        /// 设置<see cref="HttpListener.Prefixes"/>
        /// </summary>
        /// <param name="port">端口号集合</param>
        private void SetPort( params uint[] port )
        {
            if (port == null || port.Count() == 0) throw new ArgumentNullException("port");
            if (httpListener.IsListening) httpListener.Stop();

            PORT = port;
            httpListener.Prefixes.Clear();
            foreach (uint p in PORT)
            {
                httpListener.Prefixes.Add(string.Format("http://{0}:{1}/", IP, p));
            }
        }

        /// <summary>
        /// 请求响应线程执行的死循环方法
        /// </summary>
        private void ResponseThreadLoop()
        {
            while (Running)
            {
                try
                {
                    HttpListenerContext context = httpListener.GetContext();
                    HandleRequest(context);
                }
                catch (Exception e)
                {
                    ExceptionEventArgs eArgs = new ExceptionEventArgs(e);
                    if (OnError != null) OnError(this, eArgs);
                    if (!eArgs.Handled) HandleError(e);
                }
            }
        }

        /// <summary>
        /// 当用户没有处理业务级别的异常时，由 <see cref="HttpServer"/> 自行处理异常的方法
        /// </summary>
        /// <param name="e">异常</param>
        private void HandleError( Exception e )
        {
            Console.WriteLine(e);
        }

        /// <summary>
        /// 在系统线程池调用线程来处理每一个请求的方法
        /// </summary>
        /// <param name="context"></param>
        private void HandleRequest( HttpListenerContext context )
        {
            ThreadPool.QueueUserWorkItem(( state ) =>
            {
                var c = state as HttpListenerContext;
                try
                {
                    using (var handler = HandlerFactory.GetHandler(c.Request))
                    {
                        var respBytes = handler.Handle();
                        c.Response.OutputStream.Write(respBytes, 0, respBytes.Length);
                    }
                }
                finally
                {
                    c.Response.KeepAlive = false;
                    c.Response.Close();
                }
            }, context);
        }

        /// <summary>
        /// 开始运行 <see cref="HttpServer"/>
        /// </summary>
        public void Start()
        {
            if (!Running)
            {
                httpListener.Start();
                responseThread.Start();
            }
        }

        /// <summary>
        /// 停止运行 <see cref="HttpServer"/>
        /// </summary>
        public void Stop()
        {
            if (Running) httpListener.Stop();
        }
        
    }
}

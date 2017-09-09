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
    public class HttpServer : IDisposable
    {
        public string IP { get; private set; }
        public uint PORT { get; private set; }
        public string URL { get; private set; }
        public IRequestHandlerFactory HandlerFactory { get; set; }

        public bool Running { get { return httpListener != null && httpListener.IsListening; } }
        public event EventHandler<ExceptionEventArgs> OnError;


        private HttpListener httpListener;
        private Thread responseThread;

        public HttpServer( uint port, IRequestHandlerFactory factory )
        {
            if (factory != null) HandlerFactory = factory;
            else HandlerFactory = new DefaultRequestHandlerFactory();

            httpListener = new HttpListener();
            responseThread = new Thread(ResponseThreadLoop) { IsBackground = true };
            IP = Tools.GetNetAddress();
            SetPort(port);
            responseThread.Start();
        }

        private void ResponseThreadLoop()
        {
            while (true)
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

        private void HandleError( Exception e )
        {
            Debug.WriteLine(e);
        }

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

        public void SetPort( uint port )
        {
            if (httpListener.IsListening) httpListener.Stop();

            PORT = port;
            URL = string.Format("http://{0}:{1}/", IP, PORT);
            httpListener.Prefixes.Clear();
            httpListener.Prefixes.Add(URL);
        }

        public void Start()
        {
            if (!Running) httpListener.Start();
        }

        public void Stop()
        {
            if (Running) httpListener.Stop();
        }

        public void Dispose()
        {
            httpListener.Abort();
            httpListener = null;
            responseThread.Abort();
            responseThread = null;
        }
    }




}

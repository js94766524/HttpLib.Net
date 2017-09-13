using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.HttpServer;
using HttpLib.Remoting.Server;
using RemotingObject;

namespace ConsoleTest
{
    class Program
    {
        static void Main( string[] args )
        {
            StartHttpServer();

            StartRemotingServer();

            Console.ReadLine();
        }

        private static void StartRemotingServer()
        {
            RemotingServer.RegisterChannel(12600);
            RemotingServer.RegisterObject<RemotingObj>(mode: System.Runtime.Remoting.WellKnownObjectMode.Singleton);
            Console.WriteLine("Remoting Server is Ready");
        }

        static void StartHttpServer()
        {
            HttpServer server = new HttpServer(null, 12580, 12581, 12582, 12583);
            server.HandlerFactory = new DefaultRequestHandlerFactory<handler>();
            server.Start();

            if (server.Running)
            {
                Console.WriteLine("Server is running");
                foreach (var p in server.Prefixes)
                {
                    Console.WriteLine("Listening to " + p);
                }

            }
            else
            {
                Console.WriteLine("Server is not running");
            }
        }

        class handler : AbstractRequestHandler
        {
            public handler( HttpListenerRequest request ) : base(request)
            {
            }

            protected override byte[] HandleRequest( string url )
            {
                return Request.ContentEncoding.GetBytes("Here is HttpServer. You are requesting " + url);
            }
        }



    }
}

namespace RemotingObject
{
    public interface IRemotingObj
    {

        void Write( string msg,DateTime time );
    }

    public class RemotingObj : MarshalByRefObject, IRemotingObj
    {
        private DateTime LastTime;
        public void Write( string msg ,DateTime time)
        {
            Console.WriteLine(time.ToString() + "    " + msg+"    last time:"+LastTime.ToString());
            LastTime = DateTime.Parse(time.ToString());
            Console.WriteLine(LastTime.ToString());
        }
    }
}

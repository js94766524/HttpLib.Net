using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.HttpServer;

namespace ConsoleTest
{
    class Program
    {
        static void Main( string[] args )
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
            Console.ReadLine();
        }

        class handler : AbstractRequestHandler
        {
            public handler( HttpListenerRequest request ) : base(request)
            {
            }

            protected override byte[] HandleRequest( string url )
            {
                return Request.ContentEncoding.GetBytes("handle " + url);
            }
        }
    }
}

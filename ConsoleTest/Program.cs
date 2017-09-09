using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.Server;

namespace ConsoleTest
{
    class Program
    {
        static void Main( string[] args )
        {

            HttpServer server = new HttpServer(12580,null);
            server.HandlerFactory = new DefaultRequestHandlerFactory<handler>();
            server.Start();
            if (server.Running)
            {
                Console.WriteLine("Server is running");
                Console.WriteLine("Listening to " + server.URL);

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
                return Request.ContentEncoding.GetBytes("handler!");
            }
        }
    }
}

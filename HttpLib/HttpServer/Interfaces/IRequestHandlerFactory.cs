using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpLib.HttpServer.Interfaces
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler GetHandler( HttpListenerRequest request );
    }
}

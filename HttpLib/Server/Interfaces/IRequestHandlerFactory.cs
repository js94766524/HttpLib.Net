using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpLib.Server.Interfaces
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler GetHandler( HttpListenerRequest request );
    }
}

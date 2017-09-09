using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.Server.Interfaces;

namespace HttpLib.Server
{
    public class DefaultRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler GetHandler( HttpListenerRequest request )
        {
            return new DefaultRequestHandler(request);
        }
    }

    public class DefaultRequestHandlerFactory<T> : IRequestHandlerFactory where T:IRequestHandler
    {
        public IRequestHandler GetHandler( HttpListenerRequest request )
        {
            var constr = typeof(T).GetConstructor(new Type[] { typeof(HttpListenerRequest) });
            return constr.Invoke(new object[] { request }) as IRequestHandler;
        }
    }
}

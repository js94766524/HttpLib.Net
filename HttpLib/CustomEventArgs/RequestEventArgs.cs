using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpLib.CustomEventArgs
{
    public class RequestEventArgs : BaseCustomEventArgs
    {
        public HttpListenerContext Context { get; set; }

        public RequestEventArgs( HttpListenerContext context )
        {
            Context = context;
        }
    }
}

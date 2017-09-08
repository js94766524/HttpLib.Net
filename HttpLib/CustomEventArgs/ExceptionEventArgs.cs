using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLib.CustomEventArgs
{
    public class ExceptionEventArgs:BaseCustomEventArgs
    {
        private Exception e;

        public ExceptionEventArgs( Exception e )
        {
            this.e = e;
        }
        
        public Exception Error { get; set; }
    }
}

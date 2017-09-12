using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLib.EventArgs
{
    public class ExceptionEventArgs : BaseCustomEventArgs
    {
        public ExceptionEventArgs( Exception e )
        {
            this.Error = e;
        }

        public Exception Error { get; set; }
    }
}

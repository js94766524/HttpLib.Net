using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLib.EventArgs
{
    public class BaseCustomEventArgs : System.EventArgs
    {
        public bool Handled { get; set; }
    }
}

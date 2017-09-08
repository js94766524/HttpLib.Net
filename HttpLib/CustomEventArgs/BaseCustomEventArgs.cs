using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLib.CustomEventArgs
{
    public class BaseCustomEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}

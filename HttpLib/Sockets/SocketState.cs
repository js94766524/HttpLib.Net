using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace HttpLib.Sockets
{
    internal class SocketState
    {
        internal Socket socket { get; private set; }
        internal string Point { get; set; }

        internal SocketState( Socket socket )
        {
            this.socket = socket;
            Point = socket.RemoteEndPoint.ToString();
        }
    }
}

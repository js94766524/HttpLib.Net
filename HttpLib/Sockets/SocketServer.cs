using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HttpLib.Sockets
{
    public class SocketServer
    {
        private IPAddress IP;
        private IPEndPoint Point;
        private Socket ServerSocket;
        private Thread ListeningThread;
        private Dictionary<string, Socket> SocketDict = new Dictionary<string, Socket>();
        private List<Thread> workThreadList = new List<Thread>();

        public int MaxClientCount { get; set; }
        public Encoding Encoding { get; set; }

        public event ClientDel NewClientConnected;
        public event ClientDel ClientDisConnected;
        public event HandleReceiveDel OnReceivedData;

        public SocketServer( int port, string ip = "127.0.0.1" )
        {
            MaxClientCount = 10;
            Encoding = Encoding.UTF8;

            IP = IPAddress.Parse(ip);
            Point = new IPEndPoint(IP, port);
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ListeningThread = new Thread(Listening);
            ListeningThread.IsBackground = true;
        }

        private void Listening()
        {
            while (true)
            {
                Socket clientSocket = ServerSocket.Accept();
                string point = clientSocket.RemoteEndPoint.ToString();
                SocketDict[point] = clientSocket;

                Thread receiveThread = new Thread(ReceiveData);
                workThreadList.Add(receiveThread);
                receiveThread.Start(new SocketState(clientSocket));
            }
        }

        private void ReceiveData( object state )
        {
            SocketState socketState = state as SocketState;
            Socket client = socketState.socket;
            string point = socketState.Point;

            if (NewClientConnected != null) NewClientConnected.Invoke(point);

            byte[] buffer;

            while (true)
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    //获取本次通信的长度
                    buffer = new byte[8];
                    client.Receive(buffer);
                    long length = BitConverter.ToInt64(buffer, 0);
                    client.Send(Encoding.UTF8.GetBytes(SocketCommands.LENGTH_RECEIVED));

                    //如果数据总长度超过1024*1024则分段读取
                    long bufferLength = length;
                    if (length > 1024 * 1024) bufferLength = 1024 * 1024;

                    buffer = new byte[bufferLength];

                    //开始读取数据，写入MemoryStream
                    int count = 0;
                    while ((count = client.Receive(buffer)) > 0)
                    {
                        stream.Write(buffer, 0, count);
                    }

                    //将接受到的数据交给外部处理，触发OnReceivedData事件
                    byte[] receivedBytes = stream.ToArray();

                    if(OnReceivedData!= null)
                    {
                        OnReceivedData.Invoke(this, point, receivedBytes);
                    }

                }
                catch
                {
                    break;
                }
                finally
                {
                    stream.Close();
                }
            }

            SocketDict.Remove(point);
            if (ClientDisConnected != null) ClientDisConnected.Invoke(point);
        }

        public bool Start()
        {
            try
            {
                ServerSocket.Bind(Point);
                ServerSocket.Listen(MaxClientCount);
                ListeningThread.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }

    public delegate void ClientDel( string point );
    public delegate void HandleReceiveDel(SocketServer server, string point, byte[] data );
}

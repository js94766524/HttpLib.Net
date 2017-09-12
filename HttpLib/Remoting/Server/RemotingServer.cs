using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;

namespace HttpLib.Remoting.Server
{
    public static class RemotingServer
    {
        /// <summary>
        /// 注册TCP信道
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>是否注册成功</returns>
        public static bool RegisterChannel( int port )
        {
            TcpServerChannel channel = new TcpServerChannel(port);
            try
            {
                ChannelServices.RegisterChannel(channel, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 注册已知服务器类型
        /// </summary>
        /// <typeparam name="T">要注册的类型</typeparam>
        /// <param name="uri">对象地址，如果为null，则默认为类名</param>
        /// <param name="mode">默认为SingleCall模式</param>
        public static bool RegisterObject<T>( string uri = null, WellKnownObjectMode mode = WellKnownObjectMode.SingleCall ) where T : MarshalByRefObject
        {
            Type t = typeof(T);
            if (uri == null) uri = t.Name;
            try
            {
                RemotingConfiguration.RegisterWellKnownServiceType(t, uri, mode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

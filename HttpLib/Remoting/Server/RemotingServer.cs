using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
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
            //设置序列化权限
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = port;

            TcpServerChannel channel = new TcpServerChannel(props,provider);
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
        /// 如果使用SingleCall模式，无法保存变量，因为每次通信都是一个新的对象
        /// </summary>
        /// <typeparam name="T">要注册的类型</typeparam>
        /// <param name="uri">对象地址，如果为null，则默认为类名</param>
        /// <param name="mode">默认为Singleton模式</param>
        public static bool RegisterObject<T>( string uri = null, WellKnownObjectMode mode = WellKnownObjectMode.Singleton ) where T : MarshalByRefObject
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

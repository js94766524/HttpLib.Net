using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;

namespace HttpLib.Remoting.Client
{
    public static class RemotingClient
    {
        private static bool ChannelRegistered;

        /// <summary>
        /// 注册TCP信道
        /// </summary>
        private static void RegisterChannel()
        {
            try
            {
                ChannelServices.RegisterChannel(new TcpClientChannel(), true);
                ChannelRegistered = true;
            }
            catch
            {
                ChannelRegistered = false;
            }
        }

        /// <summary>
        /// 获取服务器端的远程对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="prefixUri">服务端地址，例如“tcp://localhost:6666/”</param>
        /// <param name="objectUri">类型地址，例如“RemotingObject”</param>
        /// <returns>服务器对象的代理</returns>
        public static T GetRemotingObj<T>( string prefixUri, string objectUri = null ) where T : MarshalByRefObject
        {
            if (!ChannelRegistered) RegisterChannel();
            Type t = typeof(T);
            if (objectUri == null) objectUri = t.Name;
            string url = (prefixUri.StartsWith("tcp://") ? "" : "tcp://") + prefixUri + (prefixUri.EndsWith("/") ? "" : "/") + objectUri;
            try
            {
                return Activator.GetObject(t, url) as T;
            }
            catch
            {
                return default(T);
            }
        }

    }
}

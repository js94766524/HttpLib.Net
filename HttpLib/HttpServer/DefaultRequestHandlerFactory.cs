using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.HttpServer.Interfaces;

namespace HttpLib.HttpServer
{
    /// <summary>
    /// 仅能创建DefaultRequestHandler的实例的默认工厂
    /// </summary>
    public class DefaultRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler GetHandler( HttpListenerRequest request )
        {
            return new DefaultRequestHandler(request);
        }
    }

    /// <summary>
    /// 使用反射获取泛型类型的构造方法来创建实例
    /// </summary>
    /// <typeparam name="T">IRequestHandler的实现类，需要含有符合“仅有一个HttpListenerRequest参数”要求的构造方法</typeparam>
    public class DefaultRequestHandlerFactory<T> : IRequestHandlerFactory where T:IRequestHandler
    {
        public IRequestHandler GetHandler( HttpListenerRequest request )
        {
            var constr = typeof(T).GetConstructor(new Type[] { typeof(HttpListenerRequest) });
            return constr.Invoke(new object[] { request }) as IRequestHandler;
        }
    }
}

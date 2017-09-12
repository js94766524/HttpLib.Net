using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpLib.HttpServer.Interfaces;

namespace HttpLib.HttpServer
{
    class DefaultRequestHandler : AbstractRequestHandler
    {
        public DefaultRequestHandler( HttpListenerRequest request ) : base(request) { }

        protected override byte[] HandleRequest( string url )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("TIME:").AppendLine(DateTime.Now.ToString());
            sb.Append("URL:").AppendLine(url);
            sb.Append("FROM:").AppendLine(Request.RemoteEndPoint.ToString());
            sb.Append("METHOD:").AppendLine(Request.HttpMethod);
            sb.Append("KEEP_ALIVE:").AppendLine(Request.KeepAlive.ToString());
            sb.Append("CONTENT_LENGTH:").AppendLine(Request.ContentLength64.ToString());
            sb.Append("CONTENT_TYPE:").AppendLine(Request.ContentType);
            if (Params != null)
            {
                sb.AppendLine("PARAMS:");
                foreach (string key in Params)
                {
                    sb.Append("    ").Append(key).Append("=").AppendLine(Params[key]);
                }
            }
            if (FormItems != null)
            {
                sb.AppendLine("FORM_ITEMS:");
                foreach (var kv in FormItems)
                {
                    var item = kv.Value;
                    sb.Append("    ");
                    if (item.ItemType == FormItemType.Text)
                    {
                        sb.Append(item.Name).Append("=").Append(item.GetDataAsString(Request.ContentEncoding));
                    }
                    else
                    {
                        sb.Append(item.Name).Append(" is a file. File name is ").Append(item.FileName);
                    }
                    sb.AppendLine();
                }
            }
            return Request.ContentEncoding.GetBytes(sb.ToString());
        }

    }
}

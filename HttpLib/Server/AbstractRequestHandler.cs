using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HttpLib.Server.Interfaces;

namespace HttpLib.Server
{
    public abstract class AbstractRequestHandler : IRequestHandler
    {
        public HttpListenerRequest Request { get; set; }
        public NameValueCollection Params { get; set; }
        public Dictionary<string, MultipartFormItem> FormItems { get; set; }

        public AbstractRequestHandler( HttpListenerRequest request )
        {
            Request = request;
        }

        public bool IsPostingForm()
        {
            return Request.ContentType.Contains("boundary=");
        }

        public byte[] Handle()
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    HandleGet();
                    break;
                case "POST":
                    HandlePost();
                    break;
                default:
                    break;
            }
            return HandleRequest(Request.Url.AbsolutePath);
        }

        protected abstract byte[] HandleRequest( string url );

        private void HandleGet()
        {
            string api = Request.Url.AbsolutePath;
            Params = Request.QueryString;
        }

        private void HandlePost()
        {
            if (IsPostingForm())
            {
                try
                {
                    HttpMultipartFormParser formParser = new HttpMultipartFormParser(Request);
                    FormItems = formParser.ParseIntoElementList();
                }
                catch
                {
                    ParseNameValuePost();
                }
            }
            else
            {
                ParseNameValuePost();
            }
        }

        private void ParseNameValuePost()
        {
            string ps = Request.ContentEncoding.GetString(Request.ReadInputStream());
            Params = Tools.ParseNameValues(ps);
        }
    }



}

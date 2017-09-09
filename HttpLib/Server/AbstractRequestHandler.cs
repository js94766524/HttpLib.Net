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
    /// <summary>
    /// 抽象RequestHandler类，封装了解析参数等过程，可以在实现抽象方法时直接调用GetParam方法或GetFormItem方法
    /// </summary>
    public abstract class AbstractRequestHandler : IRequestHandler
    {
        /// <summary>
        /// 是否将数据解析为参数或表单项目
        /// </summary>
        public bool ParseParams { get; set; }
        /// <summary>
        /// 请求对象
        /// </summary>
        protected HttpListenerRequest Request { get; set; }
        /// <summary>
        /// 参数集合，如果没有发送参数则为null
        /// </summary>
        protected NameValueCollection Params { get; set; }
        /// <summary>
        /// 表单项目集合，如果不是表单提交则为null
        /// </summary>
        protected Dictionary<string, MultipartFormItem> FormItems { get; set; }

        /// <summary>
        /// 构造函数，针对每一个请求都应有一个独立的Handler来处理
        /// </summary>
        /// <param name="request">请求对象</param>
        public AbstractRequestHandler( HttpListenerRequest request )
        {
            ParseParams = true;
            Request = request;
        }

        /// <summary>
        /// 判断请求是否为表单提交
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFormSubmit()
        {
            return Request.ContentType.ToLower().Contains("boundary=");
        }

        /// <summary>
        /// 实现IRequestHandler接口，处理请求
        /// </summary>
        /// <returns>返回数据的byte数组</returns>
        public byte[] Handle()
        {
            if (ParseParams) HandleParams();
            return HandleRequest(Request.Url.AbsolutePath);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        protected string GetParam( string key )
        {
            if (Params != null && Params.AllKeys.Contains(key)) return Params[key];
            else return null;
        }

        /// <summary>
        /// 获取表单提交的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <returns>项目</returns>
        protected MultipartFormItem GetFormItem( string key )
        {
            if (FormItems != null && FormItems.ContainsKey(key)) return FormItems[key];
            else return null;
        }

        /// <summary>
        /// 将数据解析为参数
        /// </summary>
        protected void HandleParams()
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    HandleGetParams();
                    break;
                case "POST":
                    HandlePostParams();
                    break;
            }
        }

        /// <summary>
        /// 解析Get方法提交的参数
        /// </summary>
        private void HandleGetParams()
        {
            string api = Request.Url.AbsolutePath;
            Params = Request.QueryString;
        }

        /// <summary>
        /// 解析Post方法提交的参数或表单项目
        /// </summary>
        private void HandlePostParams()
        {
            if (IsFormSubmit())
            {
                try
                {
                    MultipartFormParser formParser = new MultipartFormParser(Request);
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

        /// <summary>
        /// 解析Post方法提交的参数
        /// </summary>
        private void ParseNameValuePost()
        {
            string ps = Request.ContentEncoding.GetString(Request.ReadInputStream());
            Params = Tools.ParseNameValues(ps);
        }

        /// <summary>
        /// 处理请求的业务方法
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <returns>结果数据的byte数组</returns>
        protected abstract byte[] HandleRequest( string url );

        /// <summary>
        /// 实现IDisposable接口，释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Request = null;

            if (Params != null)
            {
                Params.Clear();
                Params = null;
            }

            if (FormItems != null)
            {
                FormItems.Clear();
                FormItems = null;
            }
        }
    }
}

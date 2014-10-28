using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZhongDing.Web.HttpHandle
{
    /// <summary>
    /// Summary description for AsyncBusinessHandler
    /// </summary>
    public class AsyncBusinessHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
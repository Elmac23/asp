using System;
using System.Web;

namespace l2z1
{
    public class EchoHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.AppendHeader("Content-type", "text/html");
            context.Response.Write("handler obs³uguje " + context.Request.Url);
            context.Response.End();
        }
    }

}


using System;
using System.Web;

namespace WebApplication3
{
    public class CHandler : IHttpHandler
    {
       
        public bool IsReusable
        {  
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect("B.aspx");
        }

    }
}

using System;
using System.Web;

namespace Lista3
{
    public class Global : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {            DataContextManager.DisposeCurrent();
        }
    }
}

using System;
using System.Web;

namespace Lista3
{
    public class Global : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Nothing here for now        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Correct place to dispose per-request disposable resources stored in Context.Items            DataContextManager.DisposeCurrent();
        }
    }
}

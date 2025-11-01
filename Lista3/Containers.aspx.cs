using System;
using System.Web;
using System.Web.UI;

namespace Lista3
{
    public partial class Containers : AuthPage
    {
        private const string AppCounterKey = "App.Counter";
        private const string PseudoSingletonKey = "App.PseudoSingleton";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Show current values
            object appVal = Application[AppCounterKey];
            lblApp.Text = "Application[App.Counter] = " + (appVal ?? "(null)");

            object sessVal = Session["Session.Counter"];
            lblSession.Text = "Session[Session.Counter] = " + (sessVal ?? "(null)");

            object itemsVal = Context.Items["Items.Counter"];
            lblItems.Text = "Items[Items.Counter] = " + (itemsVal ?? "(null)");

            // Pseudo-singleton info if exists
            var ps = Application[PseudoSingletonKey] as PseudoSingleton;
            lblSingleton.Text = ps != null ? ("Pseudo-singleton.Value = " + ps.Value) : "(brak pseudo-singletona)";
        }

        protected void btnIncApp_Click(object sender, EventArgs e)
        {
            // Example of synchronizing access to Application state
            lock (Application)
            {
                int v = 0;
                if (Application[AppCounterKey] != null) int.TryParse(Application[AppCounterKey].ToString(), out v);
                v++;
                Application[AppCounterKey] = v;
            }
        }

        protected void btnIncSession_Click(object sender, EventArgs e)
        {
            int v = 0;
            if (Session["Session.Counter"] != null) int.TryParse(Session["Session.Counter"].ToString(), out v);
            v++;
            Session["Session.Counter"] = v;
        }

        protected void btnTransferWithItems_Click(object sender, EventArgs e)
        {
            // Items is per-request: set a value and Server.Transfer to Target.aspx to read it within same request.
            Context.Items["Items.Counter"] = "przekazane przez Items: " + DateTime.Now.ToString("o");
            Server.Transfer("Target.aspx", false);
        }

        protected void btnCreateSingleton_Click(object sender, EventArgs e)
        {
            // Pseudo-singleton stored in Application with double-check locking
            if (Application[PseudoSingletonKey] == null)
            {
                lock (Application)
                {
                    if (Application[PseudoSingletonKey] == null)
                    {
                        Application[PseudoSingletonKey] = new PseudoSingleton { Value = "created at " + DateTime.Now.ToString("o") };
                    }
                }
            }
        }
    }

    // Simple class used as pseudo-singleton stored in Application
    public class PseudoSingleton
    {
        public string Value { get; set; }
    }
}

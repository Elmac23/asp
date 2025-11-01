using System;
using System.Web;
using System.Web.UI;

namespace Lista3
{
    // Base page that enforces naive session-based authentication.
    public abstract class AuthPage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnforceAuthentication();
        }

        private void EnforceAuthentication()
        {
            var ctx = HttpContext.Current;
            if (ctx == null) return;

            // Allow access to login page and static resources
            var appRelative = ctx.Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
            if (string.Equals(appRelative, "~/Login.aspx", StringComparison.OrdinalIgnoreCase)) return;

            // If session contains user info, allow; otherwise redirect to login with returnUrl
            if (ctx.Session != null && ctx.Session["User"] != null) return;

            string returnUrl = ctx.Request.RawUrl ?? "~/";
            string target = "~/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(returnUrl);
            ctx.Response.Redirect(target, false);
        }
    }
}

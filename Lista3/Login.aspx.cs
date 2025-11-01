using System;
using System.Web;
using System.Web.UI;

namespace Lista3
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Very naive authentication: accept any non-empty login/password            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "Podaj login i has³o.";
                return;
            }

            // Store user info in Session            Session["User"] = txtLogin.Text;

            // Redirect back to returnUrl if present            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(returnUrl);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}

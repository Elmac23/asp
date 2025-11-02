using System;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Lista3
{
    public partial class ApiDemo : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var headers = Request.Headers;
            var sb = new StringBuilder();
            sb.AppendLine("<ul>");
            foreach (string key in headers)
            {
                var val = headers[key] ?? string.Empty;
                sb.AppendFormat("<li><strong>{0}</strong>: {1}</li>",
                    HttpUtility.HtmlEncode(key),
                    HttpUtility.HtmlEncode(val));
            }
            sb.AppendLine("</ul>");
            RequestHeadersLiteral.Text = sb.ToString();
            Response.AddHeader("X-MyApp-Info", "ApiDemo;ts=" + DateTime.UtcNow.ToString("o"));
            lblResponseHeaderInfo.Text = "Dodano nag³ówek 'X-MyApp-Info' z wartoœci¹ znacznikow¹ (UTC).";

            // Mapowanie œcie¿ki wzglêdnej na fizyczn¹
            string relative = "~/App_Data"; // przyk³adowa œcie¿ka wzglêdem katalogu aplikacji
            string physical = Server.MapPath(relative);
            bool exists = System.IO.Directory.Exists(physical) || System.IO.File.Exists(physical);
            MappedPathLiteral.Text = HttpUtility.HtmlEncode(relative) + " -> " + HttpUtility.HtmlEncode(physical) + (exists ? " (istnieje)" : " (nie istnieje)");

            // statyczne odniesienie do bie¿¹cego kontekstu
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                HttpContextInfoLiteral.Text = "HttpContext.Current dostêpny. Request.Url = " + HttpUtility.HtmlEncode(ctx.Request.Url.ToString());
            }
            else
            {
                HttpContextInfoLiteral.Text = "HttpContext.Current jest null (niektóre niestandardowe w¹tki lub kontekstowe operacje mog¹ go nie udostêpniaæ).";
            }
        }
    }
}

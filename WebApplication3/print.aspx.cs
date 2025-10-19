using System;
using System.Text;
using System.Web;
using System.Web.UI;

namespace TaskSubmission
{
    public partial class print : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var model = Session["Submission"] as SubmissionModel;
            if (model == null)
            {
                // brak danych — przekieruj z powrotem
                Response.Redirect("start.aspx");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>Imię</th><td>" + HttpUtility.HtmlEncode(model.FirstName) + "</td></tr>");
            sb.AppendLine("<tr><th>Nazwisko</th><td>" + HttpUtility.HtmlEncode(model.LastName) + "</td></tr>");
            sb.AppendLine("<tr><th>Data</th><td>" + HttpUtility.HtmlEncode(model.Date) + "</td></tr>");
            sb.AppendLine("<tr><th>Nazwa zajęć</th><td>" + HttpUtility.HtmlEncode(model.Course) + "</td></tr>");
            sb.AppendLine("<tr><th>Numer zestawu</th><td>" + model.SetNumber + "</td></tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("<h3>Punkty</h3>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>Zadanie</th><th>Punkty</th></tr>");
            for (int i = 0; i < model.Points.Length; i++)
            {
                sb.AppendLine($"<tr><td>Zadanie {i + 1}</td><td>{model.Points[i]}</td></tr>");
            }
            sb.AppendLine("</table>");

            LiteralContent.Text = sb.ToString();
        }
    }
}

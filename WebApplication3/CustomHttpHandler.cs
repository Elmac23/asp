using System;
using System.IO;
using System.Web;

namespace WebApplication3
{
    public class CustomHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            //pobieranie parametrów GET i POST
            string fromGet = context.Request.QueryString["x"];
            string fromPost = context.Request.Form["x"];

            response.ContentType = "text/html";

            response.Write("<html><body><h1>Echo</h1>");
            response.Write($"<p><b>Adres:</b> {request.Url}</p>");
            response.Write($"<p><b>Metoda:</b> {request.HttpMethod}</p>");

            response.Write("<h3>Nagłówki:</h3><ul>");
            foreach (var key in request.Headers.AllKeys)
            {
                response.Write($"<li>{key}: {request.Headers[key]}</li>");
            }
            response.Write("</ul>");

            if (request.HttpMethod == "POST")
            {
                using (var reader = new StreamReader(request.InputStream))
                {
                    string body = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(body))
                        response.Write($"<h3>Treść POST:</h3><pre>{body}</pre>");
                }
            }

            response.Write("</body></html>");
        }
    }
}

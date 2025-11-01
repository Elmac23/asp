using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lista3
{
    public partial class Cookies : AuthPage
    {
        private const string TestCookieName = "MojeCookie";
        private const string ServerTestCookieName = "CookieTest_Wiarygodny";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Jeśli przychodzimy po przekierowaniu testu serwera, obsłuż to:
            // (Jeśli QueryString zawiera "serverTest=1" to jesteśmy w drugiej fazie testu)
            if (!IsPostBack)
            {
                if (Request.QueryString["serverTest"] == "1")
                {
                    // Sprawdzamy, czy cookie testowe dotarło od klienta
                    var c = Request.Cookies[ServerTestCookieName];
                    if (c != null)
                    {
                        lblServerTestResult.Text = "Wiarygodny test serwera: COOKIE ZAAKCEPTOWANE przez przeglądarkę (znaleziono cookie).";
                    }
                    else
                    {
                        lblServerTestResult.Text = "Wiarygodny test serwera: COOKIE NIE ZOSTAŁO ZWRÓCONE przez przeglądarkę.";
                    }
                }
            }
        }

        protected void btnSetCookie_Click(object sender, EventArgs e)
        {
            // Tworzymy cookie o nazwie MojeCookie
            var cookie = new HttpCookie(TestCookieName)
            {
                Value = "wartosc-przykladowa",
                Expires = DateTime.Now.AddDays(7), // wygasa za 7 dni
                HttpOnly = true, // niedostępne dla skryptów JS
                Secure = Request.IsSecureConnection // tylko dla https jeśli połączenie jest secure
            };

            // Dodajemy cookie do odpowiedzi
            Response.Cookies.Add(cookie);

            lblInfo.Text = "Ustawiono cookie '" + TestCookieName + "' (Expires +7 dni, HttpOnly = true).";
            lblCookieValue.Text = "";
        }

        protected void btnReadCookie_Click(object sender, EventArgs e)
        {
            var c = Request.Cookies[TestCookieName];
            if (c != null)
            {
                lblCookieValue.Text = string.Format("Zawartość cookie '{0}': Value = '{1}', Expires = {2}, HttpOnly = {3}",
                    TestCookieName,
                    HttpUtility.HtmlEncode(c.Value),
                    c.Expires == DateTime.MinValue ? "(brak daty wygaśnięcia - sesyjne)" : c.Expires.ToString(),
                    c.HttpOnly);
            }
            else
            {
                lblCookieValue.Text = "Cookie '" + TestCookieName + "' nie istnieje w żądaniu (Request.Cookies).";
            }
            lblInfo.Text = "";
        }

        protected void btnDeleteCookie_Click(object sender, EventArgs e)
        {
            // Aby usunąć cookie po stronie klienta, nadpisujemy cookie z datą przeszłą
            if (Request.Cookies[TestCookieName] != null)
            {
                var del = new HttpCookie(TestCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(del);
                lblInfo.Text = "Cookie '" + TestCookieName + "' zostało ustawione do usunięcia (Expires w przeszłości).";
                lblCookieValue.Text = "";
            }
            else
            {
                lblInfo.Text = "Cookie '" + TestCookieName + "' nie występowało w Request.Cookies — mimo to wysyłam polecenie usunięcia (nadpisanie).";
            }
        }

        protected void btnServerTest_Click(object sender, EventArgs e)
        {
            // Wiarygodny test: ustaw cookie i przekieruj na tę samą stronę z paramem serverTest=1.
            // Po przekierowaniu sprawdzamy Request.Cookies[ServerTestCookieName].
            var testCookie = new HttpCookie(ServerTestCookieName)
            {
                Value = "1",
                // Nie ustawiamy Expires -> cookie sesyjne; pokazuje to działanie w bieżącej sesji przeglądarki
                HttpOnly = true
            };
            Response.Cookies.Add(testCookie);

            // Przekierowanie na tę samą stronę z parametrem wskazującym drugą fazę testu
            string current = Request.Url.AbsolutePath;
            string qs = "?serverTest=1";
            Response.Redirect(current + qs, false);
            // Response.End nie jest konieczny jeśli false; kończy się metoda.
        }
    }
}
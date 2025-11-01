<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cookies.aspx.cs" Inherits="Lista3.Cookies" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Demo ciastek (HttpCookie) - Zadanie 2</title>
    <meta charset="utf-8" />
    <script>
        // Naiwny test po stronie klienta
        function naiveCookieCheck() {
            var out = document.getElementById('clientResult');
            out.innerText = "navigator.cookieEnabled = " + navigator.cookieEnabled;
        }
        window.onload = naiveCookieCheck;
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Demo HttpCookie — dodawanie / odczyt / usuwanie</h2>

        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
        <br /><br />

        <asp:Button ID="btnSetCookie" runat="server" Text="Ustaw cookie 'MojeCookie'" OnClick="btnSetCookie_Click" />
        &nbsp;
        <asp:Button ID="btnReadCookie" runat="server" Text="Odczytaj cookie 'MojeCookie'" OnClick="btnReadCookie_Click" />
        &nbsp;
        <asp:Button ID="btnDeleteCookie" runat="server" Text="Usuń cookie 'MojeCookie'" OnClick="btnDeleteCookie_Click" />
        <br /><br />

        <asp:Label ID="lblCookieValue" runat="server" Text=""></asp:Label>
        <br /><br />

        <h3>Test obsługi ciastek</h3>
        <p>
            Naiwny test (po stronie klienta):
            <span id="clientResult" style="font-weight:bold"></span>
        </p>

        <p>
            Wiarygodny test (po stronie serwera):
            <asp:Button ID="btnServerTest" runat="server" Text="Uruchom test serwera (ustaw cookie i sprawdź)" OnClick="btnServerTest_Click" />
        </p>

        <asp:Label ID="lblServerTestResult" runat="server" Text=""></asp:Label>

        <hr />
        <h4>Jak to działa (krótkie info):</h4>
        <ul>
            <li>Ustawienie cookie: Response.Cookies.Add(new HttpCookie(...))</li>
            <li>Odczyt: Request.Cookies["nazwa"]</li>
            <li>Usuwanie: nadpisanie cookie z datą przeszłą (Expires = DateTime.Now.AddDays(-1))</li>
            <li>Wiarygodny test: ustawiamy cookie testowe i robimy Redirect; jeśli w kolejnym żądaniu Request.Cookies zawiera to cookie → cookies są obsługiwane w bieżącej sesji.</li>
        </ul>
    </form>
</body>
</html>
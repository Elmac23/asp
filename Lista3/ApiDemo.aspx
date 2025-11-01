<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApiDemo.aspx.cs" Inherits="Lista3.ApiDemo" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>API Request/Server/Response Demo</title>
    <meta charset="utf-8" />
</head>
<body>
    <form id="form1" runat="server">
        <h2>Request / Server / Response API — Demo</h2>

        <h3>Request headers (odczyt):</h3>
        <asp:Literal ID="RequestHeadersLiteral" runat="server" />

        <h3>Dodany nag³ówek odpowiedzi (przyk³ad):</h3>
        <asp:Label ID="lblResponseHeaderInfo" runat="server" Text="(nag³ówek zostanie dodany do odpowiedzi)"></asp:Label>

        <h3>Mapowanie œcie¿ki (Server.MapPath):</h3>
        <asp:Literal ID="MappedPathLiteral" runat="server" />

        <h3>HttpContext.Current (statyczne odniesienie):</h3>
        <asp:Literal ID="HttpContextInfoLiteral" runat="server" />

        <hr />
        <p>Uwaga: aby zobaczyæ dodany nag³ówek odpowiedzi, sprawdŸ nag³ówki odpowiedzi (np. w DevTools &gt; Network).</p>
        <p>Login page (<a href="Login.aspx">Login.aspx</a>) is excluded from authentication requirement.</p>
    </form>
</body>
</html>
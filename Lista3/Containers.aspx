<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Containers.aspx.cs" Inherits="Lista3.Containers" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Containers Demo - Application / Session / Items</title>
    <meta charset="utf-8" />
</head>
<body>
    <form id="form1" runat="server">
        <h2>Demonstracja kontenerów serwerowych</h2>

        <h3>Application (globalny, wspó³dzielony przez wszystkie ¿¹dania)</h3>
        <asp:Label ID="lblApp" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnIncApp" runat="server" Text="Inkrementuj Application.Counter (z lock)" OnClick="btnIncApp_Click" />
        <br /><br />

        <h3>Session (dla pojedynczego u¿ytkownika)</h3>
        <asp:Label ID="lblSession" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnIncSession" runat="server" Text="Inkrementuj Session.Counter" OnClick="btnIncSession_Click" />
        <br /><br />

        <h3>Items (przechowuje dane dla bie¿¹cego ¿¹dania)</h3>
        <asp:Label ID="lblItems" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnTransferWithItems" runat="server" Text="Ustaw Items + Server.Transfer (przeka¿ w obrêbie jednego ¿¹dania)" OnClick="btnTransferWithItems_Click" />
        <br /><br />

        <h3>Pseudo-singleton (przechowywany w Application z synchronizacj¹)</h3>
        <asp:Label ID="lblSingleton" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnCreateSingleton" runat="server" Text="Uzyskaj/utwórz pseudo-singleton w Application" OnClick="btnCreateSingleton_Click" />
n        <hr />
        <p>Wskazówki:</p>
        <ul>
            <li>Application: wspó³dzielone miêdzy wszystkimi u¿ytkownikami; dostêp powinien byæ synchronizowany (Application.Lock/Unlock lub inny mechanizm lock), by zapobiec wyœcigom.</li>
            <li>Session: przechowuje dane per-u¿ytkownik; zwykle nie wymaga globalnego locku, choæ równoleg³e ¿¹dania tej samej sesji mog¹ wymagaæ uwagi.</li>
            <li>Items: krótkotrwa³e dane zwi¹zane z bie¿¹cym ¿¹daniem; nie zachowuje siê miêdzy ¿¹daniami (ale mo¿na przekazaæ przez Server.Transfer).</li>
        </ul>
    </form>
</body>
</html>
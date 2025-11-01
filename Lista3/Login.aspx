<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Lista3.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Logowanie (naiwne)</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <div>
            Login: <asp:TextBox ID="txtLogin" runat="server" />
        </div>
        <div>
            Has³o: <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>
        <asp:Button ID="btnLogin" runat="server" Text="Zaloguj" OnClick="btnLogin_Click" />
    </form>
</body>
</html>
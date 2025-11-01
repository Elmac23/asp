<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataDemo.aspx.cs" Inherits="Lista3.DataDemo" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Context Demo</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>DataContext per-request demo</h2>
        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnUseData" runat="server" Text="Use DataContext.Current and show Id" OnClick="btnUseData_Click" />
    </form>
</body>
</html>
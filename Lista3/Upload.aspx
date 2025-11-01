<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Lista3.Upload" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Binary Upload & Dynamic Download Demo</title>
    <meta charset="utf-8" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <h2>Przeœlij plik i pobierz opis (XML)</h2>
        <asp:FileUpload ID="fileUpload" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Wyœlij i pobierz opis" OnClick="btnUpload_Click" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <p>Po wys³aniu pliku serwer wygeneruje plik XML opisuj¹cy przes³any plik i zwróci go jako plik do pobrania.</p>
    </form>
</body>
</html>
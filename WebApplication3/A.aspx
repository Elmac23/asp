<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="A.aspx.cs" Inherits="WebApplication3.A" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>

    <a href="B.aspx">Idź do B</a>


    <form action="B.aspx" method="post">
    <input type="text" name="info" />
    <button type="submit">Wyślij</button>
</form>
</body>
</html>

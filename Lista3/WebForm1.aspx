<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Lista3.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
   <form id="myForm" runat="server" method="post" action="Target.aspx">
  <input type="hidden" name="p1" value="v1" />
  <input type="hidden" name="p2" value="v2" />
</form>

<a href="#" id="postLink">żądanie POST (link)</a>
<input type="button" id="getButton" value="żądanie GET (button)" />

<script>
  document.getElementById('postLink').addEventListener('click', function(e){
    e.preventDefault();
    document.getElementById('myForm').submit();
  });

  document.getElementById('getButton').addEventListener('click', function(){
    // zbuduj querystring (możesz również pobrać wartości z formularza)
    document.location.href = 'Target.aspx?p1=v1&p2=v2';
  });
</script>
</body>
</html>

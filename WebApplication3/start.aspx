<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="start.aspx.cs" Inherits="TaskSubmission.start" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Formularz zgłoszenia zadań</title>
<meta charset="utf-8" />
<style>
body { font-family: Arial, sans-serif; padding: 20px; }
.field { margin-bottom: 10px; }
label { display:inline-block; width: 120px; }
input[type=text], input[type=date] { width: 250px; }
.points { width: 60px; }
.error { color: red; }
.panel { border: 1px solid #ccc; padding: 12px; border-radius: 6px; max-width: 900px; }
</style>
</head>
<body>
<form id="form1" runat="server">
<div class="panel">
<h2>Formularz zgłoszenia zadań</h2>


<asp:Label ID="LabelError" runat="server" CssClass="error" />


<div class="field">
<label for="TextBoxFirstName">Imię:</label>
<asp:TextBox ID="TextBoxFirstName" runat="server" /></div>


<div class="field">
<label for="TextBoxLastName">Nazwisko:</label>
<asp:TextBox ID="TextBoxLastName" runat="server" /></div>


<div class="field">
<label for="TextBoxDate">Data:</label>
<asp:TextBox ID="TextBoxDate" runat="server" placeholder="YYYY-MM-DD" /></div>


<div class="field">
<label for="TextBoxCourse">Nazwa zajęć:</label>
<asp:TextBox ID="TextBoxCourse" runat="server" /></div>


<div class="field">
<label for="TextBoxSetNumber">Numer zestawu:</label>
<asp:TextBox ID="TextBoxSetNumber" runat="server" CssClass="points" /></div>


<h3>Wyniki (10 zadań)</h3>
<table>
<tr>
<th>Zadanie</th>
<th>Punkty</th>
</tr>
<tr><td>Zadanie 1</td><td><asp:TextBox ID="TextBoxP1" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 2</td><td><asp:TextBox ID="TextBoxP2" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 3</td><td><asp:TextBox ID="TextBoxP3" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 4</td><td><asp:TextBox ID="TextBoxP4" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 5</td><td><asp:TextBox ID="TextBoxP5" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 6</td><td><asp:TextBox ID="TextBoxP6" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 7</td><td><asp:TextBox ID="TextBoxP7" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 8</td><td><asp:TextBox ID="TextBoxP8" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 9</td><td><asp:TextBox ID="TextBoxP9" runat="server" CssClass="points" /></td></tr>
<tr><td>Zadanie 10</td><td><asp:TextBox ID="TextBoxP10" runat="server" CssClass="points" /></td></tr>
</table>


<br />
<asp:Button ID="ButtonSubmit" runat="server" Text="Zapisz i drukuj" OnClick="ButtonSubmit_Click" />
<asp:Button ID="ButtonClear" runat="server" Text="Wyczyść" OnClick="ButtonClear_Click" CausesValidation="false" />


<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="false" ShowSummary="true" />


<p style="margin-top:12px; font-size: 0.9em; color:#555">Uwaga: brak wpisu = 0 punktów. Pola z punktami muszą być liczbami całkowitymi (&gt;= 0).</p>
</div>
</form>
</body>
</html>
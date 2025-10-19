<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print.aspx.cs" Inherits="TaskSubmission.print" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pasek zgłoszenia - wydruk</title>
    <meta charset="utf-8" />
    <style>
        body { font-family: Arial, sans-serif; padding: 20px; }
        .print-area { max-width: 800px; margin: auto; }
        table { width: 100%; border-collapse: collapse; }
        th, td { border: 1px solid #333; padding: 6px; text-align: left; }
        .no-print { margin-top: 12px; }
        @media print {
            .no-print { display: none; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="print-area">
            <h2>Pasek zgłoszenia zadań</h2>

            <asp:Literal ID="LiteralContent" runat="server" />

            <div class="no-print">
                <button type="button" onclick="window.print();">Drukuj</button>
                <asp:HyperLink ID="LinkBack" runat="server" NavigateUrl="start.aspx">Powrót do edycji</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>

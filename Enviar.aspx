<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enviar.aspx.cs" Inherits="DashBoard.Enviar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload CssClass="form-control" ID="FileUpload1" runat="server" />
            <br />
            <asp:Button CssClass="btn btn-info" ID="Button1" runat="server" Text="Enviar" OnClick="Button1_Click"/>
        </div>
    </form>
</body>
</html>

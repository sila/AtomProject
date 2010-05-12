<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestApp.Default" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Dialog" TagPrefix="dialog" %>
<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Slider" TagPrefix="slider" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="themes/base/ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dialog:JQDialog ID="JQDialog1" Title="kur" runat="server" AutoOpen="true" ShowAnimation="Explode"
            HideAnimation="Explode">
            <Template>
                <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                Kur</Template>
        </dialog:JQDialog>
    </div>
    </form>
</body>
</html>

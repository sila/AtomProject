﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.aspx.cs" Inherits="TestApp.DatePicker" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.DatePicker" TagPrefix="calendar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JQDatePicker</title>
    <link href="themes/base/ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function Button1_onclick()
        {
            $('#temp').val(new Date());
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float: left">
            <input type="text" name="temp" id="temp" value="" />
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
            <input id="Button1" type="button" value="button" onclick="return Button1_onclick()" />
        </div>
        <div style="float: left">
            <calendar:JQDatePicker ID="JQDatePicker2" runat="server" Mode="Calendar" />
        </div>
        <div style="float: left">
            <calendar:JQDatePicker ID="JQDatePicker1" runat="server" Mode="DatePicker" />
        </div>
    </div>
    </form>
</body>
</html>

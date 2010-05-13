<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Slider.aspx.cs" Inherits="TestApp.WebForm2" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Slider" TagPrefix="slider" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JQSlider</title>
    <link href="themes/base/ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">    
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <slider:JQSlider ID="JQSlider1" runat="server" />
    </div>
    </form>
</body>
</html>

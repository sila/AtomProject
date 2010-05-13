<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Button.aspx.cs" Inherits="TestApp.Button" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Accordion" TagPrefix="accordion" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Button" TagPrefix="button" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JQButton<accordion:JQAccordion ID="JQAccordion1" runat="server">
    </accordion:JQAccordion>
    </title>    <link href="themes/base/ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">    
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <button:JQButton ID="JQButton1" runat="server" />
    </div>
    </form>
</body>
</html>

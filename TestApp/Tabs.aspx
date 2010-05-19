<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tabs.aspx.cs" Inherits="TestApp.WebForm3" %>

<%@ Register Assembly="Atom.Web" Namespace="Atom.Web.UI.WebControls.Tabs" TagPrefix="tabs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JQTabs</title>
    <link href="themes/base/ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">    
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <tabs:JQTabs ID="JQTabs1" runat="server">
            <tabs:Tab Header="tab0">
                <Template>
                    0
                </Template>
            </tabs:Tab>
            <tabs:Tab Header="tab1">
                <Template>
                    1
                </Template>
            </tabs:Tab>
            <tabs:Tab Header="tab2">
                <Template>
                    2
                </Template>
            </tabs:Tab>
            <tabs:Tab Header="tab3">
                <Template>
                    3
                </Template>
            </tabs:Tab>
        </tabs:JQTabs>
    </div>
    </form>
</body>
</html>

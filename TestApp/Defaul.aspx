<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Defaul.aspx.cs" Inherits="TestApp.Defaul" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--
16x16="http://skypejournal.com/skype-chat16x16-on-white.png"
16x16="http://farm2.static.flickr.com/1336/1231021036_5839dc0e92.jpg?v=0"
16x16="http://stevenbstout.com/WorldCup2010/assets/images/icons/warning-16x16.png"
    --%>
    <script language="javascript" type="text/javascript">
        // <!CDATA[
    

        // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
            OnTextChanged="DropDownList1_TextChanged" AutoPostBack="true">
            <asp:ListItem>top</asp:ListItem>
            <asp:ListItem>bottom</asp:ListItem>
            <asp:ListItem>vertical</asp:ListItem>
        </asp:DropDownList>
  
        <br />
        <br />
        <br />
    </div>
    </form>
</body>
</html>

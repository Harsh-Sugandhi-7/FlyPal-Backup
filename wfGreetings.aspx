<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfGreetings.aspx.vb" Inherits="Flypal.wfGreetings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FlyPal Greetings</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9,10,11" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body style="text-align: center;">
    <table width="100%">
        <tr>
            <td>
                <img id="imgGreeting" runat="server" src=""
                    alt="FlyPal Greetings" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("imgGreeting").src = "<%=mCompanyDetailForGreetings.GreetingPath %>";
        })       
    </script>
</body>
</html>

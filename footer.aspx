<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="footer.aspx.vb" Inherits="Flypal.footer" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>footer</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
			if("<%= not HttpContext.Current.Session("StyleSheet") is nothing %>"=="True")
			{
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
			}
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstable1" id="Table1" style="width: 100%; height: 25px">
        <tr>
            <!--<TD class="clsfooterLeft" style="WIDTH: 76px" align="right" background="file:///C:\Inetpub\wwwroot\FlyPalInv\aspflypal\logo1.gif"></TD>-->
            <td>
                <asp:Label ID="lblDateFormat" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Date Format is in (MM/DD/YYYY)</asp:Label>
            </td>
            <td class="clsfooterRight">
                <asp:Image ID="FlagImage" runat="server" ImageUrl="images\indian-flag.gif" Height="16px"
                    Visible="False"></asp:Image>
                <asp:Label ID="Label7" runat="server" CssClass="clsLabelauto"></asp:Label>
            </td>
        </tr>
    </table>
    &nbsp;
    </form>
    <script>
        var d = new Date();
        document.getElementById("Label7").innerHTML = 'Copyrights @ ' + d.getFullYear() + ' Bytzsoft Technologies Pvt. Ltd.';
    </script>
</body>
</html>

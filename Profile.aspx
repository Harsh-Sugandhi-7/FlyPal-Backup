<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Profile.aspx.vb" Inherits="Flypal.Profile" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Profile</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" src="DATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {

            window.open(FileName, "_top", "fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto");
        }
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            var popID = $('a.poplight[href^=#]').attr('rel');
            var popURL = $('a.poplight[href^=#]').attr('href');


            var query = popURL.split('?');
            var dim = query[1].split('&amp;');
            var popWidth = dim[0].split('=')[1];


            $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"><img src="images/close2.png"  style="margin: -47 -49px 0 0;position:relative;" class="btn_close" title="Close" alt="Close" /></a>');


            var popMargTop = ($('#' + popID).height() + 80);
            var popMargLeft = ($('#' + popID).width() + 80) / 2;

            $('#' + popID).css({
                'margin-top': '12%',
                'margin-left': -popMargLeft
            });
            $pos = $('#' + popID).position();
            var top = $pos.top;
            var body = $('#' + popID).outerHeight(true);

            var temp = top + body + 50;
            var hight = ($(window).height() > temp ? $(window).height() : temp);

            $('body').append('<div id="fade"></div>');
            $('#fade').css({ 'filter': 'alpha(opacity=70)', 'width': '100%', 'height': hight, 'background': '#000' }).fadeIn();

            return false;
        });

        $('a.close').live('click', function () {
            $('#fade , .popup_block').fadeOut(function () {
                $('#fade, a.close').remove()
                var url = "index.aspx";
                $(location).attr('href', url);
            });
            return false;

        });
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <!-- ================================================================ -->
        <table class="clstable1" align="center" border="0">
            <tr>
                <td valign="middle" colspan="2">
                    <!-- ================================================================ -->
                    <table id="tabInner1" class="clstablemain" border="0">
                        <tr>
                            <td align="left">
                                <a id="#?" class="poplight" href="#?w=500" rel="popup_name"></a>
                                <div id="popup_name" class="popup_block" align="center">
                                    <table id="Table2" class="clstable1" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">User Information</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFlyPalVersion1" CssClass="clsLabelauto" runat="server">Date :</asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblCDate" runat="server" CssClass="clsLabelAuto"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReleaseNo1" CssClass="clsLabelauto" runat="server">Name :</asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblCName" runat="server" CssClass="clsLabelauto"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPassExpInfo" runat="server" CssClass="clsLabelauto" Visible="False">Password Expiry :</asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblPassExpiryDetail" runat="server" CssClass="clsLabelauto" Visible="False"></asp:Label></td>
                                        </tr>
                                        <asp:PlaceHolder ID="phstyle" runat="server" Visible="false">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelauto">Choose Theme :</asp:Label>

                                                </td>
                                                <td>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbStyleSheet" runat="server" CssClass="clsComboBox">
                                                                    <asp:ListItem Value="Styles.css">Default</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetBlue.css">Blue</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetPink.css">Pink</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetGreen.css">Green</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetGray.css">Gray</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetBrick.css">Brick</asp:ListItem>
                                                                    <asp:ListItem Value="css/StyleSheetMagenta.css">Magenta</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:Button ID="btnApplyTheme" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Apply"
                                                                    ToolTip="Click to Apply selected theme"></asp:Button></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblCompanyDetails" runat="server" CssClass="clsLabelHeader">List of User Roles </asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:DataGrid ID="dgUser" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="UserRoleID" HeaderText="ID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RoleName" HeaderText="Role Name"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblLoginDetail" runat="server" CssClass="clsLabelHeader">Last 5 Login Details</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:DataGrid ID="dgLast5LoginUserDetails" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="EventLogID" HeaderText="EventLogID"></asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="UserID" HeaderText="UserID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SrNo" HeaderText="Sr. No."></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="LogInTimeFormatted" HeaderText="LogIn Date Time"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IPAddress" HeaderText="IP Address"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Last 5 Event Details</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:DataGrid ID="dgLast5EventLogDetails" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                                        <asp:BoundColumn Visible="False" DataField="EventLogID" HeaderText="EventLogID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SrNo" HeaderText="Sr. No."></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DateTimeFormatted" HeaderText="Date Time"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ModuleName" HeaderText="Module"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Name" HeaderText="Action"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="btnOK" runat="server" CssClass="clsButton" ToolTip="Click to Close" Text="Close"
                                                    Visible="false"></asp:Button></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <!-- ================================================================ -->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

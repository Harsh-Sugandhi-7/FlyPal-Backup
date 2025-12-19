<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectMaintenanceActivity.aspx.vb"
    Inherits="Flypal.wfSelectMaintenanceActivity" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Maintenance Activity</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Maintenance Activity</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Selection of Maintenance Activity</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table id="Table1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbRemovalComp" runat="server" CssClass="clsRadioButton" Text="Removal Comp"
                                                GroupName="a" Enabled="False"></asp:RadioButton>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbInstallComp" runat="server" CssClass="clsRadioButton" Text="Install Comp"
                                                GroupName="a" Enabled="False"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 23px">
                                            <asp:RadioButton ID="rdbRemovalAssembly" runat="server" CssClass="clsRadioButton"
                                                Text="Removal Assembly" GroupName="a" Enabled="False"></asp:RadioButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="height: 23px">
                                            <asp:RadioButton ID="rdbInstallAssembly" runat="server" CssClass="clsRadioButton"
                                                Text="Install Assembly" GroupName="a" Enabled="False"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbAssemblyService" runat="server" CssClass="clsRadioButton"
                                                Text="Assembly Service" GroupName="a"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAssembly" runat="server" Visible="False" Width="36px">L1</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbComponentService" runat="server" CssClass="clsRadioButton"
                                                Text="Component Service" GroupName="a"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbAssemblyInspection" runat="server" CssClass="clsRadioButton"
                                                Text="Assembly Inspection" GroupName="a"></asp:RadioButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbComponentInspection" runat="server" CssClass="clsRadioButton"
                                                Text="Component Inspection" GroupName="a"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbAssemblyDirective" runat="server" CssClass="clsRadioButton"
                                                Text="Assembly Directive" GroupName="a"></asp:RadioButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbComponentDirective" runat="server" CssClass="clsRadioButton"
                                                Text="Component Modification" GroupName="a"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                                <table id="Table2" cellspacing="0" align="right">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnNext" CssClass="clsButton" runat="server" Text="Next" ToolTip="Click to go onto next Page"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCancel" CssClass="clsButton" runat="server" Text="Back" ToolTip="Back to Previous Page"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

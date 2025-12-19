<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptABCAnalysis.aspx.vb"
    Inherits="Flypal.wfrptABCAnalysis" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Graph Report</title>
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
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="0" topmargin="5" rightmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">ABC Analysis Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtFromDate" runat="server" Visible="False"></uc1:SICalendar>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtToDate" runat="server" Visible="False"></uc1:SICalendar>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Enter the Percentages</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblQtyA" runat="server" CssClass="clsLabelAuto">Quantity A %</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQtyA" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblValA" runat="server" CssClass="clsLabelAuto">Value A %</asp:Label>
                            </td>
                            <td colspan="5" align="left">
                                <asp:TextBox ID="txtValA" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblQtyB" runat="server" CssClass="clsLabelAuto">Quantity B %</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQtyB" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblValB" runat="server" CssClass="clsLabelAuto">Value B %</asp:Label>
                            </td>
                            <td colspan="5" align="left">
                                <asp:TextBox ID="txtValB" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblQtyC" runat="server" CssClass="clsLabelAuto">Quantity C %</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQtyC" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblValC" runat="server" CssClass="clsLabelAuto">Value C %</asp:Label>
                            </td>
                            <td colspan="5" align="left">
                                <asp:TextBox ID="txtValC" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                            </td>
                            <td colspan="3" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblQty" runat="server" CssClass="clsLabelAuto"></asp:Label>
                            </td>
                            <td colspan="3" align="left">
                                <asp:Label ID="lblVal" runat="server" CssClass="clsLabelAuto"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonlong"
                                                    ToolTip="Click to Display Current Searching criterias." Text="Current Criteria">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Display Report"
                                                    Text="Display"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Close"
                                                    Text="Close" CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForComplience.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForComplience" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Complience Report</title>
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
<body text="#ea0e0c" bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0"
    ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="4" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Search criteria for Compliance</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsValidationSummary"
                                    ControlToValidate="cmbModelList" OnServerValidate="CustomValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Model</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbModelList" runat="server"  DataTextField="ModelName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step II. Selection of Type</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 1px" align="left">
                            </td>
                            <td style="height: 1px" align="left">
                                <asp:Label ID="lblModificationType" runat="server" CssClass="clsLabelAuto">Directive Type</asp:Label>
                            </td>
                            <td style="height: 1px" align="left" colspan="2">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbModificationType" runat="server"
                                    DataTextField="CodeType" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblDirectiveType1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right" colspan="3">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                    Text="Current Criteria" ToolTip="Click to display Current Searching criterias"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"  Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                    ToolTip="Click to close Search criteria for Compliance screen " CausesValidation="False">
                                                </asp:Button>
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfElectronicLogBook.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfElectronicLogBook" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Electronic Log Register</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Electronic Log Register</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvAssembly" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ControlToValidate="cmbAircraftAssembly" ErrorMessage="Select the Assembly" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlFromDate" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                        Width="100px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtFromDate"
                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlToDate" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                        Width="100px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtToDate"
                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Assembly</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px">
                            </td>
                            <td>
                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                            </td>
                            <td colspan="3" align="left">
                                <asp:UpdatePanel runat="server" ID="upnlAssembly" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbAircraftAssembly" runat="server" CssClass="clsComboBox3"
                                            DataValueField="ID" DataTextField="ModelSerialNo">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkShowCompliance" runat="server" CssClass="clsCheckBox" 
                                            Text="Show Compliance">
                                        </asp:CheckBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" colspan="5" align="left">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px" align="left">
                            </td>
                            <td colspan="4" align="left">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px" align="left">
                            </td>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 20px" align="left">
                            </td>
                            <td style="height: 20px" colspan="2" align="left">
                                <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td style="height: 20px" colspan="2" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong"
                                                    Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False"
                                                    ToolTip="Click to close the Electronic Log Register"></asp:Button>
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

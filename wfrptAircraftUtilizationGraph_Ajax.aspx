<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftUtilizationGraph_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAircraftUtilizationGraph_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aircraft Utilization Graph</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS" type="text/javascript">
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
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Aircraft Utilization Graph</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc2:TabContainer ID="TabContainer1" runat="server" class="clstablelistin" AutoPostBack="true">
                                    <cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            Graph I
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="tblInner" class="clstablelistin">
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary1" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                                    CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                    ValidationGroup="a" ControlToValidate="" ClientValidationFunction="ValidateChkList"
                                                                    ErrorMessage="Select atleast one Aircraft"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                                                    Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkAircraftListCount"
                                                                    ValidationGroup="a" ErrorMessage="Report does not allow more than 10 Aircrafts, please break Aircrafts into multiple report prints."></asp:CustomValidator>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Month and Year</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblAircraftStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto">Month and Year</asp:Label>
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBoxMedium_Ajax">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBoxsmall_Ajax">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <asp:CheckBox ID="chkSelectAll" CssClass="clsRadioButton" runat="server" Text="Select All" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBoxList ID="ChklistAircraft" runat="server" CssClass="clsComboBox" DataTextField="RegNo"
                                                            DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal" Width="400px">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step III. Selection of Graph Report </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td colspan="2"></td>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rdoByFlyingHour" runat="server" Checked="True" CssClass="clsRadioButton"
                                                            GroupName="Gr1" Text="By Flying Hours" />
                                                        <asp:RadioButton ID="rdoByFlyingDay" runat="server" CssClass="clsRadioButton" GroupName="Gr1"
                                                            Text="By Flying Days" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Show Classification-wise Report </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">"Classification-wise" Utilization</asp:Label>
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <asp:CheckBox ID="chkClassification" runat="server" CssClass="clsLabelAuto"
                                                            ToolTip='Check to get Classification-wise Utilization' AutoPostBack="false"
                                                            TextAlign="Left"></asp:CheckBox>
                                                    </td>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td colspan="3" align="left">
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td colspan="3" align="left">
                                                            <asp:Label ID="lblyear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td colspan="3" align="left">
                                                            <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                                Width="500px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td align="left" colspan="3">
                                                            <asp:Label ID="lblGraphType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="4">
                                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                                <table border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                                CssClass="clsButtonLong_Ajax" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" ValidationGroup="a"
                                                                                Text="Display" ToolTip="Click to Display Report" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Text="Close" ToolTip="Click to close the Day Log Book Summary screen" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="TabPanel2" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            Graph II
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="Table1" class="clstablelistin">
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary2" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Fields"
                                                                    CssClass="clsValidationSummary" ValidationGroup="b"></asp:ValidationSummary>
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                                    Display="None" ClientValidationFunction="ValidateChkListGraphII" ErrorMessage="Select atleast one Aircraft"
                                                                    ValidationGroup="b"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                                    ClientIDMode="Static" ClientValidationFunction="BetweenDatesValidation" ValidationGroup="b"
                                                                    Display="None"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="cvCategory1" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                    ControlToValidate="" ClientValidationFunction="ValidateChkAircraftIIListCount"
                                                                    ValidationGroup="b" ErrorMessage="Report does not allow more than 10 Aircrafts, please break Aircrafts into multiple report prints."></asp:CustomValidator>
                                                                <script type="text/javascript">
                                                                    function ValidateChkListGraphII(source, args) {
                                                                        args.IsValid = false;
                                                                        $("#<%=ChklistAircraftGraphII.ClientID %>").find(":checkbox").each(function () {
                                                                            if ($(this).attr("checked")) {
                                                                                args.IsValid = true;
                                                                                return;
                                                                            }
                                                                        });
                                                                    }
                                                                    function ValidateChkAircraftIIListCount(source, args) {
                                                                        var count = 0;
                                                                        args.IsValid = false;
                                                                        $("#<%=ChklistAircraftGraphII.ClientID %>").find(":checkbox").each(function () {
                                                                            if ($(this).attr("checked")) {
                                                                                count += 1;
                                                                            }
                                                                        });
                                                                        if (count <= 10) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }

                                                                    }
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <asp:Label ID="lblStepI" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblStarI" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Width="64px">From Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtStartDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'txtStartDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="txtStartDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" CssClass="clsLabelAuto" runat="server">To Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtEndDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'txtEndDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtEndDate" ID="txtEndDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <asp:Label ID="lblStepII" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBox ID="chkSelectAllGraphII" CssClass="clsRadioButton" runat="server" Text="Select All" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraftStarGraphII" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAircraftGraphII" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBoxList ID="ChklistAircraftGraphII" runat="server" CssClass="clsComboBox"
                                                            DataTextField="RegNo" DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal"
                                                            Width="400px">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="Label2GraphII" runat="server" CssClass="clsLabelHeader">Step III. Selection of Graph Parameter </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblPeriod" runat="server" CssClass="clsLabelAuto">Period</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:UpdatePanel runat="server" ID="upnlParameters" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsComboBox_Ajax" ClientIDMode="Static"
                                                                                onChange="OpenHourTypes()">
                                                                                <asp:ListItem Value="0" Text="Hour"></asp:ListItem>
                                                                                <asp:ListItem Value="1" Text="Cycles"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Landings"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButton runat="server" ID="rdoTimeInair" Text="AirBorne Time" GroupName="a"
                                                                                            ClientIDMode="Static" Checked="true" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton runat="server" ID="rdoBlockTime" Text="Block Time" GroupName="a"
                                                                                            ClientIDMode="Static" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="lblStep4GraphII" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="lblSummaryGraphII" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        <asp:UpdatePanel ID="upnlCriteriaLabels" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td align="left"></td>
                                                                        <td align="left" colspan="3">
                                                                            <asp:Label ID="lblDates1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left"></td>
                                                                        <td colspan="3" align="left">
                                                                            <asp:Label ID="lblAircraftGraphII1" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                                                Width="500px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left"></td>
                                                                        <td align="left" colspan="3">
                                                                            <asp:Label ID="lblGraphTypeGraphII" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCurrentSearchCriteriaGraphII" runat="server" CausesValidation="False"
                                                                                CssClass="clsButtonLong_Ajax" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDisplayGraphII" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                                Text="Display" ToolTip="Click to Display Report" ValidationGroup="b" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseGraphII" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Text="Close" ToolTip="Click to close the Day Log Book Summary screen" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="TabPanel3" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            Graph III
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="Table2" class="clstablelistin">
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary3" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ValidationSummary ID="Validationsummary3" runat="server" HeaderText="Fill Up The Following Fields"
                                                                    CssClass="clsValidationSummary" ValidationGroup="c"></asp:ValidationSummary>
                                                                <asp:CustomValidator ID="CustomValidator3" runat="server" CssClass="clsLabelAuto"
                                                                    Display="None" ClientValidationFunction="ValidateChkListGraphIII" ErrorMessage="Select atleast one Aircraft"
                                                                    ValidationGroup="c"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="CustomValidator4" runat="server" CssClass="clsLabelAuto"
                                                                    Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkAircraftIIIListCount"
                                                                    ValidationGroup="c" ErrorMessage="Report does not allow more than 10 Aircrafts, please break Aircrafts into multiple report prints."></asp:CustomValidator>
                                                                <script type="text/javascript">
                                                                    function ValidateChkListGraphIII(source, args) {
                                                                        args.IsValid = false;
                                                                        $("#<%=ChklistAircraftGraphIII.ClientID %>").find(":checkbox").each(function () {
                                                                            if ($(this).attr("checked")) {
                                                                                args.IsValid = true;
                                                                                return;
                                                                            }
                                                                        });
                                                                    }
                                                                    function ValidateChkAircraftIIIListCount(source, args) {
                                                                        var count = 0;
                                                                        args.IsValid = false;
                                                                        $("#<%=ChklistAircraftGraphIII.ClientID %>").find(":checkbox").each(function () {
                                                                            if ($(this).attr("checked")) {
                                                                                count += 1;
                                                                            }
                                                                        });
                                                                        if (count <= 10) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }

                                                                    }
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <span id="Span1" class="clsLabelHeader">Step I. Selection of Dates</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <span id="Span2" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label6" runat="server" CssClass="clsLabelAuto">From Month and Year</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbFrmMonth" runat="server" CssClass="clsComboBoxMedium_Ajax">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="cmbFrmYear" runat="server" CssClass="clsComboBoxsmall_Ajax">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span class="clsLabelAuto">Till Next</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMonths" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                        Width="50px">
                                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="3" Value="3" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span class="clsLabelAuto">Months</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="left">
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBox ID="chkSelectAllGraphIII" CssClass="clsRadioButton" runat="server"
                                                            Text="Select All" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBoxList ID="ChklistAircraftGraphIII" runat="server" CssClass="clsComboBox"
                                                            DataTextField="RegNo" DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal"
                                                            Width="400px">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="lblStep4GraphIII" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <asp:Label ID="lblSummaryGraphIII" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        <asp:UpdatePanel ID="upnlCriteriaLabelsGIII" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td align="left"></td>
                                                                        <td align="left" colspan="3">
                                                                            <asp:Label ID="lblDatesGIII" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left"></td>
                                                                        <td colspan="3" align="left">
                                                                            <asp:Label ID="lblAircraftGraphIII" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                                                Width="500px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        <asp:UpdatePanel ID="upnlActionBtnGIII" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCurrentSearchCriteriaGraphIII" runat="server" CausesValidation="False"
                                                                                CssClass="clsButtonLong_Ajax" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criteria" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDisplayGraphIII" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                                Text="Display" ToolTip="Click to Display Report" ValidationGroup="c" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseGraphIII" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Text="Close" ToolTip="Click to close the screen" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                </cc2:TabContainer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <script language="javascript" type="text/javascript">

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                OpenHourTypes();
            });

        </script>
        <script type="text/javascript">
            function OpenHourTypes() {

                var dd = $get("cmbPeriod");

                if (dd.selectedIndex == 0) {
                    $get('rdoTimeInair').nextSibling.innerText = "AirBorne Time"
                    $get('rdoTimeInair').style.visibility = 'visible'

                    $get('rdoBlockTime').nextSibling.innerText = "Block Time"
                    $get('rdoBlockTime').style.visibility = 'visible'
                }
                else if (dd.selectedIndex > 0) {

                    $get('rdoTimeInair').nextSibling.innerText = "";
                    $get('rdoTimeInair').style.visibility = 'hidden'

                    $get('rdoBlockTime').nextSibling.innerText = ""
                    $get('rdoBlockTime').style.visibility = 'hidden'


                }
            }
        </script>
        <script type="text/javascript">

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                if (source.id == 'cvCommon') {
                    var fromdate = $("#txtStartDate").val();
                    var todate = $("#txtEndDate").val();
                }




                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }

            }

            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    async: false,
                    data: params,
                    beforeSend: OnBeforeSend,
                    success: onSuccess,
                    error: onError
                });
                return false;
                function onSuccess(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val(result);
                    $find(extenderid).set_Text(result);
                }

                function onError(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val('');
                    $find(extenderid).set_Text('');
                }
                function OnBeforeSend() {
                    $(elem).addClass('ac_loading');
                }
            }

        </script>
        <script type="text/javascript">
            function ValidateChkList(source, args) {
                args.IsValid = false;
                $("#<%=ChklistAircraft.ClientID %>").find(":checkbox").each(function () {


                    if ($(this).prop("checked")) {
                        args.IsValid = true;
                        return false; // break out of .each loop
                    }
                });
            }
            function ValidateChkAircraftListCount(source, args) {
                var count = 0;
                args.IsValid = false;
                $("#<%=ChklistAircraft.ClientID %>").find(":checkbox").each(function () {
                    if ($(this).attr("checked")) {
                        count += 1;
                    }
                });
                if (count <= 10) {
                    args.IsValid = true;
                    return;
                }

            }
        </script>
        <%--<script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=chkSelectAll.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAll.ClientID %>").prop("checked");
                $("#<%=ChklistAircraft.ClientID %>").find(":checkbox").each(function () {
                   
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked"); alert("dsdsf");
                    }

                });
            });
            return false;
        });

    </script>--%>

        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=chkSelectAll.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAll.ClientID %>").prop("checked");
                $("#<%=ChklistAircraft.ClientID %>").find(":checkbox").each(function () {
                    $(this).prop("checked", status);
                });
            });
            return false;
        });
        </script>

        <%-- <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=chkSelectAllGraphII.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAllGraphII.ClientID %>").attr("checked");
                $("#<%=ChklistAircraftGraphII.ClientID %>").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");
                    }

                });
            });
            return false;
        });

    </script>--%>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=chkSelectAllGraphII.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAllGraphII.ClientID %>").prop("checked");
                $("#<%=ChklistAircraftGraphII.ClientID %>").find(":checkbox").each(function () {
                    $(this).prop("checked", status);
                });
            });
            return false;
        });
        </script>

        <%--    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=chkSelectAllGraphIII.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAllGraphIII.ClientID %>").attr("checked");
                $("#<%=ChklistAircraftGraphIII.ClientID %>").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");
                    }

                });
            });
            return false;
        });

    </script>--%>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=chkSelectAllGraphIII.ClientID %>").click(function () {
                 var status = $("#<%=chkSelectAllGraphIII.ClientID %>").prop("checked");
                 $("#<%=ChklistAircraftGraphIII.ClientID %>").find(":checkbox").each(function () {
                     $(this).prop("checked", status);
                 });
             });
             return false;
         });
        </script>
    </form>
</body>
</html>

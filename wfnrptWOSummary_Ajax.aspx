<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnrptWOSummary_Ajax.aspx.vb"
    Inherits="Flypal.wfnrptWOSummary_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Work Order Summary</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">


        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Work Order Summary</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                            <%--<td colspan="2" align="right">
                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                        ToolTip="Click to display Current Searching criterias" Text="Current Criteria"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                        Text="Export to Excel"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" ToolTip="Click to display report"
                                                                        Text="Display"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" ToolTip="Click to Close Work Order Summary screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep1" class="clsLabelHeader">Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="75px">
                                    <span id="lblDateRange" class="clsLabel">Date Range</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                            <asp:ListItem Value="Last Week">Last 1 Week</asp:ListItem>
                                                            <asp:ListItem Value="Last Month">Last 1 Month</asp:ListItem>
                                                            <asp:ListItem Value="Last Quarter">Last 1 Quarter</asp:ListItem>
                                                            <asp:ListItem Value="Last Year">Last 1 Year</asp:ListItem>
                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>&nbsp;
                                                    </td>
                                                    <td>
                                                        <table id="Table2" border="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                                        ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>&nbsp;
                                                    </td>
                                                    <td>
                                                        <table id="Table3" border="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                        runat="server"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
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
                            <asp:PlaceHolder ID="phCustomer" runat="server">
                                <tr>
                                    <td colspan="2" align="left">
                                        <span id="lblStep2" class="clsLabelHeader">Selection of Customer</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="75px">
                                        <span id="lblSupplier" class="clsLabel">Customer</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbCustomer" runat="server"  DataValueField="ID"
                                            DataTextField="Name">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="pp1" runat="server" Visible='<%#IIf(AppSettings("ShowNewWOFlow") = "True", False, True) %>'>
                                <asp:PlaceHolder ID="phWOType" runat="server">
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblSelectionofCAMOThirdParty" class="clsLabelHeader" runat="server">Selection of CAMO/Third Party</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="75px">
                                        <span id="lblCAMOThirdParty" class="clsLabel" runat="server">CAMO / Third Party</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbTransType" runat="server" DataValueField="ID"
                                            DataTextField="Name">
                                            <asp:ListItem Text="(ALL)" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="CAMO" Value="89" ></asp:ListItem>
                                            <asp:ListItem Text="Third Party" Value="88"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                                </asp:PlaceHolder>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlWO" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Selection of W. O. No.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="75px">
                                                        <asp:Label ID="lblWONo" runat="server" CssClass="clsLabel">W. O. No.</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <table id="Table4">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWO" runat="server"  AutoPostBack="True"
                                                                        DataValueField="WOText" DataTextField="WOText">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtWONo" runat="server" Visible="False"
                                                                        MaxLength="4" ToolTip="Enter W.O. Number"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReportType" runat="server" CssClass="clsLabel" Visible="False">Report Type</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbReportType" runat="server" Visible='<%# AppSettings("ClientCode") = "Novo" %>'>
                                                                        <asp:ListItem Value="0">1. Work Order Register</asp:ListItem>
                                                                        <asp:ListItem Value="1">2. Work Order Register with Jobs and Tasks</asp:ListItem>
                                                                    </asp:DropDownList>
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
                                <td colspan="2" align="left">
                                    <span id="lblStep5" class="clsLabelHeader">Selection of Reg. No. or Model or
                                    Serial No.</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="75px">
                                    <span id="lblRegNo" class="clsLabel">Reg. No.</span>
                                </td>
                                <td align="left">
                                    <table id="Table6" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRegNo" runat="server" MaxLength="50"
                                                    ToolTip="Enter Reg. Number"></asp:TextBox>
                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="AutoCompleteExtender1" runat="server"
                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1000"
                                                    ServicePath="wfnrptWOSummary_Ajax.aspx" ServiceMethod="GetRegTextList" TargetControlID="txtRegNo"
                                                    UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                    OnClientShowing="ClientShowing">
                                                </cc2:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <span id="lblModel" class="clsLabel">Model</span>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModel" runat="server" MaxLength="50"
                                                    ToolTip="Enter Model Name"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" MaxLength="50"
                                                    ToolTip="Enter Serial Number"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <span id="Label1" class="clsLabelHeader">Selection of Status</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="75px">
                                    <span id="lblStatus" class="clsLabel">Status</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlWOStatus" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWOStatusList" runat="server" 
                                                Visible='<%#IIf(AppSettings("ShowNewWOFlow") = "True", False, True) %>' DataValueField="ID"
                                                DataTextField="Name">
                                                <asp:ListItem Text="(ALL)" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Submitted" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Complete" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Cancelled" Value="9"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server" DataTextField="Name"
                                                Visible='<%#IIf(AppSettings("ShowNewWOFlow") = "True", True, False) %>' DataValueField="ID"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <span id="lblStepVI" class="clsLabelHeader">Selection of WO. Job Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="75px">
                                    <span id="lblWoJobType" class="clsLabel">Job Type</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlWOJobType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWOJobType" runat="server" DataValueField="ID"
                                                            DataTextField="Name" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkOtherJob" runat="server" CssClass="clsCheckBox" Text="Other Job"
                                                            TextAlign="Left" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Span1" runat="server" Visible='<%# AppSettings("ClientCode") = "BRD" %>'
                                        CssClass="clsLabelHeader">Selection of FMC(Fixed Maintenance Contract) Work Order</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Span2" runat="server" Visible='<%# AppSettings("ClientCode") = "BRD" %>'
                                        CssClass="clsLabelAuto">WO. Type</asp:Label>
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbIsFMC" runat="server" Visible='<%# AppSettings("ClientCode") = "BRD" %>'>
                                                    <asp:ListItem Selected="True" Value="0">(All)</asp:ListItem>
                                                    <asp:ListItem Value="1">FMC</asp:ListItem>
                                                    <asp:ListItem Value="2">Non-FMC</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="chkForBillingPurpose" runat="server" TabIndex="22" Visible='<%# AppSettings("ClientCode") = "BRD" %>'
                                                    CssClass="clsCheckBox" Text="Billing Purpose" TextAlign="right" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblSortLabel" runat="server" CssClass="clsLabelHeader">Selection of Sorting</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSort" class="clsLabelAuto">Sort By</span>
                                </td>
                                <td align="left"> 
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSortBy" runat="server" >
                                        <asp:ListItem Value="0" Selected="True">WO Date</asp:ListItem>
                                        <asp:ListItem Value="1">WO Planned Date</asp:ListItem>
                                        <asp:ListItem Value="2">WO Completion Date</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbOrderBy" runat="server">
                                        <asp:ListItem Value="0">Ascending</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Descending</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table8" class="clsTable1">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblWONo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReportType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRegNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCompPartNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCompSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblJobType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblJobDescription1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWoJobType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblFMC" runat="server" CssClass="clsLabelAuto" Visible='<%# AppSettings("ClientCode") = "BRD" %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left"></td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                            ToolTip="Click to display Current Searching criterias" Text="Current Criteria"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                             Text="Export to Excel"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" ToolTip="Click to display report"
                                                            Text="Display"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to Close Work Order Summary screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
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
        <%--Date Validations--%>
        <script type="text/javascript">

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                var selectedDateIndex = $get("cmbDateRange").selectedIndex;
                if (selectedDateIndex == 6) {
                    args.IsValid = false;
                    var fromdate = $("#txtFromDate").val();
                    var todate = $("#txtToDate").val();
                    if (!todate) {
                        rfvToDate.isvalid = false;
                        return;
                    }
                    if (!fromdate) {
                        rfvFromDate.isvalid = false;
                        return;
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
    </form>
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
</body>
</html>

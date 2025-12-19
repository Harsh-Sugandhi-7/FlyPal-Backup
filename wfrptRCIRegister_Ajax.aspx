<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptRCIRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptRCIRegister_Ajax" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Receipt Register</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.js"></script>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Goods Receipt Register </asp:Label>
                                            </td>

                                            <%--<td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server" 
                                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" TabIndex="0"
                                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"
                                                                            Width="100px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server"  TabIndex="0"
                                                                            Text="Display" ToolTip="Click to display report" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False" 
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close the Goods Receipt Register screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" Height="72px" CssClass="clsValidationSummary"
                                        Width="440px" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
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
                                <td>
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I Selection of Goods Receipt</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReceiptCumInvoice" runat="server" CssClass="clsLabel" Width="100px">Goods Receipt</asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlReceiptCumInvoice" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbReceiptCumInvoice" runat="server"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabel" Width="100px">Date Range</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                            <asp:ListItem Value="Last Week">Last Week</asp:ListItem>
                                                            <asp:ListItem Value="Last Month">Last Month</asp:ListItem>
                                                            <asp:ListItem Value="Last Quarter">Last Quarter</asp:ListItem>
                                                            <asp:ListItem Value="Last Year">Last Year</asp:ListItem>
                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="45px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                            ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <td style="width: 19px">
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate"  Style="margin-left: 3px;" 
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Document & its Number</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDocType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDocType" runat="server" CssClass="clsLabelAuto" Width="100px">Doc. Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDocType" CssClass="clsTextBoxTagSearchComboNewstyle" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Receipt</asp:ListItem>
                                                            <asp:ListItem Value="2">Issue</asp:ListItem>
                                                            <asp:ListItem Value="3">Order</asp:ListItem>
                                                            <asp:ListItem Value="4">Invoice</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="75">
                                                        <asp:Label ID="lblDocTypeNo" runat="server" CssClass="clsLabelAuto" Visible="False">Receipt No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReceiptText" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtInvoiceText" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtOrderText" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIssueText" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtNo" runat="server" Visible="False"
                                                            MaxLength="4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlFromType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblType" runat="server" CssClass="clsLabel" Width="100px">Type</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbType" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="14">Vendor</asp:ListItem>
                                                            <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="8">Store</asp:ListItem>
                                                            <asp:ListItem Value="16">WorkShop</asp:ListItem>
                                                            <asp:ListItem Value="17">WorkOrder</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="75px">
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False" Width="75px">Supplier</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSupplier" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAircraft" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkShop" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkOrderText" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataTextField="LocationStore"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtWONo" runat="server"  Visible="False"
                                                            MaxLength="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Selection of Int. Receipt No. , Release Note No. or D.C. No.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Width="100px">Int. Rece. No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIntReceiptNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td width="75px">
                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabel">Rel. Note No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReleaseNoteNo" runat="server"  MaxLength="200"></asp:TextBox>
                                            </td>
                                            <td width="75px">
                                                <asp:Label ID="Label5" runat="server" CssClass="clsLabel" Width="59px">D. C. No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDCNo" CssClass="clsTextBoxTagSearch" runat="server" MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Custom Bill of Entry</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAWBNo" runat="server" CssClass="clsLabel" Height="25px" Width="100px">Custom Bill of Entry</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCustomBillofEntry" runat="server" 
                                                    MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step VII. Selection of Receiving Store</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStoreCount" runat="server" class="clsLabelAuto" Font-Bold="true"
                                        Font-Size="XX-Small" ForeColor="DarkBlue"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlReceivingStore" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReceivingStore" runat="server" CssClass="clsLabel" Width="100px">Receiving Store</asp:Label>
                                                    </td>
                                                    <td width="250px">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbReceivingStore" runat="server" 
                                                            DataTextField="LocationStore" DataValueField="ID" AutoPostBack="True" Width="250px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkOnlyReceivedinSelectedStore" runat="server" CssClass="clsCheckBox"
                                                            AutoPostBack="True" Text="Only Received in Selected Store"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="optWithEffRate" runat="server" CssClass="clsRadioButton" Text="With EffRate"
                                                            GroupName="grRate"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="optWithRate" runat="server" CssClass="clsRadioButton" Text="With Rate"
                                                            GroupName="grRate" Checked="True" Enabled="true"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VIII. Selection of Status & Report Format</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Width="100px">Status</asp:Label>
                                                    </td>
                                                    <td width="250px">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Opened</asp:ListItem>
                                                            <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                            <asp:ListItem Value="4">Canceled</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkDetail" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Text="Detailed Report" Checked="True"></asp:CheckBox>
                                                        <asp:RadioButton ID="optPortrait" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Portrait" GroupName="grOrientation" Checked="True"></asp:RadioButton>
                                                        <asp:RadioButton ID="optLandscape" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Landscape" GroupName="grOrientation"></asp:RadioButton>
                                                        <asp:CheckBox ID="chkWithoutinvoicingDetail" runat="server" CssClass="clsCheckBox"
                                                            Text="Without invoicing Detail"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" CssClass="clsLabelAuto" Width="80px" Visible='<%# iif(AppSettings("ClientCode") = "Taj" ,True,False) %>'>Format</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true"
                                                            Visible='<%# iif(AppSettings("ClientCode") = "Taj" ,True,False) %>'>
                                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep9" runat="server" CssClass="clsLabelHeader">Step IX. Selection of Part Type</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlpartType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPartType" runat="server" CssClass="clsLabelAuto" Width="100px">Part Type</asp:Label>
                                                    </td>
                                                    <td width="250px">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPartType" runat="server" DataTextField="Name"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkWithDocketCharges" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Text="With docket charges"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span1" class="clsLabelHeader">Step X. Selection to show only valued parts
                                        with landing rate greater than entered value </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlHighValue" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkHighValue" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                            Text="Show only valued parts with landing rate greater than " />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCEffectiveRate" runat="server" 
                                                            Enabled="false" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep10" runat="server" CssClass="clsLabelHeader">Step XI. Selection of Part Number/Description</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto" Width="100px">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtPartDescription" runat="server" 
                                                    ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span3" class="clsLabelHeader">Step XII. Enter text to be display at bottom
                                        line of report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblText" runat="server" CssClass="clsLabelAuto" Width="100px">Text</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtBottomLine" runat="server" AutoPostBack="False" 
                                                    Text='<%# " Submitted By : " + User.Identity.Name %>' MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlIsOHRepairRecords" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblOHRepairRecords" runat="server" CssClass="clsLabelHeader" Visible="False">Step XIII. Selection of OH/Repair Receipts Records</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="69px">
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOHRepairRecords" runat="server" CssClass="clsCheckBox" Text="Return From OH/Repair"
                                                            Visible="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValuedStores" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" visible='<%# AppSettings("ClientCode")="Deccan" %>'>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblValuedStores" runat="server" CssClass="clsLabelHeader" Visible="false">Step XIV. Selection For Valued, Non-Valued Stores</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="69px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Width="100px" Visible="false">Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStoreType" runat="server" Visible="false">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Valued</asp:ListItem>
                                                            <asp:ListItem Value="2">Non-Valued</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                        <asp:Label ID="lblCase" runat="server" CssClass="clsLabelHeader" Visible="false">If case of multiple part received in one invoice with valued and non-valued receiving store, whole invoice will get consider in report.</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="70%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep11" runat="server" CssClass="clsLabelHeader">Step XV. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReceiptCumInvoice1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblIntReceiptNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReleaseNoteno" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDCNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCustomBillofEntries" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPartType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblReceivingStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server"  TabIndex="0"
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"
                                                                 />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to close the Goods Receipt Register screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="upnlHiddenField" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <input type="hidden" id="hidden_DocType" runat="server" />
                <input type="hidden" id="hidden_DocTextType" runat="server" />
                <input type="hidden" id="hidden_FromType" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                    background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                    z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
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
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            var Doc_Type = document.getElementById("hidden_DocType").value;
            var Doc_TextType = document.getElementById("hidden_DocTextType").value;
            var FromType_Type = document.getElementById("hidden_FromType").value;

            $("#<%=txtReceiptText.ClientID%>,#<%=txtIssueText.ClientID%>,#<%=txtOrderText.ClientID%>,#<%=txtWorkOrderText.ClientID%>,#<%=txtInvoiceText.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=' + Doc_Type + '&TextType=' + Doc_TextType, {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });

            $("#<%=txtAircraft.ClientID%>,#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=' + FromType_Type, {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });


            $("#<%=txtWorkShop.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=WorkShop', {
                width: 252,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

            $("#<%=txtPartDescription.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

        });
    </script>
</body>
</html>

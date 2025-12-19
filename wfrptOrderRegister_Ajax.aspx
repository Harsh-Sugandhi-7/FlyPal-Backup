<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptOrderRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptOrderRegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Purchase Order Register</asp:Label>
                                                    </td>
                                                    <%--<td align="right">
                                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                                    Text="Current Criteria" ToolTip="Click to display Current Searching criterias"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server"
                                                                                    Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" TabIndex="0" runat="server"
                                                                                    Text="Display" ToolTip="Click to display report"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                                    ToolTip="Click to close the Requisition Register screen" CausesValidation="False"></asp:Button>
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
                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Selection of Order Type</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlOrderType" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td width="100">
                                                                <asp:Label ID="lblOrderType" runat="server" CssClass="clsLabelAuto">Type Of Order</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbOrderType" runat="server"  AutoPostBack="True">
                                                                    <asp:ListItem Value="00">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="05">Outright</asp:ListItem>
                                                                    <asp:ListItem Value="38">Overhaul / Repair</asp:ListItem>
                                                                    <asp:ListItem Value="39">Rental / Lease</asp:ListItem>
                                                                    <asp:ListItem Value="31">Exchange</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsCalibrationOrder" runat="server" CssClass="clsLabelAuto" Text="Calibration Order"
                                                                    TextAlign="Right" Enabled="false"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Selection of Date</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="730px">
                                            <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px" width="100px">
                                                                <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabel" Width="100px">Date Range</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server"  AutoPostBack="True">
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
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td style="width: 19px">
                                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;" 
                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                    runat="server" CausesValidation="true"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Selection of Supplier</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblSupplier" runat="server" CssClass="clsLabelAuto">Supplier</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSupplier" runat="server" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Selection of Supplier Quotation No. & Internal Order No.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblQuotNo" runat="server" CssClass="clsLabelAuto">Supp. Quot. No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtQuotNo" runat="server" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblIntOrdNo" runat="server" CssClass="clsLabel">Int. Order  No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIntOrderNo" runat="server"  MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Selection of Order No.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblOrderTextNo" runat="server" CssClass="clsLabelAuto">Order No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtOrderTextList" runat="server"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtOrderNo" runat="server" MaxLength="8"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtAmend" runat="server" MaxLength="4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Selection of Status & Report Format</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td xml:lang="100">
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Width="100px">Status</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server" >
                                                            <asp:ListItem Value="0">Opened And Authorized</asp:ListItem>
                                                            <asp:ListItem Value="1">Opened</asp:ListItem>
                                                            <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                            <asp:ListItem Value="3">Amended</asp:ListItem>
                                                            <asp:ListItem Value="4">Canceled</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkDetail" runat="server" CssClass="clsCheckBox" Text="Detailed Report"
                                                            Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="optPortrait" runat="server" CssClass="clsRadioButton" Text="Portrait"
                                                            Checked="True" GroupName="grOrientation"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="optLandscape" runat="server" CssClass="clsRadioButton" Text="Landscape"
                                                            GroupName="grOrientation"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Selection of Priority</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblPriority" runat="server" CssClass="clsLabelAuto">Priority</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPriority" runat="server" DataTextField="Name"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Selection of Aircraft</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto" Width="100px">Aircraft</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAircraft" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep8" runat="server" CssClass="clsLabelHeader" Visible="False">Selection Of Expenses</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblExpenses" runat="server" CssClass="clsLabelAuto" Visible="False">Expenses</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbExpenses" runat="server"  Visible="False">
                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Schedule Expenses</asp:ListItem>
                                                            <asp:ListItem Value="2">Nonschedule Expenses</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Selection of Part Number</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPOTowards" runat="server" CssClass="clsLabelHeader" Visible='<%# iif( AppSettings("ClientCode")="CE" ,True,False) %>'>Selection for PO. Towards</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblPOToward" runat="server" CssClass="clsLabelAuto" Visible='<%# iif( AppSettings("ClientCode")="CE" ,True,False) %>'>PO. Towards</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPOTowards" runat="server" Visible='<%# iif( AppSettings("ClientCode")="CE" ,True,False) %>'
                                                            DataTextField="Name" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblIsPBHPurchaseStep" runat="server" CssClass="clsLabelHeader">Select To Get PBH Purchase Order</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="100">
                                                        <asp:Label ID="lblIsPBHPurchase" runat="server" CssClass="clsLabelAuto">Is PBH Purchase</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsPBHPurchase" runat="server" CssClass="clsLabelAuto" TextAlign="Right">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Display Report</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblTransType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblQuotNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblIntOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPriority1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblExpenses1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>
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
                                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                        Text="Current Criteria" ToolTip="Click to display Current Searching criterias">
                                                                    </asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"
                                                                        Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"
                                                                       ></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"
                                                                        Text="Display" ToolTip="Click to Display report"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                        ToolTip="Click to Close" CausesValidation="False">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
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
            $("#<%=txtSearch.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtOrderTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=1', {
                width: 187,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });

            $("#<%=txtAircraft.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=OrderAircraftReg', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>

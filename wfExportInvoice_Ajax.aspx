<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoice_Ajax.aspx.vb"
    Inherits="Flypal.wfExportInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export Invoice Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="clsFormHeader1Newstyle">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Export Invoice Details [New]</asp:Label>
                                                                    </td>
                                                                    <%--<td colspan="2" align="right">
                                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH" Text="Cancel"
                                                                                            ToolTip="Click to cancel Export Invoice"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH" Text="Authorize"
                                                                                            ToolTip="Click to authorize Export Invoice"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save Export Invoice"
                                                                                            ValidationGroup="a"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Export Invoice"
                                                                                            ToolTip="Click to print Export Invoice" Enabled="<%# Not mExportInvoice.IsNew %>"
                                                                                            Width="122px"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnPackingList" runat="server" CssClass="clsbtnH clsinfoH" Text='<%# iif(AppSettings("ClientCode") = "YA" OR AppSettings("ClientCode") = "TA","Shipping Order","Packing List") %>'
                                                                                            Width="122px" ToolTip="Click to print Packing List" Enabled="<%# Not mExportInvoice.IsNew %>"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnProformaInvoice" runat="server" CssClass="clsbtnH clsinfoH" Text="Proforma Invoice"
                                                                                            Width="122px" ToolTip="Click to print Proforma Invoice" Enabled="<%# Not mExportInvoice.IsNew %>"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                                            CausesValidation="False"></asp:Button>
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
                                                        <td>
                                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                            <script type="text/javascript">
                                                                function ValidateVendor(source, args) {
                                                                    args.IsValid = false;
                                                                    var dd = $get("cmbVendorList");
                                                                    if (dd.selectedIndex != 0) {
                                                                        args.IsValid = true;
                                                                        return;
                                                                    }
                                                                }
                                                                function ValidateCurrency(source, args) {
                                                                    args.IsValid = false;
                                                                    var dd = $get("cmbCurrencyList");
                                                                    if (dd.selectedIndex != 0) {
                                                                        args.IsValid = true;
                                                                        return;
                                                                    }
                                                                }
                                                            </script>
                                                            <asp:CustomValidator ID="cvCommon" runat="server" Display="None" OnServerValidate="CustomValidate"
                                                                ValidationGroup="a"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cvInvoiceDate" runat="server" ControlToValidate="txtExportInvoiceDate"
                                                                Display="None" ErrorMessage="Select Export Invoice Date" OnServerValidate="CustomValidate"
                                                                ValidationGroup="a"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cvCurrency" runat="server" ControlToValidate="cmbCurrencyList"
                                                                ValidationGroup="a" Display="None" ErrorMessage="Select Currency from the list."
                                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cvFactor" runat="server" OnServerValidate="customvalidate"
                                                                ValidationGroup="a" Display="None" ErrorMessage="Currency factor must be greater than zero."
                                                                ControlToValidate="txtConversionFactor"></asp:CustomValidator><asp:RequiredFieldValidator
                                                                    ValidationGroup="a" ID="rfvFactor" runat="server" Display="None" ErrorMessage="Currency factor must be greater than zero."
                                                                    ControlToValidate="txtConversionFactor"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvVendor" runat="server" OnServerValidate="CustomValidate"
                                                                ValidationGroup="a" Display="None" ErrorMessage="Select Consignee from the list."
                                                                ControlToValidate="cmbVendorList"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mExportInvoice.StatusName %>"
                                                                CssClass="clsLabelHeader"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlExportInvoiceDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="lblExportInvoiceDetails" class="clsLabelHeader">Export Invoice Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblDate" class="clsLabel">Date</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtExportInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                Text="" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtExportInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExportInvoiceDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="txtExportInvoiceDateWatermarkExtender" runat="server"
                                                                TargetControlID="txtExportInvoiceDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="lblExportInvoiceDetails0" class="clsLabelHeader">Consignee Details Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblNoStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblName" class="clsLabelauto">Consignee Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="True" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mExportInvoice.ConsigneeID %>"
                                                                Enabled="False">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblSupAddress" class="clsLabelauto">Consignee Address</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupAddress" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mExportInvoice.ConsigneeAddress %>" ToolTip="Enter Consignee Address"
                                                                TextMode="MultiLine">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblSupplierAttn" class="clsLabelAuto">Attention</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupplierAttn" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.ConsigneeAttention %>"
                                                                ToolTip="Enter Consignee Attention">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblSupPreCarriage" class="clsLabelAuto">Pre-Carriage by</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupPreCarriage" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.PreCarriageBy %>"
                                                                ToolTip="Enter Pre-Carriage by">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblSupPlaceofReceipt" class="clsLabelAuto">Place of Receipt Pre-Carriage</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupPlaceofReceipt" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mExportInvoice.PalceOfReceiptPreCarriage %>" ToolTip="Enter Place of Receipt Pre-Carriage">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblFlightNo" class="clsLabelAuto">Airway Bill No/B/L No</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.FlightNo %>"
                                                                ToolTip="Enter Vessel / Flight No.">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPortOfLoading" class="clsLabelAuto">Port of Loading</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPortOfLoading" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.PortOfLoading %>"
                                                                ToolTip="Enter Port of Loading">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPortOfDischarge" class="clsLabelAuto">Port of Discharge</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPortOfDischarge" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.PortOfDischarge %>"
                                                                ToolTip="Enter Port of Discharge">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblFinalDestination" class="clsLabelAuto">Final Destination</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinalDestination" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.FinalDestination %>"
                                                                ToolTip="Enter Final Destination">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblIECCodeNo" class="clsLabelAuto">IEC Code No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIECCodeNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.IECCodeNo %>"
                                                                ToolTip="Enter IEC Code No." MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblInvoiceTo" class="clsLabelAuto">Invoice To</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInvoiceTo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.InvoiceTo %>"
                                                                ToolTip="Enter Invoice To">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="6">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblNameStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblNo" class="clsLabel">No.</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtExportInvoiceText" runat="server" Text="<%# mExportInvoice.ExportInvoiceText %>"
                                                                CssClass="clsTextBoxTagSearch" onfocus="SetContextKey();" ToolTip="Enter No." MaxLength="25"
                                                                Width="208px"> </asp:TextBox>
                                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtExportInvoiceText_Autocomplete"
                                                                runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfExportInvoice_Ajax.aspx"
                                                                ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtExportInvoiceText"
                                                                UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>
                                                            <script>
                                                                function SetContextKey() {
                                                                    var autoComplete = $find('txtExportInvoiceText_Autocomplete');
                                                                    var TransTypeID = 'TransTypeID=<%=mExportInvoice.ExportTransTypeID%>¿QuotationDate=<%=mExportInvoice.ExportInvoiceDate%>';
                                                                    autoComplete.set_contextKey(TransTypeID);
                                                                }
                                                            </script>
                                                            <asp:TextBox ID="txtExportInvoiceNo" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                MaxLength="8" Text="<%# mExportInvoice.ExportInvoiceNo %>"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span id="lblBuyerDetails" class="clsLabelHeader">Buyer Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblBuyer" class="clsLabelauto">Buyer</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:DropDownList ID="cmbBuyer" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mExportInvoice.BuyerID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblBuyerAddress" class="clsLabelauto">Buyer Address</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtBuyerAddress" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                Text="<%# mExportInvoice.BuyerAddress %>" ToolTip="Enter Buyer Address" TextMode="MultiLine">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblExporterRef" class="clsLabelAuto">Exporter's Ref.</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtExporterRef" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.ExporterReference %>"
                                                                ToolTip="Enter Exporter's Ref.">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblBuyerOrderNo" class="clsLabelAuto">Order No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBuyerOrderNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.BuyerOrderNo %>"
                                                                ToolTip="Enter Order No.">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblBuyerOrderDate" class="clsLabelAuto">Order Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBuyerOrderDate" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                                CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                                Text="" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtBuyerOrderDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtBuyerOrderDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="txtBuyerOrderDateTextBoxWatermarkExtender" runat="server"
                                                                WatermarkCssClass="" TargetControlID="txtBuyerOrderDate" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblblOtherReferences" class="clsLabelAuto">Other Reference(s)</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtBuyerOtherReferences" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mExportInvoice.OtherReference %>" ToolTip="Enter Other Reference(s)">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblBuyerAttn" class="clsLabelAuto">Attention</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtBuyerAttn" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mExportInvoice.BuyerAttention %>"
                                                                ToolTip="Enter Buyer Attention">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblCountryOfOrigin" class="clsLabelAuto">Country of Origin of Goods</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtBuyerCountryOfOrigin" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mExportInvoice.CountryOfOriginOfGoods %>" ToolTip="Enter Country of Origin of Goods">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblCountryOfFinal" class="clsLabelAuto">Country of Final Destination</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtBuyerCountryOfFinal" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Text="<%# mExportInvoice.CountryOfFinalDestination %>" ToolTip="Enter Country of Final Destination">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblCurrencyStar1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCurrency" class="clsLabelauto">Currency</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="True" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mExportInvoice.CurrencyID %>"
                                                                Enabled="<%# mExportInvoice.StatusID = 1 %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblFactor" class="clsLabelAuto">Factor</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right"
                                                                Text="<%# mExportInvoice.ConversionFactor %>" ToolTip="Enter Conversion Factor"
                                                                MaxLength="9" Enabled="<%# mExportInvoice.StatusID = 1 %>">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblRoundOffRequire" class="clsLabelAuto">Round Off Required</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                                Checked="<%# mExportInvoice.IsRoundOff %>"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlExportInvoiceItem" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblInvoiceItem" class="clsLabelHeader">Export Invoice Item(s)</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgExportInvoiceItem" runat="server" AutoGenerateColumns="False"
                                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ItemID" HeaderText="ItemID" Visible="False">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No." SortExpression="PartNo">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right"
                                                                                OnTextChanged="TextChanged" Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>"
                                                                                MaxLength="8" onKeyPress="javascript:validate('Rate');" Text='<%# DataBinder.Eval(Container.DataItem,"CRate")%>'> </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Box No.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtBoxNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right"
                                                                                OnTextChanged="TextChanged" Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>"
                                                                                MaxLength="8" onKeyPress="javascript:validate('Box');" Text='<%# DataBinder.Eval(Container.DataItem,"BoxNo") %>'>
                                                                            </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Note" HeaderText="Note">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--12--%>
                                                                    <asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
                                                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--13--%>
                                                                    <asp:BoundField DataField="IssueNumber" HeaderText="Issue No.">
                                                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--14--%>


                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <span id="button">Login</span>--%>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server"
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    CommandName="DeleteRecord" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" Visible="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>"/>
                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>


                                                                </Columns>
                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlExportInvoiceBox" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblExportInvoiceBox" class="clsLabelHeaderItem">Export Invoice Box(s)</span>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="btnExportInvoiceBox" runat="server" CssClass="clsbtnH clsinfoH" Text="Add"
                                                                        ToolTip="Click to add Export Invoice Box"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnExportInvoiceBox" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                            ToolTip="Click to Add Export Invoice Box"></asp:ImageButton>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgExportInvoiceBox" runat="server" AutoGenerateColumns="False"
                                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExportInvoiceBoxNo" HeaderText="Box No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Container No.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtContainerNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ContainerNo") %>'
                                                                                Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>" CssClass="clsTextBoxTagSearchDate"
                                                                                MaxLength="50">
                                                                            </asp:TextBox>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtContainerNo" ID="txtContainerNoWatermarkExtender"
                                                                                WatermarkCssClass="waterMarkcss" runat="server" WatermarkText="Type Container No."></cc2:TextBoxWatermarkExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dimension">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtDimension" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Dimension") %>'
                                                                                Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>" CssClass="clsTextBoxTagSearchDate"
                                                                                MaxLength="50">
                                                                            </asp:TextBox>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDimension" ID="txtDimensionWatermarkExtender"
                                                                                WatermarkCssClass="waterMarkcss" runat="server" WatermarkText="L [X] H [X] W (Feet/cm)"></cc2:TextBoxWatermarkExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Net Weight">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtNetWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NetWeight") %>'
                                                                                Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>" CssClass="clsTextBoxTagSearchDate"
                                                                                MaxLength="50">
                                                                            </asp:TextBox>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtNetWeight" ID="txtNetWeightWatermarkExtender"
                                                                                WatermarkCssClass="waterMarkcss" runat="server" WatermarkText="[Weight] (Kg/Lbs)"></cc2:TextBoxWatermarkExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Gross Weight">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtGrossWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"GrossWeight") %>'
                                                                                Enabled="<%#IIf(mExportInvoice.StatusID >= 2, False, True) %>" CssClass="clsTextBoxTagSearchDate"
                                                                                MaxLength="50">
                                                                            </asp:TextBox>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtGrossWeight" ID="txtGrossWeightWatermarkExtender"
                                                                                WatermarkCssClass="waterMarkcss" runat="server" WatermarkText="[Weight] (Kg/Lbs)"></cc2:TextBoxWatermarkExtender>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlExportInvoiceTerm" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblExportInvoiceTerm" class="clsLabelHeader">Export Invoice Term(s)</span>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="btnAddTerms" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ToolTip="Click To Add Term"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnAddTerms" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                            ToolTip="Click to Add New Term"></asp:ImageButton>

                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgExportInvoiceTerm" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                    <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="TextBreak" Width="500px" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--<asp:ButtonField CommandName="DeleteTerm" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>

                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <span id="button">Login</span>--%>
                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" Visible='<%#IIf(mExportInvoice.StatusID > 1, False, True) %>'
                                                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                CommandName="DeleteTerm" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />

                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlExportInvoiceCharge" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblChargeDeatails" class="clsLabelHeader">Export Invoice Charge(s)</span>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="btnAddCharge" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ToolTip="Click To Add Charge"></asp:Button>--%>
                                                                        <asp:ImageButton ID="btnAddCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                            ToolTip="Click to Add New Charge"></asp:ImageButton>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgExportInvoiceCharge" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ChargeName" HeaderText="Charge Name">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <%--<asp:ButtonField CommandName="EditCharge" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteCharge" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>

                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <span id="button">Login</span>--%>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server" Visible='<%#IIf(mExportInvoice.StatusID > 1, False, True) %>'
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    CommandName="EditCharge" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" Visible='<%#IIf(mExportInvoice.StatusID > 1, False, True) %>'
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    CommandName="DeleteCharge" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblTotal" class="clsLabelAuto">Total</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right" Text="<%# mExportInvoice.CTotalAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblTotalOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTotalOtherCharges" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right"
                                                                Text="<%# mExportInvoice.CTotalCharges %>" ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblGrandTotal" class="clsLabelAuto">Grand Total</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right"
                                                                Text="<%# mExportInvoice.CGrandTotal %>" ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblAmountInWords" class="clsLabelAuto">Amount In Words </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAmountInWords" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mExportInvoice.AmountINWords.trim %>"
                                                                MaxLength="250" Width="370px" TextMode="MultiLine" ReadOnly="True" BackColor="#E0E0E0" Height="40px">                                                        </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="370px" Text="<%# mExportInvoice.Remark %>"
                                                                ToolTip="Enter Remark" TextMode="MultiLine" Rows="5" Height="20px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
                                                                ToolTip="Click to cancel Export Invoice"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text="Authorize"
                                                                ToolTip="Click to authorize Export Invoice"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to save Export Invoice"
                                                                ValidationGroup="a"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Export Invoice"
                                                                ToolTip="Click to print Export Invoice" Enabled="<%# Not mExportInvoice.IsNew %>"
                                                                Width="122px"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPackingList" runat="server" CssClass="clsbtnH clsinfoH1" Text='<%# iif(AppSettings("ClientCode") = "YA" OR AppSettings("ClientCode") = "TA","Shipping Order","Packing List") %>'
                                                                Width="122px" ToolTip="Click to print Packing List" Enabled="<%# Not mExportInvoice.IsNew %>"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnProformaInvoice" runat="server" CssClass="clsbtnH clsinfoH1" Text="Proforma Invoice"
                                                                Width="122px" ToolTip="Click to print Proforma Invoice" Enabled="<%# Not mExportInvoice.IsNew %>"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                                CausesValidation="False"></asp:Button>
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
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddConsignee = document.getElementById("cmbVendorList");
            var ddBuyer = document.getElementById("cmbBuyer");
            if (ddConsignee != null || ddBuyer != null) {
                var i = 0;
                if (ddConsignee.disabled == false || ddBuyer.disabled == false) {
              <% For Each item1 In mVendorList%>
                <% If item1.NotInUse = "True" Then%>
                  ddConsignee[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                  ddBuyer[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                  i = i + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>

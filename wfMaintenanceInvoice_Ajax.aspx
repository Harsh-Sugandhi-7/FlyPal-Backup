<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceInvoice_Ajax.aspx.vb"
    Inherits="Flypal.wfMaintenanceInvoice_Ajax" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Invoice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Maintenance Invoice [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            ValidationGroup="1" ErrorMessage="Maintenance Invoice Date Required" ControlToValidate="txtMaintenanceInvoiceDate"
                                            Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvVendorName" runat="server" Display="None" ErrorMessage="Select Supplier from the list"
                                            ControlToValidate="cmbVendorList" CssClass="clsLabelAuto" ClientValidationFunction="validateVendor"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvChareFor" runat="server" Display="None" ErrorMessage="Either Select Charge from the List Or Enter Charge in Text"
                                            ControlToValidate="cmbChargesFor" CssClass="clsLabelAuto" ClientValidationFunction="validateCharge"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOtherCharge" runat="server" Display="None" ErrorMessage="Charge Can not be Negative"
                                            ControlToValidate="txtOtherCharges" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" Display="None" ErrorMessage="Remark should not be greater than 150 characters"
                                            ControlToValidate="txtRemark" CssClass="clsLabelAuto" ClientValidationFunction="validateRemarkLen"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPartNo" runat="server" Display="None" ErrorMessage="Select Part No. from the List"
                                            ControlToValidate="cmbPartNo" CssClass="clsLabelAuto" ClientValidationFunction="validatePartNo"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" Display="None" ErrorMessage="Quantity Required"
                                            ControlToValidate="txtQuantity" CssClass="clsLabelAuto" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" Display="None" ErrorMessage="Quantity must be greater than Zero."
                                            ControlToValidate="txtQuantity" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validatePartNo(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbPartNo");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                            function validateCharge(source, args) {
                                                args.IsValid = false;
                                                var Index = $get("cmbChargesFor").selectedIndex;
                                                var Text = $get("txtChargeFor").value;
                                                if (Index > 0 || Text != "") {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }

                                            function validateVendor(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbVendorList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }

                                            function validateRemarkLen(source, args) {
                                                args.IsValid = false;
                                                var RemarkLength = $get("txtRemark").value.length;
                                                if (RemarkLength <= 150) {
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
                            <td>
                                <asp:UpdatePanel ID="upnlInvoiceDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="lblMaintenanceInvoiceInfo" style="font-weight: bold"><b>Maintenance Invoice
                                                            Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 12px;">
                                                                    <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td style="width: 90px;">
                                                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtMaintenanceInvoiceDate" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true" 
                                                                        onchange="ValidateDateText(this,'MaintenanceInvoiceDate_watermarkextender','true');"
                                                                        Width="100px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="MaintenanceInvoiceDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtMaintenanceInvoiceDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="MaintenanceInvoiceDate_watermarkextender" runat="server"
                                                                        ClientIDMode="Static" TargetControlID="txtMaintenanceInvoiceDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblNoStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="clsLabel">No.</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <table id="Table5" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mMaintenanceInvoice.InvoiceText %>"
                                                                                    Enabled='<%# Not Session("Edit") And mMaintenanceInvoice.isNew %>' MaxLength="25"
                                                                                    ToolTip="Enter Maintenance Invoice text">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxSmall_Ajax" Text="<%# mMaintenanceInvoice.InvoiceNo %>"
                                                                                    ClientIDMode="Static" Enabled='<%# Not Session("Edit") And mMaintenanceInvoice.isNew %>'
                                                                                    MaxLength="8" ToolTip="Enter Maintenance Invoice No.">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRefNo" class="clsLabelAuto">Ref. No.</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtRefNo" runat="server" CssClass="clsTextBoxDate_Ajax" Text="<%# mMaintenanceInvoice.InvoiceNo %>"
                                                                        ClientIDMode="Static" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="lblSupplierInfo" style="font-weight: bold"><b>Supplier Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 12px;">
                                                                    <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td style="width: 90px;">
                                                                    <span id="lblName" class="clsLabelAuto">Name</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                        DataTextField="Name" DataValueField="Id" SelectedValue="<%# mMaintenanceInvoice.VendorID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblInvoiceNo" class="clsLabelAuto">Invoice No.</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtVendorInvoiceNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mMaintenanceInvoice.VendorInvoiceNo %>"
                                                                        MaxLength="20" ToolTip="Enter Invoice No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblInvoiceDate" class="clsLabelAuto">Invoice Date</span>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtVendorInvoiceDate" runat="server" CssClass="clsTextBox_Ajax"
                                                                        onchange="ValidateDateText(this,'VendorInvoiceDate_watermarkextender','false');"
                                                                        TabIndex="1" Width="90px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="VendorInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtVendorInvoiceDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="VendorInvoiceDate_watermarkextender" runat="server"
                                                                        ClientIDMode="Static" TargetControlID="txtVendorInvoiceDate" WatermarkCssClass="clsDateTextBox"
                                                                        WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="lblPartInfo" style="font-weight: bold"><b>Part Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <span id="lblInfo" class="clsLabelAuto">Enter the Details of Parts Ordered by selecting
                                                                        the Part No. from the list and mention the Qty and the Rate.</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 12px;">
                                                                    <span id="lblPartNoStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td style="width: 90px;">
                                                                    <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                        DataValueField="Id" SelectedValue="<%# mMaintenanceInvoice.ItemId %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mMaintenanceInvoice.SerialNo %>"
                                                                        MaxLength="50" ToolTip="Enter Serial No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblChargesForStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblChargesFor" class="clsLabelAuto">Charges For</span>
                                                                </td>
                                                                <td>
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbChargesFor" runat="server" CssClass="clsComboBox_Ajax" DataTextField="ChargesFor"
                                                                                    DataValueField="ChargesFor">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblOr" class="clsLabelAuto">Or</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtChargeFor" runat="server" CssClass="clsTextBox_Ajax" MaxLength="75"
                                                                                    ToolTip="Enter Charge"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblQuantityStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblQuantity" class="clsLabelAuto">Quantity</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                                                        ClientIDMode="Static" Text="<%# mMaintenanceInvoice.Quantity %>" MaxLength="8"
                                                                        ToolTip="Enter Quantity">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRate" class="clsLabel">Rate</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax" Text="<%# mMaintenanceInvoice.Rate %>"
                                                                        ClientIDMode="Static" MaxLength="12" ToolTip="Enter Rate">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblAmount" class="clsLabelAuto">Amount</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxRightAlign_Ajax" Text="<%# mMaintenanceInvoice.Amount %>"
                                                                        ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblOtherChargesAuto" class="clsLabelAuto">Oth. Charges</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                                        Text="<%# mMaintenanceInvoice.OtherCharges %>" MaxLength="12" ToolTip="Enter Other Charges">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblGrandTotal" class="clsLabelAuto">Grand Total</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                                        Text="<%# mMaintenanceInvoice.GrandTotal %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1_Ajax" Text="<%# mMaintenanceInvoice.Remark %>"
                                                                        MaxLength="100" ToolTip="Enter Remark" Rows="10" TextMode="MultiLine">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save Maintenance Invoice"
                                                        ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to go back to the previous page">
                                                    </asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </form>
    <script type="text/javascript">
        if ("<%=System.Configuration.ConfigurationSettings.AppSettings("AutoCompleteTransText").ToString()%>" == "True") {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=txtText.ClientID%>").autocomplete('wfAutoTransText.aspx?TransTypeID=<%=30%>&ToDate=<%=mMaintenanceInvoice.Date1%>', {
                    width: 185,
                    autoFill: false,
                    matchContains: true,
                    max: 500,
                    delay: 0
                });
            });
        }
    </script>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, IsDefaultTodaysDate) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': IsDefaultTodaysDate };
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
</body>
</html>

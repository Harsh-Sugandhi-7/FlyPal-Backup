<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceOrder_Ajax.aspx.vb"
    Inherits="Flypal.wfLineMaintenanceOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Order</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function viewAttachment() {
            str = "wfFileView.aspx";
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
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="Table-MaxWidth" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                            <table id="tblinner" class="clsTablelistin" width="100%">
                                <tr id="Header">
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
													Service Order [New]
                                                </asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="ValidationSummary">
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Fields" />
                                                <asp:CustomValidator ID="cvOpeningLine" runat="server" OnServerValidate="CustomValidate"
                                                    ValidationGroup="a" ErrorMessage="Opening Line length must not be  greater than 500 character. "
                                                    ControlToValidate="txtOpeningLine" Display="None" CssClass="clsLabelAuto" />
                                                <asp:CustomValidator ID="cvOrderDate" runat="server" ControlToValidate="calOrderDate"
                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Select Order Date"
                                                    OnServerValidate="CustomValidate1" ValidationGroup="a" />
                                                <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbVendorList"
                                                    ErrorMessage="Please Select Vendor." CssClass="clsValidationSummary" />
                                                <asp:RequiredFieldValidator ID="refOrderDate" runat="server" ControlToValidate="calOrderDate"
                                                    ValidationGroup="a" CssClass="clsLabelAuto" Display="None"
                                                    ErrorMessage="Select Order Date." />
                                                <asp:CustomValidator ID="cvQuoDate" runat="server" ControlToValidate="txtQuotationDate"
                                                    ValidationGroup="a" CssClass="clsLabelAuto" Display="None"
                                                    ErrorMessage="Select Quotation Date" OnServerValidate="CustomValidate" />
                                                <asp:CustomValidator ID="cvCurrency" runat="server" ControlToValidate="cmbCurrencyList"
                                                    ValidationGroup="a" CssClass="clsLabelAuto" Display="None"
                                                    ErrorMessage="Select Currency from the list." OnServerValidate="customvalidate" />
                                                <asp:RequiredFieldValidator ID="rfvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                                    ValidationGroup="a" CssClass="clsLabelAuto" Display="None"
                                                    ErrorMessage="Currency factor must be greater than zero." />
                                                <asp:CustomValidator ID="cvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                                    ValidationGroup="a" CssClass="clsLabelAuto" Display="None"
                                                    ErrorMessage="Currency factor must be greater than zero."
                                                    OnServerValidate="customvalidate" />
                                                <asp:CustomValidator ID="cvMachine" runat="server" ClientValidationFunction="ValidateAircraft"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbMachine"
                                                    ErrorMessage="Please Select Aircraft." CssClass="clsValidationSummary" />

                                                <script type="text/javascript">

                                                    function ValidateVendor(source, args) {

                                                        args.IsValid = false;

                                                        var dd = $get("cmbVendorList");

                                                        if (dd.selectedIndex != 0) {

                                                            args.IsValid = true;

                                                            return;

                                                        }

                                                    }

                                                    //function ValidateAircraft(source, args) {

                                                    //    args.IsValid = false;

                                                    //    var dd = $get("cmbMachine");

                                                    //    if (dd.selectedIndex != 0) {

                                                    //        args.IsValid = true;

                                                    //        return;

                                                    //    }

                                                    //}

                                                    function ValidateAircraft(source, args) {

                                                        // Example condition to skip validation

														if ('<%# AppSettings("ClientCode") %>' == "PTW" || '<%# AppSettings("ClientCode") %>' == "7AR") {

                                                            // Skip validation

                                                            args.IsValid = true;

                                                            return;

                                                        }

                                                        // Normal validation

                                                        var dd = $get("cmbMachine");

                                                        args.IsValid = dd.selectedIndex != 0;

                                                    }

												</script>


                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="OrderStatusLabel">
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader"
                                                    Text="<%# mLineMaintenanceOrder.StatusName %>">
                                                </asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="OderAndSupplierDetail">
                                    <td valign="top" id="OrderDetailsAndFileAttachment">
                                        <table>
                                            <tr>
                                                <td id="OrderDetails">
                                                    <fieldset class="clsFieldSetNewStyle">
                                                        <legend>
                                                            <asp:Label runat="server" ID="lblOrderDetailHeader"
                                                                CssClass="clsLabelHeader" Text="Order Details" />
                                                        </legend>
                                                        <asp:UpdatePanel runat="server" ID="upnlOrderDetails" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="tblOrderDetails">
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Label1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Label15" class="clsLabelAuto">Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="calOrderDate" runat="server" ClientIDMode="Static"
                                                                                CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                                                onchange="ValidateDateText(this,'Date_watermarkextender','true');" Width="100px" />
                                                                            <cc2:CalendarExtender ID="calOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calOrderDate" />
                                                                            <cc2:TextBoxWatermarkExtender ID="calOrderDateWatermarkExtender" runat="server"
                                                                                TargetControlID="calOrderDate" WatermarkCssClass="clsDateTextBox"
                                                                                WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Label23" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Label19" class="clsLabelAuto">No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtText" runat="server"
                                                                                CssClass="clsTextBoxTagSearch"
                                                                                Text="<%# mLineMaintenanceOrder.Text %>"
                                                                                MaxLength="25" ToolTip="Enter text.">
                                                                            </asp:TextBox>
                                                                            <asp:TextBox ID="txtNo" runat="server" Width="48px"
                                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                Text="<%# mLineMaintenanceOrder.No %>" MaxLength="8">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblStar" runat="server"
																				Text="*" CssClass="clsLabelStar"
																				Visible='<%#IIf(AppSettings("ClientCode") = "PTW" OR AppSettings("ClientCode") = "7AR", False, True) %>' />
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblMachine" class="clsLabelAuto">Aircraft</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbMachine" runat="server"
                                                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                Enabled="<%# mLineMaintenanceOrder.IsNew %>"
                                                                                DataTextField="RegNo" DataValueField="ID"
                                                                                SelectedValue="<%# mLineMaintenanceOrder.MachineID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <span id="lblLocation" class="clsLabelAuto">Location</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbLocation" runat="server"
                                                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                Enabled="<%# mLineMaintenanceOrder.IsNew %>"
                                                                                DataTextField="Name" DataValueField="ID"
                                                                                SelectedValue="<%# mLineMaintenanceOrder.LocationID %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <span id="lblKindAttention" class="clsLabelAuto">Kind Attention</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAttention" runat="server"
                                                                                CssClass="clsTextBoxTagSearch"
                                                                                Text="<%# mLineMaintenanceOrder.Attention %>" Width="500px"
                                                                                MaxLength="50" ToolTip="Enter Kind Attention for an order.">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="FileAttachment">
                                                    <fieldset class="clsFieldSetNewStyle">
                                                        <legend>
                                                            <asp:Label runat="server" ID="lblFileAttachmentHeader"
                                                                class="clsLabelHeader" Text="Attachment" />
                                                        </legend>
                                                        <asp:UpdatePanel runat="server" ID="upnlFileAttachment" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="tblFileAttachment">
                                                                    <tr>
                                                                        <td class="clsInnerTable">
                                                                            <asp:Label runat="server" ID="lblAttachFile"
                                                                                class="clsLabelAuto" Text="Attach" />
                                                                        </td>
                                                                        <td>
                                                                            <table id="tblFileAttachmentButtons" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlFileAttachmentButtons" runat="server"
                                                                                            UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <input type="button" id="btnSelectFile"
                                                                                                                value="Select File"
                                                                                                                title="Select a File to attach."
                                                                                                                runat="server"
                                                                                                                class="clsbtnH clsinfoH1" />
                                                                                                        </td>
                                                                                                        <td class="tdControls">
                                                                                                            <asp:Button ID="btnRemoveAttach"
                                                                                                                runat="server" CssClass="clsbtnH clsinfoH1"
                                                                                                                ToolTip="Remove the Attachment added."
                                                                                                                Text="Remove Attachment"
                                                                                                                Enabled="False" Width="140px" />
                                                                                                        </td>
                                                                                                        <td class="FileAttachmentICNPAdding">
                                                                                                            <asp:ImageButton ID="AttachmentIcon"
                                                                                                                runat="server"
                                                                                                                CausesValidation="False"
                                                                                                                ImageUrl="icons/CLIP01.ICO"
                                                                                                                CssClass="FileAttachmentICN"
                                                                                                                ToolTip="Open / Download the added Attachment." />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left" valign="top">
                                        <fieldset class="clsFieldSetNewStyle">
                                            <legend>
                                                <span id="lblSupplierDetailHeader" class="clsLabelHeader">Supplier Details</span>
                                            </legend>
                                            <asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="tblSupplierDetails">
                                                        <tr>
                                                            <td>
                                                                <span id="Label24" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Label7" class="clsLabelAuto">Name</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:DropDownList ID="cmbVendorList" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    Enabled="<%# mLineMaintenanceOrder.IsNew %>"
                                                                    DataTextField="Name" DataValueField="ID"
                                                                    SelectedValue="<%# mLineMaintenanceOrder.VendorID %>"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="lblQuotationNoAndDate" class="clsLabelAuto">Quotation No & Date</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtQuotationNo" runat="server"
                                                                    CssClass="clsTextBoxTagSearch"
                                                                    Text="<%# mLineMaintenanceOrder.QuotationNo %>"
                                                                    MaxLength="50" ToolTip="Enter Quotation No."
                                                                    Enabled="<%# mLineMaintenanceOrder.StatusID = 1 %>">
                                                                </asp:TextBox>
                                                                <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                    Text="<%# mLineMaintenanceOrder.QuotationDateFormatted %>"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtQuotationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtQuotationDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtQuotationDate" ID="txtQuotationDateWatermarkExtender"
                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblCurrencyStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblCurrencyAndFactor" class="clsLabelAuto">Currency & Factor</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    Enabled="<%# mLineMaintenanceOrder.IsNew %>" DataTextField="Name" DataValueField="ID"
                                                                    SelectedValue="<%# mLineMaintenanceOrder.CurrencyID %>" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtConversionFactor" runat="server"
                                                                    CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="48px"
                                                                    Text="<%# mLineMaintenanceOrder.ConversionFactor %>"
                                                                    MaxLength="9" ToolTip="Enter Conversion Factor">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="lblBillingAddress" class="clsLabelAuto">Billing Address</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtBillingAddress" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                    Text="<%# mLineMaintenanceOrder.BillingAddress %>" Width="500px" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="Label5" class="clsLabelAuto">Opening Line</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtOpeningLine" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                    Text="<%# mLineMaintenanceOrder.OpeningLine %>" Width="500px" MaxLength="500"
                                                                    ToolTip="Enter Opening Line for an order" Rows="5" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" style="padding-inline-start: 10px;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblRoundOffRequire" class="clsLabelAuto">Round Off Required
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkIsRoundOff" runat="server"
                                                                                CssClass="clsLabelAuto" AutoPostBack="True"
                                                                                Checked="<%# mLineMaintenanceOrder.IsRoundOff %>" />
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblMaintenanceSupportPlan" class="clsLabelAuto">Maintenance Support Plan
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkMaintenanceSupportPlan" runat="server"
                                                                                CssClass="clsLabelAuto" TextAlign="Right"
                                                                                Enabled="<%# mLineMaintenanceOrder.StatusID = 1 %>"
                                                                                AutoPostBack="true" Checked="<%# mLineMaintenanceOrder.IsMSP %>" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblContractNO" runat="server"
                                                                                Text="<%# mLineMaintenanceOrder.ContractNO  %>"
                                                                                CssClass="clsLabelHeader" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr id="OrderItem">
                                    <td colspan="2">
                                        <fieldset class="clsFieldSetNewStyle">
                                            <legend>
                                                <asp:Label runat="server" ID="lblOrderItemHeader"
                                                    CssClass="clsLabelHeader" Text="Order Item(s)" />
                                                <asp:ImageButton ID="btnAddOrderItems"
                                                    runat="server" ValidationGroup="a"
                                                    ToolTip="Add Order Item(s)"
                                                    CssClass="AddNewICN"
                                                    ImageUrl="~/images/plus1.png" />
                                            </legend>
                                            <asp:UpdatePanel runat="server" ID="upnlLineMaintenanceOrderItems" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgOrderItems" runat="server" ShowHeaderWhenEmpty="True"
                                                                    AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal"
                                                                    CellPadding="5">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader"
                                                                        Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                                    <Columns>
                                                                        <%-- 0 --%>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <%-- 1 --%>
                                                                        <asp:BoundField DataField="JobDetails" HeaderText="Job Details">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <%-- 2 --%>
                                                                        <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <%-- 3 --%>
                                                                        <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <%-- 4 --%>
                                                                        <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <%-- 5 --%>
                                                                        <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <%-- 6 --%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <div id="dropDownImg" class="dropdown">
                                                                                    <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                    <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                        <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                        ToolTip="Edit record." CausesValidation="false"
                                                                                                        CommandName="EditView" ImageUrl="~/images/edit.png"
                                                                                                        Visible="<%# IIF(mLineMaintenanceOrder.StatusID > 1, False, True) %>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS"
                                                                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                        ToolTip="Delete record." CausesValidation="false" runat="server"
                                                                                                        CommandName="DeleteRecord" ImageUrl="~/images/delete.png"
                                                                                                        Visible="<%# IIF(mLineMaintenanceOrder.StatusID > 1, False, True) %>" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr id="Total">
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlTotal" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="Label8" class="clsLabelAuto">Total Amount</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotalAmt" runat="server" Width="150px"
                                                                Text="<%# mLineMaintenanceOrder.CTotalAmount %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="OrderTermsandCharges">
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle">
                                            <legend>
                                                <asp:Label runat="server" ID="LblOrderTermHeader"
                                                    CssClass="clsLabelHeader" Text="Order Term(s)" />
                                                <asp:ImageButton ID="btnAddTerms"
                                                    runat="server" ToolTip="Add Term(s)."
                                                    ImageUrl="~/images/plus1.png"
                                                    CssClass="AddNewICN" />
                                            </legend>
                                            <asp:UpdatePanel runat="server" ID="upnlOrderTerms" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgOrderTerms" runat="server" AutoGenerateColumns="False"
                                                                    ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" GridLines="Horizontal"
                                                                    CellPadding="5">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader"
                                                                        Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                                    <Columns>
                                                                        <%--0--%>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                        <%--1--%>
                                                                        <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                            <ItemStyle CssClass="TextBreak" Width="500px" />
                                                                        </asp:BoundField>
                                                                        <%--2--%>
                                                                        <asp:ButtonField CommandName="DeleteTerm" HeaderText="Remove" Text="Remove">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </td>
                                    <td valign="top">
                                        <fieldset class="clsFieldSetNewStyle">
                                            <legend>
                                                <asp:Label runat="server" ID="lblOrderChargeHeader"
                                                    CssClass="clsLabelHeader" Text="Order Charge(s)" />
                                                <asp:ImageButton ID="btnAddCharges"
                                                    runat="server" ToolTip="Add Charges(s)."
                                                    ImageUrl="~/images/plus1.png"
                                                    CssClass="AddNewICN" />
                                            </legend>
                                            <asp:UpdatePanel runat="server" ID="upnlLineMaintenanceOrderCharges" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgOrderCharges" runat="server" AutoGenerateColumns="False"
                                                                    ShowHeaderWhenEmpty="True" CssClass="clsGridNewStyle" GridLines="Horizontal"
                                                                    CellPadding="5">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader"
                                                                        Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                                    <Columns>
                                                                        <%--0--%>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                        <%--1--%>
                                                                        <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                                                        <%--2--%>
                                                                        <asp:BoundField DataField="Percentage" HeaderText="Percentage">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <%--3--%>
                                                                        <asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <%--4--%>
                                                                        <asp:ButtonField CommandName="EditCharge" HeaderText="Edit" Text="Edit">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                        <%--5--%>
                                                                        <asp:ButtonField CommandName="DeleteCharge" HeaderText="Remove" Text="Remove">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr id="GrandTotal">
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlGrandTotal" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="Label6" class="clsLabelAuto">Grand Total</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtGrandTotal" runat="server" Width="150px"
                                                                Text="<%# mLineMaintenanceOrder.CGrandTotal %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                ToolTip="Grand Total" BackColor="#E0E0E0" ReadOnly="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="Buttons">
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table2">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCancel" runat="server"
                                                                Text="Cancel" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Cancel the Service Order." />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnAuthorized" runat="server"
                                                                Text="Authorize" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Authorize Service Order." />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server"
                                                                Text="Print" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Print Service Order Report."
                                                                Enabled="<%# Not mLineMaintenanceOrder.IsNew %>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server"
                                                                Text="Save" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Save Service Order record."
                                                                ValidationGroup="a" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server"
                                                                Text="Close" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Go back to the previous page."
                                                                CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="HiddenButtons">
                                    <td>
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlHiddenButtons">
                                            <ContentTemplate>
                                                <asp:Button ID="MSPAssemblySelection" ClientIDMode="Static" runat="server"
                                                    Text="Add" CausesValidation="False" Style="display: none;" />
                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;" />
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

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <div id="ModalandScripts">

            <!-- File Upload Modal Dialog-->
            <div id="FileUploadModal">

                <div style="display: none">
                    <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
                </div>
                <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                    <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
                        src="JavaScript:''" scrolling="auto"></iframe>
                </asp:Panel>
                <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
                    PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
                </cc2:ModalPopupExtender>

                <script type="text/javascript">

                    function IFrameFileUploadStateComplete() {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    $(document).ready(function () {

                        $("#btnSelectFile").live("click", function () {

                            try {
                                $get("AjaxLoader").style.visibility = 'visible';
                                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                                if (!$.browser.msie) {
                                    $("#btnDummyFileUpload").click();
                                    $get("AjaxLoader").style.visibility = 'hidden';
                                }

                                return false;
                            } catch (e) {
                                alert(e);
                            }

                        });

                    });

                </script>

                <script type="text/javascript">

                    function ParentCallBackFunctionForFileUpload(fileattached) {
                        var FileUpwindow = $find("<%= mdlPopupFileUpload.ClientID %>");
                        //close File Upload popup window
                        FileUpwindow.hide();
                        //Free resources
                        $("#IFileUpload").attr("src", "JavaScript:''");
                        if (fileattached) {
                            //call hidden button to set file upload content to object
                            $("#hdnBtnFileUpload").click();
                        }
                    }

                </script>

            </div>
            <!-- End -->

            <div id="Scripts">

                <script type="text/javascript">
                    function CallParentCallback() {
                        parent.ParentCallBackFunctionForReceipt1();
                        return false;
                    }
                </script>

                <script type="text/javascript">

                    $(document).ready(function () {

					<% Dim mOpenFrom As String = Request.QueryString("Type") %>

					<% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then %>  
                        $('#btnCancel').attr('disabled', 'disabled');
                        $('#btnDocketCharge').attr('disabled', 'disabled');
                        $('#btnPrintTag').attr('disabled', 'disabled');
                        $('#btnPrint').attr('disabled', 'disabled');
                        $('#btnSaveAttachment').attr('disabled', 'disabled');
					<% End if %>  
                    });

                </script>

            </div>

            <!-- Popup For MSPAssemblySelection -->
            <div id="divMSPAssemblySelection">

                <div style="display: none">
                    <asp:Button runat="server" ID="btnDummyMSPAssemblySelection" Text="MSPAssemblySelection"
                        ClientIDMode="Static" />
                </div>
                <asp:Panel runat="server" ID="pnlMSPAssemblySelection" ClientIDMode="Static" HorizontalAlign="Center"
                    Style="height: 100%; width: 100%;">
                    <iframe id="IframeMSPAssemblySelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                        scrolling="auto" allowtransparency="true"></iframe>
                </asp:Panel>
                <cc2:ModalPopupExtender ID="mdlPopupMSPAssemblySelection" runat="server" TargetControlID="btnDummyMSPAssemblySelection"
                    PopupControlID="pnlMSPAssemblySelection" BackgroundCssClass="clsModalPopupBG">
                </cc2:ModalPopupExtender>
                <script type="text/javascript">
                    function OpenMSPAssemblySelectionWindow() {
                        try {
                            $("#IframeMSPAssemblySelection").attr("src", "wfMSPAssemblySelection_Ajax.aspx?Type=FromLineMaintenanceOrder");
                            $("#btnDummyMSPAssemblySelection").click();

                            return false;
                        } catch (e) {
                            alert(e);
                        }

                    }
                    function ParentCallBackFunctionForMSPAssemblySelection() {
                        var MSPAssemblySelectionwindow = $find("<%=mdlPopupMSPAssemblySelection.ClientID %>");
                        //close popup window
                        MSPAssemblySelectionwindow.hide();
                        //           release resources
                        $("#IframeMSPAssemblySelection").attr("src", "JavaScript:''");
                        //call image button
                        $("#MSPAssemblySelection").click();
                    }
                </script>

            </div>
            <!---End-->

        </div>

        <div id="DateValidationScripts">

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

        </div>

    </form>
</body>
</html>

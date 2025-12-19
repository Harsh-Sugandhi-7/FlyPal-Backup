<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFuelInvoice_Ajax.aspx.vb"
    Inherits="Flypal.wfFuelInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fuel Invoice Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Fuel Invoice [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvDate" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" Display="None" ControlToValidate="calFuelInvoiceDate" ErrorMessage="Select Date"></asp:CustomValidator><asp:RequiredFieldValidator
                                                    ID="rfvDate" runat="server" Display="None" ControlToValidate="calFuelInvoiceDate"
                                                    ValidationGroup="a" ErrorMessage="Select Date."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbVendorList" ErrorMessage="Select Vendor from the list."></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCurrency" runat="server" ClientValidationFunction="ValidateCurrency"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbCurrencyList" ErrorMessage="Select Currency from the list."></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
                                                ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbCurrencyList"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvUnit" runat="server" ClientValidationFunction="ValidateUnit"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbUnit" ErrorMessage="Select Unit."></asp:CustomValidator>
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
                                                function ValidateUnit(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbUnit");
                                                    if (dd.selectedIndex != 0) {
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mFuelInvoice.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlFuelInvoiceDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="lblFuelInvoiceDetails" class="clsLabelHeader">Fuel Invoice Details</span>
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
                                                        <asp:TextBox ID="calFuelInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFuelInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calFuelInvoiceDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="calFuelInvoiceDateWatermarkExtender" runat="server"
                                                            TargetControlID="calFuelInvoiceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblNoStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabel">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtText" runat="server" Text="<%# mFuelInvoice.Text %>" CssClass="clsTextBox_Ajax"
                                                            ToolTip="Enter no." MaxLength="25" Width="208px"> </asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="wfFuelInvoice_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                            TargetControlID="txtText" UseContextKey="False">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" Text="<%# mFuelInvoice.No %>" CssClass="clsTextBoxSmall_Ajax"
                                                            MaxLength="8"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblUnitStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblUnit" class="clsLabelAuto">Unit</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="cmbUnit" runat="server" CssClass="clsComboBoxsmall_Ajax" DataTextField="Name"
                                                            DataValueField="ID" SelectedValue="<%# mFuelInvoice.InvoiceUnitID %>">
                                                        </asp:DropDownList>
                                                    </td>
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
                                                    <td colspan="4">
                                                        <asp:Label ID="lblVendorDetails" runat="server" CssClass="clsLabelHeader">Supplier Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabel">Name</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                            Width="506px" Enabled="<%# mFuelInvoice.IsNew %>" DataTextField="Name" DataValueField="ID"
                                                            SelectedValue="<%# mFuelInvoice.VendorID %>" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblVendorInvoiceNo" class="clsLabelAuto">Supplier Invoice No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVendorInvoiceNo" runat="server" Text="<%# mFuelInvoice.VendorInvoiceNo %>"
                                                            CssClass="clsTextBox_Ajax" MaxLength="49" ToolTip="Enter no."> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendorInvoiceDate" runat="server" CssClass="clsLabelAuto">Supplier Invoice Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVendorInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" Text="<%# mFuelInvoice.VendorInvoiceDateFormatted %>"
                                                            Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtVendorInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtVendorInvoiceDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtVendorInvoiceDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtVendorInvoiceDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblCurrencyStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblCurrency" class="clsLabel">Currency</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCurrencyList" runat="server" AutoPostBack="True" CssClass="clsComboBoxLong_Ajax"
                                                            DataTextField="Name" DataValueField="ID" Enabled="<%# mFuelInvoice.IsNew %>"
                                                            SelectedValue="<%# mFuelInvoice.CurrencyID %>" Width="191px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblConvFactor" class="clsLabelauto">Factor</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                            Text="<%# mFuelInvoice.ConversionFactor %>" MaxLength="9" ToolTip="Enter conversion factor"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsButton_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" Enabled="False"
                                                                                Text="Remove Attachment" ToolTip="Click to remove attachment" Width="140px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                                ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td width="6px">
                                                &nbsp
                                            </td>
                                            <td width="80px">
                                                <span id="lblRemark" class="clsLabel" runat="server">Remark</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultilineOpening_Ajax"
                                                    Height="27px" MaxLength="250" Rows="5" Text="<%# mFuelInvoice.Remark %>" TextMode="MultiLine"
                                                    ToolTip="Enter remark"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlFuelInvoiceLogs" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblFuelInvoiceLogs" class="clsLabelHeader">Fuel Invoice Log(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" TabIndex="0" Text="Add"
                                                                        ToolTip="Click to add" ValidationGroup="a" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgFuelInvoiceLogs" runat="server" AutoGenerateColumns="False" CssClass="clsGridLog"
                                                            ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                            CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO" Text="" Visible='<%#  Eval("IsAttachmentAdded")%>'
                                                                            Width="20px" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LogDate" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No." />
                                                                <asp:BoundField DataField="RegNo" HeaderText="RegNo" >
                                                                <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FromTo" HeaderText="Sector">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="LogUpliftedFuel" HeaderText="Uplifted Fuel">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                    <FooterStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                                                <asp:BoundField DataField="UpliftedFuelInInvUnitToDispaly" HeaderText="Uplifted Fuel Inv Unit.">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Actual Fuel">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtUpliftedFuelInvUnit" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                            Enabled="<%# mFuelInvoice.StatusID = 1 %>" MaxLength="8" OnTextChanged="TextChanged"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"UpliftedFuelInvUnit") %>'></asp:TextBox>
                                                                        <asp:CustomValidator ID="cvQtyRule" runat="server" ControlToValidate="txtUpliftedFuelInvUnit"
                                                                            Display="None" ErrorMessage="Must be numbers only" OnServerValidate="CustomValidate1"
                                                                            ValidationGroup="a"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate">
                                                                    <ItemTemplate>
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                        Enabled="<%# mFuelInvoice.StatusID = 1 %>" MaxLength="12" OnTextChanged="TextChanged"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"CRate") %>'> </asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="btnCopyInTextBox" runat="server" ImageUrl="~/images/CopyRate.png" Visible="false"
                                                                                        OnClick="btnCopyInTextBox_Click" ToolTip="Click to copy rate into another text boxes" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <%--<asp:Button ID="btnCopyInTextBox" runat="server" Text="Copy Rate" CssClass="clsButton_Ajax" OnClick="btnCopyInTextBox_Click" />--%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CEffRate" HeaderText="Eff. Rate">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Remark">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine3_Ajax" Enabled="<%# mFuelInvoice.StatusID = 1 %>"
                                                                            MaxLength="250" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' TextMode="MultiLine"
                                                                            ToolTip="Enter remark">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" CssClass="clsLabelAuto"
                                                                            Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded">
                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="Attach" HeaderText="Attach" Text="Attach">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle  HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="Remove" HeaderText="Remove Attachment" Text="Remove Attachment">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle  HorizontalAlign="Left" />
                                                                </asp:ButtonField>
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
                                <td valign="top" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlFuelInvoiceCharges" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblChargeDeatails" class="clsLabelHeader">Fuel Invoice Charge(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddCharge" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ToolTip="Click to add charge"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgChargeList" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            CssClass="clsGrid" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
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
                                                                <asp:ButtonField CommandName="EditCharge" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteCharge" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
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
                                                        <span id="lblGrandTotal" class="clsLabelAuto">Total</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCTotal" runat="server" Text="<%# mFuelInvoice.CTotalAmount %>"
                                                            CssClass="clsTextBoxRightAlign_Ajax" ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblTotaolOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCTotalOtherCharge" runat="server" Text="<%# mFuelInvoice.CTotalCharges %>"
                                                            CssClass="clsTextBoxRightAlign_Ajax" ToolTip="Total other charges" BackColor="#E0E0E0"
                                                            ReadOnly="True">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblRemaining" class="clsLabelAuto">Grand Total</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCGrandTotal" runat="server" Text="<%# mFuelInvoice.CGrandTotal %>"
                                                            CssClass="clsTextBoxRightAlign_Ajax" ToolTip="Grand total" BackColor="#E0E0E0"
                                                            ReadOnly="True">
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
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to cancel"></asp:Button>
                                                        <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to authorize fuel invoice"></asp:Button>
                                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="clsButton_Ajax" ToolTip="Click to print fuel invoice"
                                                            Enabled="<%# Not mFuelInvoice.IsNew %>"></asp:Button>
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsButton_Ajax" ToolTip="Click to save fuel invoice"
                                                            ValidationGroup="a"></asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="height: 0px;">
                                <td colspan="2" style="height: 0px;">
                                    <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
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
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
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

        function OpenFileUploadWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                //                if (!$.browser.msie) {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = "hidden";
                //                }
                return false;
            } catch (e) {
                alert(e);
            }

        }

       
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
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
    <!-- End -->
    <!-- Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCommonPartList" Text="Dummy Common Part List"
            ClientIDMode="Static" CausesValidation="False" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCommonPartList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="iPopupCommonPartList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCommonPartList" runat="server" TargetControlID="btnDummyCommonPartList"
        PopupControlID="pnlPopupCommonPartList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCommonPartListStateComplete() {
            $("#btnDummyCommonPartList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenToolsWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupCommonPartList").attr("src", "wfFuelLogListPendingForInvoice_Ajax.aspx?Type=pup&LookinTypeID=0 &Name=");

                if (!$.browser.msie) {
                    $("#btnDummyCommonPartList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCommonPartList() {
            var CommonPartListWindow = $find("<%=mdlPopupCommonPartList.ClientID %>");
            //close Common Part List popup window
            CommonPartListWindow.hide();
            $("#iPopupCommonPartList").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnCommonPartList").click();
        }
    </script>
    <!-- End-->
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
</body>
</html>

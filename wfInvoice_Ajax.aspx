<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInvoice_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Invoice Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Invoice Details [New]</asp:Label>
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
                                            <asp:CustomValidator ID="cvInvoiceDate" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Invoice Date Required." ControlToValidate="txtInvoiceDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="None" ErrorMessage="Invoice Date Required."
                                                ValidationGroup="a" ControlToValidate="txtInvoiceDate" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbVendorList" ErrorMessage="Please Select Vendor."
                                                CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCurrency" runat="server" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                Display="None" ErrorMessage="Please Select Currency." ControlToValidate="cmbCurrency"
                                                CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvRemark" runat="server" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                Display="None" ErrorMessage="Remark Too Long" ControlToValidate="txtRemark" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtFactor"
                                                ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."
                                                CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
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
                                                    var dd = $get("cmbCurrency");
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
                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mInvoice.StatusName %>" CssClass="clsLabelHeader">
                                            </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlInvoiceDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="lblInvoiceDetails" class="clsLabelHeader">Invoice Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarDate" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDate" class="clsLabel">Date</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                            Text=""></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInvoiceDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtInvoiceDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtInvoiceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabel">Invoice No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInvoiceText" runat="server" Text="<%# mInvoice.Text %>" ToolTip="Enter Purchase Invoice text"
                                                            CssClass="clsTextBoxTagSearch" onfocus="SetContextKey();" MaxLength="25" Width="208px"> </asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtInvoiceText_Autocomplete"
                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfInvoice_Ajax.aspx"
                                                            ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtInvoiceText"
                                                            UseContextKey="False">
                                                        </cc2:AutoCompleteExtender>
                                                        <script>
                                                            function SetContextKey() {
                                                                var autoComplete = $find('txtInvoiceText_Autocomplete');
                                                                var TransTypeID = 'TransTypeID=<%=mInvoice.TransTypeID%>¿QuotationDate=<%=mInvoice.InvoiceDate%>';
                                                                autoComplete.set_contextKey(TransTypeID);
                                                            }
                                                        </script>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" Text="<%# mInvoice.No %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            MaxLength="8" ToolTip="Enter Purchase Invoice No"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsbtnH clsinfoH1" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
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
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="lblVendorDetails" class="clsLabelHeader">Supplier Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarDetails0" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabel">Name</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                            Width="455px" SelectedValue="<%# mInvoice.VendorID %>" DataTextField="Name" DataValueField="ID"
                                                            Enabled="<%# mInvoice.StatusID = 1 %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDCNo" class="clsLabelAuto">D.C.No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDCNo" runat="server" Text="<%# mInvoice.DCNO %>" CssClass="clsTextBoxTagSearch"
                                                            Enabled="<%# mInvoice.StatusID = 1 %>" MaxLength="25" ToolTip="Enter D.C.No.">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDCDate" class="clsLabel">D.C.Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDCDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                            Text="<%# mInvoice.DCDateFormatted %>"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDCDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDCDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDCDate" ID="txtDCDateTextBoxWatermarkExtender"
                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendorInvNo" runat="server" CssClass="clsLabelAuto">Invoice No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVendorInvNo" runat="server" Text="<%# mInvoice.VendorInvoiceNo %>"
                                                            CssClass="clsTextBoxTagSearch" Enabled="<%# mInvoice.StatusID = 1 %>" MaxLength="25"
                                                            ToolTip="Enter Supplier Invoice no.">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblInvDate" class="clsLabelAuto">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVendorInvDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                            ClientIDMode="Static" Text="<%# mInvoice.VendorInvoiceDateFormatted %>"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtVendorInvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtVendorInvDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtVendorInvDate" ID="txtVendorInvDateWatermarkExtender"
                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                                                        <asp:DropDownList ID="cmbCurrency" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            Width="200px" Enabled="<%# mInvoice.StatusID = 1 %>" DataTextField="Name" DataValueField="ID"
                                                            SelectedValue="<%# mInvoice.CurrencyID %>" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblConvFactor" class="clsLabelauto">Factor</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFactor" runat="server" Text="<%# mInvoice.ConversionFactor %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Enabled="<%# mInvoice.StatusID = 1 %>"
                                                            MaxLength="9" ToolTip="Enter Conversion Factor">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAWBNo" class="clsLabelAuto">Custom Bill of Entry</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# mInvoice.StatusID = 1 %>"
                                                            MaxLength="50" Text="<%# mInvoice.AWBNo %>">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRoundOffRequire" class="clsLabel">Round Off Required</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsRoundOff" runat="server" AutoPostBack="True" Checked="<%# mInvoice.IsRoundOff %>"
                                                            CssClass="clsLabelAuto" TextAlign="Right" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlInvoiceItems" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Invoice Item(s)" class="clsLabelHeader">Invoice Item(s):</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add" ToolTip="Click to add Purchase Invoice Item"
                                                                        ValidationGroup="a"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgInvoice" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="OrderInfo" HeaderText="Order Info." HtmlEncode="false">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info." HtmlEncode="false">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info." HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="DisplayQtyForForFourDigit" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="DisplayCRateForFourDigit" HeaderText="Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--11--%>
                                                                <asp:BoundField DataField="GROCRate" HeaderText="Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--12--%>
                                                                <asp:BoundField DataField="DisplayCEffRateForFourDigit" HeaderText="Effective Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--13--%>
                                                                <asp:BoundField DataField="GROCEffRate" HeaderText="Effective Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--14--%>
                                                                <asp:BoundField DataField="DisplayCCommercialRateForFourDigit" HeaderText="Commercial Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--15--%>
                                                                <asp:BoundField DataField="COtherCharges" HeaderText="Other Charges">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--16--%>
                                                                <asp:BoundField DataField="DisplayCAmountForFourDigit" HeaderText="Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--17--%>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--18--%>
                                                                <asp:BoundField DataField="Note" HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--19--%>
                                                                <asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
                                                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--20--%>
                                                                <asp:TemplateField HeaderText="CGST Per.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                            OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTPercentage") %>'></asp:TextBox>
                                                                        <asp:CustomValidator ID="cvCGSTPer" runat="server" ControlToValidate="txtCGSTPer"
                                                                            Display="None"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--21--%>
                                                                <asp:BoundField DataField="CGSTCAmount" HeaderText="CGST Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--22--%>
                                                                <asp:TemplateField HeaderText="SGST Per.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                            OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTPercentage") %>'
                                                                            Enabled="false"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--23--%>
                                                                <asp:BoundField DataField="SGSTCAmount" HeaderText="SGST Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--24--%>
                                                                <asp:TemplateField HeaderText="IGST Per.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtIGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                            OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTPercentage") %>'></asp:TextBox>
                                                                        <asp:CustomValidator ID="cvIGSTPer" runat="server" ControlToValidate="txtIGSTPer"
                                                                            Display="None"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--25--%>
                                                                <asp:BoundField DataField="IGSTCAmount" HeaderText="IGST Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--26--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
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
                                    <asp:UpdatePanel runat="server" ID="upnlInvoiceCharge" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblInvoiceCharge" class="clsLabelHeader">Invoice Charge(s):</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddCharge" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click to add Purchase Invoice Charge"></asp:Button>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgInvoiceCharge" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
                                                            CellPadding="3">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
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
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandName="EditCharge" Style="height: 15px;
                                                                                                width: 15px" ImageUrl="~/images/edit.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteCharge" Style="height: 20px;
                                                                                                width: 20px" ImageUrl="~/images/delete.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3" align="left">
                                                        <span id="lblOtherDetails" class="clsLabelHeader">Other Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblTotal" class="clsLabelAuto">Total</span>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtTotal" runat="server" Text="<%# mInvoice.CTotalAmount %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True" Width="150px"></asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtTotal" runat="server" Text="<%# mInvoice.DisplayCTotalAmount %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblTotalOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtTotalOtherCharges" runat="server" Text="<%# mInvoice.CTotalCharges %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Total Other Charges"
                                                            BackColor="#E0E0E0" ReadOnly="True" Width="150px">
                                                        </asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtTotalOtherCharges" runat="server" Text="<%# mInvoice.DisplayCTotalCharges %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Total Other Charges" BackColor="#E0E0E0"
                                                            ReadOnly="True">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabel">Total CGST</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtTotalCGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.CTotalCGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                        </asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtTotalCGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.DisplayCTotalCGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalSGST" runat="server" CssClass="clsLabel">Total SGST</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtTotalSGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.CTotalSGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                        </asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtTotalSGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.DisplayCTotalSGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabel">Total IGST</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtTotalIGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.CTotalIGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                        </asp:TextBox>
                                                        <%--  <asp:TextBox ID="txtTotalIGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            Text="<%# mInvoice.DisplayCTotalIGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblRemaining" class="clsLabelAuto">Grand Total</span>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtGrandTotal" runat="server" Text="<%# mInvoice.CGrandTotal %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Grand Total" BackColor="#E0E0E0"
                                                            ReadOnly="True" Width="150px">
                                                        </asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtGrandTotal" runat="server" Text="<%# mInvoice.DisplayCGrandTotal %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Grand Total" BackColor="#E0E0E0"
                                                            ReadOnly="True">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblAmountInWords" class="clsLabelAuto">Amount In Words </span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtAmountInWords" runat="server" Text="<%# mInvoice.AmountINWords.trim %>"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Amount In Words" ReadOnly="True" BackColor="#E0E0E0"
                                                            TextMode="MultiLine" Width="400px">
                                                        </asp:TextBox>
                                                        <%-- <asp:TextBox ID="txtAmountInWords" runat="server" Text="<%# mInvoice.DisplayAmountINWords.trim %>"
                                                            CssClass="clsTextBoxLong_Ajax" ToolTip="Amount In Words" ReadOnly="True" BackColor="#E0E0E0"
                                                            TextMode="MultiLine">
                                                        </asp:TextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblInvoiceDocketCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Invoice Docket Charge</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtInvoiceDocketCharge" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Invoice Docket Charge" Visible="False" ReadOnly="True" BackColor="#E0E0E0"
                                                            Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalDocketCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Total Docket Charge</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRemark" runat="server" Text="<%# mInvoice.Remark %>" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                            Width="400px" MaxLength="100" ToolTip="Enter Remark" TextMode="MultiLine"
                                                            Rows="5">
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
                                                            ToolTip="Click to cancel Purchase Invoice"></asp:Button>
                                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text="Authorize"
                                                            ToolTip="Click to authorize Purchase Invoice"></asp:Button>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save"
                                                            ToolTip="Click to save Purchase Invoice" ValidationGroup="a"></asp:Button>
                                                        <%--Ajay--%>
                                                        <asp:Button ID="btnSendMail" runat="server" class="clsbtnH clsinfoH1" Text="Send Mail"
                                                            ClientIDMode="Static" ToolTip="Click to Send Mail" Visible="<%# (mInvoice.StatusID = 2) %>"></asp:Button>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print"
                                                            ToolTip="Click to print Purchase Invoice" Enabled="<%# Not mInvoice.IsNew %>">
                                                        </asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            ToolTip="Click to go back to the previous page"></asp:Button>
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;" colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
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
    <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
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

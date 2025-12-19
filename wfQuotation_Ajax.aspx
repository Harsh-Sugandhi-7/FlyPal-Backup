<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfQuotation_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfQuotation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quotation Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
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
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Quotation [New]</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <%--<td colspan="2" align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsbtnH clsinfoH"
                                                                        ToolTip="Click to Cancel the Purchase Quotation"></asp:Button>
                                                                    <asp:Button ID="btnAmend" runat="server" Text="Amend" CssClass="clsbtnH clsinfoH" ToolTip="Click to Amend the Purchase Quotation"
                                                                        ClientIDMode="Static"></asp:Button>
                                                                    <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsbtnH clsinfoH"
                                                                        ToolTip="Click to authorize Purchase Quotation" ValidationGroup="a"></asp:Button>
                                                                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Purchase Quotation"
                                                                        Enabled="<%# Not mQuotation.IsNew %>"></asp:Button>
                                                                    <%--'Added on 25-Jul-2016
                                                                    <asp:Button ID="btnPerformInvoice" runat="server" Text="Proforma Invoice" CssClass="clsbtnH clsinfoH"
                                                                        ToolTip="Click to Print Proforma Invoice" Enabled="<%# Not mQuotation.IsNew %>"
                                                                        Visible="<%# mQuotation.TransTypeId=2 %>"></asp:Button>
                                                                    <%-- ----- 
                                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Purchase Quotation"
                                                                        ValidationGroup="a"></asp:Button>
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
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvQuotationDate" runat="server" OnServerValidate="CustomValidate"
                                                    ValidationGroup="a" Display="None" ControlToValidate="calQuotationDate" ErrorMessage="Select Quotation Date"></asp:CustomValidator><asp:RequiredFieldValidator
                                                        ID="rfvQuotationDate" runat="server" Display="None" ControlToValidate="calQuotationDate"
                                                        ValidationGroup="a" ErrorMessage="Select Quotation Date."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbVendorList" ErrorMessage="Select Vendor from the list."></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvCurrency" runat="server" ClientValidationFunction="ValidateCurrency"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbCurrencyList" ErrorMessage="Select Currency from the list."></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
                                                    ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCustomer" runat="server" OnServerValidate="CustomValidate"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbCustomer" ErrorMessage="Select Customer from the list."></asp:CustomValidator>
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text="<%# mQuotation.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlQuotationDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsQuotationDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                    <legend id="ledQuotationDetails" class="clsLabelHeader">Quotation Details</legend>
                                                    <table>

                                                        <tr>
                                                            <td>
                                                                <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblDate" class="clsLabel">Date</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="calQuotationDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                    Text=""></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calQuotationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calQuotationDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="calQuotationDateWatermarkExtender" runat="server"
                                                                    TargetControlID="calQuotationDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                                                                <asp:TextBox ID="txtText" runat="server" Text="<%# mQuotation.Text %>" CssClass="clsTextBoxTagSearch"
                                                                    onfocus="SetContextKey()" ToolTip="Enter No." MaxLength="25" Width="208px"> </asp:TextBox>
                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                    CompletionInterval="1" ServicePath="wfQuotation_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                    TargetControlID="txtText" UseContextKey="False">
                                                                </cc2:AutoCompleteExtender>
                                                                <script type="text/jscript">
                                                                    function SetContextKey() {
                                                                        var autoComplete = $find('txtText_Autocomplete');
                                                                        var TransTypeID = 'TransTypeID=<%=mQuotation.TransTypeID%>¿QuotationDate=<%=mQuotation.Date%>';
                                                                        autoComplete.set_contextKey(TransTypeID);
                                                                    }
                                                                </script>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNo" runat="server" Text="<%# mQuotation.No %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    MaxLength="8"> </asp:TextBox>
                                                                <asp:TextBox ID="txtAmend" runat="server" Text="<%# mQuotation.Amend %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    MaxLength="2"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="lblNoOfDays" class="clsLabel">No. of Day(s)</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtValidDays" runat="server" Text="<%# mQuotation.ValidDays %>"
                                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Delivery Within" MaxLength="4"
                                                                                Enabled="<%# mQuotation.StatusID = 1 %>" AutoPostBack="True"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblValidUpTo" class="clsLabel">Valid Up To</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtValidUpToDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                                onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                                Text="<%# mQuotation.ValidDateFormatted %>" AutoPostBack="True" autocomplete="off"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtValidUpToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtValidUpToDate"></cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtValidUpToDate" ID="txtValidUpToDateWatermarkExtender"
                                                                                runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:CheckBox ID="chkIsCustomer" runat="server" Text="If Quotation is on behalf of Customer"
                                                                    CssClass="clsLabelAuto" AutoPostBack="True" Checked="<%# mQuotation.IsCustomer %>"
                                                                    Visible="<%# Not(mQuotation.TransTypeID=2) %>" TextAlign="Left"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelAuto" Visible="<%# Not(mQuotation.TransTypeID=2) %>">Customer</asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsComboBox3_Ajax" Visible="<%# Not(mQuotation.TransTypeID=2) %>"
                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mQuotation.CustomerID %>"
                                                                    Enabled="False">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsSupplierDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                    <legend id="lblVendorDetail" class="clsLabelHeader" runat="server">Customer Details</legend>
                                                    <table>

                                                        <tr>
                                                            <td>
                                                                <span id="lblNameStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblName" class="clsLabel">Name</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    Enabled="<%# mQuotation.IsNew %>" DataTextField="Name" DataValueField="ID"
                                                                    SelectedValue="<%# mQuotation.VendorID %>" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblAddress" class="clsLabel">Address</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtAddress" runat="server" Text="<%# mQuotation.Address %>" CssClass="clsTextBoxTagSearchLong" Width="315"
                                                                    MaxLength="250" ToolTip="Address" TextMode="MultiLine" BackColor="#E0E0E0"
                                                                    ReadOnly="True">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblCustomerQuoteNo" runat="server" CssClass="clsLabelAuto">Supplier Quote. No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCustQuoteNo" runat="server" Text="<%# mQuotation.VendorQuoteNo %>"
                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Enter Quotation No." MaxLength="49" Enabled="<%# (CType(mQuotation.TransTypeID, FlyPal.Util.Trans) <> FlyPal.Util.Trans.RequestingForQuotation) %>"> </asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblCustomerQuoteDate" runat="server" CssClass="clsLabelAuto">Supplier Quote. Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCustQuoteDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                    onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                    Text="<%# mQuotation.VendorQuoteDateFormatted %>"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtCustQuoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCustQuoteDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtCustQuoteDate" ID="txtCustQuoteDateWatermarkExtender"
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
                                                                <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    Width="191px" Enabled="<%# mQuotation.IsNew %>" DataTextField="Name" DataValueField="ID"
                                                                    SelectedValue="<%# mQuotation.CurrencyID %>" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblConvFactor" class="clsLabelauto">Factor</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mQuotation.ConversionFactor %>"
                                                                    CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Enter Conversion Factor" MaxLength="9" Style="text-align: right"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
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
                                                                                        runat="server" class="clsbtnH clsinfoH1" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                        Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                        Height="20px" Width="20px"></asp:ImageButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblRoundOffRequire" class="clsLabel">Round Off Required</span>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                                    Checked="<%# mQuotation.IsRoundOff %>" TextAlign="Right"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset id="fdsOpeningLine" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative" visible="<%# mQuotation.TransTypeID=2 %>">
                                            <table>
                                                <tr>
                                                    <td width="6px">&nbsp
                                                    </td>
                                                    <td width="80px">
                                                        <span id="lblOpeningLine" class="clsLabel" runat="server" visible="<%# mQuotation.TransTypeID=2 %>">Opening Line</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOpeningLine" runat="server" CssClass="clsTextBoxTagSearch"
                                                            Height="27px" Width="886px" MaxLength="250" Rows="5" Text="<%# mQuotation.OpeningLine %>" TextMode="MultiLine"
                                                            ToolTip="Enter Opening Line for a Quotation" Visible="<%# mQuotation.TransTypeID=2 %>"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlQuotationItems" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsQuotationItems" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledQuotationItems">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblQuotationItems" class="clsLabelHeader">Quotation Item(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsbtnH clsinfoH"
ValidationGroup="a" ToolTip="Click to add Quotation Items"></asp:Button>--%>

                                                                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                        ToolTip="Click to Add Quotation Items" ValidationGroup="a"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgQuotationItems" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True"
                                                                    DataKeyNames="HSNACSCode" AutoGenerateColumns="false">
                                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                        <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                            <HeaderStyle Wrap="False" />
                                                                            <ItemStyle Wrap="False" />
                                                                            <FooterStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description" />
                                                                        <asp:BoundField DataField="AltPartNo" HeaderText="Alternate Part No."></asp:BoundField>
                                                                        <asp:BoundField DataField="IPCReference" HeaderText="IPC Reference"></asp:BoundField>
                                                                        <asp:BoundField DataField="EnqiryInfo" HeaderText="Tran. Info.">
                                                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                                                            <ItemStyle Wrap="True"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Qty.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8" Style="text-align: right; width: 60px"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>'></asp:TextBox>
                                                                                <asp:CustomValidator ID="cvQtyRule" runat="server" OnServerValidate="CustomValidate1"
                                                                                    ValidationGroup="a" Display="None" ControlToValidate="txtQty" ErrorMessage="Qty must be numbers only"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                                                        <asp:TemplateField HeaderText="Rate">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Style="text-align: right"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" MaxLength="12"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CRate") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Other Charge">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Style="text-align: right"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" MaxLength="12"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"COtherCharges") %>'> </asp:TextBox>
                                                                                <asp:CustomValidator ID="cvOtherChargesRule" runat="server" ErrorMessage="Other Charges must be numbers only"
                                                                                    ValidationGroup="a" ControlToValidate="txtOtherCharges" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
                                                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="CGST(%)">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWCGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    Width="30px" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTPercentage") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="CGST Amt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWCGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    ReadOnly="true" BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTCAmount") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SGST(%)">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWSGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    ReadOnly="true" BackColor="#E0E0E0" Width="30px" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTPercentage") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SGST Amt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWSGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    ReadOnly="true" BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTCAmount") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="IGST(%)">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWIGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    Width="30px" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTPercentage") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="IGST Amt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtWIGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    ReadOnly="true" BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTCAmount") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="MOQ.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtEOQ" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Style="text-align: right; width: 60px" MaxLength="8"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"EOQ") %>'></asp:TextBox>
                                                                                <asp:CustomValidator ID="cvEOQRule" runat="server" ErrorMessage="EOQ must be numbers only"
                                                                                    ValidationGroup="a" ControlToValidate="txtEOQ" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="EOQ Rate">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtEOQCRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Style="text-align: right"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" MaxLength="12"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"EOQCRate") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Bill Back Rate">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtBillBackRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Style="text-align: right"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" OnTextChanged="TextChanged" MaxLength="12"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CBillBackRate") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DeliveryInDays" HeaderText="Lead Time (Days)">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Priority">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                                    Enabled="<%# mQuotation.StatusID = 1 %>" DataSource="<%# mPriorityList %>" DataTextField="Name"
                                                                                    DataValueField="ID" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"PriorityID") %>'>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ModelName" HeaderText="Applicable To" Visible="False" />
                                                                        <asp:BoundField DataField="PaymentTerm" HeaderText="Payment Term"></asp:BoundField>
                                                                        <asp:BoundField DataField="ModelName" HeaderText="Applicable To"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Remark">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="100px"
                                                                                    TextMode="MultiLine" Enabled="<%# mQuotation.StatusID = 1 %>" MaxLength="250"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>'> </asp:TextBox>
                                                                                <asp:CustomValidator ID="cvRemark1" runat="server" ErrorMessage="Other Charges must be numbers only"
                                                                                    ValidationGroup="a" ControlToValidate="txtRemark" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Note">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="100px"
                                                                                    TextMode="MultiLine" Enabled="<%# mQuotation.StatusID = 1 %>" MaxLength="250"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Note") %>'> </asp:TextBox>
                                                                                <asp:CustomValidator ID="cvNote" runat="server" ErrorMessage="Other Charges must be numbers only"
                                                                                    ValidationGroup="a" ControlToValidate="txtNote" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>
                                                                        <%--28--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%>
                                                                                <div class="dropdown">
                                                                                    <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                        Visible='<%#IIf(mQuotation.StatusID > 1, False, True) %>' />

                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                        Visible='<%#IIf(mQuotation.StatusID > 1, False, True) %>' />
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
                                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlQuotationTerms" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsSalesOrderTerms" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOrderSalesTerms">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblQuotationTerms" class="clsLabelHeader">Quotation Term(s)</span>
                                                                </td>
                                                                <td>

                                                                    <asp:ImageButton ID="btnAddTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                        ToolTip="Click to Add Term"></asp:ImageButton>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddSupplierSpecificTerms" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                        Text="Add Supplier Specific Terms" ToolTip="Click To Add Supplier Specific Terms"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgQuotationTerms" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                        <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                            <ItemStyle CssClass="TextBreak" Width="500px" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlQuotationCharges" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsOrderCharges" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOrderCharges">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblChargeDeatails" class="clsLabelHeader">Quotation Charge(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnAddCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                        ToolTip="Click to Add Charge"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgChargeList" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
                                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
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
                                                                        <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>


                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%>
                                                                                <div class="dropdown">
                                                                                    <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                        Visible='<%#IIf(mQuotation.StatusID > 1, False, True) %>' />

                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                        Visible='<%#IIf(mQuotation.StatusID > 1, False, True) %>' />
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
                                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
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
                                                            <asp:TextBox ID="txtCTotal" runat="server" Text="<%# mQuotation.CTotalAmount %>"
                                                                CssClass="clsTextBoxTagSearch" Style="text-align: right" ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabel">Total CGST</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotalCGST" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right" Text="<%# mQuotation.CTotalCGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTotalSGST" runat="server" CssClass="clsLabel">Total SGST</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotalSGST" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right" Text="<%# mQuotation.CTotalSGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabel">Total IGST</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotalIGST" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right" Text="<%# mQuotation.CTotalIGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblTotaolOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCTotalOtherCharge" runat="server" Text="<%# mQuotation.CTotalCharges %>"
                                                                CssClass="clsTextBoxTagSearch" Style="text-align: right" ToolTip="Total Other Charges" BackColor="#E0E0E0"
                                                                ReadOnly="True">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblRemaining" class="clsLabelAuto">Grand Total</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCGrandTotal" runat="server" Text="<%# mQuotation.CGrandTotal %>"
                                                                CssClass="clsTextBoxTagSearch" Style="text-align: right" ToolTip="Grand Total" BackColor="#E0E0E0"
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
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Cancel the Purchase Quotation"></asp:Button>
                                                            <asp:Button ID="btnAmend" runat="server" Text="Amend" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Amend the Purchase Quotation"
                                                                ClientIDMode="Static"></asp:Button>
                                                            <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Click to authorize Purchase Quotation" ValidationGroup="a"></asp:Button>
                                                            <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print Purchase Quotation"
                                                                Enabled="<%# Not mQuotation.IsNew %>"></asp:Button>
                                                            <%--Added on 25-Jul-2016--%>
                                                            <asp:Button ID="btnPerformInvoice" runat="server" Text="Proforma Invoice" CssClass="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Print Proforma Invoice" Enabled="<%# Not mQuotation.IsNew %>"
                                                                Visible="<%# mQuotation.TransTypeId=2 %>"></asp:Button>
                                                            <%-- ----- --%>
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Save Purchase Quotation"
                                                                ValidationGroup="a"></asp:Button>
                                                            <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
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
                                                <asp:Button ID="hdnimgBtnReqPartList" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                <asp:Button ID="hdnBtnQuotationCharge" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                <asp:Button ID="hdnimgBtnQuotationTerm" ClientIDMode="Static" runat="server" Text="----"
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
        <!-- File Upload Modal Dialog-->
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
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                        //                        $("#IFileUpload").ready(function () {
                        //                            $("#btnDummyFileUpload").click();
                        //                            $get("AjaxLoader").style.visibility = 'hidden';
                        //                        });
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
        <!-- Common Part List Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCommonPartList" Text="Dummy Common Part List"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupCommonPartList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupCommonPartList" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCommonPartList" runat="server" TargetControlID="btnDummyCommonPartList"
            PopupControlID="pnlPopupCommonPartList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCommonPartListStateComplete() {
                var UpdatePanel1 = '<%=upnlValidationsummary.ClientID%>';
                if (Page_IsValid) {
                    $("#btnDummyCommonPartList").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                else {
                    __doPostBack(UpdatePanel1, '');
                    $get("AjaxLoader").style.visibility = "hidden";
                }
            }

            function OpenPartsWindow(ItemsCount, TransDate) {
                var Index = $get("cmbAdd").selectedIndex;
                if (Index == 0) {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupCommonPartList").attr("src", "wfCommonPartList_Ajax.aspx?Type=pup&LookinTypeID=1&Name=&OpenFrom=Quotation&TransDate=" + TransDate + "&ItemsCount=" + ItemsCount);
                        if (!$.browser.msie) {
                            $("#btnDummyCommonPartList").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                }

            }


        </script>
        <script type="text/javascript">
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
        <!-- Requisition Part List Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyReqPartList" Text="Dummy Common Part List"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupReqPartList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupReqPartList" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupReqPartList" runat="server" TargetControlID="btnDummyReqPartList"
            PopupControlID="pnlPopupReqPartList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameReqPartListStateComplete() {
                var UpdatePanel1 = '<%=upnlValidationsummary.ClientID%>';
                if (Page_IsValid) {
                    $("#btnDummyReqPartList").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                else {
                    __doPostBack(UpdatePanel1, '');
                    $get("AjaxLoader").style.visibility = "hidden";
                }
            }
            function OpenReqPartsWindow(ItemsCount, TransDate) {
                var Index = $get("cmbAdd").selectedIndex;
                if (Index == 2) {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupReqPartList").attr("src", "wfRequisitionPartList_Ajax.aspx?Type=pup&ListFor=1&TransDate=" + TransDate + "&ItemsCount=" + ItemsCount);
                        if (!$.browser.msie) {
                            $("#btnDummyReqPartList").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                }
            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForReqPartList() {
                var ReqPartListWindow = $find("<%=mdlPopupReqPartList.ClientID %>");
                //close Req Part List popup window
                ReqPartListWindow.hide();
                $("#iPopupReqPartList").attr("src", "JavaScript:''");
                //call Req image button
                $("#hdnimgBtnReqPartList").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
            $(document).ready(function () {
              <% Dim mOpenFrom As String = Request.QueryString("Type") %>
                <% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then %>  
                $('#btnCancel').attr('disabled', 'disabled');
                $('#btnPrint').attr('disabled', 'disabled');
                $('#btnSendMail').attr('disabled', 'disabled');
            <% End if %>  
            });

        </script>
        <!--QuotationCharge Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyQuotationCharge" Text="QuotationCharge" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlQuotationCharge" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeQuotationCharge" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupQuotationCharge" runat="server" TargetControlID="btnDummyQuotationCharge"
            PopupControlID="pnlQuotationCharge" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameQuotationChargeStateComplete() {
                $("#btnDummyQuotationCharge").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenQuotationChargeWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeQuotationCharge").attr("src", "wfQuotationCharge_Ajax.aspx?Type=pup");

                    $("#btnDummyQuotationCharge").click();
                    $get("AjaxLoader").style.visibility = 'hidden';

                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForQuotationCharge() {
                var QuotationChargewindow = $find("<%=mdlPopupQuotationCharge.ClientID %>");
                //close popup window
                QuotationChargewindow.hide();
                //release resources
                $("#IframeQuotationCharge").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnQuotationCharge").click();
            }
        </script>
        <!-- Term Popup Window  btnSalesOrderTermsAdd btnDummyTerm hdnimgBtnTerm pnlPopupTerm iPopupTerm mdlPopupTerm-->

        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyQuotationTerm" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

        </div>
        <asp:Panel runat="server" ID="pnlPopupQuotationTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupQuotationTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupQuotationTerm" runat="server" TargetControlID="btnDummyQuotationTerm"
            PopupControlID="pnlPopupQuotationTerm" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>

        <script type="text/javascript">

            function IFrameTermStateComplete() {

                $("#btnDummyQuotationTerm").click();
                $get("AjaxLoader").style.visibility = 'hidden';

            }
            function OpenTermWindow() {
                try {
                    $("#iPopupQuotationTerm").attr("src", "wfQuotationTerm_Ajax.aspx?Typepup=pup&Type=4&OpenFrom=11");
                    if (!$.browser.msie) {
                        $("#btnDummyQuotationTerm").click();
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>

        <script type="text/javascript">
            function ParentCallBackFunctionForQuotationTerm() {
                var TermWindow = $find("<%=mdlPopupQuotationTerm.ClientID %>");
                //close Term popup window
                TermWindow.hide();
                $("#iPopupQuotationTerm").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnQuotationTerm").click();
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddSupplier = document.getElementById("cmbVendorList");
            if (ddSupplier != null) {
                var i = 0;
                if (ddSupplier.disabled == false) {
              <% For Each item1 In mVendorList%>
                <% If item1.NotInUse = "True" Then%>
                    ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    i = i + 1;
             <% Next%>
                }
            }
            var ddCustomer = document.getElementById("cmbCustomer");
            if (ddCustomer != null) {
                if (ddCustomer.disabled == false) {
                    var j = 0;
              <% For Each item2 In mCustomerList%>
                <% If item2.NotInUse = "True" Then%>
                    ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    j = j + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->


</body>
</html>

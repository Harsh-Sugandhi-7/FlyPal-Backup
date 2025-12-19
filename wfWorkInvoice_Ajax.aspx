<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWorkInvoice_Ajax.aspx.vb"
    Inherits="Flypal.wfWorkInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Invoice Details</title>
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
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Work Invoice [New]</asp:Label>
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
                                                            function ValidateRemark(source, args) {
                                                                args.IsValid = false;
                                                                var textBox = document.getElementById("txtRemark");
                                                                var textLength = textBox.value.length;
                                                                if (textLength < 499) {
                                                                    args.IsValid = true;
                                                                    return;
                                                                }
                                                            }
                                                        </script>
                                                        <asp:CustomValidator ID="cvRemark" runat="server" Display="None" ClientValidationFunction="ValidateRemark"
                                                            ControlToValidate="txtRemark" ValidationGroup="a" ErrorMessage="Remark should not more than 500 Charcters."></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvCurrency" runat="server" ControlToValidate="cmbCurrencyList"
                                                            ClientValidationFunction="ValidateCurrency" ValidationGroup="a" Display="None"
                                                            ErrorMessage="Select Currency from the list."></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvFactor" runat="server" OnServerValidate="customvalidate"
                                                            ValidationGroup="a" Display="None" ErrorMessage="Currency factor must be greater than zero."
                                                            ControlToValidate="txtConversionFactor">
                                                        </asp:CustomValidator><asp:RequiredFieldValidator ValidationGroup="a" ID="rfvFactor"
                                                            runat="server" Display="None" ErrorMessage="Currency factor must be greater than zero."
                                                            ControlToValidate="txtConversionFactor"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                            ValidationGroup="a" Display="None" ErrorMessage="Select Vendor from the list."
                                                            ControlToValidate="cmbVendorList"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvWorkInvoiceDate" runat="server" ValidationGroup="a" Display="None"
                                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblStatus" runat="server" Text="<%# mWorkInvoice.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlWorkInvoiceDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblWorkInvoiceDetails" class="clsLabelHeader">Work Invoice Details </span>
                                                        <span id="lblDateStar1" class="clsLabelStar"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDate" class="clsLabel">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWorkInvoiceDate" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                            CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtWorkInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWorkInvoiceDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtWorkInvoiceDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtWorkInvoiceDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarWorkInvoiceNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Label1" runat="server" cssclass="clsLabelAuto">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mWorkInvoice.Text %>"
                                                            Width="208px" MaxLength="25" onfocus="SetContextKey();" ToolTip="Enter No."> </asp:TextBox>
                                                        <cc2:AutoCompleteExtender ID="txtText_Autocomplete" runat="server" ClientIDMode="Static"
                                                            CompletionInterval="1" CompletionSetCount="20" DelimiterCharacters="" Enabled="True"
                                                            MinimumPrefixLength="0" ServiceMethod="GetDistinctTextListAutoComplete" ServicePath="wfWorkInvoice_Ajax.aspx"
                                                            TargetControlID="txtText" UseContextKey="False">
                                                        </cc2:AutoCompleteExtender>
                                                        <script>


                                                            function SetContextKey() {
                                                                var autoComplete = $find('txtText_Autocomplete');
                                                                var TransTypeID = 'TransTypeID=<%=mWorkInvoice.TransTypeID%>¿QuotationDate=<%=mWorkInvoice.Date%>';
                                                                autoComplete.set_contextKey(TransTypeID);
                                                            }
                                                        </script>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxSmall_Ajax" MaxLength="8"
                                                            Text="<%# mWorkInvoice.No %>"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlVendorDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="6">
                                                        <span id="lblVendorDetails" class="clsLabelHeader">Vendor Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabelauto">Name</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                            AutoPostBack="True" Enabled="<%# mWorkInvoice.IsNew %>" SelectedValue="<%# mWorkInvoice.VendorID %>"
                                                            DataValueField="ID" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAddress" class="clsLabelauto">Address</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="clsTextBoxLong_Ajax" Text="<%# mWorkInvoice.VendorAddress %>"
                                                            ToolTip="Address" MaxLength="250" ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRefNo" class="clsLabelAuto">Ref. No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mWorkInvoice.RefNo %>"
                                                            ToolTip="Enter Ref. No." MaxLength="49">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRefDate" class="clsLabelAuto">Ref. Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRefDate" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                            CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtRefDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRefDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtRefDateTextBoxWatermarkExtender" runat="server"
                                                            TargetControlID="txtRefDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
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
                                                        <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsComboBox_Ajax"
                                                            AutoPostBack="True" DataTextField="Name" DataValueField="ID" Enabled="<%# mWorkInvoice.IsNew %>"
                                                            SelectedValue="<%# mWorkInvoice.CurrencyID %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblFactor" class="clsLabelAuto">Factor</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                            Text="<%# mWorkInvoice.ConversionFactor %>" ToolTip="Enter Conversion Factor"
                                                            MaxLength="9" Enabled="<%# mWorkInvoice.StatusID = 1 %>">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsButton_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRoundOffRequire" class="clsLabelAuto">Round Off Required</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                            Checked="<%# mWorkInvoice.IsRoundOff %>"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlWorkInvoiceItem" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblInvoiceItem" class="clsLabelHeader">Work Invoice Item(s)</span>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" TabIndex="0" Text="Add "
                                                            ValidationGroup="a" ToolTip="Click to Add the Item" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgWorkInvoiceItems" runat="server" AutoGenerateColumns="False"
                                                            CssClass="clsGrid" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
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
                                                                <asp:BoundField DataField="TaskDescription" HeaderText="Task Description">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AMEQty" HeaderText="AME">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AMECRate" HeaderText="AME Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TechQty" HeaderText="Tech.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TechCRate" HeaderText="Tech Rate">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="HelperQty" HeaderText="Helper">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="HelperCRate" HeaderText="Helper Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                    <asp:UpdatePanel runat="server" ID="upnlWorkInvoiceTools" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblWorkInvoiceTools" class="clsLabelHeaderItem">Work Invoice Tool(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnWorkInvoiceTools" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ValidationGroup="a" ToolTip="Click to add Work Invoice Tools"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgWorkInvoiceTools" runat="server" AutoGenerateColumns="False"
                                                            CssClass="clsGrid" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ToolDescription" HeaderText="Tool Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditTool" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteTool" HeaderText="Remove" Text="Remove">
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
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlWorkInvoiceTerms" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblWorkInvoiceTerms" class="clsLabelHeader">Work Invoice Terms(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddTerms" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ToolTip="Click To Add Term"></asp:Button>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgWorkInvoiceTerms" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" CssClass="clsGrid" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="TextBreak" Width="500px" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="DeleteTerm" HeaderText="Remove" Text="Remove">
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
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlWorkInvoiceCharges" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblChargeDeatails" class="clsLabelHeader">Work Invoice Charge(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddCharge" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                        ValidationGroup="a" ToolTip="Click To Add Charge"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgWorkInvoiceCharges" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" CssClass="clsGrid" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
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
                                                                <asp:ButtonField CommandName="EditCharge" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle  HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteCharge" HeaderText="Remove" Text="Remove">
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblTotal" class="clsLabelAuto">Total</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="clsTextBoxRightAlign_Ajax" Text="<%# mWorkInvoice.CTotalAmount %>"
                                                            ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblTotalOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtTotalOtherCharges" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                            Text="<%# mWorkInvoice.CTotalCharges %>" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblGrandTotal" class="clsLabelAuto">Grand Total</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                            Text="<%# mWorkInvoice.CGrandTotal %>" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblAmountInWords" class="clsLabelAuto">Amount In Words </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAmountInWords" runat="server" CssClass="clsTextBox1_Ajax" Text="<%# mWorkInvoice.AmountINWords.trim %>"
                                                            MaxLength="250" TextMode="MultiLine" ReadOnly="True" BackColor="#E0E0E0" Height="40px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1_Ajax" Text="<%# mWorkInvoice.Remark %>"
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
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="clsButton_Ajax" Text="Cancel"
                                                            ToolTip="Click to Cancel the Work Invoice"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="clsButton_Ajax" Text="Authorize"
                                                            ToolTip="Click to Authorize Work Invoice"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save Work Invoice"
                                                            ValidationGroup="a"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to Print Work Invoice"
                                                            Enabled="<%# Not mWorkInvoice.IsNew %>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to go back to the previous page">
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
             if  (ddSupplier != null) {
             var i = 0;
              if  (ddSupplier.disabled ==false)
             {
              <% For Each item1 In mVendorList%>
                <% If  item1.NotInUse ="True" Then%>
                ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceInvoice_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfLineMaintenanceInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Invoice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>

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
            <table class="clstablelistout" id="Table-MaxWidth">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Service Invoice [New]</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Service Invoice Date."
                                                    ControlToValidate="txtInvoiceDate" Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvVendor" runat="server" ErrorMessage="Select Supplier From the list."
                                                    ControlToValidate="cmbVendorList" Display="None" ClientValidationFunction="validateVendor"
                                                    ValidationGroup="1"></asp:CustomValidator>
                                                  <asp:CustomValidator ID="cvCurrency" runat="server" ErrorMessage="Select Currency from the List."
                                                    ControlToValidate="cmbCurrencyList" Display="None" ClientValidationFunction="validateCurrency"
                                                    ValidationGroup="1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvFactor" runat="server" ErrorMessage="Currency factor must be greater than zero."
                                                    ControlToValidate="txtConversionFactor" Display="None" ClientValidationFunction="validateConversionFactor"
                                                    ValidationGroup="1"></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function validateVendor(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("cmbVendorList");
                                                        if (dd.selectedIndex != 0) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }

                                                   
                                                    function validateCurrency(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("cmbCurrencyList");
                                                        if (dd.selectedIndex != 0) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }

                                                    function validateConversionFactor(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("txtConversionFactor");
                                                        if (dd.val != "") {
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
                                        <asp:UpdatePanel runat="server" ID="upnlStatus" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text="<%# mLineMaintInvoice.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="upnlInvoiceDetails" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <fieldset id="fdsInvoiceDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                                <legend id="ledOrderDetails" class="clsLabelHeader">Details</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblDateStar" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblDate" class="clsLabel">Date</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:TextBox runat="server" ID="txtInvoiceDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                                onchange="ValidateDateText(this,'txtInvoiceDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:calendarextender id="txtInvoiceDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                                enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtInvoiceDate">
                                                                            </cc2:calendarextender>
                                                                            <cc2:textboxwatermarkextender targetcontrolid="txtInvoiceDate" id="txtInvoiceDate_watermarkextender"
                                                                                clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                watermarkcssclass="clsDateTextBox">
                                                                            </cc2:textboxwatermarkextender>
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
                                                                            <asp:TextBox ID="txtInvoiceText" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mLineMaintInvoice.Text %>"
                                                                                ToolTip="Enter Service Invoice text" onfocus="SetContextKey()">
                                                                            </asp:TextBox>
                                                                            <cc2:autocompleteextender clientidmode="Static" id="txtInvoiceText_Autocomplete"
                                                                                runat="server" delimitercharacters="" enabled="True" completionsetcount="20"
                                                                                minimumprefixlength="0" completioninterval="1" servicepath="wfLineMaintenanceInvoice_Ajax.aspx"
                                                                                servicemethod="GetDistinctTextListAutoComplete" targetcontrolid="txtInvoiceText"
                                                                                usecontextkey="False">
                                                                            </cc2:autocompleteextender>
                                                                            <script type="text/jscript">
                                                                                function SetContextKey() {
                                                                                    var autoComplete = $find('txtInvoiceText_Autocomplete');
                                                                                    var TransTypeID = 'TransTypeID=69¿QuotationDate=<%=mLineMaintInvoice.LineMaintenanceInvoiceDate%>';
                                                                                    autoComplete.set_contextKey(TransTypeID);
                                                                                }
                                                                            </script>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" Text="<%# mLineMaintInvoice.No %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                MaxLength="8"> </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                         </td>
                                                                        <td>
                                                                            <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                Width="200px" DataTextField="RegNo"
                                                                                DataValueField="ID" SelectedValue="<%# mLineMaintInvoice.MachineID %>" Enabled="False">
                                                                            </asp:DropDownList>
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <span id="lblLocation" class="clsLabel">Location</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                Width="200px" DataTextField="Name"
                                                                                DataValueField="ID" SelectedValue="<%# mLineMaintInvoice.LocationID %>" Enabled="<%# mLineMaintInvoice.StatusID = 1 %>">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
                                                                                                                CssClass="FileAttachmentICN" />
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
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsSupplierDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                    <legend id="ledSupplierDetails" class="clsLabelHeader">Supplier Details</legend>
                                                    <table>

                                                        <tr>
                                                            <td>
                                                                <span id="lblNameStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblName" class="clsLabel">Name</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    Width="350px" Enabled="false" DataTextField="Name" DataValueField="ID"
                                                                    SelectedValue="<%# mLineMaintInvoice.VendorID %>" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblVendorInvNo" runat="server" CssClass="clsLabelAuto">Supplier Inv. No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVendorInvNo" runat="server" Text="<%# mLineMaintInvoice.VendorInvoiceNo %>"
                                                                    CssClass="clsTextBoxTagSearch" Enabled="<%# mLineMaintInvoice.StatusID = 1 %>" MaxLength="25"
                                                                    ToolTip="Enter Supplier Invoice no.">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblInvDate" class="clsLabelAuto">Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVendorInvDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                    Text="<%# mLineMaintInvoice.VendorInvoiceDateFormatted %>"></asp:TextBox>
                                                                <cc2:calendarextender id="txtVendorInvDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                    enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtVendorInvDate">
                                                                </cc2:calendarextender>
                                                                <cc2:textboxwatermarkextender targetcontrolid="txtVendorInvDate" id="txtVendorInvDateWatermarkExtender"
                                                                    runat="server" watermarktext="<%$AppSettings:DateFormat%>">
                                                                </cc2:textboxwatermarkextender>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>
                                                                <span id="lblCurrencyStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblCurrency" class="clsLabel">Currency / Factor</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    Enabled="false" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLineMaintInvoice.CurrencyID %>"
                                                                    AutoPostBack="True" Width="200px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mLineMaintInvoice.ConversionFactor %>"
                                                                    CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Enter Conversion Factor"
                                                                    MaxLength="9" Enabled="false"> </asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <span id="lblRoundOffRequire" class="clsLabel">Round Off Required</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                                                Checked="<%# mLineMaintInvoice.IsRoundOff %>" TextAlign="Right"></asp:CheckBox>
                                                                        </td>

                                                                    </tr>
                                                                </table>
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
                                        <asp:UpdatePanel runat="server" ID="upnlInvoiceItem" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsOrderItemDetails" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOrderItemDetails">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblInvoiceItem" class="clsLabelHeader">Item(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add" ToolTip="Click to add Service Invoice Item"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgLineMaintInvoice" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                                    AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="LineMaintOrderDateFormatted" HeaderText="Service Order Date">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="LineMaintOrderNo" SortExpression="LineMaintOrderNo" HeaderText="Service Order No.">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="JobDetails" HeaderText="Job Details">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditView" runat="server"
                                                                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        class="actionICNS" ToolTip="Click to Edit record."
                                                                                                        CommandName="EditView" ImageUrl="~/images/edit.png" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        class="actionICNS  largerActionICNS"
                                                                                                        ToolTip="Click to Delete record."
                                                                                                        CommandName="DeleteRecord" ImageUrl="~/images/delete.png" />
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
                                        <asp:UpdatePanel runat="server" ID="upnlInvoiceTerm" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsOrderTerms" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOrderTerms">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblTerms" class="clsLabelHeader">Term(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddTerm" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click To Add Term"></asp:Button>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgLineMaintInvoiceTerm" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
                                                                    CellPadding="3">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <%--0--%>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                        <%--1--%>
                                                                        <asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
                                                                            <ItemStyle CssClass="TextBreak" Width="500px" />
                                                                        </asp:BoundField>
                                                                        <%--2--%>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteTerm" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="DeleteTerm" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlInvoiceCharge" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsOrderCharges" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOrderCharges">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblOrderCharges" class="clsLabelHeader">Charge(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddCharges" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click To Add Charge"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </legend>
                                                    <table width="100%">

                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgLineMaintInvoiceCharge" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
                                                                    CellPadding="3">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
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
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="EditCharge" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="EditCharge" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="DeleteCharge" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                        CommandName="DeleteCharge" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                    <td></td>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="Fieldset1" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
                                                    <legend id="ledOtherDetails" class="clsLabelHeader">Other Details
                                                    </legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblTotal" class="clsLabel">Total</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="150px" Text="<%# mLineMaintInvoice.CTotalAmount %>"
                                                                    ReadOnly="True" BackColor="#E0E0E0">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblTotalOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTotalOtherCharges" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="150px"
                                                                    Text="<%# mLineMaintInvoice.CTotalCharges %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblGrandTotal" class="clsLabel">Grand Total</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="150px"
                                                                    Text="<%# mLineMaintInvoice.CGrandTotal %>" ReadOnly="True" BackColor="#E0E0E0">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblAmountInWords" class="clsLabelAuto">Amount In Words </span>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtAmountInWords" runat="server" Text="<%# mLineMaintInvoice.AmountINWords %>"
                                                                    CssClass="clsTextBoxTagSearch" ToolTip="Amount In Words" ReadOnly="True" BackColor="#E0E0E0"
                                                                    TextMode="MultiLine" Width="400px">
                                                                </asp:TextBox>
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
                                        <asp:UpdatePanel runat="server" ID="upnlActionBtn" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <td>
                                                                <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
                                                                    ToolTip="Click to cancel Service Invoice"></asp:Button>

                                                                <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text="Authorize"
                                                                    ValidationGroup="1" ToolTip="Click to authorize Service Invoice"></asp:Button>

                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ValidationGroup="1"
                                                                    Text="Save" ToolTip="Click to save Service Invoice"></asp:Button>

                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" ToolTip="Click to print Service Invoice"
                                                                    Enabled="<%# Not mLineMaintInvoice.IsNew %>"></asp:Button>

                                                                <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"></asp:Button>

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
                <tr style="height: 0px;">
                    <td colspan="2" style="height: 0px;">
                        <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="hdnimgBtnLineMaintenanceInvoiceTerm" ClientIDMode="Static" runat="server" Text="----"
                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                <asp:Button ID="hdnBtnLineMaintenanceInvoiceCharge" ClientIDMode="Static" runat="server" Text="----"
                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                    CausesValidation="False" Style="display: none;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
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

        <!--LineMaintenanceInvoiceCharge Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLineMaintenanceInvoiceCharge" Text="LineMaintenanceInvoiceCharge" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlLineMaintenanceInvoiceCharge" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeLineMaintenanceInvoiceCharge" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupLineMaintenanceInvoiceCharge" runat="server" targetcontrolid="btnDummyLineMaintenanceInvoiceCharge"
            popupcontrolid="pnlLineMaintenanceInvoiceCharge" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function IFrameLineMaintenanceInvoiceChargeStateComplete() {
                $("#btnDummyLineMaintenanceInvoiceCharge").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLineMaintenanceInvoiceChargeWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLineMaintenanceInvoiceCharge").attr("src", "wfLineMaintenanceInvoiceCharge_Ajax.aspx?Type=pup");

                    $("#btnDummyLineMaintenanceInvoiceCharge").click();
                    $get("AjaxLoader").style.visibility = 'hidden';

                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForLineMaintenanceInvoiceCharge() {
                var LineMaintenanceInvoiceChargewindow = $find("<%=mdlPopupLineMaintenanceInvoiceCharge.ClientID %>");
                //close popup window
                LineMaintenanceInvoiceChargewindow.hide();
                //release resources
                $("#IframeLineMaintenanceInvoiceCharge").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnLineMaintenanceInvoiceCharge").click();
            }
        </script>
        <!-- End-->

        <!-- Term Popup Window  btnLineMaintenanceInvoiceTermAdd btnDummyTerm hdnimgBtnTerm pnlPopupTerm iPopupTerm mdlPopupTerm-->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyLineMaintenanceInvoiceTerm" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

        </div>
        <asp:Panel runat="server" ID="pnlPopupLineMaintenanceInvoiceTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupLineMaintenanceInvoiceTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupLineMaintenanceInvoiceTerm" runat="server" targetcontrolid="btnDummyLineMaintenanceInvoiceTerm"
            popupcontrolid="pnlPopupLineMaintenanceInvoiceTerm" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>

        <script type="text/javascript">

            function IFrameTermStateComplete() {

                $("#btnDummyLineMaintenanceInvoiceTerm").click();
                $get("AjaxLoader").style.visibility = 'hidden';

            }
            function OpenTermWindow() {
                try {
                    $("#iPopupLineMaintenanceInvoiceTerm").attr("src", "wfLineMaintenanceInvoiceTerm_Ajax.aspx?Typepup=pup&Type=8&OpenFrom=11");
                    if (!$.browser.msie) {
                        $("#btnDummyLineMaintenanceInvoiceTerm").click();
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>

        <script type="text/javascript">
            function ParentCallBackFunctionForLineMaintenanceInvoiceTerm() {
                var TermWindow = $find("<%=mdlPopupLineMaintenanceInvoiceTerm.ClientID %>");
                //close Term popup window
                TermWindow.hide();
                $("#iPopupLineMaintenanceInvoiceTerm").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnLineMaintenanceInvoiceTerm").click();
            }
        </script>
        <!-- End-->

        <!-- File Upload Modal Dialog-->
        <div id="FileUploadModal">

            <div style="display: none">
                <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
            </div>
            <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:modalpopupextender id="mdlPopupFileUpload" runat="server" targetcontrolid="btnDummyFileUpload"
                popupcontrolid="pnlFileUpload" backgroundcssclass="clsModalPopupBG">
            </cc2:modalpopupextender>

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
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOtherCharge_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfOtherCharge_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Other Charge Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Other Charge [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvNo" runat="server" OnServerValidate="CustomValidate" Display="None"
                                            ControlToValidate="txtNo" ErrorMessage="Number Required." ValidationGroup="a"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOtherChargeDate" runat="server" OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="txtOtherChargeDate" ErrorMessage="Select OtherCharge Date"
                                            ValidationGroup="a"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvOtherChargeDate" runat="server" Display="None"
                                            ControlToValidate="txtOtherChargeDate" ErrorMessage="Select OtherCharge Date."
                                            ValidationGroup="a"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlOthrChargeDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblOrderDetails" class="clsLabelHeader">Other Charge </span>
                                                </td>
                                                <td align="left">
                                                    <span id="lblDetails" class="clsLabelHeader">Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table id="tabDetails">
                                                        <tr>
                                                            <td>
                                                                <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblDate" class="clsLabel">Date</span>
                                                            </td>
                                                            <td>
                                                                <table cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtOtherChargeDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                                AutoPostBack="true" onchange="ValidateDateText(this,'OtherChargeDate_watermarkextender','true');"
                                                                                Text="" ></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="OtherChargeDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOtherChargeDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender ID="OtherChargeDate_watermarkextender" runat="server"
                                                                                TargetControlID="txtOtherChargeDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td align="left">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblNoStar1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblNo" class="clsLabel">No.</span>
                                                            </td>
                                                            <td>
                                                                <table id="Table4" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                                Text="<%# mOtherCharge.Text %>" ToolTip="Enter Other Charge text " onfocus="SetContextKey()">
                                                                            </asp:TextBox>
                                                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                CompletionInterval="1" ServicePath="wfOtherCharge_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                                TargetControlID="txtText" UseContextKey="False">
                                                                            </cc2:AutoCompleteExtender>
                                                                            <script type="text/jscript">
                                                                                function SetContextKey() {
                                                                                    var autoComplete = $find('txtText_Autocomplete');
                                                                                    var TransTypeID = 'TransTypeID=29¿QuotationDate=<%=mOtherCharge.Date%>';
                                                                                    autoComplete.set_contextKey(TransTypeID);
                                                                                }
                                                                            </script>
                                                                        </td>
                                                                        <td class="clstablecell">
                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="8"
                                                                                Text="<%# mOtherCharge.No %>" ToolTip=" Enter Other Charge No." Enabled="false">
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
                                                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <input type="button" id="btnSelectFile" value="Select File" 
                                                                                        runat="server" class="clsbtnH clsinfoH1" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                        Text="Remove Attachment" Enabled="False" ></asp:Button>
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
                                                    </table>
                                                </td>
                                                <td valign="top">
                                                    <table id="tabOrderDetails">
                                                        <tr>
                                                            <td>
                                                                <span id="lblName" class="clsLabel">Bill Entry No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBillEntryNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    Text="<%# mOtherCharge.BillEntryNo %>" ToolTip="Enter Bill Entry No.">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="Label3" class="clsLabel">Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBillEntryDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'BillEntryDate_watermarkextender','false');"
                                                                    Text="" ></asp:TextBox>
                                                                <cc2:CalendarExtender ID="BillEntryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtBillEntryDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="BillEntryDate_watermarkextender" runat="server"
                                                                    TargetControlID="txtBillEntryDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblAddress" class="clsLabel">Master Airway Bill No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMasterAirwayBillNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" Text="<%# mOtherCharge.MasterAirwayBillNo %>" ToolTip="Enter Master Airway Bill No.">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="Label4" class="clsLabel">Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMasterAirwayBillDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'MasterAirwayBillDate_watermarkextender','false');"
                                                                    Text="" ></asp:TextBox>
                                                                <cc2:CalendarExtender ID="MasterAirwayBillDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtMasterAirwayBillDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="MasterAirwayBillDate_watermarkextender" runat="server"
                                                                    TargetControlID="txtMasterAirwayBillDate" WatermarkCssClass="clsDateTextBox"
                                                                    WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Label1" class="clsLabel">House Airway Bill No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHouseAirwayBillNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                    MaxLength="50" Text="<%# mOtherCharge.HouseAirwayBillNo %>" ToolTip="Enter House Airway Bill No">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="Label5" class="clsLabel">Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHouseAirwayBillDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'HouseAirwayBillDate_watermarkextender','false');"
                                                                    Text="" ></asp:TextBox>
                                                                <cc2:CalendarExtender ID="HouseAirwayBillDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtHouseAirwayBillDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="HouseAirwayBillDate_watermarkextender" runat="server"
                                                                    TargetControlID="txtHouseAirwayBillDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlInvoices" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="Label2" class="clsLabelHeader">Invoice List</span>
                                                            </td>
                                                            <td align="right">
                                                                <%--<asp:Button ID="btnAddInvoice" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                    ToolTip="Click to add Invoice"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnAddInvoice" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                    ToolTip="Click to Add Invoice" ></asp:ImageButton>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgInvoices" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                        Width="100%" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Vendor" HeaderText="Supplier">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorInvNo" HeaderText="Supplier Inv. No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorInvDateFormatted" HeaderText="Supplier Inv. Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Currency" HeaderText="Currency">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ConversionFactor" HeaderText="Conv. Factor">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total ">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlCharges" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblOrderTerms" class="clsLabelHeader">Charge List</span>
                                                            </td>
                                                            <td align="right">
                                                                <%--<asp:Button ID="btnAddCharge" runat="server" CssClass="clsButton_Ajax" Text="Add"
                                                                    ToolTip="Click To Add Charge"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnAddCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                    ToolTip="Click to Add Charge" ></asp:ImageButton>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgCharges" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" Width="100%" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="ChargeName" HeaderText="Charge">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Vendor" HeaderText="Service Provider">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice Number">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Currency" HeaderText="Currency">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ConversionFactor" HeaderText="Conv . Factor">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CServiceCharges" HeaderText="GST/Charge">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CGrandTotal" HeaderText="Total Amount">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                            
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditViewRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtn" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to save Other Charge"
                                                        ValidationGroup="a"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" ToolTip="Click to Print Other Charge" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to go back to the previous page">
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
    <!-- End File Upload Modal Dialog-->
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
    </form>
</body>
</html>

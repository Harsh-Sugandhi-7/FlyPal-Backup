<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTransactionsOfInventory_Ajax.aspx.vb"
    Inherits="Flypal.wfTransactionsOfInventory_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part No Status</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" nowrap>
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" class="clsFormHeader1Newstyle">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="LblTitle" runat="server" CssClass="clsFormHeader">Part Status
                                                                </asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="pnlButton" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                Text="Print" ToolTip="Click to Print" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                Text="Close" ToolTip="Click to Close" />
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidations" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvTransactions" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                            ErrorMessage="Select at least one Reference Document " Display="None" ControlToValidate="cmbDocType" ValidationGroup="a"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromdate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="From Date Required" Display="None" ControlToValidate="txtFromDate" ValidationGroup="a"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvTodate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" Display="None" ControlToValidate="txtToDate" ValidationGroup="a"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblTransactions" class="clsLabel">Transactions</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDocType" runat="server" AutoPostBack="True">
                                                                                <asp:ListItem Value="0">(SELECT)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Enquiry</asp:ListItem>
                                                                                <asp:ListItem Value="2">Quotation</asp:ListItem>
                                                                                <asp:ListItem Value="3">Purchase Order</asp:ListItem>
                                                                                <asp:ListItem Value="4">Receipt</asp:ListItem>
                                                                                <asp:ListItem Value="5">Issue</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span8" class="clsLabel">Range</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True">
                                                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblFromDate" class="clsLabel" runat="server">From Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblToDate" class="clsLabel" runat="server">To Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                ToolTip="Click to find list of records as per searching criteria" ValidationGroup="a">
                                                            </asp:ImageButton>
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
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblEnquiryList" runat="server" CssClass="clsLabelAuto " Font-Bold="True"
                                                        Visible="False">List of Enquiry as per criteria :</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgEnquiryList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal" PageSize="10"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EnquiryNo" HeaderText="Number">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Status" HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EnquiryItemQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="PriorityName" HeaderText="Priority">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblQuotationList" runat="server" CssClass="clsLabelAuto " Font-Bold="True"
                                                        Visible="False">List of Quotation as per criteria :</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgQuotationList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal" PageSize="10"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="QuotationTextNo" HeaderText="Number">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Supplier">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="CurrencyName" HeaderText="Currency">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="Status" HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="Unitame" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPurchaseOrderList" runat="server" CssClass="clsLabelAuto " Font-Bold="True"
                                                        Visible="False">List of Enquiry as per criteria :</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgOrderList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal" PageSize="10"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="OrderDate" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="OrderNumber" HeaderText="Number">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="OrderType" HeaderText="Order Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="Status" HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Supplier">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="PerDiscount" HeaderText="Discount">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="NetRate" HeaderText="Net Rate">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="DeliveryInDays" HeaderText="Delivery In Days">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                                <ItemStyle  HorizontalAlign="Right"/>
                                                            </asp:BoundField>
                                                            <%--12--%>
                                                            <asp:BoundField DataField="PriorityName" HeaderText="Priority">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblReceiptList" runat="server" CssClass="clsLabelAuto " Font-Bold="True"
                                                        Visible="False">List of Enquiry as per criteria :</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgReceiptCumInvoiceList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal" PageSize="10"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="ReceiptDate" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="ReceiptNumber" HeaderText="Number">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="RCIType" HeaderText="Receipt Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="Status" HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="FromName" HeaderText="From">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="OrderInfo" HeaderText="Order Info." HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            <%--7--%>
                                                            <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info." HtmlEncode="false">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                            <%--8--%>
                                                            <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle  HorizontalAlign="Right" Wrap="False"/>
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                 <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="StoreLocation" HeaderText="Store/Location" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            
                                                            <%--12--%>
                                                            <asp:BoundField DataField="CureDateQtr" HeaderText="Cure Info.">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:BoundField DataField="ExpDateQtr" HeaderText="Exp. Info">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            <%--14--%>
                                                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                                 <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblIssueList" runat="server" CssClass="clsLabelAuto " Font-Bold="True"
                                                        Visible="False">List of Enquiry as per criteria :</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgIssueList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="5" CssClass="clsGridNewStyle" GridLines="Horizontal" PageSize="10"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="ItemID" HeaderText="ID" Visible="False" />
                                                            <%--1--%>
                                                            <asp:BoundField DataField="IssueDate" HeaderText="Date">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="IssueNumber" HeaderText="Number">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="IssueType" HeaderText="Type">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="Status" HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="StoreName" HeaderText="From">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="IssueTo" HeaderText="Issue To">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
                                                               <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info." HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                            <%--10--%>
                                                            <asp:BoundField DataField="VendorInvoiceInfo" HeaderText="Supp. Inv. Info." HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                            <%--12--%>
                                                            <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Rel. Note Info." HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                            <%--14--%>
                                                            <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="right" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle HorizontalAlign="right"  Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--15--%>
                                                            <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                 <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--16--%>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No">
                                                                 <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--17--%>
                                                            <asp:BoundField DataField="ExpDateQtr" HeaderText="Exp. Info.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                    </asp:GridView>
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
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

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
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceiptPendingOrderList_Ajax.aspx.vb"
    Inherits="Flypal.wfReceiptPendingOrderList_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Orders/Issues/Receipts</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
        <%-- AJAX ScriptManager --%>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLedgerList" runat="server" CssClass="clsFormHeader">List Of Receipt Pending Orders</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCreateOrder" runat="server" CssClass="clsbtnH clsinfoH" Visible="False"
                                                                        ToolTip="Click to create automatic order if it does not exists" Text="Create Order"></asp:Button>
                                                                    <asp:Button ID="btnDone" runat="server" CssClass="clsbtnH clsinfoH" Visible="False"
                                                                        ToolTip="Click to add selected Part(s)." Text="Done"></asp:Button>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click To Go Back To Previous Page"></asp:Button>
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
                            <tr>
                                <td>
                                    <asp:Label ID="lblLabel" runat="server" CssClass="clsLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbFromLastOrder" runat="server" CssClass="clsRadioButton" Text="From Last Order"
                                                            GroupName="a"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbFromAllPendingOrder" runat="server" CssClass="clsRadioButton"
                                                            Text="From All Pending Order (s)" GroupName="a"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkReturnableBackFromCustomer" runat="server" CssClass="clsCheckBox"
                                                            Visible="False" Text="Returnable Back From Customer"></asp:CheckBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto" Visible="False">Part No</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                            ToolTip="Enter Part Number" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblOrderNo" class="clsLabel" runat="server" visible="false">Order No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                        AutoPostBack="True" DataValueField="Text" DataTextField="Text" Visible="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                        Width="55px" Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="1"
                                                                        Width="55px" Visible="false">
                                                                    </asp:TextBox>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlOrderList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgOrderList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" CellPadding="5"
                                                ForeColor="Black" GridLines="Horizontal" PageSize="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderDateFormatted" HeaderText="Date">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderNo" SortExpression="OrderNo" HeaderText="Order No.">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IntOrderNo" SortExpression="IntOrderNo" HeaderText="Int. Order No.">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Type">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AircraftReg" SortExpression="AircraftReg" HeaderText="For Aircraft">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="Blue" />
                                                        <ItemStyle ForeColor="Blue" Wrap="False" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlIssueList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgIssueList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" ForeColor="Black"
                                                GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField DataField="ILDateFormatted" HeaderText="Date">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IssueNo" SortExpression="IssueNo" HeaderText="Issue No.">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="VendorName" SortExpression="VendorName"
                                                        HeaderText="Customer">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="RegNo" SortExpression="RegNo" HeaderText="Aircraft">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="StoreName" SortExpression="StoreName"
                                                        HeaderText="From Store">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="ToStoreName" SortExpression="ToStoreName"
                                                        HeaderText="To Store">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="WorkShopName" SortExpression="WorkShopName"
                                                        HeaderText="Work Shop">
                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="WorkOrderNo" SortExpression="WorkOrderNo"
                                                        HeaderText="W. O. No.">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                        <HeaderStyle HorizontalAlign="Left" ForeColor="Blue" />
                                                        <ItemStyle ForeColor="Blue" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlReceiptList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgReceiptList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" ForeColor="Black"
                                                GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField DataField="RecdDateFormatted" HeaderText="Date">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="Type">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                        <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="Blue" />
                                                        <ItemStyle ForeColor="Blue" Wrap="False" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTransItemList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTransItemListResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton ID="lnkSelectAll" runat="server" CssClass="clsHyperlink1" Visible="False">Select All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgTransItemList" runat="server" CssClass="clsGridNewStyle" AllowPaging="False"
                                                            AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" ForeColor="Black"
                                                            GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Qty" SortExpression="Qty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PendingItemQty" SortExpression="PendingItemQty" HeaderText="Balance Qty.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RequestedBy" SortExpression="RequestedBy" HeaderText="Requested By">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Blue" />
                                                                    <ItemStyle ForeColor="Blue" />
                                                                </asp:ButtonField>
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
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlItemReceiptDetail" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblItemReceiptDetailResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgItemReceiptDetail" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                            ClientIDMode="Static" ShowHeaderWhenEmpty="True" ForeColor="Black"
                                                            GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" class="cbSelectRow" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%-- <asp:BoundField DataField="BalanceQty" SortExpression="BalanceQty" HeaderText="Balance Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>--%>
                                                                <asp:BoundField DataField="BalanceQtyToDisplay" SortExpression="BalanceQtyToDisplay"
                                                                    HeaderText="Balance Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReceiptNumber" SortExpression="ReceiptNumber" HeaderText="Receipt No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R. N. No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R. N. Date">
                                                                    <HeaderStyle ForeColor="black" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderNumber" SortExpression="OrderNumber" HeaderText="Order No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="IssueNumber" SortExpression="IssueNumber"
                                                                    HeaderText="Issue No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="IssueDateFormatted" SortExpression="IssueDateFormatted"
                                                                    HeaderText="Issue Date">
                                                                    <HeaderStyle Wrap="False" ForeColor="black"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
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
        <%--<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </asp:UpdateProgress>--%>
    </form>
    <script type="text/javascript">
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgItemReceiptDetail tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");

                    }
                });


            });
        });


    </script>
</body>
</html>

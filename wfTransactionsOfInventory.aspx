<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTransactionsOfInventory.aspx.vb"
    Inherits="Flypal.wfTransactionsOfInventory" %>
   
<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
 <%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Part No Status</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 
        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Part Status</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table class="clsTable1" id="Table4">
                                    <tr>
                                        <td align="left" colspan="3">
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblTransactions" runat="server" CssClass="clslabel" >Transactions</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbDocType" runat="server" CssClass="clsComboBox" AutoPostBack="True">
                                                <asp:ListItem Value="0">&lt;SELECT&gt;</asp:ListItem>
                                                <asp:ListItem Value="1">Enquiry</asp:ListItem>
                                                <asp:ListItem Value="2">Quotation</asp:ListItem>
                                                <asp:ListItem Value="3">Purchase Order</asp:ListItem>
                                                <asp:ListItem Value="4">Receipt</asp:ListItem>
                                                <asp:ListItem Value="5">Issue</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CustomValidator ID="cvTransactions" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                ErrorMessage="Select at least one Reference Document " Display="None" ControlToValidate="cmbDocType"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromdate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="From Date Required" Display="None" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvTodate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="To Date Required" Display="None" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblDate" runat="server" CssClass="clslabel" Width="48px">Date</asp:Label>
                                        </td>
                                        <td align="left" colspan="2">
                                            <table id="Table1" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top">
                                                        &nbsp;
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From</asp:Label>&nbsp;
                                                    </td>
                                                    <td>
                                                        <uc1:SICalendar ID="txtFromDate" runat="server" Visible="False"></uc1:SICalendar>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>&nbsp;
                                                    </td>
                                                    <td>
                                                        <uc1:SICalendar ID="txtToDate" runat="server" Visible="False"></uc1:SICalendar>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <table id="Table3" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to find as per criteria"
                                                Text="Find Now"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblEnquiryList" runat="server" CssClass="clsLabelHeader" Visible="False">Enquiry List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgEnquiryList" runat="server" CssClass="clsGrid" Visible="False"
                                    AutoGenerateColumns="False" PageSize="25" AllowPaging="True" DESIGNTIMEDRAGDROP="139">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="EnquiryNo" SortExpression="EnquiryNo" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorName" SortExpression="VendorName" HeaderText="Vendor">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemName" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemDescription" HeaderText="Description"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="EnquiryItemQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UnitName" HeaderText="Unit"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="PriorityName" HeaderText="Priority"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblQuotationList" runat="server" CssClass="clsLabelHeader" Visible="False">Quotation List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgQuotationList" runat="server" CssClass="clsGrid" Visible="False"
                                    AutoGenerateColumns="False" PageSize="25" AllowPaging="True" DESIGNTIMEDRAGDROP="139">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="QuotationTextNo" SortExpression="QuotationTextNo" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Qty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Unitame" HeaderText="Unit"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CRate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CAmount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblPurchaseOrderList" runat="server" CssClass="clsLabelHeader" Visible="False">Purchase Order List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgOrderList" runat="server" CssClass="clsGrid" Visible="False"
                                    AutoGenerateColumns="False" PageSize="25" AllowPaging="True">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="OrderDate" HeaderText="Date">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderNumber" SortExpression="OrderNumber" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderType" HeaderText="Order Type">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorName" HeaderText="Supplier"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Qty" SortExpression="KindAttn" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UnitName" HeaderText="Unit"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CRate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PerDiscount" HeaderText="Discount">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="NetRate" HeaderText="Net Rate">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CAmount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="DeliveryInDays" HeaderText="Delivery In Days">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PriorityName" HeaderText="Priority"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblReceiptList" runat="server" CssClass="clsLabelHeader" Visible="False">Receipt List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgReceiptCumInvoiceList" runat="server" CssClass="clsGrid" Visible="False"
                                    AutoGenerateColumns="False" PageSize="25" AllowPaging="True" DESIGNTIMEDRAGDROP="139">
                                    <SelectedItemStyle Wrap="False"></SelectedItemStyle>
                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                    <AlternatingItemStyle Wrap="False" CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle Wrap="False" CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle Wrap="False" CssClass="clsdgHeader"></HeaderStyle>
                                    <FooterStyle Wrap="False"></FooterStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="ReceiptDate" HeaderText="Date">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReceiptNumber" SortExpression="ReceiptNumber" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="RCIType" HeaderText="Receipt Type"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="FromName" HeaderText="From"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemTypeName" HeaderText="Part Type"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderNumber" HeaderText="Order No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="OrderDate" HeaderText="Order Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReleaseNoteNo" HeaderText="Rel. Note No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReleaseNotedate" HeaderText="Rel. Note Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DisplayQty" SortExpression="DisplayQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="DisplayUnitName" SortExpression="DisplayUnitName" HeaderText="Unit">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="SerialNo" HeaderText="Serial No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReceivingStoreName" HeaderText="Store"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Location" HeaderText="Location"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Curedate" HeaderText="Cure Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CureQtrs" HeaderText="Cure Qtrs"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Expirydate" HeaderText="Exp. Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ExpiryQtrs" HeaderText="Exp. Qtrs"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="BatchNo" HeaderText="Batch No."></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" Wrap="False">
                                    </PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblIssueList" runat="server" CssClass="clsLabelHeader" Visible="False">Issue List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid ID="dgIssueList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="25" AllowPaging="True" DESIGNTIMEDRAGDROP="139">
                                    <SelectedItemStyle Wrap="False"></SelectedItemStyle>
                                    <EditItemStyle Wrap="False"></EditItemStyle>
                                    <AlternatingItemStyle Wrap="False" CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle Wrap="False" CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle Wrap="False" CssClass="clsdgHeader"></HeaderStyle>
                                    <FooterStyle Wrap="False"></FooterStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ItemID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="IssueDate" HeaderText="Date">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="IssueNumber" SortExpression="IssueNumber" HeaderText="Number">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="IssueType" HeaderText="Type">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="StoreName" HeaderText="Store"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="IssueTo" HeaderText="Issue To"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemTypeName" HeaderText="Part Type"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReceiptTextNo" HeaderText="Receipt No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReceiptDate" HeaderText="Receipt Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorInvoiceNo" HeaderText="Supp. Inv. No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="VendorInvoiceDate" HeaderText="Supp. Inv. Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReleaseNoteNo" HeaderText="Rel. Note No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ReleaseNotedate" HeaderText="Rel. Note Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DisplayQty" SortExpression="DisplayQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="DisplayUnitName" SortExpression="DisplayUnitName" HeaderText="Unit">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="SerialNo" HeaderText="Serial No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Expirydate" HeaderText="Exp. Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ExpiryQtrs" HeaderText="Exp. Qtrs"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" Wrap="False">
                                    </PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrint" runat="server"  CssClass="clsButton" TabIndex="0" Text="Print" ToolTip="Click to Print" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" 
                                                    CssClass="clsButton" TabIndex="0" Text="Close" 
                                                    ToolTip="Click to Close Part No. status screen" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

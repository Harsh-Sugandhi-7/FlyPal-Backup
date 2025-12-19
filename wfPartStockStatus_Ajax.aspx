<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartStockStatus_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfPartStockStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Part Stock Status List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script language="javascript" src="jquery-1.6.1.min.js"></script>
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
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <span id="lblPartStockStatusList" class="clsFormHeader">Part Stock Status List</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlFindNowButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No./Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part Number or Description to search"
                                                                        CssClass="clsTextBoxTagSearch" Width="400px"  MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel">Category</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="300px" DataValueField="ID"
                                                                        Visible="false" DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCodeNo" runat="server" CssClass="clsLabelAuto">GSE No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGSENo" runat="server" ToolTip="Enter GSE No." CssClass="clsTextBox1_Ajax"
                                                                        MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowBERPart" runat="server" CssClass="clsLabelAuto" Text="Show BER Part">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSelectAllParts" runat="server" CssClass="clsButtonLong_Ajax" ToolTip="Click to Issue All available Parts in single click"
                                                                        Text="Select All Parts"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                       <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find the Part"
                                                            Text="Find Now"></asp:Button>--%>
                                                             <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to Find the Part"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlStockItemList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            <asp:GridView ID="dgStockItemList" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="true" AllowPaging="true" AllowSorting="True">
                                                 <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AlternateParts" SortExpression="AlternateParts" HeaderText="Alternate Parts">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Category" SortExpression="Category" HeaderText="Category">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Applicability" HeaderText="Applicability">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalQuantity" SortExpression="TotalQuantity" HeaderText="Total Stock">
                                                        <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Unit" SortExpression="Unit" HeaderText="Unit">
                                                        <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlRemovedAsReturnableFromAircraft" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult3" runat="server" CssClass="clsLabelHeader" Visible="false">Item Stock List Record(s) Found</asp:Label>
                                            <asp:GridView ID="dgRemovedAsReturnableFromAircraft" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal"
                                                ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField DataField="ReceiptItemID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ReceiptItemID"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReceiptID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ReceiptID"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReceiptTextNo" HeaderText="Receipt No." SortExpression="ReceiptTextNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AircraftRemovedQty" HeaderText="Removed Qty." SortExpression="AircraftRemovedQty">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                        <HeaderStyle    HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date" SortExpression="ReleaseNoteDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPendingItemList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">Item Stock List Record(s) Found</asp:Label>
                                            <asp:GridView ID="dgPendingItemList" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <%--0--%>
                                                    <asp:BoundField DataField="ShowStatusForRemovedAsReturnableFromAircraft">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" Font-Bold="true" />
                                                    </asp:BoundField>
                                                    <%--1--%>
                                                    <asp:BoundField HeaderText="Color" HeaderStyle-HorizontalAlign="Left">
            
                                                        <ItemStyle CssClass="clsColorLabel"  width="3px" Height="3px"/>
                                                    </asp:BoundField>
                                                    <%--2--%>
                                                    <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--3--%>
                                                    <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDescription">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--4--%>
                                                    <asp:BoundField DataField="AvailableQuantity" HeaderText="Available Qty." SortExpression="AvailableQuantity">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <%--5--%>
                                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--6--%>
                                                    <%-- <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>--%>
                                                    <%--7--%>  <%--6--%>
                                                    <%--<asp:BoundField DataField="ReceiptTextIntReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptTextIntReceiptNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" SortExpression="ReceiptTextIntReceiptNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--8--%>  <%--7--%>
                                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                        HtmlEncode="false" Visible="False">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--9--%>  <%--8--%>
                                                    <%-- <asp:BoundField DataField="OriginalReceiptDateFormatted" HeaderText="Original Receipt Date"
                                                        SortExpression="OriginalReceiptDateFormatted">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <%--10--%> <%--9--%>  <%--8--%>
                                                    <%--<asp:BoundField DataField="OriginalReceiptTextNo" HeaderText="Original Receipt No."
                                                        HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="OriginalReceiptInfo" HeaderText="Original Receipt Info"
                                                        HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField> 

                                                    <%--11--%><%--9--%>
                                                    <%-- <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <%--12--%> <%--10--%> <%--9--%>
                                                    <%--<asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date" SortExpression="ReleaseNoteDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="ReleaseNoteNoInfo" HeaderText="R.N. Info" SortExpression="ReleaseNoteDateFormatted"  HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField> 
                                                    <%--13--%> <%--10--%>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--14--%> <%--11--%>
                                                    <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--15--%> <%--12--%>
                                                    <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location" SortExpression="ReceiptItemBinLocation">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--16--%> <%--13--%>
                                                    <asp:BoundField DataField="CalibrationDueDateFormatted" HeaderText="Next Cal. Due Date">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--17--%> <%--14--%>
                                                    <asp:BoundField DataField="LandingRate" HeaderText="Landing Rate(Inv. Currency)">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                    </asp:BoundField>
                                                    <%--18--%> <%--15--%>
                                                    <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <%--19--%> <%--16--%>
                                                    <%-- <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date" SortExpression="ExpiryDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <%--20--%> <%--16--%>
                                                     <%-- <asp:BoundField DataField="ExpiryQtrs" HeaderText="Expiry Qtrs." SortExpression="ExpiryQtrs">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                     <asp:BoundField DataField="ExpiryInfo" HeaderText="Expiry  Info" SortExpression="ExpiryQtrs" HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--21--%> <%--17--%>
                                                    <%--<asp:BoundField DataField="VendorInvoiceDateFormatted" HeaderText="Supp. Inv. Date"
                                                        SortExpression="VendorInvoiceDateFormatted">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <%--22--%> <%--17--%>
                                                   <%-- <asp:BoundField DataField="VendorInvoiceNo" HeaderText="Supp. Inv. No." SortExpression="VendorInvoiceNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                   <asp:BoundField DataField="VendorInvoiceInfo" HeaderText="Supp. Inv. Info" SortExpression="VendorInvoiceNo" HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--23--%>  <%--18--%>
                                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--24--%> <%--19--%>
                                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO" Text="" Visible='<%#  Eval("ReceiptItemIsAttachmentAdded")%>'
                                                                Width="20px" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                    </asp:TemplateField>
                                                    <%--25--%> <%--20--%>
                                                    <asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                    <%--26--%> <%--21--%>
                                                    <asp:BoundField DataField="ReceiptItemIsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        HeaderText="ReceiptItemIsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--27--%> <%--22--%>
                                                    <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--28--%> <%--23--%>
                                                    <asp:BoundField DataField="CountOfComponentReservationItem" HeaderText="CountOfComponentReservationItem" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                     <%--29--%> <%--24--%>
                                                    <asp:BoundField DataField="EnabledDisabled" HeaderText="EnabledDisabled"  HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlAlternateStockList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult2" runat="server" CssClass="clsLabelHeader">Item Stock List Record(s) Found</asp:Label>
                                            <asp:GridView ID="dgAlternateStockList" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True">
                                                 <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    
                                                     
                                                    <asp:BoundField HeaderText="Color">
                                                      
                                                        <ItemStyle CssClass="clsColorLabel" width="3px" Height="3px" HorizontalAlign="Center"/>
                                                    </asp:BoundField>   <%--0--%>
                                                   
                                                    <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>    <%--1--%>
                                                    
                                                    <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDescription">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>   <%--2--%>
                                                  
                                                    <asp:BoundField DataField="AvailableQuantity" HeaderText="Available Qty." SortExpression="AvailableQuantity">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>    <%--3--%>
                                                    
                                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>  <%--4--%>
                                                   
                                                    <%--<asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>--%>   <%--5--%>
                                                                    
                                                    <%-- <asp:BoundField DataField="ReceiptTextIntReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptTextIntReceiptNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>--%>    
                                                    <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" SortExpression="ReceiptTextIntReceiptNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>  <%--6--%>  <%--5--%>
                                                    
                                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                        HtmlEncode="false" Visible="False">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>   <%--7--%> <%--6--%> 
                                                    
                                                    <%-- <asp:BoundField DataField="OriginalReceiptDateFormatted" HeaderText="Original Receipt Date"
                                                        SortExpression="OriginalReceiptDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>  <%--8--%>  <%--7--%>
                                                   
                                                    <%-- <asp:BoundField DataField="OriginalReceiptTextNo" HeaderText="Original Receipt No."
                                                        HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>  
                                                    <asp:BoundField DataField="OriginalReceiptInfo" HeaderText="Original Receipt Info"
                                                        HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>   <%--9--%>  <%--8--%>  <%--7--%>    
                                                    
                                                    <%-- <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>  <%--10--%>  <%--8--%>
                                                    
                                                    <%--  <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date" SortExpression="ReleaseNoteDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>   
                                                    <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="R.N. Info" SortExpression="ReleaseNoteDateFormatted" HtmlEncode ="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField> <%--11--%> <%--8--%>
                                                    
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>  <%--12--%>  <%--9--%>
                                                     
                                                    <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField> <%--13--%>  <%--10--%>
                                                    
                                                    <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location" SortExpression="ReceiptItemBinLocation">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>  <%--14--%> <%--11--%>
                                                      
                                                    <asp:BoundField DataField="CalibrationDueDateFormatted" HeaderText="Next Cal. Due Date">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField> <%--15--%> <%--12--%>
                                                    
                                                    <asp:BoundField DataField="LandingRate" HeaderText="Landing Rate(Inv. Currency)">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                    </asp:BoundField>  <%--16--%>  <%--13--%>
                                                    
                                                    <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                        <HeaderStyle   HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>  <%--17--%>  <%--14--%>
                                                    
                                                    <%--<asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date" SortExpression="ExpiryDateFormatted">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>  <%--18--%> <%--15--%>
                                                   
                                                    <%-- <asp:BoundField DataField="ExpiryQtrs" HeaderText="Expiry Qtrs." SortExpression="ExpiryQtrs">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>    
                                                    <asp:BoundField DataField="ExpiryInfo" HeaderText="Expiry Info" SortExpression="ExpiryQtrs" HtmlEncode ="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField> <%--19--%> <%--15--%>
                                                    
                                                    <%--<asp:BoundField DataField="VendorInvoiceDateFormatted" HeaderText="Supp. Inv. Date"
                                                        SortExpression="VendorInvoiceDateFormatted">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>  <%--20--%> <%--16--%>
                                                    
                                                    <%-- <asp:BoundField DataField="VendorInvoiceNo" HeaderText="Supp. Inv. No." SortExpression="VendorInvoiceNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="VendorInvoiceInfo" HeaderText="Supp. Inv. Info" SortExpression="VendorInvoiceNo" HtmlEncode ="false">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>  <%--21--%>  <%--16--%>
                                                    
                                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo">
                                                        <HeaderStyle   HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>  <%--22--%>  <%--17--%>
                                                    
                                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO" Text="" Visible='<%#  Eval("ReceiptItemIsAttachmentAdded")%>'
                                                                Width="20px" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                    </asp:TemplateField>  <%--23--%>  <%--18--%>
                                                     
                                                    <asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField> <%--24--%> <%--19--%>
                                                   
                                                    <asp:BoundField DataField="ReceiptItemIsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        HeaderText="ReceiptItemIsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>   <%--25--%>   <%--20--%>
                                                     
                                                    <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                        ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>  <%--26--%> <%--21--%>
                                                     
                                                    <asp:BoundField DataField="CountOfComponentReservationItem" HeaderText="CountOfComponentReservationItem" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField> <%--27--%> <%--22--%>
                                                </Columns>
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlColor" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblIndicate" runat="server" CssClass="clsLabelHeader">* : 
                                        Indicates Item is Removed as Returnable from Aircraft .</asp:Label>
                                            <asp:Label ID="lblGreen" runat="server" CssClass="clsLabelauto" BackColor="#9ae6ac"
                                                ForeColor="#9ae6ac" Visible="false">Green</asp:Label>
                                            <asp:Label ID="lblGreenInfo" runat="server" CssClass="clsLabelHeader" Visible="false">Green row indicates reserved component</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                Text="Back"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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
    <div id="DueAtMessage" class="clsInfoMessage" style="display: none" runat="server">
        <p>
            <u>Note:</u>
            <br />
            Enter Part No./Description and click on Find Now button to get Part Stock list.</p>
    </div>
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var isAsync = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (!isAsync) {
                if ("<%= page.IsPostback%>" == "False" && "<%=  Not mIssue Is Nothing  %>" == "True") {
                    $pos = $("#<%=txtSearch.ClientID%>").position();
                    var top = $pos.top;
                    var left = $pos.left;
                    var searchHeight = $("#<%=txtSearch.ClientID%>").height();
                    var margin = top + searchHeight;

                    var height = $("#tblMain").outerHeight();
                    var h = margin - height;
                    $("#DueAtMessage").css("display", "block");
                    $("#DueAtMessage").animate({ marginTop: h, marginLeft: left - 5 }, 300, 'swing', function () {
                        $("#DueAtMessage").delay(5000).fadeOut();
                    });
                }
            }
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".clsGridNewStyle").find("td").each(function () {

                if ($(this).text() == "*") {
                    $(this).css("color", "Red");
                    $("td", $(this).closest("tr")).addClass("activerow");
                }
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $(".clsGridNewStyle").find("td").each(function () {

                if ($(this).text() == "*") {
                    $(this).css("color", "Red");
                    $("td", $(this).closest("tr")).addClass("activerow");
                }
            });
        });
    </script>
</body>
</html>

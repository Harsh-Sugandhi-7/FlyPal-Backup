<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartStockStatusByReceipt_Ajax.aspx.vb"
    Inherits="Flypal.wfPartStockStatusByReceipt_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Part Stock Status List By Receipt</title>
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
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <span id="lblPartStockStatusList" class="clsFormHeader">Part Stock Status List By Receipt</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                            ValidateEmptyText="true" OnServerValidate="CustomValidate" ValidationGroup="a"
                                            Display="None" ControlToValidate="txtText"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="dgStockItemList" />
                                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
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
                                                                <span id="lblPartNo" class="clsLabelAuto">Receipt Text</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" onfocus="SetContextKey();"
                                                                    ToolTip="Enter Receipt Text" MaxLength="25" Width="208px"> </asp:TextBox>
                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                    CompletionInterval="1" ServicePath="wfPartStockStatusByReceipt_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                    TargetControlID="txtText" UseContextKey="False" CompletionListCssClass="ac_results_Main"
                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                </cc2:AutoCompleteExtender>
                                                                <script type="text/javascript">
                                                                    function SetContextKey() {
                                                                        var autoComplete = $find('txtText_Autocomplete');
                                                                        var TransTypeID = 'TransTypeID=<%=mIssue.TransTypeID%>¿QuotationDate=<%=mIssue.IDate%>';
                                                                        autoComplete.set_contextKey(TransTypeID);
                                                                    }
                                                                </script>
                                                            </td>
                                                            <td>
                                                                <span id="lblNo" class="clsLabelAuto">No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="8"
                                                                    ToolTip="Enter Receipt No."> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find the Receipt"
                                                        Text="Find Now"></asp:Button>--%>
                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                        ToolTip="Click to Find the Receipt" />
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
                                        <asp:GridView ID="dgStockItemList" runat="server" CssClass="clsGridNewStyle" CellPadding="5"
                                            GridLines="Horizontal" AutoGenerateColumns="False" DataKeyNames="ReceiptID" ShowHeaderWhenEmpty="true"
                                            AllowPaging="true" AllowSorting="True">
                                            <%--<PagerSettings Mode="NextPreviousFirstLast" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" />
                                            <AlternatingRowStyle CssClass="alt" />--%>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="RecdDateFormatted" HeaderText="Date">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RcptTextNo" HeaderText="Receipt" SortExpression="RcptTextNo">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptType" SortExpression="ReceiptType" HeaderText="From">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CostCenter" SortExpression="CostCenter" HeaderText="Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
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
                                <asp:UpdatePanel runat="server" ID="upnlPendingItemList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">Item Stock List Record(s) Found</asp:Label>
                                        <asp:GridView ID="dgPendingItemList" runat="server" CssClass="clsGridNewStyle" CellPadding="5"
                                            GridLines="Horizontal" AutoGenerateColumns="False" DataKeyNames="ReceiptItemID,ItemName"
                                            ShowHeaderWhenEmpty="true" AllowSorting="True" OnRowDataBound="OnRowDataBoundPendingItemList">
                                            <%--<PagerSettings Mode="NextPreviousFirstLast" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" />
                                            <AlternatingRowStyle CssClass="alt" />--%>
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
                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="cbSelectRow" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--0--%>
                                                <asp:BoundField DataField="ShowStatusForRemovedAsReturnableFromAircraft">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" Font-Bold="true" />
                                                </asp:BoundField>
                                                <%--1--%>
                                                <asp:BoundField HeaderText="Color">
                                                    <ItemStyle CssClass="clsColorLabel" Width="3px" Height="3px" />
                                                </asp:BoundField>
                                                <%--2--%>
                                                <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <%--3--%>
                                                <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDescription">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <%--4--%>
                                                <asp:BoundField DataField="AvailableQuantity" HeaderText="Available Qty." SortExpression="AvailableQuantity">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <%--5--%>
                                                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <%--6--%>
                                                <%-- <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>--%>
                                                <%--7--%>
                                                <%-- <asp:BoundField DataField="ReceiptTextIntReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptTextIntReceiptNo"
                                                    HtmlEncode="false">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>--%>
                                                <%--8--%>
                                                <%--7--%>
                                                <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" SortExpression="ReceiptTextIntReceiptNo"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                    HtmlEncode="false" Visible="False">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <%--9--%>
                                                <%--8--%>
                                                <%--<asp:BoundField DataField="OriginalReceiptDateFormatted" HeaderText="Original Receipt Date"
                                                    SortExpression="OriginalReceiptDateFormatted">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--10--%>
                                                <%--9--%>
                                                <%-- <asp:BoundField DataField="OriginalReceiptTextNo" HeaderText="Original Receipt No."
                                                    HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--11--%>
                                                <%--9--%>
                                                <asp:BoundField DataField="OriginalReceiptInfo" HeaderText="Original Receipt Info"
                                                    HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--12--%>
                                                <%--10--%>
                                                <%--<asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date" SortExpression="ReleaseNoteDateFormatted">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--13--%>
                                                <%--10--%>
                                                <asp:BoundField DataField="ReleaseNoteNoInfo" HeaderText="R.N. Info" SortExpression="ReleaseNoteDateFormatted"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--14--%>
                                                <%--11--%>
                                                <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--15--%>
                                                <%--12--%>
                                                <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location" SortExpression="ReceiptItemBinLocation">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--16--%>
                                                <%--13--%>
                                                <asp:BoundField DataField="LandingRate" HeaderText="Landing Rate(Inv. Currency)">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                </asp:BoundField>
                                                <%--17--%>
                                                <%--14--%>
                                                <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <%--18--%>
                                                <%--15--%>
                                                <%-- <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date" SortExpression="ExpiryDateFormatted">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--19--%>
                                                <%--16--%>
                                                <%-- <asp:BoundField DataField="ExpiryQtrs" HeaderText="Expiry Qtrs." SortExpression="ExpiryQtrs">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--20--%>
                                                <%--16--%>
                                                <asp:BoundField DataField="ExpiryInfo" HeaderText="Expiry Info" SortExpression="ExpiryQtrs"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="VendorInvoiceDateFormatted" HeaderText="Supp. Inv. Date"
                                                    SortExpression="VendorInvoiceDateFormatted">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--21--%>
                                                <%--17--%>
                                                <%-- <asp:BoundField DataField="VendorInvoiceNo" HeaderText="Supp. Inv. No." SortExpression="VendorInvoiceNo">
                                                    <HeaderStyle   HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <%--22--%>
                                                <%--17--%>
                                                <asp:BoundField DataField="VendorInvoiceInfo" HeaderText="Supp. Inv. Info" SortExpression="VendorInvoiceNo"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--23--%>
                                                <%--18--%>
                                                <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO" Text="" Visible='<%#  Eval("ReceiptItemIsAttachmentAdded")%>'
                                                            Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>
                                                <%--24--%>
                                                <%--19--%>
                                                <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--25--%>
                                                <%--20--%>
                                            </Columns>
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                        </asp:GridView>
                                        <asp:Label ID="lblIndicate" runat="server" CssClass="clsLabelHeader">* : Indicates Item is Removed as Returnable from Aircraft .</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlAlternateStockList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult2" runat="server" CssClass="clsLabelHeader" Visible="false">Item Stock List Record(s) Found</asp:Label>
                                        <asp:GridView ID="dgAlternateStockList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                            Visible="false" ShowHeaderWhenEmpty="true" AllowSorting="True">
                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAllAlt" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectAlt" runat="server" CssClass="cbSelectRow" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDescription">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AvailableQuantity" HeaderText="Available Qty." SortExpression="AvailableQuantity">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptTextIntReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptTextIntReceiptNo"
                                                    HtmlEncode="false">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                    HtmlEncode="false" Visible="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OriginalReceiptDateFormatted" HeaderText="Original Receipt Date"
                                                    SortExpression="OriginalReceiptDateFormatted">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OriginalReceiptTextNo" HeaderText="Original Receipt No."
                                                    HtmlEncode="false" SortExpression="OriginalReceiptTextNo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date" SortExpression="ReleaseNoteDateFormatted">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location" SortExpression="ReceiptItemBinLocation">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LandingRate" HeaderText="Landing Rate(Inv. Currency)">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date" SortExpression="ExpiryDateFormatted">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ExpiryQtrs" HeaderText="Expiry Qtrs." SortExpression="ExpiryQtrs">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorInvoiceDateFormatted" HeaderText="Supp. Inv. Date"
                                                    SortExpression="VendorInvoiceDateFormatted">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorInvoiceNo" HeaderText="Supp. Inv. No." SortExpression="VendorInvoiceNo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
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
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add Issue Items"
                                            ValidationGroup="a" CausesValidation="true" Text="Add"></asp:Button>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                            CausesValidation="false" Text="Back"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
            Enter Receipt Text and No. then click on Find Now button to get pending receipt
            List.</p>
    </div>
    <!--ReceiptCumInvoiceAttach Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
        PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAttachStateComplete() {
            $("#btnDummyAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForAttach() {
            var Attachwindow = $find("<%=mdlAttach.ClientID %>");
            //close popup window
            Attachwindow.hide();
            //release resources
            $("#IframeAttach").attr("src", "JavaScript:''");
            //call button click
            //  $("#hdnBtnAttach").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var isAsync = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (!isAsync) {
                if ("<%= page.IsPostback%>" == "False" && "<%=  Not mIssue Is Nothing  %>" == "True") {
                    $pos = $("#<%=txtText.ClientID%>").position();
                    var top = $pos.top;
                    var left = $pos.left;
                    var searchHeight = $("#<%=txtText.ClientID%>").height();
                    var margin = top + searchHeight;

                    var height = $("#tblMain").outerHeight();
                    var h = margin - height;
                    $("#DueAtMessage").css("display", "block");
                    $("#DueAtMessage").animate({ marginTop: h, marginLeft: left - 5 }, 300, 'swing', function () {
                        $("#DueAtMessage").delay(3500).fadeOut();
                    });
                }
            }
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgPendingItemList tr:gt(0)").find(":checkbox").each(function () {
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

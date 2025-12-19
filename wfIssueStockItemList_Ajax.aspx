<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueStockItemList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfIssueStockItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Stock Item List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form2" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblPartStockStatus" class="clsFormHeader">Stock Item List</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                Text="Back"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblNote1" class="clsLabelHeader">Following is the list of Part's Stock available
                                    in different Store with available quantity. </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="50" ToolTip="Enter Part No."
                                                        CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.ItemName %>">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblDesc" class="clsLabel">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" ToolTip="Enter Part No."
                                                        CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.ItemDesc %>">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="50" ToolTip="Enter Part No."
                                                        CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.SerialNo %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:GridView ID="dgIssueStockItemList" runat="server" CssClass="clsGridNewStyle"
                                                        CellPadding="5" GridLines="Horizontal" PageSize="25" ShowHeaderWhenEmpty="true"
                                                        AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="StarMark"></asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Rec Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptTextIntReceiptNo" SortExpression="ReceiptTextIntReceiptNo"
                                                                HeaderText="Rec. No." HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R.Note no.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AvailableQuantity" SortExpression="AvailableQuantity"
                                                                HeaderText="Stock Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EROQty" SortExpression="EROQty" HeaderText="ERO Qty">
                                                                <HeaderStyle></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                        UseSubmitBehavior="False" CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO"
                                                                        Text="" Visible='<%#  Eval("ReceiptItemIsAttachmentAdded")%>' Width="20px" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                            </asp:TemplateField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectPart">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
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
                                <span id="Label1" class="clsLabelHeader">* : Part is mentioned in the Order</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <%-- <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                            Text="Back"></asp:Button>--%>
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
    </form>
</body>
</html>

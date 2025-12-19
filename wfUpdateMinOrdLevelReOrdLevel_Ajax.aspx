<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateMinOrdLevelReOrdLevel_Ajax.aspx.vb" Inherits="Flypal.wfUpdateMinOrdLevelReOrdLevel_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Min. Order Level Re. Order Level</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet">
    
      <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="tblInner">
                                    <tr>
                                        <td colspan ="3">
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Update Min. Stock Level and Re-Order Level Screen</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Financial Year</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAs" runat="server" CssClass="clsLabelAuto">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbFromYear" runat="server" CssClass="clsComboBox_Ajax">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTo" runat="server" CssClass="clsLabelAuto">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbToYear" runat="server" CssClass="clsComboBox_Ajax">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnFindNow" TabIndex="0" runat="server" 
                                                            CssClass="clsButton_Ajax" Text="Find Now">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <table id="Table3" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnUpdateTop" TabIndex="0" runat="server" 
                                                            CssClass="clsButton_Ajax" Text="Update"
                                                            ToolTip="Click to update Min. Stock Level and Re-Order Level"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTop" TabIndex="0" runat="server" 
                                                            CssClass="clsButton_Ajax" Text="Close"
                                                            ToolTip="Click to close screen" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                        <%--    <asp:DataGrid ID="dgItems" runat="server" CssClass="clsGrid" AllowPaging="True" AutoGenerateColumns="False"
                                                PageSize="100" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                                <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="PartNumber" SortExpression="PartNumber" HeaderText="Part No.">
                                                        <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ConsumedQty" SortExpression="ConsumedQty" HeaderText="Consumed  Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White" Width="100px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="OldMinimumStockQty" SortExpression="OldMinimumStockQty"
                                                        HeaderText="Old Min. Stock  Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NewMinimumStockQty" SortExpression="NewMinimumStockQty"
                                                        HeaderText="New Min. Stock Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="OldReorderQty" SortExpression="OldReorderQty" HeaderText="Old Reorder  Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NewReorderQty" SortExpression="NewReorderQty" HeaderText="New Reorder Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                            </asp:DataGrid>--%>

                                              <asp:GridView ID="dgItems" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="clsGrid" PageSize="25" ShowHeaderWhenEmpty="true">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="" PreviousPageText="" />
                                               <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="PartNumber" HeaderText="Part No." SortExpression="PartNumber">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConsumedQty" HeaderText="Consumed  Qty." SortExpression="ConsumedQty">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OldMinimumStockQty" HeaderText="Old Min. Stock  Qty." SortExpression="OldMinimumStockQty">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="NewMinimumStockQty" HeaderText="New Min. Stock Qty." SortExpression="NewMinimumStockQty">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OldReorderQty" HeaderText="Old Reorder  Qty." SortExpression="OldReorderQty">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NewReorderQty" HeaderText="New Reorder Qty." SortExpression="NewReorderQty">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                    </asp:BoundField>

                                                    
                                                </Columns>
                                            </asp:GridView>



                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnUpdate" TabIndex="0" runat="server" 
                                                                CssClass="clsButton_Ajax" Text="Update"
                                                                ToolTip="Click to update Min. Stock Level and Re-Order Level"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                ToolTip="Click to close screen" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>

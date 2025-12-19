<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartListForSalesOrder.aspx.vb" Inherits="Flypal.wfPartListForSalesOrder" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Part Stock Status List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
    <!-- #include file= "LocalFunctionAjax.htm" -->
</asp:PlaceHolder>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="Form1" method="post" runat="server">
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td colspan="5" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPartStockStatusList" runat="server" CssClass="clsFormHeader">Part List</asp:Label>
                                            </td>
                                            <td align="right" colspan="5">
                                                <table class="clstableButton" align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                Text="Back"></asp:Button></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Part No.</asp:Label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Search Criteria"
                                        Height="25px" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox></td>
                                <td align="right">
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to Find" Text="Find Now"></asp:Button>--%>

                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                    ToolTip="Click to find" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Part Stock Status List : No.of Record Found(s).</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:DataGrid ID="dgPartStockStatusList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" OnPageIndexChanged="NewPage"
                                        AllowPaging="True" AutoGenerateColumns="False" PageSize="25" AllowSorting="True">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Part Description">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="StockQty" SortExpression="StockQty" HeaderText="Stock Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PendingQty" SortExpression="PendingQty" HeaderText="Pending Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReturnableQty" SortExpression="ReturnableQty" HeaderText="Returnable Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                        </Columns>
                                        <%--<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>--%>
                                        <%--<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />--%>
                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" Mode="NumericPages" />

                                    </asp:DataGrid></td>
                                <%--<TR>
									<TD align="right" colSpan="5">
										<TABLE class="clstableButton" align="right">
											<TR>
												<TD>
													<asp:button id="btnBack" runat="server" Cssclass="clsButton" ToolTip="Click to go back to the previous page"
														Text="Back"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>--%>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlMessageBox" Style="z-index: 111" runat="server"></asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

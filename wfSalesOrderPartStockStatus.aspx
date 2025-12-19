<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesOrderPartStockStatus.aspx.vb"
    Inherits="Flypal.wfSalesOrderPartStockStatus" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Part Stock Status List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); /

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
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblPartStockStatus" runat="server" CssClass="clstitle1">List of Parts and it's Pending Quotations</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" colspan="4">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox" ToolTip="Enter Part No."
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="middle" align="right">
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to Find "
                                                Text="Find Now"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                            </td>
                            <tr>
                                <td colspan="5">
                                    <asp:DataGrid ID="dgItemList" runat="server" CssClass="clsGrid" OnPageIndexChanged="NewPage"
                                        AllowPaging="True" AutoGenerateColumns="False" PageSize="15">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemName" HeaderText="Part No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemDescription" HeaderText="Description"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="StockQty" HeaderText="Stock Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PendingQty" HeaderText="Pending Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReturnableQty" HeaderText="Returnable Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn Text="Select" HeaderText="Detail" CommandName="Select"></asp:ButtonColumn>
                                            <asp:ButtonColumn Text="Select Part" HeaderText="Select Part" CommandName="SelectPart">
                                            </asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblPendingItemInfo" runat="server" CssClass="clsLabelHeader">Details Of Pending Items</asp:Label>
                                    </td>
                                </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">Pending Item Detail List Record(s)  found</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:DataGrid ID="dgPendingItemList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="3">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="QuotationItemID" HeaderText="QuotationItemID">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="QuotationID" HeaderText="QuotationID">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="QuotationDateFormatted" HeaderText="Date"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="QuotationTextNo" HeaderText="No."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="QuotationQty" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <table class="clstableButton" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
                                                Text="Back"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlMessageBox" Style="z-index: 111" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingToIssueItemList.aspx.vb"
    Inherits="Flypal.wfPendingToIssueItemList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Part Stock Status List</title>
    <meta name="vs_showGrid" content="False">
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
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                          
                            <td colspan="2" class="clsFormHeader1Newstyle">
                            <table  Width="100%">
                            <tr>
                            <td>
                             <asp:Label ID="lblPartStockStatusList" runat="server" CssClass="clsFormHeader">Part Stock Status List</asp:Label>
                            </td>
                            <td align="right">
                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page">
                                    </asp:Button>
                                </td>
                            </tr>
                            </table>
                             
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" ReadOnly="True" MaxLength="8" BackColor="#E0E0E0"
                                                CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <table id="Table2">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now" ToolTip="Click to Find">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Total Part Stock Status List : No.of Record(s) Found.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataGrid ID="dgItemList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                    AutoGenerateColumns="False" PageSize="20" AllowSorting="True" CellPadding="5" GridLines="Horizontal">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False" ></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Part Description">
                                            <HeaderStyle  ></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="TotalQuantity" SortExpression="TotalQuantity" HeaderText="Total Stock ">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="#FFFFFF" Width="100px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select">
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblResult2" runat="server" CssClass="clsLabelHeader">Part Stock List : Record(s) Found</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="100%" colspan="2">
                                    <asp:DataGrid ID="dgPendingList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                        PageSize="5" AllowSorting="True" Width="100%" CellPadding="5" GridLines="Horizontal">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                        <Columns>
                                            <asp:BoundColumn DataField="ReceiptText" SortExpression="ReceiptText" HeaderText="Receipt Text">
                                                <HeaderStyle  ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                <HeaderStyle Wrap="False"  ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                <HeaderStyle Wrap="False"  ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Part Desc.">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="AvailableQuantity" SortExpression="AvailableQuantity"
                                                HeaderText="Available Qty.">
                                                <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Category" SortExpression="Category" HeaderText="Category">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Nomenclature" SortExpression="Nomenclature" HeaderText="Nomenclature">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R.N. No.">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ExpiryQtrs" SortExpression="ExpiryQtrs" HeaderText="Expiry Qtrs.">
                                                <HeaderStyle  ></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderStyle-Font-Bold="true" HeaderText="View" >
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" 
                                                        CommandName="ViewRec" Height="20px" ImageUrl="icons/CLIP01.ICO" Text="" Visible='<%#  Eval("ReceiptItemIsAttachmentAdded")%>'
                                                        Width="20px" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right" colspan="2">
                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Back" ToolTip="Click to go back to the previous page">
                                    </asp:Button>
                                </td>--%>
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

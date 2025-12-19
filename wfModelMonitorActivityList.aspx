<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModelMonitorActivityList.aspx.vb" Inherits="Flypal.wfModelMonitorActivityList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Model Activity List</title>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->

    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <form id="Form1" method="post" runat="server">
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin" border="0" align="right">
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Model Activity List</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label></td>
                                <td colspan="2" align="right">
                                    <table id="Table1" border="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton" Text="Add" CausesValidation="False"
                                                    ToolTip="Click to add selected Model Activity"></asp:Button></td>
                                            <td>
                                                <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton" Text="Back" CausesValidation="False"
                                                    ToolTip="Click to exit Model Activity"></asp:Button></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:DataGrid ID="dgMonitorActivityList" runat="server" CssClass="clsGrid" ToolTip="Link Maintenance List"
                                        AutoGenerateColumns="False" DESIGNTIMEDRAGDROP="139" AllowSorting="True">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Select">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Reference Doc.">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Number" SortExpression="Number" HeaderText="Directive Number">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Show In C of A" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="RequiredManHours" HeaderText="Estd. Man Hours" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Note" HeaderText="Note"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="FrequencyValue" HeaderText="Threshold"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="Size" HeaderText="Size"></asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid></td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <table border="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsButton" Text="Add" CausesValidation="False"
                                                    ToolTip="Click to add selected Model Activity"></asp:Button></td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Back" CausesValidation="False"
                                                    ToolTip="Click to exit Model Activity"></asp:Button></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

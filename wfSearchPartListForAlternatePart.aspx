<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchPartListForAlternatePart.aspx.vb"
    Inherits="Flypal.wfSearchPartListForAlternatePart" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Part List</title>
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
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblPartList" runat="server" CssClass="clstitle1">Part List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox" ToolTip="Enter Part No."
                                    MaxLength="50"></asp:TextBox>
                            </td>
                            <td colspan="3" align="right">
                                <table id="Table2">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to find the list of Part as per searching criteria"
                                                Text="Find Now"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to close Part List screen"
                                                Text="Close" Visible="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts : 100 Record(s) found.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:DataGrid ID="dgPartList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                    AutoGenerateColumns="False" PageSize="30" AllowPaging="True">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                </asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Part No">
                                            <HeaderStyle Wrap="False" ForeColor="#FFFFFF"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="AlternatePartPresent" SortExpression="AlternatePartPresent"
                                            HeaderText="Alternate Part Present">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td colspan="5" align="right">
                                    <table class="clstableButton" align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOk" runat="server" CssClass="clsButton" ToolTip="Click to Add the Part In Alternate Part"
                                                    Text="Ok"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close Part List screen"
                                                    Text="Close"></asp:Button>
                                            </td>
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

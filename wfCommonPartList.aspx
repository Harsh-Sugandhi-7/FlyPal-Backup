<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCommonPartList.aspx.vb"
    Inherits="Flypal.wfCommonPartList" %>

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
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPartList" runat="server" CssClass="clstitle1">Part List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table class="clsTable1" id="Table1">
                                    <tr>
                                        <td>
                                            <table id="Table2" width="300">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmblookin" runat="server" CssClass="clsComboBox" AutoPostBack="True">
                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Part No.</asp:ListItem>
                                                            <asp:ListItem Value="2">Description</asp:ListItem>
                                                            <asp:ListItem Value="3">Nomenclature</asp:ListItem>
                                                            <asp:ListItem Value="4">Category</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                        <asp:Label ID="lblFor" runat="server" CssClass="clsLabelMedium" Visible="False">For</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox" Visible="False"
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to find list of Part as per searching criteria"
                                                Text="Find Now"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts : 100 Record(s) found.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataGrid ID="dgPartList" runat="server" CssClass="clsGrid" AllowPaging="True"
                                    AutoGenerateColumns="False" PageSize="20" AllowSorting="True">
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
                                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False" ForeColor="#FFFFFF"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="NomenclatureName" SortExpression="NomenclatureName" HeaderText="Nomenclature">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td align="right" colspan="2">
                                    <table class="clstableButton" align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOk" runat="server" CssClass="clsButton" ToolTip="Click to add the selected Item"
                                                    Text="Ok"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous Page"
                                                    Text="Back"></asp:Button>
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

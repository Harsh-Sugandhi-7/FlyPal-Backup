<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPrimaryCategoryList.aspx.vb" Inherits="Flypal.wfPrimaryCategoryList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html >
<head id="Head1" runat="server">
    <title>Part Type Status List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" src="DATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblPartsList" runat="server" CssClass="clstitle1">Primary Category Selection</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                </td>
                            </tr>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Label ID="lblHeader" runat="server" CssClass="clsLabelHeader" Width="550px">Please select primary category for each category.  Then click on update button.</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Category List</asp:Label>
            </td>
            <td align="right">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" CssClass="clsButton" Text="Update" 
                                ToolTip="Click to Update">
                            </asp:Button>
                        </td>
                                            </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:DataGrid ID="dgCategoryList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                    PageSize="25">
                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Category">
                            <HeaderStyle ForeColor="White"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="GLCode" HeaderText="GL Code"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Primary Category">
                            <ItemTemplate>
                                <asp:DropDownList ID="cmbPrimaryCategoryList" runat="server" 
                                    CssClass="clsCombobox1" OnSelectedIndexChanged="cmbPrimaryCategoryList_SelectedIndexChanged"
                                    AutoPostBack="True" DataTextField="Name" DataValueField="ID"
                                    DataSource="<%# mPrimaryCategoryList %>" 
                                    SelectedValue='<%# DataBinder.Eval(Container.DataItem,"PrimaryCategoryID") %>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td align="right">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpdateBottom" runat="server" CssClass="clsButton" Text="Update"
                                ToolTip="Click to Update"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:panel></TD></TR></TBODY></TABLE></form>
</body>
</html>


<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearch.aspx.vb" Inherits="Flypal.wfSearch" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Search</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
</head>
<body ms_positioning="GridLayout" topmargin="5" bottommargin="5" leftmargin="5" rightmargin="5">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Search</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Search</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCode" runat="server" CssClass="clsLabelAuto" Visible="False">Short Name</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxMedium" Visible="False"
                                    ToolTip="Enter Code" MaxLength="5"></asp:TextBox>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox" ToolTip="Enter Name"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataGrid ID="dgList" runat="server" CssClass="clsGrid" AllowSorting="True" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="GId" SortExpression="GId" HeaderText="GId">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ShortName" SortExpression="ShortName" HeaderText="Short Name">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CityName" SortExpression="CityName" HeaderText="City">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="DesignationName" SortExpression="DesignationName" HeaderText="Designation">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                            <td align="right" colspan="1">
                                <table id="Table1" height="100%" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td valign="top" align="right">
                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False">
                                            </asp:Button>
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTerm.aspx.vb" EnableViewState="false"
    Inherits="Flypal.wfTerm" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Term</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <LINK href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1" Width="536px">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblTitle" CssClass="clsTitle1" runat="server">Term [New]</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                                <asp:CustomValidator ID="cvName" runat="server" ErrorMessage="Term text should not be greater than 255 Character."
                                    ControlToValidate="txtName" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required ."
                                    ControlToValidate="txtname" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelauto">Click To Add New Record</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add the new Term"
                                    Text="New" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name </asp:Label>
                            </td>
                            <td style="width: 382px; height: 29px" colspan="2">
                                <asp:TextBox ID="txtName" runat="server" Width="382px" CssClass="clstextBox1" ToolTip="Enter Name"
                                    Text="<%# mTerm.Terms %>" MaxLength="500" TextMode="MultiLine" Height="39px">
                                </asp:TextBox>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelauto">Click To Save Current Record</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnSave" CssClass="clsButton" runat="server" ToolTip="Click to Save the Term Information"
                                    Text="Save"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Term List</asp:Label>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DataGrid ID="dgTerm" runat="server" CssClass="clsGrid" AllowSorting="True" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="TermID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Terms" SortExpression="Terms" HeaderText="Terms">
                                            <HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
                                        <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                            <td>
                                <table id="Table1" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                    <tr>
                                        <td valign="top" align="right">
                                            <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
                                                Text="Close" CausesValidation="False" Visible="<%# mTermList.Count>25 %>"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
                                                Text="Close" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

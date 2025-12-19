<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalaryHeads.aspx.vb"
    Inherits="Flypal.wfSalaryHeads" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Salary Head</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
</head>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblTitle" TabIndex="1" CssClass="clstitle1" runat="server">Salary Head Information [New]</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                    Display="None" ErrorMessage="Salary Head Required"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvDocument" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                    Display="None" ErrorMessage="Salary Head Name too Long." OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator  ID="rfvCode" runat="server" CssClass="clsLabelAuto"
                                    ControlToValidate="txtCode" Display="None" ErrorMessage="Salary Head Code Required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnNew" CssClass="clsButton" runat="server" CausesValidation="False"
                                    ToolTip="Click to Add the Salary Head" Text="New"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblDocumentDetails" runat="server" CssClass="clsLabelHeader">Salary Head Details</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="center">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label>
                            </td>
                            <td valign="middle">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Code</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxSmall" ToolTip="Enter Salary Head Code"
                                    Text="<%# mSalaryHeads.Code %>" MaxLength="5">
                                </asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="center">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label>
                            </td>
                            <td valign="middle">
                                <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox" ToolTip="Enter Salary Head Name"
                                    Text="<%# mSalaryHeads.Name %>" MaxLength="50">
                                </asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnSave" CssClass="clsButton" runat="server" ToolTip="Click to Save Salary Head Information"
                                    Text="Save"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Salary Heads List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgSalaryHeads" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Code" HeaderText="Code"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
                                        <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                            <td align="right">
                                <table id="Table1" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                    <tr>
                                        <td valign="top" align="right">
                                            <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton" CausesValidation="False"
                                                ToolTip="Click to close Salary Head Information screen" Text="Close"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
                                                ToolTip="Click to close Salary Head Information screen" Text="Close"></asp:Button>
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
    &nbsp;
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCopyUser.aspx.vb" Inherits="Flypal.wfCopyUser" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat="server">
    <title>User</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td colspan="3">
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">User</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvUN" runat="server" CssClass="clsLabelAuto" ErrorMessage="UserName Required"
                                    ControlToValidate="txtUserName" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvp" runat="server" CssClass="clsLabelAuto" ErrorMessage="Password is Required"
                                    ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvcp" runat="server" CssClass="clsLabelAuto" ErrorMessage="Confirm Password is Required"
                                    ControlToValidate="txtConPass" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvPassword" runat="server" CssClass="cslLabelAuto" ErrorMessage="Password Should be atleast 4 characters"
                                    ControlToValidate="txtPassword" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvcp" runat="server" CssClass="cslLabelAuto" ErrorMessage="Confirm Password should be same as the Password"
                                    ControlToValidate="txtConPass" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:Label>
                            </td>
                            <td colspan="1">
                                <asp:Button ID="btnNewUser" runat="server" CssClass="clsButton" ToolTip="Click to add new user"
                                    Text="New" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">User Information</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPasswordStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblUserName" runat="server" CssClass="clsLabel">User Name</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="clsTextBox" ToolTip="Enter User Name"
                                    Text="<%# mUser.Name %>" BackColor="White" MaxLength="50">
                                </asp:TextBox>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" CssClass="clsLabel">Password</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="clsTextBox" ToolTip="Enter Password"
                                    Text="<%# mUser.Password %>" BackColor="White" MaxLength="10">
                                </asp:TextBox>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblConfirmPassword" runat="server" CssClass="clsLabelStar">*</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblConfirm" runat="server" CssClass="clsLabelAuto">Confirm Password</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConPass" runat="server" CssClass="clsTextBox" ToolTip="Enter Confirm Password"
                                    Text="<%# mUser.ConfirmPassword %>" BackColor="White" MaxLength="10">
                                </asp:TextBox>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkLogon" runat="server" CssClass="clsCheckBox" Text="Change Password On Logon  &amp;nbsp;"
                                    TextAlign="Left" Checked="<%# mUser.ChangePassword %>" Visible="False"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblRoles" runat="server" CssClass="clsLabelHeader">Select Roles for this User</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lbllistrole" runat="server" CssClass="clsLabelHeader">List of User Roles </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <asp:Button ID="btnSaveTop" runat="server" CssClass="clsButton" ToolTip="Click to save the current record"
                                    Text="Save"></asp:Button>
                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to close User Information screen"
                                    Text="Close" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DataGrid ID="dgUser" runat="server" CssClass="clsGrid" ToolTip="Click to select Role for the User from User Roles List"
                                    AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="UserRoleID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                </asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="RoleName" HeaderText="Role Name"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSelectionAircraft" runat="server" CssClass="clsLabelHeader">Select Aircraft for this user</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAircraftList" runat="server" CssClass="clsLabelHeader">List of Aircraft</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DataGrid ID="dgMachine" runat="server" CssClass="clsGrid" ToolTip="Click to select Aircraft for the User from Aircraft List"
                                    AutoGenerateColumns="False">
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
                                        <asp:BoundColumn DataField="RegNo" HeaderText="Aircraft"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table id="tblNew">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSav" runat="server" CssClass="clsButton" ToolTip="Click to save the current record"
                                                Text="Save"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close User Information screen"
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
    </table>
    </form>
</body>
</html>

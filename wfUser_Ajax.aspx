<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUser_Ajax.aspx.vb" Inherits="Flypal.wfUser_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="DATEFUNCTIONS.js"></script>

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body>
    <form id="frmUser" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <asp:UpdatePanel ID="upnlUser" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 400px">
                                                            <asp:Label ID="lbltitle" runat="server" 
                                                                CssClass="clsFormHeader" Text="User" />
                                                        </td>

                                                        <td colspan="4" align="right">
                                                            <table id="tblButtons">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnNewUser" CssClass="clsbtnH clsinfoH"
                                                                            runat="server" CausesValidation="False"
                                                                            Text="New" ToolTip="Create New User." />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnSaveTop" CssClass="clsbtnH clsinfoH"
                                                                            runat="server" Text="Save"
                                                                            ToolTip="Save the Current Record." />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrint" CssClass="clsbtnH clsinfoH"
                                                                            runat="server" Text="Print" ToolTip="Print the Report." />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server"
                                                                            CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                            Text="Close" ToolTip="Close User Information screen." />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvUN" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="UserName Required" ControlToValidate="txtUserName" Display="None" />
                                                <asp:RequiredFieldValidator ID="rfvp" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Password is Required" ControlToValidate="txtPassword" Display="None" />
                                                <asp:RequiredFieldValidator ID="rfvcp" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Confirm Password is Required" ControlToValidate="txtConPass" Display="None" />
                                                <asp:CustomValidator ID="cvPassword" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Password Should be atleast 4 characters" ControlToValidate="txtPassword"
                                                    Display="None" OnServerValidate="Customvalidate" />
                                                <asp:CustomValidator ID="cvcp" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Confirm Password should be same as the Password"
                                                    ControlToValidate="txtConPass" Display="None" OnServerValidate="Customvalidate" />
                                                <asp:CustomValidator ID="cvExpPeriod" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Expiry Period must be between 1 and 365 days."
                                                    ControlToValidate="txtExpiryPeriod" Display="None" ValidateEmptyText="true"
                                                    OnServerValidate="Customvalidate" />
                                                <asp:RequiredFieldValidator ID="rfvUserEmail" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="User Email is Required" ControlToValidate="txtUserEmail" Display="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">User Information</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUserNameStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUserName" runat="server" CssClass="clsLabel">User Name</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUserName" CssClass="clsTextBoxTagSearch" 
                                                    runat="server" Text="<%# mUser.Name %>" autocomplete="off"
                                                    ToolTip="Enter User Name" BackColor="White" MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPasswordStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPassword" runat="server" CssClass="clsLabel">Password</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassword" CssClass="clsTextBoxTagSearch" 
                                                    runat="server" Text="<%# mUser.Password %>" autocomplete="off"
                                                    ToolTip="Enter Password" BackColor="White" MaxLength="15">
                                                </asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblConfirmPassword" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConfirm" runat="server" CssClass="clsLabelAuto">Confirm Password</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConPass" CssClass="clsTextBoxTagSearch" 
                                                    runat="server" Text="<%# mUser.ConfirmPassword %>"
                                                    ToolTip="Enter Confirm Password" BackColor="White" MaxLength="15">
                                                </asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExpiryPeriod" runat="server" CssClass="clsLabelAuto" 
                                                    Visible="False">Expiry Period(In Days)</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" 
                                                    ID="txtExpiryPeriod" runat="server"
                                                    Text="<%# mUser.ExpiryPeriod %>" 
                                                    ToolTip="Enter Expiry Period." BackColor="White" MaxLength="3" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUserEmail" runat="server" CssClass="clsLabelAuto">User Email</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUserEmail" CssClass="clsTextBoxTagSearch" 
                                                    runat="server" Text="<%# mUser.UserEmail %>"
                                                    ToolTip="Enter User Email ID." BackColor="White" MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:RegularExpressionValidator ID="revUser" runat="server" 
                                                    ErrorMessage="Please Enter Valid User Email ID."
                                                    ControlToValidate="txtUserEmail" Display="None" 
                                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    CssClass="clsLabelAuto" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblManagerEmail" runat="server" 
                                                    CssClass="clsLabelAuto">Manager Email</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtManagerEmail" 
                                                    CssClass="clsTextBoxTagSearch" runat="server" 
                                                    Text="<%# mUser.ManagerEmail %>"
                                                    ToolTip="Enter Manager Email ID."
                                                    BackColor="White" MaxLength="50">
                                                </asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:RegularExpressionValidator ID="revMgEmail" 
                                                    runat="server" ErrorMessage="Please Enter Valid Manager Email ID."
                                                    ControlToValidate="txtManagerEmail" 
                                                    Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    CssClass="clsLabelAuto" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblNetAccess" runat="server"
                                                    Text="Access Outside LAN?  &amp;nbsp;"
                                                     CssClass="clsLabelAuto" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkNetAccess" runat="server" 
                                                    CssClass="clsCheckBox" Checked="<%# mUser.IsAccessOutSideLAN %>" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblChangePasswordOnLogon" runat="server"
                                                    Text="Change Password On Logon  &amp;nbsp;"
                                                    CssClass="clsLabelAuto" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkLogon" runat="server"
                                                    CssClass="clsCheckBox"
                                                    Checked="<%# mUser.ChangePassword %>" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblIsCurrencywisePOLimit" runat="server"
                                                    Text="Set Currency Wise PO Limit  &amp;nbsp;"
                                                    CssClass="clsLabelAuto" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkIsCurrencywisePOLimit" 
                                                    runat="server" CssClass="clsCheckBox"
                                                    ClientIDMode="Static" onclick="Enable();"
                                                    Checked="<%# mUser.IsCurrencywisePOLimit %>" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlEmp">
                                                    <ContentTemplate>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            ID="cmbEmployeeList" runat="server" AutoPostBack="true"
                                                            DataTextField="EmpNoName" DataValueField="ID"
                                                            SelectedValue="<%# mUser.EmployeeID %>">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="left"></td>
                                        </tr>
                                        <tr>
                                            <td align="left"></td>
                                            <td colspan="3" align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnResetPassword" runat="server" Text="Reset Password"
                                                                ToolTip="Click to Reset Password"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblRoles" runat="server" CssClass="clsLabelHeader">Select Roles for this User</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table border="0" cellpadding="1" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="ClpnlRole" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle;">
                                                                        <span id="lblRoleSelection" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">List of User Roles</span>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                        <image id="imgRole" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlRole" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                    <table id="Table3" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgUser" runat="server" ToolTip="Click to select Role for the User from User Roles List"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="UserRoleID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemStyle Width="60px" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="RoleName" HeaderText="Role Name"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <cc2:collapsiblepanelextender behaviorid="clpRoleBehaviour" id="clpRole" clientidmode="Static"
                                                    runat="Server" targetcontrolid="pnlRole" expandcontrolid="ClpnlRole" collapsecontrolid="ClpnlRole"
                                                    collapsed="False" imagecontrolid="imgRole" collapsedsize="0" expandedtext="(Hide Details...)"
                                                    collapsedtext="(Show Details...)" expandedimage="~/images/collapse_blue.jpg"
                                                    collapsedimage="~/images/expand_blue.jpg" suppresspostback="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblSelectionAircraft" runat="server" CssClass="clsLabelHeader">Select Aircraft for this user</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <asp:Panel ID="ClpnlAircraft" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle;">
                                                                        <span id="lblAircraftSelection" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">
                                                                            <asp:Label ID="lblAircraftList" runat="server" CssClass="clsLabelHeader">List of Aircraft</asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                        <image id="imgAircraft" alternatetext="(Show Details...)" src="images/collapse_blue.jpg"
                                                                            style="float: right;" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlAircraft" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                    <table id="Table1" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgMachine" runat="server" ToolTip="Click to select Aircraft for the User from Aircraft List"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkSelectAllAircraft" Text="Select" AutoPostBack="True"
                                                                                    OnCheckedChanged="chkSelectAllAircraft_CheckChanged" EnableViewState="True"></asp:CheckBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="60px" HorizontalAlign="Left" />
                                                                            <ItemStyle Width="60px" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="RegNo" HeaderText="Aircraft"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <cc2:collapsiblepanelextender behaviorid="clpAircraftBehaviour" id="clpAircraft"
                                                    clientidmode="Static" runat="Server" targetcontrolid="pnlAircraft" expandcontrolid="ClpnlAircraft"
                                                    collapsecontrolid="ClpnlAircraft" collapsed="False" imagecontrolid="imgAircraft"
                                                    collapsedsize="0" expandedtext="(Hide Details...)" collapsedtext="(Show Details...)"
                                                    expandedimage="~/images/collapse_blue.jpg" collapsedimage="~/images/expand_blue.jpg"
                                                    suppresspostback="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblSelectDepartmentforthisuser" runat="server" CssClass="clsLabelHeader">Select Department for this user</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="ClpnlDepartment" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle;">
                                                                        <span id="lblDepartmentSelection" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">
                                                                            <asp:Label ID="lblListofDepartment" runat="server" CssClass="clsLabelHeader">List of Department</asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                        <image id="imgDepartment" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlDepartment" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                    <table id="Table2" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgDepartment" runat="server" ToolTip="Click to select Department for the User from Department List"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkSelectAllDept" Text="Select" AutoPostBack="True"
                                                                                    OnCheckedChanged="chkSelectAllDept_CheckChanged" EnableViewState="True"></asp:CheckBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkDepartmentSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemStyle Width="60px" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="EmployeeDepartmentName" HeaderText="Department"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <cc2:collapsiblepanelextender behaviorid="clpDepartmentBehaviour" id="clpDepartment"
                                                    clientidmode="Static" runat="Server" targetcontrolid="pnlDepartment" expandcontrolid="ClpnlDepartment"
                                                    collapsecontrolid="ClpnlDepartment" collapsed="False" imagecontrolid="imgDepartment"
                                                    collapsedsize="0" expandedtext="(Hide Details...)" collapsedtext="(Show Details...)"
                                                    expandedimage="~/images/collapse_blue.jpg" collapsedimage="~/images/expand_blue.jpg"
                                                    suppresspostback="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblSelectStoreforthisuser" runat="server" CssClass="clsLabelHeader">Select Store for this user</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="ClpnlStore" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle;">
                                                                        <span id="lblStoreSelection" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">
                                                                            <asp:Label ID="lblListofStore" runat="server" CssClass="clsLabelHeader">List of Store</asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                        <image id="imgStore" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlStore" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                    <table id="Table5" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgStore" runat="server" ToolTip="Click to select Store for the User from Store List"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkSelectAllStore" Text="Select" AutoPostBack="True"
                                                                                    OnCheckedChanged="chkSelectAllStore_CheckChanged" EnableViewState="True"></asp:CheckBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkStoreSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemStyle Width="60px" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Store" HeaderText="Store"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <cc2:collapsiblepanelextender behaviorid="clpStoreBehaviour" id="clpStore" clientidmode="Static"
                                                    runat="Server" targetcontrolid="pnlStore" expandcontrolid="ClpnlStore" collapsecontrolid="ClpnlStore"
                                                    collapsed="False" imagecontrolid="imgStore" collapsedsize="0" expandedtext="(Hide Details...)"
                                                    collapsedtext="(Show Details...)" expandedimage="~/images/collapse_blue.jpg"
                                                    collapsedimage="~/images/expand_blue.jpg" suppresspostback="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblSetcurrencywiselimittocreatepurchaseorder" runat="server" CssClass="clsLabelHeader">Select currency wise limit to create purchase order.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="ClpnlCurrency" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                <div>
                                                                    <div style="float: left; vertical-align: middle;">
                                                                        <span id="Span1" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">
                                                                            <asp:Label ID="lblListOfCurrency" runat="server" CssClass="clsLabelHeader">List of currency</asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                        <image id="imgDepartment" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlCurrency" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                    <table id="Table4" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgCurrency" runat="server" ToolTip="Click to select currency"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkCurrencySelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemStyle Width="60px" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Limit">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtLimit" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" runat="server"
                                                                                    OnTextChanged="TextChanged" MaxLength="12" Text='<%# DataBinder.Eval(Container.DataItem,"Limit") %>'>
                                                                                </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <cc2:collapsiblepanelextender behaviorid="clpCurrencyBehaviour" id="clpCurrency"
                                                    clientidmode="Static" runat="Server" targetcontrolid="pnlCurrency" expandcontrolid="ClpnlCurrency"
                                                    collapsecontrolid="ClpnlCurrency" collapsed="False" imagecontrolid="imgDepartment"
                                                    collapsedsize="0" expandedtext="(Hide Details...)" collapsedtext="(Show Details...)"
                                                    expandedimage="~/images/collapse_blue.jpg" collapsedimage="~/images/expand_blue.jpg"
                                                    suppresspostback="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="right">
                                                <table id="tblNew">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to save the current record" Visible="false"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPrintbottom" CssClass="clsbtnH clsinfoH" runat="server" Text="Print" Visible="false"
                                                                ToolTip="Click to Print the current record"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                                Text="Close" ToolTip="Click to close User Information screen" Visible="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div id="divSpinner">

                <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                    <ProgressTemplate>
                        <div class="clsAjaxLoader">
                        </div>
                        <div class="divAjaxLoader">
                            <div class="ext-el-mask-msg x-mask-loading">
                                <div class="clsLoad_ajax">
                                    <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                        ImageAlign="Middle" CssClass="ajax-loader-gif" />
                                </div>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </div>
        </div>
        <script type="text/javascript">
            var Enable = function () {
                var IsCurrencywisePOLimit = $get("chkIsCurrencywisePOLimit").checked;
                if (IsCurrencywisePOLimit) {
                    $("[id$='txtLimit']").attr('disabled', false);
                    $("[id$='chkCurrencySelect']").attr('disabled', false);

                }
                else {
                    $("[id$='txtLimit']").attr('disabled', true);
                    $("[id$='txtLimit']").val('0');
                    $("[id$='chkCurrencySelect']").attr('disabled', true);
                    $("[id$='chkCurrencySelect']").attr('checked', false);
                }
            }
        </script>
    </form>
</body>
<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        Enable();
    });  
</script>
</html>

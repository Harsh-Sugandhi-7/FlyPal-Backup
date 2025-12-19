<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfChangePassword_Ajax.aspx.vb" Inherits="Flypal.wfChangePassword_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Change Password</title>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr class="clsFormHeader1Newstyle">
                                    <td colspan="5">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Change Password</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
                                                        ToolTip="Click to Save the password" Text="Save"></asp:Button>
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH" 
                                                        ToolTip="Click to Close" Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvOldPass" runat="server" CssClass="clsLabelAuto" ErrorMessage="Old Password Required"
                                            Display="None" ControlToValidate="txtOldPassword"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" CssClass="clsLabelAuto" ErrorMessage="New Password Required"
                                            Display="None" ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvConfirmPass" runat="server" CssClass="clsLabelAuto" ErrorMessage="Confirm password Required"
                                            Display="None" ControlToValidate="txtConfrimPassword"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCheckPass" runat="server" CssClass="clsLabelAuto" ErrorMessage="New Password Must be different from Old Password."
                                            Display="None" ControlToValidate="txtNewPassword" OnServerValidate="customValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <asp:Label ID="lblRequired" runat="server" Font-Size="16px" CssClass="clsLabelAuto">
                                        You are required to change the Password
                                        </asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server" CssClass="clsLabel">User Name</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
                                            ToolTip="Enter Name" MaxLength="50" Text="<%# mUser.Name %>"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOldPassword" runat="server" CssClass="clsLabel">Old Password</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOldPassword" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Old Password"
                                            MaxLength="50" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPassword" runat="server" CssClass="clsLabel">Password</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter new Password"
                                            MaxLength="50" Text="<%# mUser.Password %>" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabel">Confirm Password</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConfrimPassword" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Confirm Password"
                                            MaxLength="50" Text="<%# mUser.ConfirmPassword %>" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
                runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                    </div>
                    <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                    Height="48px" Width="48px" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>

    </form>
</body>
</html>

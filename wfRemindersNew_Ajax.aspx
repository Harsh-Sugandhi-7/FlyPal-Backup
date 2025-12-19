<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemindersNew_Ajax.aspx.vb" Inherits="Flypal.wfRemindersNew_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Currency Information</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                    <td colspan="2">

                        <asp:UpdatePanel ID="upnlRemindersNew" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="tblInner">
                                    <tr>
                                        <td colspan="5" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Auto Reminder [Setting]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnApply" CssClass="clsbtnH clsinfoH" runat="server" Text="Apply" 
                                                            ToolTip="Click to Apply the changes" Enabled="False"></asp:Button>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                            ToolTip="Click to Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblActiveReminder" runat="server" CssClass="clsLabelHeader">Active Auto Reminder System</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbActiveYes" runat="server" CssClass="clsRadioButton" Text="Yes"
                                                            Checked="<%# mReminder.Yes %>" GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbActiveNo" runat="server" CssClass="clsRadioButton" Text="No"
                                                            Checked="<%# mReminder.No %>" GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSelect" runat="server" CssClass="clsLabelHeader">Select Days of the Week on Which Reminder Should get Active</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnMonday" runat="server" CssClass="clsCheckBox" Text="Monday"
                                                            Checked="<%# mReminder.IsOnMonday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnThursday" runat="server" CssClass="clsCheckBox" Text="Thursday"
                                                            Checked="<%# mReminder.IsOnThursday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnSunday" runat="server" CssClass="clsCheckBox" Text="Sunday"
                                                            Checked="<%# mReminder.IsOnSunday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnTuesday" runat="server" CssClass="clsCheckBox" Text="Tuesday"
                                                            Checked="<%# mReminder.IsOnTuesday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnFriday" runat="server" CssClass="clsCheckBox" Text="Friday"
                                                            Checked="<%# mReminder.IsOnFriday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnWednesday" runat="server" CssClass="clsCheckBox" Text="Wednesday"
                                                            Checked="<%# mReminder.IsOnWednesday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsOnSaturday" runat="server" CssClass="clsCheckBox" Text="Saturday"
                                                            Checked="<%# mReminder.IsOnSaturday %>" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

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

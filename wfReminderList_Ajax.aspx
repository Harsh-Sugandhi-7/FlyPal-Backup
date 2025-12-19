<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReminderList_Ajax.aspx.vb" Inherits="Flypal.wfReminderList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Reminder(s)</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <style type="text/css">

        option:hover{
            box-shadow: 0 0 10px 100px #c3d3fa inset;
        }

        option:checked{
            background-color:#4a63a0;
            color: white;
        }

    </style>

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
                        <asp:UpdatePanel ID="upnlReminderList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="tblInner">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <table id="headers" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Reminder(s)</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnShow" TabIndex="0" runat="server"
                                                            CssClass="clsbtnH clsinfoH" Text="Show"
                                                            ToolTip="Click to Display Reminder(s)"></asp:Button>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" CausesValidation="False"
                                                            ToolTip="Click to Close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CVShow" runat="server"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 103px">
                                            <table class="clstable1" id="Table3" cellspacing="0"
                                                cellpadding="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lstReminders" runat="server" CssClass="clsListBox" Height="443px" Width="100%"></asp:ListBox>
                                                    </td>
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

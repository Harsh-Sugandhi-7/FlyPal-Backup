<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUserListHavingRights_Ajax.aspx.vb"
    Inherits="Flypal.wfrptUserListHavingRights_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>User Mapping with Aircraft Store Department</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <span id="lblTitle" class="clsFormHeader">User Mapping with Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbAircraft" runat="server" Checked="True" CssClass="clsRadioButton"
                                                        GroupName="c" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <span id="lblCategory" class="clsLabel">Aircraftwise</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                        DataTextField="RegNo" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbUserwise" runat="server" CssClass="clsRadioButton" GroupName="c"
                                                        AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <span id="lblStore" class="clsLabel">Userwise</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbUserList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                        DataValueField="UserID" AutoPostBack="True" Width="250px" Enabled="false">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTopButton">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" ToolTip="Click to Print">
                                                    </asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                        ToolTip="Click to Close" CausesValidation="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </form>
</body>
</html>

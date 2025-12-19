<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnrptWOStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfnrptWOStatusList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
     <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlsearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin" border="0">
                                <tr>
                                    <td colspan="2">
                                        <span id="lbltitle" class="clstitle1">WO Status List</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Month and Year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span id="lblYear" class="clsLabelAuto">Month and Year</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBox1_Ajax">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox1_Ajax" Width="112px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <span id="lblStep3" class="clsLabelHeader">Step II. Selection of WO Type</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblModel" class="clsLabelAuto">WO Type</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbWOType" runat="server"  CssClass="clsComboBox3_Ajax"
                                            DataTextField="ModelName" DataValueField="ID">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="CAMO" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="ThirdParty" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                Text="Display" CausesValidation="true" ToolTip="Click to Display Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                Text="Close" ToolTip="Click to close the Fleet Reliability Summary screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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

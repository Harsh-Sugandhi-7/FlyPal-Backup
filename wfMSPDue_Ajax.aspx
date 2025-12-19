<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMSPDue_Ajax.aspx.vb" Inherits="Flypal.wfMSPDue_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Support Plan Due</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="3" class="clsFormHeader1">
                                        <span id="lbltitle" class="clsFormHeader">Maintenance Support Plan Due</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <span id="lblStepI" class="clsLabelHeader">Selection of As On Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="cmbRange" runat="server" Width="192px" CssClass="clsComboBox"
                                            Height="20px">
                                            <asp:ListItem Value="0">0 Days - 1 Month</asp:ListItem>
                                            <asp:ListItem Value="1">0 Days - 2 Month</asp:ListItem>
                                            <asp:ListItem Value="2">0 Days - 3 Month</asp:ListItem>
                                        </asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <span id="lblStepIII" class="clsLabelHeader">Selection of Applicable To</span>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <span id="lblStore" class="clsLabelAuto">Applicable To</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="cmbAssemblyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                            DataValueField="ID" DataTextField="ModelSerialNoPostion" Width="225px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <span id="lblStep3" class="clsLabelHeader">Selection of Maintenance Support Plan No.</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblCategory" class="clsLabelAuto">MSP No.</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbMSPText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                            AutoPostBack="True" DataTextField="Text" DataValueField="Text">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="40px"
                                            MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="right" colspan="3">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>

                                                        <td>
                                                            <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                Text="Export to Excel" ToolTip="Click to Export report" Width="110px" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                Text="Display" ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                                ToolTip="Click to close" CausesValidation="False"></asp:Button>
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
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </form>
</body>
</html>

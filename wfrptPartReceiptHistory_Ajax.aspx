<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPartReceiptHistory_Ajax.aspx.vb" Inherits="Flypal.wfrptPartReceiptHistory_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html>
<head runat="server">
    <title>Part Receipt History</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script language="javascript" id="clientEventHandlersJS">

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
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

                                     <td class="clsFormHeader1" colspan="2">
                                <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lbltitle" class="clstitle1">Part Receipt History</span>
                                                </td>
                                                <td align="right" colspan="2">
                                                    <%--<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                            Text="Export to Excel" ToolTip="Click to Export report" Width="140px"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to Close Part Receipt History screen" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                                    Display="None" ErrorMessage="Enter whole part no. and description" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of IsValued Store</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued Stores Only"
                                            Checked="True"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Part Number/Description</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server"   Width="520px" AutoPostBack="False"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIII" class="clsLabelHeader">Step III. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                     <td align="right" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            Text="Export to Excel" ToolTip="Click to Export report" Width="140px"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                            ToolTip="Click to Close Part Receipt History screen" CausesValidation="False">
                                                        </asp:Button>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>
</body>
</html>


<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOrganisationApprovalDueList_Ajax.aspx.vb"
    Inherits="Flypal.wfOrganisationApprovalDueList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Organisation Approval Due Listy</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .clstxtbox
        {
            border-top-left-radius: 20px;
            border-top-right-radius: 20px;
            border-bottom-left-radius: 20px;
            border-bottom-right-radius: 20px;
        }
    </style>
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
                                <td colspan="3">
                                    <span id="lbltitle" class="clstitle1">Organisation Approval Due List</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStepI" class="clsLabelHeader">Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtAsOnDate" runat="server" CssClass="clsTextBox_Ajax" Width="100px"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                            </cc2:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbRange" runat="server" Width="192px" CssClass="clsComboBox3_Ajax"
                                        Height="20px">
                                        <asp:ListItem Value="0">Between 0 Days - 1 Month</asp:ListItem>
                                        <asp:ListItem Value="1">Between 0 Days - 2 Month</asp:ListItem>
                                        <asp:ListItem Value="2">Between 0 Days - 3 Month</asp:ListItem>
                                        <asp:ListItem Value="3">Between 0 Days - 6 Month</asp:ListItem>
                                        <asp:ListItem Value="4">Between 0 Days - 12 Month</asp:ListItem>
                                        <asp:ListItem Value="5">Between 0 Days - 24 Month</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep3" class="clsLabelHeader">Selection of Document Details</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDocument" class="clsLabelAuto">Document</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbDocumentList" runat="server" CssClass="clsComboBox3_Ajax"
                                        DataValueField="ID" DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblTrainingOrg" class="clsLabelAuto">Document No.</span>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                        ToolTip="Enter Doc No."></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep4" class="clsLabelHeader">Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 15px">
                                                        <asp:Label ID="lblAsOnDate1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="height: 15px">
                                                        <asp:Label ID="lblRangeDisp" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDocumentCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDocumentNoCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias.">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" Text="Display" ToolTip="Click to Display Report"
                                                            CssClass="clsButton"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExpotToExcel" runat="server" ToolTip="Click to Display Report"
                                                            Text="Export To Excel" CssClass="clsButtonLong_Ajax" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                        </asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Click to close"
                                                            CssClass="clsButton"></asp:Button>
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

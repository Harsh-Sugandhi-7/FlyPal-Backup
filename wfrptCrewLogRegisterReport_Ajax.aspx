<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCrewLogRegisterReport_Ajax.aspx.vb" Inherits="Flypal.wfrptCrewLogRegisterReport_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Log Register Report</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>

    <script id="Script1" type="text/javascript">

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>


</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table border="0" id="tabMain" class="clstablelistout">
                <tr>
                    <td>
                        <table border="0" id="tabInner" class="clstablelistin">
                            <tr>
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td >
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Search Criteria For Crew Log Book</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                            <%--<td colspan="4" align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" id="tabButtons">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                        Text="Current Criteria" CausesValidation="False"
                                                                        ToolTip="Click to Display Current Searching criterias"></asp:Button></td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                        Text="Export to Excel" ToolTip="Click to Export report"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server"
                                                                        Text="Display" CausesValidation="False" ToolTip="Click to Display Report"></asp:Button></td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close"
                                                                        CausesValidation="False"
                                                                        ToolTip="Click to Close Search Criteria For Crew Log Book screen"></asp:Button></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>

                                </td>

                                

                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlDates" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tabDates">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label></td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                            runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
                                                        </asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label></td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                            runat="server">
                                                        </asp:TextBox>
                                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlAllInfo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="AllInfo">
                                                <tr>
                                                    <td>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"
                                                                    DataValueField="ID" DataTextField="RegNo">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step III. Selection of Crew/Crew combination</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:RadioButton ID="optSingle" runat="server" CssClass="clsRadioButton" Text="Single" GroupName="a" AutoPostBack="true"></asp:RadioButton>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:RadioButton ID="optBoth" runat="server" CssClass="clsRadioButton" Text="Both" GroupName="a" AutoPostBack="true"></asp:RadioButton></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCrew1" runat="server" CssClass="clsLabelAuto">Crew 1</asp:Label></td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server"></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label ID="lblCrew2" runat="server" CssClass="clsLabelAuto">Crew 2</asp:Label></td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCoPilot" runat="server"  Enabled="false"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDutyAs1" runat="server" CssClass="clsLabelAuto">Duty As 1</asp:Label></td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDutyAs1" runat="server"
                                                                    DataValueField="ID" DataTextField="DutyType">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:Label ID="lblDutyAs2" runat="server" CssClass="clsLabelAuto">Duty As 2</asp:Label></td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDutyAs2" runat="server"
                                                                    DataValueField="ID" DataTextField="DutyType" Enabled="false">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Reference Document </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="3">
                                                                <asp:CheckBox ID="chkLogNo" runat="server" CssClass="clsCheckBox" Checked="True" Text="Log No."></asp:CheckBox>
                                                                <asp:CheckBox ID="chkLogPageNo" runat="server" CssClass="clsCheckBox" Text="Log Page No."></asp:CheckBox>
                                                                <asp:CheckBox ID="chkFlightNo" runat="server" CssClass="clsCheckBox" Text="Flight No"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelHeader">Step V. Selection of Report Format </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="3">
                                                                <asp:RadioButton ID="optDetail" runat="server" CssClass="clsRadioButton" Text="Detail" GroupName="grOrientation"></asp:RadioButton>
                                                                <asp:RadioButton ID="optSummary" runat="server" CssClass="clsRadioButton" Text="Summary" GroupName="grOrientation"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlDispalyReport" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tabReport">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPilot1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCopilot" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDutyType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDutyType2" runat="server" CssClass="clsLabelAuto"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" id="tabButtons">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                             Text="Current Criteria" CausesValidation="False"
                                                            ToolTip="Click to Display Current Searching criterias"></asp:Button></td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            Text="Export to Excel" ToolTip="Click to Export report" ></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" 
                                                            Text="Display" CausesValidation="False" ToolTip="Click to Display Report"></asp:Button></td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close"
                                                            CausesValidation="False"
                                                            ToolTip="Click to Close Search Criteria For Crew Log Book screen"></asp:Button></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoEmpNoName.aspx?', {
                    width: 275,
                    autoFill: false,
                    mustMatch: false,
                    matchContains: true,
                    delay: 0
                });
                $("#<%=txtCoPilot.ClientID %>").autocomplete('wfAutoEmpNoName.aspx?', {
                    width: 275,
                    autoFill: false,
                    mustMatch: false,
                    matchContains: true,
                    delay: 0
                });
            });
        </script>
    </form>
</body>
</html>

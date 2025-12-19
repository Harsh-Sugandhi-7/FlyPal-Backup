<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForPilotLogBook_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForPilotLogBook_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <title></title>
    <script type="text/javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="2000" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td class="clsFormHeader1Newstyle">
                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Search Criteria For Pilot Log Book</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server">
                        <table id="tblInner">
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                ErrorMessage="From Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="a"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
                                    </asp:TextBox>
                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                        runat="server">
                                    </asp:TextBox>
                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
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
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"  DataValueField="MachineID"
                                                DataTextField="RegNo" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Pilot</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPilot" runat="server" CssClass="clsLabelAuto">Pilot</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbPilotList" runat="server" AutoPostBack="True"
                                        DataTextField="EmpNoName" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="chkPilot" runat="server" CssClass="clsCheckBox" Text="Pilot" Checked="True">
                                    </asp:CheckBox>
                                    <asp:CheckBox ID="chkCoPilot" runat="server" CssClass="clsCheckBox" Text="CoPilot"
                                        Checked="True"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Flight Classification </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlCheckBoxList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" runat="server" id="table4">
                                                <tr>
                                                    <td width="25px">
                                                        <asp:CheckBox ID="chkSelectAllFlightLogClassification" runat="server" AutoPostBack="true" />
                                                    </td>
                                                    <td width="100%">
                                                        <asp:Panel ID="CpnlFlightLogClassificationList" runat="server" CssClass="clsCollapsePnl"
                                                            ClientIDMode="Static" Style="border: none;">
                                                            <div style="float: left; vertical-align: middle;">
                                                                <span id="lblFlightLogClassificationList" class="clsLabelHeader" style="vertical-align: middle;
                                                                    margin-left: 2px;">Flight Log Classification </span>
                                                            </div>
                                                            <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                <image id="imgbtnClpnl" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Panel ID="pnlFlightLogClassificationList" runat="server" ClientIDMode="Static"
                                                            Visible="true">
                                                            <asp:CheckBoxList ID="ChkFlightLogClassificationList" runat="server" ClientIDMode="Static"
                                                                CssClass="clsComboBox_Ajax" DataTextField="Name" DataValueField="ID" RepeatColumns="4"
                                                                RepeatDirection="Horizontal" Width="100%">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <cc2:CollapsiblePanelExtender ID="clpFlightLogClassificationList" runat="Server"
                                                            BehaviorID="clpFlightLogClassificationListBehaviour" ClientIDMode="Static" CollapseControlID="CpnlFlightLogClassificationList"
                                                            Collapsed="True" CollapsedImage="~/images/expand_blue.jpg" CollapsedText="(Show Details...)"
                                                            ExpandControlID="CpnlFlightLogClassificationList" ExpandedImage="~/images/collapse_blue.jpg"
                                                            ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl" SkinID="CollapsiblePanelDemo"
                                                            SuppressPostBack="false" TargetControlID="pnlFlightLogClassificationList" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Selection of Reference Document </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:CheckBox ID="chkLogNo" runat="server" CssClass="clsLabel" Text="Log No." Checked="True">
                                    </asp:CheckBox>
                                    <asp:CheckBox ID="chkLogPageNo" runat="server" CssClass="clsCheckBox" Text="Log Page No.">
                                    </asp:CheckBox>
                                    <asp:CheckBox ID="chkFlightNo" runat="server" CssClass="clsLabelAuto" Text="Flight No.">
                                    </asp:CheckBox>
                                </td>
                            </tr>
                               <tr>
                                <td colspan="4">
                                <asp:UpdatePanel ID="upnlLocalUTC" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                  <asp:RadioButton ID="rdbLocal" runat="server" GroupName="a" Text="Local" Visible='<%# iif(AppSettings("ClientCode") = "GEP",True,False) %>'
                                        CssClass="clsRadioButton"></asp:RadioButton>&nbsp;&nbsp;
                                         <asp:RadioButton ID="rdbUTC" runat="server" GroupName="a" Text="UTC" Visible='<%# iif(AppSettings("ClientCode") = "GEP",True,False) %>'
                                        CssClass="clsRadioButton"></asp:RadioButton>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblPilot1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblFlightLogClassification1" runat="server" CssClass="clsLabelAuto"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" CssClass="clspanel1">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ClientIDMode="Static"
                                                            TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report" 
                                                            ValidationGroup="a" Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            ValidationGroup="a" Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server"  Text="Close" CausesValidation="False">
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
    <div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <div>
        <%--Date Validations--%>
        <script type="text/javascript">
            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $("#txtFromDate").val();
                var todate = $("#txtToDate").val();
                if (!todate) {
                    rfvToDate.isvalid = false;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
                    return;
                }
                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }
                }
                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }
            }
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    async: false,
                    data: params,
                    beforeSend: OnBeforeSend,
                    success: onSuccess,
                    error: onError
                });
                return false;
                function onSuccess(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val(result);
                    $find(extenderid).set_Text(result);
                }

                function onError(result) {
                    $(elem).removeClass('ac_loading');
                    $(elem).val('');
                    $find(extenderid).set_Text('');
                }
                function OnBeforeSend() {
                    $(elem).addClass('ac_loading');
                }
            }
        </script>
    </div>
    <script type="text/javascript">
        function showTextField() {
            var status = $("#chkSelectAllFlightLogClassification").attr("checked");
            $("#<%=ChkFlightLogClassificationList.ClientID %>").find(":checkbox").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                }
                else {
                    $(this).removeAttr("checked");
                }
            });
            return false;
        }
    </script>
    </form>
</body>
</html>

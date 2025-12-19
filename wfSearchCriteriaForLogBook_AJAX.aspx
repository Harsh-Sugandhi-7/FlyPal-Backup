<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForLogBook_AJAX.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForLogBook_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Flight Log Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="2000" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBOX id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>

            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td colspan="3" class="clsFormHeader1Newstyle">
                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader"
                            Text="Search criteria for Electronic Log Register" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server">
                            <table id="tblInner">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" 
                                                    HeaderText="Fill Up The Following Fields"
                                                    CssClass="clsValidationSummary" ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                    CssClass="clsLabelAuto" ErrorMessage="To Date Required" 
                                                    ControlToValidate="txtToDate" Display="None" ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                    ErrorMessage="To Date Required" ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                    ErrorMessage="From Date Required" ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                                    ValidationGroup="a" />
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" 
                                                    ErrorMessage="From Date should not be greater than To Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" 
                                                    CssClass="clsLabelAuto" Display="None" ControlToValidate="txtToDate" 
                                                    ErrorMessage="To Date Required" ValidationGroup="a" />
                                                <asp:CustomValidator ID="cvAircraft" runat="server" 
                                                    CssClass="clsLabelAuto" Display="None"
                                                    ControlToValidate="cmbAircraft"  ErrorMessage="Select the Aircraft" 
                                                    OnServerValidate="CustomValidations" ValidationGroup="a" />
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ValidateEmptyText="true" ControlToValidate="cmbFormat" 
                                                    ErrorMessage="Select the Aircraft" OnServerValidate="CustomValidations" ValidationGroup="a" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSelectDates" runat="server" CssClass="clsLabelHeader" 
                                            Text="Step I. Selection of Dates" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDateStar" runat="server" CssClass="clsLabelStar" Text="*" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Text="From Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" 
                                                                ClientIDMode="Static" AutoPostBack="true" runat="server" 
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" 
                                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" 
                                                                TargetControlID="txtFromDate" />
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" 
                                                                ID="FromDate_watermarkextender" ClientIDMode="Static" runat="server" 
                                                                WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Text="To Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" 
                                                                Style="margin-left: 3px;"
                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');" 
                                                                ClientIDMode="Static" runat="server" />
                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" 
                                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtToDate" />
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <asp:Label ID="lblSelectAircraft" runat="server" CssClass="clsLabelHeader" 
                                                                Text="Step II. Selection of Aircraft" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblAircraftStar" runat="server" 
                                                                CssClass="clsLabelStar" Text="*" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server" 
                                                                CssClass="clsLabelAuto" Text="Aircraft " />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                ID="cmbAircraft" runat="server" DataValueField="ID"
                                                                DataTextField="RegNo" AutoPostBack="True" />

                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlLocalUTC" UpdateMode="Conditional" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:RadioButton ID="rdbLocal" runat="server" GroupName="a" 
                                                                        Text="Local" Visible="false"
                                                                        CssClass="clsRadioButton" />
                                                                    &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rdbUTC" runat="server" GroupName="a" 
                                                                        Text="UTC" Visible="false" CssClass="clsRadioButton" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblSelectAssembly" runat="server" CssClass="clsLabelHeader"
                                                                Text="Step III. Selection of Assembly" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblAssembly" runat="server" 
                                                                CssClass="clsLabelAuto" Text="Assembly" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                ID="cmbAircraftAssembly" runat="server" AutoPostBack="true" 
                                                                DataValueField="ID" DataTextField="ModelSerialNoPostion" />
                                                            <asp:CheckBox ID="chkShowSinceTSO" runat="server" 
                                                                CssClass="clsCheckBox" Text="Show Since Overhaul Values"
                                                                Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblSelectFlightClassification" runat="server" CssClass="clsLabelHeader"
                                                                Text="Step IV. Selection of Flight Classification" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td colspan="3">
                                                            <table width="100%" runat="server" id="table4" visible="false">
                                                                <tr>
                                                                    <td width="25px">
                                                                        <input type="checkbox" style="vertical-align: bottom;" 
                                                                            id="chkSelectAllFlightLogClassification" 
                                                                            name="chkSelectAllFlightLogClassification"
                                                                            onchange="showTextField();" />
                                                                    </td>
                                                                    <td width="100%">
                                                                        <asp:Panel ID="cpnlFlightLogClassificationList" runat="server"
                                                                            CssClass="clsCollapsePnl" ClientIDMode="Static">

                                                                            <div>
                                                                                <div id="divCollapsiblePnl">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFlightLogClassificationList"
                                                                                                    class="clsLabelHeader" runat="server"
                                                                                                    Style="vertical-align: middle; margin-left: 2px;"
                                                                                                    Text="Flight Log Classification" />
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <div id="divCollapsiblePnlImg">
                                                                                                    <image id="imgbtnClpnl" alternatetext="(Show Details...)"
                                                                                                        src="images/collapse_blue.jpg" />
                                                                                                </div>

                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>

                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Panel ID="pnlFlightLogClassificationList" runat="server" 
                                                                            ClientIDMode="Static"
                                                                            Visible="true">
                                                                           
                                                                            <asp:CheckBoxList ID="ChkFlightLogClassificationList" runat="server"
                                                                                ClientIDMode="Static" CssClass="clsComboBox_Ajax" 
                                                                                DataTextField="Name" DataValueField="ID" RepeatColumns="4"
                                                                                RepeatDirection="Horizontal" Width="100%" />

                                                                        </asp:Panel>
                                                                        <cc2:CollapsiblePanelExtender ID="clpFlightLogClassificationList" 
                                                                            runat="Server" BehaviorID="clpFlightLogClassificationListBehaviour" 
                                                                            ClientIDMode="Static" 
                                                                            CollapseControlID="cpnlFlightLogClassificationList"
                                                                            Collapsed="True" CollapsedImage="~/images/expand_blue.jpg" 
                                                                            CollapsedText="(Show Details...)"
                                                                            ExpandControlID="cpnlFlightLogClassificationList"
                                                                            ExpandedImage="~/images/collapse_blue.jpg"
                                                                            ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl"
                                                                            SkinID="CollapsiblePanelDemo"
                                                                            SuppressPostBack="false" 
                                                                            TargetControlID="pnlFlightLogClassificationList" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblSelectFormat" runat="server" 
                                                                CssClass="clsLabelHeader" Text="Step V. Selection of Format" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelAuto" Text="Format" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                ID="cmbFormat" runat="server" AutoPostBack="True" />
                                                            <asp:CheckBox ID="chkMonthWise" runat="server" 
                                                                CssClass="clsCheckBox" Text="Show Monthly SubTotal" />
                                                            &nbsp;
                                                            <asp:CheckBox ID="chkTakeOffTouchDown" runat="server" 
                                                                CssClass="clsCheckBox" Text="Show TakeOff TouchDown Time" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblSelectReferenceDocument" runat="server" CssClass="clsLabelHeader"
                                                                Text="Step VI. Selection of Reference Document" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td align="left">
                                                            <asp:CheckBox ID="chkLogNo" runat="server" 
                                                                CssClass="clsLabel" Text="Log No." Checked="True" />
                                                            <asp:CheckBox ID="chkLogPageNo" runat="server" 
                                                                CssClass="clsCheckBox" Text="Log Page No." />
                                                            <asp:CheckBox ID="chkFlightNo" runat="server" 
                                                                CssClass="clsLabelAuto" Text="Flight No." />
                                                            <asp:CheckBox ID="chkRemark" runat="server" 
                                                                CssClass="clsLabelAuto" 
                                                                Text="Remark" Visible="False" />
                                                            <asp:CheckBox ID="chkFlightLogClassifications" 
                                                                runat="server"  CssClass="clsLabelAuto" 
                                                                Text="Flight Log Classifications" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblSelectActivity" runat="server"
                                                                CssClass="clsLabelHeader" Text="Step V. Selection of Activities" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td colspan="3">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowInstRem" runat="server"
                                                                            CssClass="clsCheckBox" Text="Show Install / Removal"
                                                                            Checked="True" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowCompliance" runat="server"
                                                                            CssClass="clsCheckBox" Text="Show Compliance"
                                                                            Checked="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowMaintActivity" runat="server"
                                                                            CssClass="clsCheckBox" Text="Show Maintenance Activity"
                                                                            Checked="True" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowPirepsMELSnag" runat="server"
                                                                            CssClass="clsCheckBox"
                                                                            Text='<%# IIf(AppSettings("MELSnagNomenclature") = "True",
                                                                               "Show Pireps / ADD / Defect",
                                                                               "Show Pireps / MEL / Snag") %>' Checked="True" />
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
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDisplayReport" runat="server" 
                                            CssClass="clsLabelHeader" Text="Step VII. Display Report" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSummary" runat="server" 
                                            CssClass="clsLabelAuto" Text="Your selection is as follows " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblDateRangeFrom" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblDateRangeTo" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 2px; height: 20px"></td>
                                                        <td style="height: 20px">
                                                            <asp:Label ID="lblAircraft1" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                        <td style="height: 20px"></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td colspan="2" align="left">
                                                            <asp:Label ID="lblFlightLogClassification1" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblAssembly1" runat="server" 
                                                                CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" CssClass="clspanel1">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" 
                                                                ID="btnCurrentSearchCriteria" 
                                                                TabIndex="0" runat="server" 
                                                                Text="Current Criteria" CausesValidation="False" 
                                                                ToolTip="Display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" 
                                                                runat="server" ClientIDMode="Static"
                                                                TabIndex="0" Text="Export to Excel"
                                                                ToolTip="Click to Export report"
                                                                ValidationGroup="a" 
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" 
                                                                ID="btnDisplay" TabIndex="0" runat="server"
                                                                ValidationGroup="a" Text="Display" 
                                                                ToolTip="Display Report in PDF" />
                                                        </td>
                                                        <%-- 'Added by Shital on 6-Sep-2016--%>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1"
                                                                ID="btnByMail" runat="server" 
                                                                Text="Report By Mail"
                                                                ToolTip="Receive Report through Mail." 
                                                                ValidationGroup="a" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose"
                                                                runat="server" Text="Close" 
                                                                CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                    <!--Dummy panel to open modelpopup 6-Sep-2016-->
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;" colspan="2" align="right">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static"
                                                                        runat="server" Text="----" CausesValidation="False" 
                                                                        Style="display: none;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <!--End -->
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

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <%--Date Validations--%>
        <div>

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

        <!-- Popup For Report By Mail 6-Sep-2016-->
        <div id="SendMailPopUp">

            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    scrolling="auto" allowtransparency="true"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
                PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>

            <script type="text/javascript">

                function OpenByMaiWindow() {

                    try {

                        $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                        $("#btnDummyReceipt1").click();

                        return false;

                    } catch (e) {
                        alert(e);
                    }

                }

                function ParentCallBackFunctionForSendMail() {

                    var Receiptwindow1 = $find("<%=
                            mdlPopupReceipt1.ClientID %>");
                    Receiptwindow1.hide();
                    $("#IframeReceipt1").attr("src", "JavaScript:''");

                }

                function ParentCallBackFunctionToSendMail() {

                    var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
                    Receiptwindow1.hide();
                    $("#IframeReceipt1").attr("src", "JavaScript:''");
                    $("#hdnimgLogBtnSendMail").click();

                }

            </script>

        </div>
        <!---End-->

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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptFlightDelayCancellationRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptFlightDelayCancellationRegister_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Flight Delay Cancellation Register</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Flight Delay Cancellation Register</span>
                                        </td>
                                        <%--<td align="right" colspan="5">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                    Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" Text="Display"
                                                                    ToolTip="Click to Display Report" ValidationGroup="valGroup1" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False"
                                                                    Text="Close" ToolTip="Click to Close" />
                                                            </td>
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
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                            ErrorMessage="To Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                            ErrorMessage="From Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAircraftModel" runat="server" Display="None" ControlToValidate="cmbAircraft"
                                            ErrorMessage="Please select either Model or Aircraft" ClientValidationFunction="ValidateModelAircraft"
                                            CssClass="clsLabelAuto" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvModel" runat="server" Display="None" ControlToValidate="cmbAircraft"
                                            ErrorMessage="Please select at least one Model" ClientValidationFunction="ValidateModel"
                                            CssClass="clsLabelAuto" ValidationGroup="valGroup1"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- Client side validation for comboboxes--%>
                                <script type="text/javascript">
                                    //Model Or Aircraft List
                                    function ValidateModelAircraft(source, args) {
                                        args.IsValid = false;
                                        var IsModel = $get("chkModel").checked;
                                        var IsAircraft = $get("chkAircraft").checked;
                                        if (IsModel || IsAircraft) {
                                            args.IsValid = true;
                                            return;

                                        }

                                    }
                                    //ModelList
                                    function ValidateModel(source, args) {
                                        args.IsValid = false;
                                        var IsModel = $get("chkModel").checked;

                                        if (IsModel) {
                                            if ($("[id*=ChkModelList] input").is(":checked")) {
                                                args.IsValid = true;
                                                return;
                                            }

                                        }
                                        else {
                                            args.IsValid = true;
                                            return;
                                        }

                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="5">
                                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span id="Span5" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="lblToDate" style="margin-left: 3px;" class="clsLabelAuto">To Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;" 
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true"></asp:TextBox>
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
                                                <td align="left" colspan="5">
                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Model/Aircraft</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td align="left" colspan="4">
                                                    <asp:UpdatePanel ID="upnlModelAircraft" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkModel" runat="server" CssClass="clsCheckBox" Text="Model" TextAlign="Right"
                                                                                        AutoPostBack="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBoxList ID="ChkModelList" runat="server" CssClass="clsComboBox_Ajax" DataTextField="ModelName"
                                                                                        DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal" Width="400px">
                                                                                    </asp:CheckBoxList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkAircraft" runat="server" CssClass="clsCheckBox" Text="Aircraft"
                                                                                        AutoPostBack="true" TextAlign="Right" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"  DataValueField="ID"
                                                                                        DataTextField="RegNo">
                                                                                    </asp:DropDownList>
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
                                                <td align="left" colspan="5">
                                                    <span id="Span1" class="clsLabelHeader">Step III. Selection of ATA</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left">
                                                    <span id="Span4" class="clsLabelAuto">ATA</span>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAList" runat="server" DataValueField="ID"
                                                        DataTextField="ATAChapter">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="5">
                                                    <span id="Span2" class="clsLabelHeader">Step IV. Selection of Type</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left">
                                                    <span id="Span3" class="clsLabelAuto">Type</span>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbType" runat="server">
                                                        <asp:ListItem Text="(All)" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Delay" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Cancellation" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblStep3" class="clsLabelHeader">Step V. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblModelNoCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraftCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblATACriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblTypeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                         Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" Text="Display"
                                                        ToolTip="Click to Display Report" ValidationGroup="valGroup1" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                        Text="Close" ToolTip="Click to Close" />
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
    <script type="text/javascript">
        function ControlVisibilty() {

        }
    </script>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompliedMaintenanceActivity_AJAX.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfCompliedMaintenanceActivity_AJAX" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.ModelMonitorModTypeList" %>
<%@ Import Namespace="Flypal.PartMonitorServiceTypeList" %>
<%@ Import Namespace="Flypal.ModelMonitorInspTypeList" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complied Maintenance Activities</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <script id="clientEventHandlersJS" type="text/javascript">
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
        .btn
        {
            padding: 1px;
            font-size: 8pt;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
        .label
        {
            font-weight: normal !important;
        }
        .required:before
        {
            content: "*";
            font-weight: bold;
            font-size: small;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Complied Maintenance Activities</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAircraft"
                                                OnServerValidate="CustomValidate" Display="None" ErrorMessage="Please Select the Aircraft."></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                                ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                                ErrorMessage="As On Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                                Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="custAircraft" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtARCDate"
                                                OnServerValidate="CustomValidate" Display="None" ErrorMessage="ARC Date required"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Effective Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <span id="lblFromDate" runat="server" class="clsLabelAuto">From Date</span>
                                </td>
                                <td>
                                    <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this)" Height="25px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                    onchange="ValidateDateText(this);" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" Height="25px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep2" runat="server" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblAircraftStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td align="left">
                                    <span id="lblAircraft" runat="server" class="clsLabelAuto">Aircraft</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlAircraft" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                DataTextField="RegNo" DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="Label3" runat="server" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="left">
                                    <span id="lblAssembly" runat="server" class="clsLabelAuto">Assembly</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataTextField="ModelSerialNoPostion"
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep4" runat="server" class="clsLabelHeader">Step IV. Selection of Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <table id="Table1" border="0">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkService" ClientIDMode="Static" runat="server" GroupName="a" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkInspection" ClientIDMode="Static" runat="server" GroupName="a"
                                                                Text="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkDirective" ClientIDMode="Static" runat="server" GroupName="a"
                                                                Text="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>

                                            <td width="205px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkSnag" runat="server" GroupName="a" Text="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td width="100%">
                                                            <asp:Panel ID="ClpnlSnag" Height="25px" Style="text-align: center;" runat="server" CssClass="clsCollapsePnlMultiselect">
                                                                <div>
                                                                    <div style="vertical-align: middle;">
                                                                        <asp:Label ID="Span1" CssClass="clsLabel" Style="text-align: center;" runat="server" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect","Snag") %>'></asp:Label>
                                                                    </div>
                                                                    <%-- <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                        <image id="imgDirective" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                                    </div>--%>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <%--<td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="chkSnag" runat="server" GroupName="a" Text="" />
                                                                    </td><td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:Panel ID="ClpnlSnag" runat="server" Style="border: none;">
                                                                        <asp:Label ID="lblSang" runat="server" Text="Snag" style="vertical-align: middle; margin-left: 2px;"></asp:Label>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="Span4" runat="server" class="clsLabelHeader">Step V. Selection of Left Over
                                        Percent Life</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlPercentLife" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkPercentLife" runat="server" AutoPostBack="True" CssClass="clsCheckBox" />
                                                        <span class="clsLabel">% Life LeftOver Greater/Equal</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtPercentage" runat="server" 
                                                            Enabled="False" MaxLength="4" ToolTip="Enter Percentage" Height="25px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto"></asp:CheckBox>
                                                        <span class="clsLabel">Show ONLY "APPLICABLE" records</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <asp:PlaceHolder runat="server" ID="phFormat" Visible='<%# AppSettings("ClientCode")="SUH" %>'>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="Span2" runat="server" class="clsLabelHeader">Step VI. Selection of Format
                                            and ARC Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                    <td align="left">
                                        <span id="Span3" runat="server" class="clsLabelAuto">Format</span>
                                    </td>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                                                <asp:ListItem Text="Format 1"></asp:ListItem>
                                                                <asp:ListItem Text="Format 2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlARCDate" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblARCDate" runat="server" class="clsLabelAuto required">ARC Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtARCDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                                            Height="20px" onchange="ValidateDateText(this,'ARCDate_watermarkextender');"
                                                                            ClientIDMode="Static" runat="server" CausesValidation="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtARCDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtARCDate">
                                                                        </cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtARCDate" ID="ARCDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox">
                                                                        </cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep7" runat="server" class="clsLabelHeader">Step VII. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="lblSummary" runat="server" class="clsLabelAuto">Your selection is as follows
                                        :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table7" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" CausesValidation="true"
                                                            TabIndex="0" Text="Display" ToolTip="Click to Display Report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByExcel" runat="server" TabIndex="25"
                                                            Text="Export to Excel" ToolTip="Click to Export to Excel" ValidationGroup="1"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                             />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close Search criteria for Complied Maintenance Activities screen " />
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
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

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
        <%--<script type="text/javascript">


            $("#chkService").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');

            });
            $("#chkInspection").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');
                $('#chkService').removeAttr('disabled');

            });
            $("#chkDirective").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');

            });
            $("#chkSnag").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').attr('disabled', 'disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').attr('disabled', 'disabled');
            });


            //            $(document).ready(function () {
            //                Controlvisibility($("#chkService"), "ListServiceType");
            //                Controlvisibility($("#chkInspection"), "ListInspectionType");
            //                Controlvisibility($("#chkDirective"), "ListDirectiveType");
            //            });

            //Service/inspection/Directive list checking
            function Controlvisibility(elem, childid) {
                //if selected then enable and select checkboxlist else uncheck and disable list
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    //  $('#' + childid).removeAttr('disabled');
                    $('#' + childid).prop('disabled', false);

                }
                else {
                    $('#' + childid).prop('disabled', true);
                }
                //                $('#' + childid).find(":Checkbox").each(function () {
                //                    if (status == "checked") {
                //                        $(this).attr("checked", status);
                //                        $(this).removeAttr('disabled');
                //                    }
                //                    else {
                //                        $(this).removeAttr("checked");
                //                        $(this).attr('disabled', 'disabled');
                //                    }
                //                });
            }

            function ControlvisibilityForCheckboxlist() {
                //  Controlvisibility($("#chkService"), "ListServiceType");
                //  Controlvisibility($("#chkInspection"), "ListInspectionType");
                //  Controlvisibility($("#chkDirective"), "ListDirectiveType");

            }
        </script>--%>
    </div>
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">

        $("#chkService").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
            $('[id*=ListServiceType]').multiselect('selectAll', false);
            $('[id*=ListServiceType]').multiselect('updateButtonText');

            $('[id*=ListInspectionType]').multiselect('clearSelection', true);            //Clears all selected items in Inspection list & update button Text
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });

        $("#chkInspection").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListInspectionType]').multiselect('enable', true);
            $('[id*=ListInspectionType]').multiselect('selectAll', false);
            $('[id*=ListInspectionType]').multiselect('updateButtonText');

            $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListDirectiveType]').multiselect('disable', false);
            $('[id*=ListServiceType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });
        $("#chkDirective").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListDirectiveType]').multiselect('enable', true);
            $('[id*=ListDirectiveType]').multiselect('selectAll', false);
            $('[id*=ListDirectiveType]').multiselect('updateButtonText');

            $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListInspectionType]').multiselect('clearSelection', true);

            $('[id*=ListServiceType]').multiselect('disable', false);
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });

        $("#chkSnag").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListInspectionType]').multiselect('clearSelection', true);
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListServiceType]').multiselect('disable', false);
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('[id*=ListDirectiveType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', 'disabled');
            $('#cmbFormat').val('0')
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var ServStatus = document.getElementById("chkService");
                    $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListInspectionType]').multiselect('disable', false);

                    if (ServStatus.checked) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');

                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Services',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Services',
                nSelectedText: 'Services'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');

        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListDirectiveType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var DirStatus = document.getElementById("chkDirective");
                    $('[id*=ListServiceType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListInspectionType]').multiselect('disable', false);

                    if (DirStatus.checked) {
                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');

                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Directive',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directive',
                nSelectedText: 'Directive'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListInspectionType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var Inspstatus = document.getElementById("chkInspection");
                    $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListServiceType]').multiselect('disable', false);
                    if (Inspstatus.checked) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');

                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');

                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Inspection',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Inspection',
                nSelectedText: 'Inspection'


            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var Serstatus = document.getElementById("chkService");
            if (Serstatus.checked) {

                $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                $('[id*=ListInspectionType]').multiselect('disable', false);
            }

        });
    </script>
</body>
</html>

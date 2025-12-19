<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptWOEmpWiseWorkDone_Ajax.aspx.vb"
    Inherits="Flypal.wfrptWOEmpWiseWorkDone_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Work Order Summary</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="4">
                                <span id="lbltitle" class="clstitle1">Employee Wise Work Done</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlValidations" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                            ErrorMessage="From Date Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                            ErrorMessage="To Date Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" ClientValidationFunction="BetweenDatesValidation"
                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="From Date should not be greater than To Date."
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvEmp" runat="server" ControlToValidate="cmbEmployee" CssClass="clsLabel"
                                            ValidationGroup="1" ValidateEmptyText="true" Display="None" ErrorMessage="Please Select Employee"
                                            ClientValidationFunction="ValidateItem"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function ValidateItem(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbEmployee");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                              
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblDateRange" class="clsLabel">Date Range</span>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                        <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                        <asp:ListItem Value="Last Week">Last 1 Week</asp:ListItem>
                                                        <asp:ListItem Value="Last Month">Last 1 Month</asp:ListItem>
                                                        <asp:ListItem Value="Last Quarter">Last 1 Quarter</asp:ListItem>
                                                        <asp:ListItem Value="Last Year">Last 1 Year</asp:ListItem>
                                                        <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table2" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                                    runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                    runat="server"></asp:TextBox>
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
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Employee</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblEmployeeStar" class="clsLabelStar">*</span>
                            </td>
                            <td align="left" width="75px">
                                <span id="lblEmployee" class="clsLabel">Employee</span>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbEmployee" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                    DataTextField="EmpNoName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step III. Selection of Model</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblStatus" class="clsLabel">Model</span>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtModelList" runat="server" CssClass="clsTextBox_Ajax" Width="275px"></asp:TextBox>
                                <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                    CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                </cc2:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Enter of Reg. No. </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblRegNo" class="clsLabel">Reg. No.</span>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBox_Ajax" Width="275px"
                                    MaxLength="50" ToolTip="Enter Reg. No."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <span id="lblStep5" class="clsLabelHeader">Step V. Selection of WO. Job Type</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblWoJobType" class="clsLabel">Job Type</span>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbWOJobType" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                    DataTextField="Name" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader" Visible='<%# AppSettings("ClientCode")="BRD" %>'>Step VI. Selection of Format</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" Visible='<%# AppSettings("ClientCode")="BRD" %>'
                                    CssClass="clsLabelHeader">Format</asp:Label>
                            </td>
                            <td>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbFormat" Visible='<%# AppSettings("ClientCode")="BRD" %>'
                                    runat="server" CssClass="clsComboBox_Ajax">
                                    <asp:ListItem Selected="True" Text="Format 1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Format 2" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table8" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEmployee1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRegNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblJobType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                        ToolTip="Click to display current searching criterias" Text="Current Criteria">
                                                    </asp:Button>
                                                </td>
                                                 <td>
                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Export report"
                                                        Width="100px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>" ></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to display report"
                                                        CausesValidation="true" ValidationGroup="1" Text="Display"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Employee Wise Work Done Screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
            var selectedDateIndex = $get("cmbDateRange").selectedIndex;
            if (selectedDateIndex == 6) {
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
    </form>
</body>
</html>

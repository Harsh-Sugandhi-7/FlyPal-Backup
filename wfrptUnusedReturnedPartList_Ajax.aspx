<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUnusedReturnedPartList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptUnusedReturnedPartList_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Unused Return Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <%--AJAX- ScriptManager Added--%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="4" class="clsFormHeader1Newstyle">
                                        <span id="lbltitle" class="clstitle1">Unused Return Part List</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1">
                                        </asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                            ErrorMessage="From Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                            ErrorMessage="To Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="valGroup1"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblFromDate" class="clsLabelAuto">From</span>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                         ClientIDMode="Static"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="lblToDate" class="clsLabelAuto">To</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" runat="server" Style="margin-left: 3px;"
                                                         onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"></asp:TextBox>
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
                                    <td colspan="4" align="left">
                                        <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Part Number/Description</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                    </td>
                                    <td colspan="3" align="left">
                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtPartDescription" runat="server"
                                            ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <span id="Span1" class="clsLabelHeader">Step III. Selection of Issue To</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <span id="Span2" class="clsLabelAuto">Issue To</span>
                                    </td>
                                    <td colspan="3" align="left">
                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIssueTo" runat="server" 
                                            ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <span id="lblStep4" class="clsLabelHeader">Step IV. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblToDate1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblIssueTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                        Text="Current Criteria" ToolTip="Click to display current searching criteria" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server"  ValidationGroup="valGroup1"
                                                         ToolTip="Click to Export report" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server"  Text="Display"
                                                        ToolTip="Click to display report" ValidationGroup="valGroup1" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False" 
                                                        Text="Close" ToolTip="Click to close Unused Return Part List screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtPartDescription.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                max: 100,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("div[id*='Panel1']").each(function () {
                $(this).find(":text").attr('class', 'clsTextBoxDate_Ajax');
                $(this).find(":image").css({ 'vertical-align': 'top' });
            });

            $("#<%=txtIssueTo.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=UnusedReturnIssueTo', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
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
</body>
</html>

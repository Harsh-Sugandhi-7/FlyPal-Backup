<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftCapitalizationRecords_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAircraftCapitalizationRecords_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Capitalization</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
     <script type="text/javascript">
         function openTranDetail() {
             str = "wfReports.aspx"
             window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
         }
         function openFile() {
             str = "wfExportToExcel.aspx"
             window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table>
                            <tr>
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clstitle1">Capitalization</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" Height="72px" Width="440px"
                                        HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                        ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStepI" class="clsLabelHeader">Step I. Selection Of Date Range </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtFromDate" 
                                        onchange="ValidateDateText(this);"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Format="<%$AppSettings:DateFormat%>" Enabled="true" TargetControlID="txtFromDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                                <td>
                                    <span id="lblToDate" class="clsLabelAuto">To Date</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtToDate" 
                                        onchange="ValidateDateText(this);"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Format="<%$AppSettings:DateFormat%>" Enabled="true" TargetControlID="txtToDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="txtToDateTextBoxWatermarkExtender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStepII" class="clsLabelHeader">Step II. Selection Of Aircraft/WorkShop
                                        option </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:RadioButton ID="rbAircraft" runat="server" CssClass="clsRadioButton" GroupName="b"
                                        AutoPostBack="true" Text="Aircraft" Checked="True" />
                                    <asp:RadioButton ID="rbWorkShop" runat="server" CssClass="clsRadioButton" AutoPostBack="true"
                                        GroupName="b" Text="WorkShop" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStepIII" class="clsLabelHeader">Step III. Selection Of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblMachine" class="clsLabelAuto">Aircraft</span>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataValueField="ID"
                                        DataTextField="RegNo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStepIV" class="clsLabelHeader">Step IV. Selection Of WorkShop</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblWorkShop" class="clsLabelAuto">WorkShop</span>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWorkShop" runat="server"  DataValueField="ID"
                                        DataTextField="LocationWorkShop" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStepV" class="clsLabelHeader">Step V. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div>
                                        <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblWorkShop1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" Text="Export to Excel"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                 ToolTip="Click to Export report"></asp:Button>
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                Text="Display" ToolTip="Click to display report" />
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                TabIndex="0" Text="Close" ToolTip="Click to Close Capitalization Report screen" />
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
    </form>
</body>
</html>

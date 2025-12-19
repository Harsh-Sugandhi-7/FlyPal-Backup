<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAircraftCRS_Ajax.aspx.vb"
    Inherits="Flypal.wfAircraftCRS_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Certificate of Release To Service</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="1000" ID="ScriptManager1" runat="server">
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
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                            <table id="tblinner" class="clsTablelistin">
                                <tr>

                                    <td class="clsFormHeader1" colspan="4">
                                    <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblTitle" class="clsFormHeader">Aircraft Certificate of Release To Service</span>
                                                </td>
                                                <td align="right"   valign="top">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnPrint" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                                            Text="Print" ToolTip="Click to Print the Aircraft Certificate of Release to Service Info."
                                                                            ValidationGroup="1" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            Text="Close" ToolTip="Click to go Back to Previous Page" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>

                                            </tr>
                                        </table>
                                    </tb>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Dates Check Performed From </span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                            runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                    </td>
                                    <td>
                                        <span id="lblToDate" class="clsLabelAuto">To Date</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxTagSearchDate"
                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                            runat="server"></asp:TextBox>
                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">A/C Regn. </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAircraft" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearch"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Work Order No. </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtWONo" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearch"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">CAAN Approval No. </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCAANApprovalNo" runat="server" CssClass="clsTextBoxTagSearch" Text="CAAN 145 002" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Form Tracking No. </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtFormTrackingNo" runat="server" CssClass="clsTextBoxTagSearch" Text="CRS/EE/047 E (13/14)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Approved Maintenance Program </span>
                                    </td>
                                    <td colspan="3">
                                        <table border="1">
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Issue </span>
                                                </td>
                                                <td>
                                                    <span class="clsLabelAuto">Amendment </span>
                                                </td>
                                                <td>
                                                    <span class="clsLabelAuto">Date </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtIssue" runat="server" CssClass="clsTextBoxTagSearch" Text="C" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmendment" runat="server" CssClass="clsTextBoxTagSearch" Text="C0"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDate" Style="margin-left: 3px;" CssClass="clsTextBoxTagSearchDate"
                                                        onchange="ValidateDateText(this,'Date_watermarkextender');" ClientIDMode="Static"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Work Package Ref. </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtWorkPackageRef" runat="server" CssClass="clsTextBoxTagSearch" Text="047 E (13/14)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="clsLabelAuto">Status/Work </span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRemovalReason" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                             TextMode="MultiLine" Width ="500px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td align="right" colspan="4" valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CausesValidation="true" CssClass="clsButton_Ajax"
                                                            Text="Print" ToolTip="Click to Print the Aircraft Certificate of Release to Service Info."
                                                            ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip="Click to go Back to Previous Page" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <script type="text/javascript">
            //Date validations

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $find('FromDate_watermarkextender').get_Text(); // $("#txtFromDate").val();
                var todate = $find('ToDate_watermarkextender').get_Text(); // $("#txtToDate").val();

                if (fromdate == "" || todate == "") {
                    args.IsValid = true;
                    return;
                }

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

            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
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
    </form>
</body>
</html>

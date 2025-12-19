<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptEngineOilUpliftForMonth_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptEngineOilUpliftForMonth_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <span id="lbltitle" class="clstitle1">Engine Oil Uplift</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Aircraft"
                                    ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td>
                                <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                            </td>
                            <td>
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
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
                                <span id="lblToDatee" class="clsLabelAuto">To Date</span>
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
                            <td colspan="5" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblModel" class="clsLabelAuto">Aircraft</span>
                            </td>
                            <td colspan="3" align="left">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" ClientIDMode="Static" runat="server" 
                                    DataTextField="RegNo" EnableViewState="false" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblyear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlActions">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                        ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                        ToolTip="Click to Display Report" Text="Display"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server"  ToolTip="Click to close"
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
    <script type="text/javascript">
        //Aircraft validation
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
    </script>
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

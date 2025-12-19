<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptHoursCorrection_Ajax.aspx.vb" EnableEventValidation="false"
    Inherits="Flypal.wfrptHoursCorrection_Ajax"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Hours Correction Report</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="" type="text/javascript">
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
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clstitle1">Hours Correction Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px">
                                    <span id="Label3" class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtDate"
                                        onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px" align="right">
                                    <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                </td>
                                <td align="left">
                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" ClientIDMode="Static" runat="server"
                                        DataValueField="ID" EnableViewState="false" DataTextField="RegNo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px" align="left">
                                </td>
                                <td colspan="2" align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td style="width: 2px" align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" CausesValidation="False"
                                                            ToolTip="Click to Close Hours Correction Report Screen"></asp:Button>
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
    </div>
    <%--Date Validations--%>
    <script type="text/javascript">
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
    </form>
</body>
</html>

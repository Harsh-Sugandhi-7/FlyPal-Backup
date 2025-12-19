<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTechnicalDailyReport_Ajax.aspx.vb" EnableEventValidation="false"
    Inherits="Flypal.wfTechnicalDailyReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Technical Daily Report</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="3" class="clsFormHeader1Newstyle">
                                <span id="lbltitle" class="clstitle1">Technical Daily Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtDate"
                                    ErrorMessage="Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtDate" ErrorMessage="Date Required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Aircraft"
                                    ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td width="5px">
                            </td>
                            <td>
                                <span id="lblDate" class="clsLabelAuto">Date</span>
                            </td>
                            <td>
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtDate" ClientIDMode="Static" runat="server"
                                    CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                </cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="FromDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox">
                                </cc2:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" ClientIDMode="Static" DataValueField="ID" EnableViewState="false"
                                    DataTextField="RegNo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="5px">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="2">
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
                                        <table id="Table3" align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                        Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias.">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server"  Text="Display"
                                                        ToolTip="Click to Display Report"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" Text="Close" ToolTip="Click to close " CausesValidation="False">
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

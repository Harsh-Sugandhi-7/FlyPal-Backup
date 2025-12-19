<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSectorProfileList_AJAX.aspx.vb"
    Inherits="Flypal.wfrptSectorProfileList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sector Profile Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td class="clsFormHeader1Newstyle">
                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Sector Profile Report (Flying Hours)</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvValidator" runat="server" OnServerValidate="CustomValidate"
                                ControlToValidate="cmbModel" CssClass="clsValidationSummary" Display="None"></asp:CustomValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblInner" width="100%">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                              <table id="Table1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                    runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                    AutoPostBack="True"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                    runat="server"  AutoPostBack="True"></asp:TextBox>
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
                            <td colspan="4">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Model</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server" DataValueField="ID"
                                    DataTextField="ModelName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step III. Selection of Flying</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabel">Flying Hours</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:RadioButton ID="rdbAvg" runat="server" CssClass="clsRadioButton" Text="By Average Flying"
                                    GroupName="a" Checked="True"></asp:RadioButton>
                                <asp:RadioButton ID="rdbTotal" runat="server" CssClass="clsRadioButton" Text="By Total Flying"
                                    GroupName="a"></asp:RadioButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Sectors</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Sectors</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlSectors" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkSelectAll" CssClass="clsLabelAuto" runat="server" AutoPostBack="true"
                                                        Visible="false" Text="All" />
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="chkSectors" runat="server" Width="440px" CssClass="clsComboBox_Ajax"
                                                        DataValueField="ID" DataTextField="Route" RepeatDirection="Horizontal" RepeatColumns="3">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
            <tr>
                <td align="right">
                    <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                Text="Current Criteria" ToolTip="Click to display Current Searching criterias."
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                ToolTip="Click to close Sector Profile Reports screen" CausesValidation="False">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div>
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
    <div>
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
    </form>
</body>
</html>

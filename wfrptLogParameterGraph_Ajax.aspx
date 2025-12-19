<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptLogParameterGraph_Ajax.aspx.vb"
    Inherits="Flypal.wfrptLogParameterGraph_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Log Parameter Graph</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
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
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Log Parameter Graph Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        ControlToValidate="txtFromDate" Display="None" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                        ControlToValidate="txtToDate" Display="None" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvaircraft" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAircraft"
                                        Display="None" ErrorMessage="Aircraft Required." ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvAssembly" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAssembly"
                                        Display="None" ErrorMessage="Assembly Required." ClientValidationFunction="ValidateAssembly"></asp:CustomValidator>
                                    <%--  <asp:CustomValidator ID="cvParameter" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbParameter"
                                         Display="None" ErrorMessage="Parameter Required." ClientValidationFunction="ValidateParameter"></asp:CustomValidator>--%>
                                    <asp:CustomValidator ID="cvMin" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtMin"
                                        Display="None" ClientValidationFunction="ValidatetxtMin"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvMax" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtMax"
                                        Display="None" ClientValidationFunction="ValidatetxtMax"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                        ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="5px">
                                </td>
                                <td>
                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
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
                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrencyStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDocTypeNo" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlAircraftSelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                DataValueField="ID" DataTextField="RegNo">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlAssemblySelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                AutoPostBack="True" DataValueField="ID" DataTextField="ModelSerialNoPostion"
                                                Enabled="False">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblSelectParameter" runat="server" CssClass="clsLabelHeader" Visible="False">Step IV. Selection of Parameter</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlParameterselection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblParamater1" runat="server" CssClass="clsLabelAuto" Visible="False">Parameter</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBoxList ID="cmbParameter" runat="server" CssClass="clsCheckBox" DataValueField="ParameterId"
                                                            DataTextField="ParameterName">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="clsComboBox" AutoPostBack="True"
                                                            DataValueField="ParameterId" DataTextField="ParameterName" Enabled="False" Visible="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox" Visible="False"
                                                            ReadOnly="True" BackColor="#E0E0E0" ToolTip="Description"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <table>
                                                            <tr>
                                                                <td width="55">
                                                                    <asp:Label ID="lblMin" runat="server" CssClass="clsLabelAuto" Visible="False">Min</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMin" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                        Visible="False" MaxLength="4"></asp:TextBox>
                                                                </td>
                                                                <td width="55">
                                                                    <asp:Label ID="lblMax" runat="server" CssClass="clsLabelAuto" Visible="False">Max</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMax" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                                        Visible="False" MaxLength="4"></asp:TextBox>
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
                                <td colspan="3">
                                    <asp:Label ID="lblStepIV" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
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
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMinValue" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMaxValue" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                            ToolTip="Click to display Current Searching criterias." CausesValidation="False"
                                                            Text="Current Criteria"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to Display Report" Text="Display"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Log Parameter Graph screen"
                                                            CausesValidation="False" Text="Close"></asp:Button>
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
        //Aircraft validation
        //         function ValidateParameter(source, args) {
        //             args.IsValid = false;

        //              
        //            
        //            var  chk = $get("cmbParameter");
        //             if (chk.checked) {
        //                 args.IsValid = true;
        //                 return;
        //             }
        //         }
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
        function ValidateAssembly(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAssembly");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
        function ValidatetxtMin(source, args) {
            args.IsValid = false;
            var txt1 = $get("txtMin").value;
            var txt2 = $get("txtMax").value;

            if (txt > txt2) {
                args.IsValid = true;
                return;
            }
        }

        function ValidatetxtMax(source, args) {
            args.IsValid = false;
            var txt1 = $get("txtMin").value;
            var txt2 = $get("txtMax").value;

            if (txt > txt2) {
                args.IsValid = true;
                return;
            }
        }
    </script>
    </form>
</body>
</html>

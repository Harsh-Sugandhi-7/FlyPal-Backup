<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForLifeHistoryAssembly_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForLifeHistoryAssembly_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Life History Assembly</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
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
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
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
        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="tblmain" class="clstablelistout">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Life History Assembly</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" />
                                           
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" ControlToValidate="txtToDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" InitialValue="<%$AppSettings:DateFormat%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="From Date Required" InitialValue="<%$AppSettings:DateFormat%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" ClientValidationFunction="BetweenDatesValidation"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="From Date should not be greater than To Date."></asp:CustomValidator>
                                             <asp:CustomValidator ID="cvPeriod" runat="server" ClientValidationFunction="validatePeriod"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="Period Required."></asp:CustomValidator>

                                            <script type="text/javascript">
                                                function validatePeriod(source, args) {
                                                    args.IsValid = false;
                                                    var status = $get("cmbPeriods").isDisabled;
                                                    if (status == false) {
                                                        args.IsValid = true;
                                                        return; 
                                                    }

                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From</asp:Label>
                                        </td>
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
                                        <td width="19px">
                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
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
                                    <tr>
                                        <td align="left" colspan="5">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step II. Selection of Graph Group Type</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblGraphGroupType" runat="server" CssClass="clsLabelAuto">Graph Group type</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbGraphGroupType" runat="server" CssClass="clsComboBox_Ajax"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="0">Year</asp:ListItem>
                                                <asp:ListItem Value="1">Month</asp:ListItem>
                                                <asp:ListItem Value="2">Day</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left">
                                            <asp:RadioButton ID="optLine" runat="server" CssClass="clsRadioButton" Checked="True"
                                                Text="Line" GroupName="grLine" Visible="False"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="optBar" runat="server" CssClass="clsRadioButton" Text="Bar"
                                                GroupName="grLine" Visible="False"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5">
                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly Type And Model</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssemblyType" runat="server" CssClass="clsLabelAuto">Assembly Type</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox_Ajax"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                <asp:ListItem Value="1">Airframe</asp:ListItem>
                                                <asp:ListItem Value="2">Engine</asp:ListItem>
                                                <asp:ListItem Value="3">Propeller</asp:ListItem>
                                                <asp:ListItem Value="4">Auxiliary Power Unit</asp:ListItem>
                                                <asp:ListItem Value="5">Combined Gear Box</asp:ListItem>
                                                <asp:ListItem Value="6">Main Gear Box</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblModel2" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsComboBoxLong1" AutoPostBack="True"
                                                Width="238px" DataValueField="ID" DataTextField="ModelSerialNo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblModelStar1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto" Visible="False">Model</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBox_Ajax" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto" Visible="False">Serial No</asp:Label>
                                        </td>
                                        <td align="left">
                                            <table id="Table1" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxSmall_Ajax" Visible="False"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="imgbtnModels" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                            Visible="False" CausesValidation="False" ToolTip="Click to Add Model"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Periods</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblPeriod" runat="server" CssClass="clsLabelAuto">Periods</asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="cmbPeriods" runat="server" CssClass="clsComboBox_Ajax" DataValueField="PeriodID"
                                                DataTextField="PeriodName">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="left">
                                            <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:UpdatePanel runat="server" ID="upnlDislaySearchCriteria" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td rowspan="4">
                                                                <asp:Label ID="lblDummyLabel" runat="server" CssClass="clsLabelAuto" Visible="False" Text="*"></asp:Label>
                                                            </td>
                                                            
                                                            <td colspan="2">
                                                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False">Date Range :</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblGraphGroupType1" runat="server" CssClass="clsLabelAuto" Visible="False">Graph Group type :</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblAssemblyType1" runat="server" CssClass="clsLabelAuto" Visible="False">Assembly Type :</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False">Model : </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False">Serial No :</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                        <td colspan="4" align="right">
                                            <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                                        Text="Current Criteria" ToolTip="Click to display Current Searching criterias.">
                                                                    </asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                        Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                        CausesValidation="False" ToolTip="Back to Previous Page"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForDuePeriod_AJAX.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForDuePeriod_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Due Periodwise Report</title>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1">
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Search criteria for Due</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                    InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                    ControlToValidate="txtFromDate" Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                    ErrorMessage="As On Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                    Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" ErrorMessage="Aircraft Required."
                                    ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="validateAircraft"
                                    CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function validateAircraft(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbAircraft");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of As On Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">As On Date</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
                                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                    AutoPostBack="True"></asp:TextBox>
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
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft & Period</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"  AutoPostBack="True"
                                                        DataTextField="RegNo" DataValueField="MachineID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPeriod" runat="server" CssClass="clsLabelAuto">Period</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPeriod" runat="server"   AutoPostBack="True"
                                                        DataTextField="PeriodName" DataValueField="PeriodID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step III. Selection of Due Limits / Percentage Life Remaining</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlDueLimits" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rbdDueLimits" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                                    GroupName="StepIII" Font-Bold="True" Text="Due Limits" Checked="True"></asp:RadioButton>
                                                            </td>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rbdPercent" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                                    GroupName="StepIII" Font-Bold="True" Text="Percent Life Remaining"></asp:RadioButton>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtPercentage" runat="server"  MaxLength="4" Width ="90px"
                                                                    ToolTip="Enter Percentage" Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                                        <asp:GridView ID="gdvDuePeriodLimits" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal"
                                                            CssClass="clsGridNewStyle">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Limit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLimit" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value." Width ="200px">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvPeriodLimitsValue" runat="server" ControlToValidate="txtLimit"
                                                                            Display="None" ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Estimated Flying Hours.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">(For Estimated Due-Dates Calculation)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlAvrgperiod" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rbdAvrageMonths" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                        GroupName="StepIV" Font-Bold="True" Text="Average in Months" Checked="True">
                                                    </asp:RadioButton>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="rbdSpecifyValues" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                        GroupName="StepIV" Font-Bold="True" Text="Specify Values"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAvgMnths" runat="server" CssClass="clsLabelAuto">Average for last</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAvgMnths" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="4"
                                                        ToolTip="Enter Average Months" Width="90px"></asp:TextBox>
                                                    <asp:Label ID="lblMonths" runat="server" CssClass="clsLabelAuto">Months</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Visible="False">Enter per day Values of Following Periods</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Panel ID="pnlAvragePeriod" runat="server" CssClass="clspanel1" Visible="False">
                                                        <asp:GridView ID="gdvPerDayLimit" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="PeriodID" HeaderText="PeriodID" Visible="False" />
                                                                <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Limit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLimitPerDay" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width ="200px" Height ="20px"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvPeriodLimitsValuePerDay" runat="server" ControlToValidate="txtLimitPerDay"
                                                                            Display="None" ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAvgMnths1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                        Text="Current Criteria" ToolTip="Click to display Current Searching criterias."
                                                        ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" Text="Display"
                                                        ToolTip="Click to Display Report" ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                        Text="Close" ToolTip="Back to Previous Page" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <table cellspacing="0">
                                </table>
                            </td>
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
    </form>
</body>
</html>

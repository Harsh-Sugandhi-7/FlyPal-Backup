<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSummaryofDirectiveStatus_AJAX.aspx.vb"
    Inherits="Flypal.wfSummaryofDirectiveStatus_AJAX" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Summary of Directive Status</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="5" class="clsFormHeader1">
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Summary of Directive Status</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please Select the Aircraft."
                                                ControlToValidate="cmbAircraft" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please Select the Directive."
                                                ControlToValidate="cmbType" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="As On Date Required" ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of As On Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblAsOnDate" runat="server" CssClass="clsLabelAuto">As On Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
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
                                    <asp:Label ID="lblRange" runat="server" CssClass="clsLabelAuto" Visible="False">Range</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False"
                                        Width="208px">
                                        <asp:ListItem Value="0">Between 0 Days - 1 Month</asp:ListItem>
                                        <asp:ListItem Value="1">Between 0 Days - 2 Month</asp:ListItem>
                                        <asp:ListItem Value="2">Between 0 Days - 3 Month</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlAircraft" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="MachineID"
                                                DataTextField="RegNo" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                DataTextField="ModelSerialNoPostion">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Directive</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTypeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblType" runat="server" CssClass="clsLabelAuto" Width="48px">Directive</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                        DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblTypeOC" runat="server" CssClass="clsLabelAuto" Visible="False">Type</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cnbAdType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">Opened</asp:ListItem>
                                        <asp:ListItem Value="2">Closed</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelHeader">Step V. Selection of format</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelAuto">Format</asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optSummary" runat="server" CssClass="clsRadioButton" Text="Summary"
                                        Checked="True" GroupName="a"></asp:RadioButton>
                                    <asp:RadioButton ID="optDetail" runat="server" CssClass="clsRadioButton" Text="Detail"
                                        GroupName="a"></asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step VI. Bottom Line of Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelAuto">Enter Line which you want to print at the bottom of the report.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultilineDefectAction"
                                        Width="552px" ToolTip="Enter Note" MaxLength="500" TextMode="MultiLine">Disclaimer : This list excludes additional information contained in the Detailed Status Report and AD itself that may require additional action from a new operator.</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step VII. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReportType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="5">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" align="right" class="clstableButton">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                            CssClass="clsbtnH" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH" ToolTip="Click to Export report"
                                                        Width="140px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" Text="Display"
                                                            ToolTip="Click to Display Report" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH" Text="Close" ToolTip="Click to close "
                                                            CausesValidation="false" />
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
    </form>
</body>
</html>

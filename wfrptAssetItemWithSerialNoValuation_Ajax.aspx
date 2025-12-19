<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAssetItemWithSerialNoValuation_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAssetItemWithSerialNoValuation_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Asset Valuation</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript">
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
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <span id="lbltitle" class="clstitle1">Asset Valuation For Rotables/Tools</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <td width="96px">
                                                    <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                        <asp:ListItem Value="1">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <span id="lblFromDate" class="clsLabelAuto">From</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                        Visible="False" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="lblToDate" class="clsLabelAuto">To</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                        Visible="False" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlIssueSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span1" class="clsLabelHeader">Step II.Selection of Model</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="spanModel" class="clsLabel">Model</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="ModelName"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="chkCommonOrApplicability" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                            Text="Common/No Applicability" ToolTip="Common/No Applicability" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep6" class="clsLabelHeader">Step III. Selection of Part Number/Description</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="lblSearch" class="clsLabel">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="False" CssClass="clsTextBoxRemark_Ajax"
                                                            Width="520px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span2" class="clsLabelHeader">Step IV. Selection of Store</span>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td width="96px">
                                                        
                                                    </td>
                                                    <td>
                                                       <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="lblStore" class="clsLabel">Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" TabIndex="6"
                                                            DataTextField="LocationStore" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span5" class="clsLabelHeader">Step V. Selection of Rotables/Tools</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="Span6" class="clsLabel">Rotables/Tools</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax">
                                                        <asp:ListItem Value="0">Both</asp:ListItem>
                                                        <asp:ListItem Value="1">Rotables</asp:ListItem>
                                                        <asp:ListItem Value="2">Tools</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span3" class="clsLabelHeader">Step VI Enter text to be dispaly at bottom line
                                                            of report</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="Span4" class="clsLabel">Text</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBottomLine" runat="server" AutoPostBack="False" CssClass="clsTextBoxRemark_Ajax"
                                                            Text='<%# " Submitted By : " + User.Identity.Name %>' Width="520px" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep9" class="clsLabelHeader">Landing Rate Is Considered In Asset Valuation
                                        For Rotables/Tools Calculation</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep4" class="clsLabelHeader">Step VII. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                            ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                            CausesValidation="False"></asp:Button>
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsButton_Ajax"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            Width="100px" ToolTip="Click to Export report" Text="Export to Excel" ValidationGroup="a">
                                                        </asp:Button>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to Display Report" ValidationGroup="a" Text="Display"></asp:Button>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Asset Valuation For Rotables/Tools screen"
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>

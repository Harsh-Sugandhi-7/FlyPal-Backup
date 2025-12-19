<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPendingToIssueAsLoanReturnToStore_Ajax.aspx.vb" EnableEventValidation="false"
    Inherits="Flypal.wfrptPendingToIssueAsLoanReturnToStore_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending To Issue As Loan Return To Store</title>
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
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
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
                            <td colspan="2">
                                <span id="lbltitle" class="clstitle1">Pending to issue as loan return to Store</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td width="85px">
                                                    <span id="lblDateRange" class="clsLabelAuto">Date Range</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ClientIDMode="Static" ID="cmbDateRange" CssClass="clsComboBox_Ajax"
                                                        runat="server" AutoPostBack="True">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of From Store</span>
                            </td>
                        </tr>
                        <tr>
                            <td width="96px">
                            </td>
                            <td>
                                <asp:Label ID="lblStoreCount" runat="server" class="clsLabelAuto" 
                                    Font-Bold="true" Font-Size="XX-Small" ForeColor="DarkBlue"></asp:Label>
                            </td>
                        </tr>
                         <tr>

                                                    <td width="85px">
                                                        <span ID="lblFromStore" class="clsLabel">From Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbFromStore" runat="server" CssClass="clsComboBox3_Ajax" 
                                                            DataTextField="LocationStore" DataValueField="Id" EnableViewState="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                        <tr>
                            <td colspan="2">
                                <span ID="lblStep" class="clsLabelHeader">Step III. Selection of To Store</span>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <span id="lblToType" class="clsLabel">To Store</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="LocationStore" EnableViewState="false"
                                    DataValueField="Id">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Part Number</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSearch" class="clsLabel">Search</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax" AutoPostBack="False"
                                    Width="520px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step V. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                <ContentTemplate>
                                       <table border="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblFromStore1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="lblVendor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close Purchase Order screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
            if ($get("cmbDateRange").selectedIndex === 0)
                return;
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
                if (elem.id = "txtFromDate") {
                    SetContextKey();
                }
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#txtSearch").autocomplete('wfAutoItemList.aspx?', {
                width: $("#txtSearch").outerWidth(),
                autoFill: false,
                matchContains: true,
                max: 50,
                delay: 0
            });
        });
    </script>
    </form>
</body>
</html>

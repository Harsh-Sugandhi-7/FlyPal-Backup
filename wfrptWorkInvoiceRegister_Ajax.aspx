<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptWorkInvoiceRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptWorkInvoiceRegister_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Work Invoice Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
       
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2">
                                <span id="lbltitle" class="clstitle1">Work Invoice Register</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                    ValidationGroup="a"></asp:ValidationSummary>
                                <script type="text/javascript">
                                    function showTextField() {
                                        var SearchIndex = $get("cmbDateRange").selectedIndex;

                                        var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                        var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                        var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                        var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                        if (SearchIndex == 0) {
                                            txtFromDateobj.style.display = 'none';
                                            txtToDateobj.style.display = 'none';
                                            lblFromDateobj.style.display = 'none';
                                            lblToDateobj.style.display = 'none';
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDateRange" class="clsLabelAuto">Date Range</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDateSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                        <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                        <asp:ListItem Value="Last Week">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="Last Month">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="Last Quarter">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="Last Year">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table2" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                    CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this);"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="Calender_watermarkextender" runat="server" TargetControlID="txtFromDate"
                                                                    WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                                    ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtToDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                    CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this);"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtToDate"
                                                                    WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                    ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
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
                            <td colspan="2">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Vendor</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblSupplier" class="clsLabelAuto">Vendor</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBox3_Ajax"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep7" class="clsLabelHeader">Step III. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias"
                                                        ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Work Invoice Register screen"
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            showTextField();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Vendor', {
                width: 275,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            var index;
            index = $get("cmbDateRange").selectedIndex;
            if (index == 6) {
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

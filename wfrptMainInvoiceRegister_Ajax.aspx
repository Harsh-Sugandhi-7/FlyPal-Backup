<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptMainInvoiceRegister_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptMainInvoiceRegister_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Maintenance Invoice Register</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table class="clstablelistin" id="tblInner" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Maintenance Invoice Register</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="85">
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto">Date Range</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                            Width="185px">
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
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender','true');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                            ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender','true');" ClientIDMode="Static"
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Maintenance Invoice No.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMainInvNoSelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="85">
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Main. Inv. No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbMaintenanceInvoiceText" runat="server" CssClass="clsComboBox_Ajax"
                                                            AutoPostBack="True" DataTextField="Text" DataValueField="Text" Width="185px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxMedium_Ajax" Visible="False"
                                                            MaxLength="4" Height="15px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step III. Selection of Supplier Invoice Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="274">
                                                <asp:Label ID="lblVendorInvDate" runat="server" CssClass="clsLabelAuto">Supplier Invoice Date</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFromInvDate" runat="server" CssClass="clsLabelAuto">From</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromInvDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromInvDate_watermarkextender','false');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="FromInvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromInvDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromInvDate" ID="FromInvDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                                <asp:CustomValidator ID="cvSuppDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Supplier Invoice From Date should not be greater than Supplier Invoice To Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblToInvDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToInvDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'ToInvDate_watermarkextender','false');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="ToInvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToInvDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToInvDate" ID="ToInvDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Supplier</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSupplierSelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="85">
                                                        <asp:Label ID="lblBy" runat="server" CssClass="clsLabelAuto">By</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbBy" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Supplier</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="75">
                                                        <asp:Label ID="lblSupplier" runat="server" CssClass="clsLabelAuto" Visible="False">Supplier</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBoxRemark_Ajax" Visible="False"
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Selection of Charge For</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="85">
                                                <asp:Label ID="lblChargesFor" runat="server" CssClass="clsLabelAuto">Charges For</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtChargesFor" runat="server" CssClass="clsTextBox_Ajax" MaxLength="75"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Part Number/Description</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="85">
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxRemark_Ajax" Width="520px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VII. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblMainInvNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblInvoiceDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblChargesForDisp" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias." />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                Text="Display" ToolTip="Click to Display Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                TabIndex="0" Text="Close" ToolTip="Click to close the Maintenance Invoice Register screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
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
            if (source.id == "cvSuppDate") {
                args.IsValid = false;

                var fromdate = $find('FromInvDate_watermarkextender').get_Text(); /*$("#txtFromInvDate").val();*/
                var todate = $find('ToInvDate_watermarkextender').get_Text(); /*$("#txtToInvDate").val();*/

                if (fromdate == "" || todate == "") {
                    args.IsValid = true;
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
            else if (source.id == "cvCommon") {
                var selectedDateIndex = $get("cmbDateRange").selectedIndex;
                if (selectedDateIndex == 6) {
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
            


        }

        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 302,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>

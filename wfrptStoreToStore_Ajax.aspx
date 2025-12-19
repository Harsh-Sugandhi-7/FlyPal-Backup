<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptStoreToStore_Ajax.aspx.vb"
    Inherits="Flypal.wfrptStoreToStore_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Store To Store Transfer</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript" language="javascript">
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
  
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
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
    <table class="clstablelistout" id="tblmain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" class="clsFormHeader1">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Store To Store Transfer (Loan)</asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rbLoanTrans" />
                                        <asp:AsyncPostBackTrigger ControlID="rbShowPlaneTransactions" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                            ErrorMessage="From Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                            ErrorMessage="To Date Required" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvStore" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="validateFromStore"
                                            ErrorMessage="Issuing Store Required.." Display="None" ControlToValidate="cmbFromStoreList"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvToStore" runat="server" CssClass="clsLabelAuto" ClientValidationFunction="validateToStore"
                                            ErrorMessage="Receiving Store Required..." Display="None" ControlToValidate="cmbToStoreList"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateFromStore(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbFromStoreList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                            function validateToStore(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbToStoreList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Label2" class="clsLabelHeader">Step I. Selection of Store</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                    Font-Bold="true" class="clsLabelAuto"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblIssueStore" class="clsLabel">(Loan) Issuing Store </span>
                            </td>
                            <td>
                                <table id="Table4" border="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFromStoreList" runat="server" 
                                                DataValueField="ID" DataTextField="LocationStore">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span id="LblReceiveAircraft" class="clsLabelAuto">(Loan) Receiving Store </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbToStoreList" runat="server" 
                                                DataValueField="ID" DataTextField="LocationStore">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step II. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDateRange" class="clsLabel">Date Range</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server"   AutoPostBack="True">
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
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
                                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_Watermarkextender');"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="FromDate_CalendarExtender" ClientIDMode="Static" runat="server"
                                                                    CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_Watermarkextender"
                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtToDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
                                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'ToDate_Watermarkextender');"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="ToDate_CalendarExtender" ClientIDMode="Static" runat="server"
                                                                    CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_Watermarkextender"
                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
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
                            <td colspan="2" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step III. Selection of Transaction</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlTransaction" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbLoanTrans" runat="server" CssClass="clsRadioButton" GroupName="x"
                                                        Checked="True" Text="Show loan transaction" onClick="Controlvisibility();"></asp:RadioButton>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbShowPlaneTransactions" runat="server" CssClass="clsRadioButton"
                                                        GroupName="x" Text="Show plain transaction" onClick="Controlvisibility();"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 23px">
                                                </td>
                                                <td style="height: 23px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsLoanIssued" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Show loan issued to other Store"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIssuedToStore" runat="server" CssClass="clsCheckBox" Text="Show issued to other Store.">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsLoanTaken" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Show loan received by another Store"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkReceivedByStore" runat="server" CssClass="clsCheckBox" Text="Show issued by another Store.">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsLoanReturn" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Show loan return by another Store"></asp:CheckBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsLoanGetBack" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Show receipts against loan issued"></asp:CheckBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Part Number/Description</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSearch" class="clsLabel">Search</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
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
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblIssueStore1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblReceiveStore1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblToDate1" runat="server" CssClass="clsLabelAuto"></asp:Label>
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
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                        Text="Current Criteria" ToolTip="Click to display current searching criterias">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH" ToolTip="Click to Export report"
                                                        Width="140px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                        Text="Display" ToolTip="Click to display report" ValidationGroup="valGroup1">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                        ToolTip="Click to Close Store to Store Transfer screen" CausesValidation="False">
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
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

        });
    </script>
    <script type="text/javascript">
        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
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
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                //                beforeSend: function (xhr, settings) {
                //                    $("[id$=processing]").dialog();
                //                },
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
    <script type="text/javascript">
        function Controlvisibility() {
            //if selected then enable and select checkbox else uncheck and disable list
            var status = $('#rbLoanTrans').attr('checked');
            if (status == "checked") {
                $('#chkIsLoanIssued').attr('checked', status);
                $('#chkIsLoanTaken').attr('checked', status);
                $('#chkIsLoanReturn').attr('checked', status);
                $('#chkIsLoanGetBack').attr('checked', status);

                $('#chkIsLoanIssued').removeAttr("disabled");
                $('#chkIsLoanTaken').removeAttr("disabled");
                $('#chkIsLoanReturn').removeAttr("disabled");
                $('#chkIsLoanGetBack').removeAttr("disabled");

                $('#chkIssuedToStore').removeAttr('checked');
                $('#chkReceivedByStore').removeAttr('checked');

                $('#chkIssuedToStore').attr('disabled', 'disabled');
                $('#chkReceivedByStore').attr('disabled', 'disabled');
                $('#lbltitle').text('Store to Store transfer (Loan)');
            }
            else {
                $('#chkIsLoanIssued').attr('disabled', 'disabled');
                $('#chkIsLoanTaken').attr('disabled', 'disabled');
                $('#chkIsLoanReturn').attr('disabled', 'disabled');
                $('#chkIsLoanGetBack').attr('disabled', 'disabled');

                $('#chkIsLoanIssued').removeAttr("checked");
                $('#chkIsLoanTaken').removeAttr("checked");
                $('#chkIsLoanReturn').removeAttr("checked");
                $('#chkIsLoanGetBack').removeAttr("checked");

                $('#chkIssuedToStore').removeAttr('disabled');
                $('#chkReceivedByStore').removeAttr('disabled');

                $('#chkIssuedToStore').attr('checked', true);
                $('#chkReceivedByStore').attr('checked', true);
                $('#lbltitle').text('Store to Store transfer');
            }


        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            Controlvisibility();
        });       
    </script>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptStoreToAircraft_Ajax.aspx.vb"
    Inherits="Flypal.wfrptStoreToAircraft_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Store To Aircraft Transfer</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Store to Aircraft Transfer</asp:Label>
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
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvstore" runat="server" CssClass="clslabelauto" ErrorMessage="Store Required"
                                            Display="None" ControlToValidate="cmbStore" ClientValidationFunction="validateStore"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvAircraft" runat="server" CssClass="clslabelauto"
                                            ErrorMessage="Aircraft Required" Display="None" ControlToValidate="txtAircraftList"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
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
                                        <script type="text/javascript">
                                            function validateStore(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbStore");
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
                                <span id="Label2" class="clsLabelHeader">Step I. Selection of Store & Aircraft</span>
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
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td width="70px">
                                            <span id="lblStore" class="clsLabel">Store</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server"  DataValueField="ID"
                                                DataTextField="LocationStore">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="45px">
                                            <span id="LblAircraft" class="clsLabelAuto">Aircraft</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAircraftList" runat="server" CssClass="clsTextBoxTagSearch"  ></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
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
                            <td width="70px">
                                <span id="lblDateRange" class="clsLabel">Date Range</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td width="275px">
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
                                                <td width="45px">
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
                                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="valGroup1"></asp:CustomValidator>
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
                            <td width="70px">
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlTransaction" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbLoanTrans" runat="server" CssClass="clsRadioButton" onClick="Controlvisibility();"
                                                        Checked="True" Text="Show loan transaction" GroupName="x"></asp:RadioButton>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbShowPlaneTransactions" runat="server" CssClass="clsRadioButton"
                                                        onClick="Controlvisibility();" Text="Show plain transaction" GroupName="x"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsStoreToAircraft" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Store to Aircraft"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsStoreToAircraftPlane" runat="server" CssClass="clsCheckBox"
                                                        Text="Store to Aircraft"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsAircraftToStore" runat="server" CssClass="clsCheckBox" Checked="True"
                                                        Text="Aircraft to Store"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsAircraftToStorePlane" runat="server" CssClass="clsCheckBox"
                                                        Text="Aircraft to Store"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Part Number</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="70px">
                                <span id="lblSearch" class="clsLabel">Search</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" Width="520px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
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
                                                    <asp:Label ID="lblStore1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                        Width="140px" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                        Text="Display" ToolTip="Click to display report" ValidationGroup="valGroup1">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                        ToolTip="Click to Close Store to Aircraft Transfer screen" CausesValidation="False">
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
            $("#<%=txtAircraftList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Aircraft', {
                width: 185,
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
                $('#chkIsStoreToAircraft').attr('checked', status);
                $('#chkIsAircraftToStore').attr('checked', status);

                $('#chkIsStoreToAircraft').removeAttr("disabled");
                $('#chkIsAircraftToStore').removeAttr("disabled");

                $('#chkIsStoreToAircraftPlane').removeAttr('checked');
                $('#chkIsAircraftToStorePlane').removeAttr('checked');

                $('#chkIsStoreToAircraftPlane').attr('disabled', 'disabled');
                $('#chkIsAircraftToStorePlane').attr('disabled', 'disabled');
                $('#lbltitle').text('Store to Aircraft transfer (Loan)');
            }
            else {
                $('#chkIsStoreToAircraft').attr('disabled', 'disabled');
                $('#chkIsAircraftToStore').attr('disabled', 'disabled');

                $('#chkIsStoreToAircraft').removeAttr("checked");
                $('#chkIsAircraftToStore').removeAttr("checked");

                $('#chkIsStoreToAircraftPlane').removeAttr('disabled');
                $('#chkIsAircraftToStorePlane').removeAttr('disabled');

                $('#chkIsStoreToAircraftPlane').attr('checked', true);
                $('#chkIsAircraftToStorePlane').attr('checked', true);
                $('#lbltitle').text('Store to Aircraft transfer');
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

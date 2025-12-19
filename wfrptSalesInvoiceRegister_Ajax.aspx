<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSalesInvoiceRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSalesInvoiceRegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Invoice Register</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">

                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Sales Invoice Register</asp:Label>
                                            </td>

                                            <%--<td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                            Text="Current Criteria" ToolTip="Click to display Current Searching criterias"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server"
                                                                            Text="Export to Excel" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" TabIndex="0" runat="server"
                                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                            ToolTip="Click to close the Sales Invoice Register screen" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>

                                        </tr>
                                    </table>

                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
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
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Sales Invoice Type</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="700px">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="80px">
                                                        <asp:Label ID="lblType" runat="server" CssClass="clsLabel">Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSalesInvoiceType" runat="server">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="23">Issue</asp:ListItem>
                                                            <asp:ListItem Value="74">None</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="700px">
                                    <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="80px">
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabel">Date Range</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                            <asp:ListItem Value="Last Week">Last Week</asp:ListItem>
                                                            <asp:ListItem Value="Last Month">Last Month</asp:ListItem>
                                                            <asp:ListItem Value="Last Quarter">Last Quarter</asp:ListItem>
                                                            <asp:ListItem Value="Last Year">Last Year</asp:ListItem>
                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="40px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
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
                                                    <td style="width: 19px">
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step III. Selection of Document & its Number</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDocType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td width="80px">
                                                        <asp:Label ID="lblDocType" runat="server" CssClass="clsLabel">Doc. Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDocType" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="0">None</asp:ListItem>
                                                            <asp:ListItem Value="1">Issue</asp:ListItem>
                                                            <asp:ListItem Value="2">Sales Invoice</asp:ListItem>
                                                            <asp:ListItem Value="3">Sales Order</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="75px">
                                                        <asp:Label ID="lblDocTypeNo" runat="server" CssClass="clsLabelAuto" Visible="False">Issue No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIssueTextList" runat="server" Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSalesOrderText" runat="server"  Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSalesInvoiceTextList" runat="server" 
                                                            Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtNo" runat="server"  Visible="False"
                                                            MaxLength="4" Columns="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Customer</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="80px">
                                                <asp:Label ID="Label5" runat="server" CssClass="clsLabel">Customer</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtCustomer" runat="server"
                                                   ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Selection of status & Report Format</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="80px">
                                                <asp:Label ID="lblStatus2" runat="server" CssClass="clsLabelAuto">Status</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server">
                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                    <asp:ListItem Value="1">Opened</asp:ListItem>
                                                    <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                    <asp:ListItem Value="4">Canceled</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDetail" runat="server" CssClass="clsCheckBox" Text="Detailed Report"
                                                    Checked="True"></asp:CheckBox>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="optPortrait" runat="server" CssClass="clsRadioButton" Text="Portrait"
                                                    Checked="True" GroupName="grOrientation"></asp:RadioButton>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="optLandscape" runat="server" CssClass="clsRadioButton" Text="Landscape"
                                                    GroupName="grOrientation"></asp:RadioButton>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="optWithGST" runat="server" CssClass="clsRadioButton" Text="With GST"
                                                    GroupName="grOrientation" Visible='<%# AppSettings("IsGSTApplicable")="True" %>'></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Part Number/Description</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="80px">
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server"  AutoPostBack="False"
                                                    ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step VII. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                                Text="Current Criteria" ToolTip="Click to display Current Searching criterias">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server" 
                                                                Text="Export to Excel" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"
                                                                Text="Display" ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                ToolTip="Click to close the Sales Invoice Register screen" CausesValidation="False">
                                                            </asp:Button>
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
        <asp:UpdatePanel ID="upnlHiddenFields" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <input type="hidden" id="hidden_DocType" runat="server" />
                <input type="hidden" id="hidden_DocTextType" runat="server" />
                <input type="hidden" id="hidden_FromType" runat="server" />
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
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtCustomer.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Customer', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtSalesOrderText.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=9', {
                width: 187,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtSalesInvoiceTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=11', {
                width: 187,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtIssueTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=3', {
                width: 187,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>

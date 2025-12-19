<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAdvanceCoreReturns_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAdvanceCoreReturns_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Advance Core Returns</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script id="clientEventHandlersJS" type="text/javascript">
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
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmrptPartHitory" runat="server">
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1">
                                <span class="clsFormHeader" id="lbltitle" >Advance Core Returns</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <span id="Span1" class="clsLabelHeader">Step I. Selection of Date</span>
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
                                                                        <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                                            <asp:ListItem Value="Last Week">Last 1 Week</asp:ListItem>
                                                                            <asp:ListItem Value="Last Month">Last 1 Month</asp:ListItem>
                                                                            <asp:ListItem Value="Last Quarter">Last 1 Quarter</asp:ListItem>
                                                                            <asp:ListItem Value="Last Year">Last 1 Year</asp:ListItem>
                                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table2" border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtFromDate" ClientIDMode="Static"
                                                                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
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
                                                                    <td>
                                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table3" border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox  CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtToDate" Style="margin-left: 3px;" 
                                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                                        runat="server"></asp:TextBox>
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
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <span id="Span2" class="clsLabelHeader">Step II. Selection of Suppliers</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="Span3" class="clsLabelAuto">Suppliers</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbSupplier" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID"
                                                        DataTextField="Name">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <span id="lblStep1" class="clsLabelHeader">Step III. Selection of Part Number/Description</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        CssClass="clsTextBoxTagSearchLong" onChange="SetPartIdonChange()"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="lblStep2" class="clsLabelHeader">Step IV. Selection of Serial No.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSerialNo" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        CssClass="clsTextBoxTagSearch" Visible="true">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                            CompletionInterval="1" ServicePath="wfrptUnderWarrantyItemList_Ajax.aspx" ServiceMethod="GetPartNoDescriptionList"
                                            TargetControlID="txtSearch" OnClientItemSelected="" UseContextKey="False" ContextKey=""
                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                            OnClientShowing="ClientShowing">
                                        </asp:AutoCompleteExtender>
                                        <asp:AutoCompleteExtender ID="txtSerialNo_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                            CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" ContextKey=""
                                            DelimiterCharacters="" EnableCaching="false" Enabled="True" MinimumPrefixLength="0"
                                            OnClientHiding="ClientHiding" OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
                                            OnClientShowing="ClientShowing" OnClientShown="ClientHiding" ServiceMethod="GetSerialNo"
                                            ServicePath="wfrptUnderWarrantyItemList_Ajax.aspx" TargetControlID="txtSerialNo"
                                            UseContextKey="True">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSerachCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                          <tr>
                                                <td>
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                    <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto">

                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server"
                                                        CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server"  Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        ToolTip="Click to Export report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False" 
                                                        TabIndex="0" Text="Close" />
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
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
</body>
</html>

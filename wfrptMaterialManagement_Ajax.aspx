<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptMaterialManagement_Ajax.aspx.vb"
    Inherits="Flypal.wfrptMaterialManagement_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Material Management For Lifed Items</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    </script>
    <style type="text/css">
        .TextBreak /* break word */ {
            white-space: -moz-pre-wrap !important; /* Mozilla, since 1999 */
            white-space: -pre-wrap; /* Opera 4-6 */
            white-space: -o-pre-wrap; /*  Opera 7 */
            white-space: pre-wrap; /*    css-3 */
            word-wrap: break-word; /*  Internet Explorer 5.5+ */
            word-break: break-all;
            white-space: normal;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
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
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1" colspan="2">
                                        <span class="clsFormHeader" id="lbltitle">Material Management For Lifed Items</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationSummary" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                    ValidationGroup="1" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" Display="None" InitialValue="<%$AppSettings:DateFormat%>"
                                                    ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="To Date should be greater than From Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter Part No."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                                    ValidationGroup="1" Display="None" ErrorMessage="Enter whole Part No. and Description"
                                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td></td>
                                                                    <td width="80px">
                                                                        <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtFromDate" ClientIDMode="Static"
                                                                                        ReadOnly="true" BackColor="Gainsboro" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                        Enabled="true" ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span1" class="clsLabelAuto">To Date</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtToDate" Style="margin-left: 3px;"
                                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                                                        runat="server"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="left">
                                                                        <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Part No.</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <span id="lblAircraft" class="clsLabelAuto">Part No.</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSearch" autocomplete="off" runat="server"
                                                                            ClientIDMode="Static" AutoPostBack="true"></asp:TextBox>
                                                                        <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                                            Text="With Alternate Part"></asp:CheckBox>
                                                                        <asp:CheckBox ID="chkIsValued" runat="server" Checked="True" CssClass="clsCheckBox"
                                                                            Text="Include Valued Stores Only" />
                                                                        <!-- AutoComplete Extender-->
                                                                        <cc2:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                                            ServicePath="" ServiceMethod="GetPartNoDescriptionList" TargetControlID="txtSearch"
                                                                            UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                            CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                        </cc2:AutoCompleteExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="left">
                                                                        <span id="Span2" class="clsLabelHeader">Step III. Selection of Aircraft</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="Span3" class="clsLabelAuto">Aircraft</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbAircraft" runat="server" ClientIDMode="Static"
                                                                            DataTextField="RegNo" DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="left">
                                                                        <span id="lblType" class="clsLabelHeader">Step IV. Estimated Flying Hours(Per Day)</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="1"></td>
                                                                                <td>
                                                                                    <asp:GridView CssClass="clsGridNewStyle" ID="gdPerDayLimit" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal"
                                                                                        ShowHeaderWhenEmpty="true">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="PeriodID" HeaderText="PeriodID" Visible="False" />
                                                                                            <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Limit">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px" ID="txtLimitPerDay" runat="server" BackColor="White"
                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
                                                                                                    </asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
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
                                        <span id="lblDisplayReport" class="clsLabelHeader">Step V. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblComponent1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblAC" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblEstimated" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server"
                                                                Text="Current Criteria" ToolTip="Click to display current searching criterias"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" Text="Display"
                                                                CausesValidation="true" ValidationGroup="1" ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" Text="Close" ToolTip="Click to close"
                                                                CausesValidation="False"></asp:Button>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartName" />
        <%-- Autocomplete functions to set id--%>
        <script type="text/javascript">
            function SetID(source, e) {
                //get id from autocomplete list
                var node;
                var IDValue = e.get_value();
                var NameValue = e.get_text();

                if (IDValue) node = e.get_item();
                else {
                    IDValue = e.get_item().parentNode._value;
                    node = e.get_item().parentNode;
                }
                //Set id to relevent hidden field 
                var textboxID, textboxName;
                if (source._id == "txtSearch_AutoCompleteExtender") {
                    textboxID = document.getElementById('PartID');
                    textboxName = document.getElementById('PartName');
                }
                textboxID.value = IDValue;
                textboxName.value = NameValue;

            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetPartIdonChange(source, extenderid) {
                var popup = $find(extenderid);
                var complist = popup.get_completionList();
                var text = $(source).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;

                        if (extenderid == "txtSearch_AutoCompleteExtender") {
                            textboxID = document.getElementById('PartID');
                        }
                        textboxID.value = val;
                        //textboxName.value = text;
                        return;
                    }

                }

                if (extenderid == "txtSearch_AutoCompleteExtender") {
                    document.getElementById('PartID').value = '';
                    //document.getElementById('PartName').value = '';
                }
            }

        </script>
        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations

            //From Date -To Date validation
            function BetweenDatesValidation(source, args) {
                args.IsValid = false;
                var fromdate = $find('FromDate_watermarkextender').get_Text(); // $("#txtFromDate").val();
                var todate = $find('ToDate_watermarkextender').get_Text(); // $("#txtToDate").val();

                if (fromdate == "" || todate == "") {
                    args.IsValid = true;
                    return;
                }

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

            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    data: params,
                    async: false,
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
        <script type="text/javascript">

            //Service list checking
            function ControlvisibilityForCheckboxlist(elem, childid) {
                //if selected then enable and select checkboxlist else uncheck and disable list
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#' + childid).removeAttr('disabled');
                }
                else {
                    $('#' + childid).attr('disabled', 'disabled');
                }

                $('#' + childid).find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        $(this).removeAttr('disabled');
                    }
                    else {
                        $(this).removeAttr("checked");
                        $(this).attr('disabled', 'disabled');
                    }
                });
            }

            //wo no checkbox status change event
            function ControlVisibilityForFormat(elem) {
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#cmbFormat').attr('disabled', 'disabled');
                    $('#cmbFormat').val('0');
                }
                else {
                    $('#cmbFormat').removeAttr('disabled');
                }
            }
        </script>
    </form>
</body>
</html>

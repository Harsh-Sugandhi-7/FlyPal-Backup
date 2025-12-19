<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPendingToIssueAsLoanReturnToCustomer_AJAX.aspx.vb"
    Inherits="Flypal.wfrptPendingToIssueAsLoanReturnToCustomer_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
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
                                <td>
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Pending to issue as loan return to Customer</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="6">
                                                        <span id="Span1" class="clsLabelHeader">Step I. Selection of Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100px">
                                                        <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDateRange" runat="server" AutoPostBack="True" CssClass="clsComboBox_Ajax">
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
                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                            ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                            ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                            ErrorMessage="To Date Required."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblStep2" runat="server" Class="clsLabelHeader">Step II. Selection of Customer</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100px">
                                                        <span id="lblFromStore" runat="server" class="clsLabel">From Customer</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSupplier" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                            DataValueField="Id">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep" runat="server" class="clsLabelHeader">Step III. Selection of Store</span>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td width="96px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                                            Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStore" runat="server" class="clsLabelAuto">Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="LocationStore"
                                                            DataValueField="Id">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Part Number/Description</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblSearch" runat="server" class="clsLabel">Search</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtPartDescription" autocomplete="off" runat="server" Width="520"
                                                            CssClass="clsTextBox1_Ajax" AutoPostBack="True" onchange="SetPartIdonChange(this,'txtPartDescription_AutoCompleteExtender')"></asp:TextBox>
                                                        <!-- AutoComplete Extender-->
                                                       <%-- <cc2:AutoCompleteExtender ID="txtPartDescription_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="" ServiceMethod="GetItemList" TargetControlID="txtPartDescription"
                                                            UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                                        </cc2:AutoCompleteExtender>--%>
                                                         <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtPartDescription_AutoCompleteExtender"
                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetItemList"
                                                            TargetControlID="txtPartDescription" OnClientItemSelected="SetID" UseContextKey="False"
                                                            ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                            OnClientShowing="ClientShowing">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
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
                                                    <td align="left">
                                                        <asp:Label ID="lblVendor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
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
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
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
                                                            TabIndex="0" Text="Close" ToolTip="Click to close the pending to issue as loan return to supplier screen " />
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
    <div>
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
    </div>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
    <%-- Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtPartDescription_AutoCompleteExtender") {
                textbox = document.getElementById('PartID');
            }
            textbox.value = value;
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

                    if (extenderid == "txtPartDescription_AutoCompleteExtender") {
                        textbox = document.getElementById('PartID');
                    }
                    textbox.value = val;
                    return;
                }

            }

            if (extenderid == "txtPartDescription_AutoCompleteExtender") {
                document.getElementById('PartID').value = '';
            }
        }
        
    </script>
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
    <%--Date Validations--%>
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
            var index;
            index = $get("cmbDateRange").selectedIndex;
            if (index == 6) {
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

        }
       
    </script>
    </form>
</body>
</html>

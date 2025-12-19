<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUCRComponentReliability_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptUCRComponentReliability_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>UCR(Unscheduled Component Removal) Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                                <span id="lbltitle" class="clstitle1">UCR (Unscheduled Component Removal) Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;">
                                <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                Width="100px" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                Width="100px" onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
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
                        <tr>
                            <td align="left" colspan="2">
                                <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Model and Serial No.
                                    from which Part is removed</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td align="left" style="width: 70px;">
                                            <asp:Label ID="lblModelNo" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtModelNo" runat="server" CssClass="clsTextBox_Ajax" onchange="SetModelIdonChange(this,'txtModel_Autocomplete')"
                                                ToolTip="Select Model."></asp:TextBox>
                                            <cc2:AutoCompleteExtender ID="txtModel_Autocomplete" runat="server" DelimiterCharacters=""
                                                Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                ServicePath="wfrptUCRComponentReliability_Ajax.aspx" ServiceMethod="GetModelList"
                                                TargetControlID="txtModelNo" UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                OnClientItemSelected="SetModelID">
                                            </cc2:AutoCompleteExtender>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                ToolTip="Enter Serial No."></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep6" class="clsLabelHeader">Step III. Selection of Supplier</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;">
                                <span id="lblCustomer" class="clsLabel">Supplier</span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCustomerList" runat="server" CssClass="clsTextBox_Ajax" Width="275px"></asp:TextBox>
                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtCustomerList_AutoCompleteExtender"
                                                runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetCustomerList"
                                                TargetControlID="txtCustomerList" UseContextKey="True" ContextKey=""
                                                CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                            </cc2:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Part </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <table>
                                    <tr>
                                        <td align="left" style="width: 70px;">
                                            <asp:Label ID="lblCPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtCPartNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                ToolTip="Enter Component Part No."></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblCSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtCSerialNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                ToolTip="Enter Component Serial No."></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Span2" class="clsLabelHeader">Step V. Selection of ATA</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;">
                                <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox2_Ajax"
                                                DataValueField="ID" DataTextField="ATAChapter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="Span1" class="clsLabelHeader">Step VI. Warranty and Percenatage Calculations</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;">
                                
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkWarranty" runat="server" CssClass="clsCheckBox" Text="Warranty"
                                                ToolTip="If checked Report will show only such Parts that are under Warranty" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkPercentage" runat="server" CssClass="clsCheckBox" Text="Percentage Comparisons" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep5" class="clsLabelHeader">Step VII. Display Report</span>
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
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRemovalFrom" runat="server" CssClass="clsLabelAuto" Visible="False">Removal From Model : </asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblModelNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRemovalof" runat="server" CssClass="clsLabelAuto" Visible="False">Removal Of : </asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblCPartNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblCSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                        CssClass="clsButtonLong_Ajax" Text="Current Criteria" ToolTip="Click to Display Current Searching criteria." />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="clsButtonLong_Ajax" Text="Export To Excel"
                                                        ToolTip="Click to Display Report" ValidationGroup="1" Visible="<%$AppSettings:ShowExportToExcelButton%>"/>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to Close" />
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
    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnModelId" runat="server" ClientIDMode="Static" />
    <%--
    Autocomplete functions to set id--%>
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
            if (source._id == "txtCustomerList_AutoCompleteExtender") {
                textbox = document.getElementById('hdnCustomerID');
            }

            textbox.value = value;
        }

        function SetModelID(source, e) {
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
            if (source._id == "txtModel_Autocomplete") {
                textbox = document.getElementById('hdnModelId');
            }
            textbox.value = value;
        }

        function SetModelIdonChange(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "txtModel_Autocomplete") {
                        textbox = document.getElementById('hdnModelId');
                    }
                    textbox.value = val;
                    return;
                }

            }

            if (extenderid == "txtModel_Autocomplete") {
                document.getElementById('hdnModelId').value = '';
            }
        }
    </script>
    <%--Date Validations--%>
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
    <script type="text/javascript">
        function validateCheckBox(source, args) {
            var IsInstallation = $get("chkInstallation").checked;
            var IsRemoval = $get("chkRemoval").checked;
            var IsCompliance = $get("chkCompliance").checked;

            args.IsValid = false;
            if (IsInstallation || IsRemoval || IsCompliance) {
                args.IsValid = true;

            }
        }
    </script>
    <script type="text/javascript">
        //wo no checkbox status change event
        function ControlTSICSIVisibility() {
            
            var IsInstallation = $get("chkInstallation").checked;
            var IsRemoval = $get("chkRemoval").checked;
            var IsRemUnschedule = $get("chkIsRemUnschedule").checked;
            var str="<%=System.Configuration.ConfigurationManager.AppSettings("ClientCode").ToString()%>";
          
            if (IsInstallation || IsRemoval || IsRemUnschedule) {
                if (str == "BA" || str=="YA" || str=="TA") {
                    $("#chkTSICSI").css('visibility', 'visible');
                    $("#chkTSICSI").next().css('visibility', 'visible');
                }
                else {
                    $("#chkTSICSI").css('visibility', 'hidden');
                    $("#chkTSICSI").next().css('visibility', 'hidden');
                    $("#chkTSICSI").removeAttr('checked');
                }
            }
            else {
                $("#chkTSICSI").css('visibility', 'hidden');
                $("#chkTSICSI").next().css('visibility', 'hidden');
                $("#chkTSICSI").removeAttr('checked');
            }
        }
        function ControlVisibility(elem) {
            var status = $(elem).attr('checked');
            if (status == "checked") {
                $("#chkIsRemUnschedule").css('visibility', 'visible');
                $("#chkIsRemUnschedule").next().css('visibility', 'visible');
             
            }
            else {
                $("#chkIsRemUnschedule").css('visibility', 'hidden');
                $("#chkIsRemUnschedule").next().css('visibility', 'hidden');
                $("#chkIsRemUnschedule").removeAttr('checked');
            }
            ControlTSICSIVisibility();
        }
    </script>
    </form>
</body>
</html>

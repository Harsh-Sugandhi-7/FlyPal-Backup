<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfProjectRegisterReport_Ajax.aspx.vb" Inherits="Flypal.wfProjectRegisterReport_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" Src="MSGBox.ascx" TagPrefix="msgBox" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="discrepancyRegisterReportForm" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <msgBox:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="mainDiv">
            <table id="mainTbl" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="contentTbl" class="clstablelistin">
                                <tr>
                                    <td colspan="4" class="clsFormHeader1Newstyle">
                                        <asp:Label CssClass="clsFormHeader lblHeader" runat="server"
                                            Text="Project Register">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlValidationErrors" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="vsDiscrepancyRegister" runat="server" ValidationGroup="DiscrepancyRegister"
                                                    CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields."></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="From Date is Required." ControlToValidate="txtFromDate" Display="None"
                                                    ValidationGroup="DiscrepancyRegister">
                                                </asp:RequiredFieldValidator>

                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="To Date is Required." ControlToValidate="txtToDate" Display="None"
                                                    ValidationGroup="DiscrepancyRegister">
                                                </asp:RequiredFieldValidator>

                                                <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ClientValidationFunction="BetweenDatesValidation" ValidationGroup="DiscrepancyRegister"
                                                    ErrorMessage="From Date should not be greater than To Date."></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep1" class="clsLabelHeader">Selection of Dates</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px" autocomplete="Off"
                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');">
                                        </asp:TextBox>
                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                    </td>
                                    <td>
                                        <span id="lblToDate" class="clsLabelAuto">To Date</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
                                            onchange="ValidateDateText(this,'ToDate_WatermarkExtender');" autocomplete="Off">
                                        </asp:TextBox>
                                        <cc2:CalendarExtender ID="ToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_WatermarkExtender"
                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep2" class="clsLabelHeader">Selection of Project No.</span>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <span id="lblProjectNo" class="clsLabelAuto">Project No.</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbProjectText" runat="server"
                                            CssClass="clsTextBoxTagSearchComboNewstyle"
                                            ClientIDMode="Static" DataTextField="Text" DataValueField="Text">
                                        </asp:DropDownList>



                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox runat="server" ID="txtNo" MaxLength="4" Width="40px"
                                            CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td colspan="4">
                                        <span id="lblStepReg" class="clsLabelHeader">Selection of Reg. No.</span>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabel">Reg. No.</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRegNo" TabIndex="26" runat="server" CssClass="clsTextBoxTagSearch"
                                            ToolTip="Enter Reg. No." autocomplete="off"></asp:TextBox>
                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="AutoCompleteExtender1" runat="server"
                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1000"
                                            ServicePath="wfProjectRegisterReport_Ajax.aspx" ServiceMethod="GetRegTextList" TargetControlID="txtRegNo"
                                            UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                            OnClientShowing="ClientShowing">
                                        </cc2:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="4">
                                        <span id="lblStepModel" class="clsLabelHeader">Selection of Model No.</span>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>

                                    </td>
                                     
                                    <td colspan="3">
                                        <asp:TextBox ID="txtModelNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            AutoPostBack="true" ToolTip="Enter Model No.">
                                        </asp:TextBox>
                                        <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelNo"
                                            ServiceMethod="GetModelNameList" MinimumPrefixLength="0" EnableCaching="true"
                                            CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing" ServicePath="wfProjectRegisterReport_Ajax.aspx">
                                        </cc2:AutoCompleteExtender>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="4">
                                        <span id="lblStepSerial" class="clsLabelHeader">Selection of Serial No.</span>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch"
                                            ToolTip="Enter Serial No.">
                                        </asp:TextBox>
                                    </td>
                                     
                                </tr>

                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep3" class="clsLabelHeader">Selection of Customer</span>
                                    </td>
                                </tr>

                                <tr>

                                    <td>
                                        <span id="lblCustomer" class="clsLabelAuto">Customer</span>

                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="cmbCustomer" runat="server"
                                            CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID" DataTextField="Name">
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep7" runat="server" class="clsLabelHeader">Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="false">
																Your Selections are as follows.
                                                            </asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaFromDate" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaToDate" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblSearchCriteriaCustomer" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaProjectText" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaProjectNo" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>



                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaRegNo" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaModel" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaSerialNo" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSearchCriteria" runat="server"
                                                                CssClass="clsbtnH clsinfoH1" Text="Current Criteria"
                                                                ToolTip="Click to Display Current Searching criterias" />
                                                        </td>

                                                        <td>
                                                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                ValidationGroup="DiscrepancyRegister" CausesValidation="true"
                                                                Text="Export To Excel" ToolTip="Click to Export Report" />
                                                        </td>

                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                ValidationGroup="DiscrepancyRegister" CausesValidation="true"
                                                                Text="Display" ToolTip="Click to Display Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False"
                                                                CssClass="clsbtnH clsinfoH1" Text="Close"
                                                                ToolTip="Click to close Project Register screen" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </form>

    <script type="text/javascript" src="modules/jquery/jquery-2.2.4.min.js"></script>

    <script id="clientEventHandlersJS" type="text/javascript">

        //#region Open Report Page

        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

        //Date Range Validation
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

        //Date Validation
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
</body>
</html>


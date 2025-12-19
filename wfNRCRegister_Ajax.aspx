<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRCRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfNRCRegister_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NRC Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="5400">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
            width="100%">
            <tr>
                <td>
                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                            AppSettings("ClientCode") = "AAP",
                                                                                                            "Defect Register",
                                                                                                            "NRC Register") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                            ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                            ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlNRCDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tblNRCDetail">
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepI" class="clsLabelHeader">Step I. Selection of Date</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblDate" class="clsLabelAuto">From</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                                    runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                                <%--<span id="Span1" class="clsLabelAuto">To</span>--%>
                                                            </td>
                                                            <td>
                                                                <span id="Span1" class="clsLabelAuto">To</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
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
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftList" runat="server"
                                                                    DataValueField="ID" DataTextField="RegNo">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblStepIII" runat="server" CssClass="clsLabelHeader" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                    AppSettings("ClientCode") = "AAP",
                                                                                                                                    "Step III. Selection of Reported By",
                                                                                                                                    "Step III. Selection of Raised By") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRaisedBy" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                    AppSettings("ClientCode") = "AAP",
                                                                                                                                    "Reported By",
                                                                                                                                    "Raised By") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRaisedBy" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                    AutoPostBack="true"  onChange="SetEmpIdonChange('txtRaisedBy','txtRaisedBy_Autocomplete')"></asp:TextBox>
                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtRaisedBy_Autocomplete" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                    CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtRaisedBy"
                                                                    OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                    OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                </cc2:AutoCompleteExtender>
                                                                <asp:HiddenField ID="hdnRaisedByEmpID" runat="server" ClientIDMode="Static" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepIV" class="clsLabelHeader">Step IV. Selection of ATA</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAChapter" runat="server"
                                                                    DataValueField="ID" DataTextField="ATAChapter">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepV" class="clsLabelHeader">Step V. Selection of Done By AME</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblDoneByAME" class="clsLabelAuto">AME</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtDoneByAME" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                    AutoPostBack="true" onChange="SetEmpIdonChange('txtDoneByAME','txtDoneByAME_Autocomplete')"></asp:TextBox>
                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByAME_Autocomplete" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                    CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByAME"
                                                                    OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                    OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                </cc2:AutoCompleteExtender>
                                                                <asp:HiddenField ID="hdnDoneByAMEID" runat="server" ClientIDMode="Static" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepVI" class="clsLabelHeader">Step VI. Selection of Done By Tech.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblDoneByTech" class="clsLabelAuto">Tech.</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtDoneByTech" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                    AutoPostBack="true" onChange="SetEmpIdonChange('txtDoneByTech','txtDoneByTech_Autocomplete')"></asp:TextBox>
                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtDoneByTech_Autocomplete" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                    CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtDoneByTech"
                                                                    OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                    OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                </cc2:AutoCompleteExtender>
                                                                <asp:HiddenField ID="hdnDoneByTechID" runat="server" ClientIDMode="Static" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStepVII" class="clsLabelHeader">Step VII. Enter Place</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblPlace" class="clsLabelAuto">Place</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtPlace" runat="server" ToolTip="Enter Place"
                                                                    MaxLength="50">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lblStepVIII" CssClass="clsLabelHeader" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                       AppSettings("ClientCode") = "AAP",
                                                                                                                                       "Step VIII. Enter Defect Reported",
                                                                                                                                       "Step VIII. Enter Observation") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblObservation" CssClass="clsLabelAuto" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                       AppSettings("ClientCode") = "AAP",
                                                                                                                                       "Defect Reported",
                                                                                                                                       "Enter Observation") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtObservation" runat="server" >
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblStepIX" runat="server" CssClass="clsLabelHeader" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                                AppSettings("ClientCode") = "AAP", "Step IX. Enter Rectification Action taken", "Step IX. Enter Rectification") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRectification" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                                                AppSettings("ClientCode") = "AAP", "Rectification Action taken", "Rectification") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRectification" runat="server">
                                                                </asp:TextBox>
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
                            <td>
                                <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFrom" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTo" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRaised" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblATAC" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAMEName" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTechName" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPlaceName" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblObser" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRec" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table id="Table8">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                        Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                        Text="Display" ToolTip="Click to display report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                        ToolTip="Click to Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
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
            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtRaisedBy_Autocomplete") {
                textbox = document.getElementById('hdnRaisedByEmpID');
            }
            if (source._id == "txtDoneByAME_Autocomplete") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (source._id == "txtDoneByTech_Autocomplete") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmpIdonChange(cntrl, extender) {
            var cntrlName = '#' + cntrl;
            var popup = $find(extender);
            var complist = popup.get_completionList();
            var text = $(cntrlName).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;
                    if (cntrl == "txtRaisedBy") {
                        var textbox = document.getElementById('hdnRaisedByEmpID');
                    }
                    if (cntrl == "txtDoneByAME") {
                        textbox = document.getElementById('hdnDoneByAMEID');
                    }
                    if (cntrl == "txtDoneByTech") {
                        textbox = document.getElementById('hdnDoneByTechID');
                    }
                    textbox.value = val.toString();
                    return;
                }
            }
            if (cntrl == "txtRaisedBy") {
                var textbox = document.getElementById('hdnRaisedByEmpID');
            }
            if (cntrl == "txtDoneByAME") {
                textbox = document.getElementById('hdnDoneByAMEID');
            }
            if (cntrl == "txtDoneByTech") {
                textbox = document.getElementById('hdnDoneByTechID');
            }
            textbox.value = '';
            return;
        }
    </script>
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
    </form>
</body>
</html>

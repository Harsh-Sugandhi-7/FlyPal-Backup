<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLicenceNoWiseMaintenanceActivityList_AJAX.aspx.vb"
    Inherits="Flypal.wfLicenceNoWiseMaintenanceActivityList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aircraft Maintenance</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
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
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel CssClass="clspanel1" ID="pnlmain" runat="server">
                            <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td class="clsFormHeader1" colspan="2">
                                                <asp:Label CssClass="clsFormHeader" ID="lbltitle" runat="server">Licence No. wise Work Done</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
                                                    Display="None" ControlToValidate="txtLicenceNo" ErrorMessage="Licence No. Required"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlFromDetails" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtFromDate" ClientIDMode="Static"
                                                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                                        ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" ID="txtToDate" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                        ClientIDMode="Static" runat="server"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Licence No.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="lblLicenceNo" runat="server" Width="72px" CssClass="clsLabelAuto">Licence No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtLicenceNo" runat="server" ToolTip="Enter License No. "
                                                    MaxLength="200"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step III. Selection of Activities</asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td></td>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="180px">
                                                            <asp:CheckBox ID="chkShowCompliance" runat="server" CssClass="clsCheckBox" Text="Show Compliance"
                                                                Checked="True" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkShowPirepsMELSnag" runat="server" CssClass="clsCheckBox" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Show Pireps/ADD/Defect","Show Pireps/MEL/Snag") %>'
                                                                Checked="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td align="left">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="180px">
                                                            <asp:CheckBox ID="chkInstallRemoval" runat="server" CssClass="clsCheckBox" Text="Show Install/Removal"
                                                                Checked="True" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkShowMaintActivity" runat="server" CssClass="clsCheckBox" Text="Show Maintenance Activity"
                                                                Checked="True" Visible="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Model</span>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <span id="lblStatus" class="clsLabel">Model</span>
                                            </td>

                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModelList" runat="server" ></asp:TextBox>
                                                <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                    CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                </cc2:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step V. Selection of Reg No.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="115px">
                                                <asp:Label ID="Label1" runat="server" Width="72px" CssClass="clsLabelAuto">Reg No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRegNo" runat="server" MaxLength="50"
                                                    ToolTip="Enter Reg.  No."></asp:TextBox>
                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="AutoCompleteExtender1" runat="server"
                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1000"
                                                    ServicePath="wfnrptWOSummary_Ajax.aspx" ServiceMethod="GetRegTextList" TargetControlID="txtRegNo"
                                                    UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                    OnClientShowing="ClientShowing">
                                                </cc2:AutoCompleteExtender>
                                            </td>

                                            </td>
                                        
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLicenceNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server" ClientIDMode="Static"
                                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                                </td>
                                                                <td align="right" colspan="1">
                                                                    <asp:Button CssClass="clsbtnH" ID="btnPrint" runat="server" CausesValidation="True"
                                                                        TabIndex="0" Text="Print" ToolTip="Click to Print" />
                                                                </td>
                                                                <td align="right" colspan="1">
                                                                    <asp:Button CssClass="clsbtnH" ID="btnBack" runat="server" CausesValidation="False"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
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
        </div>
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
        <script language="javascript" type="text/javascript">

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                onResize();
            });

        </script>
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtLicenceNo.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx', {
                width: 250,
                autoFill: false,
                matchContains: true,
                max: 20,
                delay: 0
            });
        });
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDayLogBookSummary_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfDayLogBookSummary_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Day Log Book Summary</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Day Log Book Summary</span>
                                        </td>
                                        <%--<td align="right" colspan="5">
                                            <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                                    CausesValidation="False" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ClientIDMode="Static" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                    TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report"
                                                                    ValidationGroup="a" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"
                                                                    Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                                    ToolTip="Click to receive Report through mail" ValidationGroup="a" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                    Text="Close" ToolTip="Click to close the Day Log Book Summary screen"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
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
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                            </td>
                            <td>
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
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
                                <span id="lblToDate" class="clsLabelAuto">To Date</span>
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
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                <span id="lblAircraft" class="clsLabelAuto">Aircraft </span>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataValueField="ID"
                                    ClientIDMode="Static" DataTextField="RegNo">
                                </asp:DropDownList>
                                <cc2:CascadingDropDown ID="csdAircraft" runat="server" TargetControlID="cmbAircraft"
                                    Category="Machine" ServiceMethod="GetAircraftList" LoadingText="Loading Aircraft..." ClientIDMode="Static"
                                    UseContextKey="True" ServicePath="wfDayLogBookSummary_Ajax.aspx"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftAssembly" runat="server"
                                    DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                </asp:DropDownList>
                                <cc2:CascadingDropDown ID="csdAssembly" LoadingText="Loading Assembly..." runat="server"
                                    ClientIDMode="Static" TargetControlID="cmbAircraftAssembly" ParentControlID="cmbAircraft"
                                    ServiceMethod="GetAssemblyList" Category="Assembly" UseContextKey="True"  ServicePath="wfDayLogBookSummary_Ajax.aspx"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="Label2" class="clsLabelHeader">Step IV. Selection of Flight Classification
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblFlightLogClassification" class="clsLabelAuto">Flight Log Classification</span>
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFlightLogClassification" runat="server"
                                    EnableViewState="false" DataValueField="ID" DataTextField="Name">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblStep4" class="clsLabelHeader">Step V. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" colspan="4">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblFlightLogClassification1" runat="server" CssClass="clsLabelAuto"
                                                        Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                        CausesValidation="False" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ClientIDMode="Static"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report"
                                                        ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"
                                                        Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                        ToolTip="Click to receive Report through mail" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server"  CausesValidation="False"
                                                        Text="Close" ToolTip="Click to close the Day Log Book Summary screen"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup 6-Sep-2016-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" colspan="2" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
    <!-- Popup For Report By Mail 6-Sep-2016-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
        PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReceipt1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgLogBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
    <script type="text/javascript">
        //Aircraft validation
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;

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
                if (elem.id == "txtFromDate") {
                    SetContextKey();
                }
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
        //Set context key(from date) for Assembly combobox
        function pageLoad() {
            SetContextKey();
        }
        function SetContextKey() {
            $find("csdAssembly")._contextKey = $get("txtFromDate").value;
        }
    </script>
</body>
</html>

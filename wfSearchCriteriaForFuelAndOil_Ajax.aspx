<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForFuelAndOil_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfSearchCriteriaForFuelAndOil_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Search criteria For fuel and Oil</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="4" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Search criteria for Fuel And Oil Register</span>
                                        </td>

                                        <%--<td align="right" colspan="4">
                                            <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                    TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server"
                                                                    Text="Export to Excel" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnMail" TabIndex="0" runat="server"
                                                                    Text="By Mail" ToolTip="Click to Mail report" Width="100px"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" TabIndex="0"
                                                                    Text="Display" ToolTip="Click to display report" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False"
                                                                    TabIndex="0" Text="Close" ToolTip="Click to Close Search criteria for Fuel and Oil Register" />
                                                            </td>
                                                        </tr>
                                                        <!--Dummy panel to open modelpopup-->
                                                        <tr style="height: 0px;">
                                                            <td style="height: 0px;" colspan="2" align="right">
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <!--End -->
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>

                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
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
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                <asp:CustomValidator ID="csvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="" ClientValidationFunction="ValidateChkList" ErrorMessage="Select at least one aircraft."></asp:CustomValidator>
                                <asp:CustomValidator ID="csvUnit" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="" ClientValidationFunction="ValidateUnit" ErrorMessage="Select Unit From List."></asp:CustomValidator>
                                <script type="text/javascript">
                                    function ValidateChkList(source, args) {
                                        var chk = document.getElementById('chkForAircraftWiseSummary').checked;
                                        var dd = $get("cmbUnitList");
                                        if (dd.selectedIndex != 0 && chk == true) {
                                            args.IsValid = false;
                                            $("#<%=ChklistAircraftWiseSummary.ClientID %>").find(":checkbox").each(function () {
                                                if ($(this).attr("checked")) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            });
                                        }
                                        else {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }

                                    function ValidateUnit(source, args) {

                                        var chk = document.getElementById('chkForAircraftWiseSummary').checked;
                                        var dd = $get("cmbUnitList");
                                        if (chk == true) {
                                            args.IsValid = false;
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;
                                            }

                                        }
                                        else {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblFromDate" class="clsLabelAuto">From Date</span>
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
                            </td>
                            <td>
                                <span id="lblToDate" class="clsLabelAuto">To Date</span>
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
                        <tr>
                            <td colspan="4" align="left">
                                <span id="Span1" class="clsLabelHeader">Step II. Aircraft Wise Summary</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel runat="server" ID="upnlChkForAircraftwiseSummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkForAircraftWiseSummary" runat="server" CssClass="clsCheckBox"
                                            AutoPostBack="true" TabIndex="3" Text="Aircraft Wise Summary" ToolTip="Select For Category Wise Report." />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlUnitwiseAircraft" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkUnit" runat="server" CssClass="clsCheckBox" Visible="false"
                                            AutoPostBack="true" TabIndex="3" Text="Unit" />
                                        <span id="Span2" class="clsLabelAuto">Unit</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlUnitList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbUnitList" runat="server" DataValueField="ID"
                                            AutoPostBack="true" DataTextField="Name" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel runat="server" ID="upnlAircraftWiseSummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkSelectAll" CssClass="clsLabelAuto" runat="server" AutoPostBack="true"
                                                        Visible="false" Text="All" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="ChklistAircraftWiseSummary" runat="server" CssClass="clsComboBox"
                                                        DataTextField="RegNo" DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal"
                                                        ToolTip="AircraftWiseSummary List" Visible="True" Width="500px">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step III. Selection of Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblAircraft" class="clsLabelAuto">Aircraft </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel runat="server" ID="upnlAircraft" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataValueField="ID"
                                            AutoPostBack="true" DataTextField="RegNo" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlTank" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltank" CssClass="clsLabelAuto" Text="Tank" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbTankList" runat="server" DataValueField="ID"
                                                        Visible="false" DataTextField="Name" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAssembly" CssClass="clsLabelAuto" Text="Assembly" runat="server"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataValueField="AssemblyStatusID"
                                                        Visible="false" DataTextField="ModelSerialNoPostion" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStepFormat" runat="server" CssClass="clsLabelHeader">Step IV.  Select Format of Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFormat" runat="server" CssClass="clsLabel">Format</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlFormat" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblStepIII" class="clsLabelHeader">Step V. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"
                                                        Text="Export to Excel" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                </td>
                                                 <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnMail" TabIndex="0" runat="server"
                                                        Text="By Mail" ToolTip="Click to Mail report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                        TabIndex="0" Text="Close" ToolTip="Click to Close Search criteria for Fuel and Oil Register" />
                                                </td>
                                            </tr>
                                             <!--Dummy panel to open modelpopup-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
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
        //Aircraft validation
        function ValidateAircraft(source, args) {
            args.IsValid = false;
            var dd = $get("cmbAircraft");
            var chk = document.getElementById('chkForAircraftWiseSummary').checked;
            if (dd.selectedIndex != 0 || chk == true) {
                args.IsValid = true;
                return;

            }

        }
    </script>
     <!-- Popup For Report By Mail -->
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
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>

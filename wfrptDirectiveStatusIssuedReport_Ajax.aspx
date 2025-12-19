<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptDirectiveStatusIssuedReport_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptDirectiveStatusIssuedReport_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Directive Issued Register</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
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
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Directive Issued Register</span>
                                        </td>

                                        <%--<td align="right" colspan="6">
                                            <asp:UpdatePanel ID="upnlBtns" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                    TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                    Text="Export to Excel" ToolTip="Click to Export report"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" TabIndex="0"
                                                                    Text="Display" ToolTip="Click to Display Report" />
                                                            </td>
                                                            <%-- 'Added by Shital on 18-Dec-2019
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnByMail" runat="server" Text="Send Mail"
                                                                    ToolTip="Click to receive Report through mail" />
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False"
                                                                    Text="Close" ToolTip="Click to Close For Directive Status screen" />
                                                            </td>
                                                        </tr>
                                                        <!-- Dummy panel to open modelpopup 'Added by Shital on 18-dec-2019 -->
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
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>

                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
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
                                            ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ClientValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAssembly" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please Select the Assembly."
                                            ControlToValidate="cmbAssembly" Display="None" ClientValidationFunction="ClientValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Please Select the Directives." ControlToValidate="cmbAdType" Display="None"
                                            OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Effective Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span id="Label3" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                <span id="lblDirective" class="clsLabelAuto">From Date</span>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"  style="height:25px; "
                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                            </td>
                            <td align="left">
                                <span id="Label2" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="Label1" class="clsLabelAuto">To Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="height: 25px;" onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                runat="server" CausesValidation="true"></asp:TextBox>
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
                            <td colspan="5">
                                <span id="Label6" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                            </td>
                            <td colspan="3" align="left">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataValueField="ID"
                                    ClientIDMode="Static" DataTextField="RegNo">
                                </asp:DropDownList>
                                <cc2:CascadingDropDown  ID="csdAircraft" runat="server" TargetControlID="cmbAircraft"
                                    Category="Machine" ServiceMethod="GetAircraftList" LoadingText="Loading Aircraft..."
                                    UseContextKey="True" ServicePath="wfrptDirectiveStatusIssuedReport_Ajax.aspx"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="Label7" class="clsLabelHeader">Step III. Selection of Assembly</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span id="Label4" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                            </td>
                            <td colspan="3" align="left">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbAssembly" runat="server" DataValueField="ID"
                                    DataTextField="ModelSerialNoPostion">
                                </asp:DropDownList>
                                <cc2:CascadingDropDown ID="csdAssembly" LoadingText="Loading Assembly..." runat="server"
                                    TargetControlID="cmbAssembly" ParentControlID="cmbAircraft" ServiceMethod="GetAssemblyList"
                                    Category="Assembly" UseContextKey="True" ServicePath="wfrptDirectiveStatusIssuedReport_Ajax.aspx"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="Label8" class="clsLabelHeader">Step IV. Selection of Directive Type</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span id="Label5" class="clsLabelStar">*</span>
                            </td>
                            <td align="left">
                                <span id="lblType" class="clsLabelAuto">Directive</span>
                            </td>
                            <td colspan="3" align="left">
                                <%--<asp:DropDownList ID="cmbModificationType" runat="server" CssClass="clsComboBox3_Ajax"
                                    DataValueField="ID" DataTextField="Name">
                                </asp:DropDownList>--%>
                                <asp:Panel ID="pnlDirectiveType" runat="server">
                                    <asp:UpdatePanel runat="server" ID="upnlDirectiveType" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                AutoPostBack="true" DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="Span2" class="clsLabelAuto">Type</span>
                            </td>
                            <td>
                                <asp:Panel ID="Panel1" runat="server">
                                    <asp:UpdatePanel runat="server" ID="upnlDirectiveSubType" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ListBox ID="ListDirectiveSubType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="Span3" class="clsLabelHeader">Step V. Selection of Open or Closed</span>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblTypeOC" class="clsLabelAuto">Type</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAdType" runat="server">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Open</asp:ListItem>
                                    <asp:ListItem Value="2">Closed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <span id="lblSortBy" class="clsLabelHeader">Step VI. Selection of Sorting Criteria</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblSort" class="clsLabelAuto">Sort By</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSortBy" runat="server">
                                    <asp:ListItem Value="0" Selected="True">Directive No.</asp:ListItem>
                                    <asp:ListItem Value="1">Issue Date</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>


                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="optAscending" runat="server" CssClass="clsRadioButton" Text="Ascending"
                                                Checked="True" GroupName="grOrientation"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="optDescending" runat="server" CssClass="clsRadioButton" Text="Descending"
                                                GroupName="grOrientation"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>


                            </td>
                        </tr>
                          <asp:PlaceHolder ID="PlaceHolder4" runat="server" >
                          <tr>
                                        <td colspan="6">
                                            <span id="Span4" class="clsLabelHeader">Step VI. Selection of Format</span>
                                        </td>
                                    </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                               <span id="lblFormat" class="clsLabelAuto">Format</span>
                            </td>
                            <td>
                               <asp:DropDownList ID="cmbFormat" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="true">
                                    <asp:ListItem Value="0">Format 1</asp:ListItem>
                                    <asp:ListItem Value="1">Format 2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td colspan="5">
                                <span id="lblStep5" class="clsLabelHeader">Step VII. Bottom Line of Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="Span1" class="clsLabelAuto">Enter Line which you want to print at the bottom
                                    of the report.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax" height="50px"
                                    Width="552px" MaxLength="500" TextMode="MultiLine" ToolTip="Enter Note">I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <span id="lblStep6" class="clsLabelHeader">Step VIII. Display Report</span>
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
                                        <table cellspacing="0" border="0" width="100%">
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
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:UpdatePanel ID="upnlBtns" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                         TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        Text="Export to Excel" ToolTip="Click to Export report" ></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                        Text="Display" ToolTip="Click to Display Report" />
                                                </td>
                                                <%-- 'Added by Shital on 18-Dec-2019 --%>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Send Mail"
                                                        ToolTip="Click to receive Report through mail" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False" 
                                                        Text="Close" ToolTip="Click to Close" />
                                                </td>
                                            </tr>
                                            <!-- Dummy panel to open modelpopup 'Added by Shital on 18-dec-2019 -->
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
    <script type="text/javascript">
        //Aircraft validation
        function ClientValidation(source, args) {
            args.IsValid = false;
            var dd = $get(source.controltovalidate);
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
                if (elem.id = "txtFromDate") {
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
    <!-- Popup For Report By Mail 14-Sep-2016-->
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
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DirectiveMultiSelect() {
            $('[id*=ListDirectiveType],[id*=ListDirectiveSubType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: '(Select)',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directives',
                nSelectedText: 'Directives'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');


        }

        function disableDirectiveSubType() {

            $('[id*=ListDirectiveSubType]').multiselect('clearSelection', true);
            $('[id*=ListDirectiveSubType]').multiselect('disable', false);

        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            DirectiveMultiSelect();
        });
    </script>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompliedMaintenanceDirectiveMatrix_AJAX.aspx.vb" Inherits="Flypal.wfCompliedMaintenanceDirectiveMatrix_AJAX" %>

<!DOCTYPE html>
<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.ModelMonitorModTypeList" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compliance Matrix</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
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
    <style type="text/css">
        .style1 {
            height: 26px;
        }

        .btn {
            padding: 1px;
            font-size: 8pt;
        }

        .TextBox {
            box-sizing: Content-box;
        }

        .label {
            font-weight: normal !important;
        }

        .required:before {
            content: "*";
            font-weight: bold;
            font-size: small;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="4" class="clsFormHeader1Newstyle">
                                        <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Compliance Matrix</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAircraft"
                                                    OnServerValidate="CustomValidate" Display="None" ErrorMessage="Please Select the Aircraft."></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                    InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                                    ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                                    ErrorMessage="As On Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                                    Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                    ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please Select the Directive."
                                                    ControlToValidate="cmbType" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Effective Date</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td>
                                        <span id="lblFromDate" runat="server" class="clsLabelAuto">From Date</span>
                                    </td>
                                    <td>
                                        <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" onchange="ValidateDateText(this)" Height="25px"></asp:TextBox>
                                                    <cc2:calendarextender id="calFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                        enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate">
                                                    </cc2:calendarextender>
                                                    <cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="FromDate_watermarkextender"
                                                        clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                        watermarkcssclass="clsDateTextBox">
                                                    </cc2:textboxwatermarkextender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                        onchange="ValidateDateText(this);" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" Height="25px"></asp:TextBox>
                                                    <cc2:calendarextender id="calToDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                        enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtToDate">
                                                    </cc2:calendarextender>
                                                    <cc2:textboxwatermarkextender targetcontrolid="txtToDate" id="ToDate_watermarkextender"
                                                        clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                        watermarkcssclass="clsDateTextBox">
                                                    </cc2:textboxwatermarkextender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <span id="lblStep2" runat="server" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblAircraftStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td align="left">
                                        <span id="lblAircraft" runat="server" class="clsLabelAuto">Aircraft</span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnlAircraft" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                    DataTextField="RegNo" DataValueField="ID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="Label3" runat="server" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left">
                                        <span id="lblAssembly" runat="server" class="clsLabelAuto">Assembly</span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataTextField="ModelSerialNoPostion"
                                                    DataValueField="ID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep4" runat="server" class="clsLabelHeader">Step IV. Selection of Type</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTypeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblType" runat="server" CssClass="clsLabelAuto" Width="48px">Directive</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                            DataTextField="Name">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkApplicable" runat="server" visible="false" CssClass="clsLabelAuto"></asp:CheckBox>
                                       <%-- <span class="clsLabel">Show ONLY "APPLICABLE" records</span>--%>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep7" runat="server" class="clsLabelHeader">Step V. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <span id="lblSummary" runat="server" class="clsLabelAuto">Your selection is as follows
                                     :</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table7" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3"></td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" CausesValidation="true"
                                                                TabIndex="0" Text="Display" ToolTip="Click to Display Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByExcel" runat="server" TabIndex="25"
                                                                Text="Export to Excel" ToolTip="Click to Export to Excel" ValidationGroup="1" Visible="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to close Search criteria for screen " />
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
                        //        contentType: "application/json",
                        cache: false,
                        data: params,
                        async: false,
                        beforeSend: OnBeforeSend,
                        //                beforeSend: function (xhr, settings) {
                        //                    $("[id$=processing]").dialog();
                        //                },
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
        </div>
    </form>
</body>
</html>

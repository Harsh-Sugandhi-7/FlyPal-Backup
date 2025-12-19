<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportMaintActivitiesToExcel.aspx.vb"
    Inherits="Flypal.wfExportMaintActivitiesToExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.ModelMonitorModTypeList" %>
<%@ Import Namespace="Flypal.PartMonitorServiceTypeList" %>
<%@ Import Namespace="Flypal.ModelMonitorInspTypeList" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        .style1
        {
            height: 26px;
        }
        .btn
        {
            padding: 1px;
            font-size: 8pt;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
        .label
        {
            font-weight: normal !important;
        }
        .required:before
        {
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
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Export Maintenance Activities</asp:Label>
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


                                           <%-- <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Aircraft from the list."
                                            ControlToValidate="cmbAircraft" Display="None"></asp:CustomValidator>--%>

                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                                ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator I="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of As On Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                </td>
                                <td>
                                    <span id="lblFromDate" runat="server" class="clsLabelAuto">As On Date</span>
                                </td>
                                <td>
                                    <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static" Height="24px"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
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
                                <td colspan="4" align="left">
                                    <span id="Label3" runat="server" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="left">
                                    <span id="lblAssembly" runat="server" class="clsLabelAuto">Assembly</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataTextField="ModelSerialNoPostion"
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="chkShowAssembly" ClientIDMode="Static" runat="server" GroupName="b"  CssClass="clsLabelAuto" Checked="true"
                                                    Text="Show Assembly" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="chkShowComponent" ClientIDMode="Static" runat="server" GroupName="b"  CssClass="clsLabelAuto" 
                                                    Text="Show Component" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <span id="lblStep4" runat="server" class="clsLabelHeader">Step IV. Selection of Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <table id="Table1" border="0">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkService" ClientIDMode="Static" runat="server" GroupName="a" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkInspection" ClientIDMode="Static" runat="server" GroupName="a"
                                                                Text="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chkDirective" ClientIDMode="Static" runat="server" GroupName="a"
                                                                Text="" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlPercentLife" runat="server" UpdateMode="Conditional" >
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Visible="false" ></asp:CheckBox>
                                            <span class="clsLabel"  style="display:none">Show ONLY "APPLICABLE" records</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <span id="lblSummary" runat="server" class="clsLabelAuto">Your selection is as follows
                                        :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
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
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                </td>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByExcel" runat="server" TabIndex="25"
                                                            Text="Export to Excel" ToolTip="Click to Export to Excel" 
                                                            />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close" />
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
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
        <%--<script type="text/javascript">


            $("#chkService").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');

            });
            $("#chkInspection").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');
                $('#chkService').removeAttr('disabled');

            });
            $("#chkDirective").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').removeAttr('disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').removeAttr('disabled');

            });
            $("#chkSnag").live("click", function () {
                var status = $(this).attr('checked');
                if (status)
                    ControlvisibilityForCheckboxlist();
                $('#chkPercentLife').removeAttr("checked");
                $('#chkPercentLife').attr('disabled', 'disabled');
                $('#txtPercentage').attr('disabled', 'disabled');
                $('#txtPercentage').val('');
                $('#chkApplicable').attr('disabled', 'disabled');
            });


            //            $(document).ready(function () {
            //                Controlvisibility($("#chkService"), "ListServiceType");
            //                Controlvisibility($("#chkInspection"), "ListInspectionType");
            //                Controlvisibility($("#chkDirective"), "ListDirectiveType");
            //            });

            //Service/inspection/Directive list checking
            function Controlvisibility(elem, childid) {
                //if selected then enable and select checkboxlist else uncheck and disable list
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    //  $('#' + childid).removeAttr('disabled');
                    $('#' + childid).prop('disabled', false);

                }
                else {
                    $('#' + childid).prop('disabled', true);
                }
                //                $('#' + childid).find(":Checkbox").each(function () {
                //                    if (status == "checked") {
                //                        $(this).attr("checked", status);
                //                        $(this).removeAttr('disabled');
                //                    }
                //                    else {
                //                        $(this).removeAttr("checked");
                //                        $(this).attr('disabled', 'disabled');
                //                    }
                //                });
            }

            function ControlvisibilityForCheckboxlist() {
                //  Controlvisibility($("#chkService"), "ListServiceType");
                //  Controlvisibility($("#chkInspection"), "ListInspectionType");
                //  Controlvisibility($("#chkDirective"), "ListDirectiveType");

            }
        </script>--%>
    </div>
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">

        $("#chkService").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
            $('[id*=ListServiceType]').multiselect('selectAll', false);
            $('[id*=ListServiceType]').multiselect('updateButtonText');

            $('[id*=ListInspectionType]').multiselect('clearSelection', true);            //Clears all selected items in Inspection list & update button Text
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });

        $("#chkInspection").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListInspectionType]').multiselect('enable', true);
            $('[id*=ListInspectionType]').multiselect('selectAll', false);
            $('[id*=ListInspectionType]').multiselect('updateButtonText');

            $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListDirectiveType]').multiselect('disable', false);
            $('[id*=ListServiceType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });
        $("#chkDirective").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListDirectiveType]').multiselect('enable', true);
            $('[id*=ListDirectiveType]').multiselect('selectAll', false);
            $('[id*=ListDirectiveType]').multiselect('updateButtonText');

            $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListInspectionType]').multiselect('clearSelection', true);

            $('[id*=ListServiceType]').multiselect('disable', false);
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', false);

        });

        $("#chkSnag").live("click", function () {
            var status = $(this).attr('checked');
            if (status)

                $('[id*=ListServiceType]').multiselect('clearSelection', true);
            $('[id*=ListInspectionType]').multiselect('clearSelection', true);
            $('[id*=ListDirectiveType]').multiselect('clearSelection', true);

            $('[id*=ListServiceType]').multiselect('disable', false);
            $('[id*=ListInspectionType]').multiselect('disable', false);
            $('[id*=ListDirectiveType]').multiselect('disable', false);
            $('#cmbFormat').attr('disabled', 'disabled');
            $('#cmbFormat').val('0')
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var ServStatus = document.getElementById("chkService");
                    $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListInspectionType]').multiselect('disable', false);

                    if (ServStatus.checked) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');

                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Services',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Services',
                nSelectedText: 'Services'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');

        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListDirectiveType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var DirStatus = document.getElementById("chkDirective");
                    $('[id*=ListServiceType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListInspectionType]').multiselect('disable', false);

                    if (DirStatus.checked) {
                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');

                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Directive',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directive',
                nSelectedText: 'Directive'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListInspectionType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var Inspstatus = document.getElementById("chkInspection");
                    $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                    $('[id*=ListServiceType]').multiselect('disable', false);
                    if (Inspstatus.checked) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');

                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');

                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Inspection',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Inspection',
                nSelectedText: 'Inspection'


            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var Serstatus = document.getElementById("chkService");
            if (Serstatus.checked) {

                $('[id*=ListDirectiveType]').multiselect('disable', false);                   //* Disable the multiselect ListBOx 
                $('[id*=ListInspectionType]').multiselect('disable', false);
            }

        });
    </script>
</body>
</html>

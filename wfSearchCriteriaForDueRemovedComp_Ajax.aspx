<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForDueRemovedComp_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfSearchCriteriaForDueRemovedComp_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Due Periodwise Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
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
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
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
    </style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Removed Components Due Report</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                                ControlToValidate="txtFromDate" Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                                ErrorMessage="As On Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                                Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateSelection(source, args) {
                                                    args.IsValid = false;

                                                    var ServStatus = document.getElementById("chkService");
                                                    var InspStatus = document.getElementById("chkInspection");
                                                    var DirStatus = document.getElementById("chkDirective");
                                                    var $items = $('.active').length;


                                                    if ((ServStatus.checked || InspStatus.checked || DirStatus.checked) && $items > 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }
                                            </script>
                                            <script type="text/javascript">
                                                //Model validation
                                                function ValidateModel(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbModel");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;

                                                    }

                                                }
                                                function ValidateComp(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbComp");
                                                    var dd1 = $get("cmbModel");
                                                    if ((dd1.disabled == false && dd.disabled == false && dd1.selectedIndex > 0) || (dd1.disabled == true) || (dd1.disabled == false && dd1.selectedIndex == 0)) {
                                                        args.IsValid = true;
                                                        return;

                                                    }

                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">As On Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagDateSearch" Height ="24px"
                                                            ClientIDMode="Static" onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ClientIDMode="Static" TargetControlID="txtFromDate"
                                                            ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep4" class="clsLabelHeader">Step II. Selection of Removed Part No. &
                                                            Serial No.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblComp" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlComp" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtComp" autocomplete="off" runat="server" CssClass="clsTextBoxTagSearch" Height ="24px"
                                                                    AutoPostBack="True" onfocus="GetAsOnDate()"  ></asp:TextBox>
                                                                <!-- AutoComplete Extender-->
                                                                <cc2:AutoCompleteExtender ID="txtComp_AutoCompleteExtender" runat="server" Enabled="True"
                                                                    CompletionSetCount="20" MinimumPrefixLength="0" CompletionInterval="1" ServicePath=""
                                                                    ServiceMethod="GetPartNoList" TargetControlID="txtComp" UseContextKey="True"
                                                                    ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetPartID">
                                                                </cc2:AutoCompleteExtender>
                                                                <script type="text/javascript">
                                                                    function GetAsOnDate() {
                                                                        var autoComplete = $find('txtComp_AutoCompleteExtender');

                                                                        var str = $("#txtFromDate").val();
                                                                        $("#txtSerialNo").val('');
                                                                        autoComplete.set_contextKey(str);
                                                                    }
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Serial No</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtSerialNo" autocomplete="off" runat="server" CssClass="clsTextBoxTagSearch" Height ="24px"
                                                                    AutoPostBack="True" onfocus="GetAsOnDateSerialNo()"></asp:TextBox>
                                                                <!-- AutoComplete Extender-->
                                                                <cc2:AutoCompleteExtender ID="txtSerialNo_AutoCompleteExtender" runat="server" Enabled="True"
                                                                    CompletionSetCount="20" MinimumPrefixLength="0" CompletionInterval="1" ServicePath=""
                                                                    ServiceMethod="GetSerialNoList" TargetControlID="txtSerialNo" UseContextKey="True"
                                                                    ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                                                </cc2:AutoCompleteExtender>
                                                                <script type="text/javascript">
                                                                    function GetAsOnDateSerialNo() {
                                                                        var autoComplete = $find('txtSerialNo_AutoCompleteExtender');
                                                                        var str1 = $("#txtFromDate").val();
                                                                        var str2 = $("#txtComp").val();
                                                                        var str = str1 + '=' + str2
                                                                        autoComplete.set_contextKey(str);
                                                                    }
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="Span1" class="clsLabelHeader">Step III. Selection of Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table id="Table1" border="0" width="100%">
                                        <tr>
                                            <td width="225px">
                                                <table>
                                                    <tr>
                                                        <td width="25px">
                                                            <asp:CheckBox Text="" ID="chkService" runat="server" ClientIDMode="Static" />
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
                                                &nbsp;&nbsp;
                                            </td>
                                            <td width="225px">
                                                <table>
                                                    <tr>
                                                        <td width="25px">
                                                            <asp:CheckBox Text="" ID="chkInspection" runat="server" ClientIDMode="Static" />
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
                                                &nbsp;&nbsp;
                                            </td>
                                            <td width="225px">
                                                <table>
                                                    <tr>
                                                        <td width="25px">
                                                            <asp:CheckBox Text="" ID="chkDirective" runat="server" ClientIDMode="Static" />
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
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Due Limits / Percentage Life Remaining</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDueLimits" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="150px">
                                                        <asp:RadioButton ID="rbdDueLimits" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIII" Font-Bold="True" Text="Due Limits" Checked="True"></asp:RadioButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rbdPercent" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIII" Font-Bold="True" Text="Percent Life Remaining"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearchSmall" Height ="24px"
                                                            MaxLength="4" ToolTip="Enter Percentage" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                                            <asp:GridView ID="gdvDuePeriodLimits" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                               <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                <Columns>
                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Limit">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLimit" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Height ="24px" Width="185px"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value."
                                                                                BackColor="White"> </asp:TextBox>
                                                                            <asp:CustomValidator ID="cvPeriodLimitsValue" runat="server" Display="None" ControlToValidate="txtLimit"
                                                                                ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1" ValidationGroup="1"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Step V. Enter The Limit For Forecasting</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" width="142px">
                                                <asp:Label ID="lblLimit" runat="server" CssClass="clsLabelAuto">Limit</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtForecastingLimit" runat="server" CssClass="clsTextBoxTagSearchSmall" Height="24px"
                                                    MaxLength="4" ToolTip="Enter Limit">30</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchingCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPart1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;
                                                        <asp:Label ID="lblComp1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="true"
                                                           CssClass="clsbtnH clsinfoH1" TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias." />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPreview" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH1"
                                                            TabIndex="0" Text="Preview" ToolTip="Click to Preview Report" Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Text="Display" ToolTip="Click to Display Report" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" TabIndex="25"
                                                            Text="Report By Mail" ToolTip="Click to receive Report through mail" ValidationGroup="1"
                                                            Width="140px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByExcel" runat="server" CssClass="clsbtnH" TabIndex="25"
                                                            Text="Export to Excel" ToolTip="Click to Export to Excel" ValidationGroup="1"
                                                            Width="140px" Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Back to Previous Page" />
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup-->
                                                <tr style="height: 0px;">
                                                    <td align="right" colspan="2" style="height: 0px;">
                                                        <asp:UpdatePanel ID="upnlImgBtn" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgBtnSendMail" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                                    Style="display: none;" Text="----" />
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
        <script type="text/javascript">
            //function for wo no checkbox visibility
            function ControlvisibilityForWONo(flag) {
                if (flag == "True") {
                    $("#chkwithWONo").css('visibility', 'visible');
                    $("#chkwithWONo").next().css('visibility', 'visible');
                    ControlVisibilityForFormat($("#chkwithWONo"));
                }
                else {
                    $("#chkwithWONo").css('visibility', 'hidden');
                    $("#chkwithWONo").next().css('visibility', 'hidden');
                    $("#chkwithWONo").removeAttr('checked');
                    ControlVisibilityForFormat($("#chkwithWONo"));
                }

            }
            //Service/inspection/Directive list checking
            function ControlvisibilityForCheckboxlist(elem, childid) {
                //if selected then enable and select checkboxlist else uncheck and disable list
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#' + childid).removeAttr('disabled');
                }
                else {
                    $('#' + childid).attr('disabled', 'disabled');
                }

                $('#' + childid).find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        $(this).removeAttr('disabled');
                    }
                    else {
                        $(this).removeAttr("checked");
                        $(this).attr('disabled', 'disabled');
                    }
                });
            }

            //wo no checkbox status change event
            function ControlVisibilityForFormat(elem) {
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#cmbFormat').attr('disabled', 'disabled');
                    $('#cmbFormat').val('0');
                }
                else {
                    $('#cmbFormat').removeAttr('disabled');
                }
            }
        </script>
    </div>
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="CompID" />
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
            if (source._id == "txtSerialNo_AutoCompleteExtender") {
                textbox = document.getElementById('CompID');
            }
            textbox.value = value;
        }
        function SetPartID(source, e) {
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
            if (source._id == "txtComp_AutoCompleteExtender") {
                textbox = document.getElementById('PartID');
            }
            textbox.value = value;
        }
        //        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        //        function SetPartIdonChange(source, extenderid) {
        //            var popup = $find(extenderid);
        //            var complist = popup.get_completionList();
        //            var text = $(source).val().toLowerCase();
        //            for (var i = 0; i < complist.childNodes.length; i++) {
        //                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
        //                if (text == texttocompare) {
        //                    var val = complist.childNodes[i]._value;

        //                    if (extenderid == "txtComp_AutoCompleteExtender") {
        //                        textbox = document.getElementById('PartID');
        //                    }
        //                    if (extenderid == "txtSerialNo_AutoCompleteExtender") {
        //                        textbox = document.getElementById('CompID');
        //                    }
        //                    textbox.value = val;
        //                    return;
        //                }

        //            }

        //            //            if (extenderid == "txtComp_AutoCompleteExtender") {
        //            //                document.getElementById('CompID').value = '';
        //            //            }
        //        }
        
    </script>
    <!---End-->
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("#chkService").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('selectAll', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                $('[id*=ListServiceType]').multiselect('disable', false);                       // * disable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('refresh');
            }
        });
        $("#chkInspection").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListInspectionType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListInspectionType]').multiselect('selectAll', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);                       // * disable the multiselect ListBOx
                $('[id*=ListInspectionType]').multiselect('refresh');
            }
        });
        $("#chkDirective").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListDirectiveType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListDirectiveType]').multiselect('selectAll', false);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                $('[id*=ListDirectiveType]').multiselect('disable', false);                       // * disable the multiselect ListBOx
                $('[id*=ListDirectiveType]').multiselect('refresh');
            }
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var ServStatus = document.getElementById("chkService");
                    if (ServStatus.checked == false) {
                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');
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
                    if (DirStatus.checked == false) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');
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
                    if (Inspstatus.checked == false) {
                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');
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
</body>
</html>

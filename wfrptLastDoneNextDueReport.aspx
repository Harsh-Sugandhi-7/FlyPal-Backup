<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptLastDoneNextDueReport.aspx.vb"
    Inherits="Flypal.wfrptLastDoneNextDueReport" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>C of A Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
        .btn {
            padding: 1px;
            font-size: 8pt;
        }

        .TextBox {
            box-sizing: Content-box;
        }

        .label {
            font-weight: normal !important;
            font-style: normal;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="1200" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
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
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="clsFormHeader1Newstyle">
                                                                <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Search criteria for C of A</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage=""
                                                                    ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                    ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate" ClientValidationFunction="ValidateType"></asp:CustomValidator>
                                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                                    ErrorMessage="As On Date Required" ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                                                <script type="text/javascript">
                                                                    function ValidateAircraft(source, args) {
                                                                        args.IsValid = false;
                                                                        source.errormessage = 'Please select the Aircraft and Assembly.'
                                                                        var dd = $get("cmbAircraft");

                                                                        if (dd.selectedIndex != 0) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }

                                                                    }
                                                                    function ValidateType(source, args) {
                                                                        args.IsValid = false;
                                                                        if ('<%# AppSettings("ShowMaintenanceForNewClients") %>' == "True") {
                                                                            source.errormessage = 'Please select the type.'
                                                                        }
                                                                        else {
                                                                            source.errormessage = 'Please select the Service/Inspection.'
                                                                        }
                                                                        var $items = $('.active').length;
                                                                        if ($items != 0) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }
                                                                    }
                                                                </script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlAsOnDate" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td colspan="3">
                                                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10px"></td>
                                                            <td width="70px">
                                                                <span id="lblFromDate" class="clsLabel">As On Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtFromDate"
                                                                    onchange="ValidateDateText(this,'txtFromDate_watermarkextender');" Height="25px"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="txtFromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlSelectionofAircraft" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="10px">
                                                                            <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td width="70px">
                                                                            <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                                                DataTextField="RegNo" DataValueField="MachineID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabel">Assembly</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbAssembly" runat="server" DataValueField="ID"
                                                                                DataTextField="ModelSerialNoPostion">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of ATA</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblATAChapter" class="clsLabel">ATA Chapter</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbATAChapter" runat="server"
                                                                                DataValueField="ID" DataTextField="ATAChapter">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkAssembly" Text="Show Assembly Inspections" Checked="true" runat="server"
                                                                                CssClass="clsCheckBox" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkComponent" Text="Show Component Inspections" Checked="true"
                                                                                runat="server" CssClass="clsCheckBox" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStep5" class="clsLabelHeader">Step V. Selection of Type</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblTypeStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel runat="server" ID="upnType" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:CheckBoxList ID="cmbType" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                    DataValueField="ID" DataTextField="Name" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                                </asp:CheckBoxList>
                                                                <span id="Span3" class="clsLabel" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>Type</span>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlServiceType" runat="server" CssClass="clsPanel1">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel runat="server" ID="upnlServiceType" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:PlaceHolder ID="phInspection" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                                        <asp:Panel ID="pnlInspectionType" runat="server" CssClass="clsPanel1">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:UpdatePanel runat="server" ID="upnlInspectionType" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                                    DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </asp:PlaceHolder>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:CheckBox ID="chkNotApplicable" Text="With &quot;Not Applicable&quot;" runat="server" CssClass="clsCheckBox" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="Span2" class="clsLabelHeader">Step VI. Selection of Format</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="10px"></td>
                                                    <td width="70px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabel">Format</asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:UpdatePanel ID="upnFormat" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                                                    <asp:ListItem Text="Format 1" Value="1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Format 2" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbllinkedAct" runat="server" Visible="false" CssClass="clsLabelHeader"> * With Linked Maintenance Activity</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlCurrentCriteria" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlCriteria" runat="server" Visible="false">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <span id="Span1" class="clsLabelHeader">Step VII. Current Criteria</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblATAChapter1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ClientIDMode="Static" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                                TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                                                Text="Display" ToolTip="Click to Display Report" />
                                                                        </td>
                                                                        <%-- 'Added by Shital on 14-Sep-2016--%>
                                                                        <td>
                                                                            <asp:Button ID="btnByMail" runat="server" CssClass="clsButton_Ajax" Text="Report By Mail"
                                                                                Visible="false" ToolTip="Click to receive Report through mail" Width="96px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                                TabIndex="0" Text="Close" ToolTip="Click to Close" />
                                                                        </td>
                                                                    </tr>
                                                                    <!-- Dummy panel to open modelpopup 'Added by Shital on 14-Sep-2016 -->
                                                                    <tr style="height: 0px;">
                                                                        <td style="height: 0px;" colspan="2" align="right">
                                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                                <ContentTemplate>
                                                                                    <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                    <asp:HiddenField ID="hdnService" runat="server" />
                                                                                    <asp:HiddenField ID="hdnInspection" runat="server" />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <!--End -->
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
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
    <script type="text/javascript">
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
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        function disableEnable() {

            var hvService = $('#hdnService').val();
            var hvInsp = $('#hdnInspection').val();
            ServiceMultiSelect();
            InspMultiSelect();

            if (hvService == 'True') {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('selectAll', false);
                //  $('[id*=ListServiceType]').multiselect('refresh');
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            else if (hvService == 'False' || hvService == '') {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                // $('[id*=ListServiceType]').multiselect('refresh');
                $('[id*=ListServiceType]').multiselect('disable', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            if (hvInsp == 'True') {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('selectAll', false);
                //    $('[id*=ListInspectionType]').multiselect('refresh');
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

            else if (hvInsp == 'False' || hvInsp == '') {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                //   $('[id*=ListInspectionType]').multiselect('refresh');
                $('[id*=ListInspectionType]').multiselect('disable', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

        }

    </script>
    <script type="text/javascript">
        function disableEnableOnPageLoad() {

            var hvService = $('#hdnService').val();
            var hvInsp = $('#hdnInspection').val();
            ServiceMultiSelect();
            InspMultiSelect();

            if (hvService == 'True') {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            else if (hvService == 'False' || hvService == '') {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                $('[id*=ListServiceType]').multiselect('disable', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            if (hvInsp == 'True') {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

            else if (hvInsp == 'False' || hvInsp == '') {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

        }

    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            ServiceMultiSelect();
            InspMultiSelect();
            //  disableEnable();
            disableEnableOnPageLoad();
        });

        //   Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function ServiceMultiSelect() {
            $('[id*=ListServiceType]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                nSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');

            //   });
        }
    </script>
    <script type="text/javascript">

        // Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function InspMultiSelect() {
            $('[id*=ListInspectionType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;


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
            //  });
        }
    </script>
</body>
</html>

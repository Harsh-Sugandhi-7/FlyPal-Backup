<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForInspection_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForInspection_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Inspection Status Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="900" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
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
                            <td colspan="3" class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <span id="lbltitle" class="clsFormHeader">Search criteria for Inspection Report</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the Aircraft and Assembly"
                                            ControlToValidate="cmbAircraft" Display="None" ValidationGroup="valGroup1" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the Inspection"
                                            ControlToValidate="cmbType" Display="None" ClientValidationFunction="ValidateInspection"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="As On Date Required" ControlToValidate="txtAsOnDate" Display="None"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!-- Client side validation for comboboxes-->
                                <script type="text/javascript">
                                    //Aircraft List
                                    function ValidateAircraft(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbAircraft");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;

                                        }

                                    }
                                    //Inspection List
                                    function ValidateInspection(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbType");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;

                                        }

                                    }
                                    

                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="12px">
                                                </td>
                                                <td width="65px">
                                                    <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtAsOnDate" ClientIDMode="Static"
                                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="12px">
                                                    <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                        DataValueField="ID" DataTextField="RegNo">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="left">
                                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="12px">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataValueField="ID"
                                                        DataTextField="ModelSerialNoPostion">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAirframeDueAsOf" runat="server" CssClass="clsCheckBox" Text="Show Due As Of Airframe Values" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Inspection</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td width="12px" valign="middle">
                                            <span id="lblTypeStar1" class="clsLabelStar">*</span>
                                        </td>
                                        <td width="65px">
                                            <span id="lblType" class="clsLabelAuto">Inspection</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbType" runat="server" DataValueField="ID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="Label2" class="clsLabelHeader">Step V. Bottom Line of Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="Label3" class="clsLabelAuto">Enter Line which you want to print at the bottom
                                    of the report.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle" ID="txtBottomLine" runat="server"
                                    ToolTip="Enter Note" TextMode="MultiLine" MaxLength="500" Width="552px">I hereby certify that the data specified above has been certified throughout : Engineering Department Manager : ____________________   Date : __________        </asp:TextBox>
                            </td>
                            <!--CHK For clsTextBoxMultilineDefectActionAuto AJAX -->
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblStep6" class="clsLabelHeader">Step VI. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                        ToolTip="Click to Display Current Searching criterias." CausesValidation="False"
                                                        Text="Current Criteria"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" ToolTip="Click to Display Report"
                                                        Text="Display" ValidationGroup="valGroup1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to Close"
                                                        CausesValidation="False" Text="Close"></asp:Button>
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
</body>
</html>

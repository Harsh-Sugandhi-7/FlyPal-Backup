<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptFanBladeMonitoring_Ajax.aspx.vb"
    Inherits="Flypal.wfrptFanBladeMonitoring_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Engine LLP Status List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .aspNetDisabled
        {
            color: Black !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
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
                            <td class="clsFormHeader1Newstyle">
                                <span id="lbltitle" class="clstitle1">Fan Blades Distribution Plot</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtAsOnDate"
                                    ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtAsOnDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="cmbAircraft" ErrorMessage="Select Aircraft from list" ClientValidationFunction="ValidateCombo"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function ValidateCombo(source, args) {
                                        args.IsValid = false;
                                        var dd = $get(source.controltovalidate);
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;

                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblInstallationDate" class="clsLabelAuto">Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtAsOnDate" 
                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True" 
                                                        ClientIDMode="Static" DataTextField="RegNo" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataTextField="ModelSerialNoPositionBA"
                                                        ClientIDMode="Static" DataValueField="ID">
                                                    </asp:DropDownList>
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
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server"  TabIndex="0"
                                                        Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"
                                                         />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                        Text="Display" ToolTip="Click to display report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server"  Text="Close"
                                                        ToolTip="Click to Close" CausesValidation="False"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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

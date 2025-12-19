<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptTLPRegister.aspx.vb"
    Inherits="Flypal.wfrptTLPRegister" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
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
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblLedgerList" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblLedgerList" runat="server" CssClass="clsFormHeader">TLP Register</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ValidationGroup="1" ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvtxtFromDate" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date required"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvtxtToDate" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date required"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ClientValidationFunction="BetweenDatesValidation" ValidationGroup="1" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtFromDate" 
                                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabel">To Date</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtToDate" 
                                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="False"
                                                                DataTextField="RegNo" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step III. Selection of Flight Classification</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblFlightLogClassification" runat="server" CssClass="clsLabelAuto">Classification</asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFlightLogClassification" runat="server"
                                                                 DataTextField="Name" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Format</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelAuto">Format</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="True">
                                                                <asp:ListItem Text="Format 1" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Format 2 (Aircraft Utilization)" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                        <td>
                                                            <asp:PlaceHolder ID="phRadiobuttons" runat="server" Visible ="false" >
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdbICAO" Text='Show "ICAO"' runat="server" GroupName="w" Checked="True" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdbCode" Text='Show "Short Name"' runat="server" GroupName="w" /></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:PlaceHolder>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
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
                                                                    </table>
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
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                                Text="Current Criteria" ToolTip="Click to display Current Searching criterias."
                                                                CausesValidation="True"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" CausesValidation="True" 
                                                                Text="Display" ValidationGroup="1" ToolTip="Click to Display Report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" TabIndex="25"
                                                                CausesValidation="True" Text="Report By Mail" ToolTip="Click to receive Report through mail"
                                                                ValidationGroup="1"  />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByExcel" runat="server" TabIndex="25"
                                                                CausesValidation="True" Text="Export to Excel" ToolTip="Click to Export to Excel"
                                                                ValidationGroup="1" Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                ToolTip="Click to close Audit Findings Report screen" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <!--Dummy panel to open modelpopup-->
                                                    <tr style="height: 0px;">
                                                        <td align="right" colspan="5" style="height: 0px;">
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
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="100" ClientIDMode="Static" DynamicLayout="false"
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
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

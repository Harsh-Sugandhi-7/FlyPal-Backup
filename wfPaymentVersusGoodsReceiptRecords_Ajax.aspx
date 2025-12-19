<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPaymentVersusGoodsReceiptRecords_Ajax.aspx.vb"
    Inherits="Flypal.wfPaymentVersusGoodsReceiptRecords_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payment Advice List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <%-- <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />--%>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">PAYMENT VERSUS GOODS RECEIPT REGISTER</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblStepDate" class="clsLabelHeader">Selection of Date Range</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                                ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblFrom" class="clsLabelAuto">From</span>
                                                </td>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                                <span id="lblTo" class="clsLabelAuto">To</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
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
                                                    <span id="lblSupplier" class="clsLabelHeader">Selection of Supplier</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSupp" class="clsLabel">Supplier </span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="cmbSupplier" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                        DataTextField="Name">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblPaymentAdviceNumber" class="clsLabelHeader">Selection of Payment Advice
                                                        No. </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabel">Payment Advice No</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlPaymentAdvice" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbPaymentAdviceText" runat="server" CssClass="clsComboBox_Ajax"
                                                                AutoPostBack="true" DataTextField="Text" DataValueField="Text">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlPaymentAdviceNo" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="7"
                                                                onchange="setattr(this);" ToolTip="Enter Payment Advice Number">0</asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblOrderNumber" class="clsLabelHeader">Selection of Order No. </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblOrderNo" class="clsLabelAuto">Order No.</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Text"
                                                        AutoPostBack="true" DataValueField="Text">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="7"
                                                        onchange="setattr(this);" ToolTip="Enter Order Number">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="Span2" class="clsLabelHeader">Selection of GRN/GRO No. </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span3" class="clsLabelAuto">GRN/GRO No.</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsComboBox_Ajax"
                                                        DataTextField="Text" AutoPostBack="true" DataValueField="Text">
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="7"
                                                        onchange="setattr(this);" ToolTip="Enter Receipt Number">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="Span4" class="clsLabelHeader">Enter Proforma Inv# </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span5" class="clsLabelAuto">Proforma Inv#</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtProformaInv" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Proforma Inv#"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        ValidationGroup="a" Text="Display" ToolTip="Click to Display" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
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
    </form>
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
</body>
</html>

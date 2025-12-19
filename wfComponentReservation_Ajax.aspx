<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComponentReservation_Ajax.aspx.vb"
    Inherits="Flypal.wfComponentReservation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reservation Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Reservation Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                        <asp:CustomValidator ID="cvCommon" runat="server" Display="None" OnServerValidate="CustomValidate"
                                                            ValidationGroup="a"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" ValidationGroup="a"
                                                            Display="None" ErrorMessage="Remark should be 500 characters." OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvAircraft" runat="server" OnServerValidate="CustomValidate"
                                                            ValidationGroup="a" Display="None" ErrorMessage="Select Aircraft." ControlToValidate="cmbAircraftList"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvRevdate" runat="server" OnServerValidate="customvalidate"
                                                            ValidationGroup="a" Display="None" ErrorMessage="Reservation date should be grater receipt date."
                                                            ControlToValidate="txtReservationDate"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlComponentReservationDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; border-width: 1px"
                                                class="clsFieldSet">
                                                <legend class="clsFieldSet1"><b>Reservation Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextPartNo" runat="server" CssClass="clsLabelHeader" Text="<%# mComponentReservation.PartNo %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextSerialNo" runat="server" CssClass="clsLabelHeader" Text="<%# mComponentReservation.SerialNo %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblReceiptNo" class="clsLabel">Receipt No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltxtReceiptNo" runat="server" CssClass="clsLabelHeader" Text="<%# mComponentReservation.ReceiptNo %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="lblReceiptDate" class="clsLabel">Receipt Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltxtReceiptDate" runat="server" CssClass="clsLabelHeader" Text="<%# mComponentReservation.ReceiptDateFormatted %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblDate" class="clsLabel">Reservation Date</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtReservationDate" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                                CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                Text="" Width="100px" Enabled="<%# mComponentReservation.CountOfUsedInIssueItem=0 %>"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtReservationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReservationDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="txtReservationDateWatermarkExtender" runat="server"
                                                                TargetControlID="txtReservationDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblAircraftStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblAircraft" class="clsLabelauto">Aircraft</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsComboBox_Ajax"
                                                                AutoPostBack="True" DataTextField="RegNo" DataValueField="ID" SelectedValue="<%# mComponentReservation.MachineID %>"
                                                                Enabled="<%# mComponentReservation.CountOfUsedInIssueItem=0 %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblRemark" class="clsLabelauto">Remark</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
                                                                Text="<%# mComponentReservation.ReserveForRemark %>" ToolTip="Enter Remark" TextMode="MultiLine"
                                                                MaxLength="500" Enabled="<%# mComponentReservation.CountOfUsedInIssueItem=0 %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save"
                                                            ValidationGroup="a"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsButton_Ajax" ToolTip="Click to close"
                                                            CausesValidation="False"></asp:Button>
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
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAuditComplianceDueReport_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAuditComplianceDueReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Compliance Due Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
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
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>

                                    <td colspan="2" class="clsFormHeader1">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lbltitle" class="clsFormHeader">Audit Compliance Due Report</span>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnSendMail" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Send Mail" ToolTip="Click to Mail report"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to close" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDate" class="clsLabelAuto">Date</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" runat="server" ID="txtDate" Width="100px"
                                                    onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepII" class="clsLabelHeader">Step II. Selection of Audit No. </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblAuditNo" class="clsLabelAuto">Audit No.</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAuditInfoList" runat="server" DataTextField="AuditNo"
                                            DataValueField="AuditNo">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIII" class="clsLabelHeader">Step III. Selection of Department </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDepartment" class="clsLabelAuto">Department</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDepartmentList" runat="server" DataTextField="Name"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIV" class="clsLabelHeader">Step IV. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAudit" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDeparmentName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                     <td>
                                                        <asp:Button ID="btnSendMail" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Send Mail" ToolTip="Click to Mail report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                            ToolTip="Click to close" CausesValidation="False"></asp:Button>
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

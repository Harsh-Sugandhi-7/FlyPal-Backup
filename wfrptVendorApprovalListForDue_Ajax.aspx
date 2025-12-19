<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptVendorApprovalListForDue_Ajax.aspx.vb" Inherits="Flypal.wfrptVendorApprovalListForDue_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Vendor Document Approval List For Due</title>
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
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel CssClass="clspanel1" ID="pnlmain" runat="server">
                            <table class="clstablelistin" id="tblInner">
                                <tr>
                                    <td class="clsFormHeader1" colspan="2">
                                        <span class="clsFormHeader" id="lbltitle">Vendor Document Approval List For Due</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepI" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtDate"
                                                    onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                                <cc2:calendarextender id="txtDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                    enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtDate">
                                                </cc2:calendarextender>
                                                <cc2:textboxwatermarkextender targetcontrolid="txtDate" id="Date_watermarkextender"
                                                    clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                    watermarkcssclass="clsDateTextBox">
                                                </cc2:textboxwatermarkextender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepIII" class="clsLabelHeader">Step II. Selection of Vendor</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblStore" class="clsLabelAuto">Vendor</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbVendor" runat="server" DataTextField="Name"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStep4" class="clsLabelHeader">Step III. Selection of Vendor Type</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblCategory" class="clsLabelAuto">Vendor Type</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbVendorType" runat="server"
                                            DataTextField="Name" DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStep5" class="clsLabelHeader">Step IV. With/Without Document Approval</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblWith Audit Due" class="clsLabelAuto"></span>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkWithWithoutDocumentApproval" runat="server" Checked="true"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepV" class="clsLabelHeader">Step V. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblVendorType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="0" Text="Current Criteria" ToolTip=" Click to display current searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to Close" />
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


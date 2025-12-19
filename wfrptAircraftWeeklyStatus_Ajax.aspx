<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftWeeklyStatus_Ajax.aspx.vb" Inherits="Flypal.wfrptAircraftWeeklyStatus_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Weekly Status Report</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="" type="text/javascript">
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1" colspan="3">
                                    <span class="clsFormHeader" id="lbltitle" >Aircraft Weekly Status Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px">
                                   
                                </td>
                                <td>
                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtDate"  
                                        onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Model</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px" align="right">
                                    
                                </td>
                                <td align="left">
                                    <span id="lblAircraft" class="clsLabelAuto">Model </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbAircraft" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                        DataValueField="ID" EnableViewState="false" DataTextField="ModelName">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px" align="left">
                                </td>
                                <td colspan="2" align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td style="width: 2px" align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"  
                                                            Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnDisplay" TabIndex="0" runat="server"  
                                                            Text="Display" ToolTip="Click to Display Report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server"   Text="Close" CausesValidation="False"
                                                            ToolTip="Click to Close Aircraft Weekly Status Report Screen"></asp:Button>
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
         <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <%--Date Validations--%>
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

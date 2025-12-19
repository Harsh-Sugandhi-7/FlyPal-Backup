
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeService_Ajax.aspx.vb" Inherits="Flypal.wfSearchCriteriaForEmployeeService_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Document Due Reporty</title>
     <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

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
            <td colspan="1">
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">

                <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="4" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                         <td>
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Employee Service</asp:Label>
                                        </td>

                                        
                                       
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFrom" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                                <table id="Table4" cellspacing="1" cellpadding="1" border="0">
                                    <tr>
                                        <td>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                       
                                             <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>

                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lblTo" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                            </td>
                            <td>
                                
                                  <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                       
                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                            </cc2:CalendarExtender>

                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Employee</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 14px">
                                <asp:Label ID="lblEmployee" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                            </td>
                            <td style="height: 14px">
                                <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                    DataTextField="EmpNoName" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td style="height: 14px">
                            </td>
                            <td style="height: 14px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Service</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblService" runat="server" CssClass="clsLabelAuto">Service</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbServiceList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows : </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblServiceCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table class="clstableButton" id="Table3" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                CausesValidation="False" ToolTip="Click to Display Current Searching criterias."
                                                Text="Current Criteria"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDisplay" runat="server" ToolTip="Click to Display Report" Text="Display"
                                                CssClass="clsbtnH clsinfoH1"></asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnClose" runat="server" ToolTip="Click to close Employee Service screen"
                                                Text="Close" CssClass="clsbtnH clsinfoH1"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
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
</body>
</html>

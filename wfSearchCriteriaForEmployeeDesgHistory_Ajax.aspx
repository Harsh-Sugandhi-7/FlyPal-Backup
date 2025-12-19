<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeDesgHistory_Ajax.aspx.vb" Inherits="Flypal.wfSearchCriteriaForEmployeeDesgHistory_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
   
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Designation-Salary History Report</title>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="3" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Employee Designation-Salary History Report</asp:Label>
                                 &nbsp;
                                &nbsp;
                            </td>
                            
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvEmployee" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="cmbCrewList" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvtxtFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date required"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvtxtToDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" Width="66px" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <table id="Table1" cellspacing="0">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                           
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                       
                                            <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                            </cc2:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDate" runat="server" Width="52px" CssClass="clsLabelAuto">To Date</asp:Label>
                                        </td>
                                        <td>
                                          
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                       
                                            <cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                            </cc2:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Employee</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 62px" align="left">
                                <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                            </td>
                            <td align="left" colspan="2">
                                <asp:DropDownList ID="cmbCrewList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="EmpNoName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblStepIII" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblCrewName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="3">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                    Text="Current Criteria" ToolTip="Click to display Current Searching criterias.">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                    ToolTip="Click to close Employee Designation-Salary History screen" CausesValidation="False">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </div>
        <div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
</html>

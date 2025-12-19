<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHangarPlanningReport1.aspx.vb"
    Inherits="Flypal.wfHangarPlanningReport1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
     <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
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
        <table class="clstablelistout" id="Table1">
            <tr>
                <td>
                    <asp:Panel ID="Panel1" CssClass="clsPanel1" runat="server">
                        <table id="Tbb">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tblLedgerList" class="clstablelistin">
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="lblLedgerList" class="clstitle1">Hangar Planning Report</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                                                ValidationGroup="a"></asp:ValidationSummary>
                                                                            <asp:RequiredFieldValidator ID="rdatetimefrom" runat="server" CssClass="clsLabelAuto"
                                                                                ErrorMessage="Datetime Required" Display="None" ControlToValidate="txtFromDate"
                                                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rdatetimeto" runat="server" CssClass="clsLabelAuto"
                                                                                ErrorMessage="Datetime Required" Display="None" ControlToValidate="txtToDate"
                                                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                                                ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="a"></asp:CustomValidator>
                                                                            <%--    <asp:RequiredFieldValidator ID="rTextNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="No. Required"
                                                            Display="None" ControlToValidate="txtNo" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                                            <asp:RegularExpressionValidator ID="rxpTextNo" runat="server" ErrorMessage="Only Numbers allowed"
                                                                                ValidationExpression="\d+" ControlToValidate="txtNo" ValidationGroup="a"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span7" class="clsLabelHeader">Step I. Selection of Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span5" class="clsLabelHeader">From</span>
                                                    </td>
                                                    <td>
                                                        <%--  <asp:Label ID="lblFromDateTime" runat="server" CssClass="clsLabel">FromDateTime</asp:Label>--%>
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="Span6" class="clsLabelHeader">To</span>
                                                    </td>
                                                    <td>
                                                        <%--             <asp:Label ID="lblDateTimeTo" runat="server" CssClass="clsLabel" Height="16px" 
                                            Visible="True">DateTimeTo</asp:Label>--%>
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="TextBoxWatermarkExtender1"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span1" class="clsLabelHeader">Step II. Selection of Hangar</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span9" class="clsLabelHeader">Hangar</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="cmbHanger" runat="server" CssClass="clsComboBox_Ajax" DataTextField="HHangerWithCity"
                                                            DataValueField="HID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span2" class="clsLabelHeader">Step III. Selection of Aircraft</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span10" class="clsLabelHeader">Aircraft</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataTextField="HAicraftWithModelSerialNo"
                                                            DataValueField="HID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span8" class="clsLabelHeader">Step IV. Selection of Hangar Planning Number</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span3" class="clsLabelHeader">Text</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbText" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Text"
                                                            DataValueField="Text">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="Span4" class="clsLabelHeader">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="10"
                                                            Visible="True" Width="184px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="Right" colspan="4">
                                                        <asp:UpdatePanel ID="upnlPrint" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax " Text="Print"
                                                                    ToolTip="Click To Print Hangar Report" ValidationGroup="a" />
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax " Text="Close"
                                                                    ToolTip="Click to close Report Screen" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <%--  <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax " Text="Print"    ToolTip="Click To Print Hangar List" />  --%>
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
    <script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
       SetPageLayout();
         if ($.browser.msie) {
             parent.IFrameHangerStateComplete();
         }
       
      
    });
     <% End if %>
       Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

       function SetPageLayout()
       {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
          ReSetPageLayout();
          //onResize();//for Top bottom link
           <% End if %>
       }
       function ReSetPageLayout()
       {
       $("body,html").css({ 'background-color': 'transparent' });
          var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }


    </script>
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
        //        function btnSelectFile_onclick() {

        //        }

    </script>
</body>
</html>

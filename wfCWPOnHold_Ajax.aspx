<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPOnHold_Ajax.aspx.vb"
    Inherits="Flypal.wfCWPOnHold_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>CWP On Hold</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td>
                                <span id="lblListEnquiry" class="clstitle1">On Hold Details</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOnHoldDate"
                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Date Required"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDefect" runat="server" ControlToValidate="txtRemark"
                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Remark Required"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtRemark" ErrorMessage="Remark should not be greater than 500 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                         <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtRemark" OnServerValidate="CustomValidate" Display="None"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtRemark':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 500) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;



                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlCWPCompDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span8" class="clsLabelAuto">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOnHoldDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtOnHold_CalendarExtender','false');"
                                                            ClientIDMode="Static" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtOnHoldDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOnHoldDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtOnHoldDate_Watermarkextender" runat="server"
                                                            TargetControlID="txtOnHoldDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span3" class="clsLabelAuto">Remark/Note</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" ClientIDMode="Static"
                                                            ToolTip="Enter Remark/Note" TextMode="MultiLine" Width="385px"></asp:TextBox>
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
                                <asp:UpdatePanel ID="upnlACtionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add the CWP ComponentOn Hold Status"
                                                        Text="Ok"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="false" Text="Back"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForCWPOnHold();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
            <% Dim mopen As String = Request.QueryString("Type") %>
            <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
               SetPageLayout();
                 if ($.browser.msie) {
                     parent.IFrameCWPOnHoldStateComplete();
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
                  onResize();//for Top bottom link
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
    <%--End--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, DefaultValue) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': DefaultValue };
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

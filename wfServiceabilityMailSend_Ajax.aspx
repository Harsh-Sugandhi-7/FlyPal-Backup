<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfServiceabilityMailSend_Ajax.aspx.vb"
    Inherits="Flypal.wfServiceabilityMailSend_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Serviceability Mail</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function resizeTextBox(txt) {
            txt.style.height = "1px";
            txt.style.height = (1 + txt.scrollHeight) + "px";

        }
        function OnResize(txt) {
            $(txt).animate({ width: 500, height: txt.scrollHeight }, "fast");
        }
        function OnLostResize(txt) {
            $(txt).animate({ width: 500, height: 100 }, "fast");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                            <td colspan="2">
                                <span id="lbltitle" class="clstitle1">Aircraft Serviceability Mail</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvReqMailID" runat="server" ValidationGroup="1" Display="None"
                                    ErrorMessage="Please Enter at least one Valid Email-ID" ControlToValidate="txtMailIDs"
                                    CssClass="" ClientValidationFunction="validateEmailID" ValidateEmptyText="true"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvMailIDs" runat="server" ValidationGroup="1" Display="None"
                                    ControlToValidate="txtMailIDs" ErrorMessage="Please Enter Valid Email-ID" CssClass=""
                                    ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCc" runat="server" ValidationGroup="1" Display="None"
                                    ControlToValidate="txtCCIDs" ErrorMessage="Please Enter Valid Cc Email-ID" CssClass=""
                                    ClientValidationFunction="validateMultipleCcEmailsCommaSeparated"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvBcc" runat="server" ValidationGroup="1" Display="None"
                                    ControlToValidate="txtBCCIDs" ErrorMessage="Please Enter Valid Bcc Email-ID"
                                    CssClass="" ClientValidationFunction="validateMultipleBccEmailsCommaSeparated"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function validateEmailID(source, args) {

                                        var ToEmailIDs = $("#txtMailIDs").val();
                                        if (ToEmailIDs == '') {
                                            args.IsValid = false;
                                            return;
                                        }
                                    }
                                    function validateEmail(field) {
                                        var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
                                        return (regex.test(field)) ? true : false;
                                    }
                                    function validateMultipleEmailsCommaSeparated(source, args) {
                                        var text = $("#txtMailIDs").val();
                                        var seperator = ',';
                                        if (text != '') {
                                            var result = text.split(seperator);
                                            for (var i = 0; i < result.length; i++) {
                                                if (result[i] != '') {
                                                    if (!validateEmail(result[i].trim())) {
                                                        args.IsValid = false;
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    function validateMultipleCcEmailsCommaSeparated(source, args) {
                                        var text = $("#txtCCIDs").val();
                                        var seperator = ',';
                                        if (text != '') {
                                            var result = text.split(seperator);
                                            for (var i = 0; i < result.length; i++) {
                                                if (result[i] != '') {
                                                    if (!validateEmail(result[i].trim())) {
                                                        args.IsValid = false;
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    function validateMultipleBccEmailsCommaSeparated(source, args) {
                                        var text = $("#txtBCCIDs").val();
                                        var seperator = ',';
                                        if (text != '') {
                                            var result = text.split(seperator);
                                            for (var i = 0; i < result.length; i++) {
                                                if (result[i] != '') {
                                                    if (!validateEmail(result[i].trim())) {
                                                        args.IsValid = false;
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                   
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span class="clsLabelAuto">Report Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                AutoPostBack="true" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="clsLabelAuto">Mail Preview</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtMail" runat="server" CssClass="clsTextBoxLong1_Ajax" Height="100px"
                                    Width="500px" onFocus="OnResize(this)" onkeyup="resizeTextBox(this)" onblur="OnLostResize(this)"
                                    TextMode="MultiLine" ReadOnly="true" BackColor="Gainsboro"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="clsLabelHeader">Mail will be sent to following Recipients, please enter
                                    comma separated Email-ID's.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="clsLabelAuto">To...</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMailIDs" runat="server" CssClass="clsTextBoxMultiLine_Ajax" Height="40px"
                                    TextMode="MultiLine" Width="385px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="clsLabelAuto">Cc...</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCCIDs" runat="server" CssClass="clsTextBoxMultiLine_Ajax" Height="40px"
                                    TextMode="MultiLine" Width="385px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="clsLabelAuto" style="display: none;">Bcc...</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBCCIDs" runat="server" CssClass="clsTextBoxMultiLine_Ajax" Height="40px"
                                    TextMode="MultiLine" Visible="false" Width="275px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="clsLabelAuto">Remark</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine_Ajax" ClientIDMode="Static"
                                    Height="40px" TextMode="MultiLine" Width="385px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 115px;">
                                <span id="Span5" class="clsLabelAuto">Report Generated By</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportGenratedBy" runat="server" AutoPostBack="false" CssClass="clsTextBox_Ajax"
                                    Text='<%# " Submitted By : " + User.Identity.Name %>' Width="385px" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSendMail" runat="server" CssClass="clsButton_Ajax" Text="Send Mail"
                                                        ValidationGroup="1" ToolTip="Click to send report by mail" Width="96px" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close "
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;" colspan="2" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForSendMail();
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
                parent.IFrameSendMailStateComplete();
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
    </form>
</body>
</html>

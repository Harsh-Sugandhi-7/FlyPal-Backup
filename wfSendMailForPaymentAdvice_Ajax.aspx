<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSendMailForPaymentAdvice_Ajax.aspx.vb" Inherits="Flypal.wfSendMailForPaymentAdvice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Mail for Payment Advice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <%-- <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />--%>
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
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Send Mail</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvReqMailID" runat="server" ValidationGroup="1" Display="None"
                                    ErrorMessage="Please Enter at least one Valid Email-ID" ControlToValidate="txtMailIDs"
                                    CssClass="" ClientValidationFunction="validateEmailID" ValidateEmptyText="true"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvMailIDs" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtMailIDs"
                                    ErrorMessage="Please Enter Valid Email-ID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCc" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtCCIDs"
                                    ErrorMessage="Please Enter Valid Cc Email-ID" CssClass="" ClientValidationFunction="validateMultipleCcEmailsCommaSeparated"></asp:CustomValidator>
                                <%--<asp:CustomValidator ID="cvBcc" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtBCCIDs"
                                    ErrorMessage="Please Enter Valid Bcc Email-ID" CssClass="" ClientValidationFunction="validateMultipleBccEmailsCommaSeparated"></asp:CustomValidator>--%>
                                <script type="text/javascript">
                                    function validateEmailID(source, args) {
                                        var MandatoryEmailID = $("#lblToMailID").text();
                                        var optionalEmailID = $("#txtMailIDs").val();
                                        if (MandatoryEmailID == '' && optionalEmailID == '') {
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

//                                    function validateMultipleBccEmailsCommaSeparated(source, args) {
//                                        var text = $("#txtBCCIDs").val();
//                                        var seperator = ',';
//                                        if (text != '') {
//                                            var result = text.split(seperator);
//                                            for (var i = 0; i < result.length; i++) {
//                                                if (result[i] != '') {
//                                                    if (!validateEmail(result[i])) {
//                                                        args.IsValid = false;
//                                                        return;
//                                                    }
//                                                }
//                                            }
//                                        }

//                                    }
                                   
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSendMailDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <span class="clsLabelHeader">Payment Advice will be sent to following Email-ID’s.</span>
                                                </td>
                                            </tr>
                                           <%-- <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToMailID" runat="server" CssClass="clsLabelAuto" Text="<%$AppSettings:PaymentAdviceToAC%>">
                                                    </asp:Label>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td colspan="2">
                                                    <span class="clsLabelHeader">Other than below Recipients, please enter comma separated
                                                        Email-ID's to sent the Payment Advice.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">To...</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMailIDs" runat="server"  Height="30px" Text="<%$AppSettings:PaymentAdviceToAC%>"
                                                        ClientIDMode="Static" CssClass="clsTextBoxLong1_Ajax"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Cc...</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCCIDs" runat="server" CssClass="clsTextBoxLong1_Ajax" Height="30px"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                           <%-- <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Bcc...</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBCCIDs" runat="server" CssClass="clsTextBoxLong1_Ajax" Height="50px"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSendMail" runat="server" CssClass="clsButton_Ajax" Text="Send"
                                                        ToolTip="Click to Send Requisition by Mail" ValidationGroup="1" CausesValidation="true" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to go back to the previous page" />
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

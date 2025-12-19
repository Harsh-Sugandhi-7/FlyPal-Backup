<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEncryptDecrypt.aspx.vb"
    Inherits="Flypal.wfEncryptDecrypt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Encrypt/Decrypt Text</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">Encrypt/Decrypt Text</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:ValidationSummary ID="Validationsummary1" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="2"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvPlainText" runat="server" Display="None" ControlToValidate="txtPlainText"
                                    ErrorMessage="Please enter Text to be Encrypted." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvEncryptedText" runat="server" Display="None" ControlToValidate="txtEncryptedText"
                                    ErrorMessage="Please enter Text to be Decrypted." ValidationGroup="2"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlOtherDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table8">
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblHeader" class="clsLabelHeader">Enter Plain Text in Format : &#39;True/False&#39;
                                                        + &#39;$$&#39; + &#39;Aircraft RegNo.&#39;</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblPlainText" class="clsLabelAuto">Plain Text</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtPlainText" runat="server" ClientIDMode="Static" CssClass="clsTextBoxMultilineTask_Ajax"
                                                        TextMode="MultiLine" MaxLength="100" ToolTip="Enter Text to be Encrypted" Width="576px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="imgEncrypt" ToolTip="Click to Encrypt Text" runat="server" Height="40px"
                                                        Width="40px" ImageUrl="images/down.png" ValidationGroup="1" />
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="imgDecrypt" runat="server" ToolTip="Click to Decrypt Text" Height="40px"
                                                        Width="40px" ImageUrl="images/up.png" ValidationGroup="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblEncryptedText" class="clsLabelAuto">Encrypted Text</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEncryptedText" runat="server" CssClass="clsTextBoxMultilineTask_Ajax"
                                                        TextMode="MultiLine" MaxLength="100" ToolTip="Enter Text to be decrypted">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                              <asp:Button ID="btnUpdateAllMachines" runat="server" CssClass="clsButton_Ajax" Text="Update All Machine's"
                                                        Width="130px" />
                                                            </td>
                                                            <td>
                                                              <asp:Button ID="btnUpdateNotInUseReadOnlyStatusOfAllMachines" runat="server" CssClass="clsButton_Ajax" Text="Update All Machine's Not In Use"
                                                        Width="190px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                  
                                                </td>
                                                <td align="right" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="clsButton_Ajax" Text="Update Machine"
                                                                    ValidationGroup="2" Width="100px" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                    Text="Close" />
                                                            </td>
                                                        </tr>
                                                    </table>
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
    <%--<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </asp:UpdateProgress>--%>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEncryptDecryptText();
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
                parent.IFrameEncryptDecryptTextStateComplete();
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

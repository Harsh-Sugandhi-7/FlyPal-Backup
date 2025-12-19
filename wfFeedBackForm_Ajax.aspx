<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFeedBackForm_Ajax.aspx.vb"
    Inherits="Flypal.wfFeedBackForm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FeedBack</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var popID = $('a.poplight[href^=#]').attr('rel');
            var popURL = $('a.poplight[href^=#]').attr('href');
            var query = popURL.split('?');
            var dim = query[1].split('&amp;');
            var popWidth = dim[0].split('=')[1];
            var tempwidth = $('#' + popID).outerWidth(true);

            //            $('#' + popID).fadeIn().css({ 'width': Number(tempwidth) }).prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -47 -46 0 782;" class="btn_close" title="Close" alt="Close" /></a>');
            $('#' + popID).fadeIn().css({ 'width': Number(tempwidth) - 60 });

            var popMargTop = ($('#' + popID).height() + 80) / 2;
            var popMargLeft = ($('#' + popID).width() + 80) / 2;

            $('#' + popID).css({
                'margin-top': 100,
                'margin-left': -popMargLeft
            });

            $pos = $('#' + popID).position();
            var top = $pos.top;
            var body = $('#' + popID).outerHeight(true);

            var temp = top + body + 50;
            var hight = ($(window).height() > temp ? $(window).height() : temp);
            $('#' + popID).css('padding', 0);

            $('body').append('<div id="fade"></div>');
            $('#fade').css({ 'filter': 'alpha(opacity=70)', 'width': '100%', 'height': hight, 'background': '#000' }).fadeIn();
            return false;
        });
        $('a.close').live('click', function () {
            $('#fade , .popup_block').fadeOut(function () {
                $('#fade, a.close').remove()
                var url = "Login.aspx";
                $(location).attr('href', url);
            });
            return false;
        });
    </script>
</head>
<body>
    <form id="form21" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <a class="poplight" href="#?w=500" style="direction:none;" rel="popup_name"></a>
    <div id="popup_name" class="popup_block" align="center">
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clstitle1">
                                    <span id="lblTitle" class="clstitle1">FeedBack</span></br></br> <span id="Label2"
                                        class="clstitle1">Please help us to improve our services by giving your valuable
                                        feedback.</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlFeedBack" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <fieldset class="clsFieldSet" style="border-width: 1px;text-align:center;">
                                                            <legend><b><span style="color: Red;">*</span> How likely is that you would recommend
                                                                FlyPal to your Aviation friend(s) or colleague(s)?</b></legend>
                                                            <asp:Label ID="lblLow" runat="server" Text="Least Recommended" CssClass="clsLabelHeader"
                                                                ForeColor="Red"></asp:Label>
                                                            &nbsp;
                                                            <asp:RadioButton ID="RadioButton1" runat="server" Text="1" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton2" runat="server" Text="2" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton3" runat="server" Text="3" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton4" runat="server" Text="4" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton5" runat="server" Text="5" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton6" runat="server" Text="6" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="red" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton7" runat="server" Text="7" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="Orange" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton8" runat="server" Text="8" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="Orange" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton9" runat="server" Text="9" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="Green" ForeColor="White" Font-Names="Arial Black" />
                                                            <asp:RadioButton ID="RadioButton10" runat="server" Text="10" GroupName="F" Font-Bold="True"
                                                                Font-Size="Larger" AutoPostBack="true" BackColor="Green" ForeColor="White" Font-Names="Arial Black" />&nbsp;
                                                            <asp:Label ID="lblHigh" runat="server" Text="Highly Recommended" Font-Bold="true"
                                                                ForeColor="Green"></asp:Label>
                                                        </fieldset>
                                                    </td>
                                                    <%-- <td>
                                                    <span class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="How likely is that you would recommend FlyPal to your Aviation friend(s) or colleague(s)?"
                                                        CssClass="clsLabelHeader"></asp:Label>
                                                </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <legend><b><span style="color: Red;">*</span>
                                                                <asp:Label ID="lblSuggestionQuestion" runat="server" Text="How can we improve your experience?"
                                                                    CssClass="clsLabelAuto" Font-Bold="true"></asp:Label></b></legend>
                                                            <asp:TextBox ID="txtSuggestionAnswer" runat="server" CssClass="clsTextBoxRemark_Ajax"
                                                                TextMode="MultiLine" Height="80px" autocomplete="off"></asp:TextBox>
                                                        </fieldset>
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
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="clsButton_Ajax"
                                                ClientIDMode="Static" ToolTip="Click to Submit FeedBack"></asp:Button>
                                            <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                CausesValidation="False"></asp:Button>
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
</body>
</html>

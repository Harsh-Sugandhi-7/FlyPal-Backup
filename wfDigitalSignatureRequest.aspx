<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDigitalSignatureRequest.aspx.vb" Inherits="Flypal.wfDigitalSignatureRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Digital Signature Request</title>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <asp:UpdatePanel ID="upnlNew" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="clstablelistout" id="tblmain">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="clsFormHeader1" style="width: 400px">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Digital Signature Request</asp:Label>
                                                                                </td>
                                                                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <td align="right">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                                            Text="New" ToolTip="Click to add new Place" />
                                                                                                    </td>

                                                                                                    <td>
                                                                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                                            Text="Close" ToolTip="Click to close Place screen" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>

                                                                                        </td>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <asp:UpdatePanel runat="server" ID="upnlValidation" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <td>
                                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                                                    ValidationGroup="a"></asp:ValidationSummary>
                                                                                <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAuthorizedUserList"
                                                                                    Display="None" ErrorMessage="Select Authorized User from the list." ClientValidationFunction="ValidateCity"
                                                                                    ValidationGroup="a"></asp:CustomValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvModuleName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtModuleName"
                                                                                    Display="None" ErrorMessage="Module Name Required." ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
                                                                                    Display="None" ErrorMessage="Description Required." ValidationGroup="a"></asp:RequiredFieldValidator>


                                                                                <script type="text/javascript">
                                                                                    function ValidateCity(source, args) {
                                                                                        args.IsValid = false;
                                                                                        var dd = $get("cmbAuthorizedUserList");
                                                                                        if (dd.selectedIndex != 0) {
                                                                                            args.IsValid = true;
                                                                                            return;
                                                                                        }
                                                                                    }
                                                                                </script>
                                                                            </td>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div>
                                                    <asp:UpdatePanel runat="server" ID="upnlDS" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 100%">
                                                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -5px">

                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="lblAuthorizedUserListStar" class="clsLabelStar">*</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblAuthorizedUserList" class="clsLabelAuto">Authorized User</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbAuthorizedUserList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                                                                DataValueField="UserID"
                                                                                                ClientIDMode="Static">
                                                                                            </asp:DropDownList>

                                                                                        </td>

                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td></td>
                                                                                        <td>
                                                                                            <span id="lblName" class="clsLabelAuto">Module Name</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtModuleName" runat="server" CssClass="clsTextBoxTagSearch" Enabled="false"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="lblDescriptionStar" class="clsLabelStar">*</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblDescription" class="clsLabelAuto">Description</span>

                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtDescription" runat="server" autocomplete="off" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" MaxLength="1000" TextMode="MultiLine">
                                                                                            </asp:TextBox>

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>

                                                                                        <td colspan="3" align="right">
                                                                                            <asp:Button ID="btnSubmit" runat="server" CssClass="clsbtnH clsinfoH1" Text="Submit" ToolTip="Click to submit the request"
                                                                                                ValidationGroup="a" />
                                                                                        </td>
                                                                                    </tr>

                                                                                </table>
                                                                            </fieldset>
                                                                        </div>
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
        <!--call parent function after completing subroutine..(when page open as popup)-->
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForDigitalSignatureRequest();
                return false;
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
  <% Dim mopen As String = Request.QueryString("Type") %>
  <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameDigitalSignatureRequestStateComplete();
                }


            });
  <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
    <% Dim mopenas As String = Request.QueryString("Type") %>
       <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
        <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
</body>
</html>

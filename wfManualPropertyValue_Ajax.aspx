<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualPropertyValue_Ajax.aspx.vb"
    Inherits="Flypal.wfManualPropertyValue_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manual Property Value Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain" style="width:450px">
        <tr>
            <td>
                <table id="tblInner" class="clstablelistin">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="clsFormHeader1Newstyle" colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Manual Property Value Information</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="OK" ValidationGroup="a" ToolTip="Click to Add Manual Property Value"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="a"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvProperty" runat="server" ClientValidationFunction="validateName"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbPropertyList" ErrorMessage="Manual Property Required."></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Manual Property Value should be less than or equal to 50 charecters."
                                                    Display="None" ControlToValidate="txtName" ClientValidationFunction="validateName"
                                                    ValidationGroup="a"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                    ErrorMessage="Manual Property Value Required." Display="None" ValidationGroup="a"
                                                    CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function validateName(source, args) {
                                                        var ControlName = source.controltovalidate;
                                                        switch (ControlName) {
                                                            case 'txtName':
                                                                var Value = $get(ControlName).value.length;
                                                                if (Value > 50) {
                                                                    args.IsValid = false;
                                                                    return
                                                                }
                                                                break;
                                                            case 'cmbPropertyList':
                                                                var Value = $get(ControlName);
                                                                if (Value.selectedIndex == 0) {
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
                                    <td colspan="4">
                                        <span id="lblManualPropertyValue" class="clsLabelHeader">Manual Property Value</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlManualPropertyValue" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblPropertyStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblProperty" class="clsLabelAuto">Property</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbPropertyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                SelectedValue="<%# mManual.ManualPropertyValues.CurrentItem.ManualPropertyID %>"
                                                                DataValueField="ID" DataTextField="Name">
                                                            </asp:DropDownList>
<%--                                                            <asp:Button ID="btnAddProperty" runat="server" CssClass="clsButtonGrid_Ajax" Text="..." ValidationGroup="a"
                                                                CausesValidation="false" ToolTip="Click To Add new Property"></asp:Button>--%>

                                                            <asp:ImageButton ID="btnAddProperty" runat="server" ImageUrl="~/images/plus1.png"
                                                                Height="22px" Width="24px" ToolTip="Click to Add New Property" CausesValidation="False" ></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblValueStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblValue" class="clsLabelAuto">Value</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mManual.ManualPropertyValues.CurrentItem.Value %>"
                                                                ToolTip="Enter Property Value" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td align="right" colspan="4">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="OK" ValidationGroup="a"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH" Text="Back">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnManualPropertyValue" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
    <!--PropertyValue Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyManualPropertyValue" Text="PropertyValue"
            CausesValidation="true" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlManualPropertyValue" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeManualPropertyValue" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPropertyValue" runat="server" TargetControlID="btnDummyManualPropertyValue"
        PopupControlID="pnlManualPropertyValue" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameManualPropertyStateComplete() {
            $("#btnDummyManualPropertyValue").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenManualPropertyValueWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeManualPropertyValue").attr("src", "wfManualProperty_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyManualPropertyValue").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForManualProperty() {
            var ManualPropertyValueWindow = $find("<%=mdlPopupPropertyValue.ClientID %>");
            //close popup window
            ManualPropertyValueWindow.hide();
            //release resources
            $("#IframeManualPropertyValue").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnManualPropertyValue").click();
        }
    </script>
    <!-- End-->
    <!--call parent function after completing subroutine..(when page open as popup)-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPropertyValue();
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
             parent.IFramePropertyValueStateComplete();
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

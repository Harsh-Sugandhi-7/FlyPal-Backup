<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceInvoiceCharge_Ajax.aspx.vb"
    Inherits="Flypal.wfLineMaintenanceInvoiceCharge_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Service Invoice Charge</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="5" topmargin="5" rightmargin="5">
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
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table class="clstablelistin" id="tblLedgerList">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Service Invoice Charge</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                            CausesValidation="False" Text="Back"></asp:Button>
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
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvCharge" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Charge Name Required" ControlToValidate="cmbCharge"></asp:CustomValidator><asp:CustomValidator
                                                        ID="cvPercentage" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                        Display="None" ErrorMessage="Percentage should  be Greater than 0" ControlToValidate="txtPercentage"></asp:CustomValidator><asp:CustomValidator
                                                            ID="cvAmount" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                            Display="None" ErrorMessage="Amount should be Greater than 0" ControlToValidate="txtChargeAmount"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlOtherChargeDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="lblOtherChargeDetails" class="clsLabelHeader">Other Charge Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarCharge" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCharge" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                DataTextField="Name" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <%-- <asp:Button ID="imgbtnCharge" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Charge"
                                                        CausesValidation="False" Text="..."></asp:Button>--%>

                                                            <asp:ImageButton ID="imgbtnCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                ToolTip="Click to Add New Charge" CausesValidation="False"></asp:ImageButton>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblPercentage" class="clsLabel">Percentage </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearch"
                                                                ToolTip="Enter Percentage" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceCharges.currentItem.Percentage %>"
                                                                MaxLength="12" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblChargeAmount" class="clsLabelAuto">Charge Amount </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearch"
                                                                ToolTip="Enter Charge Amount" Text="<%# mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.CChargeAmount %>"
                                                                MaxLength="12" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr style="height: 0px;">
                                    <td colspan="2" style="height: 0px;">
                                        <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnimgBtnChargeList" ClientIDMode="Static" runat="server" Text="----"
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
        </div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForLineMaintenanceInvoiceCharge();
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
                    parent.IFrameLineMaintenanceInvoiceChargeStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
        <!-- Charge List Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyChargeList" Text="Dummy Charge List"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupChargeList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupChargeList" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupChargeList" runat="server" TargetControlID="btnDummyChargeList"
            PopupControlID="pnlPopupChargeList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameChargeListStateComplete() {
                var UpdatePanel1 = '<%=upnlValidationSummary.ClientID%>';
                if (Page_IsValid) {
                    $("#btnDummyChargeList").click();
                    //$get("AjaxLoader").style.visibility = "hidden";
                }
                else {
                    __doPostBack(UpdatePanel1, '');
                    //$get("AjaxLoader").style.visibility = "hidden";
                }
            }

            function OpenChargeWindow() {
                try {

                    //$get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupChargeList").attr("src", "wfCharge_Ajax.aspx?Typepup=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyChargeList").click();
                        //$get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForChargeList() {
                var ChargeListWindow = $find("<%=mdlPopupChargeList.ClientID %>");
                //close Common Part List popup window
                ChargeListWindow.hide();
                $("#iPopupChargeList").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnChargeList").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>

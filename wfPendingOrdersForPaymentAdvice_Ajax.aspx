<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingOrdersForPaymentAdvice_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingOrdersForPaymentAdvice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Pending Orders For Payment Advice</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblOrderNo" class="clsLabel">Order No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                            DataValueField="Text" DataTextField="Text">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="6"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                        ToolTip="Click to find list of Order as per searching criteria." />
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
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlPendingOrdersForPaymentAdvice" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgPendingOrdersForPaymentAdvice" runat="server" CssClass="clsGrid"
                                                ClientIDMode="Static" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                                                PageSize="25" AllowPaging="True">
                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="hideGridColumn" />
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="hideGridColumn" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNumber" Text="<%# Container.DataItemIndex + 1 %>" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="OrderTextNo" HeaderText="Order No. ">
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle Wrap="False" />
                                                        <FooterStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
                                                    <asp:BoundField DataField="OrderAmountWithCurrency" HeaderText="Total Order Value(In Order Currency)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Order Value(Base Currency)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="PendingPaymentAdviceCAmountWithCurrency" HeaderText="Pending Amount(In Order Currency)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="PendingPaymentAdviceAmount" HeaderText="Pending Amount(Base Currency)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:ButtonField Text="Select" HeaderText="Select" HeaderStyle-HorizontalAlign="Left"
                                                        CommandName="Select"></asp:ButtonField>
                                                    <asp:ButtonField Text="Remove" HeaderText="Remove From Pending List" HeaderStyle-HorizontalAlign="Left"
                                                        Visible="false" CommandName="RemoveFromPendingList"></asp:ButtonField>
                                                </Columns>
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClearPendingList" runat="server" CssClass="clsButton_Ajax" Width="200px"
                                                Text="Remove Order(s) from Pending List" ToolTip="Click to clear Order(s) from pending list"
                                                CausesValidation="False"></asp:Button>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to the previous page"
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
    <div>
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
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgPendingOrdersForPaymentAdvice tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        SetRow($(this));
                    }
                    else {
                        $(this).removeAttr("checked");
                        SetRow($(this));
                    }

                });
            });
        });

        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }

        function pageLoad() {
            var status;
            $("#dgPendingOrdersForPaymentAdvice tr:gt(0)").find(":checkbox").each(function () {
                status = $(this).attr("checked");
                if (status == "checked") {
                    SetRow($(this));
                }
                else {
                    //$(this).removeAttr("checked");
                    SetRow($(this));
                }

            });

        }
    </script>
    <!-- Payment Advice Order Detail Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyOrdersForPaymentAdvice" Text="" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlOrdersForPaymentAdvice" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeOrdersForPaymentAdvice" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupOrdersForPaymentAdvice" runat="server" TargetControlID="btnDummyOrdersForPaymentAdvice"
        PopupControlID="pnlOrdersForPaymentAdvice" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameOrdersForPaymentAdviceStateComplete() {
            $("#btnDummyOrdersForPaymentAdvice").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenOrdersForPaymentAdviceWindow() {
            try {

                //$get("AjaxLoader").style.visibility = 'visible';
                $("#IframeOrdersForPaymentAdvice").attr("src", "wfOrderAndInvoiceDetail_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyOrdersForPaymentAdvice").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionOrdersForPaymentAdvice() {
            var OrdersForPaymentAdvicewindow = $find("<%=mdlPopupOrdersForPaymentAdvice.ClientID %>");
            //close Payment Advice popup window
            OrdersForPaymentAdvicewindow.hide();
            //           release resources
            $("#IframeOrdersForPaymentAdvice").attr("src", "JavaScript:''");

            //call image button

            $("#hdnBtnOrdersForPaymentAdvice").click();

        }
        function ParentCallBackFunction() {
            var OrdersForPaymentAdvicewindow = $find("<%=mdlPopupOrdersForPaymentAdvice.ClientID %>");
            //close Payment Advice popup window
            OrdersForPaymentAdvicewindow.hide();
            //           release resources
            $("#IframeOrdersForPaymentAdvice").attr("src", "JavaScript:''");

            //call image button

            $("#hdnBtnOrdersForPaymentAdvice").click();
            parent.ParentPendingOrdersForPaymentAdvice();
            return false;
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentPendingOrdersForPaymentAdvice();
            return false;
        }
       
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
    //   if ($.browser.msie) {
             parent.IFramePendingOrdersForPaymentAdviceStateComplete();
 //      }
         
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
</body>
</html>

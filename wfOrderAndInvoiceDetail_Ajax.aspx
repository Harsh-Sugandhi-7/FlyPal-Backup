<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOrderAndInvoiceDetail_Ajax.aspx.vb"
    Inherits="Flypal.wfOrderAndInvoiceDetail_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Order For Payment Advice Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
                    <asp:UpdatePanel runat="server" ID="upnlOrderandInvoiceDetail" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Order & Invoice Details</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                        Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                    <asp:CustomValidator ID="CustValidator" runat="server" ControlToValidate="txtSupplierDate"
                                                        Display="None" OnServerValidate="customvalidate" ValidationGroup="1"></asp:CustomValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblOrderDate" class="clsLabel">Order Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="calPaymentDate" runat="server" Enabled="false" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderDateFormatted %>"
                                                                CssClass="clsTextBox_Ajax" ReadOnly="true" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="lblRef" class="clsLabel">Order No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrderNo" Enabled="false" runat="server" ReadOnly="true" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderTextNo %>"
                                                                CssClass="clsTextBox_Ajax" onfocus="SetContextKey()" ToolTip="Enter No." MaxLength="25"
                                                                Width="140px"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span1" class="clsLabel">Supplier</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupplier" runat="server" Enabled="false" ClientIDMode="Static"
                                                                CssClass="clsTextBox2_Ajax" ReadOnly="true" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.VendorName %>"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <%--<span id="Span2" class="clsLabel">Currency</span>--%>
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtCurrency" runat="server" 
                                       ReadOnly="true"  CssClass="clsTextBox_Ajax" onfocus="SetContextKey()" Width="140px"> </asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span3" class="clsLabel">Advice Value in</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtTotalValue" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                                                MaxLength="12" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.COrderAmount %>"
                                                                CssClass="clsTextBox_Ajax" Width="100px"></asp:TextBox>
                                                            <asp:TextBox ID="txtCurrency" runat="server" Enabled="false" Text="<%# mPaymentAdvice.CurrencyName %>"
                                                                ReadOnly="true" CssClass="clsTextBox_Ajax" onfocus="SetContextKey()" Width="100px"> </asp:TextBox>
                                                            <span id="Span6" class="clsLabel">@</span>
                                                            <asp:TextBox ID="txtConversionFactor" runat="server" Enabled="false" Text="<%# mPaymentAdvice.ConversionFactor %>"
                                                                ReadOnly="true" CssClass="clsTextBox_Ajax" onfocus="SetContextKey()" Width="100px"> </asp:TextBox>
                                                            <asp:TextBox ID="txtCTotalValue" runat="server" Enabled="false" ClientIDMode="Static"
                                                                CssClass="clsTextBox_Ajax" ReadOnly="true" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount %>"
                                                                Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span2" class="clsLabel">Value (In Order Currency)</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtPAValueInOrderCurr" runat="server" Enabled="false" Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.PAValueInOrderCurrency %>"
                                                                ReadOnly="true" CssClass="clsTextBox_Ajax" onfocus="SetContextKey()" Width="150px"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span4" class="clsLabel">Supplier Inv. Date</span>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSupplierDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                                            Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.SupplierInvoiceDateFormatted %>"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','true');" Width="100px"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calPaymentDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtSupplierDate">
                                                                        </cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="calPaymentDateWatermarkExtender" runat="server"
                                                                            TargetControlID="txtSupplierDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                        </cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span5" class="clsLabel">No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNo" runat="server" ClientIDMode="Static" MaxLength="50" CssClass="clsTextBox_Ajax"
                                                                            Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.SupplierInvoiceNo %>"
                                                                            Width="100px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span7" class="clsLabel">Remark</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtRemark" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                                                Text="<%# mPaymentAdvice.PaymentAdviceItems.CurrentItem.Remark %>" CssClass="clsTextBoxMultiLine1_Ajax"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="4">
                                                            <asp:Button ID="btnAdd" runat="server" CausesValidation="true" ValidationGroup="1"
                                                                CssClass="clsButton_Ajax" Text="OK" ToolTip="Click to Save Payment Advice" />
                                                            &nbsp;
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to go back to the previous page" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunction();
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
                    parent.IFrameOrdersForPaymentAdviceStateComplete();
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
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
</body>
</html>

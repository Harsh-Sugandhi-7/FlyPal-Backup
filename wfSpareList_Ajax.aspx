<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSpareList_Ajax.aspx.vb"
    Inherits="Flypal.wfSpareList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Required Spare Parts</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                    <table class="clsTablelistin" id="tblinner">
                        <asp:PlaceHolder ID="PlaceHolderTaskCard" runat="server">
                            <tr>
                                <td class="clsFormHeader1">
                                    <span id="lblTaskCardTitle" class="clsFormHeader" runat="server">Task Card</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgTaskList" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                PageSize="3">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No.">
                                                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" Wrap="False" />
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" Wrap="False" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Task" HeaderText="Task">
                                                        <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="500px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Note" HeaderText="Note">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlaceHolderSpare" runat="server">
                        <tr>
                            <td class="clsFormHeader1">
                                <span id="lblSpareTitle" class="clsFormHeader" runat="server">Required Spare Parts</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgPartList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5"
                                            EnableViewState="false" PageSize="50" AllowPaging="true" AllowSorting="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                           <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right"></PagerStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundField>
                                                <asp:BoundField DataField="ItemName" SortExpression="PartNo" ReadOnly="True" HeaderText="Part No.">
                                                    <HeaderStyle Wrap="true"  HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemDescription" SortExpression="Description" HeaderText="Description">
                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SpareQty" HeaderText="Required Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StockBalanceQty" HeaderText="Stock Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="On Order Qty." DataField="OnOrderQty">
                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastInvoiceRate" HeaderText="Invoice Rate">
                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastPurchaseAmt" HeaderText="Base Rate">
                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                         </asp:PlaceHolder>
                         <asp:PlaceHolder ID="PlaceHolderTools" runat="server">
                        <tr>
                            <td class="clsFormHeader1">
                                <span id="lblToolsTitle" class="clsFormHeader" runat="server">Tools</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgKitList" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                            PageSize="3" GridLines="Horizontal" CellPadding="5">
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle HorizontalAlign="Right" />
                                            <RowStyle CssClass="clsdgItem" />
                                             <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Part No.">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                         </asp:PlaceHolder>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                        Text="Back" ToolTip="Click to go Back to Previous Page" />
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
            parent.ParentCallBackFunctionForSpareList();
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
                    parent.IFrameSpareListStateComplete();
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
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSparesInspectionKitList_Ajax.aspx.vb"
    Inherits="Flypal.wfSparesInspectionKitList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Spares Inspection Kit Information</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>


                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Spares Inspection kit List</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlAddButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnBack" ValidationGroup="1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close Spares Inspection Kit List" CausesValidation="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgKitList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                CssClass="clsGridNewStyle" AllowPaging="True" PageSize="25" ShowHeaderWhenEmpty="true"
                                                PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="Id" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="KitName" HeaderText="Inspection Kit Name" SortExpression="KitName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemCount" HeaderText="Item Count" SortExpression="ItemCount">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="SelectRec" HeaderText="Select" Text="Select">
                                                        <HeaderStyle Width="10px" HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </form>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForInspKit();
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
                parent.IFrameInspKitStateComplete();
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
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReqItemsViewForWO_Ajax.aspx.vb"
    Inherits="Flypal.wfReqItemsViewForWO_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition(s)</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
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
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblinner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">Requisition Item(s)</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <table id="Table1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                          Text="Close" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" ID="upnlIndents" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <%--<td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <table id="Table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                                            Visible="<%# mRequisitionItemsNew.Count > 25 %>" Text="Close" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlIndentList" UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Requisition Item(s) : 0 Record(s) found.</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="dgIndents" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                                                AutoGenerateColumns="False" AllowSorting="True" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                                <Columns>
                                                                                    <%--<asp:BoundField DataField="RequisitionNo" HeaderText="Requisition No.">
                                                                                    <HeaderStyle Wrap="False" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                                </asp:BoundField>--%>
                                                                                    <asp:TemplateField HeaderText="Requisition No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lblReqNoLabel" runat="server" Text='<%# Eval("RequisitionNo") %>'
                                                                                                CommandName="ReqNo" CommandArgument='<%#Eval("ReqID") %>'>
                                                                                            </asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="ReqDateFormatted" HeaderText="Requisition Date">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="AlternatePart" HeaderText="Alt. Part.">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="True" CssClass="TextBreak"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="RequestedQty" HeaderText="Qty.">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnBottom" UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <table id="Table2">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseBottom" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                                                Text="Close" CausesValidation="False" Visible="true"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>--%>
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForRequisitionView();
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
                    parent.IFrameRequisitionViewComplete();
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
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmpCAAuthorizationHistory_Ajax.aspx.vb" Inherits="Flypal.wfEmpCAAuthorizationHistory_Ajax" %>

<%@ Import Namespace="Flypal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company Authorization List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>

                                                                    <asp:Label ID="LblTitle" runat="server" CssClass="clsFormHeader">Company Authorization History List
                                                                    </asp:Label>

                                                                </td>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>

                                                                            <td>
                                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                    Text="Close" ToolTip="Click to Close" />
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
                                </td>
                            </tr>


                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">

                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgEmpCAAuthorizationHistoryList" runat="server" AllowSorting="False" AllowPaging="true"
                                                            AutoGenerateColumns="False" CellPadding="10" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                            EnableViewState="true" GridLines="Horizontal"
                                                            PageSize="10" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                            <Columns>
                                                                 <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />

                                                                <%--1--%>
                                                                <asp:BoundField DataField="EmpCAAuthorizationDate" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="Number" HeaderText="No." SortExpression="CAAuthorizationNo" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="CANumber" HeaderText="CA No." SortExpression="CANumber">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="EmployeeName" HeaderText="Employee" SortExpression="Employee">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="EmployeeCode" HeaderText="Code" SortExpression="Employee">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="CAInitialIssueDate" HeaderText="Issue Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="CAValidUpto" HeaderText="Valid Upto">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="RevisionNo" HeaderText="Revision No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="RevisionDate" HeaderText="Revision Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:ButtonField CommandName="DetailView" HeaderText="Detail" Text="Detail">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
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
                parent.ParentCallBackFunctionForEmpCAAuthorizationHistory();
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
                    parent.IFrameEmpCAAuthorizationHistoryStateComplete();
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


        <!--EmpCAAuthorization Details Form Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpCAAuthorizationDetails" Text="EmpCAAuthorizationDetails"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpCAAuthorizationDetails" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpCAAuthorizationDetails" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpCAAuthorizationDetails" runat="server" TargetControlID="btnDummyEmpCAAuthorizationDetails"
            PopupControlID="pnlEmpCAAuthorizationDetails" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpCAAuthorizationDetailsStateComplete() {
                $("#btnDummyEmpCAAuthorizationDetails").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }
            function OpenEmpCAAuthorizationDetailsWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpCAAuthorizationDetails").attr("src", "wfEmpCAAuthorization_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpCAAuthorizationDetails").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForEmpCAAuthorizationDetails() {
                var EmpCAAuthorizationDetailswindow = $find("<%=mdlPopupEmpCAAuthorizationDetails.ClientID %>");
                //close EmpCAAuthorization History popup window
                EmpCAAuthorizationDetailswindow.hide();
                //           release resources
                $("#IframeEmpCAAuthorizationDetails").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpCAAuthorizationDetails").click();
            }
        </script>
        <!-- End-->

    </form>
</body>
</html>

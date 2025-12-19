<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDocumentHistoryList_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDocumentHistoryList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Document History List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
                 
        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body ms_positioning="GridLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDocumentHistoryDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td class="clsFormHeader1Newstyle">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lblListOrder" class="clsFormHeader">Employee Document History List</span>
                                                            </td>

                                                            <td align="right">
                                                                <table class="clstableButton" align="right">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close "
                                                                                Text="Close"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--<div style="width: 660px;">
                                                        <table class="clsGrid" style="width: 660px; border-collapse: collapse;" cellpadding="0"
                                                            cellspacing="0">
                                                            <tr>
                                                                <td width="110px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeDocumentHistoryList','$DocumentName')">
                                                                        Document Name</a>
                                                                </td>
                                                                <td width="60px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeDocumentHistoryList','$DocNo')">
                                                                        Doc. No.</a>
                                                                </td>
                                                                <td width="90px" class="clsdgHeader">
                                                                    <span>Date of Issue</span>
                                                                </td>
                                                                <td width="100px" class="clsdgHeader">
                                                                    <a style="color: white;">Place Of Issue</a>
                                                                </td>
                                                                <td width="150px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeDocumentHistoryList','$IssuingAuthority')">
                                                                        Issuing Authority</a>
                                                                </td>
                                                                <td width="100px" class="clsdgHeader">
                                                                    <a style="color: white;" href="javascript:__doPostBack('dgEmployeeDocumentHistoryList','$Remark')">
                                                                        Remark</a>
                                                                </td>
                                                                <td width="50px" class="clsdgHeader">
                                                                    <span>Attach</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>--%>

                                                    <div>
                                                        <asp:GridView ID="dgEmployeeDocumentHistoryList" runat="server" AllowPaging="true" PageSize="5"
                                                            ClientIDMode="Static" AutoGenerateColumns="False" AllowSorting="True" Style="width: 660px;"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeader="True" DataKeyNames="ID" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
															<PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DocumentName" SortExpression="DocumentName" HeaderText="Document Name">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DocNo" SortExpression="DocNo" HeaderText="Doc. No.">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DateOfIssueFormatted" HeaderText="Date of Issue">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PlaceOfIssue" SortExpression="PlaceOfIssue" HeaderText="Place Of Issue">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="DateOfExpiryFormatted" HeaderText="Date of Expiry">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssuingAuthority" SortExpression="IssuingAuthority" HeaderText="Issuing Authority">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                    <HeaderStyle></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="170px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="View">
                                                                    <ItemStyle CssClass="TextBreak"  Width="50px" Wrap="true" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                       <%-- <asp:ImageButton ID="lnkDocumentView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="Attach" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                            Visible='<%#  Eval("ImageSize")>0 %>' />--%>
                                                                        <asp:ImageButton ID="lnkDocumentView" runat="server" CommandArgument='<%# Eval("ID") %>' 
                                                                            CommandName="Attach" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("ImageSize")>0 %>'/>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td align="right">
                                                    <table class="clstableButton" align="right">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close to Go back to the Previous Screen"
                                                                    Text="Close"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
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
            parent.ParentCallBackFunctionForEmpDocumentHistory();
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
                parent.IFrameEmpDocumentHistoryStateComplete();
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfConditionCheckItemHistoryList_Ajax.aspx.vb"
    Inherits="Flypal.wfConditionCheckItemHistoryList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Equipment Maintenance Item History List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
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
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblListOrder" runat="server" CssClass="clsFormHeader">Equipment Maintenance Item History List</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table class="clstableButton" align="right">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close to Go back to the Previous Screen"></asp:Button>
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
                            <td align="right">
                                <table>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:GridView ID="dgConditionCheckItemHistoryList" runat="server" AllowPaging="True"
                                    AllowSorting="True" DataKeyNames="ID" AutoGenerateColumns="False" 
                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                    PageSize="25" ShowHeaderWhenEmpty="True">
                                    <RowStyle CssClass="clsdgItem" />
                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                            ItemStyle-CssClass="hideGridColumn">
                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                            <HeaderStyle HorizontalAlign="Left"  Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Frequency" HeaderText="Interval" SortExpression="Frequency">
                                            <HeaderStyle  HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConditionCheckInterval" HeaderText="Interval In" SortExpression="ConditionCheckInterval">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConditionCheckNo" HeaderText="No." SortExpression="ConditionCheckNo">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DoneOnDate" HeaderText="Done On Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConditionCheckServicedInspected" HeaderText="Condition Check/Serviced Inspected"
                                            SortExpression="ConditionCheckServicedInspected">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                    Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table class="clstableButton" align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to Close to Go back to the Previous Screen">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForConditionCheckItemHistory();
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
             parent.IFrameConditionCheckItemHistoryStateComplete();
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
</html>

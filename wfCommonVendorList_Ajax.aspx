<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCommonVendorList_Ajax.aspx.vb"
    Inherits="Flypal.wfCommonVendorList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Vendor List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
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
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblVendorList" class="clsFormHeader">Vendor List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="clstableButton">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOkTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add selected Supplier"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                    ToolTip="Click to go back to the previous page"></asp:Button>
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
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Vendors : 100 Record(s) found.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgbtnAddNewSupplier" runat="server" ImageUrl="~/images/plus1.png"
                                                                    Height="22px" Width="24px" ToolTip="Click to add new Supplier in Master" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table class="clstableButton">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOkTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add selected Supplier">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to go back to the previous page"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgVendorList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                        AllowSorting="True" ShowHeaderWhenEmpty="true" EnableViewState="false" DataKeyNames="ID"
                                                        ClientIDMode="Static">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging"></PagerStyle>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Vendor">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cityname" SortExpression="cityname" HeaderText="City">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="statename" SortExpression="statename" HeaderText="State">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="countryname" SortExpression="countryname" HeaderText="Country">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="contactperson" SortExpression="contactperson" HeaderText="Contact Person">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstableButton">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOk" runat="server" Text="Ok" ToolTip="Click to add selected Supplier" 
                                                        CssClass="clsbtnH clsinfoH1" Visible="false">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to go back to the previous page" Visible="false">
                                                    </asp:Button>
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
            parent.ParentCallBackFunctionForSupplier();
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
                parent.IFrameSupplierStateComplete();
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

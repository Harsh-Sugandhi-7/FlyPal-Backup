<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectListForNewRequisition_Ajax.aspx.vb"
    Inherits="Flypal.wfSelectListForNewRequisition_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Work Order List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlWODetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="clsFormHeader1Newstyle">
                                                    <table>
                                                        <td style="width: 99%" valign="middle">
                                                            <span id="lblTitle"  class="clsFormHeader">Work Order List</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" CausesValidation="False"
                                                                        ToolTip="Click to close WorkOrder List screen"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlWO" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblWOText" runat="server" class="clsLabel">WO.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbWO" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="WOText"
                                                                            DataValueField="WOText" AutoPostBack="true">
                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblNo" runat="server" class="clsLabel">No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall"  MaxLength="4"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblRegNo" runat="server" class="clsLabel">Reg. No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearchSmall" 
                                                                            MaxLength="10"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                            ToolTip="Click to find list of Receipt as per searching criteria" />--%>
                                                                             <asp:ImageButton ID="btnFindNow" runat="server" ValidationGroup="a" ImageUrl="~/images/Search2.png"
                                                                CssClass="clsSearch2btn" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgWOList" runat="server" CssClass="clsGridNewStyle" EnableViewState="False"
                                                        ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10" AutoGenerateColumns="False"
                                                        AllowSorting="true" DataKeyNames="Id,MachineID" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="Id" HeaderText="Id"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField Visible="False" DataField="MachineID" HeaderText="MachineID"></asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="WODateFormatted" HeaderText="WO Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="WONumber" HeaderText="WO No." SortExpression="WONumber">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:ButtonField CommandName="SparesView" DataTextField="SpareCountInString" HeaderText="View Spares">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:ButtonField>
                                                            <%--6--%>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                <HeaderStyle HorizontalAlign="Left" />
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
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" CausesValidation="False"
                                            ToolTip="Click to close WorkOrder List screen"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
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
            parent.ParentCallBackFunctionForWOList();
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
                parent.IFrameWOListStateComplete();
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
    <!-- Spare List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySpareList" Text="Spare List" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSpareList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSpareList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSpareList" runat="server" TargetControlID="btnDummySpareList"
        PopupControlID="pnlSpareList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSpareListStateComplete() {
            $("#btnDummySpareList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSpareListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSpareList").attr("src", "wfnWOJobSpareView_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySpareList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForSpareList() {
            var SpareListwindow = $find("<%=mdlPopupSpareList.ClientID %>");
            //close popup window
            SpareListwindow.hide();
            //           release resources
            $("#IframeSpareList").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSpareList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>

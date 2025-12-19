<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionItemSearch_Ajax.aspx.vb"
    Inherits="Flypal.wfRequisitionItemSearch_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition Item Search</title>
   <%-- <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
      <link href="Styles.css" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin" border="0">
                        <tr>
                             <td colspan="2">                                                 
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <table>
                                                    <tr>
                                                        <td style="width: 99%" valign="middle">
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Requisition Part No. Selection</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnCreate" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to add New Part in Part Master"
                                                                Text="Create" ValidationGroup="1" CausesValidation="true" Visible="<%# mRequisitionNew.TransTypeID=71 %>">
                                                            </asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Requisition Part No. selection screen"
                                                                Text="Close" CausesValidation="False"></asp:Button>
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
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                    ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" CssClass="clsLabelAuto"
                                    ControlToValidate="txtPartCreate" Display="None" ErrorMessage="Part No Required"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabelAuto"
                                    ControlToValidate="txtDescription" Display="None" ErrorMessage="Description Required"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblHeading" class="clsLabelHeader">Search Part No. to add in Requisition(Shows
                                    only 100 Records)</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabel">Part No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        ToolTip="Enter Part No."></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="Span1" class="clsLabel">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescriptionSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                        MaxLength="200" ToolTip="Enter Description."></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                   <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Part No"
                                                        Text="Find Now" CausesValidation="False"></asp:Button>--%>
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ValidationGroup="a" ImageUrl="~/images/Search2.png"
                                                                CssClass="clsSearch2btn" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts :  Record(s) found.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="left">
                                                    <asp:GridView ID="dgPartList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true" EnableViewState="false" DataKeyNames="ItemId,PartNo"
                                                        PageSize="25" AllowPaging="True" AllowSorting="True" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="PartNo" SortExpression="PartNo" ReadOnly="True" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="AlternatePart" HeaderText="Alternate Parts">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="FirstPriorityPart" HeaderText="First Priority Part">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField HeaderText="One Time Purchase" SortExpression="OneTimePurchase" DataField="OneTimePurchase">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="AvailableQtyForItemGrid" SortExpression="AvailableQtyForItemGrid"
                                                                HeaderText="Available Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="Unit" SortExpression="Unit" HeaderText="Unit">
                                                                <HeaderStyle ForeColor="black"></HeaderStyle>
                                                                <ItemStyle></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField HeaderText="Contracted With Supplier" SortExpression="ContractedVendorName"
                                                                DataField="ContractedVendorName">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <%--10--%>
                                                            <asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
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
                            <td colspan="2" align="left">
                                <asp:Label ID="Label1" CssClass="clsLabelHeader" runat="server" Visible="<%# mRequisitionNew.TransTypeID=71 %>">Enter Part No. and Description for new Part</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="lblPartCreate" CssClass="clsLabelAuto" Visible="<%# mRequisitionNew.TransTypeID=71 %>">Part No</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPartCreate" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="50"
                                    ToolTip="Enter Part No" Visible="<%# mRequisitionNew.TransTypeID=71 %>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblDescription" CssClass="clsLabelAuto" runat="server" Visible="<%# mRequisitionNew.TransTypeID=71 %>">Description</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="100"
                                    Visible="<%# mRequisitionNew.TransTypeID=71 %>" ToolTip="Enter Part Description"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <%--<td>
                                                    <asp:Button ID="btnCreate" runat="server" CssClass="clsButton_Ajax" ToolTip=" Click to add New Part in Part Master"
                                                        Text="Create" ValidationGroup="1" CausesValidation="true" Visible="<%# mRequisitionNew.TransTypeID=71 %>">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Requisition Part No. selection screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
            parent.ParentCallBackFunctionForRequisitionItemSearch();
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
                parent.IFrameRequisitionItemSearchStateComplete();
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

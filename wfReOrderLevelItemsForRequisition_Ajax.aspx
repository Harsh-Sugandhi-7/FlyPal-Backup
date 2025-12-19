<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReOrderLevelItemsForRequisition_Ajax.aspx.vb"
    Inherits="Flypal.wfReOrderLevelItemsForRequisition_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Re-Order Level Items</title>
   <%-- <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
   <link href="Styles.css" rel="stylesheet" type="text/css" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });


        });
    </script>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        window.onload = blinknow;
        function blinknow() {
            var e = document.getElementById("<%=lblCount.ClientID%>");
            if (e != null) {
                e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
                setTimeout("blinknow();", 990);
            }
        }    
    </script>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td class="style1">
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin" border="0">
                        <%--  <tr>
                            <td>
                                <span id="lblTitle" class="clstitle1" runat="server">Requisition Part No. Selection</span>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lblTitle" class="clsFormHeader" runat="server">Requisition Part No. Selection</span>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnOkTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add the selected item" />
                                                                <asp:Button ID="btnExportTop" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Export to Excel" ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                                </asp:Button>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" />
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
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblCount" CssClass="clsLabelHeader"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                    ValidationGroup="1"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<span id="lblHeading" class="clsLabelHeader">Search Part No. to add in Requisition</span>--%>
                                <asp:UpdatePanel ID="upnlShowEntries" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        &nbsp;
                                        <asp:Label ID="lblShowEntries" runat="server" Text="Show Entries"></asp:Label>
                                        <asp:DropDownList ID="cmbShowEntries" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                            Width="55px" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                            <asp:ListItem Value="0">5</asp:ListItem>
                                            <asp:ListItem Value="1">10</asp:ListItem>
                                            <asp:ListItem Value="2">15</asp:ListItem>
                                            <asp:ListItem Value="3">20</asp:ListItem>
                                            <asp:ListItem Value="4">25</asp:ListItem>
                                            <asp:ListItem Value="5">30</asp:ListItem>
                                            <asp:ListItem Value="6">40</asp:ListItem>
                                            <asp:ListItem Value="7">45</asp:ListItem>
                                            <asp:ListItem Value="8">50</asp:ListItem>
                                            <asp:ListItem Value="9">55</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td>
                                <span id="lblHeading" class="clsLabelHeader" style="display: none">Search Part No. to
                                    add in Requisition </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <%-- <td>
                                                    <span id="lblPartNo" class="clsLabel">Part No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="50"
                                                        AutoPostBack="true" ToolTip="Enter Part No."></asp:TextBox>
                                                </td>--%>
                                                <td align="right">
                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Part No"
                                                        Visible="false" Text="Find Now" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%-- <td>
                                                    <span id="lblDescription" class="clsLabel">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescriptionSearch" runat="server" CssClass="clsTextBox3_Ajax"
                                                        MaxLength="200" ToolTip="Enter Description." AutoPostBack="true"></asp:TextBox>
                                                </td>--%>
                                                <td>
                                                    <asp:CheckBox ID="chkConsiderAlternatePart" runat="server" Text="Consider Alternate Part(s) Stock also for Re-Order Qty. calculation"
                                                        Visible='<%# AppSettings("ClientCode")="BA" %>' AutoPostBack="true" CssClass="clsCheckBox" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts :  Record(s) found.</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                    DataValueField="ID" DataTextField="Name" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%-- <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOkTop" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to add the selected item" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnExportTop" TabIndex="0" runat="server" CssClass="clsButton" Text="Export to Excel"
                                                                            ToolTip="Click to Export report" Width="100px" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Back" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:GridView ID="dgPartList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                        CellPadding="25" ForeColor="Black" GridLines="Horizontal" ShowHeaderWhenEmpty="true"
                                                        PageSize="10" AllowPaging="True" AllowSorting="false" DataKeyNames="ID">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField DataField="ItemId" HeaderText="ItemId" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%-- <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                        <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>--%>
                                                                    <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="PartNo" SortExpression="PartNo" ReadOnly="True" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="AlternatePart" HeaderText="Alternate Parts">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="FirstPriorityPart" HeaderText="First Priority Part">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="Category" HeaderText="Category">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField HeaderText="One Time Purchase" SortExpression="OneTimePurchase" DataField="OneTimePurchase">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="MinStockLevel" SortExpression="MinStockLevel" HeaderText="Min. Level">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="MaxStockLevel" SortExpression="MaxStockLevel" HeaderText="Max. Level">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="AvailableQtyForItemGridOfOpenAuthorizedReceipt" SortExpression="AvailableQtyForItemGridOfOpenAuthorizedReceipt"
                                                                HeaderText="Available Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="MinReOrderLevel" SortExpression="MinReOrderLevel" HeaderText="Re-Order Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--12--%>
                                                            <asp:BoundField DataField="OnRequisitionQtyOfOpenAuthorizedRequisition" SortExpression="OnRequisitionQtyOfOpenAuthorizedRequisition"
                                                                HeaderText="On Requisition Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:BoundField DataField="OnOrderQtyOfOpenAuthorizedOrder" SortExpression="OnOrderQtyOfOpenAuthorizedOrder"
                                                                HeaderText="On Order Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--14--%>
                                                            <asp:BoundField DataField="Unit" SortExpression="Unit" HeaderText="Unit">
                                                                <HeaderStyle ForeColor="black"></HeaderStyle>
                                                                <ItemStyle></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--15--%>
                                                            <asp:BoundField HeaderText="Contracted With Supplier" SortExpression="ContractedVendorName"
                                                                DataField="ContractedVendorName">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--16--%>
                                                            <asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus"
                                                                Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <%--17--%>
                                                            <asp:BoundField HeaderText="PONosYetToReceive" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn" DataField="PONosYetToReceive">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--18--%>
                                                            <asp:BoundField HeaderText="MinStockLevel" DataField="MinStockLevel" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--19--%>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            <%--20--%>
                                                            <asp:BoundField DataField="AvailableQtyForItemGrid" SortExpression="AvailableQtyForItemGrid"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="Available Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--21--%>
                                                            <asp:BoundField DataField="OnRequisitionQty" SortExpression="OnRequisitionQty" HeaderText="On Requisition Qty."
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--22--%>
                                                            <asp:BoundField DataField="OnOrderQty" SortExpression="OnOrderQty" HeaderText="On Order Qty."
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
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
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto" BackColor="255,128,128"
                                    Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True,False) %>'
                                    ForeColor="255,128,128">Green</asp:Label>
                                <asp:Label ID="Span1" runat="server" CssClass="clsLabel" Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True,False) %>'>Available Qty. + On Order Qty. + On Requisition Qty. > Min Stock Level
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" BackColor="SkyBlue"
                                    Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True,False) %>'
                                    ForeColor="SkyBlue">Green</asp:Label>
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True,False) %>'>Available Qty. + On Order Qty.+ On Requisition Qty. <= Min Stock Level
                                </asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOk" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to add the selected item" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsButton" Text="Export to Excel"
                                                        ToolTip="Click to Export report" Width="100px" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Back" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
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
            parent.ParentCallBackFunctionForCommonPartList();
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
                parent.IFrameCommonPartListStateComplete();
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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgPartList tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        SetRow($(this));
                    }
                    else {
                        $(this).removeAttr("checked");
                        SetRow($(this));
                    }
                });
            });
        });
        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }
        function pageLoad() {
            var status;
            $("#dgPartList tr:gt(0)").find(":checkbox").each(function () {
                status = $(this).attr("checked");
                if (status == "checked") {
                    SetRow($(this));
                }
                else {
                    //$(this).removeAttr("checked");
                    SetRow($(this));
                }
            });
        }
    </script>
    </form>
</body>
</html>

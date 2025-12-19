<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartStatus.aspx.vb"
    Inherits="Flypal.wfPartStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part Status</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <span id="lblPartStockStatusList" class="clsFormHeader">Part Status</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnCloseBottom" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" CausesValidation="False" ToolTip="Click to Close"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPartInfo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartNumber" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                        </td>
                                                        <td colspan="7">
                                                            <asp:Label ID="lblPartDescription" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStockQty" class="clsLabelAuto">Stock Qty.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartStockQty" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <span id="lblServiceableStockQty" class="clsLabelAuto">Serviceable Stock</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartServiceableStockQty" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <span id="lblUnserviceableStockQty" class="clsLabelAuto">UnServiceable Stock</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartUnserviceableStockQty" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <span id="lblOnOrderQty" class="clsLabelAuto">On Order Qty.</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPartOnOrderQty" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPurchaseOrderInfo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px" id="fdsLast3Purchasesdetails"
                                                runat="server">
                                                <legend><span class="clsLabelHeader">Last 3 Purchases details</span></legend>
                                                <asp:GridView ID="dgPurchaseOrderInfo" runat="server" CssClass="clsGridNewStyle"
                                                    AutoGenerateColumns="false" AllowSorting="true" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OrderNumber" HeaderText="Order No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VendorName" HeaderText="Supplier">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Invoice Date">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Right" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Right" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LeadTime" HeaderText="Lead Time">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Right" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlRequisitionInfo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px" id="fdsLast3Requisitiondetails"
                                                runat="server">
                                                <legend><span class="clsLabelHeader">Last 3 Issues</span></legend>
                                                <asp:GridView ID="dgRequisitionInfo" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
                                                    AllowSorting="true" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IssueNumber" HeaderText="Issue No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptItemSerialNo" HeaderText="Serial No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Aircraft">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WONo" HeaderText="WO. NO.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WorkShop" HeaderText="Workshop">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequestedQty" HeaderText="Qty.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Right" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequisitionDate" HeaderText="Req. Date">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequisitionNumber" HeaderText="Requisition No.">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EmployeeName" HeaderText="Requested By">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReasonForRequest" HeaderText="Reason For Request">
                                                            <HeaderStyle ForeColor="Black" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlQuotationList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px" id="fdsLast3QuotationCompared"
                                                runat="server" visible="false">
                                                <legend><span class="clsLabelHeader">Quotation Comparison</span></legend>
                                                <asp:GridView ID="dgQuotationList" DataKeyNames="QuotationItemID" runat="server"
                                                    CssClass="clsGridNewStyle" AutoGenerateColumns="False" PageSize="3" Width="100%"
                                                    ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ItemID"></asp:BoundField>
                                                        <%--1--%>
                                                        <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="QuotationTextNo" HeaderText="Quotation No.">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="QuotationDateFormatted" HeaderText="Quotation Date">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="QuotationItemQtyForGrid" HeaderText="Qty.">
                                                            <HeaderStyle HorizontalAlign="right" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="QuotationItemCRate" HeaderText="Rate">
                                                            <HeaderStyle HorizontalAlign="right" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:BoundField DataField="CurrencySymbol" HeaderText="Curr. Symbol">
                                                            <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="false" HorizontalAlign="left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="QuotationItemEffRate" HeaderText="Base Rate">
                                                            <HeaderStyle HorizontalAlign="right" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--9--%>
                                                        <asp:BoundField DataField="ItemType" SortExpression="ItemType" HeaderText="Cond.">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--10--%>
                                                        <asp:BoundField DataField="RequisitionTextNo" HeaderText="Enquiry No.">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--11--%>
                                                        <asp:BoundField DataField="EnquiryDateFormatted" HeaderText="Enquiry Date">
                                                            <HeaderStyle Wrap="False" ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--12--%>
                                                        <asp:BoundField DataField="LeadTime" HeaderText="Lead Time">
                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Right" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlAlternateParts" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px" id="fdsAlternateParts" runat="server">
                                                <legend>
                                                    <asp:Label ID="lblAlternateParts" runat="server" CssClass="clsLabelHeader"> Alternate Parts :</asp:Label></legend>
                                                <asp:GridView ShowHeaderWhenEmpty="True" ID="gdvAlternate" runat="server" CssClass="clsGridNewStyle"
                                                    EnableViewState="False" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="AlternatePartID,PartName"
                                                    GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="AlternatePartID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                            ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField DataTextField="PartName" HeaderText="Part No." CommandName="PartStatus">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IPCReference" HeaderText="IPC Reference" HeaderStyle-CssClass="hideGridColumn"
                                                            ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </fieldset>
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
    </div>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPartStatus();
            return false;
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
        SetPageLayout();
            if ($.browser.msie) {
                parent.IFramePartStatusStateComplete();
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
            <% If Not mopenas Is Nothing AndAlso mopenas = "FromPurchaseOrder" Then %>  
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

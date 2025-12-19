<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfStockCardExpendable_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfStockCardExpendable_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Stock Card Expendable</title>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .coloryellow
        {
            background: Yellow;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
            var e = document.getElementById("<%=ItemNotInUseImage.ClientID%>");
            if (e != null) {
                e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
                setTimeout("blinknow();", 750);
            }
        }       
    </script>
    <table id="Table1" class="clstablelistout" border="0">
        <tr>
            <td align="left">
                <table id="Table2" class="clsTablelistin" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <span id="lblTitle" class="clstitle1">Stock Card Expendable</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <table id="Table4" border="0" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <span id="Label6" class="clsLabelAuto">Part No. :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextPartNo" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label4" class="clsLabelAuto">Description :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextDescription" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStorageLife" class="clsLabelAuto">Storage Life :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextStorageLife" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblMaxStockLeve" class="clsLabelAuto">Max Stock Level :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextMaxStockLevel" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblReOrderLevel" class="clsLabelAuto">Re-Order Quantity :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextReOrderLevel" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label7" class="clsLabelAuto" visible="true">Balance Qty. : </span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextBalQty" runat="server" CssClass="clsLabelHeader" Visible="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSupersedes" class="clsLabelAuto">Supersedes :</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="spanItemCategory" class="clsLabelAuto">Item Category :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblItemCategory" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblBinCardNumber1" class="clsLabelAuto">Bin Card No. :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBinCardNumber" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblOnetimepurchase" runat="server" CssClass="clsLabelHeader" Visible="false">One time purchase</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <table id="Table5" border="0" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <span id="lblUnit" class="clsLabelAuto">UM :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextUnit" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="lblEssentialcategory" class="clsLabelAuto">Essential Category :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextEssentialcategory" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblLocation" class="clsLabelAuto">Location :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextLocation" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblScrapLife" class="clsLabelAuto">Scrap Life :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextScrapLife" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblMinStockLevel" class="clsLabelAuto">Min Stock Level :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextMinStockLevel" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td>
                                                            <span id="lblOrderQty" class="clsLabelAuto">Order Qty. :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltextOrderQty" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td>
                                                            <span id="lblApplicability" class="clsLabelAuto" width="96px">Applicability :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblApplicabilityList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSupersededBy" class="clsLabelAuto">Superseded By :</span>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSerialNo" class="clsLabelAuto" style="width: 96px;">Serial No. :</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSerialNoList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblView" runat="server" CssClass="clsLabel">View Attachment</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" ID="upnlAttachment" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="30px"
                                                                        ImageUrl="~/icons/Attachment.png" Width="30px" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblItemNotInUse" runat="server" CssClass="clsLabel" Visible="false">Part is not applicable since :</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblItemNotInUseDate" runat="server" CssClass="clsLabelHeader" Visible="false"></asp:Label>
                                                        </td>
                                                        <td rowspan="3">
                                                            <asp:Image ID="ItemNotInUseImage" runat="server" ImageUrl="~/icons/technical_wrench_cross.ico"
                                                                Height="31px" Width="52px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblItemNotInUseNotes" runat="server" CssClass="clsLabel">Note :</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblItemNotInUseNote" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Button ID="btnShowPartStatus" runat="server" CausesValidation="False" Visible='<%# AppSettings("ClientCode")="BA"%>'
                                                                CssClass="clsButtonLong_Ajax" Text="Part Status" ToolTip="Click for Part Status" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAlternateParts" runat="server" CssClass="clsLabelHeader">Alternate Part No. :</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gdvAlternate" runat="server" ShowHeaderWhenEmpty="true" CssClass="clsGrid"
                                                                ToolTip="Alternate Part List" AutoGenerateColumns="False" EnableViewState="false"
                                                                AllowSorting="True" AllowPaging="True" PageSize="5">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="AlternatePartID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:ButtonField DataTextField="PartName" HeaderText="Part No." CommandName="Attach">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                                        <HeaderStyle HorizontalAlign="Left" />
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
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span id="lblServiceablePartList" class="clsLabelHeader">Expendable Stock List</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlStockCardGrid" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gdvStockCard" EnableViewState="False" ShowHeaderWhenEmpty="True"
                                        runat="server" CssClass="clsGrid" ToolTip="Stock Card Expendable" AutoGenerateColumns="False"
                                        AllowSorting="True">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <%--0--%>
                                            <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--1--%>
                                            <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--2--%>
                                            <asp:ButtonField DataTextField="ReceiptNo" HeaderText="GRN Reference" CommandName="GRNReference">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <%--3--%>
                                            <asp:BoundField DataField="PartStatus" SortExpression="PartStatus" HeaderText="Part Status">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--4--%>
                                            <asp:BoundField DataField="Source" SortExpression="Source" HeaderText="Source">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--5--%>
                                            <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--6--%>
                                            <asp:BoundField DataField="ReceiptQty" HeaderText="Receipt Qty.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--7--%>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--8--%>
                                            <asp:BoundField DataField="OrderNo" SortExpression="OrderNo" HeaderText="Order No.">
                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                            </asp:BoundField>
                                            <%--9--%>
                                            <asp:BoundField Visible="False" DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                            </asp:BoundField>
                                            <%--10--%>
                                            <asp:BoundField Visible="False" DataField="CommercialRate" HeaderText="Purchase Price">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--11--%>
                                            <asp:BoundField Visible="False" DataField="Rate" HeaderText="OH/RPR Cost">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--12--%>
                                            <asp:BoundField Visible="False" DataField="BatchNo" HeaderText="Batch No.">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--13--%>
                                            <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <%--14--%>
                                            <asp:ButtonField DataTextField="IssueNo" HeaderText="Issue Reference" CommandName="IssueReference">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <%--15--%>
                                            <asp:BoundField DataField="IssueQty" HeaderText="Issue Qty.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--16--%>
                                            <asp:BoundField DataField="BalQty" HeaderText="Bal. Qty.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--17--%>
                                            <asp:BoundField DataField="CostCenter" SortExpression="CostCenter" HeaderText="Cost Center">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <%--18--%>
                                            <asp:BoundField DataField="Remark" HeaderText="Remarks" HtmlEncode="false">
                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--19--%>
                                            <asp:ButtonField Text="Track" HeaderText="Track" CommandName="Show">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <%--20--%>
                                            <asp:BoundField DataField="tabIssueItemLoanQty" HeaderText="tabIssueItemLoanQty"
                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" />
                                                <ItemStyle CssClass="hideGridColumn" />
                                            </asp:BoundField>
                                            <%--21--%>
                                            <asp:BoundField DataField="IsReturnableFromAircraft" HeaderText="IsReturnableFromAircraft"
                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" />
                                                <ItemStyle CssClass="hideGridColumn" />
                                            </asp:BoundField>
                                            <%--22--%>
                                            <asp:BoundField DataField="IssueStatusID" HeaderText="IssueStatusID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            <%--23--%>
                                            <asp:BoundField DataField="EROQty" HeaderText="EROQty" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--24--%>
                                            <asp:BoundField DataField="ReceiptItemRemark" HeaderText="ReceiptItemRemark" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--25--%>
                                            <asp:BoundField DataField="IsConsiderAsAsset" HeaderText="IsConsiderAsAsset" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--26--%>
                                            <asp:BoundField DataField="ReceiptTransTypeID" HeaderText="ReceiptTransTypeID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--27--%>
                                            <asp:BoundField DataField="IsConvertToOutright" HeaderText="IsConvertToOutright"
                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--28--%>
                                            <asp:BoundField DataField="OrderItemReceiptBalanceQty" HeaderText="OrderItemReceiptBalanceQty">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--29--%>
                                            <asp:BoundField DataField="OrderTransTypeID" HeaderText="OrderTransTypeID">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--30--%>
                                            <asp:BoundField DataField="OrderItemEROQty" HeaderText="OrderItemEROQty">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--31--%>
                                            <asp:BoundField DataField="TransTypeID" HeaderText="TransTypeID">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--32--%>
                                            <asp:BoundField DataField="AircraftRemovedQty" HeaderText="AircraftRemovedQty">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 33--%>
                                            <asp:BoundField DataField="ItemNotInUse" HeaderText="ItemNotInUse">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 34--%>
                                            <asp:BoundField DataField="IssueTransTypeID" HeaderText="IssueTransTypeIDd">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 35--%>
                                            <asp:BoundField DataField="IsCapitalize" HeaderText="IsCapitalize">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 36--%>
                                            <asp:BoundField DataField="IsConsiderAsAsset" HeaderText="IsConsiderAsAsset">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span id="Span7" class="clsLabelHeader">PPS(part purchase),MRN,Order,Open GRN Info.</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlOpenReceipt" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgOpenReceipt" EnableViewState="False" ShowHeaderWhenEmpty="True"
                                        runat="server" CssClass="clsGrid" ToolTip="Stock Card Expendable" AutoGenerateColumns="False"
                                        AllowSorting="True">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <%--0--%>
                                            <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--1--%>
                                            <asp:BoundField DataField="ReqDate" HeaderText="Req. Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--2--%>
                                            <asp:BoundField DataField="ReqNo" HeaderText="Req. No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--3--%>
                                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--4--%>
                                            <asp:BoundField DataField="OrderNo" HeaderText="Order No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--5--%>
                                            <asp:BoundField DataField="OrderItemQty" HeaderText="Order Qty.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--6--%>
                                            <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--7--%>
                                            <asp:BoundField DataField="ReceiptNo" HeaderText="GRN Reference">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--8--%>
                                            <asp:BoundField DataField="Source" HeaderText="Source">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--9--%>
                                            <asp:BoundField DataField="ReceiptItemExpiryDate" HeaderText="Expiry Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--10--%>
                                            <asp:BoundField DataField="ReceiptItemQty" HeaderText="Receipt Qty.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%--11--%>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <%-- 12--%>
                                            <asp:BoundField DataField="OrderItemEROQty" HeaderText="OrderItemEROQty"  >
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 13--%>
                                            <asp:BoundField DataField="IsOverhaul" HeaderText="IsOverhaul"  >
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- 14--%>
                                            <asp:BoundField DataField="TransTypeID" HeaderText="TransTypeID"  >
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" CssClass="clsColorLabel" BackColor="#8080ff"
                                ForeColor="#8080ff"></asp:Label>
                            <span id="Span3" class="clsLabel">Pending To Receive From Aircraft As Core Unit Return
                                (Issue Date Column)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblIssueStatusIsOpen" runat="server" CssClass="clsColorLabel" BackColor="#005cb8"
                                ForeColor="#005cb8"> </asp:Label>
                            <span id="Span4" class="clsLabel">Issue Status Is Open(Issue Date Column)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblPurple" runat="server" CssClass="clsColorLabel" BackColor="#FF00FF"
                                ForeColor="#FF00FF"> </asp:Label>
                            <span id="Span5" class="clsLabel">GRO Receipt To Outright(Order No. Column)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblOrange" runat="server" CssClass="clsColorLabel" BackColor="#FFC082"
                                ForeColor="#FFC082"> </asp:Label>
                            <span id="Span1" class="clsLabel">Pending To Issue For Exchange (Core Unit Return) (Order
                                No. Column)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblMaroon" runat="server" CssClass="clsColorLabel" BackColor="#800000"
                                ForeColor="#800000"> </asp:Label>
                            <span id="Span6" class="clsLabel">Advance Core-return (Issue Date Column)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblDarkBlue" runat="server" CssClass="clsColorLabel" BackColor="#000099"
                                ForeColor="#000099"> </asp:Label>
                            <span id="Span2" class="clsLabel">Pending to Issue to aircraft against receipt from
                                aircraft</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblDarkRed" runat="server" CssClass="clsColorLabel" BackColor="Red"
                                ForeColor="Red"> </asp:Label>
                            <span id="Span8" class="clsLabel">Part Is Not In Use</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblNoIssueColor" runat="server" CssClass="clsColorLabel" BackColor="#3A8F8C"
                                ForeColor="Red"></asp:Label>
                            <span id="Span10" class="clsLabel" style="margin-top: 5px">Overhaul/Repair Order Pending
                                for Issue (Order No. Column) </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblCapitalizedIssueColor" runat="server" CssClass="clsColorLabel"
                                BackColor="#008000" ForeColor="#008000"></asp:Label>
                            <span id="lblCapitalizedIssueSpan" class="clsLabel" style="margin-top: 5px">Capitalized
                                Issue (Issue Date Column) </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblDiscardIssueColor" runat="server" CssClass="clsColorLabel" BackColor="#c0c0c0"
                                ForeColor="#c0c0c0"></asp:Label>
                            <span id="lblDiscardIssueSpan" class="clsLabel" style="margin-top: 5px">Discard Issue(Issue
                                Date Column) </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblReceiptmarkedasassetColor" runat="server" CssClass="clsColorLabel"
                                BackColor="#00ff80" ForeColor="#00ff80"></asp:Label>
                            <span id="lblReceiptmarkedasassetSpan" class="clsLabel" style="margin-top: 5px">Receipt
                                marked as asset(Receipt Date Column) </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblSalesIssueColor" runat="server" CssClass="clsColorLabel" BackColor="#ffff80"
                                ForeColor="#ffff80"></asp:Label>
                            <span id="lblSalesIssueSpan" class="clsLabel" style="margin-top: 5px">Sales Issue(Issue
                                Date Column) </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsButton" ToolTip="Click to print"
                                                    CausesValidation="False" Text="Print"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCreateRequisition" runat="server" CausesValidation="False" Visible='<%# AppSettings("ClientCode")="BA"%>'
                                                    CssClass="clsButtonLong_Ajax" Text="Create Requisition" ToolTip="Click to Create New Requisition" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False">
                                                </asp:Button>
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
        <tr style="height: 0px;">
            <td style="height: 0px;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                    <ContentTemplate>
                        <asp:Button ID="hdnBtnReceipt" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                            Style="display: none;"></asp:Button>
                        <asp:Button ID="hdnBtnReceipt1" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                            Style="display: none;"></asp:Button>
                        <asp:Button ID="hdnBtnIssue" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                            Style="display: none;"></asp:Button>
                        <asp:Button ID="hdnBtnShowPartNoStatus" ClientIDMode="Static" runat="server" Text="Add"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <!-- Popup For Issue -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyIssue" Text="Issue" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlIssue" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeIssue" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlPopupIssue" runat="server" TargetControlID="btnDummyIssue"
        PopupControlID="pnlIssue" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function OpenIssueWindow() {
            try {
                $("#IframeIssue").attr("src", "wfIssue_Ajax.aspx?Type=FromwfStockCard");

                //                if ($.browser.msie) {
                $("#btnDummyIssue").click();
                //                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForIssue() {
            var Issuewindow = $find("<%=mdlPopupIssue.ClientID %>");
            //close popup window
            Issuewindow.hide();
            //           release resources
            $("#IframeIssue").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnIssue").click();
        }
    </script>
    <!---End-->
    <!-- Popup For RCI -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt" Text="Receipt" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlPopupReceipt" runat="server" TargetControlID="btnDummyReceipt"
        PopupControlID="pnlReceipt" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function OpenReceiptWindow() {
            try {
                $("#IframeReceipt").attr("src", "wfReceiptCumInvoice_Ajax.aspx?Type=FromwfStockCard");

                //                if ($.browser.msie) {
                $("#btnDummyReceipt").click();
                //                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForReceipt() {
            var Receiptwindow = $find("<%=mdlPopupReceipt.ClientID %>");
            //close popup window
            Receiptwindow.hide();
            //           release resources
            $("#IframeReceipt").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnReceipt").click();
        }
    </script>
    <!---End-->
    <!-- Popup For Receipt -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
        PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function OpenReceipt1Window() {
            try {
                $("#IframeReceipt1").attr("src", "wfReceipt_Ajax.aspx?Type=FromwfStockCard");

                //                if ($.browser.msie) {
                $("#btnDummyReceipt1").click();
                //                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForReceipt1() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnReceipt1").click();
        }
    </script>
    <!---End-->
    <!-- Popup For ShowPartNoStatus -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyShowPartNoStatus" Text="ShowPartNoStatus"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlShowPartNoStatus" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeShowPartNoStatus" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlPopupShowPartNoStatus" runat="server" TargetControlID="btnDummyShowPartNoStatus"
        PopupControlID="pnlShowPartNoStatus" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function OpenShowPartNoStatusWindow() {
            try {
                $("#IframeShowPartNoStatus").attr("src", "wfrptShowPartNoStatus_Ajax.aspx?Type=FromwfStockCard");
                $("#btnDummyShowPartNoStatus").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForShowPartNoStatus() {
            var ShowPartNoStatuswindow = $find("<%=mdlPopupShowPartNoStatus.ClientID %>");
            //close popup window
            ShowPartNoStatuswindow.hide();
            //           release resources
            $("#IframeShowPartNoStatus").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnShowPartNoStatus").click();
        }
    </script>
    <!---End-->
    <!--Attachment Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
        PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAttachStateComplete() {
            $("#btnDummyAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForAttach() {
            var Attachwindow = $find("<%=mdlAttach.ClientID %>");
            //close popup window
            Attachwindow.hide();
            //release resources
            $("#IframeAttach").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnAttach").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>

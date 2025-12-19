<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssuetoSupplierasReturn_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfIssuetoSupplierasReturn_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Part Stock Status List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script language="javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
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
        <div>
            <table class="clstablelistout" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblLedgerList" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1">
                                        <table>
                                            <tr>
                                                <td style="width: 99%" valign="middle">
                                                    <span id="lblPartStockStatusList" class="clsFormHeader">Part Stock To Return To Supplier</span>
                                                </td>

                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlFindNowButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part Number or Description to search"
                                                                            CssClass="clsTextBoxTagSearch" Width="200px" MaxLength="50">
                                                                        </asp:TextBox>
                                                                        <input type="text" id="myInput" placeholder="Search here.." title="Type in a name"
                                                                            onkeyup="Search_Gridview(this,dgPendingItemList)" class="clsTextBoxTagSearch" style="display: none" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel">Category</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="200px" DataValueField="ID"
                                                                            DataTextField="Name">
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                    <td>
                                                                        <asp:Label ID="lblSupplier" runat="server" CssClass="clsLabelAuto">Supplier</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSupplier" runat="server"
                                                                            CssClass="clsTextBoxTagSearch" Width="200px" MaxLength="100">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to Find the Part" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>


                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlPendingItemList" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">Item Stock List Record(s) Found</asp:Label>
                                                <asp:GridView ID="dgPendingItemList" runat="server" CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="true" AllowSorting="False" AllowPaging="False">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <%--0--%>
                                                        <asp:BoundField DataField="ShowStatusForRemovedAsReturnableFromAircraft">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" Font-Bold="true" />
                                                        </asp:BoundField>
                                                        <%--1--%>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <%# "Part No." + "</br>" + "Description" %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("ItemName") + "</br>" + Eval("ItemDesc") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--2--%>
                                                        <asp:BoundField DataField="Vendor" HeaderText="Supplier" SortExpression="Vendor">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--3--%>
                                                        <asp:BoundField DataField="AvailableQuantity" HeaderText="Available Qty." SortExpression="AvailableQuantity">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%--4--%>
                                                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--5--%>
                                                        <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" SortExpression="ReceiptTextIntReceiptNo"
                                                            HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--6--%>
                                                        <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                            HtmlEncode="false" Visible="False">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--7--%>
                                                        <asp:BoundField DataField="ReleaseNoteNoInfo" HeaderText="R.N. Info" SortExpression="ReleaseNoteDateFormatted" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--8--%>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--9--%>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <%# "Store" + "</br>" + "Location" %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("StoreName") + "</br>" + Eval("ReceiptItemBinLocation") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--10--%>

                                                        <asp:BoundField DataField="LandingRate" HeaderText="Landing Rate(Inv. Currency)">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                                        </asp:BoundField>
                                                        <%--11--%>
                                                        <asp:BoundField DataField="EffRate" HeaderText="Landing Rate">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%--12--%>
                                                        <asp:BoundField DataField="ExpiryInfo" HeaderText="Expiry  Info" SortExpression="ExpiryQtrs" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--13--%>
                                                        <asp:ButtonField CommandName="SelectRecord" HeaderText="Return" Text="Return">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>

                                                        <%--14--%>
                                                        <asp:BoundField DataField="ReceiptItemIsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                            HeaderText="ReceiptItemIsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--15--%>
                                                        <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                            ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--16--%>
                                                        <asp:BoundField DataField="CountOfComponentReservationItem" HeaderText="CountOfComponentReservationItem" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--17--%>
                                                        <asp:BoundField DataField="EnabledDisabled" HeaderText="EnabledDisabled" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <%--18--%>
                                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>



                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
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
        <div id="DueAtMessage" class="clsInfoMessage" style="display: none" runat="server">
            <p>
                <u>Note:</u>
                <br />
                Enter Part No./Description and click on Find Now button to get Part Stock list.
            </p>
        </div>
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var isAsync = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (!isAsync) {
                if ("<%= page.IsPostback%>" == "False" && "<%=  Not mIssue Is Nothing  %>" == "True") {
                    $pos = $("#<%=txtSearch.ClientID%>").position();
                    var top = $pos.top;
                    var left = $pos.left;
                    var searchHeight = $("#<%=txtSearch.ClientID%>").height();
                    var margin = top + searchHeight;

                    var height = $("#tblMain").outerHeight();
                    var h = margin - height;
                    $("#DueAtMessage").css("display", "block");
                    $("#DueAtMessage").animate({ marginTop: h, marginLeft: left - 5 }, 300, 'swing', function () {
                        $("#DueAtMessage").delay(5000).fadeOut();
                    });
                }
            }
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".clsGridNewStyle").find("td").each(function () {

                if ($(this).text() == "*") {
                    $(this).css("color", "Red");
                    $("td", $(this).closest("tr")).addClass("activerow");
                }
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $(".clsGridNewStyle").find("td").each(function () {

                if ($(this).text() == "*") {
                    $(this).css("color", "Red");
                    $("td", $(this).closest("tr")).addClass("activerow");
                }
            });
        });
    </script>
    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("dgPendingItemList");
            var rowData;
            var regex = /(&nbsp;|<([^>]+)>)/ig
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML.replace(regex, '');
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('.search_textbox').each(function (i) {
                $(this).quicksearch("[id*=dgPendingItemList] tr:not(:has(th))", {
                    'testQuery': function (query, txt, row) {
                        return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
    </script>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComponentReservationStockList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfComponentReservationStockList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Stock Status List</title>
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
                                <td>
                                    <span id="lblPartStockStatusList" class="clstitle1">Component Stock Status List</span>
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
                                                                    <span id="lblReservation" class="clsLabel">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtReservationDate" runat="server" AutoPostBack="true" ClientIDMode="Static"
                                                                        CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                        Text="" Width="100px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtReservationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReservationDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtReservationDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtReservationDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No./Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part Number or Description to search"
                                                                        CssClass="clsTextBox1_Ajax" MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto" >Serial No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" ToolTip="Serial No." CssClass="clsTextBox1_Ajax"
                                                                        MaxLength="50"  >
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel" Visible="false">Category</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                                        Visible="false" DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkShowBERPart" runat="server" CssClass="clsLabelAuto" Text="Show BER Part"
                                                                        Visible="false"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find the Part"
                                                            Text="Find Now"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlComponentReservationStockList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">Item Stock List Record(s) Found</asp:Label>
                                            <asp:GridView ID="dgComponentReservationStockList" runat="server" CssClass="clsGrid"
                                                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True"
                                                PageSize="100">
                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <%--0--%>
                                                    <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--1--%>
                                                    <asp:BoundField DataField="ItemDesc" HeaderText="Description" SortExpression="ItemDescription">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" CssClass="TextBreak" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" CssClass="TextBreak" />
                                                    </asp:BoundField>
                                                    <%--2--%>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--3--%>
                                                    <asp:BoundField DataField="AvailableQuantity" HeaderText="Stock Qty." SortExpression="AvailableQuantity">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <%--4--%>
                                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--5--%>
                                                    <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--6--%>
                                                    <asp:BoundField DataField="ReceiptTextIntReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptTextIntReceiptNo"
                                                        HtmlEncode="false">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--7--%>
                                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." SortExpression="ReceiptNo"
                                                        HtmlEncode="false" Visible="False">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundField>
                                                    <%--8--%>
                                                    <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--9--%>
                                                    <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location" SortExpression="ReceiptItemBinLocation">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                     
                                                    
                                                    <%--10--%>
                                                    <asp:BoundField DataField="ExpiryInformation" HeaderText="Expiry Info.">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--11--%>
                                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No." SortExpression="BatchNo">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    
                                                    <%--12--%>
                                                    <asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                    <%--13--%>
                                                    <asp:BoundField DataField="ReceiptItemIsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        HeaderText="ReceiptItemIsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--14--%>
                                                    <asp:BoundField DataField="Color" HeaderStyle-CssClass="hideGridColumn" HeaderText="Color"
                                                        ItemStyle-CssClass="hideGridColumn">
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
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                Text="Back"></asp:Button>
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
    <div id="DueAtMessage" class="clsInfoMessage" style="display: none" runat="server">
        <p>
            <u>Note:</u>
            <br />
            Enter Part No./Description and click on Find Now button to get Part Stock list.</p>
    </div>
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }

       
    </script>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfApprovedQuotationItems_Ajax.aspx.vb"
    Inherits="Flypal.wfApprovedQuotationItems_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Approved Quotation Items</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <%-- AJAX ScriptManager --%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5">
                                <span id="lbltitle" class="clstitle1">Approved Quotation Items</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="Label2" class="clsLabelHeader">Select Order Date and select the Part you want
                                    to Add in Order.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDate" class="clsLabelAuto">Order Date</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="calOrderDate" CssClass="clsTextBox_Ajax" Width="100px"
                                    onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calOrderDate">
                                </cc2:CalendarExtender>
                            </td>
                            <td>
                                <span id="lblPartNo" class="clsLabelAuto">Part No</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part No" CssClass="clsTextBox_Ajax"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span id="Label1" class="clsLabelAuto">Select Part from the list and check to select
                                    the Part Information or click on Back button to go back to previous page.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlPartList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        <asp:GridView ID="dgPartList" runat="server" CssClass="clsGrid" AllowPaging="True"
                                            AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="Item ID"></asp:BoundField>
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TotalPendingQTY" HeaderText="Total Req. Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlQuotationItems" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        <asp:GridView ID="dgQuotationItems" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="QuotationDateFormatted" HeaderText="Date">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="QuotationNo" HeaderText="Quotation No.">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier"></asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Quotation Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="PurchaseBalQty" HeaderText="Pending Order Qty.">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Currency" HeaderText="Currency">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnOk" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click To  Add the Item In Order"
                                            Enabled="False"></asp:Button>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click To Go Back To Order Detail"
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

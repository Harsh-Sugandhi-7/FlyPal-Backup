<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingLoanToReturn_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingLoanToReturn_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Pending Loan To Return</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Loan Pending To Return To Store</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Issue Date Required"
                                    ControlToValidate="txtDate" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblLabel" class="clsLabelAuto">Enter date to create Issue and click Find Now
                                    button to get Receipts list accordingly.</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <span id="lblDate" class="clsLabelAuto">Issue Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                        onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        ToolTip="Enter Part No"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                  <%--  <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Search the Record"
                                                        ValidationGroup="a" Text="Find Now"></asp:Button>
--%>
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                ToolTip="Click to Search the Record" ValidationGroup="a">
                                                            </asp:ImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:GridView ID="dgPendingList" runat="server" CellPadding="5" GridLines="Horizontal" CssClass="clsGridNewStyle" AllowSorting="True"
                                                        ShowHeaderWhenEmpty="true" EnableViewState="false" AllowPaging="true" PageSize="10"
                                                        DataKeyNames="ReceiptItemID" AutoGenerateColumns="False">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ReceiptItemID" HeaderText="ReceiptItemID">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part Number">
                                                                <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Part Desc.">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Nomenclature" SortExpression="Nomenclature" HeaderText="Nomenclature">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Category" SortExpression="Category" HeaderText="Category">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptTextIntReceiptNo" SortExpression="ReceiptTextIntReceiptNo"
                                                                HeaderText="Receipt Text" HtmlEncode="false" >
                                                                <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                                <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanTakenQty" SortExpression="LoanTakenQty" HeaderText="Loan Taken Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" SortExpression="LoanQty" HeaderText="Loan To Return Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromStoreName" SortExpression="FromStoreName" HeaderText="From">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ToStoreName" SortExpression="ToStoreName" HeaderText="To Store">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R.N. No.">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpQtrYear" SortExpression="ExpQtrYear" HeaderText="Expiry Qtrs.">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQtyTaken" SortExpression="StockBalanceQty" HeaderText="Loan Taken Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" SortExpression="LoanQty" HeaderText="Balance Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
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
                                <asp:UpdatePanel ID="upnlACtionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                            Text="Back" CausesValidation="False"></asp:Button>


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
    <%--Date Validations--%>
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
    </form>
</body>
</html>

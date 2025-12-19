<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingToReturnForExchangeRepair_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingToReturnForExchangeRepair_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Pending To Return For Exchange/Repair/Overhaul</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblTitle" class="clsFormHeader">Pending To Return For Exchange/Repair/Overhaul</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"
                                                ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
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
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdbOrders" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                        GroupName="grIssue" Checked="True" Text="Show Purchase Order(s) Pending for Issue">
                                                    </asp:RadioButton>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdbRaceipts" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                        GroupName="grIssue" Text="Show Purchase Receipt(s) Pending for Issue" Visible="False">
                                                    </asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblLabel" class="clsLabelAuto" style="padding-left: 3px;">Enter date to create
                                                        Issue and click Find Now button to get Orders list accordingly.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <span id="lblIssueDate" class="clsLabelAuto">Issue Date</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td align="left">
                                                                <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    ToolTip="Enter Part No."></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    ToolTip="Enter Serial No."></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <%--<asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" Text="Find Now"
                                                        ValidationGroup="a" ToolTip="Click to Search the Record"></asp:Button>--%>
                                                          <asp:ImageButton ID="btnFindNow" runat="server" ValidationGroup="a" ImageUrl="~/images/Search2.png"
                                                                CssClass="clsSearch2btn" ToolTip="Click to Search the Record"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgPendingList" runat="server" CssClass="clsGridNewStyle" CellPadding="5"
                                                        GridLines="Horizontal" PageSize="25" EnableViewState="false" ShowHeaderWhenEmpty="true"
                                                        AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptText" SortExpression="ReceiptText" HeaderText="Number">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part Number">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Part Desc.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="LoanTakenQty" SortExpression="LoanTakenQty"
                                                                HeaderText="Loan Taken Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" SortExpression="LoanQty" HeaderText="ERO Qty.">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Order Type">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R.N. No.">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpQtrYear" SortExpression="ExpQtrYear" HeaderText="Expiry Qtrs">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectPart">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
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
                                        <table>
                                            <tr>
                                                <%--<td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="False"></asp:Button>
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
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

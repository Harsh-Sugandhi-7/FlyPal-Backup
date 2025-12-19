<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPPendingOrderItemList_Ajax.aspx.vb"
    Inherits="Flypal.wfCWPPendingOrderItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending Order Item List For CWP</title>
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
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lblTitle" class="clstitle1">Pending Order Item List For CWP</span>
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
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <span id="lblLabel" class="clsLabelAuto" style="padding-left: 3px;">Enter Date, Part
                                                No For Search .</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <span id="lblIssueDate" class="clsLabelAuto">Date</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender');"
                                                                    ClientIDMode="Static"></asp:TextBox>
                                                                <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                </cc2:CalendarExtender>
                                                            </td>
                                                            <td align="left">
                                                                <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                    AutoPostBack="true" ToolTip="Enter Part No"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                    ToolTip="Click to Search the Record" ValidationGroup="a" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlTopButton" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnTopClose" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                    ToolTip="Click to close" CausesValidation="False"></asp:Button>
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
                                            <asp:UpdatePanel ID="upnlPendingCWPOrderItemList" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgPendingCWPOrderItemList" runat="server" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="clsGrid" EnableViewState="false" PageSize="25"
                                                        ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="OrderItemID" HeaderText="OrderItemID" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrderDateFormatted" HeaderText="Date">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrderNo" HeaderText="Order No." SortExpression="OrderNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrderType" HeaderText="Type" SortExpression="OrderType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RegNo" HeaderText="Reg.
                            No." SortExpression="RegNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptItemSerialNo" HeaderText="Serial No." SortExpression="ReceiptItemSerialNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField CommandName="SelectPart" HeaderText="Select" Text="Select">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlBottomButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBottomClose" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close" CausesValidation="False"></asp:Button>
                                                </td>
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

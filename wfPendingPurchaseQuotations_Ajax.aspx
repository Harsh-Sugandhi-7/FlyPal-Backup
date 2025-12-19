<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingPurchaseQuotations_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingPurchaseQuotations_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Quotation List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="wfgroup" method="post" runat="server">
        <%-- AJAX ScriptManager --%>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Pending Quotation List</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnDone" runat="server" CssClass="clsbtnH clsinfoH" Text="Done" Enabled="False"
                                                                        ToolTip="Click to add selected Item(s)"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click To Go Back To Previous Page"></asp:Button>
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
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblDate" class="clsLabelAuto">Order Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                            onchange="ValidateDateText(this,'Date_watermarkextender');" AutoPostBack="true"></asp:TextBox>
                                                        <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbFromLastQuotation" runat="server" CssClass="clsRadioButton"
                                                            Text="From Last Quotation" GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbFromAllPendingQuotation" runat="server" CssClass="clsRadioButton"
                                                            Text="From All Pending Quotation(s)" GroupName="a" AutoPostBack="True" Checked="True"></asp:RadioButton>
                                                    </td>
                                                    <td align="right">
                                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now">
                                                    </asp:Button>--%>
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to Search the Record" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTransList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            <asp:GridView ID="dgTransList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
                                                AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
                                                CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QuotationTextNo" SortExpression="QuotationTextNo" HeaderText="Quotation No.">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConversionFactor" SortExpression="ConversionFactor" HeaderText="Conversion Factor">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                        <ItemStyle ForeColor="Blue" Wrap="False" />
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTransItemList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            <asp:GridView ID="dgTransItemList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"
                                                                onclick="CheckUncheck(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QuotationQty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CRate" HeaderText="Rate">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="COtherCharges" HeaderText="Other Charges">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CAmount" HeaderText="Amount">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <script type="text/javascript">
        function CheckUncheck(chkBoxAll) {
            var str = chkBoxAll.id;
            var status = $("#" + str).attr("checked");
            $("#dgTransItemList" + " tr:gt(0)").find(":checkbox[id*=" + str.substring(0, 'chkSelect') + "]").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                }
                else {
                    $(this).removeAttr("checked");
                }
            });
        }
    </script>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoicePendingIssueList_Ajax.aspx.vb"
    Inherits="Flypal.wfExportInvoicePendingIssueList_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Issue List For Export Invoice</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
</head>
<body>
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
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblTitle" class="clsFormHeader">Pending Issue List For Export Invoice</span>
                                        </td>
                                        <td colspan="5" align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblAsOnDateDate" class="clsLabelAuto">As On Date</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearchDate"
                                    onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender runat="server" ID="txtDateCalendarExtender" CssClass="cal_Theme1"
                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                </cc2:CalendarExtender>
                            </td>
                            <td>
                                <span id="lblPartNo" class="clsLabelAuto">Part No</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part No" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                            </td>
                            <td align="right">
                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now">  </asp:Button>--%>

                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" 
                                CssClass="clsSearch2btn" ToolTip="Click to find list as per searching criteria" />
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblIssue" class="clsLabelAuto">Issue</span>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="cmbIssueText" runat="server" CssClass="clsTextBoxTagSearch" DataValueField="Text"
                                 Height="24px" DataTextField="Text">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                    ToolTip="Enter Issue No."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblIssueTo" class="clsLabelAuto">Issue To</span>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="cmbToType" runat="server" CssClass="clsTextBoxTagSearch" DataValueField="Text"
                                    DataTextField="Text" Height="24px" Visible="True">
                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                    <asp:ListItem Value="1">Vendor(Consignee)</asp:ListItem>
                                    <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                    <asp:ListItem Value="8">Store</asp:ListItem>
                                    <asp:ListItem Value="16">WorkShop</asp:ListItem>
                                    <asp:ListItem Value="17">WorkOrder</asp:ListItem>
                                    <asp:ListItem Value="18">Requisition</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblOrder" class="clsLabelAuto">Order</span>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearch" DataValueField="Text"
                                  Height="24px" DataTextField="Text">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"></asp:TextBox>
                                <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearchSmall"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="upnlPendingIssueList" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        <asp:GridView ID="dgPendingIssueList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True"
                                            PageSize="50" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="Item ID"></asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="IssueID" HeaderText="IssueID"></asp:BoundField>
                                                <asp:BoundField DataField="IssueDateFormatted" HeaderText="Date">
                                                    <HeaderStyle ></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssueNumber" SortExpression="IssueNumber" HeaderText="Issue No.">
                                                    <HeaderStyle Wrap="False" ></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Issue Type">
                                                    <HeaderStyle Wrap="False" ></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FromStoreWithLocation" SortExpression="FromStoreWithLocation" HeaderText="Store">
                                                    <HeaderStyle Wrap="False" ></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date"></asp:BoundField>
                                                <asp:BoundField DataField="OrderNumber" HeaderText="Order No." SortExpression="OrderNumber">
                                                    <HeaderStyle  />
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
                        <%--<tr>
                            <td colspan="5" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                            Text="Back" CausesValidation="False"></asp:Button>
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

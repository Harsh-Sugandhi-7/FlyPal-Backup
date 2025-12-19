<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEnquiriesForQuotation_Ajax.aspx.vb"
    Inherits="Flypal.wfEnquiriesForQuotation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>List of Pending Enquiries</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <style type="text/css">
        .clsScroll {
            display: none !important;
        }
    </style>
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
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">List of Pending Enquiries</span>
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
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransactionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                            OnTextChanged="txtTransactionDate_TextChanged" AutoPostBack="true" onchange="ValidateDateText(this,'TransactionDate_watermarkextender');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtTransactionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtTransactionDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TransactionDate_watermarkextender" runat="server"
                                                            TargetControlID="txtTransactionDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            AutoPostBack="True">
                                                            <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Enquiry</asp:ListItem>
                                                            <asp:ListItem Value="2">Part No.</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbEnquiryText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            AutoPostBack="True" Visible="False" DataTextField="Text" DataValueField="Text">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                            MaxLength="100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                            MaxLength="4"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now"></asp:Button>--%>

                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                            ToolTip="Click to find list of record as per searching criteria" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlEnquiryList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            <asp:GridView ID="dgEnquiryList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True"
                                                DataKeyNames="VendorID" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="EnquiryID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="EnquiryDateFormatted" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EnquiryTextNo" SortExpression="EnquiryTextNo" HeaderText="Number">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Vendor">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRecord"></asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlEnquiryItems" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                            <asp:GridView ID="dgEnquiryItems" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"
                                                                onclick="CheckUncheck(this);" />

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>' />
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
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<tr>
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
                            </tr>--%>
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
            $("#dgEnquiryItems" + " tr:gt(0)").find(":checkbox[id*=" + str.substring(0, 'chkSelect') + "]").each(function () {
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

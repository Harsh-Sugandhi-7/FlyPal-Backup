<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingSalesOrderList_Ajax.aspx.vb" EnableEventValidation="false" 
    Inherits="Flypal.wfPendingSalesOrderList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Pending Sales Order List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                            <table width="100%">
                            <tr> 
                            <td>
                             <span id="lblQuotationList" class="clsFormHeader">List Of Pending Sales Orders</span>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to previous page"
                                            Text="Back"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            </tr>
                            </table>
                               
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function showTextField() {
                                        var SearchIndex = $get("cmbSearch").selectedIndex;

                                        var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                        var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                        var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                        var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                        if (SearchIndex != 1) {
                                            txtFromDateobj.style.display = 'none';
                                            txtToDateobj.style.display = 'none';
                                            lblFromDateobj.style.display = 'none';
                                            lblToDateobj.style.display = 'none';
                                        }
                                        else {
                                            var DateIndex = $get("cmbDate").selectedIndex;
                                            if (DateIndex == 0) {
                                                txtFromDateobj.style.display = 'none';
                                                txtToDateobj.style.display = 'none';
                                                lblFromDateobj.style.display = 'none';
                                                lblToDateobj.style.display = 'none';
                                            }
                                        }

                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblSearch" class="clsLabel" style="height: 10px; width: 48px;">Search</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="True">
                                                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Date</asp:ListItem>
                                                        <asp:ListItem Value="2">Sales Order</asp:ListItem>
                                                        <asp:ListItem Value="3">Part No.</asp:ListItem>
                                                        <asp:ListItem Value="4">Quotation</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <span id="L1" class="clsLabel" style="width: 20px;"></span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                        Visible="False">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbQuotationText" runat="server" CssClass="clsTextBoxTagSearch"
                                                        AutoPostBack="True" Visible="False" DataValueField="Text" DataTextField="Text">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbText" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="True"
                                                        Visible="False" DataValueField="Text" DataTextField="Text">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtItemName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"></asp:TextBox>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label>
                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                        MaxLength="4"></asp:TextBox>
                                                    <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabel">To Date </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblInfo" class="clsLabelAuto">Select Sales Order from the list.</span>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlFind" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                       <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Find" Text="Find Now" />--%>
                                         <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to Find"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlSalesOrderListGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Sales Order as per criteria : Record(s) found</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:GridView ID="dgSalesOrderList" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal" EnableViewState="false"
                                                        ShowHeaderWhenEmpty="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                                                        AllowPaging="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                         <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SalesOrderTextNo" SortExpression="SalesOrderTextNo" HeaderText="Number">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Customer">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total"
                                                                DataFormatString="{0:00.00}">
                                                                <HeaderStyle HorizontalAlign="Right"  ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="Status" SortExpression="Status" HeaderText="Status">
                                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
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
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlSalesOrderItemListGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                        Font-Bold="True">List of Sales Order Items as per criteria : Record(s) found</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:GridView ID="dgSalesOrderItems" runat="server" CssClass="clsGridNewStyle"  CellPadding="5" GridLines="Horizontal" ShowHeaderWhenEmpty="true"
                                                        EnableViewState="false" PageSize="5" AllowSorting="True" AutoGenerateColumns="False"
                                                        AllowPaging="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                         <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="SalesOrderItemID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False"   HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                                <HeaderStyle   HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssueBalanceQty" SortExpression="IssueBalanceQty" HeaderText="Issue Balance Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" Width="100px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
                            
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            showTextField();
        });    
    </script>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfExportInvoiceList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfExportInvoiceList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export Invoice List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td width="100%" class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblExportInvoiceList" runat="server" CssClass="clsFormHeader">List Export Invoice</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>

                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                            ToolTip="Click to Add New Export Invoice" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to close List of Export Invoice screen" CausesValidation="False"></asp:Button>
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
                                        <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <table>

                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span8" class="clsLabel">Range</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True">
                                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                        <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                        <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="lblFD" class="clsLabel" runat="server">From Date</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="lblTD" class="clsLabel" runat="server">To Date</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span3" class="clsLabel">Invoice No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbInvoiceText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                        AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="6"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right" valign="top">
                                                            <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
                                                                        CssClass="clsSearch2btn" ToolTip="Click to find list of Export Invoice as  per searching criteria" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                        <div>
                                                                            <div style="float: left; vertical-align: middle; width: 100%">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                                                                class="clsLabelHeader">Advance Search</span>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                                <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" colspan="2">
                                                            <asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnFindNow" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="spIssueNo" class="clsLabel">Issue No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbIssueText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                        AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtIssueNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                        Width="55px"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span7" class="clsLabel">Status</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Opened</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                                                        <asp:ListItem Value="4">Cancelled</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span5" class="clsLabel">Consignee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                        MaxLength="100"></asp:TextBox>
                                                                                </td>
                                                                                <td></td>
                                                                                <td></td>
                                                                                <td></td>
                                                                            </tr>



                                                                        </table>
                                                                    </asp:Panel>
                                                                    <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
                                                                        ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
                                                                        CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
                                                                        CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                                        ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                        SuppressPostBack="false" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%">
                                        <span id="lblInfo" class="clsLabelAuto" style="display: none">Select Export Invoice
                                        from the list. Click On Edit Link To Modify The Selected Export Invoice.Click on
                                        Delete link to delete the selected Export Invoice. Click on Add New button to add
                                        a New Export Invoice.</span>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                            ToolTip="Click to Add New Export Invoice" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                            ToolTip="Click to close List of Export Invoice screen" CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Receipt-Cum-Invoice as per criteria: Record(s) found.</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:GridView ID="dgExportInvoiceList" runat="server" AllowPaging="True" AllowSorting="True"
                                                                AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25"
                                                                ShowHeaderWhenEmpty="True">
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                        <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExportInvoiceDateFormatted" HeaderText="Date">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExportInvoiceNumber" SortExpression="ExportInvoiceNumber"
                                                                        HeaderText="Export Invoice No.">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Consignee" SortExpression="Consignee" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn" HeaderText="Consignee">
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass="hideGridColumn"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--<asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>

                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <span id="button">Login</span>--%>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                    CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="PnlPaging" runat="server">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <div style="width: 100%;">
                                                                    <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                    class="letterbox" />
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First"></span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous"></span>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div align="center">
                                                                                    <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                    </asp:TextBox>
                                                                                    <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                                        Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                        Length="300" />
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                            </td>
                                                                            <td>
                                                                                <span>of </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Export Invoice"
                                                                Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Export Invoice screen"
                                                                Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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
        </div>
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
    </form>
    <!-- Slider control events  -->
    <script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }


    </script>
    <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }


    </script>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>
    <!-- End  -->
</body>
</html>

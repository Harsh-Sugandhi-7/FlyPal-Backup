<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWorkInvoiceList_Ajax.aspx.vb"
    Inherits="Flypal.wfWorkInvoiceList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List Of Work Invoice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
                                <td width="100%">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblListOfWorkInvoice" runat="server" CssClass="clstitle1">List Of Work Invoice
                                            </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabel">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSearchCriteria" runat="server" CssClass="clsComboBox_Ajax"
                                                            AutoPostBack="True">
                                                            <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Date</asp:ListItem>
                                                            <asp:ListItem Value="2">Work Invoice</asp:ListItem>
                                                            <asp:ListItem Value="3">Vendor</asp:ListItem>
                                                            <asp:ListItem Value="4">Status</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                            Visible="False">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsComboBox_Ajax" Visible="False">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Opened</asp:ListItem>
                                                            <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                            <asp:ListItem Value="4">Canceled</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbWorkInvoiceText" runat="server" CssClass="clsComboBox_Ajax"
                                                            AutoPostBack="True" Visible="False" DataValueField="Text" DataTextField="Text">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                            MaxLength="100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Visible="False">No.</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                            MaxLength="8"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server" Visible="False">From Date </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Visible="False">To Date </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
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
                                <td width="100%">
                                    <span id="lblInfo" class="clsLabelAuto">Select Work Invoice from the list. Click on
                                        Edit link to Modify or Delete link to Delete the selected Work Invoice. Click on
                                        Add New button to Add a New Work Invoice.</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Work Invoice as per criteria : Record(s) found</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                            ToolTip="Click to find list of Work Invoice as per searching criteria." />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" Text="Add New"
                                                            ToolTip="Click to Add New Work Invoice" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" Text="Print"
                                                            ToolTip="Click to Print Work Invoice List" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                            ToolTip="Click to close List of Work Invoice screen." CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:GridView ID="dgWorkInvoiceList" runat="server" AllowPaging="True" AllowSorting="True"
                                                            AutoGenerateColumns="False" CssClass="clsGrid" EnableViewState="False" PageSize="25"
                                                            ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WorkInvoiceNumber" HeaderText="Number" SortExpression="WorkInvoiceNumber">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRecord" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
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
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" Text="Add New"
                                                            ToolTip="Click to Add New Work Invoice" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BtnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print "
                                                            ToolTip="Click to Print Work Invoice List" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close List of Work Invoice screen."
                                                            CausesValidation="False"></asp:Button>
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
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCostComparisonOfAPart_Ajax.aspx.vb"
    Inherits="Flypal.wfrptCostComparisonOfAPart_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part Cost Comparison</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="frmrptPartHitory" method="post" runat="server">
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
                                <td class="clsFormHeader1">
                                    <span class="clsFormHeader" id="lbltitle">Part Cost Comparison</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPartNo" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbPartList" ErrorMessage="Select the Part No." ClientValidationFunction="ValidatePartNo"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                //Aircraft validation
                                                function ValidatePartNo(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbPartList");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;

                                                    }

                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" width="100%">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtFromDate"
                                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtToDate"
                                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabelAuto">Part No.</span>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPartList" runat="server" DataTextField="Name"
                                                                                    DataValueField="ID" TabIndex="8">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblSupplier" class="clsLabelAuto">Supplier</span>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSupplier" runat="server" DataTextField="Name"
                                                                                    DataValueField="ID" TabIndex="8">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblType" runat="server" CssClass="clsLabel">Tran. Type </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" AutoPostBack="True">
                                                                                    <asp:ListItem Value="0" Selected="True">Outright</asp:ListItem>
                                                                                    <asp:ListItem Value="1">GRO</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                                                    Text="With Alternate Part" />
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ValidationGroup="a"
                                                                    ToolTip="Click to find list of Issue as per searching criteria" Text="Find Now"></asp:Button>--%>
                                                                <asp:ImageButton CssClass="clsSearch2btn" ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" ToolTip="Click to find records as per criteria." ValidationGroup="a" />
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
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="TopbtnExport" runat="server" TabIndex="0" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            ToolTip="Click to Export report" Enabled="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="TopbtnDisplay" runat="server" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="TopbtnClose" runat="server" CausesValidation="False"
                                                            Text="Close" ToolTip="Click to close" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of details for the Part No. as per criteria :  Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgPartList" runat="server" AllowPaging="True" AllowSorting="false"
                                                            AutoGenerateColumns="False" PageSize="25"
                                                            ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="PartNo" HeaderText="Part No." SortExpression="PartNo">
                                                                    <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemType" HeaderText="Type" SortExpression="ItemType">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Invoice Date" SortExpression="InvoiceDateFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice No." SortExpression="InvoiceNumber">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date" SortExpression="OrderDateFormatted">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderNumber" HeaderText="Order No." SortExpression="OrderNumber">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ConversionFactor" HeaderText="Factor" SortExpression="ConversionFactor">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No." SortExpression="ReleaseNoteNo">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Qty" HeaderText="Qty." SortExpression="Qty">
                                                                    <HeaderStyle  HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CRate" HeaderText="Rate" SortExpression="CRate">
                                                                    <HeaderStyle  HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CCommercialRate" HeaderText="Commercial Rate" SortExpression="CCommercialRate">
                                                                    <HeaderStyle  HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
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
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server" TabIndex="0" Text="Export to Excel"
                                                            ToolTip="Click to Export report" Width="100px" Visible="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" TabIndex="0" Text="Display" 
                                                            ToolTip="Click to display report" Visible="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False" 
                                                            Text="Close" ToolTip="Click to close" Visible="true" />
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
    </form>
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
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUnderWarrantyUnserviceableItemList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptUnderWarrantyUnserviceableItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Under Warranty Unserviceable Item List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/minus.png";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/plus.png";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblMain" class="clstablelistout" border="0">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table id="tblInner" class="clstablelistin" border="0">
                                <tr>
                                    <td class="clsFormHeader1">
                                        <span class="clsFormHeader" id="lblTitle">Warranty Part Status</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblFromDate" class="clsLabel">From Date </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtFromDate"
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            <%--  <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>--%>
                                                        </td>
                                                        <td align="right">
                                                            <span id="lblToDate" class="clsLabel">To Date </span>
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
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSearch" runat="server" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List as per searching criteria"
                                                    Text="Find Now" ValidationGroup="a"></asp:Button>--%>
                                                <asp:ImageButton CssClass="clsSearch2btn" ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" ToolTip="Click to find records as per criteria." />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto " Font-Bold="True">List of Under Warranty Unserviceable Item as per criteria : Record(s) found</asp:Label>
                                                <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgItemList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    PageSize="25" ShowHeaderWhenEmpty="True" SelectedRowStyle-BackColor="ButtonShadow"
                                                    BorderStyle="Solid" ForeColor="#333333"
                                                    DataKeyNames="ItemID,SerialNo,ReceiptItemID">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a href="javascript:showNestedGridView('ID-<%# Eval("ReceiptItemID") %>');">
                                                                    <img id="imageID-<%# Eval("ReceiptItemID") %>" alt="Click to show/hide details" border="0"
                                                                        src="images/plus.png" />
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ItemID" HeaderText="ItemID" Visible="False" />
                                                        <asp:BoundField DataField="ReceiptItemID" HeaderText="ReceiptItemID" Visible="False" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle Wrap="False" Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyStartDateFormatted" HeaderText="Warranty Start Date">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyExpiryDateFormatted" HeaderText="Warranty Expiry Date">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="100%" bgcolor="White" width="0px">
                                                                        <div id="ID-<%# Eval("ReceiptItemID") %>" style="display: none; position: relative; left: 25px;">
                                                                            <asp:GridView ID="dgTransactionDetails" runat="server" AutoGenerateColumns="False"
                                                                                OnRowCommand="dgTransactionDetails_RowCommand" Width="60%" BorderStyle="Solid"
                                                                                CellPadding="0" ForeColor="#333333" CssClass="clsGridLog" AlternatingRowStyle-CssClass="alt"
                                                                                RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
                                                                                DataKeyNames="ID,InvoiceID,Type" ShowHeaderWhenEmpty="True" PageSize="5">
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"></asp:BoundField>
                                                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" Visible="false"></asp:BoundField>
                                                                                    <asp:BoundField DataField="Type" HeaderText="Type" Visible="false"></asp:BoundField>
                                                                                    <asp:BoundField DataField="TypeName" HeaderText="Transaction">
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
                                                                                    </asp:BoundField>
                                                                                    <asp:ButtonField DataTextField="TranasactionNo" HeaderText="No." CommandName="TranasactionNo">
                                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="left" Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="SerialNo" HeaderText="Part No./Serial No." HtmlEncode="false">
                                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="left" Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                                                                        <ItemStyle HorizontalAlign="right" Wrap="false" />
                                                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="GROEffRate" HeaderText="GRO. Rate">
                                                                                        <ItemStyle HorizontalAlign="right" Wrap="false" />
                                                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="right" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="ControlDark" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="BtnPrint" runat="server" CausesValidation="False"
                                                                Text="Print" ToolTip="Click to Print" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False"
                                                                Text="Close" ToolTip="Click to Close" />
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
        </div>
        <%--Date Validations--%>
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
        <%--End --%>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
</body>
</html>

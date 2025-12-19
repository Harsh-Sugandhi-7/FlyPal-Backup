<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPendingPaymentListForReceipt_Ajax.aspx.vb"
    Inherits="Flypal.wfrptPendingPaymentListForReceipt_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Pending Payment List For Receipt</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td colspan="3" class="clsFormHeader1">
                                    <span id="lbltitle" class="clsFormHeader">Pending Payment List</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvvendor" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Please Select The Supplier" Display="None" ControlToValidate="txtSupplier"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvvendor" runat="server" ErrorMessage="Please Select The Vendor"
                                                Display="None" CssClass="clsLabelAuto" ControlToValidate="txtSupplier" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection Of Supplier</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                       <%-- <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Find Now" ToolTip=" Click to Find as per search criteria"></asp:Button>--%>
                                                          <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip=" Click to Find as per search criteria" />          
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgPartSearch" runat="server" AllowPaging="true" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                            CssClass="clsGridNewStyle" PagerSettings-Mode="NumericFirstLast" PageSize="25" GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                           <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="InvoiceID" HeaderText="Invoice ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="VendorID" HeaderText="Vendor ID"></asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Invoice Date">
                                                                    <HeaderStyle ></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceText" SortExpression="InvoiceText" HeaderText="Invoice Text">
                                                                    <HeaderStyle ></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceNo" SortExpression="InvoiceNo" HeaderText="Invoice No.">
                                                                    <HeaderStyle Wrap="False" ></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Amount"
                                                                    >
                                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PendingAmount" SortExpression="PendingAmount" HeaderText="Pending Amount"
                                                                   >
                                                                    <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
                                                                    <HeaderStyle  Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
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
                                <td colspan="3">
                                    <span id="lblStep3" class="clsLabelHeader">Step II. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4" cellspacing="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSuppliebName" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceDate" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceText" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceNo" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGrandAmount" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPendingAmount" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" align="right" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Click to Close Pending Payment List screen" />
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>

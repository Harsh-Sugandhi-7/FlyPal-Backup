<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartReliability_Ajax.aspx.vb"
    Inherits="Flypal.wfPartReliability_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Part Reliability</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
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
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                <tr>
                                                <td colspan="3" class="clsFormHeader1Newstyle">
                                                <span id="lblTitle" class="clstitle1">Part Reliability</span>
                                                </td>
                                                </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                HeaderText="Fill Up The Following Fields" />
                                                            <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ClientIDMode="Static"
                                                                ControlToValidate="txtSerialNo" CssClass="clsLabelAuto" Display="None" ErrorMessage="Serial No. Required"></asp:RequiredFieldValidator>
                                                                <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                            Display="None" ErrorMessage="Enter Whole Part No. and Description." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter Part No." ClientIDMode="Static" ></asp:RequiredFieldValidator>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span1" class="clsLabel">Part No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server" ToolTip="Select Part No./Description"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server"  ToolTip="Enter Serial No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="right">
                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now" ToolTip="Click to find list of records as per searching criteria" />--%>
                                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of records as  per searching criteria" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                                                 PageSize="25" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                <PagerStyle HorizontalAlign="Right" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="white" Font-Bold="True" ForeColor="black" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IssueNumber" HeaderText="Issue No."></asp:BoundField>
                                                                    <asp:BoundField DataField="RegNo" HeaderText="Reg. No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date" />
                                                                    <asp:BoundField DataField="ReceiptNumber" HeaderText="Receipt No." />
                                                                    <asp:BoundField DataField="StoreName" HeaderText="Store" />
                                                                    <asp:BoundField DataField="ItemType" HeaderText="Item Type" />
                                                                    <asp:BoundField DataField="ServiceabilityInDays" HeaderText="Serviceability (In Days)">
                                                                        <HeaderStyle HorizontalAlign="Right" />
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
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrint" runat="server"  CausesValidation="false" ToolTip="Click To Print"
                                                            Text="Print" />
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="false" ToolTip="Click To Close Part Reliability Screen"
                                                            Text="Close" />
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
    <!-- City Main Popup -->
    <!-------------------->
    </form>
    <script type="text/javascript">
         Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
             $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                 width: 275,
                 autoFill: false,
                 matchContains: true,
                 delay: 0
             });
         });       
     </script>
</body>

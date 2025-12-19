<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRFIDStockCheck_Ajax.aspx.vb"
    Inherits="Flypal.wfRFIDStockCheck_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Stock Check</title>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
    <form id="form2" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td colspan="6">
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <div style="width: 100%">
                            <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr class="clsFormHeader1Newstyle">
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="spntitle" class="clsFormHeader" runat="server">Stock Check</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                Text="Close" ToolTip="Click to Close Screen" />
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Enter RFID No.(s)" ControlToValidate="txtRFIDNo"
                                                    Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span id="Span1" class="clsLabelHeader" runat="server">Enter comma separated RFID Tag
                                                No's and click on Find Now button to get stock of items(s)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="spnPartNo" class="clsLabelMedium">RFID No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRFIDNo" runat="server" TextMode="MultiLine" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                    ClientIDMode="Static" ToolTip="Enter RFID Nos" Style="width: 702px; height: 62px;"></asp:TextBox>
                                            </td>
                                            <td colspan="2" align="right">
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
                                                    CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
                                                    ValidationGroup="1" CausesValidation="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="width: 100%">
                            <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="dgPartSearch" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast"
                                                    PageSize="50" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PartNo" HeaderText="Part No." SortExpression="PartNo">
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptTextNo" HeaderText="Receipt No.">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="StockBalanceQty" SortExpression="StockBalanceQty" HeaderText="Stock Qty.">
                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitName">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Store" HeaderText="Store" SortExpression="Store">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No." SortExpression="ReleaseNoteNo">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReleaseNoteDate" HeaderText="Release Note Date" SortExpression="ReleaseNoteDate">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
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

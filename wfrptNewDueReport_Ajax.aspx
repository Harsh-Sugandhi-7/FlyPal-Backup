<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptNewDueReport_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptNewDueReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Change Rate</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGrid">
                            <ContentTemplate>
                                <asp:Panel CssClass="clspanel1" ID="pnlmain" runat="server">
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td class="clsFormHeader1" colspan="2">

                                                <span class="clsFormHeader">Express Due Report</span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table id="Table1" class="clsTable1">
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelAuto" id="lblSearch">Aircraft</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                                DataTextField="RegNo" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span class="clsLabelAuto" id="Span1">Due Limit</span>
                                                        </td>
                                                        <td>
                                                            <asp:GridView CssClass="clsGridNewStyle" ID="gdvDuePeriodLimits" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />

                                                                <Columns>
                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Limit">
                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                        <ItemTemplate>
                                                                            <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px" ID="txtLimit" runat="server"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "PeriodLimit") %>' ToolTip="Enter corresponding Limit Value."
                                                                                BackColor="White"> </asp:TextBox>
                                                                        </ItemTemplate>

                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                                <table id="Table4">
                                                    <tr>
                                                        <td>
                                                            <%--<asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                                Text="Find Now" ToolTip="Click to find as per criteria"></asp:Button>--%>
                                                            <asp:ImageButton CssClass="clsSearch2btn" ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" ToolTip="Click to find records as per criteria." />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span class="clsLabelHeader">Note: Change search criteria and click on Find Now to get
                                                record(s)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:UpdatePanel ID="upnlBtns1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH" ID="btnPrint" TabIndex="0" Visible="false" runat="server"
                                                                        Text="Print"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH" ID="btnExport" TabIndex="0" runat="server"
                                                                        Text="Export to Excel"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH" ID="btnClose1" TabIndex="0" runat="server"
                                                                        Text="Close" ToolTip="Click to close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" align="left">
                                                <div style="width: 100%; margin-bottom: 3px;">
                                                    <asp:Label CssClass="clsLabelHeader" ID="lblResult" runat="server">List of Parts :</asp:Label>
                                                </div>
                                                <div style="width: 100%">
                                                    <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgPartSearch" runat="server" ClientIDMode="Static" PageSize="25"
                                                        ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" AllowPaging="false">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="AircraftAssemblyDetails" HeaderText="Assembly Info." HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ActivityTypeName" HeaderText="Activity" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Code" HeaderText="Monitor Info.">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DescDetails" HeaderText="Description" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" Width="300px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DoneOnDate" HeaderText="Done On">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DoneOnValue" HeaderText="Effective From/Done On" HtmlEncode="false">
                                                                <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DueOnValue" HeaderText="Due" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DueOnValueAsofAssembly" HeaderText="Due As Of Assembly"
                                                                HtmlEncode="false">
                                                                <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyCurrentValueByAirFrame" HeaderText="Due As Of Airframe"
                                                                HtmlEncode="false">
                                                                <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ElapsedValue" HeaderText="Elapsed" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingValue" HeaderText="Remaining" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PlannedWODetails" HeaderText="WO Details" HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left"></td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
                                                    <ContentTemplate>
                                                        <table id="Table2" cellpadding="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH" runat="server" Text="Close" ToolTip="Click to close"
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
    </form>
</body>
</html>

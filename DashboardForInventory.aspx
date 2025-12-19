<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardForInventory.aspx.vb"
    Inherits="Flypal.DashboardForInventory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Dashboard For Inventory</title>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tabOuter" border="0" cellspacing="1" cellpadding="1" style="width: 1100px">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlRadio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbPartNo" runat="server" GroupName="a" Text="Part No." AutoPostBack="True"
                                            Font-Size="10pt" CssClass="clsRadioButton" Checked="True"></asp:RadioButton>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbSerialNo" runat="server" Font-Size="10pt" GroupName="a" Text="Serial No."
                                            AutoPostBack="True" CssClass="clsRadioButton"></asp:RadioButton>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbReferenceNo" runat="server" GroupName="a" Font-Size="10pt"
                                            Text="Reference No." AutoPostBack="True" CssClass="clsRadioButton"></asp:RadioButton>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbCodeNo" runat="server" GroupName="a" Font-Size="10pt" Text="TaskCard/Code No."
                                            AutoPostBack="True" CssClass="clsRadioButton"></asp:RadioButton>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="L1" Height="7px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlControls" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <%--<asp:TextBox ID="txtSearch" runat="server" AutoPostBack="False" CssClass="clsTextBoxRemark"
                                        Font-Size="9pt" Visible="False" Width="400px"></asp:TextBox>--%>
                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                            placeholder="Enter Part No."></asp:TextBox>
                                    </td>
                                    <%-- <td>
                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto" Font-Size="9pt"
                                        Visible="False" Width="120px">Serial No.</asp:Label>
                                </td>--%>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtSerialNo" runat="server" placeholder="Enter Serial No." AutoComplete="off" MaxLength="100" CssClass="clsTextBoxSearch_Ajax"
                                            Width="370px" Visible="False"></asp:TextBox>
                                    </td>
                                    <%--<td>
                                    <asp:Label ID="lblReferenceNo" runat="server" CssClass="clsLabelAuto" Width="90px"
                                        Font-Size="9pt" Visible="False">Reference No.</asp:Label>
                                </td>--%>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtReferenceNo" runat="server" Visible="False" placeholder="Enter Reference No." CssClass="clsTextBoxSearch_Ajax"
                                            AutoComplete="off" Width="370px"></asp:TextBox>
                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txReferenceNo_Autocomplete" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                            CompletionInterval="1" ServicePath="DashboardForInventory.aspx" ServiceMethod="GetReferenceList"
                                            TargetControlID="txtReferenceNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                        </cc2:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl" runat="server" CssClass="clsLabel" Width="9px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                            ToolTip="Click to search"></asp:ImageButton>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:ImageButton ID="btnCloseImageButton" runat="server" ImageUrl="~/images/close.png"
                                            CssClass="clsSearch2btn" ToolTip="Click to close"></asp:ImageButton>
                                        <%-- <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="clsbtnH clsinfoH"
                                        ToolTip="Click to close"></asp:Button>--%>
                                    </td>
                                </tr>
                            </table>
                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                CompletionInterval="1" ServicePath="DashboardForInventory.aspx" ServiceMethod="GetPartNoDescriptionList"
                                TargetControlID="txtSearch" OnClientItemSelected="" UseContextKey="False" ContextKey=""
                                CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                OnClientShowing="ClientShowing">
                            </cc2:AutoCompleteExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlItemStockList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblItemList" runat="server" CssClass="clsLabelHeader2" Visible="False" BackColor="#95A3C7"
                                            Width="100%">From Stores</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgItemStockList1" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                            DataKeyNames="PartName,PartDescription" CellPadding="5" GridLines="Horizontal">
                                            <RowStyle Wrap="False" CssClass="clsdgItem"></RowStyle>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
                                            <Columns>
                                                <asp:BoundField DataField="ItemID" HeaderText="ItemID">
                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="PartName" HeaderText="Part No.">
                                                <HeaderStyle HorizontalAlign="Left" ForeColor="#FFFFFF"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                            </asp:BoundField>--%>
                                                <asp:ButtonField DataTextField="PartNoDescription" HeaderText="Part No./Description"
                                                    CommandName="PartStatus" ControlStyle-Font-Underline="false">
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:ButtonField>
                                                <%--<asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Wrap="true" ></ItemStyle>
                                            </asp:BoundField>--%>
                                                <asp:BoundField DataField="ServiceablePartBalQty" SortExpression="ServiceablePartBalQty"
                                                    HeaderText="Serviceable Stock Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right" BackColor="YellowGreen"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BalQty" HeaderText="Stock Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Unit" HeaderText="Unit"></asp:BoundField>
                                                <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MinStockLevel" SortExpression="MinStockLevel" HeaderText="Min. Stock Level">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Stock Detail" HeaderText="Stock Detail" CommandName="StockDetail"></asp:ButtonField>
                                                <asp:ButtonField Text="Transactions" HeaderText="Transactions" CommandName="Transactions"></asp:ButtonField>
                                                <asp:ButtonField Text="Bin Card" HeaderText="Bin Card" CommandName="BinCard"></asp:ButtonField>
                                                <asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlItemReceiptIssueTransactions" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblItemReceiptIssueTransactions" runat="server" CssClass="clsLabelHeader2" BackColor="#95A3C7"
                                            Visible="False" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgItemReceiptIssueTransactions1" runat="server" CssClass="clsGridNewStyle"
                                            Visible="False" AutoGenerateColumns="False" PageSize="25" AllowPaging="True"
                                            CellPadding="5" GridLines="Horizontal">
                                            <SelectedRowStyle Wrap="False"></SelectedRowStyle>
                                            <EditRowStyle Wrap="False"></EditRowStyle>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundField DataField="PHDate" HeaderText="Date">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IdentityNo" SortExpression="ReceiptNumber" HeaderText="Number">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ToFrom" HeaderText="From">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssueTo" HeaderText="To">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Rel. Note No.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReleaseNoteDate" HeaderText="Rel. Note Date">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RecQty" SortExpression="RecQty" HeaderText="In Qty">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssQty" SortExpression="IssQty" HeaderText="Out Qty.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StartDate" HeaderText="Cure Date">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CureQtrYear" HeaderText="Cure Qtrs">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ExpiryDate" HeaderText="Exp. Date">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ExpQtrYear" HeaderText="Exp. Qtrs">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                    <HeaderStyle HorizontalAlign="left" ForeColor="Black" Wrap="false"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Wrap="false"></ItemStyle>
                                                </asp:BoundField>
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
                <td>
                    <asp:UpdatePanel ID="upnlInstallationRemoval" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblInstallationRemoval1" runat="server" CssClass="clsLabelHeader2" BackColor="#95A3C7"
                                            Visible="False" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgInstallationRemoval1" runat="server" CssClass="clsGridNewStyle"
                                            AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal" Visible="False">
                                            <RowStyle Wrap="False" CssClass="clsdgItem"></RowStyle>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                            <HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
                                            <Columns>
                                                <asp:BoundField DataField="Event" HeaderText="Event" HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Date" HeaderText="Date">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RegNoModelSerialNo" HeaderText="INST. ON A/C REGN/ S/N. &amp; Assembly"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PartDet" HeaderText="Part Details"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneBy" HeaderText="By ACTIVITY">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyInstallationValueFormatted" HeaderText="INST. AT A/C HRS/LANDINGS/CYCLES/RINS"
                                                    HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CompInstRemValue" HeaderText="SINCE NEW" HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CompOverHaulValue" HeaderText="SINCE OH/CHK/INSP." HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IsRemoved" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsRemoved" ItemStyle-CssClass="hideGridColumn" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="RemoveRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName="RemoveRecord" Style="height: 20px; width: 20px" ToolTip="Remove Component" ImageUrl="~/images/remove.jpg"
                                                            Visible='<%# Eval("Event").ToString() = "Installation" andAlso not Eval("IsRemoved")  %>' />
                                                        <asp:ImageButton ID="InstallRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName="InstallSelected" ToolTip="Install Component" ImageUrl="~/images/InstallSelect.png"
                                                            Visible='<%# Eval("Event").ToString() = "Removal" andAlso   Eval("IsRemoved") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlModelMonitorAMPRefStatusList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblModelMonitorAMPRefStatusList" runat="server" CssClass="clsLabelHeader2" BackColor="#95A3C7"
                                            Visible="False" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgModelMonitorAMPRefStatusList" Visible="False" runat="server" AllowPaging="true"
                                            AutoGenerateColumns="False" PageSize="5" ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle"
                                            CellPadding="5" GridLines="Horizontal">
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                            <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
											<PagerSettings FirstPageText="First" LastPageText="Last" />
											<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TaskNoCodeNo" HeaderText="Code No." SortExpression="TaskNoCodeNo">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AircraftDetails" SortExpression="AircraftDetails" HtmlEncode="false" HeaderText="Aircraft Details">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HtmlEncode="false"
                                                    HeaderText="Assembly Info" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TypeName" HeaderText="Task Type" HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HtmlEncode="false"
                                                    HeaderText="Maintenanace Info" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On" SortExpression="DoneOnFormatted">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PeriodNameForWeb" HtmlEncode="false" HeaderText="Period" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FrequencyValue" HtmlEncode="false" HeaderText="Frequency">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnValue" HtmlEncode="false" HeaderText="Effective From/ DoneOn Value">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CurrentValue" HtmlEncode="false" HeaderText="Current">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ElapsedValue" HtmlEncode="false" HeaderText="Elapsed">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DueOnValue" HtmlEncode="false" HeaderText="Due At.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RemainingValue" HtmlEncode="false" HeaderText="Remaining">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IsApplicable" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="IsApplicable"></asp:BoundField>
                                                <asp:BoundField DataField="IsMaster" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="IsMaster"></asp:BoundField>
                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%-- <span id="button">Login</span>--%>
                                                        <div class="dropdown">
                                                            <div class="dropdownbtn-content">
                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="EditViewRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                CommandName="ComplyRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/Comply.jpg"
                                                                                Enabled='<%#  Eval("IsApplicable")%>' />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="IDHistory" runat="server" Visible='<%# not Eval("IsMaster")%>' CommandArgument='<%# Eval("ID") %>'
                                                                                CommandName="HistoryRec" ImageUrl="~/images/History.png" />
                                                                        </td>
                                                                      <%--  <td>
                                                                            <asp:ImageButton ID="View" runat="server" Visible='<%#  Eval("IsAttachmentAdded")%>' CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                        </td>--%>
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
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlTaskCardList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTaskCardResultList" runat="server" CssClass="clsLabelHeader2" Visible="False" BackColor="#95A3C7"
                                            Width="100%">From Stores</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgTaskCardList" runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                            ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AutoGenerateColumns="False"
                                            CellPadding="5" GridLines="Horizontal">
                                            <RowStyle Wrap="False" CssClass="clsdgItem"></RowStyle>
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TaskDesc" HeaderText="Description/Subject">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AMPIssueRev" HeaderText="AMP Issue/Rev">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="INSPTypeInterval" HeaderText="INSP. Type Interval">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RevDate" HeaderText="Revision Date">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Equipment" HeaderText="Equipment">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Material" HeaderText="Material">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EstimatedHours" HeaderText="Estimated Hr.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
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
        <%--autocomplete css functions--%>
        <script type="text/javascript">
            //bold input value in list...
            function ClientPopulated(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
            //Alternate item style
            function ClientShowing(source, eventArgs) {
                $.elements = $(source.get_completionList());
                $.elements.find(".ac_results_li").each(function (i) {
                    if (i % 2 == 0) {
                        //$(this).addClass("ac_even");
                    }
                    else {
                        $(this).addClass("ac_odd");
                    }
                });
            }
            //add loader to textbox
            function ClientPopulating(source, e) {
                $("#" + source._element.id).addClass("ac_loading");
            }
            //remove loader from textbox
            function ClientHiding(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
        </script>
        <%--End--%>
        <!-- Popup For ShowPartNoStatus -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyShowPartNoStatus" Text="ShowPartNoStatus"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlShowPartNoStatus" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeShowPartNoStatus" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupShowPartNoStatus" runat="server" TargetControlID="btnDummyShowPartNoStatus"
            PopupControlID="pnlShowPartNoStatus" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function OpenShowPartNoStatusWindow() {
                try {
                    $("#IframeShowPartNoStatus").attr("src", "wfrptShowPartNoStatus_Ajax.aspx?Type=FromPurchaseOrder");
                    $("#btnDummyShowPartNoStatus").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForShowPartNoStatus() {
                var ShowPartNoStatuswindow = $find("<%=mdlPopupShowPartNoStatus.ClientID %>");
                //close popup window
                ShowPartNoStatuswindow.hide();
                //           release resources
                $("#IframeShowPartNoStatus").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnShowPartNoStatus").click();
            }
        </script>
        <!---End-->
        <!-- Popup For PartStatus -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPartStatus" Text="PartStatus" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPartStatus" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframePartStatus" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupPartStatus" runat="server" TargetControlID="btnDummyPartStatus"
            PopupControlID="pnlPartStatus" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFramePartStatusStateComplete() {
                if (Page_IsValid) {
                    $("#btnDummyPartStatus").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                else {

                    $get("AjaxLoader").style.visibility = "hidden";
                }
            }
            function OpenPartStatusWindow() {
                try {
                    $("#IframePartStatus").attr("src", "wfPartStatus.aspx?Type=FromDashboardForInventory");
                    $("#btnDummyPartStatus").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForPartStatus() {
                var PartStatuswindow = $find("<%=mdlPopupPartStatus.ClientID %>");
                //close popup window
                PartStatuswindow.hide();
                //           release resources
                $("#IframePartStatus").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnPartStatus").click();
            }
        </script>
        <!---End-->

        <!--Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyServiceHistory" Text="Service History" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeServiceHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupServiceHistory" runat="server" TargetControlID="btnDummyServiceHistory"
            PopupControlID="pnlServiceHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceHistoryStateComplete() {
                $("#btnDummyServiceHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeServiceHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorServiceStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyServiceHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForServiceHistory() {
                var ServiceHistorywindow = $find("<%=mdlPopupServiceHistory.ClientID %>");
                //close Service History popup window
                ServiceHistorywindow.hide();
                //           release resources
                $("#IframeServiceHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnServiceHistory").click();
            }
        </script>
        <!-- End-->

        <!--Inspection History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspectionHistory" Text="Inspection History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlInspectionHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspectionHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspectionHistory" runat="server" TargetControlID="btnDummyInspectionHistory"
            PopupControlID="pnlInspectionHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspectionHistoryStateComplete() {
                $("#btnDummyInspectionHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspectionHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeInspectionHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorInspStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyInspectionHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspectionHistory() {
                var InspectionHistorywindow = $find("<%=mdlPopupInspectionHistory.ClientID %>");
                //close Inspection History popup window
                InspectionHistorywindow.hide();
                //           release resources
                $("#IframeInspectionHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnInspectionHistory").click();
            }
        </script>
        <!-- End-->
        <!--Directive History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDirectiveHistory" Text="Directive History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDirectiveHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDirectiveHistory" runat="server" TargetControlID="btnDummyDirectiveHistory"
            PopupControlID="pnlDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameDirectiveHistoryStateComplete() {
                $("#btnDummyDirectiveHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDirectiveHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDirectiveHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyDirectiveHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForDirectiveHistory() {
                var DirectiveHistorywindow = $find("<%=mdlPopupDirectiveHistory.ClientID %>");
                //close Directive History popup window
                DirectiveHistorywindow.hide();
                //           release resources
                $("#IframeDirectiveHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnDirectiveHistory").click();
            }
        </script>
        <!-- End-->
        <!--Comp Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCompServiceHistory" Text="Comp Service History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlCompServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeCompServiceHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCompServiceHistory" runat="server" TargetControlID="btnDummyCompServiceHistory"
            PopupControlID="pnlCompServiceHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCompServiceHistoryStateComplete() {
                $("#btnDummyCompServiceHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenCompServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeCompServiceHistory").attr("src", "wfUpdateComplyHistoryCompMonitorServiceStatusList_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyCompServiceHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForCompServiceHistory() {
                var CompServiceHistorywindow = $find("<%=mdlPopupCompServiceHistory.ClientID %>");
                //close Comp Service History popup window
                CompServiceHistorywindow.hide();
                //           release resources
                $("#IframeCompServiceHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnCompServiceHistory").click();
            }
        </script>
        <!-- End-->
        <!-- Comply History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
            PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameRemHistoryStateComplete() {
                $("#btnDummyRemHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenCompInspHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeRemHistory").attr("src", "wfUpdateComplyHistoryCompMonitorInspStatusList_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyRemHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForRemHistory() {
                var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
                //close Removal History popup window
                RemHistorywindow.hide();
                //           release resources
                $("#IframeRemHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnInspHistory").click();
            }
        </script>
        <!-- End-->
         <!--Comp Directive History Popup Window -->
   <div style="display: none">
       <asp:Button runat="server" ID="btnDummyCompDirectiveHistory" Text="Comp Directive History"
           ClientIDMode="Static" />
   </div>
   <asp:Panel runat="server" ID="pnlCompDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
       Style="height: 100%; width: 100%;">
       <iframe id="IframeCompDirectiveHistory" frameborder="0" height="100%" width="100%"
           src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
   </asp:Panel>
   <cc2:ModalPopupExtender ID="mdlPopupCompDirectiveHistory" runat="server" TargetControlID="btnDummyCompDirectiveHistory"
       PopupControlID="pnlCompDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
   </cc2:ModalPopupExtender>
   <script type="text/javascript">
       function IFrameCompDirectiveHistoryStateComplete() {
           $("#btnDummyCompDirectiveHistory").click();
           $get("AjaxLoader").style.visibility = 'hidden';
       }

       function OpenCompDirectiveHistoryWindow() {
           try {

               $get("AjaxLoader").style.visibility = 'visible';
               $("#IframeCompDirectiveHistory").attr("src", "wfUpdateComplyHistoryCompMonitorModStatusList_AJAX.aspx?Type=pup");

               if (!$.browser.msie) {
                   $("#btnDummyCompDirectiveHistory").click();
                   $get("AjaxLoader").style.visibility = 'hidden';
               }

               return false;
           } catch (e) {
               alert(e);
           }

       }
       function ParentCallBackFunctionForCompDirectiveHistory() {
           var CompDirectiveHistorywindow = $find("<%=mdlPopupCompDirectiveHistory.ClientID %>");
           //close Comp Directive History popup window
           CompDirectiveHistorywindow.hide();
           //           release resources
           $("#IframeCompDirectiveHistory").attr("src", "JavaScript:''");
           //call image button
           $("#hdnBtnCompDirectiveHistory").click();
       }
   </script>
   <!-- End-->
    </form>
    <%--<script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("dgItemStockList1");
            var rowData;
            var regex = /(&nbsp;|<([^>]+)>)/ig
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML.replace(regex, '');
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }    
    </script>--%>
    <%-- <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 400,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>--%>
</body>
</html>

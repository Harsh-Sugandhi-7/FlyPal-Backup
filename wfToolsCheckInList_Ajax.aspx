<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfToolsCheckInList_Ajax.aspx.vb"
    Inherits="Flypal.wfToolsCheckInList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tools Check In List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 06-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="clsFormHeader1Newstyle">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 99%" valign="middle">
                                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List Of Tools Received</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Item"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close Tools Received List screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 1%" align="center">
                                                        <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                            class="fa fa-star fa-spin fa-5x circle-icon"
                                                            title="Mark As Favourites"></i>
                                                            <%--  Ajay 07-Nov-2022--%>
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
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
                                                                                <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" 
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
                                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabel">Receipt No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbRecText" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                                    DataTextField="Text" DataValueField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                                    MaxLength="100"></asp:TextBox>
                                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="184px"
                                                                                    Visible="False" MaxLength="10"></asp:TextBox>
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
                                                                <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ValidationGroup="a"
                                                                ToolTip="Click to find list of Tools Received as per searching criteria" Text="Find Now">
                                                            </asp:Button>--%>
                                                                <asp:ImageButton ID="btnFindNow" runat="server" ValidationGroup="a" ImageUrl="~/images/Search2.png"
                                                                    CssClass="clsSearch2btn" ToolTip="Click to find list of Tools Received as per searching criteria" />
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
                                                                    <table>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <span id="Span2" class="clsLabel">Returned By Employee</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtReceivedFromEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                                    AutoPostBack="true" CssClass="clsTextBoxTagSearch" onChange="SetEmpIdonChange('txtReceivedFromEmployee','txtReceivedFromEmployee_Autocomplete')"></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtReceivedFromEmployee_Autocomplete"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                    MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfToolsCheckInList_Ajax.aspx"
                                                                                    ServiceMethod="GetEmployeeList" TargetControlID="txtReceivedFromEmployee" OnClientItemSelected="SetID"
                                                                                    UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                    OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                </cc2:AutoCompleteExtender>
                                                                                <asp:HiddenField ID="hdnReceivedFromEmployeeId" runat="server" ClientIDMode="Static" />
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span6" class="clsLabelAuto" style="width: 100%">Category </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    DataValueField="ID" DataTextField="Name">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <span id="lblPartNoSearch" class="clsLabel">Part No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblDescriptionSearch" class="clsLabel">Description</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDescriptionSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>

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
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>&nbsp;
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">As per criteria :  Record(s) found.</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="clsLabel">Tools Check In Against</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbToolsCheckInAgainst" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchComboSmall"
                                                                        DataValueField="ID" DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbWorkOrder" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                        DataTextField="WONumber" AutoPostBack="true" DataValueField="ID" Visible="false">
                                                                    </asp:DropDownList>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                &nbsp;
                                                                <asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
                                                                &nbsp;

                                                            <asp:DropDownList ID="cmbShowE" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="55px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                                <asp:ListItem Value="0">5</asp:ListItem>
                                                                <asp:ListItem Value="1">10</asp:ListItem>
                                                                <asp:ListItem Value="2">15</asp:ListItem>
                                                                <asp:ListItem Value="3">20</asp:ListItem>
                                                                <asp:ListItem Value="4">25</asp:ListItem>
                                                                <asp:ListItem Value="5">30</asp:ListItem>
                                                                <asp:ListItem Value="6">40</asp:ListItem>
                                                                <asp:ListItem Value="7">45</asp:ListItem>
                                                                <asp:ListItem Value="8">50</asp:ListItem>
                                                                <asp:ListItem Value="9">55</asp:ListItem>
                                                            </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <%--  <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Item"
                                                                            Text="Add New" CausesValidation="False"></asp:Button>--%>
                                                                        </td>
                                                                        <td>
                                                                            <%--<asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Tools Received List screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button>--%>
                                                                            <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
                                                                                AutoPostBack="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgToolsReceiptList" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                            ShowHeaderWhenEmpty="true" AllowPaging="True" AutoGenerateColumns="False" PageSize="25"
                                                            CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ReceiptID" HeaderText="ReceiptID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--0--%>
                                                                <asp:BoundField Visible="False" DataField="InvoiceID" HeaderText="InvoiceID" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="RecCumInvDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt / Invoice No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="ToolsReceivedByEmployeeName" SortExpression="ToolsReceivedByEmployeeName"
                                                                    HeaderText="Returned By">
                                                                    <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <%--<asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ReceiptID") %>'
                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField><%--7 
                                                          <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="Delete" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ReceiptID") %>'
                                                                        CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField><%--8--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <%--11--%>
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ReceiptID") %>'
                                                                                                CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ReceiptID") %>'
                                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                    </tr>
                                                                                   
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TransID" HeaderStyle-CssClass="hideGridColumn" HeaderText="TransTypeID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
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
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <%--<asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Item"
                                                        Text="Add New" CausesValidation="False"></asp:Button>--%>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Tools Received screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>--%>
                                                    </td>
                                                    <%--Ajay 07-Nov-2022--%>
                                                    <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                        Style="display: none;"></asp:Button>
                                                    <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
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
        <%-- Autocomplete functions to set id--%>
        <script type="text/javascript">
            function SetID(source, e) {
                //get id from autocomplete list
                var node;
                var value = e.get_value();

                if (value) node = e.get_item();
                else {
                    value = e.get_item().parentNode._value;
                    node = e.get_item().parentNode;
                }

                var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
                source.get_element().value = text;

                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtReceivedFromEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnReceivedFromEmployeeId');
                }
                textbox.value = value.toString();
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetEmpIdonChange(cntrl, extender) {
                var cntrlName = '#' + cntrl;
                var popup = $find(extender);
                var complist = popup.get_completionList();
                var text = $(cntrlName).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;
                        if (cntrl == "txtReceivedFromEmployee") {
                            textbox = document.getElementById('hdnReceivedFromEmployeeId');
                        }
                        textbox.value = val.toString();
                        return;
                    }
                }
                if (cntrl == "txtReceivedFromEmployee") {
                    textbox = document.getElementById('hdnReceivedFromEmployeeId');
                }
                textbox.value = '';
                return;
            }
        </script>
        <!--Ajay S 07-Nov-2022 -->
        <script type="text/javascript">
            function FunctionFav(x) {
                if (x.classList.contains("fa-star")) {
                    x.classList.remove("fa-star");
                    x.classList.add("fa-star-o");
                    x.style.color = 'black';
                    x.style.border = 'black';
                    $("#hdnBtnRemoveFav").click();
                }
                else {
                    x.classList.remove("fa-star-o");
                    x.classList.add("fa-star");
                    x.style.color = '#fff';
                    x.style.border = 'black';
                    $("#hdnBtnMarkFav").click();
                }
            }
            function MarkFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
        </script>
        <!--Ajay E -->
    </form>
</body>
</html>

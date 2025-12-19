<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfQuotationList_Ajax.aspx.vb"
    Inherits="Flypal.wfQuotationList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quotation List</title>
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
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblQuotationList" runat="server" CssClass="clsFormHeader">List Of Quotation</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Quotation"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to print Quotation List"
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Quotation screen."
                                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                                                                                <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
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
                                                                                <span id="lblFrom" class="clsLabel" runat="server">From Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                                </cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox">
                                                                                </cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblTo" class="clsLabel" runat="server">To Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
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
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblPartNoSearch" class="clsLabel">Part No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabel">Quotation No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbQuotationText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    MaxLength="6"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="6"></asp:TextBox>
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
                                                                    ToolTip="Click to find list of Quotation as per searching criteria" Text="Find Now">
                                                                </asp:Button>--%>

                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                    ToolTip="Click to find list of Quotation as  per searching criteria" />
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
                                                                <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnFindNow" Style="max-height: 200px;
                                                                    overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblEnquiryNo" class="clsLabel" runat="server">Enquiry No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbEnquiryText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEnquiryNo" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblVendor" class="clsLabel" runat="server">Vendor</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtVendorName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="SpanStatus" class="clsLabel">Status</span>
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
                                                                                <span id="Span7" class="clsLabel">Priority</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="160px">
                                                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Low</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Medium</asp:ListItem>
                                                                                    <asp:ListItem Value="3">High</asp:ListItem>
                                                                                    <asp:ListItem Value="4">AOG</asp:ListItem>
                                                                                </asp:DropDownList>
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
                                <td colspan="2">
                                    <span id="lblInfo" class="clsLabelAuto" style="display: none">Select Quotation from
                                        the list. Click On Edit Link To Modify The Selected Quotation.Click On Delete link
                                        To Delete The Selected Quotation.Click On Add New button To Add A New Quotation.</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Quotation as per criteria : Record(s) found</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <%--<td>
                                                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Quotation"
                                                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to print Quotation List"
                                                                                            Text="Print" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Quotation screen."
                                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>--%>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgQuotationList" runat="server" AllowPaging="True" AllowSorting="True"
                                                            AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25" ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="QuotationTextNo" HeaderText="Number" SortExpression="QuotationTextNo">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total" SortExpression="CGrandTotal">
                                                                    <HeaderStyle HorizontalAlign="Right"  Wrap="false"/>
                                                                    <ItemStyle HorizontalAlign="Right"  Wrap="false"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false"/>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"/>
                                                                </asp:BoundField>
                                                                <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Attach">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="View" CommandName="ViewRec" CommandArgument="1"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("ImageSize") > 0 %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>




                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                                Visible='<%#  Eval("ImageSize") > 0 %>' />
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


                                                                <asp:BoundField DataField="ImageSize" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Quotation"
                                                            Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Quotation List"
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Quotation screen."
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

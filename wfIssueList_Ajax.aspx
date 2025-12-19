<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfIssueList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Issue List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 08-Nov-2022--%>
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
        function openFilel() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="clsFormHeader1Newstyle">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 99%" valign="middle">
                                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List Of Issue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Issue"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Issue List"
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close Issue List screen"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 1%" align="center">
                                                        <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                            class="fa fa-star fa-spin fa-5x circle-icon"
                                                            title="Mark As Favourites"></i>
                                                            <%--  Ajay 08-Nov-2022--%>
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
                                                                                <span id="lblFromDate" class="clsLabel" runat="server">From Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblToDate" class="clsLabel" runat="server">To Date</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span3" class="clsLabel">Issue No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbIssueText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtIssueNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--    <tr>
                                                                        <td>
                                                                            <span id="lblPartNoSearch" class="clsLabel">Part No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblSerialNoSearch" class="clsLabel">Serial No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSerialNoSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                MaxLength="100">
                                                                            </asp:TextBox>
                                                                        </td>--%>
                                                                        <%-- <td>
                                                                            <span id="Span3" class="clsLabel">Issue No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbIssueText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtIssueNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                Width="55px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>--%>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ValidationGroup="a"
                                                                ToolTip="Click to find list of Issue as per searching criteria" Text="Find Now">
                                                            </asp:Button>--%>
                                                                <asp:ImageButton ID="btnFindNow" runat="server" ValidationGroup="a" ImageUrl="~/images/Search2.png"
                                                                    CssClass="clsSearch2btn" ToolTip="Click to find list of Issue as per searching criteria" />
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
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span2" class="clsLabel">Order No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                                <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearch" Visible="false"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span1" class="clsLabel">Receipt No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    AutoPostBack="True" DataTextField="Text" DataValueField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span6" class="clsLabelAuto" style="width: 100%">Issue To </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbIssueToType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    AutoPostBack="true" DataTextField="Text" DataValueField="Text">
                                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Supplier</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                                                    <asp:ListItem Value="3">Store</asp:ListItem>
                                                                                    <asp:ListItem Value="4">Discard</asp:ListItem>
                                                                                    <asp:ListItem Value="5">Customer</asp:ListItem>
                                                                                    <asp:ListItem Value="6">WorkShop</asp:ListItem>
                                                                                    <%--<asp:ListItem Value="7">WorkOrder</asp:ListItem>
                                                                                <asp:ListItem Value="8">Requisition</asp:ListItem>--%>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="Span5" class="clsLabel">Req. No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    AutoPostBack="True" DataTextField="Text" DataValueField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtReqNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span4" class="clsLabel">WO. No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbWoText" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
                                                                                    DataTextField="WOText" DataValueField="WOText">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
                                                                                    Width="55px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="Span7" class="clsLabel">Status</span>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboSmall">
                                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Opened</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                                                    <asp:ListItem Value="4">Cancelled</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblFromStore" class="clsLabelAuto" style="width: 100%">From Store</span>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="txtFromStore" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblReleaseNoteNoSearch" class="clsLabelAuto" style="width: 100%">Release Note
                                                                                No.</span>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="txtReleaseNoteNoSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblBatchNoSearch" class="clsLabel">Batch No.</span>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtBatchNoSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    MaxLength="100">
                                                                                </asp:TextBox>
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
                                                                            <td></td>
                                                                            <td>
                                                                                <span id="lblSerialNoSearch" class="clsLabel">Serial No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSerialNoSearch" runat="server" CssClass="clsTextBoxTagSearch"
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
                                <td style="padding-left: 3px;">
                                    <span id="lblInfo" class="clsLabelAuto" style="display: none">Select Issue from the
                                    list. Click On Edit Link To Modify The Selected Issue. Click On Delete Link To Delete
                                    The Selected Issue. Click On Add New button To Add A New Issue.</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">

                                    <asp:UpdatePanel ID="upnlIssueTo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">As per criteria :  Record(s) found.</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <span id="lblIssueTo" class="clsLabelAuto">Issue To</span>

                                                        <asp:DropDownList ID="cmbIssueTo" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="128px"
                                                            AutoPostBack="True">
                                                            <asp:ListItem Value="0">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="1">Store</asp:ListItem>
                                                            <asp:ListItem Value="2">Supplier</asp:ListItem>
                                                            <asp:ListItem Value="3">Discard</asp:ListItem>
                                                            <asp:ListItem Value="4">Customer</asp:ListItem>
                                                            <asp:ListItem Value="5">WorkShop</asp:ListItem>
                                                            <asp:ListItem Value="6">WorkOrder</asp:ListItem>
                                                        </asp:DropDownList>

                                                        <span id="lblAs" class="clsLabelAuto">As</span>

                                                        <asp:DropDownList ID="cmbIssueAs" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                            DataValueField="IDWithSameValue">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <%-- <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Issue"
                                                                    Text="Add New" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Issue List"
                                                                    Text="Print" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Issue List screen"
                                                                    Text="Close" CausesValidation="False"></asp:Button>
                                                            </td>--%>
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

                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                &nbsp;
                                                        <asp:Label ID="lblShowEntries" CssClass="clsLabelAuto" runat="server" Text="Show Entries"></asp:Label>
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
                                                        <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgIssueList" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                            ShowHeaderWhenEmpty="true" PageSize="25" AllowPaging="false" AutoGenerateColumns="False"
                                                            CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ILDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="IssueNo" SortExpression="IssueNo" HeaderText="Number">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Issue Type">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Issue To">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="AuthorizedByName" SortExpression="AuthorizedByName" HeaderText="Authorized By ">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                                <%-- <asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                                <%--9--%>
                                                                <%-- <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                                <%--10--%>
                                                                <%-- <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                        Visible='<%#  Eval("Size")>0 %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                                <%--11--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                                                Visible='<%#  Eval("Size")>0 %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="Size" HeaderText="Size"></asp:BoundField>
                                                                <%--12--%>
                                                                <asp:BoundField DataField="TransID" HeaderText="TransTypeID" SortExpression="TransID"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <%-- 13--%>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <asp:Panel ID="PnlPaging" runat="server">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <div style="width: 100%;">
                                                                    <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                    class="letterbox" />
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First"></span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous"></span>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div align="center">
                                                                                    <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                    </asp:TextBox>
                                                                                    <cc2:SliderExtender ID="SliderExtender11" runat="server" TargetControlID="Slidercontrol"
                                                                                        Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                        Length="300" />
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                            </td>
                                                                            <td>
                                                                                <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                            </td>
                                                                            <td>
                                                                                <span>of </span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <%--<td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Issue"
                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print list of Issues"
                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Issue screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>--%>
                                                    <%--Ajay 08-Nov-2022--%>
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
        <!--Sankalp 04-09-25 WorkOrderAttach Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAttachStateComplete() {
                $("#btnDummyAttach").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAttachWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAttach").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForAttach() {
                var Attachwindow = $find("<%=mdlAttach.ClientID %>");
                //close popup window
                Attachwindow.hide();
                //release resources
                $("#IframeAttach").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnAttach").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
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
        <!-- Slider control events  -->
        <script type="text/javascript">
            //initialize slider control and attach events
            function pageLoad(sender, e) {
                var slider = $find('<%=SliderExtender11.ClientID %>');
                if (slider) {
                    slider.add_slideStart(sliderStart);
                    slider.add_slideEnd(sliderEnd);
                    slider.add_valueChanged(valChanged);
                }
            }


        </script>
        <script type="text/javascript">
            function valChanged() {
                var showval = $('#valuetodisplay');
                var curval = $('#<%=Slidercontrol.ClientID %>');
                showval.html(curval.val());
            }


        </script>
        <script type="text/javascript">

            function sliderStart() {
                $('#valuetodisplay').css('display', 'inline-block');
            }
        </script>
        <script type="text/javascript">
            function sliderEnd() {
                $('#valuetodisplay').css('display', 'none');

            }
        </script>
        <script type="text/javascript">
            function setValue(val) {
                if (val === 0) {//first
                    var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender11.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender11.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender11.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender11.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                    slider.set_Value(maxval);
                }
            }
        </script>
        <!-- End  -->
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

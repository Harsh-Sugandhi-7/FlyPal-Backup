<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionList_Ajax.aspx.vb" enableEventValidation="true"
    Inherits="Flypal.wfRequisitionList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Requisition List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
   <%-- <link id="MainStyle" type="text/css" rel="stylesheet">--%>
   <link href="Styles.css" rel="stylesheet" type="text/css" />
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
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" nowrap>
                                <%--   <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="LblTitle" runat="server" CssClass="clstitle1">List of Requisition(s)
                                            <asp:Label ID="lblTotal" runat="server" CssClass="clstitle1"></asp:Label></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" class="clsFormHeader1Newstyle">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="LblTitle" runat="server" CssClass="clsFormHeader">List of Requisition(s)
                                                                    </asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                Text="Add New" ToolTip="Click to Add New Requisition" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Text="Print" Visible="false" ToolTip="Click to Print list of Requisition" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                Text="Close" ToolTip="Click to Close list of Requisition" />
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
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
                                                                            <span id="lblFromDate" class="clsLabel" runat="server">From Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                            </cc2:CalendarExtender>
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
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span3" class="clsLabel">Requisition No.</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                AutoPostBack="True" DataTextField="Text" DataValueField="Text">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Width="40px"
                                                                                MaxLength="8"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Requisition as per searching criteria"
                                                                Text="Find Now" ValidationGroup="a"></asp:Button>--%>
                                                            <asp:ImageButton ID="imgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                ToolTip="Click to find list of Requisition as per searching criteria" ValidationGroup="a">
                                                            </asp:ImageButton>
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
                                                            <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="imgFindNow" Style="max-height: 200px;
                                                                overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                <table>
                                                                    <tr>
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
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="txtDescriptionSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                MaxLength="100">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Span7" class="clsLabel">Status</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
                                                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Opened</asp:ListItem>
                                                                                <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                                                <asp:ListItem Value="4">Cancelled</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblRequisitionLocation" class="clsLabelAuto">Location</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbRequisitionLocation" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                                DataTextField="Name" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblType" class="clsLabelAuto" runat="server" visible="false">Type</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbType" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="160px"
                                                                                Visible="False">
                                                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                                <asp:ListItem Value="1">Part Request</asp:ListItem>
                                                                                <asp:ListItem Value="2">Part Purchase</asp:ListItem>
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
                                <span id="lblInfo" class="clsLabelAuto" style="display: none">Select Requisition from
                                    the list. Click on Edit link to Modify the Selected Requisition. Click on Delete
                                    link to Delete the Selected Requisition. Click on Add New button to Add a New Requisition.</span>
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="2">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto " Font-Bold="True">List of Requisition as per criteria : Record(s) found</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblGreen" runat="server" CssClass="clsColorLabel" Height="18px" Width="18px"
                                                        BackColor="Green" ForeColor="Green"></asp:Label>
                                                    <asp:Label ID="lblGreenInfo" runat="server" CssClass="clsLabelauto">Completed Requisition</asp:Label>&nbsp;
                                                    <asp:Label ID="lblYellow" runat="server" CssClass="clsColorLabel" BackColor="Yellow"
                                                        Height="18px" Width="18px" Visible="false" ForeColor="Yellow"></asp:Label>
                                                    <asp:Label ID="lblYellowInfo" runat="server" CssClass="clsLabelauto" Visible="false">Completed Order</asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="lblOrange" runat="server" CssClass="clsColorLabel" BackColor="Orange"
                                                        Height="18px" Width="18px" Visible="false" ForeColor="Orange"></asp:Label>
                                                    <asp:Label ID="lblOrangeInfo" runat="server" CssClass="clsLabelauto" Visible="false">Partial Order</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton ID="lnkCreateRequisition" runat="server" Width="300px" CssClass="clsHyperlink1"
                                                                            Font-Underline="true" ToolTip="Click to create Requisition of Job Spares Items(s)"
                                                                            Visible="false">Create Re-Order Level Item(s) Requisition</asp:LinkButton>
                                                                    </td>
                                                                    <%--    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Text="Add New" ToolTip="Click to Add New Requisition" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Text="Print" Visible="false" ToolTip="Click to Print list of Requisition" />
                                                                    </td>
                                                                 <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Text="Close" ToolTip="Click to Close list of Requisition" />
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            &nbsp;
                                                            <asp:Label ID="lblShowEntries" runat="server" Text="Show Entries"></asp:Label>
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
                                               
                                                <td align="right" colspan="2">
                                                    <asp:UpdatePanel ID="upnlSearchBox" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
                                                                AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="dgRequisitionList" runat="server" AllowSorting="True" AllowPaging="true"
                                                        AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                        OnRowDataBound="OnRowDataBound" EnableViewState="true" GridLines="Horizontal"
                                                        PageSize="10" ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                        <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"  />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle HorizontalAlign="Left"  ForeColor="Black" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionTextNo" HeaderText="Requisition No."   SortExpression="RequisitionTextNo">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" ForeColor="Black"/>
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WorkShopName" HeaderText="WorkShop" SortExpression="WorkShopName">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" ForeColor="Black"  />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReqTypeName" HeaderText="Type" SortExpression="ReqTypeName">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionEngineeringBranch" HeaderText="Branch" SortExpression="RequisitionEngineeringBranch">
                                                                <HeaderStyle HorizontalAlign="Left"   ForeColor="Black"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="Location">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Requested By" SortExpression="EmployeeName">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"/>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black" />
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="WONO" HeaderText="WO No." SortExpression="WONO">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <%--<asp:ButtonField CommandName="EditRec" HeaderStyle-HorizontalAlign="Left" HeaderText="Edit/View"
                                                                Text="Edit/View" />
                                                            <asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <%--12--%>
                                                            <%--<asp:ButtonField CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left" HeaderText="Delete"
                                                                Text="Delete" />--%>
                                                            <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                        CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                                                            <table id="T1" class="clsGridNew_Ajax" >
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandName="EditRec" ToolTip="Click to edit"
                                                                                            Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRec" ToolTip="Click to delete"
                                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                    </td>
                                                                                    <%--<td>
                                                                            <asp:ImageButton ID="LinkButton22" runat="server" CommandName="PrintRec" ImageUrl="~/images/print.png"
                                                                                CssClass="Actionbtn" />
                                                                        </td>--%>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <%--  <asp:Button ID="button" runat="server" Text="Button"  CssClass="clsButton_Ajax"/>--%>
                                                                        <%-- <asp:ImageButton ID="LinkButton3" runat="server"  ImageUrl="~/images/Arrowup.png" 
                                                                            CssClass="clsActionbtn" />--%>
                                                                        <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                            Style="cursor: pointer" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--12--%>
                                                            <asp:BoundField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemStyle CssClass="clsColorLabel" Height="5px" Width="5px" />
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:BoundField DataField="SumIssueBalQty" HeaderStyle-CssClass="hideGridColumn"
                                                                HeaderText="SumIssueBalQty" ItemStyle-CssClass="hideGridColumn" />
                                                            <%--14--%>
                                                            <asp:BoundField DataField="ReqStatus" HeaderStyle-CssClass="hideGridColumn" HeaderText="ReqStatus"
                                                                ItemStyle-CssClass="hideGridColumn" />
                                                            <%--15--%>
                                                            <asp:BoundField DataField="ReqTypeID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ReqTypeID"
                                                                ItemStyle-CssClass="hideGridColumn" />
                                                            <%--16--%>
                                                            <asp:BoundField DataField="ReqTransTypeID" HeaderText="ReqTransTypeID" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn" />
                                                            <%--17--%>
                                                            <asp:BoundField DataField="SumOfReceiptItemQty" HeaderText="SumOfReceiptItemQty"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                                            <%--18--%>
                                                            <asp:BoundField DataField="SumRequestedQty" HeaderText="SumRequestedQty" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn" />
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
                        <%--  <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Add New" ToolTip="Click to Add New Requisition" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Print" Visible="false" ToolTip="Click to Print list of Requisition" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to Close list of Requisition" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
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
    </form>
</body>
</html>

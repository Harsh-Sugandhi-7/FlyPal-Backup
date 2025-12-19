<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDiscrepancyCorrectiveActionList_AJAX.aspx.vb"
	Inherits="Flypal.DiscrepancyCorrectiveActionListPage" EnableEventValidation="false" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtlkt" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Discrepancy List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="js/query-1.7.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="frmDiscrepancyList" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table class="clstablelistout Table-MaxWidth" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table id="tblInner" class="clstablelistin" width="100%">
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td class="clsFormHeader1Newstyle">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"
                                                                            Text="Discrepancy List" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="right">
                                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="btnAddNew" runat="server" 
																			CssClass="clsbtnH clsinfoH" 
																			ToolTip="Add New Discrepancy"
                                                                            Text="Add New" CausesValidation="False" />
                                                                        <asp:Button ID="btnClose" runat="server" 
																			CssClass="clsbtnH clsinfoH" 
																			ToolTip="Close Discrepancy List Screen"
                                                                            Text="Close" CausesValidation="False" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td id="tdFavICN" align="center">
                                                    <span id="spFavICN">
                                                        <i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
                                                            class="fa fa-star fa-spin fa-5x circle-icon"></i>
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="Validationsummary2" 
											runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" 
											ValidationGroup="a" />
                                        <asp:RequiredFieldValidator ID="rfvFromDate" 
											runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="From Date Required" 
											ControlToValidate="txtFromDate" Display="None"
                                            ValidationGroup="a" />
                                        <asp:RequiredFieldValidator ID="rfvToDate" 
											runat="server" ControlToValidate="txtToDate"
                                            CssClass="clsLabelAuto" Display="None"
											ErrorMessage="To Date Required" ValidationGroup="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server"
                                                                CssClass="clsLabelAuto" Text="Aircraft" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAircraft" runat="server"
                                                                CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataValueField="ID" DataTextField="RegNo"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDiscrepancyNo" runat="server" 
																CssClass="clsLabelAuto"
                                                                Text="Discrepancy No." />
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlDiscrepancyNo" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtText" runat="server"
                                                                                    CssClass="clsTextBoxTagSearch" Width="140px"
                                                                                    ToolTip="Enter Text" />
                                                                                <ajaxtlkt:AutoCompleteExtender
                                                                                    ID="txtText1_Autocomplete"
                                                                                    runat="server"
                                                                                    TargetControlID="txtText"
                                                                                    ServiceMethod="GetTextList"
                                                                                    ServicePath="wfDiscrepancyCorrectiveActionList_AJAX.aspx"
                                                                                    MinimumPrefixLength="0"
                                                                                    CompletionSetCount="10"
                                                                                    CompletionListCssClass="ac_results_Main"
                                                                                    CompletionListItemCssClass="ac_results_li"
                                                                                    CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                    OnClientPopulated="ClientPopulated"
                                                                                    OnClientPopulating="ClientPopulating"
                                                                                    OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                    OnClientShowing="ClientShowing" />
                                                                                <asp:TextBox ID="txtNo" runat="server"
																					CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
																					ToolTip="Enter Number" Width="30px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" 
                                                                CssClass="clsLabelAuto" Text="From Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFromDate" 
                                                                CssClass="clsTextBoxTagSearchDate"
                                                                Width="100px" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                autocomplete="off" />
                                                            <ajaxtlkt:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"
                                                                CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtFromDate" />
                                                            <ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtFromDate"
                                                                ID="FromDate_watermarkextender" ClientIDMode="Static" runat="server"
                                                                WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox" />
                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto"
                                                                Display="None" ClientValidationFunction="BetweenDatesValidation"
                                                                ValidationGroup="a"
                                                                ErrorMessage="From Date should not be greater than To Date " />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" 
                                                                CssClass="clsLabelAuto" Text="To Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                Width="100px" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                autocomplete="off" />
                                                            <ajaxtlkt:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server"
                                                                CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="txtToDate" />
                                                            <ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtToDate"
                                                                ID="ToDate_watermarkextender" ClientIDMode="Static" runat="server"
                                                                WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" />
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="btnFindNow" runat="server" 
											ImageUrl="~/images/Search2.png"
                                            ToolTip="Search Discrepancies as per searching criteria"
                                            ValidationGroup="1" CausesValidation="false" 
											class="clsSearch2btn" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
                                                    <div>
                                                        <div id="divCollapsiblePnl">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label id="lblMastersSelection" runat="server"
																			CssClass="clsLabelHeader" Text="Advance Search" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <div id="divCollapsiblePnlImg">
                                                                            <image id="imgMasters" src="images/collapse_blue.jpg"
                                                                                alternatetext="(Show Details...)" />
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
                                        <asp:UpdatePanel runat="server" ID="upnlAvanceSearchContent" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlAdvancedSearchContent" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAssembly" runat="server" 
																	CssClass="clsLabelAuto" Text="Assembly" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAssembly" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataTextField="ModelSerialNoPostion"
                                                                    DataValueField="AssemblyStatusID"
                                                                    Width="225px" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblATAChapter" runat="server"
																	CssClass="clsLabelAuto" Text="ATA Chapter" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbATAChapter" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataValueField="ID" DataTextField="ATAChapter"
                                                                    Width="210px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblIncidentType"
																	CssClass="clsLabelAuto" Text="Incident Type" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbIncidentType" runat="server" 
                                                                    CssClass="clsTextBoxTagSearchComboNewstyle" 
																	DataValueField="ID" DataTextField="Name" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblStatus" runat="server"
																	CssClass="clsLabelAuto" Text="Status" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbStatus" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboSmall1">
                                                                    <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                    <asp:ListItem Value="3">Open</asp:ListItem>
                                                                    <asp:ListItem Value="2">Deferred</asp:ListItem>
                                                                    <asp:ListItem Value="1">Closed</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblMELOrDeviation" class="clsLabelAuto"
																	runat="server" Text="MEL / Deviation" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbMELSnag" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboSmall1">
                                                                    <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                    <asp:ListItem Value="1">MEL</asp:ListItem>
                                                                    <asp:ListItem Value="2">Deviation</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>                                                           
                                                            <td>
                                                                <asp:Label runat="server" ID="lblDefectType" 
																	CssClass="clsLabelAuto" Text="Reported As" /> 
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbDefectType" runat="server"
                                                                    CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                    <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Pireps</asp:ListItem>
                                                                    <asp:ListItem Value="2">Maintenance Defect</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <ajaxtlkt:CollapsiblePanelExtender BehaviorID="clpBehaviour" ID="clpextAdvancedSearch"
                                                    ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearchContent"
                                                    ExpandControlID="pnlAdvancedSearch" CollapseControlID="pnlAdvancedSearch"
                                                    Collapsed="True" ImageControlID="imgMasters"
                                                    CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                    ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                    SuppressPostBack="false" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 43px">
                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="spnImportFromCRS" title="Import Discrepancies(s)"
                                                        runat="server">
                                                        <i id="iImportFromCRS" runat="server"
                                                            onclick="ImportFromCRS(this)"
                                                            style="font-size: 18px; color: white; border: black; cursor: pointer"
                                                            class="fa fa-refresh fa-spin fa-5x circle-iconGreen">
                                                        </i>
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlShowEntriesDDL" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblShowEntriesDDL" runat="server" Text="Show Entries" />
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall"
                                                                ID="ddlShowEntries" runat="server" Width="55px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ShowEntriesChanged">
                                                                <asp:ListItem Value="0">5</asp:ListItem>
                                                                <asp:ListItem Value="1">10</asp:ListItem>
                                                                <asp:ListItem Value="2">15</asp:ListItem>
                                                                <asp:ListItem Value="3">20</asp:ListItem>
                                                                <asp:ListItem Value="4" Selected="True">25</asp:ListItem>
                                                                <asp:ListItem Value="5">30</asp:ListItem>
                                                                <asp:ListItem Value="6">40</asp:ListItem>
                                                                <asp:ListItem Value="7">45</asp:ListItem>
                                                                <asp:ListItem Value="8">50</asp:ListItem>
                                                                <asp:ListItem Value="9">55</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div>
                                                    <asp:GridView ID="dgDiscrepancyCorrectiveActionList" runat="server" DataKeyNames="ID"
                                                        ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                        AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle"
                                                        GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
                                                            ForeColor="black" HorizontalAlign="Left" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First"
                                                            LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black"
                                                            HorizontalAlign="Right" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="DefectNo" 
																SortExpression="DefectReportNo"
                                                                HeaderText="Discrepancy No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="DateOfOccurrenceFormatted" 
                                                                HeaderText="Date Of Occurrence">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="LogNoPageNo" 
																SortExpression="LogNo" HeaderText="Log No."
                                                                HtmlEncode="False">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="ATACodeSubATACode"
																SortExpression="ATACodeSubATACode"
                                                                HeaderText="ATA">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="MELOrCDLTag" 
																SortExpression="MELOrCDLTag"
                                                                HeaderText="Category">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="Defect" SortExpression="Defect"
                                                                HeaderText="Discrepancy" HtmlEncode="False">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="InvestigationStatusDiscrepancyText"
                                                                SortExpression="InvestigationStatusDiscrepancyText"
                                                                HeaderText="Status">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:BoundField DataField="ItemSequenceNo" 
																SortExpression="ItemSequenceNo"
                                                                HeaderText="Item Sequence No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" Font-Bold="true" />
                                                            </asp:BoundField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="NextDue" 
																HeaderText="Due" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="RectifiedDateFormatted" 
																HeaderText="Close Date" Visible="False">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="RectifiedLogText" 
																SortExpression="RectifiedLogText"
                                                                HeaderText="Rectified Log No."
																HtmlEncode="False" Visible="False">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <%--12--%>
                                                            <asp:BoundField DataField="PreventionTaken" 
																HeaderText="Watchlist Instruction"
                                                                Visible="False">
                                                                <ItemStyle Wrap="True" />
                                                            </asp:BoundField>
                                                            <%--13--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" 
																ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
                                                                            CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" 
																							CssClass="actionICNS"
                                                                                            runat="server" CausesValidation="false"
                                                                                            CommandArgument='<%# Eval("ID") %>'
                                                                                            ToolTip="Edit this record."
                                                                                            CommandName="EditRec" 
																							ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="deleteICN"
                                                                                            CssClass="actionICNS  largerActionICNS"
                                                                                            runat="server" ToolTip="Delete this record."
                                                                                            CommandArgument='<%# Eval("ID") %>'
                                                                                            CommandName="DeleteRec" 
																							ImageUrl="~/images/delete.png"
                                                                                            CausesValidation="false" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="printICN"
                                                                                            CssClass="hideGridColumn" runat="server"
                                                                                            CommandArgument='<%# Eval("ID") %>'
                                                                                            ToolTip="Print the Crystal Report" 
																							Visible="false"
                                                                                            CommandName="PrintRec" 
																							ImageUrl="~/images/print.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="viewICN"
																							CssClass="FileAttachmentICN"
																							runat="server"
																							ToolTip="View the Attachment Added."
																							CommandArgument='<%# Eval("ID") %>'
																							CommandName="AttachRec"
																							ImageUrl="icons/CLIP01.ICO"
																							CausesValidation="false"
																							Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--14--%>
                                                            <asp:TemplateField Visible="False" HeaderText="Troubleshooting"
                                                                ItemStyle-Wrap="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" 
																		CausesValidation="false"
                                                                        Text='<%#Eval("TroubleShootWithCount")%>'
                                                                        CommandArgument='<%# Eval("ID") %>' 
																		CommandName="TroubleShootRec">		
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--15--%>
                                                            <asp:TemplateField Visible="False" HeaderText="View Details"
                                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="True" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetails" 
																		runat="server" 
																		CausesValidation="false"
                                                                        Text='View Details'
                                                                        CommandArgument='<%# Eval("ID") %>' 
																		CommandName="ViewDetails">		
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--16--%>
                                                            <asp:TemplateField Visible="False" HeaderText="View Troubleshooting"
                                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="True" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewTroubleshoot"
																		runat="server" 
                                                                        CausesValidation="false" 
																		Text='Troubleshooting'
                                                                        CommandArgument='<%# Eval("ID") %>'
																		CommandName="ViewTroubleshoot">		
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--17--%>
                                                            <asp:TemplateField Visible="False" HeaderText="Add To Inspection"
                                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="True" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAddToInspection"
																		runat="server"
																		CausesValidation="false"
																		Text='Add To Inspection'
																		CommandArgument='<%# Eval("ID") %>'
																		CommandName="AddToInspection" >
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--18--%>
                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" 
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                                            <%--19--%>
                                                            <asp:BoundField DataField="TotalTroubleShootCount" 
																HeaderText="TotalTroubleShootCount" 
                                                                HeaderStyle-CssClass="hideGridColumn" 
																ItemStyle-CssClass="hideGridColumn" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static"
                                                                runat="server" Text="----" CausesValidation="False"
                                                                Style="display: none;" />
                                                            <asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static"
                                                                runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;" />
                                                            <asp:Button ID="hdnBtnImportCRSLogs" ClientIDMode="Static"
                                                                runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlHiddenButtons">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnDiscrepancyTroubleShoot" ClientIDMode="Static" runat="server"
                                                    CausesValidation="False" Style="display: none;" />
                                                <asp:Button ID="hdnBtnModelInspMaster" ClientIDMode="Static" runat="server"
                                                    CausesValidation="false" Style="display: none;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <div id="divSpinner">

                <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                    <ProgressTemplate>
                        <div class="clsAjaxLoader">
                        </div>
                        <div class="divAjaxLoader">
                            <div class="ext-el-mask-msg x-mask-loading">
                                <div class="clsLoad_ajax">
                                    <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                        ImageAlign="Middle" CssClass="ajax-loader-gif" />
                                </div>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </div>

            <div>

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
                        var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

                <script type="text/javascript">

					function fnMarkFavouriteUnFavourite(x) {

						if (x.classList.contains("fa-star")) {
                            x.classList.remove("fa-star");
                            x.classList.add("fa-star-o");
                            x.style.color = 'black';
                            x.style.border = 'black';
                            $("#hdnBtnRemoveFavourite").click();
						}
                        else {
                            x.classList.remove("fa-star-o");
                            x.classList.add("fa-star");
                            x.style.color = '#fff';
                            x.style.border = 'black';
                            $("#hdnBtnMarkFavourite").click();
						}

                    }
					function MarkAsFavourite() {

                        var redstar = document.getElementById("<%=favICN.ClientID%>");
                        redstar.classList.add("fa-star");
                        redstar.classList.remove("fa-star-o");
                        redstar.style.color = '#fff';
                        redstar.style.border = 'black';

					}

                    function RemoveFromFavourite() {

						var redstar = document.getElementById("<%=favICN.ClientID%>");
                        redstar.classList.add("fa-star-o");
                        redstar.classList.remove("fa-star");
						redstar.style.border = 'black';

					}

                </script>

            </div>

        </div>

        <div>

			<!-- Discrepancy Detail Popup Window Added By Prashant 12-Mar-2024-->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyDiscrepancyDetail" Text="Discrepancy Detail" ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlDiscrepancyDetail" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">
					<iframe id="IframeDiscrepancyDetail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
						allowtransparency="true" scrolling="auto"></iframe>
				</asp:Panel>
				<ajaxtlkt:ModalPopupExtender ID="mdlPopupDiscrepancyDetail" runat="server" TargetControlID="btnDummyDiscrepancyDetail"
					PopupControlID="pnlDiscrepancyDetail" BackgroundCssClass="clsModalPopupBG">
				</ajaxtlkt:ModalPopupExtender>
				<script type="text/javascript">

					function IFrameDiscrepancyDetailComplete() {

						$("#btnDummyDiscrepancyDetail").click();
						$get("AjaxLoader").style.visibility = 'hidden';

					}

					function OpenDiscrepancyDetailWindow() {

						try {

							var TransTypeID = "<%=CInt(Session("TransTypeID"))%>";

							console.log("TransTypeID = " + TransTypeID);

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeDiscrepancyDetail").attr("src", "wfDiscrepancyCorrectiveAction.aspx?Type=pup&OpenFromWatchDiscrepanciesLink=WatchDiscrepanciesLink?&TransTypeID=" + TransTypeID);

							if (!$.browser.msie) {
								$("#btnDummyDiscrepancyDetail").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}

							return false;

						} catch (e) {
							console.error("Error ocuured while opening the detail page. Refer the Error " + e);
							alert(e);
						}

					}

					function ParentCallBackFunctionForDiscrepancyDetail() {

						var DiscrepancyDetailwindow = $find("<%=mdlPopupDiscrepancyDetail.ClientID %>");

						DiscrepancyDetailwindow.hide();
						$("#IframeDiscrepancyDetail").attr("src", "JavaScript:''");
						$("#hdnBtnDiscrepancyDetail").click();

					}

				</script>

			</div>
			<!-- End-->

			<!-- Discrepancy TroubleShoot Popup Window -->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyDiscrepancyTroubleShoot" 
						Text="Discrepancy TroubleShoot" ClientIDMode="Static" />
				</div>

				<asp:Panel runat="server" ID="pnlDiscrepancyTroubleShoot" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">

					<iframe id="IframeDiscrepancyTroubleShoot" frameborder="0" 
						height="100%" width="100%" src="JavaScript:''"
						allowtransparency="true" scrolling="auto"></iframe>

				</asp:Panel>

				<ajaxtlkt:ModalPopupExtender ID="mdlPopupDiscrepancyTroubleShoot" 
					runat="server" TargetControlID="btnDummyDiscrepancyTroubleShoot"
					PopupControlID="pnlDiscrepancyTroubleShoot" BackgroundCssClass="clsModalPopupBG">
				</ajaxtlkt:ModalPopupExtender>

				<script type="text/javascript">

					function IframeDiscrepancyTroubleShootViewStateComplete() {

						$("#btnDummyDiscrepancyTroubleShoot").click();
						$get("AjaxLoader").style.visibility = 'hidden';

					}

					function OpenDiscrepancyTroubleShootWindow() {

						try {

							var TransTypeID = "<%= Request.QueryString("TransTypeID") %>";

							console.log("TransTypeID = " + TransTypeID);

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeDiscrepancyTroubleShoot").attr("src", "wfDiscrepancyTroubleShoot.aspx?Type=pup&TransTypeID=" + TransTypeID);

							if (!$.browser.msie) {
								$("#btnDummyDiscrepancyTroubleShoot").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}

							return false;

						} catch (e) {
							console.error("Error ocuured while opening TroubleShoot Window. Refer the Error " + e);
							alert(e);
						}

					}

					function ParentCallBackFunctionForDiscrepancyTroubleShoot() {

						try {

							var DiscrepancyTroubleShootwindow = $find("<%=mdlPopupDiscrepancyTroubleShoot.ClientID %>");

							DiscrepancyTroubleShootwindow.hide();
							$("#IframeDiscrepancyTroubleShoot").attr("src", "JavaScript:''");
							$("#hdnBtnDiscrepancyTroubleShoot").click();

						} catch (e) {
							console.error("Error ocuured in ParentCallBackFunctionForDiscrepancyTroubleShoot(). Refer the Error " + e);
							alert(e);
						}

					}

				</script>

			</div>
			<!-- End-->

			<!-- Discrepancy TroubleshootView Popup Window Added By Prashant 12-Mar-2024 -->
			<div>

				<div style="display: none">

					<asp:Button runat="server" ID="btnDummyDiscrepancyTroubleshootView"
						Text="DiscrepancyTroubleshootView" ClientIDMode="Static" />

				</div>

				<asp:Panel runat="server" ID="pnlDiscrepancyTroubleshootView"
					ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">

					<iframe id="IframeDiscrepancyTroubleshootView" frameborder="0"
						height="100%" width="100%" src="JavaScript:''"
						allowtransparency="true" scrolling="auto"></iframe>

				</asp:Panel>
				<ajaxtlkt:ModalPopupExtender ID="mdlPopupDiscrepancyTroubleshootView" runat="server"
					TargetControlID="btnDummyDiscrepancyTroubleshootView"
					PopupControlID="pnlDiscrepancyTroubleshootView"
					BackgroundCssClass="clsModalPopupBG">
				</ajaxtlkt:ModalPopupExtender>

				<script type="text/javascript">

					function IframeDiscrepancyTroubleshootViewStateComplete() {

						$("#btnDummyDiscrepancyTroubleshootView").click();
						$get("AjaxLoader").style.visibility = 'hidden';

					}

					function OpenDiscrepancyTroubleshootView() {

						try {

							var TransTypeID = "<%= Request.QueryString("TransTypeID") %>";

							console.log("TransTypeID = " + TransTypeID);

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeDiscrepancyTroubleshootView").attr("src", "wfDiscrepancyTroubleShootView_Ajax.aspx?Type=pup&TransTypeID=" + TransTypeID);

							if (!$.browser.msie) {
								$("#btnDummyDiscrepancyTroubleshootView").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}

							return false;

						} catch (e) {
							console.error("Error ocuured while opening TroubleShoot Window. Refer the Error " + e);
							alert(e);
						}

					}

					function ParentCallBackFunctionForDiscrepancyTroubleshootView() {

						try {

							var DiscrepancyTroubleshootViewWindow = $find("<%=mdlPopupDiscrepancyTroubleshootView.ClientID %>");

							DiscrepancyTroubleshootViewWindow.hide();
							$("#IframeDiscrepancyTroubleshootView").attr("src", "JavaScript:''");
							$("#hdnBtnDiscrepancyTroubleshootView").click();

						} catch (e) {
							console.error("Error ocuured in ParentCallBackFunctionForDiscrepancyTroubleshootView(). Refer the Error " + e);
							alert(e);
						}

					}

					function ImportFromCRS(x) {
						$("#hdnBtnImportCRSLogs").click();
					}

				</script>

			</div>
			<!-- End-->

			<!--Model Insp Master Popup Window Added By Prashant 4-Mar-2024-->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyModelInspMaster" Text="Model Insp Master"
						ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlModelInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">
					<iframe id="IframeModelInspMaster" frameborder="0" height="100%" allowtransparency="true"
						width="100%" src="JavaScript:''" scrolling="auto"></iframe>
				</asp:Panel>
				<ajaxtlkt:ModalPopupExtender ID="mdlPopupModelInspMaster" runat="server" TargetControlID="btnDummyModelInspMaster"
					PopupControlID="pnlModelInspMaster" BackgroundCssClass="clsModalPopupBG">
				</ajaxtlkt:ModalPopupExtender>
				<script type="text/javascript">
					function IFrameModelInspMasterStateComplete() {
						$("#btnDummyModelInspMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					function OpenModelInspMasterWindow() {
						try {

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeModelInspMaster").attr("src", "wfModelMonitorInspection_Ajax.aspx?Type=pup&GChildPage2=Index.aspx");

							if (!$.browser.msie) {
								$("#btnDummyModelInspMaster").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}


							//});


							return false;
						} catch (e) {
							alert(e);
						}

					}
					function ParentCallBackFunctionForModelInspMaster() {
						var ModelInspMasterwindow = $find("<%=mdlPopupModelInspMaster.ClientID %>");
						//close Model Insp Master popup window
						ModelInspMasterwindow.hide();
						//           release resources
						$("#IframeModelInspMaster").attr("src", "JavaScript:''");
						//call Model Insp Master image button
						$("#hdnBtnModelInspMaster").click();
					}
				</script>

			</div>
			<!-- End-->

        </div>

    </form>

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

</body>
</html>

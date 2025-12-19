<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPurchaseOrderList_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfPurchaseOrderList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Purchase Order List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
		function openFilel() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfExportToExcel.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
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
		<script type="text/javascript">
			window.onload = blinknow;
			function blinknow() {
				var e = document.getElementById("<%=imgNewLink.ClientID%>");

				e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
				setTimeout("blinknow();", 1200);
			}

		</script>
		<div>
			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<asp:PlaceHolder runat="server" ID="PlaceHolder11" Visible='<%#IIf(AppSettings("NewUi") = "True" And OrderType <> 2, True, False) %>'>
									<tr>
										<td align="right">
											<table>
												<tr>
													<td>
														<asp:Button ID="btnNewUi" runat="server" CssClass="clsbtnH clsinfoH1"
															Text="Check Out New Application" CausesValidation="False"></asp:Button>
													</td>
													<td>
														<asp:Image ID="imgNewLink" runat="server" ImageUrl="~/images/new.png" Height="45px" />
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td class="clsFormHeader1Newstyle">
										<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<table>
																<tr>
																	<td style="width: 99%" valign="middle">
																		<asp:Label ID="lblPurchaseOrderList" runat="server" CssClass="clsFormHeader">List of Purchase Orders</asp:Label>
																	</td>
																	<td></td>
																	<td>
																		<asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Purchase Order"
																			Text="Add New" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Purchase Order List"
																			Text="Print" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnExportTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Export to Excel"
																			ToolTip="Click to Export report" Width="100px" CausesValidation="False" Visible='<%# iif(AppSettings("ClientCode") = "CE" and AppSettings("ShowExportToExcelButton") = "True",True,False) %>' />
																	</td>
																	<td>
																		<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Purchase Order screen."
																			Text="Close" CausesValidation="False"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
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
																					<span id="Span8" class="clsLabelAuto">Range</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True">
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
																					<asp:Label ID="lblFromDate" CssClass="clsLabelAuto" runat="server">From Date </asp:Label>
																				</td>
																				<td>
																					<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
																						onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
																					<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																						Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																						ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																				</td>
																				<td>
																					<asp:Label ID="lblToDate" CssClass="clsLabelAuto" runat="server">To Date </asp:Label>
																				</td>
																				<td colspan="3">
																					<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
																						onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
																					<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
																						Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																						ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																				</td>
																				<td>
																					<span id="Span1" class="clsLabelAuto">Order No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True" DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
																						Width="55px"></asp:TextBox>
																				</td>
																				<td>
																					<asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="1"
																						Width="55px"></asp:TextBox>
																				</td>
																				<td></td>
																			</tr>
																		</table>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right" valign="top">
															<asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:ImageButton ID="btnFindNow" runat="server"
																		ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
																		ToolTip="Click to find list of Order as per searching criteria." />
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td valign="top" colspan="2">
															<asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
																		<div>
																			<div id="divCollapsiblePnl">
																				<table width="100%">
																					<tr>
																						<td>
																							<span id="lblMastersSelection" class="clsLabelHeader">Advance Search
																							</span>
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
															<asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnFindNow" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																		<table width="100%">
																			<tr>
																				<td>
																					<span id="Span3" class="clsLabelAuto">Int. Order #</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtInternalOrderNo" runat="server" CssClass="clsTextBoxTagSearch"
																						MaxLength="100"></asp:TextBox>
																				</td>
																				<td>
																					<span id="Span7" class="clsLabelAuto">Status</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboSmall">
																						<asp:ListItem Value="0">(All)</asp:ListItem>
																						<asp:ListItem Value="1">Opened</asp:ListItem>
																						<asp:ListItem Value="2">Authorized</asp:ListItem>
																						<asp:ListItem Value="4">Cancelled</asp:ListItem>
																					</asp:DropDownList>
																				</td>
																				<td></td>
																				<td>
																					<span id="Span6" class="clsLabelAuto" style="width: 100%">Priority</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboSmall">
																						<asp:ListItem Value="0">None</asp:ListItem>
																						<asp:ListItem Value="1">Low</asp:ListItem>
																						<asp:ListItem Value="2">Medium</asp:ListItem>
																						<asp:ListItem Value="3">High</asp:ListItem>
																						<asp:ListItem Value="4">AOG</asp:ListItem>
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:Label ID="lblForAircraftSearch" CssClass="clsLabelAuto" runat="server">Aircraft</asp:Label>
																				</td>
																				<td>
																					<asp:TextBox ID="txtForAircraftSearch" runat="server" CssClass="clsTextBoxTagSearch"
																						MaxLength="100">
																					</asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="lblQuotationNo" class="clsLabelAuto">Quotation #</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtQuotationNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"></asp:TextBox>
																				</td>
																				<td>
																					<span id="lblRequisitionNo" class="clsLabelAuto" runat="server" visible="false">Requ. #</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True" DataTextField="Text" DataValueField="Text" Visible="false">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtRequisitionNo" runat="server" CssClass="clsTextBoxTagSearch"
																						MaxLength="6" Visible="false" Width="55px"></asp:TextBox>
																				</td>
																				<td>
																					<span id="lblOrderTypeSearch" class="clsLabelAuto" runat="server" visible="false">Order Type</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbSearchOrderType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True" Visible="<%# OrderType <> 2 %>">
																						<asp:ListItem Value="0" Selected="True">(All)</asp:ListItem>
																						<asp:ListItem Value="5">New Purchase</asp:ListItem>
																						<asp:ListItem Value="31">Exchange</asp:ListItem>
																						<asp:ListItem Value="38">OverHaul</asp:ListItem>
																						<asp:ListItem Value="100">Repair</asp:ListItem>
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:Label ID="lblPOTowardsSearch" CssClass="clsLabelAuto" runat="server" Visible="false">PO Towards </asp:Label>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbPOTowards" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						DataTextField="Name" DataValueField="ID" Visible="false">
																					</asp:DropDownList>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="lblPartNoSearch" class="clsLabelAuto">Part #</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																				<td>
																					<asp:Label ID="lblSupplier" CssClass="clsLabelAuto" runat="server">Supplier</asp:Label>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtSupplier" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																				<td>
																					<span id="Span2" class="clsLabelAuto">PBH Purchase</span>&nbsp;
																				</td>
																				<td>
																					<asp:CheckBox ID="chkIsPBHPurchase" runat="server" CssClass="clsLabelAuto" TextAlign="Right" />
																				</td>
																				<td></td>
																				<td></td>
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
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="left">
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">As per criteria : Record(s) found</asp:Label>
														</td>
														<td align="right">
															<table>
																<tr>
																	<td>
																		<asp:Label ID="lblCreatePurchaseOrderfor" CssClass="clsLabelAuto" runat="server"
																			Visible="<%# OrderType <> 2 %>">Create Purchase Order for</asp:Label>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbOrderType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																			AutoPostBack="True" Visible="<%# OrderType <> 2 %>">
																			<asp:ListItem Value="5" Selected="True">New Purchase</asp:ListItem>
																			<asp:ListItem Value="31">Exchange</asp:ListItem>
																			<asp:ListItem Value="38">OverHaul</asp:ListItem>
																			<asp:ListItem Value="100">Repair</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																	<td>
																		<asp:Label ID="lblAgainst" CssClass="clsLabelAuto" runat="server">Against</asp:Label>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbPOAgainstType" runat="server" CssClass="clsTextBoxTagSearchComboSmall">
																		</asp:DropDownList>
																	</td>
																	<td>
																		<span id="lblFor" class="clsLabelAuto">For</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbFor" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																			Width="100px">
																			<asp:ListItem Value="1" Selected="True">Self</asp:ListItem>
																			<asp:ListItem Value="2">Customer</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
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
															<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td align="left">
																				<asp:Label ID="lblYellowGreen" runat="server" CssClass="clsColorLabel"
																					BackColor="YellowGreen" ForeColor="YellowGreen"
																					Height="18px" Width="18px"> 
																				</asp:Label>
																				<asp:Label ID="lblYellowGreenInfo" runat="server" CssClass="clsLabelauto">
																					Pending / Partial Order
																				</asp:Label>
																				<asp:Label ID="lblGreen" runat="server" CssClass="clsColorLabel"
																					BackColor="Green" ForeColor="Green"
																					Height="18px" Width="18px"> 
																				</asp:Label>
																				<asp:Label ID="lblGreenInfo" runat="server" CssClass="clsLabelauto">
																					Completed Order
																				</asp:Label>
																			</td>
																			<td>
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
															<asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AllowSorting="True"
																DataKeyNames="ID" AutoGenerateColumns="False" CssClass="clsGridNewStyle" PageSize="25"
																ShowHeaderWhenEmpty="True" OnRowDataBound="OnRowDataBound" GridLines="Horizontal"
																CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<%--0--%>
																	<asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--1--%>
																	<asp:BoundField DataField="OrderDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" />
																	</asp:BoundField>
																	<%--2--%>
																	<asp:BoundField DataField="OrderNo" HeaderText="Number" SortExpression="OrderNo">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" />
																	</asp:BoundField>
																	<%--3--%>
																	<asp:BoundField DataField="IntOrderNo" HeaderText="Int. Order No." SortExpression="IntOrderNo">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--4--%>
																	<asp:BoundField DataField="OrderType" HeaderText="Type" SortExpression="OrderType">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--5--%>
																	<asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" />
																	</asp:BoundField>
																	<%--6--%>
																	<asp:BoundField DataField="KindAttn" HeaderText="Kind Attn." SortExpression="KindAttn"
																		Visible="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--7--%>
																	<asp:BoundField DataField="QuotationInfo" HeaderText="Quotation Info." HtmlEncode="false"
																		Visible="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--8--%>
																	<asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total" SortExpression="CGrandTotal">
																		<HeaderStyle HorizontalAlign="Right" Wrap="false" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<%--9--%>
																	<asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--10--%>
																	<asp:BoundField DataField="DeliveryWithinDays" HeaderText="Delivery in Days" SortExpression="DeliveryWithinDays">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<%-- Sankalp for 7AR only --%>
																	<%--11--%>
																	<asp:BoundField DataField="OrderDueDateFormatted" HeaderText="Due Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" />
																	</asp:BoundField>

																	<%--12--%>
																	<asp:BoundField DataField="AircraftReg" HeaderText="AC Tail" SortExpression="AircraftReg">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--13--%>
																	<asp:BoundField DataField="POTowards" HeaderText="PO. Towards" SortExpression="POTowards">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--14--%>
																	<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--15--%>
																	<asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
																		<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--16--%>
																	<asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
																		<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--17--%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																		<ItemTemplate>
																			<div class="dropdown">
																				<div class="dropdownbtn-content">
																					<table id="T1" class="clsGridNew_Ajax" dir="ltr">
																						<tr>
																							<td>
																								<asp:ImageButton ID="EditView" runat="server"
																									CommandArgument='<%# Eval("ID") %>' CommandName="EditView"
																									class="actionICNS" ToolTip="Click to Edit record."
																									ImageUrl="~/images/edit.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="DeleteRecord" runat="server"
																									CommandArgument='<%# Eval("ID") %>'
																									CommandName="DeleteRecord"
																									class="actionICNS  largerActionICNS"
																									ToolTip="Click to Delete record."
																									ImageUrl="~/images/delete.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="View" runat="server"
																									CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
																									CssClass="FileAttachmentICN"
																									ToolTip="Click to View Attachment(s)."
																									ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
																	<%--18--%>
																	<asp:BoundField DataField="TransID" HeaderStyle-CssClass="hideGridColumn" HeaderText="TransTypeID"
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--19--%>
																	<asp:BoundField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
																		<ItemStyle CssClass="clsColorLabel" Height="3px" Width="3px" HorizontalAlign="Center"
																			VerticalAlign="Middle" />
																	</asp:BoundField>
																	<%--20--%>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
																	<%--21--%>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="SumReceiptBalanceQty" HeaderText="Sum of Receipt Balance Qty"></asp:BoundField>
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

		</div>
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

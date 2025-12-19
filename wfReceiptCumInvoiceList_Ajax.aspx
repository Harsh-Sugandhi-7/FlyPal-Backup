<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceiptCumInvoiceList_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfReceiptCumInvoiceList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Goods Receipt List</title>
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
	</script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<%-- Ajay 07-Nov-2022--%>
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" id="clientEventHandlersJS">
		function openFile() {
			str = "wfFileView.aspx";
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
								<asp:PlaceHolder runat="server" ID="PlaceHolder11"
									Visible='<%# IIf(AppSettings("NewUi") = "True", True, False) %>'>
									<tr>
										<td align="right">
											<table>
												<tr>
													<td>
														<asp:Button ID="btnCheckoutNewApplication" runat="server"
															CssClass="clsbtnH clsinfoH1"
															Text="Check Out New Application" CausesValidation="False" />
													</td>
													<td>
														<asp:Image ID="imgCheckoutNewApplication" runat="server"
															ImageUrl="~/images/new.png"
															Height="45px" />
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td class="clsFormHeader1Newstyle">
															<table>
																<tr>
																	<td style="width: 99%" valign="middle">
																		<asp:Label ID="lblList" runat="server" CssClass="clsFormHeader" Style="width: 100%">List of Goods Receipt</asp:Label>
																	</td>
																	<td>
																		<asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Goods Receipt"
																			Text="Add New" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print List of Goods Receipt"
																			Text="Print" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Goods Receipt screen"
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
																					<span id="Span8" class="clsLabel">Range</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True">
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
																					<span id="Span1" class="clsLabel">Receipt No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True" DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
																						Width="55px"></asp:TextBox>
																				</td>
																			</tr>

																		</table>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right" valign="top">
															<asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
																<ContentTemplate>
																	<%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                    ToolTip="Click to find list of Goods Receipt as per searching criteria" />--%>
																	<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Goods Receipt as per searching criteria" />
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
																				<td>
																					<span id="Span6" class="clsLabelAuto" style="width: 100%">Received From </span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbReceivedFromType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True">
																						<asp:ListItem Value="0">(All)</asp:ListItem>
																						<asp:ListItem Value="1">Supplier</asp:ListItem>
																						<asp:ListItem Value="2">Aircraft</asp:ListItem>
																						<asp:ListItem Value="3">Store</asp:ListItem>
																						<asp:ListItem Value="4">Customer</asp:ListItem>
																						<asp:ListItem Value="5">WorkShop</asp:ListItem>
																						<%--<asp:ListItem Value="6">Work Order</asp:ListItem>--%>
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="Span5" class="clsLabel">Invoice No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbInvoiceText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																						AutoPostBack="True" DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="6"
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
																				<td colspan="2">
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
																					<span id="lblDCNoSearch" class="clsLabel">D. C. No.</span>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtDCNoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																				<td>
																					<span id="lblGSENoSearch" class="clsLabel">GSE. No.</span>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtGSENoSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																				<td>
																					<span id="lblCustomBillofEntrySearch" class="clsLabelAuto" style="width: 100%">Cust.
                                                                                    Bill of Entry</span>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtCustomBillofEntrySearch" runat="server" CssClass="clsTextBoxTagSearch"
																						MaxLength="100">
																					</asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="lblInternalReceiptNoSearch" class="clsLabelAuto" style="width: 100%">Internal
                                                                                    Receipt No.</span>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtInternalReceiptNoSearch" runat="server" CssClass="clsTextBoxTagSearch"
																						MaxLength="100">
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
																				<td colspan="2">
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
									<td width="100%">
										<span id="lblInfo" class="clsLabelAuto" style="display: none">Select Goods Receipt from
                                        the List. Click on Edit Link to Modify or Delete link to Delete the selected Goods
                                        Receipt. Click on Add New button to Add a New Goods Receipt.Click On View Link to
                                        see attached File.</span>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnlResult" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="left">&nbsp;
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">As per criteria: Record(s) found.</asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:Label ID="lblRecivedFrom" CssClass="clsLabelAuto" runat="server">Received From</asp:Label>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbReceivedFrom" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																					AutoPostBack="True">
																					<asp:ListItem Value="0">Supplier</asp:ListItem>
																					<asp:ListItem Value="1">Aircraft</asp:ListItem>
																					<asp:ListItem Value="2">Store</asp:ListItem>
																					<asp:ListItem Value="3">Customer</asp:ListItem>
																					<asp:ListItem Value="4">WorkShop</asp:ListItem>
																					<asp:ListItem Value="5">Work Order</asp:ListItem>
																				</asp:DropDownList>
																			</td>
																			<td>
																				<asp:Label ID="lblAs" CssClass="clsLabelAuto" runat="server">As</asp:Label>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbReceivedAs" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																					DataValueField="ID" DataTextField="Name">
																				</asp:DropDownList>
																			</td>
																			<td align="left"></td>
																			<td align="left"></td>
																			<td align="right">
																				<%--<asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Goods Receipt"
                                                            Text="Add New" CausesValidation="False"></asp:Button>--%>
																			</td>
																			<td>
																				<%-- <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List of Goods Receipt"
                                                            Text="Print" CausesValidation="False"></asp:Button>--%>
																			</td>
																			<td align="right">
																				<%-- <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Goods Receipt screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>--%>
																			</td>
																		</tr>
																	</table>
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
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="Left">
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
															<%-- <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Receipts screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>--%>
															<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:TextBox ID="txtSearchBox" runat="server" CssClass="clsTextBoxTagSearch" placeholder="Search here"
																		AutoPostBack="true"></asp:TextBox>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td align="right" colspan="2">
															<asp:GridView ID="dgReceiptCumInvoiceList" runat="server" AllowSorting="True"
																AutoGenerateColumns="False" CssClass="clsGridNewStyle" PageSize="25" ShowHeaderWhenEmpty="True"
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
																	<%-- 0--%>
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
																	<asp:BoundField DataField="IntReceiptNo" SortExpression="IntReceiptNo" HeaderText="Internal Receipt No.">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--4--%>
																	<asp:BoundField DataField="RCIType" SortExpression="RCIType" HeaderText="From">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--5--%>
																	<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
																		<HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
																	</asp:BoundField>
																	<%--6--%>
																	<asp:BoundField DataField="VendorInvoiceNo" SortExpression="VendorInvoiceNo" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="Supplier Invoice No.">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left" CssClass="hideGridColumn"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--7--%>
																	<asp:BoundField DataField="VendorInvoiceDateFormatted" SortExpression="VendorInvoiceDateFormatted"
																		HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="Supplier Invoice Date">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left" CssClass="hideGridColumn"></HeaderStyle>
																		<ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="hideGridColumn"></ItemStyle>
																	</asp:BoundField>
																	<%--8--%>
																	<asp:BoundField DataField="DCNo" SortExpression="DCNo" HeaderText="D.C.No." HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left" CssClass="hideGridColumn"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--9--%>
																	<asp:BoundField DataField="DCDateFormatted" SortExpression="DCDateFormatted" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="D.C.Date">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left" CssClass="hideGridColumn"></HeaderStyle>
																		<ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="hideGridColumn"></ItemStyle>
																	</asp:BoundField>
																	<%--Sankalp 10 --%>
																	<asp:BoundField DataField="AWBNo" SortExpression="AWBNo" HeaderText="Cust. Bill of Entry">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--11--%>
																	<asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--12--%>
																	<asp:BoundField DataField="CGrantTotal" SortExpression="CGrantTotal" HeaderText="Grand Total">
																		<HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundField>
																	<%--13--%>
																	<asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--14--%>
																	<asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--15--%>
																	<asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
																		<HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--16--%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																		<%--17--%>
																		<ItemTemplate>
																			<div class="dropdown">
																				<div class="dropdownbtn-content">
																					<table id="T1" class="clsGridNew_Ajax">
																						<tr>
																							<td>
																								<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
																									CommandName="EditView" CssClass="actionICNS" ImageUrl="~/images/edit.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									CommandName="DeleteRecord" CssClass="largerActionICNS" ImageUrl="~/images/delete.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									CommandName="ViewRec" CssClass="FileAttachmentICN" ImageUrl="icons/CLIP01.ICO"
																									Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--18--%>
																	<asp:BoundField DataField="TransID" HeaderStyle-CssClass="hideGridColumn" HeaderText="TransTypeID"
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--19--%>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																		HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																	<%--20--%>
																</Columns>
																<%--<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />--%>
																<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
																<SortedAscendingCellStyle BackColor="#F7F7F7" />
																<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
																<SortedDescendingCellStyle BackColor="#E5E5E5" />
																<SortedDescendingHeaderStyle BackColor="#242121" />
															</asp:GridView>
														</td>
													</tr>
												</table>
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
																					<cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
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
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td></td>
														<td></td>
														<td></td>
														<td></td>
														<td align="left"></td>
														<td align="left"></td>
														<td align="right">
															<%--<asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Goods Receipt"
                                                            Text="Add New" CausesValidation="False"></asp:Button>--%>
														</td>
														<td>
															<%-- <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List of Goods Receipt"
                                                            Text="Print" CausesValidation="False"></asp:Button>--%>
														</td>
														<td align="right">
															<%--    <asp:Button ID="btnBottomClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of Goods Receipt screen"
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
	<!-- Slider control events  -->
	<script type="text/javascript">
		//initialize slider control and attach events
		function pageLoad(sender, e) {
			var slider = $find('<%=SliderExtender1.ClientID %>');
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
				var slider = $find('<%=SliderExtender1.ClientID %>');
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
				var slider = $find('<%=SliderExtender1.ClientID %>');
				slider.set_Value(curval);


			}
			else if (val === 2) {//next
				var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
				curval = curval + 1;
				$('#<%=txtPageDisplay.ClientID %>').val(curval);
				$('#<%=Slidercontrol.ClientID %>').val(curval);
				var slider = $find('<%=SliderExtender1.ClientID %>');
				slider.set_Value(curval);
				//                            sliderStart();
				//                            valChanged();
				//                            sliderEnd();

			}
			else if (val === 3) {//last
				var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
				var slider = $find('<%=SliderExtender1.ClientID %>');
				var maxval = slider.get_Maximum();
				$('#<%=txtPageDisplay.ClientID %>').val(maxval);
				$('#<%=Slidercontrol.ClientID %>').val(maxval);
				slider.set_Value(maxval);
			}
		}
	</script>
	<!-- End  -->
</body>
</html>

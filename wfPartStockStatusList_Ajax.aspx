<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartStockStatusList_Ajax.aspx.vb"
	Inherits="Flypal.wfPartStockStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Part Stock Status List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">
		.ScrollStyle {
			height: 20px;
			overflow: scroll;
		}
	</style>
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
							<table id="tblLedgerList" class="clstablelistin">
								<tr>
									<td colspan="3" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span id="lblPartStockStatusList" class="clsFormHeader">Part Stock Status List</span>
												</td>
												<td align="right">
													<asp:Button ID="btnAddNewPart" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip=" Click to add new part"
														Text="Add New Part"></asp:Button>

													<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip="Click to go back to the previous page"
														Text="Back"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblSearch" class="clsLabel">Part No.</span>
									</td>
									<td>
										<asp:TextBox ID="txtSearch" runat="server" ToolTip="Enter Part Number" CssClass="clsTextBoxTagSearch"
											MaxLength="50">
										</asp:TextBox>
									</td>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnlFindNowButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:ImageButton ID="btnFindNow" runat="server"
																ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
																ToolTip="Click to search the part" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<asp:UpdatePanel runat="server" ID="upnlPartStockStatusList" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
													Part Stock Status List : No.of Record Found(s).
												</asp:Label>

												<asp:GridView ID="dgPartStockStatusList" runat="server" CssClass="clsGridNewStyle"
													AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="10"
													GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
													<PagerSettings FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ItemId" HeaderStyle-CssClass="hideGridColumn" HeaderText="ItemId"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
															<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="ATA" HeaderText="ATA" SortExpression="ATA">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
															<HeaderStyle HorizontalAlign="Left" Wrap="False" />
															<ItemStyle HorizontalAlign="Left" Wrap="False" Font-Bold="True" />
														</asp:BoundField>
														<asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" Wrap="true" />
														</asp:BoundField>
														<asp:BoundField DataField="AlternateParts" HeaderText="Alternate Parts">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
														</asp:BoundField>
														<asp:BoundField DataField="FirstPriorityPart" HeaderText="First Priority Part">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
														</asp:BoundField>
														<asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="Applicability" HeaderText="Applicability">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="StockQty" HeaderText="Stock Qty." SortExpression="StockQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Wrap="False" Font-Bold="True" />
														</asp:BoundField>
														<asp:BoundField DataField="PendingQty" HeaderText="On Order Qty." SortExpression="PendingQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Font-Bold="True" />
														</asp:BoundField>
														<asp:BoundField DataField="EROQty" HeaderText="ERO Qty." SortExpression="EROQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Font-Bold="True" />
														</asp:BoundField>
														<asp:BoundField DataField="ReturnableQty" HeaderText="Returnable Qty." SortExpression="ReturnableQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Font-Bold="True" />
														</asp:BoundField>
														<asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitName">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
															<HeaderStyle Wrap="False" HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:ButtonField>
														<asp:ButtonField CommandName="SelectPart" HeaderText="Select Part" Text="Select Part">
															<HeaderStyle Wrap="False" HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:ButtonField>
														<asp:ButtonField CommandName="StockDetail" HeaderText="Stock Detail" Text="Stock Detail">
															<HeaderStyle Wrap="False" HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:ButtonField>
														<asp:ButtonField CommandName="LastTenPurchases" HeaderText="Last 10 Purchases" Text="Last 10 Purchases">
															<HeaderStyle Wrap="False" HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:ButtonField>
													</Columns>
													<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
													<PagerStyle CssClass="paging" HorizontalAlign="Right" />
												</asp:GridView>
												<asp:Label ID="lblEROInfo" runat="server" CssClass="clsLabelHeader">
													ERO Qty : Quantity reserve for Exchange / Repair / Overhaul
												</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<asp:UpdatePanel runat="server" ID="upnlPendingSOItemList" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblDalesOrderDetail" runat="server" CssClass="clsLabelHeader" Visible="False">Details of Pending Sales Order Item (s) </asp:Label>
												<asp:GridView ID="dgPendingSOItemList" runat="server" CssClass="clsGridNewStyle"
													AutoGenerateColumns="false" AllowSorting="true" PageSize="5" GridLines="Horizontal"
													CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
													<PagerSettings FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="FromItemID" HeaderStyle-CssClass="hideGridColumn" HeaderText="FromItemID"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
															<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="FromItemParentID" HeaderStyle-CssClass="hideGridColumn"
															HeaderText="FromItemParentID" ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
															<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="FromDateFormatted" HeaderText="Date">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="FromTextNo" HeaderText="No." SortExpression="FromTextNo">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="FromItemQty" HeaderText="Qty." SortExpression="FromItemQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Wrap="False" />
														</asp:BoundField>
														<asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:ButtonField>
													</Columns>
													<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
													<PagerStyle CssClass="paging" HorizontalAlign="Right" />
												</asp:GridView>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<asp:UpdatePanel runat="server" ID="upnlReorderLevelList" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader" Visible="False">List of Reorder Item (s)</asp:Label>
												<asp:GridView ID="dgReorderLevelList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
													AllowSorting="true" PageSize="5" GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
													<PagerSettings FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ItemId" HeaderStyle-CssClass="hideGridColumn" HeaderText="ItemId"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
															<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="ItemDescription" HeaderText="Description" SortExpression="ItemDescription">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="StockQty" HeaderText="Min Stock Level" SortExpression="StockQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="StockQty" HeaderText="Stock Qty." SortExpression="StockQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="OnOrderQty" HeaderText="OnOrderQty" SortExpression="On Order Qty.">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" />
														</asp:BoundField>
														<asp:BoundField DataField="Rate" HeaderText="Pending Qty." SortExpression="Rate">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" />
														</asp:BoundField>
														<asp:BoundField DataField="ReturnableQty" HeaderText="Rate" SortExpression="ReturnableQty">
															<HeaderStyle HorizontalAlign="Right" />
															<ItemStyle HorizontalAlign="Right" />
														</asp:BoundField>
														<asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:ButtonField>
													</Columns>
													<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
													<PagerStyle CssClass="paging" HorizontalAlign="Right" />
												</asp:GridView>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>

			<%--Last 10 Purchases--%>
			<asp:Panel runat="server" ID="pnlLast10Purchases" CssClass="clspanel1">
				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyLast10Purchases" Text="Last 10 Purchases" />
				</div>
				<div style="width: 100%">
					<asp:UpdatePanel runat="server" ID="upnlLast10Purchases" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="clstablelistin" id="tblInner">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Last 10 Purchases details for the Part No.-</asp:Label>
												</td>
												<td align="right">
													<asp:Button ID="btnLast10PurchasesClose" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip="Click to go back to the previous page" Text="Close" CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<fieldset id="lblLast10Purchases" class="clsFieldSetNewStyle" style="border-width: 1px">
											<asp:GridView ID="dgList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
												AllowSorting="true" GridLines="Horizontal" CellPadding="5">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Invoice Date">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice No.">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="OrderDateFormatted" HeaderText="Order Date">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="OrderNumber" HeaderText="Order No.">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="VendorName" HeaderText="Supplier">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="CurrencyName" HeaderText="Currency">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="ConversionFactor" HeaderText="Conv. Factor">
														<HeaderStyle HorizontalAlign="Right" Wrap="False" />
														<ItemStyle HorizontalAlign="Right" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No.">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="Qty" HeaderText="Qty.">
														<HeaderStyle HorizontalAlign="Right" Wrap="False" />
														<ItemStyle HorizontalAlign="Right" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
														<HeaderStyle HorizontalAlign="Left" Wrap="False" />
														<ItemStyle HorizontalAlign="Left" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="CRate" HeaderText="Rate">
														<HeaderStyle HorizontalAlign="Right" Wrap="False" />
														<ItemStyle HorizontalAlign="Right" Wrap="False" />
													</asp:BoundField>
													<asp:BoundField DataField="CCommercialRate" HeaderText="Commercial Rate">
														<HeaderStyle HorizontalAlign="Right" Wrap="False" />
														<ItemStyle HorizontalAlign="Right" Wrap="False" />
													</asp:BoundField>
												</Columns>
												<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
												<PagerStyle CssClass="paging" HorizontalAlign="Right" />
											</asp:GridView>
										</fieldset>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</asp:Panel>
			<cc2:ModalPopupExtender runat="server" ID="mdeLast10Purchases" TargetControlID="btnDummyLast10Purchases"
				PopupControlID="pnlLast10Purchases" BackgroundCssClass="clsModalPopupBGForSecondPage">
			</cc2:ModalPopupExtender>
			<%--End Of Last 10 Purchases--%>

			<%--Quantity Details--%>
			<asp:Panel runat="server" ID="pnlQuantityDetails" CssClass="clspanel1" Style='overflow: auto; width: auto; height: 100%;'>
				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyQuantityDetails" Text="Quantity Details" />
				</div>
				<div style="width: 100%">
					<asp:UpdatePanel runat="server" ID="upnlQuantityDetails" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="clstablelistout" id="Table1">
								<tr>
									<td>
										<asp:Panel ID="Panel1" CssClass="clspanel1" runat="server">
											<table id="TABLE2" class="clstablelistin">
												<tr>
													<td class="clsFormHeader1Newstyle">
														<table width="100%">
															<tr>
																<td>
																	<asp:Label ID="lblQuantityDetails" runat="server" CssClass="clsFormHeader">Quantity Details For Part No. -</asp:Label>
																</td>
																<td align="right">
																	<asp:Button ID="btnlQuantityDetailsClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to go back to the previous page" Text="Close" CausesValidation="False"></asp:Button>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<fieldset id="lblStockDetails" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend><b>Stock Details :</b></legend>
															<asp:GridView ID="dgStock" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
																ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="TransDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="No" HeaderText="Receipt No.">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FromWhom" HeaderText="Receive From">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ReleaseNoteNo" HeaderText="Rele. Note No.">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="PartStatus" HeaderText="Part Status">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ReceiptBalQty" HeaderText="Qty.">
																		<HeaderStyle HorizontalAlign="Right" Wrap="False" />
																		<ItemStyle HorizontalAlign="Right" Wrap="False" />
																	</asp:BoundField>
																</Columns>
																<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																<PagerStyle CssClass="paging" HorizontalAlign="Right" />
															</asp:GridView>
														</fieldset>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td>
														<fieldset id="lblPendingOutrightOrdersToReceive" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend><b>Pending Outright Orders To Receive (On Order) Details :</b></legend>
															<asp:GridView ID="dgPending" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
																ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="TransDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="No" HeaderText="Order No.">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="SupplierName" HeaderText="Supplier">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="OrderType" HeaderText="Type">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="Priority" HeaderText="Priority">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ReceiptBalQty" HeaderText="Bal. Qty.">
																		<HeaderStyle HorizontalAlign="Right" Wrap="False" />
																		<ItemStyle HorizontalAlign="Right" Wrap="False" />
																	</asp:BoundField>
																</Columns>
																<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																<PagerStyle CssClass="paging" HorizontalAlign="Right" />
															</asp:GridView>
														</fieldset>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td>
														<fieldset id="lblPendingExchangeRepairOverhaulOrdersDetails" class="clsFieldSetNewStyle"
															style="border-width: 1px">
															<legend><b>Pending Exchange/Repair/Overhaul Orders Details :</b></legend>
															<asp:GridView ID="dgPendingExchangeRepairOverhaulOrders" runat="server" CssClass="clsGridNewStyle"
																AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" GridLines="Horizontal"
																CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="TransDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="No" HeaderText="Order No.">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="SupplierName" HeaderText="Supplier">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="OrderType" HeaderText="Type">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="Priority" HeaderText="Priority">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="EROQty" HeaderText="ERO Qty.">
																		<HeaderStyle HorizontalAlign="Right" Wrap="False" />
																		<ItemStyle HorizontalAlign="Right" Wrap="False" />
																	</asp:BoundField>
																</Columns>
																<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																<PagerStyle CssClass="paging" HorizontalAlign="Right" />
															</asp:GridView>
														</fieldset>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td>
														<fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend><b>Loan Returnable Details :</b></legend>
															<asp:GridView ID="dgReturnable" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="false"
																ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="TransDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="No" HeaderText="Issue No.">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FromWhom" HeaderText="Issue To">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ReceiptBalQty" HeaderText="Loan Qty.">
																		<HeaderStyle HorizontalAlign="Right" Wrap="False" />
																		<ItemStyle HorizontalAlign="Right" Wrap="False" />
																	</asp:BoundField>
																</Columns>
																<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																<PagerStyle CssClass="paging" HorizontalAlign="Right" />
															</asp:GridView>
														</fieldset>
													</td>
												</tr>
											</table>
										</asp:Panel>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</asp:Panel>
			<cc2:ModalPopupExtender runat="server" ID="mdeQuantityDetails" TargetControlID="btnDummyQuantityDetails"
				PopupControlID="pnlQuantityDetails" BackgroundCssClass="clsModalPopupBGForSecondPage">
			</cc2:ModalPopupExtender>
			<%--End Of Quantity Details--%>
		</div>
	</form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptShowPartNoStatus_Ajax.aspx.vb"
	Inherits="Flypal.wfrptShowPartNoStatus_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Part No Status</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<%--  <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
	<link href="Styles.css" rel="stylesheet" type="text/css" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script id="clientEventHandlersJS" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
		function openFilel() {
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
		<table id="tblmain" class="clstablelistout" border="0" style="z-index: 102; left: 7px; position: absolute; top: 7px">
			<tr>
				<td>
					<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
								<table id="tblInner" class="clstablelistin" border="0">
									<tr>
										<td colspan="2">
											<table width="100%">
												<tr>
													<td class="clsFormHeader1Newstyle">
														<table>
															<tr>
																<td style="width: 99%" valign="middle">
																	<span id="lbltitle" class="clsFormHeader">Part No. Status with its Alternate Parts</span>
																</td>
																<td align="right">
																	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<table id="Table3" border="0" cellspacing="0">
																				<tr>
																					<td>
																						<asp:Button ID="btnPrint1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																							Text="Print"></asp:Button>
																					</td>
																					<td>
																						<asp:Button ID="btnCreateRequisitionTop" runat="server" CausesValidation="False"
																							Visible='<%# AppSettings("ClientCode")="BA" %>' CssClass="clsbtnH clsinfoH" Text="Create Requisition"
																							ToolTip="Click to Create New Requisition" />
																					</td>
																					<td>
																						<asp:Button ID="btnClose1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																							Text="Close" CausesValidation="False" ToolTip="Click to Close Part No. status screen"></asp:Button>
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
										<td align="left">
											<table style="width: 80%; height: 44px" id="Table2" class="clsTable1" border="0">
												<tr>
													<td style="width: 67px" align="left">
														<asp:Label ID="lblPartNo1" runat="server" CssClass="clsLabelAuto">Part #</asp:Label>
													</td>
													<td align="left">
														<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
															MaxLength="100"></asp:TextBox>
													</td>
													<td>
														<asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
															MaxLength="100"></asp:TextBox>
													</td>
													<td>
														<asp:Label ID="lblUnit" runat="server" CssClass="clsLabelAuto">Unit</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
															MaxLength="100"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:CheckBox ID="chkShowOpenTransactionAlso" runat="server" AutoPostBack="true"
															Text='With "OPEN TRANSACTION(s)"' CssClass="clsCheckBox" />
													</td>
													<td colspan="3">
														<asp:CheckBox ID="chkIsValuedStore" runat="server" AutoPostBack="true" Text='ONLY "Valued Store"'
															CssClass="clsCheckBox" />
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<table class="clsLabelHeaderNewStyle" style="width: 100%; height: 20px;">
												<tr>
													<td>
														<asp:Label ID="lblInfo" runat="server" Visible="False" CssClass="clsLabelHeaderInfoNewStyle"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:GridView ID="dgStockPartStatus" runat="server" ShowHeaderWhenEmpty="false" PageSize="5"
												EmptyDataText="There are no data records to display." CellPadding="5" CssClass="clsGridNewStyle"
												AutoGenerateColumns="False" ForeColor="Black" GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<%--0--%>
													<asp:BoundField DataField="PartNo" HeaderText="Part #" HtmlEncode="False">
														<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
														<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
													</asp:BoundField>
													<%--1--%>
													<asp:BoundField DataField="Description" HeaderText="Description">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													</asp:BoundField>
													<%--2--%>
													<asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date">
														<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
														<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
													</asp:BoundField>
													<%--3--%>
													<asp:BoundField DataField="ReceiptNo" HeaderText="Receipt #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<%--4--%>
													<asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note #">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--5--%>
													<asp:BoundField DataField="ReleaseNotedate" HeaderText="Release Note date">
														<HeaderStyle HorizontalAlign="Left" Wrap="true" />
														<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
													</asp:BoundField>
													<%--6--%>
													<asp:BoundField DataField="ExpDateQtr" HeaderText="Expiry Date/Qtrs">
														<HeaderStyle HorizontalAlign="Left" />
														<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
													</asp:BoundField>
													<%--7--%>
													<asp:BoundField DataField="CalibrationDueDate" HeaderText="Calibration Due Date">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--8--%>
													<asp:BoundField DataField="EquipmentMaintenanceDueDate" HeaderText="Equipment Maint Due Date">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--9--%>
													<asp:BoundField DataField="StockBalanceQty" HeaderText="Stock Balance Qty.">
														<HeaderStyle HorizontalAlign="Right" Wrap="true" />
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<%--10--%>
													<asp:BoundField DataField="Unit" HeaderText="Unit">
														<HeaderStyle HorizontalAlign="Right" Wrap="false" />
														<ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
													</asp:BoundField>
													<%--11--%>
													<asp:BoundField DataField="SerialNoBatchNo" HeaderText="Sr.No./Batch No.">
														<HeaderStyle HorizontalAlign="Left" Wrap="false" />
													</asp:BoundField>
													<%--12--%>
													<asp:BoundField DataField="StoreName" HeaderText="Store Name">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--13--%>
													<asp:BoundField DataField="Location" HeaderText="Location">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--14--%>
													<asp:BoundField DataField="Note" HeaderText="Note">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--15--%>
													<asp:BoundField DataField="ItemTypeID" HeaderText="ItemTypeID">
														<HeaderStyle CssClass="hideGridColumn" />
														<ItemStyle CssClass="hideGridColumn" />
													</asp:BoundField>
													<%--16--%>
													<asp:BoundField DataField="ItemTypeName" SortExpression="ItemTypeName" HeaderText="Part Type">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<%--17--%>
													<asp:TemplateField HeaderText="Color Code">
														<HeaderStyle Width="20px" HorizontalAlign="Left"></HeaderStyle>
														<ItemTemplate>
															<asp:Label ID="lblColor" runat="server" CssClass="clsColorLabel" Height="18px" Width="18px"></asp:Label>
														</ItemTemplate>
													</asp:TemplateField>
													<%--18--%>
													<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
														<ItemTemplate>
															<asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ReceiptItemID") %>'
																CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
																Visible='<%#  Eval("IsAttachmentAdded")%>' />
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>
													<%--19--%>
													<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
														DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
													<%--20--%>
													<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
														DataField="ReceiptItemID" HeaderText="ReceiptItem ID"></asp:BoundField>
												</Columns>
												<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
												<SortedAscendingCellStyle BackColor="#F7F7F7" />
												<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
												<SortedDescendingCellStyle BackColor="#E5E5E5" />
												<SortedDescendingHeaderStyle BackColor="#242121" />
											</asp:GridView>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<table class="clsLabelHeaderNewStyle" style="width: 100%; height: 20px;">
												<tr>
													<td>
														<asp:Label ID="lblInfo1" runat="server" Visible="False" CssClass="clsLabelHeaderInfoNewStyle"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:GridView ID="dgOnOrderPartStatus" ShowHeaderWhenEmpty="false" runat="server"
												EmptyDataText="There are no data records to display." CellPadding="5" PageSize="5"
												CssClass="clsGridNewStyle" AutoGenerateColumns="False" ForeColor="Black" GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="PartNo" HeaderText="Part #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Description" HeaderText="Description">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="OrderDate" HeaderText="Order Date">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="OrderNo" HeaderText="Order #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="IntOrderNo" SortExpression="IntOrderNo" HeaderText="Int. Order #">
														<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="SupplierName" HeaderText="Supplier Name">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="OrderQty" HeaderText="Order Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="ReceivedQty" HeaderText="Received Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="OnOrderQty" HeaderText="On Order Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="DeliveryInDays" HeaderText="Delivery In Days">
														<HeaderStyle HorizontalAlign="Right" />
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
												</Columns>
												<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
												<SortedAscendingCellStyle BackColor="#F7F7F7" />
												<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
												<SortedDescendingCellStyle BackColor="#E5E5E5" />
												<SortedDescendingHeaderStyle BackColor="#242121" />
											</asp:GridView>
											</div>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<table class="clsLabelHeaderNewStyle" style="width: 100%; height: 20px;">
												<tr>
													<td>
														<asp:Label ID="lblInfo2" runat="server" Visible="False" CssClass="clsLabelHeaderInfoNewStyle"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:GridView ID="dgReturnablePartStatus" ShowHeaderWhenEmpty="false" runat="server"
												CellPadding="5" EmptyDataText="There are no data records to display." PageSize="5"
												CssClass="clsGridNewStyle" AutoGenerateColumns="False" ForeColor="Black" GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="PartNo" HeaderText="Part #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Description" HeaderText="Description">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="IssueNo" HeaderText="Issue #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="ToTypeName" HeaderText="Type Name">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="FromName" HeaderText="From Name">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="ReceiptBalanceQty" HeaderText="Receipt Balance Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
												</Columns>
												<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
												<SortedAscendingCellStyle BackColor="#F7F7F7" />
												<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
												<SortedDescendingCellStyle BackColor="#E5E5E5" />
												<SortedDescendingHeaderStyle BackColor="#242121" />
											</asp:GridView>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<table class="clsLabelHeaderNewStyle" style="width: 100%; height: 20px;">
												<tr>
													<td>
														<asp:Label ID="lblInfo3" runat="server" Visible="False" CssClass="clsLabelHeaderInfoNewStyle"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="1" align="left">
											<asp:GridView ID="dgPartsInTransit" runat="server" ShowHeaderWhenEmpty="false" PageSize="5"
												CellPadding="5" EmptyDataText="There are no data records to display." CssClass="clsGridNewStyle"
												AutoGenerateColumns="False" ForeColor="Black" GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ItemName" HeaderText="Part #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Description" HeaderText="Description">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="IssueDate" HeaderText="Issue Date">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="IssueNumber" HeaderText="Issue #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="FromStoreName" HeaderText="From Store">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="ToStoreName" HeaderText="To Store">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="ToReceiveQty" HeaderText="Receipt Balance Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
														DataField="ItemTypeID" HeaderText="ItemTypeID"></asp:BoundField>
													<asp:BoundField DataField="SerialNoBatchNo" HeaderText="Sr.No/Batch #">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="ItemTypeName" SortExpression="ItemTypeName" HeaderText="Part Type">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:TemplateField HeaderText="Color Code">
														<HeaderStyle Width="10px" HorizontalAlign="Left"></HeaderStyle>
														<ItemTemplate>
															<asp:Label ID="lblColor" runat="server" Width="50px"></asp:Label>
														</ItemTemplate>
													</asp:TemplateField>
												</Columns>
											</asp:GridView>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblRequisitionPartStatus" runat="server" CssClass="clsLabelHeaderInfoNewStyle"
												Visible="False">Requisition Part Status</asp:Label>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:GridView ID="dgRequisitionPartStatus" ShowHeaderWhenEmpty="true" runat="server"
												CellPadding="5" Visible="False" PageSize="5" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
												ForeColor="Black" GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ReqPartNo" HeaderText="Requested Part #">
														<HeaderStyle Wrap="False"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="ReqDescription" HeaderText="Description"></asp:BoundField>
													<asp:BoundField DataField="RequestedQty" HeaderText="Requested Qty.">
														<HeaderStyle HorizontalAlign="Right" />
													</asp:BoundField>
													<asp:BoundField DataField="EngApprovedIssueQty" HeaderText="Issue Approved Qty.">
														<HeaderStyle HorizontalAlign="Right" />
													</asp:BoundField>
													<asp:BoundField DataField="RequisitionItemStatus" HeaderText="Status">
														<HeaderStyle HorizontalAlign="Right" />
													</asp:BoundField>
												</Columns>
												<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
												<SortedAscendingCellStyle BackColor="#F7F7F7" />
												<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
												<SortedDescendingCellStyle BackColor="#E5E5E5" />
												<SortedDescendingHeaderStyle BackColor="#242121" />
											</asp:GridView>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<table class="clsLabelHeaderNewStyle" style="width: 100%; height: 20px;">
												<tr>
													<td>
														<asp:Label ID="lblNewRequisitionPartStatus" CssClass="clsLabelHeaderInfoNewStyle"
															runat="server" Visible="False">Requisition Part Status</asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:GridView ID="dgNewRequisitionPartStatusList" ShowHeaderWhenEmpty="false" runat="server"
												CellPadding="5" EmptyDataText="There are no data records to display." Visible="False"
												PageSize="5" CssClass="clsGridNewStyle" AutoGenerateColumns="False" ForeColor="Black"
												GridLines="Horizontal">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeaderNewStyle" Font-Bold="True" ForeColor="black" />
												<PagerSettings FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="RequisitionNo" HeaderText="Requisition #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="ReqDateFormatted" HeaderText="Requisition Date">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="EmployeeName" HeaderText="Indent By">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="PartNo" HeaderText="Requested Part #">
														<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Description" HeaderText="Description">
														<HeaderStyle HorizontalAlign="Left" />
													</asp:BoundField>
													<asp:BoundField DataField="RequestedQty" HeaderText="Requested Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="IssueBalQty" HeaderText="Issue Balance Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="OrderBalQty" HeaderText="Order Balance Qty.">
														<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
												</Columns>
												<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
												<SortedAscendingCellStyle BackColor="#F7F7F7" />
												<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
												<SortedDescendingCellStyle BackColor="#E5E5E5" />
												<SortedDescendingHeaderStyle BackColor="#242121" />
											</asp:GridView>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left"></td>
									</tr>
									<tr>
										<td align="left" colspan="2">
											<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" BackColor="Olive" ForeColor="Olive">Olive</asp:Label>
											<span id="Span3" class="clsLabel">Open Transaction(s)</span>
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
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForShowPartNoStatus();
				return false;
			}
		</script>
	</form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingSalesInvoiceItem_Ajax.aspx.vb"
	Inherits="Flypal.wfPendingSalesInvoiceItem_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>List Of Pending Items For Sales Invoice</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="Table-MaxWidth" class="clstablelistout">
			<tr>
				<td>
					<table id="tblInner" class="clstablelistin">
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table width="100%">
											<tr>
												<td colspan="4" class="clsFormHeader1Newstyle">
													<table width="100%">
														<tr>
															<td>
																<asp:Label ID="lblTitle" class="clsFormHeader">List Of Pending Items For Sales Invoice</asp:Label>
															</td>
															<td align="right">
																<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" 
																			ToolTip="Click to go back to previous page."
																			Text="Back"></asp:Button>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td>
													<table id="Table2">
														<tr>
															<td>
																<asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
															</td>
															<td>
																<asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" 
																	ToolTip="Enter Part No"></asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td align="right">
													<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
														ToolTip="Click to search as per Criteria."
														CausesValidation="false" class="clsSearch2btn" />
												</td>
											</tr>
											<tr>
												<td>
													<br />
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:GridView ID="dgPartStockStatusList" runat="server" ShowHeaderWhenEmpty="true"
														AutoGenerateColumns="False" AllowSorting="True" CssClass="clsGridNewStyle" 
														GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
															Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField Visible="False" DataField="ItemID" HeaderText="Item ID"></asp:BoundField>
															<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Part Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Qty" SortExpression="Qty" HeaderText="Balance Qty.">
																<HeaderStyle HorizontalAlign="Right" ForeColor="#FFFFFF"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:ButtonField>
														</Columns>
													</asp:GridView>
												</td>
											</tr>
											<tr>
												<td>
													<br />
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:GridView ID="dgItemOrderIssueDetail" runat="server" ShowHeaderWhenEmpty="true"
														AutoGenerateColumns="False" AllowSorting="True" CssClass="clsGridNewStyle"
														GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10"
														EnableViewState="false">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
															Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="BalanceQty" SortExpression="BalanceQty" HeaderText="Balance Qty.">
																<HeaderStyle HorizontalAlign="Right" ForeColor="#FFFFFF"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="IssueNumber" SortExpression="IssueNumber" HeaderText="Issue No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="IssueDateFormatted" SortExpression="IssueDateFormatted"
																HeaderText="Issue Date">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReceiptNumber" SortExpression="ReceiptNumber" HeaderText="Receipt No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReceiptDateFormatted" SortExpression="ReceiptDateFormatted"
																HeaderText="Receipt Date">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R. N. No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReleaseNoteDateFormatted" SortExpression="ReleaseNoteDateFormatted"
																HeaderText="R. N. Date">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Type">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:ButtonField>
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
							
						</tr>
					</table>
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

	</form>
</body>
</html>

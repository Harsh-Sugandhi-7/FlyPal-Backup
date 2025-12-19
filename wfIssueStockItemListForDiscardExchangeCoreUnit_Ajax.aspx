<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax.aspx.vb" Inherits="Flypal.wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
	<title>Stock Item List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<style type="text/css">

		#lblNote1 {
			display: block;
			margin-block: 5px;
		}

	</style>

</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="Form2" method="post" runat="server">

		<asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>

		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lbltitle" class="clsFormHeader">Stock Item List
												</span>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="tblActionBtns">
															<tr>
																<td>
																	<asp:Button ID="btnBack" CssClass="clsbtnH clsinfoH"
																		runat="server" ToolTip="Click to go back to the previous page"
																		Text="Back" CausesValidation="False"></asp:Button>
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
									<span id="lblNote1" class="clsLabelHeader">Following is the list of Part's Stock available
                                    in different Store with available quantity. </span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1">
												<tr>
													<td>
														<span id="lblPartNo" class="clsLabel">Part No.</span>
													</td>
													<td>
														<asp:TextBox ID="txtPartNo" runat="server" MaxLength="50" ToolTip="Enter Part No."
															CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.ItemName %>">
														</asp:TextBox>
													</td>
													<td>
														<span id="lblDesc" class="clsLabel">Description</span>
													</td>
													<td>
														<asp:TextBox ID="txtDesc" runat="server" MaxLength="50" ToolTip="Enter Part No."
															CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.ItemDesc %>">
														</asp:TextBox>
													</td>
													<td>
														<span id="lblSerialNo" class="clsLabel">Serial No.</span>
													</td>
													<td>
														<asp:TextBox ID="txtSerialNo" runat="server" MaxLength="50" ToolTip="Enter Part No."
															CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0" ReadOnly="True" Text="<%# mPendingToReturnForExchangeRepairInfo.SerialNo %>">
														</asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td colspan="6">
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td colspan="6">
														<asp:GridView ID="dgIssueStockItemList" runat="server" AllowSorting="True"
															ShowHeaderWhenEmpty="true" EnableViewState="false" AutoGenerateColumns="False"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
															AllowPaging="True" PageSize="10">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField DataField="StarMark"></asp:BoundField>
																<asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Rec Date">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ReceiptTextIntReceiptNo" SortExpression="ReceiptTextIntReceiptNo"
																	HeaderText="Rec. No." HtmlEncode="false">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="R.Note no.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="AvailableQuantity" SortExpression="AvailableQuantity"
																	HeaderText="Stock Qty.">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="EROQty" SortExpression="EROQty" HeaderText="ERO Qty">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectPart">
																	<HeaderStyle HorizontalAlign="Left" />
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
								<td>
									<span id="Label1" class="clsLabelHeader">* : Part is mentioned in the Order</span>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>

		<!-- Ajax Loader -->
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

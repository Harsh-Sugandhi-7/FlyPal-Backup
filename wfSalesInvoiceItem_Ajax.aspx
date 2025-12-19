<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesInvoiceItem_Ajax.aspx.vb"
	Inherits="Flypal.wfSalesInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Sales Invoice Item Details</title>
	<script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
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
		<asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout">
				<tr>
					<td>
						<asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
							<ContentTemplate>
								<table width="100%">
									<tr>
										<td class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Sales Invoice Item [New]</asp:Label>
													</td>
													<td align="right">
														<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
															<ContentTemplate>
																<table id="Table1" border="0">
																	<tr>
																		<td>
																			<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add Sales Invoice Item"
																				Text="OK"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
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
											<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
												HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
											<asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ErrorMessage="Part Required"
												ControlToValidate="txtPartNo" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity Required"
												ControlToValidate="txtQty" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ErrorMessage="Part Description Required."
												ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvRate" runat="server" ErrorMessage="Rate Required"
												ControlToValidate="txtRate" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvOtherCharge" runat="server" ErrorMessage="Other Charge Must be greater than Zero."
												ControlToValidate="txtOtherCharges" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvQty" runat="server" ErrorMessage="Quantity must be greater than Zero."
												ControlToValidate="txtQty" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvCRate" runat="server" ErrorMessage="Rate Must be greater than Zero."
												ControlToValidate="txtRate" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
										</td>
									</tr>
								</table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<asp:Panel runat="server" ID="Panel3" Style="width: auto;">
							<asp:UpdatePanel runat="server" ID="upnlQuotationItem" UpdateMode="Conditional">
								<ContentTemplate>
									<fieldset class="clsFieldSetNewStyle">
										<legend>
											<b>Sales Invoice Item</b>
										</legend>
										<table>
											<tr>
												<td></td>
												<td>
													<span id="spnSrNo" class="clsLabel">Sr. No.</span>
												</td>
												<td>
													<asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
														MaxLength="4" ReadOnly="True" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.SrNo %>"
														ToolTip="Sr. No." Width="36px"></asp:TextBox>
												</td>
												<td></td>
											</tr>
											<tr>
												<td>
													<span id="spnPartNoStar" class="clsLabelStar">*</span>
												</td>
												<td>
													<span id="spnPartNo" class="clsLabel">Part No.</span>
												</td>
												<td colspan="2">
													<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
														ReadOnly='<%# Session("Edit") %>' Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemName %>"
														ToolTip="Enter Part No.">
													</asp:TextBox>
													<asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" 
														Height="22px" Width="24px" ToolTip="Click to Select New Part No." 
														CausesValidation="False"></asp:ImageButton>

												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="spnDescription" class="clsLabel">Description</span>
												</td>
												<td colspan="2">
													<asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
														MaxLength="50" ReadOnly="True" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemDescription %>"
														ToolTip="Part Description"></asp:TextBox>
												</td>
											</tr>
										</table>
									</fieldset>
								</ContentTemplate>
							</asp:UpdatePanel>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td>
						<asp:UpdatePanel runat="server" ID="upnlSalesInvoiceItemInformation" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:PlaceHolder runat="server" Visible="<%# mSalesInvoice.TransTypeID = 23 %>">
								<fieldset class="clsFieldSetNewStyle">
									<legend>
										<b>Sales Invoice Item Information</b>
									</legend>
									<table>
										<tr>
											<td>
												<span id="Span1" class="clsLabelStar" style="color: WhiteSmoke">*</span>
											</td>
											<td>
												<asp:Label ID="lblOrderIssueNo" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">Issue No.</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtOrderIssueNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.IssueNumber %>" CssClass="clsTextBoxTagSearch"
													Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
											<td>
												<asp:Label ID="lblOrderIssueDate" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">Issue Date</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtOrderIssueDate" runat="server" ReadOnly="True" BackColor="#E0E0E0" Width="100px"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.IssueDateFormatted %>"
													CssClass="clsTextBoxTagSearchDate" Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<asp:Label ID="lblReceiptNo" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">Receipt No.</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtReceiptNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ReceiptNumber %>" CssClass="clsTextBoxTagSearch"
													Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
											<td>
												<asp:Label ID="lblReceiptDate" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">Receipt Date</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtReceiptDate" runat="server" ReadOnly="True" BackColor="#E0E0E0" Width="100px"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ReceiptDateFormatted %>"
													CssClass="clsTextBoxTagSearchDate" Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<asp:Label ID="lblReleaseNote" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">Rel. Note No.</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtRelNoteNo" runat="server" ReadOnly="True" BackColor="#E0E0E0"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ReleaseNoteNo %>" CssClass="clsTextBoxTagSearch"
													Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
											<td>
												<asp:Label ID="lblRelNoteDate" runat="server" CssClass="clsLabel" Visible="<%# mSalesInvoice.TransTypeID=23 %>">R. Note  Date</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtRelNoteDate" runat="server" ReadOnly="True" BackColor="#E0E0E0" Width="100px"
													Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.ReleaseNoteDateFormatted %>"
													CssClass="clsTextBoxTagSearchDate" Visible="<%# mSalesInvoice.TransTypeID=23 %>">
												</asp:TextBox>
											</td>
										</tr>
									</table>
								</fieldset>
								</asp:PlaceHolder>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<fieldset class="clsFieldSetNewStyle">
							<legend>
								<b>Values</b>
							</legend>
							<table>
								<tr>
									<td>
										<span id="spnQtyStar" class="clsLabelStar">*</span>
									</td>
									<td>
										<span id="lblQuantity" class="clsLabel">Qty.</span>
									</td>
									<td colspan="2">
										<asp:TextBox ID="txtQty" runat="server" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty %>"
											CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Enter Quantity" Enabled="<%#  IIf(mSalesInvoice.SalesInvoiceItems.CurrentItem.TransTypeID = 74 And mSalesInvoice.SalesInvoiceItems.CurrentItem.IsSerialized = True, False, True) %>"
											Width="150px">
										</asp:TextBox>
										<asp:TextBox ID="txtQtyUnit" runat="server" Width="96px" ReadOnly="True" BackColor="#E0E0E0"
											Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.unit %>" CssClass="clsTextBoxTagSearch">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblStarRate" class="clsLabelStar">*</span>
									</td>
									<td>
										<span id="lblRate" class="clsLabel">Rate</span>
									</td>
									<td>
										<asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="12"
											Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate %>" ToolTip="Enter Rate"
											Width="150px"></asp:TextBox>
									</td>
									<td>
										<asp:TextBox ID="txtRateCurrency" runat="server" Width="96px" ReadOnly="True" BackColor="#E0E0E0"
											Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.Currency %>" CssClass="clsTextBoxTagSearch">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblOtherCharges" class="clsLabelAuto">Oth. Charges</span>
									</td>
									<td colspan="2">
										<asp:TextBox ID="txtOtherCharges" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
											MaxLength="12" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.COtherCharges %>"
											ToolTip="Enter Other Charge" Width="150px"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblAmount" class="clsLabelAuto">Amount</span>
									</td>
									<td colspan="2">
										<asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
											MaxLength="12" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.CAmount %>"
											ToolTip="Amount" Width="150px"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblEffRate" class="clsLabelAuto">Effective Rate</span>
									</td>
									<td colspan="2">
										<asp:TextBox ID="txtCEffRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
											MaxLength="12" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.CEffRate %>"
											ToolTip="Effective Rate" Width="150px"></asp:TextBox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<asp:Panel runat="server" ID="pnlRemarkNote" Style="width: auto;">
							<fieldset class="clsFieldSetNewStyle" visible="<%# mSalesInvoice.TransTypeID=23 %>">
								<legend>
									<b>Remark/Note</b>
								</legend>
								<table>
									<tr>
										<td>
											<span id="Span2" class="clsLabelStar" style="color: WhiteSmoke">*</span>
										</td>
										<td>
											<span id="spnRemark" class="clsLabel">Remark<asp:CustomValidator ID="cvRemark" runat="server"
												ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 100."
												OnServerValidate="CustomValidate"></asp:CustomValidator>
											</span>
										</td>
										<td>
											<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="250"
												Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.Remark %>" TextMode="MultiLine"
												ToolTip="Enter Remark." Width="300px" Height="60px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td></td>
										<td>
											<span id="Span6" class="clsLabelAuto">Note<asp:CustomValidator ID="cvNote" runat="server"
												ControlToValidate="txtNote" CssClass="clsLabelAuto" 
												Display="None" ErrorMessage="Max. Length should be 150."
												OnServerValidate="CustomValidate"></asp:CustomValidator>
											</span>
										</td>
										<td>
											<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="250"
												Text="<%# mSalesInvoice.SalesInvoiceItems.CurrentItem.Note %>" TextMode="MultiLine"
												ToolTip="Enter Note." Width="300px" Height="60px"></asp:TextBox>
										</td>
									</tr>
								</table>
							</fieldset>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</div>

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

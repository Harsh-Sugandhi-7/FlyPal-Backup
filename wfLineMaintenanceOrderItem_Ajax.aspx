<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceOrderItem_Ajax.aspx.vb"
	Inherits="Flypal.wfLineMaintenanceOrderItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
		<div>
			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
							<table id="tblinner" class="clsTablelistin">
								<tr id="Header">
									<td colspan="5" class="clsFormHeader1Newstyle">
										<table id="tblHeader" width="100%">
											<tr>
												<td>
													<asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">
														Service Order Item [New]
													</asp:Label>
												</td>
												<td colspan="5" align="right">
													<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<table id="Table1">
																<tr>
																	<td>
																		<asp:Button ID="btnSave" runat="server"
																			CssClass="clsbtnH clsinfoH" Text="Ok" 
																			ToolTip="Click to add Item in Order Item List" />
																	</td>
																	<td>
																		<asp:Button ID="btnBack" runat="server" 
																			CssClass="clsbtnH clsinfoH" Text="Back" 
																			ToolTip="Click to go back to the previous page"
																			CausesValidation="False" />
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
								<tr id="ValidationSummary">
									<td colspan="5">
										<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvJobDetails" runat="server" ErrorMessage="Job Details Required"
													ControlToValidate="txtJobDetails" CssClass="clsLabelAuto" Display="None">
												</asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity Required"
													ControlToValidate="txtQty" CssClass="clsLabelAuto" Display="None">
												</asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvRate" runat="server" ErrorMessage="Rate Required"
													ControlToValidate="txtRate" CssClass="clsLabelAuto" Display="None">
												</asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvJobDetails" runat="server" ErrorMessage="Rate Must be greater than Zero."
													ControlToValidate="txtJobDetails" Display="None" OnServerValidate="customvalidate"
													CssClass="clsLabelAuto">
												</asp:CustomValidator>
												<asp:CustomValidator ID="cvCRate" runat="server" ControlToValidate="txtRate" CssClass="clsLabelAuto"
													Display="None" ErrorMessage="Rate Must be greater than Zero." OnServerValidate="customvalidate">
												</asp:CustomValidator>
												<asp:CustomValidator ID="cvQty" runat="server" ControlToValidate="txtQty" CssClass="clsLabelAuto"
													Display="None" ErrorMessage="Quantity must be greater than Zero." OnServerValidate="customvalidate">
												</asp:CustomValidator>
												<asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" CssClass="clsLabelAuto"
													Display="None" OnServerValidate="customvalidate">
												</asp:CustomValidator>
												<asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtNote" CssClass="clsLabelAuto"
													DESIGNTIMEDRAGDROP="319" Display="None" OnServerValidate="customvalidate">
												</asp:CustomValidator>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<span id="lblOrderInfo" class="clsLabelHeader">Service Order Item Information</span>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblSrNo" class="clsLabel">Sr. No.</span>
									</td>
									<td>
										<asp:TextBox ID="txtSrNo" runat="server" 
											CssClass="clsTextBoxTagSearchSmall" ReadOnly="True"
											BackColor="#E0E0E0" MaxLength="5" 
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.SrNo %>">
										</asp:TextBox>
									</td>
									<td colspan="2" align="right"></td>
								</tr>
								<tr>
									<td>
										<span id="lblStarDesc" class="clsLabelStar">*</span>
									</td>
									<td>
										<span id="lblJobDetails" class="clsLabel">Job Details</span>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtJobDetails" runat="server" MaxLength="500"
											CssClass="clsTextBoxSearch_Ajax" BackColor="White" Height="46px"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.JobDetails %>"
											ToolTip="Enter Job Details." TextMode="MultiLine">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td>
										<span id="Label2" class="clsLabelStar">*</span>
									</td>
									<td>
										<span id="lblQuant" class="clsLabel">Quantity</span>
									</td>
									<td>
										<asp:TextBox ID="txtQty" runat="server" ToolTip="Enter Quantity." Width="85px"
											CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Qty %>">
											
										</asp:TextBox>
									</td>
									<td>
										<span id="lblUnit" class="clsLabel">Unit</span>
									</td>
									<td>
										<asp:TextBox ID="txtUnit" runat="server" ToolTip="Enter Unit."
											CssClass="clsTextBoxTagSearchSmall" MaxLength="10" Width="85px"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Unit %>">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td>
										<span id="Label9" class="clsLabelStar">*</span>
									</td>
									<td>
										<span id="lblRate" class="clsLabel">Rate</span>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtRate" runat="server" ToolTip="Enter Rate." Width="85px"
											CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="12"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.CRate %>">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblAmount" class="clsLabel">Amount</span>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtAmount" runat="server"
											CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
											ReadOnly="True" BackColor="#E0E0E0" Width="85px"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.CAmount %>"
											MaxLength="12">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblRemark" class="clsLabel">Remark</span>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtRemark" runat="server" Width="408px"
											CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="250"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Remark %>"
											ToolTip="Enter Remark." TextMode="MultiLine">
										</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td></td>
									<td>
										<span id="lblNote" class="clsLabel">Note</span>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtNote" runat="server" Width="408px"
											CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="250"
											Text="<%# mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.Note %>"
											ToolTip="Enter Note." TextMode="MultiLine">
										</asp:TextBox>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</div>
	</form>
</body>
</html>

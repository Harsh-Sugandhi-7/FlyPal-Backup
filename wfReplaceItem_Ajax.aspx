<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReplaceItem_Ajax.aspx.vb"
	Inherits="Flypal.wfReplaceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Replace Item</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
							<table width="100%">
								<tr class="clsFormHeader1Newstyle">
									<td>
										<table width="100%">
											<tr>
												<td>
													<span id="lblTitle" class="clsFormHeader">Replace Item</span>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
													CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvItem" runat="server" ErrorMessage="Select Item." ControlToValidate="cmbItem"
													Display="None" ClientValidationFunction="ValidationItem" CssClass="clsValidationSummary"></asp:CustomValidator><asp:CustomValidator
														ID="cvReplaceWithItem" runat="server" ErrorMessage="Select Replace With Item."
														ControlToValidate="cmbReplaceWithItem" Display="None" ClientValidationFunction="ValidationReplaceWithItem"
														CssClass="clsValidationSummary"></asp:CustomValidator>
												<asp:CustomValidator ID="cv" runat="server" ClientValidationFunction="ValidateBothCategories"
													ControlToValidate="cmbReplaceWithItem" CssClass="clsValidationSummary" Display="None"
													ErrorMessage="Please Select Different Replace With Item."></asp:CustomValidator>
												<script type="text/javascript">
													function ValidationItem(source, args) {
														var dd = $get("cmbItem");
														args.IsValid = true;
														if (dd.selectedIndex == 0) {
															args.IsValid = false;
															return;
														}
													}
												</script>
												<script type="text/javascript">
													function ValidationReplaceWithItem(source, args) {
														var dd = $get("cmbReplaceWithItem");
														args.IsValid = true;
														if (dd.selectedIndex == 0) {
															args.IsValid = false;
															return;
														}
													}
												</script>
												<script type="text/javascript">
													function ValidateBothCategories(source, args) {
														var e = document.getElementById("cmbItem");
														var ItemID = e.options[e.selectedIndex].value;
														var e1 = document.getElementById("cmbReplaceWithItem");
														var ReplaceWithItemID = e1.options[e1.selectedIndex].value;
														args.IsValid = true;

														if (ItemID == ReplaceWithItemID) {
															args.IsValid = false;
														}
													}
												</script>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlItemDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fsItemDetails" style="padding: 0px 4px 0px 0px; width: auto; border-width: 1px"
													class="clsFieldSetNewStyle">
													<legend><b>Item Information </b></legend>
													<table>
														<tr>
															<td>
																<span id="lblItemStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblItem" class="clsLabelAuto">Item</span>
															</td>
															<td colspan="3">
																<asp:DropDownList ID="cmbItem" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																	DataTextField="Name" AutoPostBack="true">
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblDesc" runat="server" CssClass="clsLabel" Visible="False">Description:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblDescription" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblUn" runat="server" CssClass="clsLabel" Visible="False">Unit:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblUnit" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblCat" runat="server" CssClass="clsLabel" Visible="False">Category:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblCategory" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblSerialize" runat="server" CssClass="clsLabel" Visible="False">Serialize:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblSerializeStatus" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel" Visible="False">Serial No.:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblSerialNumbser" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblCalibration" runat="server" CssClass="clsLabel" Visible="False">Calibration Interval:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblCalibrationInterval" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
															<td></td>
															<td></td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlReplaceWithItemDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fsReplaceWithItemDetails" style="padding: 0px 4px 0px 0px; width: auto; border-width: 1px"
													class="clsFieldSetNewStyle">
													<legend><b>Replace With Item Information </b></legend>
													<table>
														<tr>
															<td>
																<span id="lblReplaceWithItemStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblReplaceWithItem" class="clsLabelAuto">Replace With Item</span>
															</td>
															<td colspan="2">
																<asp:DropDownList ID="cmbReplaceWithItem" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																	DataTextField="Name" AutoPostBack="true">
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblReplaceWithItemDesc" runat="server" CssClass="clsLabel" Visible="False">Description:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemDescription" runat="server" CssClass="clsLabelHeader"
																	Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemUn" runat="server" CssClass="clsLabel" Visible="False">Unit:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemUnit" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblReplaceWithItemCat" runat="server" CssClass="clsLabel" Visible="False">Category:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemCategory" runat="server" CssClass="clsLabelHeader"
																	Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemSerialize" runat="server" CssClass="clsLabel" Visible="False">Serialize:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemSerializeStatus" runat="server" CssClass="clsLabelHeader"
																	Visible="False"></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemSerialNo" runat="server" CssClass="clsLabel" Visible="False">Serial No.:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemSerialNumbser" runat="server" CssClass="clsLabelHeader" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblReplaceWithItemCalibration" runat="server" CssClass="clsLabel"
																	Visible="False">Calibration Interval:</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblReplaceWithItemCalibrationInterval" runat="server" CssClass="clsLabelHeader"
																	Visible="False"></asp:Label>
															</td>
															<td></td>
															<td></td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table3" border="0" cellspacing="1" cellpadding="1">
													<tr>
														<td>
															<asp:Label ID="lblYellow" runat="server" BackColor="Yellow" ForeColor="Yellow">Green</asp:Label>
															<asp:Label ID="lblInformationMismatch" runat="server" CssClass="clsLabelHeader">Information Mismatch</asp:Label>
															<asp:Label ID="lblGreen" runat="server" BackColor="Green" ForeColor="Green">Green</asp:Label>
															<asp:Label ID="lblSerialNoSame" runat="server" CssClass="clsLabelHeader">Serial No. Same</asp:Label>
														</td>
														<td></td>
														<td align="right">
															<asp:Button ID="btnReplaceNDelete" runat="server"
																CssClass="clsbtnH clsinfoH1" Text="Replace &amp; Delete"
																ToolTip="Click to Replace &amp;  Delete Old Item." Width="120px"></asp:Button>
															<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH1"
																Text="Close" ToolTip="Click to Close"
																CausesValidation="False"></asp:Button>
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
	</form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceiptcumInvoiceItem_Ajax.aspx.vb"
	Inherits="Flypal.wfReceiptcumInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Receiving Part Information</title>
	<script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<script type="text/javascript">
			window.onload = blinknow;
			function blinknow() {
				var e = document.getElementById("<%=ImgID.ClientID%>");
				if (e != null) {
					e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
					setTimeout("blinknow();", 750);
				}
			}
		</script>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout">
				<tr>
					<td colspan="2">
						<asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
							<ContentTemplate>
								<table width="100%">
									<tr>
										<td class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Receiving Part [New]</asp:Label>
													</td>
													<td align="right">
														<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
															<ContentTemplate>
																<table id="Table1" border="0">
																	<tr>
																		<td>
																			<asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add Part in Goods Receipt List"
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
											<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields" />
											<asp:CustomValidator ID="cvSrNoLen" runat="server" ControlToValidate="txtSerialNo"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Max lenght of Serial No should be 50."
												OnServerValidate="CustomValidate"></asp:CustomValidator>
											<asp:RequiredFieldValidator ID="rfvStore" runat="server" ControlToValidate="cmbStore"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Store Required."></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvSrNo" runat="server" ControlToValidate="txtSrNo"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Sr. No. Required."></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Part No. Required."></asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQuantity"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Quantity Required."></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvPartType" runat="server" ControlToValidate="cmbPartType"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Please select the Part Type."
												OnServerValidate="CustomValidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvStore" runat="server" ControlToValidate="cmbStore" CssClass="clsLabelAuto"
												Display="None" ErrorMessage="Please select the Store." OnServerValidate="CustomValidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvRelNoteNo" runat="server" ControlToValidate="txtReleaseNote"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Max lenght should be 50."
												OnServerValidate="CustomValidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvCRate" runat="server" Display="None" ErrorMessage="Should be Non-Zero Positive Value."
												ControlToValidate="txtCRate" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"></asp:CustomValidator>
											<asp:CustomValidator ID="cvQty" runat="server" ControlToValidate="txtQuantity" Display="None"
												ErrorMessage="Should be Non-Zero Positive Value." OnServerValidate="CustomValidate"
												CssClass="clsLabelAuto"></asp:CustomValidator>
											<asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDescription"
												CssClass="clsLabel" Display="None" ErrorMessage="Description Required, For That Select Part From List"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvCodeNo" runat="server" ControlToValidate="txtCodeNo" Display="None"
												ErrorMessage="Code No. Required" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
												ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="cvLoc" runat="server" ControlToValidate="txtLocation" CssClass="clsLabelAuto"
												Display="None" ErrorMessage="Bin Location Require" OnServerValidate="CustomValidate"
												ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="cvCustVal" runat="server" Display="None" OnServerValidate="CustomValidate1"
												CssClass="clsLabelAuto"></asp:CustomValidator>
											<asp:CustomValidator ID="CvIsAirworthinessCheck" runat="server" Display="None" OnServerValidate="CustomValidate1"
												ClientValidationFunction="validateAirworthiness" ErrorMessage="Checked,if Airworthiness Inspection performed."
												CssClass="clsLabelAuto"></asp:CustomValidator>
											<asp:CustomValidator ID="csWarStatus" runat="server" ControlToValidate="cmbWarrantyStatus"
												Display="None" ErrorMessage="Please Select Warranty Status As Accepted Or Rejected"
												OnServerValidate="CustomValidate" CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="cvRelNo" runat="server" ControlToValidate="txtReleaseNote"
												CssClass="clsLabelAuto" Display="None" ErrorMessage="Release Note No Require."
												OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="csFaultFound" runat="server" ControlToValidate="cmbFaultFound"
												Display="None" ErrorMessage="Fault found or not Please Select Yes/No" OnServerValidate="CustomValidate"
												CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
											<asp:Button ID="hdnAddPeriod" runat="server" Text="----" CausesValidation="False"
												Style="display: none;"></asp:Button>
											<%--  'Added by Shital on 07-Sept-2016--%>
											<script type="text/javascript">
												function validateAirworthiness(sender, args) {
													var chkAirworthiness = document.getElementById("<%=chkAirworthiness.ClientID %>");
													if (chkAirworthiness != null) {
														if (chkAirworthiness.checked == true) {
															args.IsValid = true;
														} else {
															args.IsValid = false;
														}
													} else {
														args.IsValid = true;
													}
												}
											</script>
										</td>
									</tr>
								</table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td>
						<table>
							<tr>
								<td align="right">
									<asp:UpdatePanel runat="server" ID="upnlAttentionInfo" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Image ID="ImgID" runat="server" ImageUrl="~/images/Attention.ico" Visible="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID > 0 %>" />
											<asp:Label ID="lblImageTagName" runat="server" CssClass="clsLabel" Text='<%# " ATTENTION! " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName + " OBSERVE PRECAUTIONS FOR HANDLING." %>'
												Visible="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID > 0 %>"
												ForeColor="Red"></asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</td>
					<td align="right">
						<asp:Label ID="lblSerializedStatus" runat="server" CssClass="clsLabelAuto" Font-Bold="True"
							Visible="False">Receiving Serialized Part</asp:Label>
					</td>
				</tr>
				<!--**********************************************************-->
				<tr>
					<td colspan="2" valign="top">
						<fieldset id="Fieldset14" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
							class="clsFieldSet">
							<legend class="clsFieldSet1"><b>Part Information</b></legend>
							<table width="100%">
								<tr>
									<td valign="top">
										<asp:Panel runat="server" ID="Panel3" Style="width: auto;">
											<asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
												<ContentTemplate>
													<fieldset id="Fieldset9" style="padding: 0px 2px 0px 0px; width: auto; border-style: none"
														class="clsFieldSetNewStyle">
														<table>
															<tr>
																<td>&nbsp;
																</td>
																<td>
																	<span id="spnSrNo" class="clsLabel">No.</span>
																</td>
																<td>
																	<asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
																		MaxLength="4" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SrNo %>"
																		ToolTip="Sr. No." Width="36px"></asp:TextBox>
																</td>
																<td>&nbsp;
																</td>
															</tr>
															<tr>
																<td>
																	<span id="spnPartNoStar" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="spnPartNo" class="clsLabel">Part No.</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
																		ReadOnly='<%# Session("Edit") %>' Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName %>"
																		ToolTip="Enter Part No.">
																	</asp:TextBox>
																</td>
																<td>
																	<asp:ImageButton ID="imgPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																		Enabled='<%# Not Session("Edit") %>' CausesValidation="False"
																		Width="24px" ToolTip="Click to Select New Part No."></asp:ImageButton>
																</td>
																<td>
																	<asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH1" Text="Alternate Part"
																		ToolTip="Click to add Alternate Part" Visible="<%# mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 27 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54 Or mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 13 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 28 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 57 Or mReceiptCumInvoice.TransTypeID = 46 Or mReceiptCumInvoice.TransTypeID = 47 Or mReceiptCumInvoice.TransTypeID = 56 Or mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 62 Or mReceiptCumInvoice.TransTypeID = 66%>" />
																</td>
															</tr>
															<tr>
																<td>&nbsp;
																</td>
																<td>
																	<span id="spnDescription" class="clsLabel">Description</span>&nbsp;
																</td>
																<td colspan="2">
																	<asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchLong"
																		TextMode="MultiLine" Height="36px" MaxLength="50" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemDescription %>"
																		ToolTip="Part Description"></asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>&nbsp;
																</td>
																<td>
																	<asp:CheckBox ID="ChkIsConsiderAsAsset" runat="server" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsConsiderAsAsset %>"
																		CssClass="clsCheckBox" Enabled="<%#mReceiptCumInvoice.StatusID = 1%>" Text="Consider As Asset"
																		TextAlign="Left" Visible="<%# mReceiptCumInvoice.TransTypeID=67 Or mReceiptCumInvoice.TransTypeID=10 Or (mReceiptCumInvoice.TransTypeID= 9 AND (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2)) %>" />
																</td>
																<td colspan="2">
																	<asp:CheckBox ID="chkRemovedasReturnableFromAircraft" runat="server" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovedAsReturnableFromAircraft %>"
																		CssClass="clsCheckBox" Text="Removed as Returnable From Aircraft" TextAlign="Left"
																		Visible="<%# mReceiptCumInvoice.TransTypeID=9 %>" />
																</td>
															</tr>
														</table>
													</fieldset>
												</ContentTemplate>
											</asp:UpdatePanel>
										</asp:Panel>
									</td>
									<td valign="top">
										<asp:Panel runat="server" ID="Panel4" Style="width: auto;" Visible="<%#Not (mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) %>">
											<asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
												<ContentTemplate>
													<fieldset id="Fieldset10" style="padding: 0px 2px 0px 0px; width: auto; border-style: none"
														class="clsFieldSetNewStyle">
														<table>
															<tr>
																<td>
																	<asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabel" Text='<%# IIf(Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID.Equals(Guid.Empty), "Issue No.", IIf(Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID.Equals(Guid.Empty), "Order No", "Ord./Iss.No.")) %>'
																		Visible="<%# not (mReceiptCumInvoice.TransTypeID = 9  or mReceiptCumInvoice.TransTypeID = 48 or mReceiptCumInvoice.TransTypeID = 50 or mReceiptCumInvoice.TransTypeID = 53 or mReceiptCumInvoice.TransTypeID = 57) %>">
																	</asp:Label>
																</td>
																<td>
																	<asp:TextBox ID="txtOrderIssNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderIssueNo %>"
																		ToolTip="Order/Issue No." Visible="<%# not (mReceiptCumInvoice.TransTypeID = 9 or mReceiptCumInvoice.TransTypeID = 48 or mReceiptCumInvoice.TransTypeID = 50 or mReceiptCumInvoice.TransTypeID = 53or mReceiptCumInvoice.TransTypeID = 57) %>">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<asp:Label ID="lblDate1" runat="server" CssClass="clsLabel" Text='<%# IIf(Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID.Equals(Guid.Empty), "Issue Date.", IIf(Not mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID.Equals(Guid.Empty) , "Order Date", "Ord./Iss.Date")) %>'
																		Visible="<%# not (mReceiptCumInvoice.TransTypeID = 9  or mReceiptCumInvoice.TransTypeID = 48 or mReceiptCumInvoice.TransTypeID = 50 or mReceiptCumInvoice.TransTypeID = 53 or mReceiptCumInvoice.TransTypeID = 57) %>">
																	</asp:Label>
																</td>
																<td>
																	<asp:TextBox ID="txtOrderDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
																		ReadOnly="true" ClientIDMode="Static" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IODateFormatted %>"></asp:TextBox>
																	<cc2:CalendarExtender ID="txtOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOrderDate"></cc2:CalendarExtender>
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtOrderDate" ID="txtOrderDateWatermarkExtender"
																		runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblRequestedBy" class="clsLabel" runat="server" visible="<%#Not (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) %>">Requested By</span>
																</td>
																<td>
																	<asp:TextBox ID="txtRequestedBy" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		MaxLength="250" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RequestedBy %>"
																		ToolTip="Requested By" Visible="<%# not (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 57) %>">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblWarrantyStatus" class="clsLabel" runat="server" visible="<%# mReceiptCumInvoice.TransTypeID = 10 %>">Warranty Status</span>
																</td>
																<td>
																	<asp:DropDownList ID="cmbWarrantyStatus" runat="server" CssClass="clsTextBoxTagSearchCombo"
																		Visible="<%# mReceiptCumInvoice.TransTypeID = 10 %>" DataTextField="Name" DataValueField="ID"
																		SelectedValue="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyApplicableStatus %>"
																		Width="200px">
																	</asp:DropDownList>
																</td>
															</tr>
														</table>
													</fieldset>
												</ContentTemplate>
											</asp:UpdatePanel>
										</asp:Panel>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<fieldset id="Fieldset16" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
							class="clsFieldSet">
							<legend class="clsFieldSet1"><b>Receiving Information</b></legend>
							<table width="100%">
								<tr>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlReceivingInformation" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="Fieldset5" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
													class="clsFieldSet">
													<table width="100%">
														<tr>
															<td>&nbsp;
															</td>
															<td>
																<span id="spnPartStatus" class="clsLabelAuto">Part Status</span>
															</td>
															<td>
																<table>
																	<tr>
																		<td>
																			<asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchCombo"
																				DataTextField="Name" DataValueField="ID" SelectedValue="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID %>"
																				Width="200px" AutoPostBack="True">
																			</asp:DropDownList>
																		</td>
																		<td>
																			<asp:ImageButton ID="ImgPartType" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																				CausesValidation="False" Style="margin-top: -1px" Width="24px" ToolTip="Click to Add New Part Type"></asp:ImageButton>
																		</td>
																		<td>
																			<asp:Label ID="lblColor" runat="server" CssClass="clsColorLabel" Style="margin-top: -1px"></asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="lblPartStatus" runat="server" CssClass="clsLabelHeader"></asp:Label>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
														<tr>
															<td>
																<span id="spnQtyStar" class="clsLabelStar">*</span>&nbsp;
															</td>
															<td>
																<span id="spnQty" class="clsLabel">Qty.</span> &nbsp;
															</td>
															<td>
																<asp:UpdatePanel runat="server" ID="upnlQuantity" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:TextBox ID="TextBox1" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																			MaxLength="9" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty %>"
																			ToolTip="Enter Quantity." Width="50px" ReadOnly="true" BackColor="Gainsboro"
																			Style="display: none;"></asp:TextBox>
																		<asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																			MaxLength="9" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty %>"
																			ToolTip="Enter Quantity." Width="50px" AutoPostBack='<%# AppSettings("ClientCode") = "BA" %>'></asp:TextBox>
																		<asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchCombo"
																			DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" SelectedValue="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID %>"
																			Width="140px" Enabled="False">
																		</asp:DropDownList>
																		&nbsp;
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label runat="server" ID="lblReleaseNoteNoStar" CssClass="clsLabelStar" Visible="<%$AppSettings:ReleaseNoteNoRequire%>">*</asp:Label>
															</td>
															<td>
																<span id="lblReleaseNoteNo" class="clsLabel">Rele. Note No.</span>&nbsp;
															</td>
															<td>
																<asp:TextBox ID="txtReleaseNote" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
																	Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo %>"
																	ToolTip="Enter Release Note No." Width="265px"></asp:TextBox>
																<asp:TextBox ID="txtReleaseNoteDate" runat="server" CssClass="clsTextBoxTagSearch"
																	Width="100px" AutoPostBack="true" ClientIDMode="Static" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDateFormatted %>"></asp:TextBox>
																<cc2:CalendarExtender ID="txtReleaseNoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReleaseNoteDate"></cc2:CalendarExtender>
																<cc2:TextBoxWatermarkExtender TargetControlID="txtReleaseNoteDate" ID="txtReleaseNoteDateWatermarkExtender"
																	runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																&nbsp;
															</td>
														</tr>
														<tr>
															<td>
																<span id="spnStoreStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="spnStore" class="clsLabelAuto">Store</span> &nbsp;
															</td>
															<td>
																<asp:UpdatePanel runat="server" ID="upnlStore" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchCombo"
																			DataTextField="LocationStore" AutoPostBack="true" DataValueField="ID" Enabled='<%#Not Session("Enable") = True %>'
																			SelectedValue="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID %>">
																		</asp:DropDownList>
																		&nbsp;
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="lblFaultFoundStar" runat="server" Text="*" CssClass="clsLabelStar"
																	Visible='<%#IIf(mReceiptCumInvoice.TransTypeID = 10 And AppSettings("ClientCode") = "BA", True, False) %>'></asp:Label>
															</td>
															<td>
																<asp:Label ID="lblFaultFound" runat="server" Text="Fault found" CssClass="clsLabel"
																	Visible='<%#IIf(mReceiptCumInvoice.TransTypeID = 10 And AppSettings("ClientCode") = "BA", True, False) %>'></asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbFaultFound" runat="server" CssClass="clsTextBoxTagSearchCombo"
																	SelectedValue="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FaultFound %>"
																	Width="200px" Visible='<%# iif(mReceiptCumInvoice.TransTypeID = 10 And AppSettings("ClientCode") = "BA", True, False) %>'>
																	<asp:ListItem Value="0">(SELECT)</asp:ListItem>
																	<asp:ListItem Value="1">Yes</asp:ListItem>
																	<asp:ListItem Value="2">No</asp:ListItem>
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td></td>
															<td colspan="2" align="left">
																<asp:UpdatePanel runat="server" ID="upnlRateValues" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table>
																			<tr>
																				<td>
																					<span id="lblRate" class="clsLabelAuto">Rate</span>&nbsp;
																				</td>
																				<td>
																					<table style="margin-top: -1px;">
																						<tr>
																							<td>
																								<asp:TextBox ID="txtCRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																									MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate %>"
																									ToolTip="Enter Rate" Width="150px" ReadOnly="true" BackColor="Gainsboro" Style="display: none; margin-left: 12px"></asp:TextBox>
																								<asp:TextBox ID="txtDisplayCRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																									MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate %>"
																									ToolTip="Enter Rate" Width="150px" AutoPostBack='<%# AppSettings("ClientCode") <> "BA" %>'
																									Style="margin-left: 20px"></asp:TextBox>
																							</td>
																							<td>
																								<asp:TextBox ID="txtRateCurrency" runat="server" CssClass="clsTextBoxTagSearchSmall"
																									ReadOnly="True" BackColor="#E0E0E0" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency %>">
																								</asp:TextBox>
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<asp:Label ID="lblOtherCharges" runat="server" CssClass="clsLabelAuto" Visible="False">Oth. Charges</asp:Label>
																				</td>
																				<td>
																					<asp:TextBox ID="txtCOtherCharges" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Style="margin-left: 20px" MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges %>"
																						ToolTip="Enter Other Charges" Visible="False" Width="150px"></asp:TextBox>
																				</td>
																				<td></td>
																			</tr>
																			<tr>
																				<td>
																					<asp:Label ID="lblAmount" runat="server" CssClass="clsLabelAuto" Visible="<%# mReceiptCumInvoice.TransTypeID <> 10 %>">Amount</asp:Label>
																				</td>
																				<td colspan="2">
																					<asp:TextBox ID="txtCAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount %>"
																						Width="150px" ReadOnly="true" BackColor="Gainsboro" Style="display: none; margin-left: 22px"></asp:TextBox>
																					<asp:TextBox ID="txtDisplayCAmount" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Style="margin-left: 20px" MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount %>"
																						Width="150px" AutoPostBack='<%# AppSettings("ClientCode") = "BA" %>'></asp:TextBox>
																				</td>
																			</tr>
																		</table>

																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td></td>
															<td colspan="2">
																<%-- GRO  put here--%>
																<asp:Panel runat="server" ID="pnlGRORate" Style="width: auto;" Visible="<%# (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) %>">
																	<asp:UpdatePanel runat="server" ID="upnlGRORate" UpdateMode="Conditional">
																		<ContentTemplate>
																			<fieldset id="Fieldset3" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
																				class="clsFieldSet">
																				<%--<legend><b>GRO Expense</b></legend>--%>
																				<table style="margin-top: -8px; margin-left: -5px;">
																					<tr>
																						<td>
																							<span id="lblGRORate" class="clsLabel">GRO Rate</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtGROCRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																								MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate %>"
																								ToolTip="Enter GRO Rate" Visible="<%# (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) %>"
																								Width="150px" Style="margin-top: -5px; margin-left: 43px"></asp:TextBox>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<span id="lblGROAmount" class="clsLabelAuto">Amount</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtGROCAmount" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																								MaxLength="12" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount %>"
																								Visible="<%# (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) %>"
																								Width="150px" Style="margin-left: 43px"></asp:TextBox>
																						</td>
																					</tr>
																				</table>
																			</fieldset>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</asp:Panel>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlReceivingInformation1" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="Fieldset6" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
													class="clsFieldSet">
													<table>
														<tr>
															<td>
																<asp:Label ID="lblBarcodeNo" runat="server" Visible="<%$AppSettings:Barcode%>" CssClass="clsLabel">Barcode No.</asp:Label>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																	Style="margin-left: 34px" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BarcodeNo %>"
																	Visible="False">
																</asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<span id="lblSerialNo" class="clsLabel">Serial No.</span>
															</td>
															<td>
																<asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																	Style="margin-left: 34px" AutoPostBack="true" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo %>"
																	ToolTip="Enter Serial No.">
																</asp:TextBox>
																<asp:CustomValidator ID="cvSerialNo" runat="server" ControlToValidate="txtSerialNo"
																	CssClass="clsLabelAuto" Display="None" ErrorMessage="Serial No Required. " OnServerValidate="CustomValidate"></asp:CustomValidator>
															</td>
															<td></td>
														</tr>
														<tr>
															<td>
																<span id="lblBinLocation" class="clsLabel">Bin Location</span>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																	Style="margin-left: 34px" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Location %>"
																	ToolTip="Enter Location of the Store.">
																</asp:TextBox>
																<asp:CustomValidator ID="cvLocation" runat="server" ControlToValidate="txtLocation"
																	CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 50."
																	OnServerValidate="CustomValidate"></asp:CustomValidator>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="lblBatchNo" runat="server" CssClass="clsLabel">Batch No.</asp:Label>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtBatchNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																	Style="margin-left: 34px" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo %>"
																	ToolTip="Enter Batch No. for an Part">
																</asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="lblCodeNo" runat="server" CssClass="clsLabel" Visible="false">Code No.</asp:Label>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtCodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="20"
																	Style="margin-left: 33px" Visible="false" ToolTip="Code No." Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo %>">
																</asp:TextBox>
															</td>
														</tr>
														<tr>
															<td colspan="3">
																<asp:UpdatePanel runat="server" ID="upnlEffectiveRate" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
																			class="clsFieldSet">
																			<table style="margin-top: -3px; margin-left: -5px;">
																				<tr>
																					<td>
																						<span id="Span2" class="clsLabelAuto">Effective Rate</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtCEffectiveRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate %>"
																							Width="150px" BackColor="#E0E0E0" ReadOnly="True" Style="display: none;"></asp:TextBox>
																						<asp:TextBox ID="txtDisplayCEffectiveRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCEffRate %>"
																							Width="150px" BackColor="#E0E0E0" ReadOnly="True" Style="margin-left: 9px"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<span id="Span3" class="clsLabelAuto">Commercial Rate</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtCommercialRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate %>"
																							Width="150px" ToolTip="Enter Commercial Rate" Style="display: none; margin-left: 15px"></asp:TextBox>
																						<asp:TextBox ID="txtDisplayCommercialRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							MaxLength="12" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCCommercialRate %>"
																							Width="150px" ToolTip="Enter Commercial Rate" AutoPostBack="true" Style="margin-left: 9px"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<tr>
																<td colspan="3">
																	<asp:Panel runat="server" ID="pnlGROEffectiveRate" Style="width: auto;" Visible="<%# (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) %>">
																		<asp:UpdatePanel runat="server" ID="upnlGROEffectiveRate" UpdateMode="Conditional">
																			<ContentTemplate>
																				<fieldset id="Fieldset4" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
																					class="clsFieldSet">
																					<table style="margin-top: -8px; margin-left: -5px;">
																						<tr>
																							<td>
																								<span id="Span5" class="clsLabelAuto">GRO Effective Rate</span>
																							</td>
																							<td>
																								<asp:TextBox ID="TextBox4" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																									MaxLength="12" ReadOnly="True" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCEffRate %>"
																									Visible="<%# (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) %>"
																									Width="150px"></asp:TextBox>
																							</td>
																						</tr>
																					</table>
																				</fieldset>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</asp:Panel>
																</td>
															</tr>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:Label ID="lblValues" runat="server" CssClass="clsLabelHeader" Visible="false">Values</asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:UpdatePanel ID="upnlTabDetails" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<cc2:TabContainer ID="tabReceiptDetailsContainer" runat="server" class="clstablelistin"
									Visible="true">
									<cc2:TabPanel ID="tabExpiryDetails" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Expiry(s)" ID="lblExpiry"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="pnlExpiryDetails" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="upnlExpiryInformation" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9500; border-style: none;"
															class="clsFieldSet">
															<table>

																<tr>
																	<td valign="top" colspan="4">
																		<asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date should be Later to Start Date."
																			OnServerValidate="customvalidate"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvExpiryDate" runat="server" ControlToValidate="txtExpiryDate"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date Should be Later to Start Date."
																			OnServerValidate="CustomValidate"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvcureqtrs" runat="server" ControlToValidate="txtCureQtrs"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvCureYrs" runat="server" ControlToValidate="txtCureYear"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvExpQtrs" runat="server" ControlToValidate="txtExpQrts"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvExpYrs" runat="server" ControlToValidate="txtExpYear"
																			CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
																	</td>
																</tr>

																<tr>
																	<td colspan="4">
																		<asp:Label ID="lblExpPeriod" runat="server" CssClass="clsLabelAuto" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryPeriod %>">
																		</asp:Label>
																	</td>
																</tr>

																<tr>
																	<td colspan="4">
																		<table id="Table4" border="0" cellpadding="0" cellspacing="0" runat="server" visible='<%#IIf(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth = 0 Or (AppSettings("ClientCode") = "IND"), True, False) %>'>
																			<tr>
																				<td>
																					<span id="lblOthers" class="clsLabel">Others</span>&nbsp;
																				</td>
																				<td>
																					<asp:CheckBox ID="chkIsExpiryNA" runat="server" AutoPostBack="True" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryNA %>"
																						CssClass="clsCheckBox" Text="N/A" />
																					<asp:CheckBox ID="chkIsExpiryUnlimited" runat="server" AutoPostBack="True" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryUnlimited %>"
																						CssClass="clsCheckBox" Text="Unlimited" />
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>

																<tr>
																	<td>
																		<span id="lblStartDate" class="clsLabel">Cure Date</span>&nbsp;
																	</td>
																	<td>
																		<asp:TextBox ID="txtStartDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
																			AutoPostBack="true" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDateFormatted %>"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="txtStartDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																	<td>&nbsp; <span id="lstExpiryDate" class="clsLabel">Expiry Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtExpiryDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
																			Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDateFormatted %>"
																			AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtExpiryDate" ID="txtExpiryDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																</tr>

																<tr>
																	<td colspan="4">
																		<table id="Table5" border="0" cellpadding="0" cellspacing="0" runat="server" visible="<%# ((mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth <> 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter <> 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryItem))%>">
																			<tr>
																				<td>
																					<span id="Label3" class="clsLabel">Cure Quarter</span>&nbsp;
																				</td>
																				<td>&nbsp;
                                                                                <asp:TextBox ID="txtCureQtrs" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					Enabled="<%# (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter > 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter = 0) %>"
																					MaxLength="1" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs %>"
																					ToolTip="Enter Quarter." Width="37px"></asp:TextBox>
																					<asp:Label ID="Label5" runat="server">/</asp:Label>
																					<asp:TextBox ID="txtCureYear" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Enabled="<%# (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter > 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryMonth = 0 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryQuarter = 0) %>"
																						MaxLength="4" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear %>"
																						ToolTip="Enter Cure Year." Width="64px"></asp:TextBox>
																				</td>
																				<td>&nbsp; &nbsp; &nbsp;
																				</td>
																				<td>
																					<span id="Label4" class="clsLabel">Expiry Quarter</span>
																				</td>
																				<td>&nbsp;&nbsp;
																				</td>
																				<td>
																					<asp:TextBox ID="txtExpQrts" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						MaxLength="1" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs %>"
																						ToolTip="Enter Expiry Quarter." Width="35px"></asp:TextBox>
																					<asp:Label ID="Label6" runat="server">/</asp:Label>
																					<asp:TextBox ID="txtExpYear" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						MaxLength="4" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear %>"
																						ToolTip="Enter Expiry Year." Width="76px"></asp:TextBox>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>

																<tr>
																	<td>
																		<asp:CheckBox ID="chkIsExpiryItem" runat="server" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryItem %>"
																			CssClass="clsCheckBox" Text="Is Expiry Item" Style="display: none;" />
																	</td>
																</tr>
															</table>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabWarranty" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Warranty(s)" ID="Label1"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="Panel2" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="Fieldset8" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9500; border-style: none"
															class="clsFieldSet">
															<table width="100%">
																<tr>
																	<td colspan="4">
																		<asp:CheckBox ID="chkIsInWarranty" runat="server" AutoPostBack="True" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty %>"
																			CssClass="clsLabelAuto" Text="Under Warranty" TextAlign="Left" />
																		&nbsp; &nbsp; <span id="spnIn" class="clsLabelAuto">In</span> &nbsp; &nbsp;<asp:TextBox
																			ID="txtWarrantyInDays" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																			MaxLength="4" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays %>"
																			Width="30px"></asp:TextBox>
																		<span id="lblDays" class="clsLabelAuto">Days</span>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="Span1" class="clsLabel">Start Date</span>
																		<asp:TextBox ID="txtWarrantyStartDate" runat="server" CssClass="clsTextBoxTagSearch"
																			Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDateFormatted %>"
																			onchange="ValidateDateText(this,'Date_watermarkextender','false');" Width="100px"
																			AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtWarrantyStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyStartDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtWarrantyStartDate" ID="txtWarrantyStartDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																		<span id="lblExpiryDate1" class="clsLabel">End Date</span>&nbsp;&nbsp;
                                                                    <asp:TextBox ID="txtWarrantyExpiryDate" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDateFormatted %>"
																		Enabled="false" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
																		Width="100px" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtWarrantyExpiryDate_CalendarExtender" runat="server"
																			CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyExpiryDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtWarrantyExpiryDate" ID="txtWarrantyExpiryDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																</tr>
																<tr>
																	<td colspan="4">
																		<asp:CheckBox ID="chkIsTransitDamage" runat="server" Checked="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsTransitDamage %>"
																			CssClass="clsLabelAuto" Text="Transit Damage" TextAlign="Left" />
																	</td>
																</tr>
															</table>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabBenchCheck" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Benchcheck(s)/Calibration(s)/Equipment Maintenance(s)"
												ID="Label7"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="pnlCalibrationInfo" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="upnlCalibrationInfo" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="Fieldset11" style="padding: 0px 0px 0px 0px; width: auto; z-index: 9000; margin-left: 5px;">
															<legend class="clsFieldSet1"><b>Benchcheck/Calibration Information</b></legend>
															<table>
																<tr>
																	<td>
																		<span id="Span4" class="clsLabel">Calibration Start Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtCalibrationDoneOnDate" runat="server" CssClass="clsTextBoxTagSearch"
																			Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDateFormatted %>"
																			Width="100px" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtCalibrationDoneOnDate_CalendarExtender" runat="server"
																			CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCalibrationDoneOnDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtCalibrationDoneOnDate" ID="txtCalibrationDoneOnDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="Span12" class="clsLabel">Manufacturing Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtManufacturingDate" runat="server" CssClass="clsTextBoxTagSearch"
																			onchange="ValidateDateText(this,'txtManufacturingDateWatermarkExtender','false');"
																			Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManufacturingDateFormatted %>"
																			Width="100px" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtManufacturingDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtManufacturingDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtManufacturingDate" ID="txtManufacturingDateWatermarkExtender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																</tr>
															</table>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
											<asp:Panel runat="server" ID="pnlConditionCheckInfo" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="upnlConditionCheckInfo" UpdateMode="Conditional">
													<ContentTemplate>
														<table width="100%">
															<tr>
																<td>
																	<fieldset id="Fieldset15" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;">
																		<legend class="clsFieldSet1"><b>Equipment Maintenance Information</b></legend>
																		<table>
																			<tr>
																				<td>
																					<asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtServicedInspectedDoneOnDate"
																						Display="None" ErrorMessage="Serviced/Inspected date Required" OnServerValidate="CustomValidate"
																						CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="Span14" class="clsLabel">Add Start Date by click on + button</span>
																					<asp:TextBox ID="txtServicedInspectedDoneOnDate" runat="server" CssClass="clsTextBoxTagSearch"
																						Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServicedInspectedDoneOnDateFormatted %>"
																						Width="100px" AutoPostBack="true" ClientIDMode="Static" Style="display: none;"></asp:TextBox>
																					<cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
																						Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtServicedInspectedDoneOnDate"></cc2:CalendarExtender>
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtServicedInspectedDoneOnDate" ID="TextBoxWatermarkExtender1"
																						runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																				</td>
																				<td>
																					<asp:ImageButton ID="imgbtnReceiptItemServiceInspection" runat="server" ImageUrl="~/images/plus1.png"
																						Height="22px" Width="24px" ToolTip="Click To Add Start Date" CausesValidation="true"
																						Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" />
																				</td>
																			</tr>
																		</table>
																	</fieldset>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabExcessQty" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Excess,Short,Rejected Item(s)" ID="lblExcessHeader"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlExcessQty" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td>
																<span class="clsLabelAuto">Excess Qty.</span>
															</td>
															<td>
																<asp:TextBox ID="txtExcessQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	AutoPostBack="true" ClientIDMode="Static" MaxLength="9" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExcessQty %>"
																	ToolTip="Enter Excess Quantity." Width="50px"></asp:TextBox>
															</td>
															<td>
																<span class="clsLabelAuto">Short Qty.</span>
															</td>
															<td>
																<asp:TextBox ID="txtShortQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	AutoPostBack="true" ClientIDMode="Static" MaxLength="9" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ShortQty %>"
																	ToolTip="Enter Short Quantity." Width="50px"></asp:TextBox>
															</td>
															<td>
																<span class="clsLabelAuto">Rejected Qty.</span>
															</td>
															<td>
																<asp:TextBox ID="txtRejectedQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" ClientIDMode="Static" MaxLength="9"
																	Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RejectedQty %>"
																	ToolTip="Enter Rejected Quantity." Width="50px"></asp:TextBox>
															</td>
															<%--'Added By Shital 07-Sep-2016--%>
															<td>
																<asp:CheckBox ID="chkAirworthiness" runat="server" CssClass="clsLabelAuto" Text="Airworthiness Inspection Performed"
																	Visible="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss %>"
																	TextAlign="Left"></asp:CheckBox>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabSinceOH" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Since New/Since Overhaul" ID="lblSinceOH"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="Panel5" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="upnlTSNTSOValues" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:GridView ID="dgPeriods" runat="server" CssClass="clsGridNewStyle" AllowPaging="false"
																		AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" CellPadding="5"
																		ForeColor="Black" GridLines="Horizontal" PageSize="5">
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<RowStyle CssClass="clsdgItem" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																		<PagerSettings FirstPageText="First" LastPageText="Last" />
																		<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																		<Columns>
																			<asp:BoundField DataField="Name" HeaderText="Periods"></asp:BoundField>
																			<asp:TemplateField HeaderText="TSN Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtTSNValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "TSNValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "1" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvTSNValue" runat="server" Display="None" ControlToValidate="txtTSNValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>
																			<asp:TemplateField HeaderText="TSOH Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtTSOHValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "TSOValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "1" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvTSOHValue" runat="server" Display="None" ControlToValidate="txtTSOHValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>


																			<asp:TemplateField HeaderText="TSI Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtTSIValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "TSIValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "1" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvTSIValue" runat="server" Display="None" ControlToValidate="txtTSIValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>



																			<asp:TemplateField HeaderText="CSN Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCSNValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CSNValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "3" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvCSNValue" runat="server" Display="None" ControlToValidate="txtCSNValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>

																			<asp:TemplateField HeaderText="CSO Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCSOValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CSOValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "3" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvtxtCSOValue" runat="server" Display="None" ControlToValidate="txtCSOValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>


																			<asp:TemplateField HeaderText="CSI Value">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCSIValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						Width="80px" ToolTip="Enter corresponding Period Value." Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CSIValueFormatted") %>' Visible='<%#  Eval("PeriodID") = "3" %>'>
																					</asp:TextBox>
																					<asp:CustomValidator ID="cvtxtCSIValue" runat="server" Display="None" ControlToValidate="txtCSIValue"
																						OnServerValidate="CustomValidate"></asp:CustomValidator>
																				</ItemTemplate>
																			</asp:TemplateField>


																			<asp:ButtonField CommandName="ForDelete" HeaderText="Remove" Text="Remove" />

																			<asp:BoundField DataField="PeriodID"
																				HeaderText="PeriodID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
																				<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																				<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																			</asp:BoundField>
																		</Columns>
																		<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
																		<SortedAscendingCellStyle BackColor="#F7F7F7" />
																		<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
																		<SortedDescendingCellStyle BackColor="#E5E5E5" />
																		<SortedDescendingHeaderStyle BackColor="#242121" />
																	</asp:GridView>
																</td>
																<td valign="top">
																	<asp:ImageButton ID="ImgAddPeroid" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																		Style="margin-top: 10px" Width="24px" ToolTip="Click to Add New Peroid" CausesValidation="false"></asp:ImageButton>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabKitList" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Kit Item(s) List" ID="lblKitList"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="Panel7" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="upnlReceiptItemKitItems" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="fsReceiptItemKitItems" style="padding: 0px 4px 0px 0px; width: auto; border-width: 1px"
															class="clsFieldSet">
															<legend class="clsFieldSet1"><b>Kit Item(s) List</b></legend>
															<table>
																<tr>
																	<td>
																		<asp:GridView ID="dgReceiptItemKitItems" runat="server" CssClass="clsGridNewStyle"
																			AllowPaging="false" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
																			CellPadding="5" ForeColor="Black" GridLines="Horizontal" PageSize="5">
																			<RowStyle CssClass="clsdgItem" />
																			<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																			<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																			<PagerSettings FirstPageText="First" LastPageText="Last" />
																			<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																			<Columns>
																				<asp:BoundField DataField="ItemName" HeaderText="Part No." ItemStyle-Wrap="false" />
																				<asp:BoundField DataField="ItemDescription" HeaderText="Description" ItemStyle-Wrap="false" />
																				<asp:BoundField DataField="KitItemQty" HeaderText="Qty.">
																					<HeaderStyle HorizontalAlign="Right" />
																					<ItemStyle HorizontalAlign="Right" />
																				</asp:BoundField>
																				<asp:TemplateField HeaderText="Serial No.">
																					<ItemTemplate>
																						<asp:TextBox ID="txtSerialNoForItemIDOfKitItem" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNoForItemIDOfKitItem") %>'
																							ToolTip="Enter Serial No.">
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateField>
																				<asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Remark">
																					<ItemTemplate>
																						<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine3_Ajax" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																							MaxLength="250" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' TextMode="MultiLine"
																							ToolTip="Enter remark">
																						</asp:TextBox>
																					</ItemTemplate>
																					<HeaderStyle HorizontalAlign="Left" />
																				</asp:TemplateField>
																				<asp:BoundField DataField="ItemIDFromKitItem" HeaderStyle-CssClass="hideGridColumn"
																					HeaderText="ItemIDFromKitItem" ItemStyle-CssClass="hideGridColumn">
																					<HeaderStyle CssClass="hideGridColumn" />
																					<ItemStyle CssClass="hideGridColumn" />
																				</asp:BoundField>
																				<asp:BoundField DataField="KitItemID" HeaderStyle-CssClass="hideGridColumn" HeaderText="KitItemID"
																					ItemStyle-CssClass="hideGridColumn">
																					<HeaderStyle CssClass="hideGridColumn" />
																					<ItemStyle CssClass="hideGridColumn" />
																				</asp:BoundField>
																			</Columns>
																			<SelectedRowStyle BackColor="ControlDark" />
																		</asp:GridView>
																	</td>
																	<td valign="top">
																		<asp:Button ID="btn" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
																			Visible="false" Height="20px" Text="..." ToolTip="Click to add New Period" />
																	</td>
																</tr>
															</table>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
									<cc2:TabPanel ID="tabRemark" runat="server" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Remark/Note" ID="lblRemark"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="Panel6" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<span id="spnRemark" class="clsLabel">Remark<asp:CustomValidator ID="cvRemark" runat="server"
																		ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" ErrorMessage="Remark Max. Length should be 500."
																		OnServerValidate="CustomValidate"></asp:CustomValidator>
																	</span>
																</td>
																<td>
																	<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" s
																		MaxLength="500" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Remark %>"
																		TextMode="MultiLine" ToolTip="Enter Remark." Width="350px"></asp:TextBox>
																</td>
																<td>
																	<span id="Span6" class="clsLabelAuto">Note<asp:CustomValidator ID="cvNote" runat="server"
																		ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" ErrorMessage="Note Max. Length should be 500."
																		OnServerValidate="CustomValidate"></asp:CustomValidator>
																	</span>
																</td>
																<td>
																	<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																		MaxLength="500" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Note %>"
																		TextMode="MultiLine" ToolTip="Enter Note." Width="350px"></asp:TextBox>
																</td>
															</tr>
															<tr>
															</tr>
															<tr>
																<td>
																	<span id="Span13" class="clsLabelAuto" style="display: none">Previous WorkScope<asp:CustomValidator
																		ID="cvPreviousWorkScope" runat="server" ControlToValidate="txtPreviousWorkScope"
																		CssClass="clsLabelAuto" Display="None" ErrorMessage="Previous Work Scope Max. Length should be 500."
																		OnServerValidate="CustomValidate"></asp:CustomValidator>
																	</span>
																</td>
																<td>
																	<asp:TextBox ID="txtPreviousWorkScope" runat="server" CssClass="clsTextBoxTagSearch"
																		Height="36px" MaxLength="500" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PreviousWorkScope %>"
																		TextMode="MultiLine" ToolTip="Enter Previous Work Scope." Width="250px" Visible="false"></asp:TextBox>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="TabOtherInfo" runat="server" CssClass="clsPanel1" Visible='<%#IIf(AppSettings("ClientCode") = "BAP", True, False) %>'>
										<HeaderTemplate>
											<asp:Label runat="server" Text="Other Info." ID="Label8"></asp:Label>
										</HeaderTemplate>
										<ContentTemplate>
											<asp:Panel runat="server" ID="Panel9" Style="width: auto;">
												<asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
													<ContentTemplate>
														<table>

															<tr>
																<td>&nbsp;
                                                                    <span id="lblCompID" class="clsLabel">Comp. ID</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtCompID" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CompID %>"></asp:TextBox>
																</td>

																<td>
																	<span id="lblHazmatID" class="clsLabel">Hazmat ID</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtHazmatID" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.HazmatID %>"></asp:TextBox>
																</td>
																<td>&nbsp; <span id="lblCertificateNo" class="clsLabel">Certificate No</span>
																</td>
																<td>
																	<asp:TextBox ID="txtCertificateNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertificateNo %>"
																		ClientIDMode="Static"></asp:TextBox>
																</td>

															</tr>

															<tr>

																<td>&nbsp; <span id="lblRevisionNo" class="clsLabel">Revision No</span>
																</td>
																<td>
																	<asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionNo %>"
																		ClientIDMode="Static"></asp:TextBox>
																</td>


																<td>
																	<span id="lblRevisionDate" class="clsLabel">Revision Date</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtRevisionDate" runat="server" CssClass="clsTextBoxTagSearch"
																		Width="100px" ClientIDMode="Static" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RevisionDateFormatted %>"
																		onchange="ValidateDateText(this,'Date_watermarkextender','false');"></asp:TextBox>
																	<cc2:CalendarExtender ID="txtRevisionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevisionDate"></cc2:CalendarExtender>
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtRevisionDate" ID="txtRevisionDateTextBoxWatermarkExtender"
																		runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	&nbsp;
																</td>


																<td>&nbsp;
                                                                    <span id="lblCertifyingRemarks" class="clsLabel">Certifying Remarks</span>
																</td>
																<td>
																	<asp:TextBox ID="txtCertifyingRemarks" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertifyingRemarks %>"
																		ClientIDMode="Static"></asp:TextBox>
																</td>

															</tr>

															<tr>

																<td>&nbsp;
                                                                    <span id="lblWORONo" class="clsLabel">Work Order RO No</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtWORONo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WorkOrderRONo %>"></asp:TextBox>
																</td>



																<td>
																	<span id="lblWCRepVendor" class="clsLabel">Work Card No(Rep. Vendor)</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtWCRepVendor" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WorkCardNoRepVendor %>"></asp:TextBox>
																</td>

																<td>&nbsp;
                                                                    <span id="lblCertificateType" class="clsLabel">Certificate Type</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtCertificateType" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CertificateType %>"></asp:TextBox>
																</td>

															</tr>
															<tr>
																<td>&nbsp;
                                                                    <span id="lblApprovalNo" class="clsLabel">Approval No</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtApprovalNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ApprovalNo %>"></asp:TextBox>
																</td>


																<td>
																	<span id="lblWarehouseNo" class="clsLabel">Warehouse</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtWarehouseNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Warehouse %>"></asp:TextBox>
																</td>

																<td>&nbsp;
                                                                    <span id="lblManfLot" class="clsLabel">Manf. Lot</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtManfLot" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManfLot %>"></asp:TextBox>
																</td>

															</tr>
															<tr>

																<td>&nbsp;
                                                                    <span id="lblInspectedDate" class="clsLabel">Inspected Date</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtInspectedDate" runat="server" CssClass="clsTextBoxTagSearch"
																		Width="100px" ClientIDMode="Static" Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedDateFormatted %>"
																		onchange="ValidateDateText(this,'Date_watermarkextender','false');"></asp:TextBox>
																	<cc2:CalendarExtender ID="txtInspectedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInspectedDate"></cc2:CalendarExtender>
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtInspectedDate" ID="txtInspectedDateTextBoxWatermarkExtender"
																		runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	&nbsp;
																</td>




																<td>
																	<span id="lblInspectedBy" class="clsLabel">Inspected By</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtInspectedBy" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InspectedBy %>"></asp:TextBox>
																</td>



																<td>&nbsp;
                                                                    <span id="lblLastRemovalPosition" class="clsLabel">Last Removal Position</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtLastRemovalPosition" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.LastRemovalPosition %>"></asp:TextBox>
																</td>

															</tr>

															<tr>

																<td>&nbsp;
                                                                    <span id="lblRemovalReason" class="clsLabel">Removal Reason</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtRemovalReason" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovalReason %>"></asp:TextBox>
																</td>



																<td>
																	<span id="lblNHAPartNo" class="clsLabel">Ex-NHA Part No.</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtNHAPartNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.NHAPartNo %>"></asp:TextBox>
																</td>

																<td>&nbsp;
                                                                    <span id="lblNHASerialNo" class="clsLabel">Ex-NHA Serial No.</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtNHASerialNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.NHASerialNo %>"></asp:TextBox>
																</td>

															</tr>
															<tr>

																<td>&nbsp;
                                                                    <span id="lblPackageWONo" class="clsLabel">Package/WO No.</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtPackageWONo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PackageWONo %>"></asp:TextBox>
																</td>



																<td>
																	<span id="lblCR" class="clsLabel">CR</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtCR" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CR %>"></asp:TextBox>
																</td>

																<td>&nbsp;
                                                                    <span id="lblStationWC" class="clsLabel">Station/WC</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtStationWC" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StationWC %>"></asp:TextBox>
																</td>

															</tr>
															<tr>
																<td>&nbsp;
                                                                        <span id="lblRemovalType" class="clsLabel">Removal Type</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtRemovalType" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovalType %>"></asp:TextBox>
																</td>


																<td>
																	<span id="lblRemovedBy" class="clsLabel">Removed By</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtRemovedBy" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RemovedBy %>"></asp:TextBox>
																</td>
																<td>&nbsp;
                                                                    <span id="lblInstallPart" class="clsLabel">Install Part</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtInstallPart" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallPart %>"></asp:TextBox>
																</td>

															</tr>
															<tr>
																<td>&nbsp;
                                                                    <span id="lblInstallSerial" class="clsLabel">Install Serial</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtInstallSerial" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallSerial %>"></asp:TextBox>
																</td>
																<td>
																	<span id="lblInstallBy" class="clsLabel">Install By</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtInstallBy" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InstallBy %>"></asp:TextBox>
																</td>
																<td>&nbsp;
                                                                    <span id="lblDiscrepancyNo" class="clsLabel">Discrepancy No</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtDiscrepancyNo" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DiscrepancyNo %>"></asp:TextBox>
																</td>

															</tr>
															<tr>
																<td>&nbsp;
                                                                        <span id="lblRepeatDiscrepancy" class="clsLabel">Repeat Discrepancy</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtRepeatDiscrepancy" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RepeatDiscrepancy %>"></asp:TextBox>
																</td>


																<td>
																	<span id="lblIncident" class="clsLabel">Incident</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtIncident" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Incident %>"></asp:TextBox>
																</td>


																<td>&nbsp;
                                                                    <span id="lblCausedDelay" class="clsLabel">Caused Delay</span>&nbsp;
																</td>
																<td>
																	<asp:TextBox ID="txtCausedDelay" runat="server" CssClass="clsTextBoxTagSearch"
																		Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CausedDelay %>"></asp:TextBox>
																</td>

															</tr>
															<td>&nbsp;
                                                                      <span id="lblDiscrepancyDescription" class="clsLabel">Discrepancy Description</span>&nbsp;
															</td>
															<td>
																<asp:TextBox ID="txtDiscrepancyDescription" runat="server" CssClass="clsTextBoxTagSearch"
																	Text="<%# mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DiscrepancyDescription %>"></asp:TextBox>
															</td>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
										</ContentTemplate>
									</cc2:TabPanel>
								</cc2:TabContainer>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<fieldset id="Fieldset19" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
							class="clsFieldSet">
							<legend class="clsFieldSet1"><b>File Attachment(s)</b></legend>
							<asp:Panel runat="server" ID="Panel8" Style="width: auto;">
								<asp:Panel runat="server" ID="pnlAttachment" Style="width: auto;">
									<asp:UpdatePanel runat="server" ID="upnlAttachment" UpdateMode="Conditional">
										<ContentTemplate>
											<%--Added by Shital on 25-Jun-2020--%>
											<fieldset id="Fieldset17" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
												class="clsFieldSet">
												<asp:UpdatePanel ID="upnlRCIAttachment" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table width="70%">
															<tr>
																<td style="height: 15px">
																	<asp:UpdatePanel ID="upnldgRCIAttachment" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:GridView ID="dgRCIAttachment" ToolTip="List of File Attachment(s)" runat="server"
																				CssClass="clsGridNewStyle" AutoGenerateColumns="False" DataKeyNames="ID" ShowHeaderWhenEmpty="True"
																				AllowSorting="True" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																				PageSize="5" AllowPaging="False">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																				<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																				<PagerSettings FirstPageText="First" LastPageText="Last" />
																				<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																				<Columns>
																					<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																					<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																					<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																						<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																					</asp:BoundField>
																					<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																						<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="File Name">
																						<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																						<ItemTemplate>
																							<asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
																								ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
																								Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>' Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
																								Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
																						</ItemTemplate>
																						<HeaderStyle HorizontalAlign="Center" />
																						<ItemStyle HorizontalAlign="Center" />
																					</asp:TemplateField>
																					<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
																								CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
																								CausesValidation="false" />
																						</ItemTemplate>
																						<HeaderStyle HorizontalAlign="Center" />
																						<ItemStyle HorizontalAlign="Center" />
																					</asp:TemplateField>
																				</Columns>
																			</asp:GridView>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td valign="top">
																	<asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
																		Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false"
																		Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"></asp:ImageButton>
																	<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
																		CausesValidation="False" Style="display: none;"></asp:Button>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</asp:Panel>
							</asp:Panel>
						</fieldset>
					</td>
				</tr>
				<tr style="height: 0px;">
					<td style="height: 0px;" colspan="2">
						<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
							<ContentTemplate>
								<asp:Button ID="hdnBtnReceiptItemServiceInspection" ClientIDMode="Static" runat="server"
									Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
			</table>
		</div>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
			runat="server">
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

		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
			PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileUploadStateComplete() {
				$("#btnDummyFileUpload").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenFileUploadWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
					//                if (!$.browser.msie) {
					$("#btnDummyFileUpload").click();
					$get("AjaxLoader").style.visibility = "hidden";
					//                }
					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFileUpload(fileattached) {
				var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
				//close File Upload popup window
				FileUpwindow.hide();
				//Free resources
				$("#IFileUpload").attr("src", "JavaScript:''");
				if (fileattached) {
					//call hidden button to set file upload content to object
					$("#hdnBtnFileUpload").click();
				}
			}
		</script>
		<!-- End File Upload Modal Dialog-->
		<!-- Compstatus Modal PopUp -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyCompstatus" Text="Dummy Compstatus" />
		</div>
		<asp:Panel runat="server" ID="pnlPopUp" Style="display: none">
			<div>
				<table class="clstablelistout" id="Table2">
					<tr>
						<td>
							<asp:UpdatePanel runat="server" ID="upnlCompstatus" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="TABLE3" class="clstablelistin">
										<tr>
											<td colspan="4">
												<span id="Label2" class="clstitle1">Component Removal Details</span>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span class="clsLabelAuto">Following part is removed from Aircraft by Maintenance Section.
                                                If its the same part to be received then click on Yes and if its not then click
                                                on No otherwise click on Back to go to previous page. </span>
											</td>
										</tr>
										<tr>
											<td>
												<span id="Span7" class="clsLabelAuto">Part No</span>
											</td>
											<td>
												<asp:TextBox ID="txtCompStatusPart" runat="server" CssClass="clsTextBoxTagSearch"
													ReadOnly="True"></asp:TextBox>
											</td>
											<td>
												<span id="Span8" class="clsLabelAuto">Serial No</span>
											</td>
											<td>
												<asp:TextBox ID="txtCompSerialNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="Span9" class="clsLabelAuto">Description</span>
											</td>
											<td colspan="3">
												<asp:TextBox ID="txtCompDesc" runat="server" BackColor="#E0E0E0" TextMode="MultiLine"
													CssClass="clsTextBoxMultiLine1_Ajax" ReadOnly="True"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="Span10" class="clsLabelAuto">Removed From A/C</span>
											</td>
											<td>
												<asp:TextBox ID="txtCompRegNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
													BackColor="Gainsboro"></asp:TextBox>
											</td>
											<td>
												<span id="Span11" class="clsLabelAuto">Removed Date</span>
											</td>
											<td>
												<asp:TextBox ID="txtCompRemoveDate" runat="server" CssClass="clsTextBoxTagSearch"
													ReadOnly="True" BackColor="Gainsboro"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td colspan="4" align="right">
												<table>
													<tr>
														<td>
															<asp:Button ID="btnSelectCompStatus" runat="server" CssClass="clsButton" ToolTip="Click to select Maint. Removal Entry"
																Text="Yes"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnNoCompStatus" runat="server" CssClass="clsButton" ToolTip="Click to skip Maint. Removal Entry"
																Text="No"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnCloseCompStatus" runat="server" CssClass="clsButton" ToolTip="Click to Close"
																Text="Back" CausesValidation="False"></asp:Button>
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
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="lnkCompStatus_ModalPopupExtender" runat="server" TargetControlID="btnDummyCompstatus"
			PopupControlID="pnlPopUp" BackgroundCssClass="clsModalPopupBG" BehaviorID="ModalBehaviourID">
		</cc2:ModalPopupExtender>
		<!-- Receipt Item Service Inspection Pop pup-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyReceiptItemServiceInspection" />
		</div>
		<asp:Panel runat="server" ID="pnlReceiptItemServiceInspection" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IReceiptItemServiceInspection" allowtransparency="true" frameborder="0"
				height="100%" width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupReceiptItemServiceInspection" runat="server"
			TargetControlID="btnDummyReceiptItemServiceInspection" PopupControlID="pnlReceiptItemServiceInspection"
			BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameReceiptItemServiceInspectionStateComplete() {
				$("#btnDummyReceiptItemServiceInspection").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}
			function AddReceiptItemServiceInspection() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IReceiptItemServiceInspection").attr("src", "wfReceiptItemServiceInspection_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyReceiptItemServiceInspection").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForReceiptItemServiceInspection() {
				var ReceiptItemServiceInspectionwindow = $find("<%=mdlPopupReceiptItemServiceInspection.ClientID %>");
				//close Ass Insp Maint Done By Emp popup window
				ReceiptItemServiceInspectionwindow.hide();
				//Free resources
				$("#IReceiptItemServiceInspection").attr("src", "JavaScript:''");
				$("#hdnBtnReceiptItemServiceInspection").click();
			}
		</script>
		<!-- End -->
		<%-- Open period--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlAddPeriod" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IframeAddPeriod" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupTaskCardStep" runat="server" TargetControlID="btnDummyAddPeriod"
			PopupControlID="pnlAddPeriod" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameStateComplete() {
				$("#btnDummyAddPeriod").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenAddPeriodWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeAddPeriod").attr("src", "wfSelectPeriod_Ajax.aspx?Type=pup");

					//                if (!$.browser.msie) {
					$("#btnDummyAddPeriod").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					//}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForAddPeriod() {
				var TaskCardStepwindow = $find("<%=mdlPopupTaskCardStep.ClientID %>");
				//close Task Card Step popup window
				TaskCardStepwindow.hide();
				//           release resources
				$("#IframeAddPeriod").attr("src", "JavaScript:''");
				//call image button
				$("#hdnAddPeriod").click();
			}
		</script>
		<!-- End-->
	</form>
	<script type="text/javascript">
		//Date validations;
		function ValidateDateText(elem, extenderid, TobeReset) {

			var datevalue = $(elem).val();
			var resetTodaysDate = TobeReset;
			var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
</body>
</html>

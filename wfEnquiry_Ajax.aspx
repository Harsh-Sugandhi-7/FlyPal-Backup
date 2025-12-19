<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEnquiry_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfEnquiry_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Enquiry Details</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

	<style type="text/css">
		.ComboBoxPadding {
			margin-left: 12px;
		}
	</style>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>

</head>
<body>
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblMain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
						<table id="tblinner" class="clsTablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server"
															CssClass="clsFormHeader">Enquiry Details [New]</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlValidationSAummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
												HeaderText="Fill Up The Following Fields" ValidationGroup="1" />
											<asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Select Enquiry Date." ControlToValidate="txtEnquiryDate" Display="None"
												ValidationGroup="1" />
											<asp:CustomValidator ID="cvCustomer" runat="server" ErrorMessage="Select Customer from the list."
												ControlToValidate="cmbCustomer" Display="None" ClientValidationFunction="valiDateCustomer"
												ValidationGroup="1" />
											<asp:CustomValidator ID="cvVendor" runat="server" ErrorMessage="Select Customer from the list"
												ControlToValidate="cmbVendorList" Display="None"
												ClientValidationFunction="validateVendorForSalesEnquiry"
												ValidationGroup="1" />
											<script type="text/javascript">
												function valiDateCustomer(source, args) {
													var status = $("#chkIsCustomer").is(":checked");
													if (status == true) {
														var SelectedCustIndex = $get("cmbCustomer").selectedIndex;
														if (SelectedCustIndex == 0) {
															args.IsValid = false;
															return;
														}

													}
												}

												function validateVendorForSalesEnquiry(source, args) {
													var IsSalesEnq = "<%# Enquiry.TransTypeID=1 %>";
													if (IsSalesEnq) {
														var SelectedCustIndex = $get("cmbVendorList").selectedIndex;
														if (SelectedCustIndex == 0) {
															args.IsValid = false;
															return;
														}
													}

												}
											</script>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# Enquiry.StatusName %>">
											</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<table id="Table2" border="0" width="100%">

										<tr>
											<td valign="top" colspan="2">
												<asp:UpdatePanel ID="upnlEnquiryDetails" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="fdsEnquiryDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
															<legend id="ledEnquiryDetails" class="clsLabelHeader">Enquiry Details</legend>
															<asp:Panel ID="pnlEnquiryDetails" runat="server" CssClass="clspanel1">
																<table id="Table13" class="clsTable1" border="0">
																	<tr>
																		<td>
																			<span id="lblDateStar1" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblDate" class="clsLabelAuto">Date</span>
																		</td>
																		<td>
																			<asp:TextBox runat="server" ID="txtEnquiryDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																				onchange="ValidateDateText(this,'EnquiryDate_watermarkextender');" AutoPostBack="true"></asp:TextBox>
																			<cc2:CalendarExtender ID="txtEnquiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																				Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEnquiryDate"></cc2:CalendarExtender>
																			<cc2:TextBoxWatermarkExtender TargetControlID="txtEnquiryDate" ID="EnquiryDate_watermarkextender"
																				ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																				WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<span id="lblStarEnquiryNo" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblNo" class="clsLabelAuto">No.</span>
																		</td>
																		<td valign="top">
																			<table id="Table3" cellspacing="0" cellpadding="0">
																				<tr>
																					<td>
																						<asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# Enquiry.Text %>"
																							MaxLength="25" ToolTip="Enter Enquiry text">
																						</asp:TextBox>
																						<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
																							DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" CompletionInterval="1"
																							ServicePath="wfEnquiry_Ajax.aspx" ServiceMethod="GetTextList" TargetControlID="txtText"
																							UseContextKey="false" ContextKey="" CompletionListCssClass="ac_results_Main"
																							CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
																							OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
																							OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
																						</cc2:AutoCompleteExtender>
																					</td>
																					<td>
																						<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# Enquiry.No %>"
																							MaxLength="8" ToolTip="Enter Enquiry No.">
																						</asp:TextBox>
																					</td>
																					<td></td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblIntOrderNo" runat="server" CssClass="clsLabelAuto" Visible="<%# Enquiry.TransTypeID=1 %>">Source</asp:Label>
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbSource" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="<%# Enquiry.TransTypeID=1 %>"
																				DataTextField="Name" DataValueField="ID" SelectedValue="<%# Enquiry.EnquirySourceID %>">
																			</asp:DropDownList>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td colspan="2">
																			<asp:CheckBox ID="chkIsCustomer" runat="server" CssClass="clsCheckBox" Text="If Enquiry is on behalf of Customer"
																				Visible="<%# Not(Enquiry.TransTypeID=1) %>" Checked="<%# Enquiry.IsCustomer %>"
																				AutoPostBack="True"></asp:CheckBox>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelAuto">Customer</asp:Label>
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="Name"
																				DataValueField="ID" SelectedValue="<%# Enquiry.CustomerID %>" Enabled="False">
																			</asp:DropDownList>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td valign="top">
												<asp:UpdatePanel ID="upnlVendorDetails" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<fieldset id="fdsVendorDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
															<legend id="lblVendorDetail" class="clsLabelHeader" runat="server">Vendor Details</legend>
															<asp:Panel ID="pnlVendorDetails" runat="server" CssClass="clspanel1">
																<table id="Table5" class="clsTable1" border="0">
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto" Visible="<%# Enquiry.TransTypeID = 1 %>">Name</asp:Label>
																		</td>
																		<td>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td>
																						<asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																							Visible="<%# ((CType(Enquiry.TransTypeID, flypal.Util.Trans) <> FlyPal.Util.Trans.RequestingForQuotation) and (CType(Enquiry.TransTypeID, flypal.Util.Trans) <> FlyPal.Util.Trans.OverHaulRepairEnquiry) and (CType(Enquiry.TransTypeID, flypal.Util.Trans) <> FlyPal.Util.Trans.RentialLeaseEnquiry)) %>"
																							DataTextField="Name" DataValueField="ID" AutoPostBack="true" Enabled="<%# Enquiry.IsNew %>">
																						</asp:DropDownList>
																					</td>
																					<td>
																						<asp:ImageButton ID="btnName" runat="server"
																							ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																							Visible="<%# Enquiry.TransTypeID = 1 %>"
																							ToolTip="Click to Add New Customer" CausesValidation="False" />
																					</td>
																				</tr>
																			</table>
																		</td>
																		<td>
																			<asp:Button ID="btnSuppliers" runat="server" CssClass="clsbtnH clsinfoH1" Text="Select Supplier(s) to send Enquiry."
																				ToolTip="Click to Select Supplier" Visible="<%# Not CType(Enquiry.TransTypeID, flypal.Util.Trans) = FlyPal.Util.Trans.Enquiry %>"
																				CausesValidation="False" Width="234px"></asp:Button>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblAddress" runat="server" CssClass="clsLabelAuto" Visible="<%# Enquiry.TransTypeID = 1 %>">Address</asp:Label>
																		</td>
																		<td colspan="2">
																			<asp:TextBox ID="txtAddress" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# Enquiry.Address %>"
																				Visible="<%# Enquiry.TransTypeID = 1 %>" MaxLength="250" ToolTip="Address" Height="30px"
																				TextMode="MultiLine" BackColor="#E0E0E0" ReadOnly="True">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblCustomerEnqNo" runat="server" CssClass="clsLabelAuto" Enabled="<%# (CType(Enquiry.TransTypeID, Flypal.Util.Trans) <> Flypal.Util.Trans.RequestingForQuotation) %>">Customer Enq. No.</asp:Label>
																		</td>
																		<td colspan="2">
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td>
																						<asp:TextBox ID="txtCustomerEnqNo" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							Text="<%# Enquiry.VendorEnqNo %>" MaxLength="25" ToolTip="Enter Customer Enquiry No."
																							Enabled="<%# (CType(Enquiry.TransTypeID, Flypal.Util.Trans) <> Flypal.Util.Trans.RequestingForQuotation) %>">
																						</asp:TextBox>
																					</td>
																					<td>&nbsp
                                                                                <asp:Label ID="lblCustomerEnqDate" runat="server" CssClass="clsLabelAuto" Enabled="<%# (CType(Enquiry.TransTypeID, Flypal.Util.Trans) <> Flypal.Util.Trans.RequestingForQuotation) %>">Customer Enq. Date</asp:Label>
																					</td>
																					<td>&nbsp
                                                                                <asp:TextBox runat="server" ID="txtCustomerEnqDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																					onchange="ValidateDateText(this,'CustomerEnqDate_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="txtCustomerEnqDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																							Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCustomerEnqDate"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="txtCustomerEnqDate" ID="CustomerEnqDate_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																					</td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td colspan="3">
																			<asp:GridView ID="dgEnqSupplierList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID"
																				Visible="<%# (CType(Enquiry.TransTypeID, flypal.Util.Trans) <> FlyPal.Util.Trans.Enquiry) %>"
																				ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
																				<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																				<PagerStyle CssClass="paging" HorizontalAlign="Right" />
																				<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																				<RowStyle CssClass="clsdgItem"></RowStyle>
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																					<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
																						</ItemTemplate>
																						<ItemStyle Width="40px" />
																					</asp:TemplateField>
																					<asp:BoundField DataField="VendorName" HeaderText="Supplier Name">
																						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																						<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="200px" Wrap="true" />
																					</asp:BoundField>
																					<asp:BoundField DataField="ContactPerson" HeaderText="Contact Person">
																						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																						<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="210px" Wrap="true" />
																					</asp:BoundField>
																					<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
																						<HeaderStyle HorizontalAlign="Left" />
																						<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
																					</asp:ButtonField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<fieldset id="fdsOpeningLine" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
													<legend id="ledOpeningLine" class="clsLabelHeader">Opening Line</legend>
													<asp:TextBox ID="txtOpeningLine" runat="server" CssClass="clsTextBoxTagSearch" Height="27px"
														Text="<%# Enquiry.OpeningLine %>" Visible="<%# Not(Enquiry.TransTypeID=1) %>"
														Width="99%">
													</asp:TextBox>
												</fieldset>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlEnquiryItem" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsEnquiryItem" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledEnquiryItem">
													<table>
														<tr>
															<td>
																<span id="Label2" class="clsLabelHeaderItem">Enquiry Item(s)</span>
															</td>
															<td>
																<table id="Table1" border="0">
																	<tr>
																		<td>
																			<asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
																				<asp:ListItem Value="0">Single Part</asp:ListItem>
																				<asp:ListItem Value="1">Multiple Parts</asp:ListItem>
																			</asp:DropDownList>
																		</td>
																		<td>
																			<asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																				ToolTip="Click to Add Enquiry Item" ValidationGroup="1"></asp:ImageButton>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</legend>
												<table width="100%">
													<tr>
														<td></td>
													</tr>
													<tr>
														<td>
															<asp:GridView ID="dgEnquiryItems" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true"
																AutoGenerateColumns="False">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																<PagerStyle CssClass="paging" HorizontalAlign="Right"></PagerStyle>
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<Columns>
																	<%--0--%>
																	<asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--1--%>
																	<asp:BoundField DataField="ItemName" HeaderText="Part No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--2--%>
																	<asp:BoundField DataField="ItemDescription" HeaderText="Description">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--3--%>
																	<asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--4--%>
																	<asp:TemplateField HeaderText="Qty.">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		<ItemTemplate>
																			<asp:TextBox ID="txtQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'
																				CssClass="clsTextBoxRightAlignQty_Ajax" MaxLength="8">
																			</asp:TextBox>
																			<asp:CustomValidator ID="cvBrokenRules" runat="server" OnServerValidate="CustomValidation"
																				Display="None" ControlToValidate="txtQty" ValidationGroup="1"></asp:CustomValidator>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--5--%>
																	<asp:BoundField DataField="UnitName" HeaderText="Unit">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%--6--%>
																	<asp:TemplateField HeaderText="Priority">
																		<ItemTemplate>
																			<asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																				SelectedValue='<%# DataBinder.Eval(Container.DataItem,"PriorityID") %>' DataValueField="ID"
																				DataTextField="Name" DataSource="<%# mPriorityList %>">
																			</asp:DropDownList>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--7--%>
																	<asp:BoundField DataField="RequisitionNumber" SortExpression="RequisitionNumber"
																		HeaderText="MRN No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--8--%>
																	<asp:BoundField DataField="ModelName" HeaderText="Applicable To"></asp:BoundField>
																	<%--9--%>
																	<asp:TemplateField HeaderText="Required In Days">
																		<ItemTemplate>
																			<asp:TextBox ID="txtReqinDays" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredInDays") %>'
																				CssClass="clsTextBoxRightAlignQty_Ajax">
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--10--%>
																	<asp:TemplateField HeaderText="IPC Reference">
																		<ItemTemplate>
																			<asp:TextBox ID="txtIPCReference" runat="server" CssClass="clsTextBoxDate_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "IPCReference") %>'>
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--11--%>
																	<asp:TemplateField HeaderText="Note">
																		<ItemTemplate>
																			<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxDate_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "Note") %>'>
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--12--%>
																	<asp:TemplateField HeaderText="Remark">
																		<ItemTemplate>
																			<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxDate_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'>
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<%--13--%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																		<ItemTemplate>
																			<%-- <span id="button">Login</span>--%>
																			<div class="dropdown">
																				<div class="dropdownbtn-content">
																					<table id="T1" class="clsGridNew_Ajax">
																						<tr>
																							<td>
																								<asp:ImageButton ID="EditView" runat="server"
																									CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																									CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px"
																									Visible='<%#IIf(Enquiry.StatusID > 1, False, True) %>' />
																							</td>
																							<td>
																								<asp:ImageButton ID="DeleteRecord" runat="server"
																									CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																									CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px"
																									Visible='<%#IIf(Enquiry.StatusID > 1, False, True) %>' />
																							</td>
																						</tr>
																					</table>
																				</div>
																				<asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
																			</div>
																		</ItemTemplate>
																		<HeaderStyle HorizontalAlign="Center" />
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlEnquiryTerm" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsEnquiryTerm" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledEnquiryTerm">
													<table>
														<tr>
															<td>
																<span id="lblOrderTerms" class="clsLabelHeaderItem">Enquiry Term(s)</span>
															</td>
															<td>
																<asp:UpdatePanel ID="upnlAddEnquiryTerm" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table>
																			<tr>
																				<td align="right">
																					<asp:ImageButton ID="btnAddTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																						ToolTip="Click to Add Enquiry Term" ValidationGroup="1"></asp:ImageButton>

																				</td>
																				<td align="right">
																					<asp:Button ID="btnAddSupplierSpecificTerms" runat="server" CssClass="clsButtonLong2"
																						Text="Add Supplier Specific Terms" ToolTip="Click To Add Supplier Specific Terms"
																						ValidationGroup="1" Visible="false"></asp:Button>
																				</td>
																			</tr>
																		</table>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</legend>
												<table>
													<tr>
														<td valign="top">
															<asp:GridView ID="dgEnquiryTerms" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
																ShowHeaderWhenEmpty="true" PageSize="3">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																<PagerStyle CssClass="paging" HorizontalAlign="Right"></PagerStyle>
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<Columns>
																	<asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
																		<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																		<ItemStyle Wrap="true" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderText="Remove">
																		<ItemTemplate>
																			<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CausesValidation="false"
																				CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																		</ItemTemplate>
																		<HeaderStyle HorizontalAlign="Center" />
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>

																</Columns>
															</asp:GridView>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
															ToolTip="Click to Cancel Enquiry"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
															ClientIDMode="Static" ToolTip="Click to Send Mail" Visible="<%# (Enquiry.StatusID = 2) %>"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text="Authorize"
															ToolTip="Click to Authorize Enquiry" ValidationGroup="1"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to save Enquiry"
															ValidationGroup="1"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" Enabled="<%# Not Enquiry.IsNew %>"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to go back to the previous page"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<!--Dummy panel to open modelpopup-->
							<tr style="height: 0px;">
								<td style="height: 0px;">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
										<ContentTemplate>
											<asp:Button ID="hdnimgBtnSupplier" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnimgBtnReqPartList" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnimgBtnEnquiryTerm" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<!--End -->
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

		<div id="ModalPopUp">

			<!-- Term List Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyEnquiryTerm" Text="Dummy Enquiry Term" ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlPopupEnquiryTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
				<iframe id="iPopupEnquiryTerm" frameborder="0" allowtransparency="true" height="100%"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupEnquiryTerm" runat="server" TargetControlID="btnDummyEnquiryTerm"
				PopupControlID="pnlPopupEnquiryTerm" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
				function IFrameEnquiryTermStateComplete() {
					$("#btnDummyEnquiryTerm").click();
					$get("AjaxLoader").style.visibility = "hidden";
				}
				$(document).ready(function () {
					$("#btnAddTerm").live("click", function () {
						try {
							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupEnquiryTerm").attr("src", "wfEnquiryTerm_Ajax.aspx?Type=pup&OpenFrom=3");
							if (!$.browser.msie) {
								$("#btnDummyEnquiryTerm").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

							return false;
						} catch (e) {
							alert(e);
						}


					});
				});
			</script>
			<script type="text/javascript">
				function ParentCallBackFunctionForEnquiryTerm() {
					var EnquiryTermWindow = $find("<%=mdlPopupEnquiryTerm.ClientID %>");
					//close Enquiry Term popup window
					EnquiryTermWindow.hide();
					$("#iPopupEnquiryTerm").attr("src", "JavaScript:''");
					//call Enquiry Term button
					$("#hdnimgBtnEnquiryTerm").click();
				}
			</script>
			<!-- End-->

			<!-- Common Part List Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyCommonPartList" Text="Dummy Common Part List"
					ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlPopupCommonPartList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
				<iframe id="iPopupCommonPartList" frameborder="0" allowtransparency="true" height="100%"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupCommonPartList" runat="server" TargetControlID="btnDummyCommonPartList"
				PopupControlID="pnlPopupCommonPartList" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
				function IFrameCommonPartListStateComplete() {
					var UpdatePanel1 = '<%=upnlValidationSAummary.ClientID%>';
					if (Page_IsValid) {
						$("#btnDummyCommonPartList").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}
					else {
						__doPostBack(UpdatePanel1, '');
						$get("AjaxLoader").style.visibility = "hidden";
					}
				}

				function OpenPartsWindow(ItemsCount, TransDate) {
					var Index = $get("cmbAdd").selectedIndex;
					if (Index == 1) {
						try {

							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupCommonPartList").attr("src", "wfCommonPartList_Ajax.aspx?Type=pup&LookinTypeID=1&Name=&OpenFrom=Enquiry&TransDate=" + TransDate + "&ItemsCount=" + ItemsCount);
							if (!$.browser.msie) {
								$("#btnDummyCommonPartList").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

							return false;
						} catch (e) {
							alert(e);
						}
					}
				}

			</script>
			<script type="text/javascript">
				function ParentCallBackFunctionForCommonPartList() {
					var CommonPartListWindow = $find("<%=mdlPopupCommonPartList.ClientID %>");
					//close Common Part List popup window
					CommonPartListWindow.hide();
					$("#iPopupCommonPartList").attr("src", "JavaScript:''");
					//call ata image button
					$("#hdnimgBtnCommonPartList").click();
				}
			</script>
			<!-- End-->

			<!-- Requisition Part List Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyReqPartList" Text="Dummy Common Part List"
					ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlPopupReqPartList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
				<iframe id="iPopupReqPartList" frameborder="0" allowtransparency="true" height="100%"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupReqPartList" runat="server" TargetControlID="btnDummyReqPartList"
				PopupControlID="pnlPopupReqPartList" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
				function IFrameReqPartListStateComplete() {
					$("#btnDummyReqPartList").click();
					$get("AjaxLoader").style.visibility = "hidden";
				}

				function OpenReqPartsWindow(ItemsCount, TransDate) {
					var Index = $get("cmbAdd").selectedIndex;
					if (Index == 2) {
						try {
							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupReqPartList").attr("src", "wfRequisitionPartList_Ajax.aspx?Type=pup&ListFor=0&TransDate=" + TransDate + "&ItemsCount=" + ItemsCount);
							if (!$.browser.msie) {
								$("#btnDummyReqPartList").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

							return false;
						} catch (e) {
							alert(e);
						}
					}
				}



			</script>
			<script type="text/javascript">
				function ParentCallBackFunctionForReqPartList() {
					var ReqPartListWindow = $find("<%=mdlPopupReqPartList.ClientID %>");
					//close Req Part List popup window
					ReqPartListWindow.hide();
					$("#iPopupReqPartList").attr("src", "JavaScript:''");
					//call Req image button
					$("#hdnimgBtnReqPartList").click();
				}
			</script>
			<!-- End-->

			<!-- Popup For By Mail -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
					scrolling="auto" allowtransparency="true"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
				PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>

			<script type="text/javascript">

				function OpenMailWindow() {

					try {

						$("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
						$("#btnDummyForByMail").click();

						return false;

					} catch (e) {
						console.error("Error Occured, refer the Error Message " + e);
					}

				}

				function ParentCallBackFunctionForSendMail() {

					var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
					//close popup window
					ForByMailwindow.hide();
					//           release resources
					$("#IframeForByMail").attr("src", "JavaScript:''");

				}

				function ParentCallBackFunctionToSendMail() {

					var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
					//close popup window
					ForByMailwindow.hide();
					//           release resources
					$("#IframeForByMail").attr("src", "JavaScript:''");
					//call image button
					$("#hdnimgBtnSendMail").click();

				}

			</script>

			<!---End-->

			<script type="text/javascript">
				$(document).ready(function () {
                  <% Dim mOpenFrom As String = Request.QueryString("Type") %>
                    <% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then %>  
					$('#btnCancel').attr('disabled', 'disabled');
					$('#btnPrint').attr('disabled', 'disabled');
					$('#btnSendMail').attr('disabled', 'disabled');
                <% End if %>  
				});

			</script>

		</div>

		<%--autocomplete css functions--%>
		<script type="text/javascript">
			//bold input value in list...
			function ClientPopulated(source, eventArgs) {
				$("#" + source._element.id).removeClass("ac_loading");
			}
			//Alternate item style
			function ClientShowing(source, eventArgs) {
				$.elements = $(source.get_completionList());
				$.elements.find(".ac_results_li").each(function (i) {
					if (i % 2 == 0) {
						//$(this).addClass("ac_even");
					}
					else {
						$(this).addClass("ac_odd");
					}
				});
			}
			//add loader to textbox
			function ClientPopulating(source, e) {
				$("#" + source._element.id).addClass("ac_loading");
			}
			//remove loader from textbox
			function ClientHiding(source, eventArgs) {
				$("#" + source._element.id).removeClass("ac_loading");
			}
		</script>
		<%--End--%>

		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'true' };
				$.ajax({
					type: "POST",
					url: "DateValidationHandler.ashx",
					//        contentType: "application/json",
					cache: false,
					data: params,
					async: false,
					beforeSend: OnBeforeSend,
					//                beforeSend: function (xhr, settings) {
					//                    $("[id$=processing]").dialog();
					//                },
					success: onSuccess,
					error: onError
				});

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

	<!-- Highlight DropDownList Item Color-->
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			var ddSupplier = document.getElementById("cmbVendorList");
			if (ddSupplier != null) {
				var i = 0;
				if (ddSupplier.disabled == false) {
              <% For Each item1 In mVendorList%>
                <% If item1.NotInUse = "True" Then%>
					ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
					i = i + 1;
             <% Next%>
				}
			}

			var ddCustomer = document.getElementById("cmbCustomer");
			if (ddCustomer != null) {
				if (ddCustomer.disabled == false) {
					var j = 0;
              <% For Each item2 In mCustomerList%>
                <% If item2.NotInUse = "True" Then%>
					ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
					j = j + 1;
             <% Next%>
				}
			}
		});
	</script>
	<!-- End Highlight DropDownList Item Color-->

</body>
</html>

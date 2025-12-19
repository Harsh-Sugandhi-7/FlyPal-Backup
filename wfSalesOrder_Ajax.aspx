<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesOrder_Ajax.aspx.vb" Inherits="Flypal.wfSalesOrder_Ajax" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
<head runat="server">
	<title>Sales Order List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>

	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<link id="MainStyle" type="text/css" rel="stylesheet">

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" id="clientEventHandlersJS">

		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openFilel() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openFile() {
			str = "wfExportToExcel.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

	</script>

</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>

		<table id="tblMain" class="clstablelistout">
			<tr>
				<td width="100%" colspan="2">
					<asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
						<table id="tblinner" class="clsTablelistin">
							<tr>
								<td width="100%" class="clsFormHeader1Newstyle" colspan="2">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Sales Order [New]</asp:Label>
											</td>
											<td colspan="2" align="right">
												<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table4">
															<tr>
																<td>
																	<asp:Button ID="btnCancel" runat="server" Text="Cancel"
																		CssClass="clsbtnH clsinfoH" ToolTip="Click to Cancel the Sales Order"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnAuthorized" runat="server" Text="Authorize"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to Authorize the Sales Order"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnSave" runat="server" Text="Save"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to Save Sales Order"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnPrint" runat="server" Text="Print"
																		CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Sales Order"
																		Enabled="<%# Not mSalesOrder.IsNew %>"></asp:Button>
																</td>
																<td align="right">
																	<asp:Button ID="btnBack" runat="server" Text="Close"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to go back to the previous page"></asp:Button>
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
								<td colspan="2">
									<asp:UpdatePanel ID="upnlValidationSAummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields">
											</asp:ValidationSummary>
											<asp:CustomValidator ID="cvCurrency" runat="server" ErrorMessage="Select Currency from the list."
												ControlToValidate="cmbCurrencyList" Display="None" OnServerValidate="customvalidate">
											</asp:CustomValidator>
											<asp:RequiredFieldValidator ID="rfvFactor" runat="server"
												ErrorMessage="Currency factor must be greater than zero."
												ControlToValidate="txtConversionFactor" Display="None">
											</asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvFactor" runat="server" ErrorMessage="Currency factor must be greater than zero."
												ControlToValidate="txtConversionFactor" Display="None" OnServerValidate="customvalidate">
											</asp:CustomValidator>
											<asp:CustomValidator ID="cvSalesOrder" runat="server" OnServerValidate="CustomValidate"
												ValidationGroup="a" Display="None" ControlToValidate="txtCustRefNo" ErrorMessage="">
											</asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="right">
									<asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
										<ContentTemplate>
									<asp:Label ID="lblStatus" runat="server" Text="<%# mSalesOrder.StatusName %>" CssClass="clsLabelHeader"></asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<asp:UpdatePanel runat="server" ID="upnlSalesOrderDetails" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsOrderDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
												<legend id="ledOrderDetails" class="clsLabelHeader">Sales Order Details</legend>
												<table id="tabDetails" class="clsTable1" border="0">
													<tr>
														<td>
															<asp:Label ID="lblDate1" runat="server" CssClass="clsLabelStar">*</asp:Label>
														</td>
														<td>
															<asp:Label ID="lblDate" runat="server" CssClass="clsLabel">Date</asp:Label>
														</td>
														<td>
															<table cellspacing="0" cellpadding="0">
																<tr>
																	<td>
																		<asp:TextBox runat="server" ID="txtSalesOrderDate" CssClass="clsTextBoxTagSearchDateWOList"
																			CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
																			onchange="ValidateDateText(this,'SalesOrderDate_watermarkextender');"></asp:TextBox>
																		<cc2:CalendarExtender ID="txtSalesOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtSalesOrderDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtSalesOrderDate" ID="SalesOrderDate_watermarkextender"
																			ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																			WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																	</td>
																	<td></td>
																</tr>
															</table>
															<asp:CustomValidator ID="cvSalesOrderDate" runat="server" ErrorMessage="Select Sales Order Date"
																ControlToValidate="txtSalesOrderDate" Display="None" OnServerValidate="CustomValidate">
															</asp:CustomValidator>
															<asp:RequiredFieldValidator ID="rfvSalesOrderDate" runat="server" ErrorMessage="Select SalesOrder Date."
																ControlToValidate="txtSalesOrderDate" Display="None">
															</asp:RequiredFieldValidator>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
														</td>
														<td>
															<asp:Label ID="lblNo" runat="server" CssClass="clsLabel">No.</asp:Label>
														</td>
														<td>
															<table id="Table1" cellspacing="0" cellpadding="0">
																<tr>
																	<td>
																		<asp:TextBox ID="txtText" runat="server" Text="<%# mSalesOrder.Text %>" CssClass="clsTextBoxTagSearch"
																			Height="25px" MaxLength="25" ToolTip="Enter Sales Order Text">
																		</asp:TextBox>
																		&nbsp;
																	</td>
																	<td class="clstablecell">

																		<asp:TextBox ID="txtNo" runat="server" Text="<%# mSalesOrder.No %>" CssClass="clsTextBoxTagSearchSmall"
																			Height="25px" MaxLength="8" ToolTip="Enter Sales Order No.">
																		</asp:TextBox>
																	</td>
																	<td class="clstablecell">
																		<asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxAmd" MaxLength="2"
																			Visible="False"></asp:TextBox>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<asp:Label ID="lblCustRefNo" runat="server" CssClass="clsLabel"> Cust. Ref. No.</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtCustRefNo" runat="server" Text="<%# mSalesOrder.CustomerReferenceNo %>"
																Height="25px" CssClass="clsTextBoxTagSearchSmall" MaxLength="50" ToolTip="Enter Customer Reference No.">
															</asp:TextBox>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td valign="top">
									<asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsSupplierDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
												<legend id="ledSupplierDetails" class="clsLabelHeader">Customer Details</legend>
												<table id="tabSalesOrderDetails" class="clsTable1" border="0">
													<tbody>
														<tr>
															<td>
																<asp:Label ID="lblNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label>
															</td>
															<td></td>
															<td>
																<asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" AutoPostBack="True"
																	SelectedValue="<%# mSalesOrder.VendorID %>" DataValueField="ID" DataTextField="Name"
																	Enabled="<%# mSalesOrder.IsNew %>" Width="327px">
																</asp:DropDownList>
																<asp:CustomValidator ID="cvVendor" runat="server" ErrorMessage="Select Vendor from the list."
																	ControlToValidate="cmbVendorList" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblAddress" runat="server" CssClass="clsLabel">Address</asp:Label>
															</td>
															<td></td>
															<td>
																<asp:TextBox ID="txtAddress" runat="server" Text="<%# mSalesOrder.Address %>" CssClass="clsTextBoxTagSearchMultilineNewstyle" Height="36px"
																	Width="327px" MaxLength="250" ToolTip="Address" ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine">
																</asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="lblCurrencyName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblCurrency" runat="server" CssClass="clsLabel">Currency</asp:Label>
															</td>
															<td></td>
															<td>
																<table>
																	<tr>
																		<td>
																			<asp:DropDownList ID="cmbCurrencyList" runat="server"
																				CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																				SelectedValue="<%# mSalesOrder.CurrencyID %>" DataValueField="ID" DataTextField="Name"
																				Enabled="<%# mSalesOrder.IsNew %>">
																			</asp:DropDownList>
																		</td>
																		<td></td>
																		<td>
																			<asp:Label ID="lblFactorStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="lblConvFactor" runat="server" CssClass="clsLabelAuto">Factor</asp:Label>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mSalesOrder.ConversionFactor %>"
																				Height="25px" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right" MaxLength="9" ToolTip=" Enter Conversion Factor"></asp:TextBox>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
														<tr>
															<td></td>
															<td colspan="2">
																<asp:Label ID="lblRoundOffRequire" runat="server" CssClass="clsLabelAuto">Round Off Required</asp:Label>
															</td>
															<td>
																<asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
																	Checked="<%# mSalesOrder.IsRoundOff %>"></asp:CheckBox>
															</td>
														</tr>
													</tbody>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel runat="server" ID="upnlSalesOrderItems" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsOrderItemDetails" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledOrderItemDetails">
													<table id="Table5" border="0" cellspacing="0">
														<tr>
															<td>
																<asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader" Width="130px">Sales Order Item(s)</asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbAdd" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
																	<asp:ListItem Value="0">Single Part</asp:ListItem>
																	<asp:ListItem Value="1">Multiple Parts</asp:ListItem>
																	<asp:ListItem Value="Quotation">Quotation</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td>
																<asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																	ToolTip="Click to Add Sales Order Item"></asp:ImageButton>
															</td>
														</tr>
													</table>
												</legend>
												<br />
												<table width="100%">
													<asp:GridView ID="dgSalesOrderItems" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
														AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
														<PagerSettings FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<%--0--%>
															<asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
															<%--1--%>
															<asp:BoundField DataField="ItemName" HeaderText="Part #">
																<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																<ItemStyle HorizontalAlign="Left" Wrap="false" />
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="ItemDescription" HeaderText="Description" />
															<%--3--%>
															<asp:BoundField DataField="QuotationNo" HeaderText="Quotation No."></asp:BoundField>
															<%--4--%>
															<asp:BoundField DataField="QuotationDateFormatted" HeaderText="Quotation Date">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--5--%>
															<asp:TemplateField HeaderText="Qty.">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
																<ItemTemplate>
																	<asp:TextBox ID="txtQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>'
																		CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Height="25px" MaxLength="8" AutoPostBack="true" OnTextChanged="AddAttributesForGridControls">
																	</asp:TextBox>
																	<asp:CustomValidator ID="cvBrokenRules" runat="server" ControlToValidate="txtQty"
																		Display="None" OnServerValidate="CustomValidate1">
																	</asp:CustomValidator>
																	<asp:RequiredFieldValidator ID="rfvQty" runat="server" ErrorMessage="Qty. Required"
																		ControlToValidate="txtQty" Display="None">
																	</asp:RequiredFieldValidator>
																</ItemTemplate>
															</asp:TemplateField>
															<%--6--%>
															<asp:BoundField DataField="UnitName" HeaderText="Unit"></asp:BoundField>
															<%--7--%>
															<asp:TemplateField HeaderText="Rate">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
																<ItemTemplate>
																	<asp:TextBox ID="txtRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CRate") %>'
																		Height="25px" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right" MaxLength="12" AutoPostBack="true" OnTextChanged="AddAttributesForGridControls">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateField>
															<%--8--%>
															<asp:TemplateField HeaderText="Other Charges">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
																<ItemTemplate>
																	<asp:TextBox ID="txtOtherCharge" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"COtherCharges") %>'
																		Height="25px" CssClass="clsTextBoxTagSearchSmall" Style="text-align: right" MaxLength="12" AutoPostBack="true" OnTextChanged="AddAttributesForGridControls">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateField>
															<%--9--%>
															<asp:BoundField DataField="CAmount" HeaderText="Amount">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<%--10--%>
															<asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
																<HeaderStyle HorizontalAlign="left"></HeaderStyle>
																<ItemStyle HorizontalAlign="left"></ItemStyle>
															</asp:BoundField>
															<%--11--%>
															<asp:TemplateField HeaderText="CGST(%)">
																<ItemTemplate>
																	<asp:TextBox ID="txtWCGST" runat="server" CssClass="clsTextBoxRightAlignQty" ClientIDMode="Static"
																		Width="40px" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTPercentage") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--12--%>
															<asp:TemplateField HeaderText="CGST Amt.">
																<ItemTemplate>
																	<asp:TextBox ID="txtWCGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty" ReadOnly="true"
																		BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTCAmount") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--13--%>
															<asp:TemplateField HeaderText="SGST(%)">
																<ItemTemplate>
																	<asp:TextBox ID="txtWSGST" runat="server" CssClass="clsTextBoxRightAlignQty" ClientIDMode="Static"
																		ReadOnly="true" BackColor="#E0E0E0" Width="40px" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTPercentage") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--14--%>
															<asp:TemplateField HeaderText="SGST Amt.">
																<ItemTemplate>
																	<asp:TextBox ID="txtWSGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty" ReadOnly="true"
																		BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTCAmount") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--15--%>
															<asp:TemplateField HeaderText="IGST(%)">
																<ItemTemplate>
																	<asp:TextBox ID="txtWIGST" runat="server" CssClass="clsTextBoxRightAlignQty" ClientIDMode="Static"
																		Width="40px" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTPercentage") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--16--%>
															<asp:TemplateField HeaderText="IGST Amt.">
																<ItemTemplate>
																	<asp:TextBox ID="txtWIGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty" ReadOnly="true"
																		BackColor="#E0E0E0" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTCAmount") %>'>
																	</asp:TextBox>
																</ItemTemplate>
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:TemplateField>
															<%--17--%>
															<asp:BoundField DataField="ModelName" HeaderText="Applicable To"></asp:BoundField>
															<%--18--%>
															<asp:TemplateField HeaderText="Note">
																<ItemTemplate>
																	<asp:TextBox ID="txtNote" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Note") %>' TextMode="MultiLine"
																		Height="25px" CssClass="clsTextBoxTagSearch">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateField>
															<%--19--%>
															<asp:TemplateField HeaderText="Remark">
																<ItemTemplate>
																	<asp:TextBox ID="txtRemark" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>' TextMode="MultiLine"
																		Height="25px" CssClass="clsTextBoxTagSearch">
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateField>
															<%--20--%>

															<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																<ItemTemplate>
																	<div class="dropdown">
																		<div id="divd" class="dropdownbtn-content" runat="server">
																			<table id="T1" class="clsGridNew_Ajax">
																				<tr>
																					<td>
																						<asp:ImageButton ID="Edit" runat="server"
																							CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																							CommandName="EditRec"
																							CssClass="actionICNS"
																							ImageUrl="~/images/edit.png"
																							ToolTip="Click to Edit record."
																							Visible='<%#IIf(mSalesOrder.StatusID > 1, False, True) %>' />
																					</td>
																					<td>
																						<asp:ImageButton ID="Delete" runat="server"
																							CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																							CommandName="DeleteRec" class="actionICNS  largerActionICNS"
																							ToolTip="Click to Delete record."
																							ImageUrl="~/images/delete.png"
																							Visible='<%#IIf(mSalesOrder.StatusID > 1, False, True) %>' />
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
														</Columns>
														<SelectedRowStyle BackColor="ControlDark" />
													</asp:GridView>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td valign="top" width="75%">
									<asp:UpdatePanel runat="server" ID="upnlOrderCharge" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsOrderCharges" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledOrderCharges">
													<table id="Table2">
														<tr>
															<td>
																<asp:Label ID="lblChargeDeatails" runat="server" CssClass="clsLabelHeader" Width="130px">
																	Sales Order Charge(s)
																</asp:Label>
															</td>
															<td align="right">
																<asp:ImageButton ID="btnAddCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																	ToolTip="Click to Add Sales Order Charge"></asp:ImageButton>
															</td>
														</tr>
													</table>
												</legend>
												<br />
												<asp:GridView ID="dgChargeList" runat="server" ShowHeaderWhenEmpty="True"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="false">
													<PagerSettings Mode="NextPreviousFirstLast" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
													<AlternatingRowStyle CssClass="alt" />
													<Columns>
														<%--0--%>
														<asp:BoundField DataField="SrNo" HeaderText="Sr.No."></asp:BoundField>
														<%--1--%>
														<asp:BoundField DataField="ChargeName" HeaderText="Charge"></asp:BoundField>
														<%--2--%>
														<asp:BoundField DataField="Percentage" HeaderText="Percentage">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundField>
														<%--3--%>
														<asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundField>
														<%--4--%>
														<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
															ItemStyle-HorizontalAlign="Center">
															<HeaderStyle HorizontalAlign="Center" />
															<ItemStyle HorizontalAlign="Center" />
															<ItemTemplate>
																<div id="dropDownImg" class="dropdown">
																	<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
																		CssClass="clsActionbtn" />
																	<div id="dropdownICN-content" class="dropdownbtn-content">
																		<table id="dropdown-content" class="clsGridNew_Ajax">
																			<tr>
																				<td>
																					<asp:ImageButton ID="Edit" runat="server"
																						CommandArgument='<%# Container.DataItemIndex %>'
																						CommandName="EditRec"
																						CssClass="actionICNS"
																						ImageUrl="~/images/edit.png"
																						ToolTip="Click to Edit record."
																						Visible='<%#IIf(mSalesOrder.StatusID > 1, False, True) %>' />
																				</td>
																				<td>
																					<asp:ImageButton ID="Delete" runat="server"
																						CommandArgument='<%# Container.DataItemIndex %>'
																						CommandName="DeleteRec"
																						CssClass="actionICNS  largerActionICNS"
																						ToolTip="Click to Delete record."
																						ImageUrl="~/images/delete.png"
																						Visible='<%#IIf(mSalesOrder.StatusID > 1, False, True) %>' />
																				</td>
																			</tr>
																		</table>
																	</div>
																</div>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Center" />
															<ItemStyle HorizontalAlign="Center" />
														</asp:TemplateField>
													</Columns>
												</asp:GridView>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td valign="top" align="right" width="25%">
									<asp:UpdatePanel runat="server" ID="upnlGrandTotal" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsTotal" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledOrderTotal">
													<table id="Table2">
														<tr>
															<td>
																<asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">
																	Other Details
																</asp:Label>
															</td>
														</tr>
													</table>
												</legend>
												<table id="Table3" width="100%">
													<tr>
														<td align="right">
															<asp:Label ID="lblGrandTotal" runat="server" CssClass="clsLabel">Total</asp:Label>
														</td>
														<td align="right">
															<asp:TextBox ID="txtCTotal" runat="server"
																Text="<%# mSalesOrder.CTotalAmount %>"
																Height="25px" Width="80px"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right" ToolTip="Total"
																ReadOnly="True" BackColor="#E0E0E0">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td align="right">
															<asp:Label ID="lblTotaolOtherCharges" runat="server" CssClass="clsLabelAuto">Total Other Charges</asp:Label>
														</td>
														<td align="right">
															<asp:TextBox ID="txtCTotalOtherCharge" runat="server"
																Text="<%# mSalesOrder.CTotalCharges %>"
																Height="25px" Width="80px"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right"
																ToolTip="Total Other Charges" ReadOnly="True"
																BackColor="#E0E0E0">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabel">Total CGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalCGST" runat="server"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right"
																Text="<%# mSalesOrder.CTotalCGSTAmount %>"
																Height="25px" Width="80px"
																ReadOnly="True" BackColor="#E0E0E0">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblTotalSGST" runat="server" CssClass="clsLabel">Total SGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalSGST" runat="server"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right"
																Text="<%# mSalesOrder.CTotalSGSTAmount %>"
																Height="25px" Width="80px"
																ReadOnly="True" BackColor="#E0E0E0">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabel">Total IGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalIGST" runat="server"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right"
																Text="<%# mSalesOrder.CTotalIGSTAmount %>"
																Height="25px" Width="80px"
																ReadOnly="True" BackColor="#E0E0E0">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td align="right">
															<asp:Label ID="Label3" runat="server" CssClass="clsLabel">Grand Total</asp:Label>
														</td>
														<td align="right">
															<asp:TextBox ID="txtCGrandTotal" runat="server"
																Text="<%# mSalesOrder.CGrandTotal %>"
																Height="25px" Width="80px"
																CssClass="clsTextBoxTagSearch"
																Style="text-align: right"
																ToolTip="Grand Total" ReadOnly="True"
																BackColor="#E0E0E0">
															</asp:TextBox>
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
									<br />
								</td>
							</tr>
							<tr>
								<td valign="top" colspan="2">
									<asp:UpdatePanel runat="server" ID="upnlSalesOrderTerms" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsSalesOrderTerms" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
												<legend id="ledOrderSalesTerms">
													<table>
														<tr>
															<td align="center">
																<asp:Label ID="lblSalesOrderTerms" runat="server" CssClass="clsLabelHeader" Width="130px">
																	Sales Order Term(s)
																</asp:Label>
															</td>
															<td align="right">
																<asp:ImageButton ID="btnAddTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																	ToolTip="Click to Add Sales Order Term"></asp:ImageButton>
															</td>
															<td align="right">
																<asp:Button ID="btnAddCustomerSpecificTerms" runat="server" CssClass="clsbtnH clsinfoH1"
																	Text="Add Customer Specific Terms" ToolTip="Click To Add Customer Specific Terms"></asp:Button>
															</td>
														</tr>
													</table>
												</legend>
												<br />
												<table width="100%">
													<tr>
														<td valign="top">
															<asp:GridView ID="dgSalesOrderTerms" runat="server" ShowHeaderWhenEmpty="True"
																CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="false">
																<PagerSettings Mode="NextPreviousFirstLast" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																<AlternatingRowStyle CssClass="alt" />

																<Columns>
																	<asp:BoundField DataField="SrNo" HeaderText="Sr.No."></asp:BoundField>
																	<asp:BoundField DataField="Terms" HeaderText="Terms and conditions"></asp:BoundField>
																	<asp:TemplateField HeaderText="Remove">
																		<ItemTemplate>
																			<asp:ImageButton ID="Delete" runat="server"
																				CommandArgument='<%# Container.DataItemIndex %>'
																				CommandName="DeleteRec"
																				class="actionICNS  largerActionICNS"
																				ToolTip="Click to Delete record."
																				ImageUrl="~/images/delete.png"
																				Visible='<%#IIf(mSalesOrder.StatusID > 1, False, True) %>' />
																		</ItemTemplate>
																	</asp:TemplateField>
																</Columns>

																<SelectedRowStyle BackColor="ControlDark" />
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
								<td colspan="2" align="right">
									<asp:UpdatePanel runat="server" ID="upnlHiddenButtons" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="tblHiddenButtons">
												<tr>
													<td>
														<asp:Button ID="hdnBtnSalesCharge" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnimgBtnQuotationList" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnImgBtnSalesOrderTerms" ClientIDMode="Static" runat="server"
															CausesValidation="false" Style="display: none" />
														<asp:Button ID="hdnImgBtnSalesOrderCharges" ClientIDMode="Static" runat="server"
															CausesValidation="false" Style="display: none" />
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

		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySalesCharge" Text="SalesCharge" CausesValidation="false"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlSalesCharge" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeSalesCharge" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupSalesCharge" runat="server" TargetControlID="btnDummySalesCharge"
			PopupControlID="pnlSalesCharge" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameSalesChargeStateComplete() {
				$("#btnDummySalesCharge").click();
				// $get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenSalesChargeWindow() {
				try {

					// $get("AjaxLoader").style.visibility = 'visible';
					$("#IframeSalesCharge").attr("src", "wfSalesOrderCharge.aspx?Type=pup");

					$("#btnDummySalesCharge").click();
					//    $get("AjaxLoader").style.visibility = 'hidden';

					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForSalesCharge() {
				var SalesChargewindow = $find("<%=mdlPopupSalesCharge.ClientID %>");
				//close popup window
				SalesChargewindow.hide();
				//release resources
				$("#IframeSalesCharge").attr("src", "JavaScript:''");
				//call button click
				$("#hdnBtnSalesCharge").click();
			}
		</script>

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
					//$get("AjaxLoader").style.visibility = "hidden";
				}
				else {
					__doPostBack(UpdatePanel1, '');
					// $get("AjaxLoader").style.visibility = "hidden";
				}
			}

			function OpenPartsWindow(ItemsCount, TransDate) {
				var Index = $get("cmbAdd").selectedIndex;
				if (Index == 1) {
					try {

						// $get("AjaxLoader").style.visibility = "visible";
						$("#iPopupCommonPartList").attr("src", "wfCommonPartList_Ajax.aspx?Type=pup&LookinTypeID=1&Name=&OpenFrom=Enquiry&TransDate=" + TransDate + "&ItemsCount=" + ItemsCount);
						if (!$.browser.msie) {
							$("#btnDummyCommonPartList").click();
							//  $get("AjaxLoader").style.visibility = "hidden";
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


		<!-- Quotation List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyQuotationList" Text="Dummy Common Part List"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupQuotationList" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupQuotationList" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupQuotationList" runat="server" TargetControlID="btnDummyQuotationList"
			PopupControlID="pnlPopupQuotationList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameQuotationListStateComplete() {
				var UpdatePanel1 = '<%=upnlValidationSAummary.ClientID%>';
				if (Page_IsValid) {
					$("#btnDummyQuotationList").click();
					//$get("AjaxLoader").style.visibility = "hidden";
				}
				else {
					__doPostBack(UpdatePanel1, '');
					// $get("AjaxLoader").style.visibility = "hidden";
				}
			}

			function OpenQuotesWindow(ItemsCount, TransDate) {
				var Index = $get("cmbAdd").selectedIndex;
				if (Index == 2) {
					try {

						// $get("AjaxLoader").style.visibility = "visible";
						$("#iPopupQuotationList").attr("src", "wfPendingSalesQuotations.aspx?Type=pup");
						if (!$.browser.msie) {
							$("#btnDummyQuotationList").click();
							//  $get("AjaxLoader").style.visibility = "hidden";
						}

						return false;
					} catch (e) {
						alert(e);
					}
				}
			}

		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForQuotationList() {
				var QuotationListWindow = $find("<%=mdlPopupQuotationList.ClientID %>");
				//close Common Part List popup window
				QuotationListWindow.hide();
				$("#iPopupQuotationList").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnQuotationList").click();
			}
		</script>
		<!-- End-->

		<!-- Term Popup Window  btnSalesOrderTermsAdd btnDummyTerm hdnimgBtnTerm pnlPopupTerm iPopupTerm mdlPopupTerm-->

		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySalesOrderTerm" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

		</div>
		<asp:Panel runat="server" ID="pnlPopupSalesOrderTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupSalesOrderTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupSalesOrderTerm" runat="server" TargetControlID="btnDummySalesOrderTerm"
			PopupControlID="pnlPopupSalesOrderTerm" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">

			function IFrameTermStateComplete() {

				$("#btnDummySalesOrderTerm").click();
				$get("AjaxLoader").style.visibility = 'hidden';

			}
			function OpenTermWindow() {
				try {
					$("#iPopupSalesOrderTerm").attr("src", "wfSalesOrderTerm.aspx?Typepup=pup&Type=5");
					if (!$.browser.msie) {
						$("#btnDummySalesOrderTerm").click();
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>

		<script type="text/javascript">
			function ParentCallBackFunctionForTerm() {
				var TermWindow = $find("<%=mdlPopupSalesOrderTerm.ClientID %>");
				//close Term popup window
				TermWindow.hide();
				$("#iPopupSalesOrderTerm").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnSalesOrderTerm").click();
			}
		</script>
		<!-- End-->

	</form>
</body>

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
	});    
</script>
<!-- End Highlight DropDownList Item Color-->
</html>


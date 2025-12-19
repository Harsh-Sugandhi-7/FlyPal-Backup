<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesInvoice_Ajax.aspx.vb"
	Inherits="Flypal.wfSalesInvoice_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Sales Invoice Details</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
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
					<td width="100%" colspan="2">
						<asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Sales Invoice Details [New]</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td colspan="2" align="right">
													<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
														<ContentTemplate>
															<table>
																<tr>
																	<td>
																		<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to Cancel the Sales Invoice"></asp:Button>
																		<asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to authorize Sales Invoice" ValidationGroup="a"></asp:Button>
																		<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to Save Sales Invoice" ValidationGroup="a"></asp:Button>
																		<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to Print Sales Invoice" Enabled="<%# Not mSalesInvoice.IsNew %>"></asp:Button>
																		<asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
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
										<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvInvoiceDate" runat="server" ErrorMessage="Select Invoice Date"
													ControlToValidate="txtInvoiceDate" Display="None" OnServerValidate="CustomValidate"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvOrderDate" runat="server" ErrorMessage="Select Invoice Date."
													ControlToValidate="txtInvoiceDate" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvVendor" runat="server" ErrorMessage="Select Vendor from the list."
													ControlToValidate="cmbVendorList" Display="None" OnServerValidate="CustomValidate"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:CustomValidator ID="cvFactor" runat="server" ErrorMessage="Currency factor must be greater than zero."
													ControlToValidate="txtConversionFactor" Display="None" OnServerValidate="customvalidate"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvFactor" runat="server" ErrorMessage="Currency factor must be greater than zero."
													ControlToValidate="txtConversionFactor" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvCustomer" runat="server" OnServerValidate="CustomValidate"
													ValidationGroup="a" Display="None" ControlToValidate="txtDispatchNo" ErrorMessage=""></asp:CustomValidator>
												<script type="text/javascript">
													function ValidateVendor(source, args) {
														args.IsValid = false;
														var dd = $get("cmbVendorList");
														if (dd.selectedIndex != 0) {
															args.IsValid = true;
															return;
														}
													}
													function ValidateCurrency(source, args) {
														args.IsValid = false;
														var dd = $get("cmbCurrencyList");
														if (dd.selectedIndex != 0) {
															args.IsValid = true;
															return;
														}
													}
												</script>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblStatus" runat="server" Text="<%# mSalesInvoice.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<table width="100%">
											<tr>
												<td valign="top">
													<asp:UpdatePanel runat="server" ID="upnlSalesInvoiceDetails" UpdateMode="Conditional">
														<ContentTemplate>
															<fieldset class="clsFieldSetNewStyle">
																<legend>
																	<span id="lblInvoiceDetails" class="clsLabelHeader">Sales Invoice Details</span>
																</legend>
																<table>
																
																	<tr>
																		<td>
																			<span id="lblDateStar1" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblDate" class="clsLabel">Date</span>
																		</td>
																		<td colspan="2">
																			<asp:TextBox ID="txtInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
																				AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
																				Text="" Width="100px"></asp:TextBox>
																			<cc2:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																				Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInvoiceDate"></cc2:CalendarExtender>
																			<cc2:TextBoxWatermarkExtender ID="txtInvoiceDateWatermarkExtender" runat="server"
																				TargetControlID="txtInvoiceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<span id="lblNoStar" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblNo" class="clsLabel">No.</span>
																		</td>
																		<td>
																		<asp:TextBox ID="txtInvoiceText" runat="server" CssClass="clsTextBoxTagSearch"
																			MaxLength="25" Text="<%# mSalesInvoice.Text %>" ToolTip="Enter Text">
																		</asp:TextBox>
																		</td>
																		<td>
																			<asp:TextBox ID="txtInvoiceNo" runat="server" Text="<%# mSalesInvoice.No %>"
																				CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																				MaxLength="8"> 
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
															<fieldset class="clsFieldSetNewStyle">
																<legend>
																	<asp:Label ID="lblVendorDetails" runat="server" CssClass="clsLabelHeader">Customer Details</asp:Label>
																</legend>
																<table>
																	<tr>
																		<td colspan="4"></td>
																	</tr>
																	<tr>
																		<td>
																			<span id="lblNameStar" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblName" class="clsLabel">Name</span>
																		</td>
																		<td colspan="4">
																			<asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
																				Enabled="<%# mSalesInvoice.StatusID = 1 %>" DataTextField="Name"
																				DataValueField="ID" SelectedValue="<%# mSalesInvoice.VendorID %>" AutoPostBack="True">
																			</asp:DropDownList>
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblDispatchNo" runat="server" CssClass="clsLabelAuto">Dispatch No.</asp:Label>
																		</td>
																		<td>
																			<asp:TextBox ID="txtDispatchNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mSalesInvoice.DispatchNo %>"
																				MaxLength="50" ToolTip="Enter Dispatch No." Enabled="<%# mSalesInvoice.StatusID = 1 %>">
																			</asp:TextBox>
																		</td>
																		<td></td>
																		<td>
																			<asp:Label ID="lblVendorDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
																		</td>
																		<td>
																			<asp:TextBox ID="txtDispatchDate" runat="server" CssClass="clsTextBoxTagSearchDate"
																				onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
																				Text="<%# mSalesInvoice.DispatchDateFormatted %>"></asp:TextBox>
																			<cc2:CalendarExtender ID="txtDispatchDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																				Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDispatchDate"></cc2:CalendarExtender>
																			<cc2:TextBoxWatermarkExtender TargetControlID="txtDispatchDate" ID="txtDispatchDateWatermarkExtender"
																				runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<span id="lblCurrencyStar" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblCurrency" class="clsLabel">Currency</span>
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																				Width="191px" Enabled="<%# mSalesInvoice.StatusID = 1 %>" DataTextField="Name"
																				DataValueField="ID" SelectedValue="<%# mSalesInvoice.CurrencyID %>" AutoPostBack="True">
																			</asp:DropDownList>
																		</td>
																		<td>
																			<span id="lblStarFactor" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblConvFactor" class="clsLabelAuto">Factor</span>
																		</td>
																		<td>
																			<asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mSalesInvoice.ConversionFactor %>"
																				CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Enter Conversion Factor" MaxLength="9"
																				Enabled="<%# mSalesInvoice.StatusID = 1 %>"> </asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td>&nbsp;
																		</td>
																		<td>
																			<span id="lblRoundOffRequire" class="clsLabelAuto">Round Off Required</span>
																		</td>
																		<td colspan="4">
																			<asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
																				Checked="<%# mSalesInvoice.IsRoundOff %>" TextAlign="Right"></asp:CheckBox>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlSalesInvoiceItems" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset class="clsFieldSetNewStyle">
													<legend>
														<table>
															<tr>
																<td>
																	<span id="lblInvoiceItem" class="clsLabelHeader">Sales Invoice Item(s)</span>
																</td>
																<td>
																	<asp:ImageButton ID="btnAdd" runat="server"
																		ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																		ToolTip="Click to Add Sales Invoice Item"></asp:ImageButton>
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">
														<tr>
															<td>
																<asp:GridView ID="dgSalesInvoiceItem" runat="server" ShowHeaderWhenEmpty="True"
																	DataKeyNames="HSNACSCode" CssClass="clsGridNewStyle" GridLines="Horizontal"
																	CellPadding="5" AutoGenerateColumns="false">
																	<PagerSettings Mode="NextPreviousFirstLast" />
																	<RowStyle CssClass="clsdgItem" />
																	<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black"
																		Font-Bold="True" HorizontalAlign="Left" />
																	<AlternatingRowStyle CssClass="alt" />
																	<Columns>
																		<%--0--%>
																		<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																		<%--1--%>
																		<asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
																		</asp:BoundField>
																		<%--2--%>
																		<asp:BoundField DataField="ItemName" HeaderText="Part No.">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
																		</asp:BoundField>
																		<%--3--%>
																		<asp:BoundField DataField="ItemDescription" HeaderText="Description">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		</asp:BoundField>
																		<%--4--%>
																		<asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
																			<HeaderStyle HorizontalAlign="left"></HeaderStyle>
																			<ItemStyle HorizontalAlign="left"></ItemStyle>
																		</asp:BoundField>
																		<%--5--%>
																		<asp:BoundField DataField="ItemTypeName" HeaderText="Part Type"></asp:BoundField>
																		<%--6--%>
																		<asp:BoundField DataField="IssueNumber" HeaderText="Issue No.">
																			<HeaderStyle Wrap="False"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--7--%>
																		<asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--8--%>
																		<asp:BoundField DataField="ReceiptNumber" HeaderText="Receipt No.">
																			<HeaderStyle Wrap="False"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--9--%>
																		<asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--10--%>
																		<asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No.">
																			<HeaderStyle Wrap="False"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--11--%>
																		<asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N.Date">
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<%--12--%>
																		<asp:BoundField DataField="Qty" HeaderText="Qty.">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--13--%>
																		<asp:BoundField DataField="Unit" HeaderText="Unit">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		</asp:BoundField>
																		<%--14--%>
																		<asp:BoundField DataField="CRate" HeaderText="Rate">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--15--%>
																		<asp:BoundField DataField="COtherCharges" HeaderText="Other Charges">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--16--%>
																		<asp:BoundField DataField="CAmount" HeaderText="Amount">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--17--%>
																		<asp:TemplateField HeaderText="CGST(%)">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWCGST" runat="server" CssClass="clsTextBoxTagSearchQty"
																					ClientIDMode="Static" Width="40px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"CGSTPercentage") %>'> 
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--18--%>
																		<asp:TemplateField HeaderText="CGST Amt.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWCGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty"
																					ReadOnly="true" BackColor="#E0E0E0" Width="60px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"CGSTCAmount") %>'> 
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--19--%>
																		<asp:TemplateField HeaderText="SGST(%)">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWSGST" runat="server" CssClass="clsTextBoxRightAlignQty"
																					ClientIDMode="Static" ReadOnly="true" BackColor="#E0E0E0" Width="40px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"SGSTPercentage") %>'> 
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--20--%>
																		<asp:TemplateField HeaderText="SGST Amt.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWSGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty"
																					ReadOnly="true" BackColor="#E0E0E0" Width="60px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"SGSTCAmount") %>'> 
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--21--%>
																		<asp:TemplateField HeaderText="IGST(%)">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWIGST" runat="server" CssClass="clsTextBoxRightAlignQty"
																					ClientIDMode="Static" Width="40px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"IGSTPercentage") %>'>
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--22--%>
																		<asp:TemplateField HeaderText="IGST Amt.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtWIGSTAmt" runat="server" CssClass="clsTextBoxRightAlignQty"
																					ReadOnly="true" BackColor="#E0E0E0" Width="60px"
																					Text='<%# DataBinder.Eval(Container.DataItem,"IGSTCAmount") %>'>
																				</asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--23--%>
																		<asp:BoundField DataField="Remark" HeaderText="Remark"></asp:BoundField>
																		<%--24--%>
																		<asp:BoundField DataField="Note" HeaderText="Note"></asp:BoundField>
																		<%--25--%>
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
																									<asp:ImageButton ID="EditView" runat="server"
																										CommandArgument='<%# Container.DataItemIndex %>'
																										CommandName="EditView" class="actionICNS"
																										ImageUrl="~/images/edit.png"
																										ToolTip="Click to Edit record."
																										Visible='<%#IIf(mSalesInvoice.StatusID > 1, False, True) %>' />
																								</td>
																								<td>
																									<asp:ImageButton ID="DeleteRecord" runat="server"
																										CommandArgument='<%# Container.DataItemIndex %>'
																										CausesValidation="false" class="actionICNS  largerActionICNS"
																										CommandName="DeleteRecord" ToolTip="Click to Delete record."
																										ImageUrl="~/images/delete.png"
																										Visible='<%#IIf(mSalesInvoice.StatusID > 1, False, True) %>' />
																								</td>
																							</tr>
																						</table>
																					</div>
																				</div>
																			</ItemTemplate>
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
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<table width="100%">
											<tr>
												<td colspan="1" width="71%" valign="top">
													<asp:UpdatePanel runat="server" ID="upnlSalesInvoiceCharge" UpdateMode="Conditional">
														<ContentTemplate>
															<fieldset class="clsFieldSetNewStyle">
																<legend>
																	<table>
																		<tr>
																			<td>
																				<span id="lblInvoiceCharge" class="clsLabelHeader">Sales Invoice Charge(s)</span>
																			</td>
																			<td>
																				<asp:ImageButton ID="btnAddCharge" runat="server"
																					ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					ToolTip="Click to Add Charge"></asp:ImageButton>
																			</td>
																		</tr>
																	</table>
																</legend>
																<table width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgSalesInvoiceCharge" runat="server" AutoGenerateColumns="False"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
																				<PagerSettings Mode="NextPreviousFirstLast" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black"
																					Font-Bold="True" HorizontalAlign="Left" />
																				<AlternatingRowStyle CssClass="alt" />
																				<Columns>
																					<%--0--%>
																					<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																					<%--1--%>
																					<asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
																					<%--2--%>
																					<asp:BoundField DataField="ChargeName" HeaderText="Charge" />
																					<%--3--%>
																					<asp:BoundField DataField="Percentage" HeaderText="Percentage">
																						<HeaderStyle HorizontalAlign="Right" />
																						<ItemStyle HorizontalAlign="Right" />
																						<FooterStyle HorizontalAlign="Right" />
																					</asp:BoundField>
																					<%--4--%>
																					<asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
																						<HeaderStyle HorizontalAlign="Right" />
																						<ItemStyle HorizontalAlign="Right" />
																						<FooterStyle HorizontalAlign="Right" />
																					</asp:BoundField>
																					<%--5--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
																						ItemStyle-HorizontalAlign="Center">
																						<HeaderStyle HorizontalAlign="Center" />
																						<ItemStyle HorizontalAlign="Center" />
																						<ItemTemplate>
																							<div id="dropDownImg" class="dropdown">
																								<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																								<div id="dropdownICN-content" class="dropdownbtn-content">
																									<table id="dropdown-content" class="clsGridNew_Ajax">
																										<tr>
																											<td>
																												<asp:ImageButton ID="EditView" runat="server"
																													CommandArgument='<%# Container.DataItemIndex %>'
																													CommandName="EditView" class="actionICNS" ImageUrl="~/images/edit.png"
																													ToolTip="Click to Edit record."
																													Visible='<%#IIf(mSalesInvoice.StatusID > 1, False, True) %>' />
																											</td>
																											<td>
																												<asp:ImageButton ID="DeleteRecord" runat="server"
																													CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"
																													CommandName="DeleteRecord" class="actionICNS  largerActionICNS"
																													ImageUrl="~/images/delete.png" ToolTip="Click to Delete record."
																													Visible='<%#IIf(mSalesInvoice.StatusID > 1, False, True) %>' />
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
																				<SelectedRowStyle BackColor="ControlDark" />
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td valign="top" width="29%" align="right">
													<asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
														<ContentTemplate>
															<fieldset class="clsFieldSetNewStyle">
																<legend>
																	<span id="lblOtherCharges" class="clsLabelHeader">Other Details</span>
																</legend>
																<table align="right" width="100%">
																	<tr>
																		<td align="right">
																			<span id="lblGrandTotal" class="clsLabelAuto cls-RightAlignLabel">
																				Total
																			</span>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtCTotal" runat="server" Text="<%# mSalesInvoice.CTotalAmount %>"
																				CssClass="clsTextBoxTagSearch" ToolTip="Total "
																				BackColor="#E0E0E0" Style="text-align: right"
																				Width="80px" ReadOnly="True"></asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabelAuto cls-RightAlignLabel">
																				Total CGST
																			</asp:Label>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtTotalCGST" runat="server"
																				CssClass="clsTextBoxTagSearch" Style="text-align: right"
																				Width="80px" Text="<%# mSalesInvoice.CTotalCGSTAmount %>"
																				ReadOnly="True" BackColor="#E0E0E0">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<asp:Label ID="lblTotalSGST" runat="server" Cssclass="clsLabelAuto cls-RightAlignLabel">
																				Total SGST
																			</asp:Label>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtTotalSGST" runat="server"
																				CssClass="clsTextBoxTagSearch" Style="text-align: right"
																				Width="80px" Text="<%# mSalesInvoice.CTotalSGSTAmount %>"
																				ReadOnly="True" BackColor="#E0E0E0">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabelAuto cls-RightAlignLabel">
																				Total IGST
																			</asp:Label>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtTotalIGST" runat="server"
																				CssClass="clsTextBoxTagSearch" Style="text-align: right"
																				Width="80px" Text="<%# mSalesInvoice.CTotalIGSTAmount %>"
																				ReadOnly="True" BackColor="#E0E0E0">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<span id="lblTotaolOtherCharges" class="clsLabelAuto cls-RightAlignLabel-120">
																				Total Other Charges
																			</span>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtCTotalOtherCharge" runat="server"
																				Width="80px" Text="<%# mSalesInvoice.CTotalCharges %>"
																				CssClass="clsTextBoxTagSearch" ToolTip="Total Other Charges"
																				BackColor="#E0E0E0" Style="text-align: right"
																				ReadOnly="True">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<span id="lblRemaining" class="clsLabelAuto cls-RightAlignLabel">
																				Grand Total
																			</span>
																		</td>
																		<td align="right">
																			<asp:TextBox ID="txtCGrandTotal" runat="server"
																				Text="<%# mSalesInvoice.CGrandTotal %>" Style="text-align: right"
																				CssClass="clsTextBoxTagSearch" ToolTip="Grand Total"
																				Width="80px" BackColor="#E0E0E0"
																				ReadOnly="True">
																			</asp:TextBox>
																		</td>
																	</tr>
																	<tr>
																		<td align="right">
																			<asp:Label ID="lblAmountInWords" runat="server" CssClass="clsLabelAuto cls-RightAlignLabel">
																				Amount In Words
																			</asp:Label>
																		</td>
																		<td>
																			<asp:TextBox ID="txtAmountInWords" runat="server"
																				CssClass="clsTextBoxTagSearchMultilineNewstyle"
																				Text="<%# mSalesInvoice.AmountINWords.trim %>"
																				MaxLength="250" ReadOnly="True" BackColor="#E0E0E0"
																				TextMode="MultiLine" Height="40px">
																			</asp:TextBox>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset class="clsFieldSetNewStyle">
													<legend>
														<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto" Font-Bold="true">Remark</asp:Label>
													</legend>
													<asp:TextBox ID="txtRemark" runat="server" Text="<%# mSalesInvoice.Remark %>"
														ToolTip="Enter Remark." Enabled="<%# mSalesInvoice.StatusID = 1 %>"
														CssClass="clsTextBoxSearch_Ajax" MaxLength="100"
														Height="40px" Width="100%">
													</asp:TextBox>
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
										<asp:UpdatePanel runat="server" ID="upnlSalesInvoiceTerms" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset class="clsFieldSetNewStyle">
													<legend>
														<table>
															<tr>
																<td>
																	<span id="lblSalesOrderTerms" class="clsLabelHeader">Sales Invoice Term(s)</span>
																</td>
																<td>
																	<asp:ImageButton ID="btnAddTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																		ToolTip="Click to Add Term"></asp:ImageButton>
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">
														<tr>
															<td>
																<asp:GridView ID="dgSalesInvoiceTerms" runat="server" AutoGenerateColumns="False"
																	Width="100%" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True">
																	<PagerSettings Mode="NextPreviousFirstLast" />
																	<RowStyle CssClass="clsdgItem" />
																	<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																	<AlternatingRowStyle CssClass="alt" />
																	<Columns>
																		<%--0--%>
																		<asp:BoundField DataField="SrNo" HeaderText="Sr.No."
																			HeaderStyle-Width="10px" ItemStyle-Width="10px" />
																		<%--1--%>
																		<asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
																			<ItemStyle CssClass="TextBreak" Width="600px" />
																		</asp:BoundField>
																		<%--2--%>
																		<asp:TemplateField ItemStyle-HorizontalAlign="Center"
																			HeaderText="Remove" HeaderStyle-HorizontalAlign="Center"
																			HeaderStyle-Width="50px" ItemStyle-Width="50px">
																			<ItemTemplate>
																				<asp:ImageButton ID="DeleteRecord" runat="server"
																					CommandArgument='<%# Container.DataItemIndex %>'
																					CommandName="DeleteRecord" class="actionICNS  largerActionICNS"
																					ToolTip="Click to Delete record."
																					ImageUrl="~/images/delete.png" />
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
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
								<tr style="height: 0px;">
									<td colspan="2" style="height: 0px;">
										<asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnimgBtnReqPartList" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnImgBtnSalesInvoiceTerms" ClientIDMode="Static" runat="server"
													CausesValidation="false" Style="display: none" />
												<asp:Button ID="hdnImgBtnSalesInvoiceCharges" ClientIDMode="Static" runat="server"
													CausesValidation="false" Style="display: none" />
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
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

		<script type="text/javascript">

			//Date validations
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

		<!-- Charge Popup Window -->

		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySalesInvoiceCharge" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

		</div>
		<asp:Panel runat="server" ID="pnlPopUpSalesInvoiceCharge" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iFrameSalesInvoiceCharge" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpSalesInvoiceCharge" runat="server" TargetControlID="btnDummySalesInvoiceCharge"
			PopupControlID="pnlPopUpSalesInvoiceCharge" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">

			function IFrameSalesInvoiceChargeStateComplete() {

				$("#btnDummySalesInvoiceCharge").click();
				$get("AjaxLoader").style.visibility = 'hidden';

			}
			function OpenSalesInvoiceChargeWindow() {
				try {
					$("#iFrameSalesInvoiceCharge").attr("src", "wfSalesInvoiceCharge_Ajax.aspx?Typepup=pup");
					if (!$.browser.msie) {
						$("#btnDummySalesInvoiceCharge").click();
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>

		<script type="text/javascript">

			function ParentCallBackFunctionForSalesInvoiceCharge() {

				var TermWindow = $find("<%=mdlPopUpSalesInvoiceCharge.ClientID %>");
				//Close Charge Pop-Up Window
				TermWindow.hide();
				$("#iFrameSalesInvoiceCharge").attr("src", "JavaScript:''");
				//Call Hidden Image button Click Event
				$("#hdnImgBtnSalesInvoiceCharges").click();

			}

		</script>
		<!-- End-->

		<!-- Term Popup Window -->

		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySalesInvoiceTerm" Text="Dummy Term" ClientIDMode="Static" CausesValidation="false" />

		</div>
		<asp:Panel runat="server" ID="pnlPopUpSalesInvoiceTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iFrameSalesInvoiceTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpSalesInvoiceTerm" runat="server" TargetControlID="btnDummySalesInvoiceTerm"
			PopupControlID="pnlPopUpSalesInvoiceTerm" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">

			function IFrameSalesInvoiceTermStateComplete() {

				$("#btnDummySalesInvoiceTerm").click();
				$get("AjaxLoader").style.visibility = 'hidden';

			}
			function OpenSalesInvoiceTermWindow() {
				try {
					$("#iFrameSalesInvoiceTerm").attr("src", "wfSalesInvoiceTerm_Ajax.aspx?Typepup=pup&Type=9");
					if (!$.browser.msie) {
						$("#btnDummySalesInvoiceTerm").click();
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>

		<script type="text/javascript">

			function ParentCallBackFunctionForSalesInvoiceTerm() {

				var TermWindow = $find("<%=mdlPopUpSalesInvoiceTerm.ClientID %>");
				//Close Term Pop-Up Window
				TermWindow.hide();
				$("#iFrameSalesInvoiceTerm").attr("src", "JavaScript:''");
				//Call Hidden Image button Click Event
				$("#hdnImgBtnSalesInvoiceTerms").click();

			}

		</script>
		<!-- End-->

	</form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueForUnusedReturn.aspx.vb"
	Inherits="Flypal.wfIssueForUnusedReturn" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Issue Details</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript">
		function OpenLocation(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<style type="text/css">

		#lblPartList {
			display: block;
			width: 250px;
		}

</style>

</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblMain" class="clstablelistout" border="0">
			<tr>
				<td>
					<table id="tblinner" class="clsTablelistin" border="0">
						<tr>
							<td colspan="2" class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td class="clsFormHeader1Newstyle">
											<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Issue Details [New]</asp:Label>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:ValidationSummary ID="Validationsummary1" CssClass="clsValidationSummary" runat="server"
											Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
										<asp:CustomValidator ID="cvBrokenRules" runat="server" ControlToValidate="cmbToType"
											Display="None" ValidationGroup="1" OnServerValidate="customValidate" ValidateEmptyText="true"></asp:CustomValidator>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:UpdatePanel ID="upnlIssueDetails" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="tabDetails" width="100%">
											<tr>
												<td colspan="4" style="height: 20px"></td>
												<td align="right" colspan="2" style="height: 20px">
													<asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# mIssue.StatusName %>"> </asp:Label>
												</td>
											</tr>
											<tr>
												<td colspan="3" style="height: 21px">
													<asp:Label ID="lblIssueDetails" runat="server" CssClass="clsLabelHeader">
														Issue Details
													</asp:Label>
												</td>
												<td colspan="3" style="height: 21px">
													<asp:Label ID="lblDestinationDetails" runat="server" CssClass="clsLabelHeader">
														Destination Details
													</asp:Label>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="lblDateStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td>
													<asp:Label ID="lblIssueDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
												</td>
												<td>
													<table id="Table4" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td></td>
															<td></td>
														</tr>
													</table>
													<asp:TextBox runat="server" ID="IssueDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
														onchange="ValidateDateText(this,'IssueDate_watermarkextender');">
													</asp:TextBox>
													<cc2:CalendarExtender ID="IssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
														Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="IssueDate"></cc2:CalendarExtender>
													<cc2:TextBoxWatermarkExtender TargetControlID="IssueDate" ID="IssueDate_watermarkextender"
														ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
														WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
												</td>
												<td></td>
												<td>
													<asp:Label ID="lblIssueTo" runat="server" CssClass="clsLabelAuto">Issue To</asp:Label>
												</td>
												<td>
													<asp:DropDownList ID="cmbToType" runat="server" AutoPostBack="True" 
														CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static" DataTextField="Type" 
														DataValueField="ID" Enabled="<%# mIssue.IsNew And mIssue.TransTypeID = 0 %>"
														SelectedValue="<%# mIssue.ToTypeID %>">
													</asp:DropDownList>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="lblStarIssueNo" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td>
													<asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
												</td>
												<td>
													<table id="Table3" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td>
																<asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
																	Text="<%# mIssue.Text %>" ToolTip="Enter Text">
																</asp:TextBox>
															</td>
															<td class="clstablecell">
																<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" 
																	MaxLength="4" Text="<%# mIssue.No %>" ToolTip="Enter No.">
																</asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td>
													<asp:Label ID="lblSelectDetailsStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td>
													<asp:Label ID="lblSelectDetails" runat="server" CssClass="clsLabelAuto">
														Select Details
													</asp:Label>
												</td>
												<td>
													<asp:DropDownList ID="cmbLocationStore" runat="server" AutoPostBack="True"
														CssClass="clsTextBoxTagSearchComboNewstyleLong" Enabled="<%# mIssue.IsNew %>"
														DataTextField="LocationStore" DataValueField="ID" DESIGNTIMEDRAGDROP="396"
														SelectedValue="<%# mIssue.ToStoreID %>" Visible="False">
													</asp:DropDownList>
													<asp:DropDownList ID="cmbVendorList" runat="server"
														CssClass="clsTextBoxTagSearchComboNewstyleLong" Visible="False"
														DataTextField="Name" DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
														SelectedValue="<%# mIssue.VendorID %>">
													</asp:DropDownList>
													<asp:DropDownList ID="cmbAircraftList" runat="server" AutoPostBack="True" 
														CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="RegNo" 
														DataValueField="ID" Enabled="<%# mIssue.IsNew %>" SelectedValue="<%# mIssue.MachineID %>"
														Visible="False">
													</asp:DropDownList>
													<asp:DropDownList ID="cmbWorkShop" runat="server" AutoPostBack="True" 
														CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="LocationWorkShop" 
														DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
														SelectedValue="<%# mIssue.WorkShopID %>" Visible="False">
													</asp:DropDownList>
													<asp:DropDownList ID="cmbWorkOrder" runat="server" AutoPostBack="True" 
														CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="WONumber" 
														DataValueField="ID" Enabled="<%# mIssue.IsNew %>" SelectedValue="<%# mIssue.nWOID %>"
														Visible="False">
													</asp:DropDownList>
												</td>
											</tr>
											<tr>
												<td style="height: 22px">
													<asp:Label ID="lblStoreStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:Label ID="lblStore" runat="server" CssClass="clsLabelAuto">Store</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:DropDownList ID="cmbStoreList" runat="server" AutoPostBack="True"
														CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="LocationStore"
														DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
														SelectedValue="<%# mIssue.StoreID %>">
													</asp:DropDownList>
												</td>
												<td style="height: 22px"></td>
												<td style="height: 22px">
													<asp:Label ID="lblPerson" runat="server" CssClass="clsLabelAuto">Person</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:TextBox ID="txtPerson" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
														Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="25" Rows="2" Width="278px"
														Text="<%# mIssue.Person %>" TextMode="MultiLine" ToolTip="Enter Person ">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="height: 22px"></td>
												<td style="height: 22px">
													<asp:Label ID="lblBarcodeNo" runat="server" CssClass="clsLabelAuto" Visible="False">
														Barcode No.
													</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:TextBox ID="txtBarcodeIssue" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
														Text="<%# mIssue.BarcodeNo %>" Visible="False">
													</asp:TextBox>
												</td>
												<td style="height: 22px"></td>
												<td style="height: 22px">
													<asp:Label ID="lblAWBNo" runat="server" CssClass="clsLabelAuto">AWB No.</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
														Text="<%# mIssue.AWBNo %>">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="height: 20px"></td>
												<td style="height: 20px"></td>
												<td style="height: 20px"></td>
												<td style="height: 20px"></td>
												<td style="height: 20px">
													<asp:Label ID="lblVoucherNo" runat="server" CssClass="clsLabelAuto">Voucher No.</asp:Label>
												</td>
												<td style="height: 22px">
													<asp:TextBox ID="txtVoucherNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
														Text="<%# mIssue.VoucherNo %>">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="height: 14px"></td>
												<td style="height: 14px"></td>
												<td style="height: 14px"></td>
												<td style="height: 14px"></td>
												<td style="height: 14px">
													<asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto" Visible="False">
														Work Order
													</asp:Label>
												</td>
												<td align="right" style="height: 14px">
													<asp:DropDownList ID="cmbWO" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" 
														DataTextField="WONumber" DataValueField="ID" Enabled="False" 
														SelectedValue="<%# mIssue.nWOID %>" Visible="False">
													</asp:DropDownList>
													<asp:Button ID="btnWOReturnParts" runat="server" CssClass="clsbtnH clsinfoH" Text="Return"
														Visible="False" />
												</td>
											</tr>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td></td>
												<td>
													<asp:Label ID="lblRegNo" runat="server" CssClass="clsLabelAuto" 
														Visible="<%# (mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomer) %>">
														Reg. No.
													</asp:Label>
												</td>
												<td align="left" valign="top">
													<asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
														Text="<%# mIssue.RegNo %>" 
														Visible="<%# (mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomer) %>">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
												</td>
												<td colspan="4">
													<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" 
														Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="150" Rows="2" Width="278px"
														Text="<%# mIssue.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark">
													</asp:TextBox>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="left" style="height: 47px">
								<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table2" class="clstableButton" border="0">
											<tr>
												<td>
													<asp:Label ID="lblParts" runat="server" CssClass="clsLabelHeaderItem">
														Issue Item(s)
													</asp:Label>
												</td>
												<td>
													<asp:Button ID="btnAddItem" runat="server" CssClass="clsbtnH clsinfoH1"
														Text="Add" ToolTip="Click to Add New Issue Part" />
												</td>
												<td>
													<asp:Label ID="lblBarcodeNos" runat="server" CssClass="clsLabelAuto"
														Visible="False">Barcode No.
													</asp:Label>
												</td>
												<td>
													<asp:TextBox ID="txtBarcodeItem" runat="server" CssClass="clsTextBoxTagSearch newclass"
														Visible="False">
													</asp:TextBox>
												</td>
												<td>
													<asp:Button ID="btnAddBarcodeItem" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
														ToolTip="Click to Add Barcode No" Visible="False" />
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:UpdatePanel ID="upnlGridViewIssueItem" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:GridView ID="GVIssueItems" runat="server" AutoGenerateColumns="False"
											CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
											<AlternatingRowStyle CssClass="clsdgAltItem" />
											<RowStyle CssClass="clsdgItem" />
											<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"
												HorizontalAlign="Left" />
											<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
											<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
											<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right"/>
											<Columns>
												<asp:BoundField DataField="SRNo" HeaderText="Sr.No."></asp:BoundField>
												<asp:BoundField DataField="ItemName" HeaderText="Part No.">
													<HeaderStyle Wrap="False" />
													<ItemStyle Wrap="False" />
												</asp:BoundField>
												<asp:BoundField DataField="Itemdesc" HeaderText="Description"></asp:BoundField>
												<asp:BoundField DataField="ItemTypeName" HeaderText="Part Type"></asp:BoundField>											
												<asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" HtmlEncode="false">
													<HeaderStyle HorizontalAlign="Left" />
													<ItemStyle Wrap="False" />
												</asp:BoundField>												
												<asp:BoundField DataField="OriginalReceiptInfo" HeaderText="Original Receipt Info"
													HtmlEncode="false">
													<HeaderStyle HorizontalAlign="Left" />
													<ItemStyle Wrap="False" />
												</asp:BoundField>
												<asp:BoundField DataField="VendorInvoiceNo" HeaderText="Supp. Invoice No.">
													<ItemStyle Wrap="False" />
												</asp:BoundField>
												<asp:BoundField DataField="VendorInvoiceDateFormatted" HeaderText="Supp. Invoice Date">
													<ItemStyle Wrap="False" />
												</asp:BoundField>												
												<asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info" HtmlEncode="false">
													<HeaderStyle HorizontalAlign="Left" />
													<ItemStyle Wrap="False" />
												</asp:BoundField>
												<asp:TemplateField HeaderText="Qty.">
													<ItemTemplate>
														<asp:TextBox ID="txtQty" runat="server" AutoPostBack="True"
															CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
															Enabled="<%# (mIssue.StatusID = 1) %>" MaxLength="8" OnTextChanged="TextChanged"
															Text='<%# DataBinder.Eval(Container.DataItem, "DisplayQty") %>'>
														</asp:TextBox>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="ReturnQty">
													<ItemStyle HorizontalAlign="Right" />
													<ItemTemplate>
														<asp:TextBox ID="txtReturnQty" runat="server"
															CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
															MaxLength="8" OnTextChanged="TextChanged"></asp:TextBox>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:BoundField DataField="DisplayUnitName" HeaderText="Unit"></asp:BoundField>
												<asp:BoundField DataField="DiscardAmt" HeaderText="Discard Amt." Visible="false">
													<HeaderStyle HorizontalAlign="Right" />
													<ItemStyle HorizontalAlign="Right" />
												</asp:BoundField>
												<asp:BoundField DataField="SerialNo" HeaderText="Serial No."></asp:BoundField>												
												<asp:BoundField DataField="ExpiryQtrDateInfo" HeaderText="Expiry Date / Qtrs.">
													<HeaderStyle HorizontalAlign="Left" />
													<ItemStyle Wrap="False" />
												</asp:BoundField>
												<asp:BoundField DataField="OutGoingReleaseNoteNo" HeaderText="Outgoing Release Note No."></asp:BoundField>
												<asp:BoundField DataField="Remark" HeaderText="Remark"></asp:BoundField>
												<asp:BoundField DataField="Note" HeaderText="Note"></asp:BoundField>
												<asp:BoundField DataField="BatchNo" HeaderText="RNN No."></asp:BoundField>
												<asp:BoundField DataField="WOReturnQty" HeaderText="WO. Return Qty" Visible="False"></asp:BoundField>
											</Columns>
										</asp:GridView>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td align="right" colspan="2">
								<asp:UpdatePanel ID="upnlSaveClose" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table>
											<tr>
												<td align="right">
													<asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="False"
														Text="Cancel" ToolTip="Click to Cancel the Issue" />

													<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save"
														ToolTip="Click to save Issue"
														ValidationGroup="1" CausesValidation="true" />

													<asp:Button ID="btnReleaseNoteNo" runat="server" CssClass="clsbtnH clsinfoH1"
														Enabled="<%# Not mIssue.IsNew %>"
														Text="Release Note " ToolTip="Click to Print Release Note No." />

													<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1"
														Enabled="<%# Not mIssue.IsNew %>"
														Text="Print" ToolTip="Click to Print the Issue" />

													<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1"
														Text="Close" ToolTip="Click to go back to the previous page" />
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

		<!-- Unused Returned Parts -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyUnusedReturnedParts" Text="Dummy Unused Returned Parts" />
		</div>
		<asp:Panel runat="server" ID="pnlUnusedReturnedParts" Style="display: none">
			<div>
				<table class="clstablelistout" id="Table6">
					<tr>
						<td>
							<asp:UpdatePanel runat="server" ID="upnlUnusedReturnedParts" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="Table7" class="clstablelistin">
										<tr>
											<td colspan="2" class="clsFormHeader1Newstyle">
												<table>
													<tr>
														<td>
															<span id="lblPartList" class="clsFormHeader">Unused Return Part Details</span>
														</td>
														<td align="right" colspan="2">
															<asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" 
																Text="Ok" ToolTip="Click to Add Unused Return Part Details"
																ValidationGroup="valGroup1" />
															<asp:Button ID="btnCloseUnusedReturnPart" 
																runat="server" CssClass="clsbtnH clsinfoH"
																Text="Close" ToolTip="Click to close Unused Return Part Details screen" />
														</td>
													</tr>
												</table>												
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
													Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvReturnDate" runat="server" Display="None" ErrorMessage="Name is too long"
													ControlToValidate="calReturnDate" OnServerValidate="customvalidate" ValidateEmptyText="true"
													ValidationGroup="valGroup1"></asp:CustomValidator>
												<asp:CustomValidator ID="cvRemarkForUnsedParts" runat="server" Display="None" ErrorMessage="Name is too long"
													ControlToValidate="txtRemarkForUnsedParts" OnServerValidate="customvalidate"
													ValidateEmptyText="true" ValidationGroup="valGroup1"></asp:CustomValidator>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Return Date</asp:Label>
											</td>
											<td>
												<table id="Table9">
													<tr>
														<td>
															<asp:TextBox ID="calReturnDate" runat="server" AutoPostBack="true"
																CssClass="clsTextBoxTagSearch" Width="100px">
															</asp:TextBox>
															<cc2:CalendarExtender ID="calReturnDate_CalendarExtender" runat="server" 
																CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" 
																TargetControlID="calReturnDate">
															</cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="calReturnDate" 
																ID="calReturnDate_TextBoxWatermarkExtender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox">
															</cc2:TextBoxWatermarkExtender>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblRemark1" runat="server" CssClass="clsLabel">Remark</asp:Label>
											</td>
											<td>
												<table id="Table8">
													<tr>
														<td>
															<asp:TextBox ID="txtRemarkForUnsedParts" runat="server" 
																CssClass="clsTextBoxMultiLineLong_Ajax" Width="278px"
																ToolTip="Enter Remark" MaxLength="150" TextMode="MultiLine" Rows="2">
															</asp:TextBox>
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
		<cc2:ModalPopupExtender ID="mdlPopUpUnusedReturnedParts" runat="server" TargetControlID="btnDummyUnusedReturnedParts"
			PopupControlID="pnlUnusedReturnedParts" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
	</form>

	<%--Added by Harsh on 4th June 2024 For FLYPAL-1635 Material Out Cosmetic changes--%>
	<script type="text/javascript" id="dateValidations">

		//Date Validation
		function ValidateDateText(elem, extenderid) {

			var datevalue = $(elem).val();
			var params = { 'Date': datevalue, 'SetDefault': 'true' };
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

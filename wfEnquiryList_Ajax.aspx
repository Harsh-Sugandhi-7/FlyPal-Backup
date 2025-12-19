<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEnquiryList_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.EnquiryListPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Enquiry List</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout Table-MaxWidth" id="tblMain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblInner" class="clstablelistin">
							<asp:PlaceHolder runat="server" ID="PlaceHolder11" 
								Visible='<%# IIf(AppSettings("NewUi") = "True", True, False) %>'>
								<tr>
									<td align="right">
										<table>
											<tr>
												<td>
													<asp:Button ID="btnCheckoutNewApplication" runat="server"
														CssClass="clsbtnH clsinfoH1"
														Text="Check Out New Application" CausesValidation="False" />
												</td>
												<td>
													<asp:Image ID="imgCheckoutNewApplication" runat="server"
														ImageUrl="~/images/new.png"
														Height="45px" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</asp:PlaceHolder>
							<tr>
								<td colspan="2" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblEnquiryList" runat="server" 
															CssClass="clsFormHeader" Text="List Of Enquiry" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAddNew" runat="server" 
																		CssClass="clsbtnH clsinfoH" Text="Add New"
																		ToolTip="Click to Add New Enquiry" 
																		CausesValidation="False" />
																</td>
																<td>
																	<asp:Button ID="btnPrint" runat="server" 
																		CssClass="clsbtnH clsinfoH" Text="Print"
																		ToolTip="Click to Print List of  Enquiry" 
																		CausesValidation="False" />
																</td>
																<td>
																	<asp:Button ID="btnClose" runat="server" 
																		CssClass="clsbtnH clsinfoH" Text="Close"
																		ToolTip="Click to close List of Enquiry screen" 
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
							<tr>
								<td colspan="2">
									<asp:ValidationSummary ID="Validationsummary2" 
										runat="server" CssClass="clsValidationSummary"
										HeaderText="Fill Up The Following Fields" 
										ValidationGroup="a" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<table>
																		<tr>
																			<td>
																				<span id="Span8" class="clsLabel">Range</span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbDate" runat="server" 
																					CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
																					<asp:ListItem Value="0">(ALL)</asp:ListItem>
																					<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																					<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																					<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																					<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																					<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																					<asp:ListItem Value="6">Between Dates</asp:ListItem>
																				</asp:DropDownList>
																			</td>
																			<td>
																				<span id="lblFrom" class="clsLabel" runat="server">From Date</span>
																			</td>
																			<td>
																				<asp:TextBox runat="server" ID="txtFromDate" 
																					CssClass="clsTextBoxTagSearchDate" Width="100px"
																					onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
																				<cc2:calendarextender id="txtFromDate_CalendarExtender" 
																					runat="server" cssclass="cal_Theme1"
																					enabled="true" format="<%$AppSettings:DateFormat%>" 
																					targetcontrolid="txtFromDate" />
																				<cc2:textboxwatermarkextender targetcontrolid="txtFromDate" 
																					id="FromDate_watermarkextender"
																					clientidmode="Static" runat="server" 
																					watermarktext="<%$AppSettings:DateFormat%>"
																					watermarkcssclass="clsDateTextBox" />
																			</td>
																			<td>
																				<span id="lblTo" class="clsLabel" runat="server">To Date</span>
																			</td>
																			<td>
																				<asp:TextBox runat="server" ID="txtToDate" 
																					CssClass="clsTextBoxTagSearchDate" Width="100px"
																					onchange="ValidateDateText(this,'ToDate_watermarkextender');" />
																				<cc2:calendarextender id="txtToDate_CalendarExtender1" 
																					runat="server" cssclass="cal_Theme1"
																					enabled="true" format="<%$AppSettings:DateFormat%>" 
																					targetcontrolid="txtToDate" />
																				<cc2:textboxwatermarkextender targetcontrolid="txtToDate" 
																					id="ToDate_watermarkextender"
																					clientidmode="Static" runat="server" 
																					watermarktext="<%$AppSettings:DateFormat%>"
																					watermarkcssclass="clsDateTextBox" />
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<span id="lblPartNoSearch" class="clsLabel">Part No.</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtPartNoSearch" runat="server" 
																					CssClass="clsTextBoxTagSearch" MaxLength="100" />
																			</td>
																			<td>
																				<span id="Span3" class="clsLabel">Enquiry No.</span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbEnquiryText" 
																					runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					AutoPostBack="True" DataValueField="Text" 
																					DataTextField="Text" />
																			</td>
																			<td>
																				<asp:TextBox ID="txtEnquiryNo" 
																					runat="server" 
																					CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					MaxLength="6" />
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</td>
													<td align="right" valign="top">
														<asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:ImageButton ID="btnFindNow" runat="server" 
																	ImageUrl="~/images/Search2.png" 
																	CssClass="clsSearch2btn"
																	ToolTip="Filter the Enquiry list as per criteria" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" 
																	Style="border: none;">
																	<div>
																		<div style="float: left; vertical-align: middle; width: 100%">
																			<table width="100%">
																				<tr>
																					<td>
																						<span style="vertical-align: middle; margin-left: 2px; 
																								width: 100%" id="lblMastersSelection"
																							class="clsLabelHeader">Advance Search</span>
																					</td>
																					<td align="right">
																						<div style="float: right; vertical-align: middle; 
																									margin-right: 5px;">
																							<image id="imgMasters" 
																								src="images/collapse_blue.jpg" 
																								alternatetext="(Show Details...)" />
																						</div>
																					</td>
																				</tr>
																			</table>
																		</div>
																	</div>
																</asp:Panel>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td valign="top" colspan="2">
														<asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Panel ID="pnlAdvancedSearch" runat="server" 
																	DefaultButton="btnFindNow" 
																	Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																	<table>
																		<tr>
																			<td>
																				<span id="lblVendor" class="clsLabel" runat="server">Vendor</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtVendorName" 
																					runat="server" CssClass="clsTextBoxTagSearch"
																					MaxLength="100" />
																			</td>
																			<td>
																				<span id="Span7" class="clsLabel">Status</span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbStatus" runat="server" 
																					CssClass="clsTextBoxTagSearchComboNewstyle">
																					<asp:ListItem Value="0">(ALL)</asp:ListItem>
																					<asp:ListItem Value="1">Opened</asp:ListItem>
																					<asp:ListItem Value="2">Authorized</asp:ListItem>
																					<asp:ListItem Value="4">Cancelled</asp:ListItem>
																				</asp:DropDownList>
																			</td>
																		</tr>
																	</table>
																</asp:Panel>
																<cc2:collapsiblepanelextender behaviorid="clpMastersBehaviour"
																	id="clpAdvancedSearch" clientidmode="Static" runat="Server" 
																	targetcontrolid="pnlAdvancedSearch" expandcontrolid="ClpnlAdvancedSearch"
																	collapsecontrolid="ClpnlAdvancedSearch" collapsed="True" 
																	imagecontrolid="imgMasters" collapsedsize="0" 
																	expandedtext="(Hide Details...)" collapsedtext="(Show Details...)"
																	expandedimage="~/images/collapse_blue.jpg" 
																	collapsedimage="~/images/expand_blue.jpg"
																	suppresspostback="false" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server"
															CssClass="clsLabelAuto" Font-Bold="True" />
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:GridView ID="dgEnqList" runat="server" CssClass="clsGridNewStyle"
															GridLines="Horizontal" CellPadding="5" DataKeyNames="ID"
															ShowHeaderWhenEmpty="true" AllowSorting="True" 
															AllowPaging="True" AutoGenerateColumns="False"
															PageSize="25">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
																Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" 
																FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging"
																ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																<%--1--%>
																<asp:BoundField DataField="DateFormatted" HeaderText="Date">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--2--%>
																<asp:BoundField DataField="EnquiryNo" 
																	SortExpression="EnquiryNo" HeaderText="Number">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																	<ItemStyle Wrap="False" />
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="VendorName" 
																	SortExpression="VendorName" HeaderText="Supplier"
																	HtmlEncode="false">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle HorizontalAlign="Left" 
																		Width="500px" Wrap="true" CssClass="TextBreak" />
																</asp:BoundField>
																<%--4--%>
																<asp:BoundField DataField="Status" 
																	SortExpression="Status" HeaderText="Status">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>
																<%--5--%>
																<asp:BoundField DataField="UserName" 
																	SortExpression="UserName" HeaderText="Created By">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>
																<%--6--%>
																<asp:BoundField DataField="AuthorizedBy" 
																	SortExpression="AuthorizedBy" HeaderText="Authorized By">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>
																<%--7--%>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" 
																	HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" 
																				runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="EditView" runat="server"
																								CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
																								CommandName="EditRec" ImageUrl="~/images/edit.png"
																								CssClass="actionICNS" CausesValidation="false"
																								ToolTip="Edit this record." />
																						</td>
																						<td>
																							<asp:ImageButton ID="DeleteRecord" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								CommandName="DeleteRec" ImageUrl="~/images/delete.png"
																								ToolTip="Delete this record." CausesValidation="false"
																								CssClass="actionICNS  largerActionICNS" />
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

		<script type="text/javascript">

			//From Date -To Date validation
			function BetweenDatesValidation(source, args) {
				args.IsValid = false;
				var fromdate = $("#txtFromDate").val();
				var todate = $("#txtToDate").val();
				if (!todate) {
					rfvToDate.isvalid = false;
					return;
				}
				if (!fromdate) {
					rfvFromDate.isvalid = false;
					return;
				}
				var param = { 'FromDate': fromdate, 'ToDate': todate };
				$.ajax({
					type: "POST",
					url: "BetweenDateValidationHandler.ashx",
					cache: false,
					data: param,
					async: false,
					beforeSend: OnBeforeSnd,
					success: onSuces,
					error: onErr
				});

				function onSuces(result) {
					$get("AjaxLoader").style.visibility = 'hidden';
					if (result == "True") {
						args.IsValid = true;
						return;
					}

				}

				function onErr(result) {
					$get("AjaxLoader").style.visibility = 'hidden';
					source.errormessage = result;
					return;
				}
				function OnBeforeSnd() {
					$get("AjaxLoader").style.visibility = 'visible';
				}

			}

			//Date validations
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

	</form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesInvoiceList_Ajax.aspx.vb"
	Inherits="Flypal.wfSalesInvoiceList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Sales Invoice List</title>
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
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblSalesIncvoiceList" runat="server" CssClass="clsFormHeader">
																List of Sales Invoice
																<asp:Label ID="lblTotal" runat="server"></asp:Label>
															</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>

												<td align="right">
													<asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
														<ContentTemplate>
															<table>
																<tr>
																	<td align="right">
																		<asp:Button ID="btnAddNewTop" runat="server" 
																			CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Sales Invoice"
																			Text="Add New" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnPrintTop" runat="server" 
																			CssClass="clsbtnH clsinfoH" ToolTip="Click to print Sales Invoice List"
																			Text="Print" CausesValidation="False"></asp:Button>
																	</td>
																	<td align="right">
																		<asp:Button ID="btnCloseTop" runat="server"
																			CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Sales Invoice screen."
																			Text="Close" CausesValidation="False"></asp:Button>
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
										<asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
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
																						<asp:ListItem Value="0">(All)</asp:ListItem>
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
																					<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
																						onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
																					<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																						Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																						ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																				</td>
																				<td>
																					<span id="lblTo" class="clsLabel" runat="server">To Date</span>
																				</td>
																				<td>
																					<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
																						onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
																					<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
																						Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																						ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="lblPartNoSearch" class="clsLabel">Part No.</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtPartNoSearch" runat="server" CssClass="clsTextBoxTagSearch" 
																						MaxLength="100">
																					</asp:TextBox>
																				</td>
																				<td>
																					<span id="Span3" class="clsLabel">Sales Invoice No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbSalesInvoiceText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																						AutoPostBack="True" DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtSalesInvoiceNo" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						MaxLength="6"></asp:TextBox>
																				</td>
																				<td></td>
																			</tr>
																		</table>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right" valign="top">
															<asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
																		ToolTip="Click to find list of Sales Invoice as  per searching criteria" />
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td valign="top" colspan="2">
															<asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="cpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
																		<div>
																			<div id="divCollapsiblePnl">
																				<table width="100%">
																					<tr>
																						<td>
																							<span id="lblMastersSelection" class="clsLabelHeader">Advance Search
																							</span>
																						</td>
																						<td align="right">
																							<div id="divCollapsiblePnlImg">
																								<image id="imgMasters" src="images/collapse_blue.jpg"
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
																	<asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnFindNow" 
																		Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																		<table>
																			<tr>
																				<td>
																					<span id="lblIssueNo" class="clsLabel" runat="server">Issue No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbIssueText" runat="server" 
																						CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																						DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtIssueNo" runat="server" 
																						CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8"></asp:TextBox>
																				</td>
																				<td>
																					<span id="lblSalesOrderNo" class="clsLabel" runat="server">Sales Order No.</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbSalesOrderText" runat="server"
																						CssClass="clsTextBoxTagSearchComboNewstyle"
																						AutoPostBack="True" DataValueField="Text" DataTextField="Text">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:TextBox ID="txtSalesOrderNo" runat="server" 
																						CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						MaxLength="8"></asp:TextBox>
																				</td>
																				<td>
																					<span id="Span7" class="clsLabel">Status</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbStatus" runat="server" 
																						CssClass="clsTextBoxTagSearchComboNewstyle">
																						<asp:ListItem Value="0">(All)</asp:ListItem>
																						<asp:ListItem Value="1">Opened</asp:ListItem>
																						<asp:ListItem Value="2">Authorized</asp:ListItem>
																						<asp:ListItem Value="4">Cancelled</asp:ListItem>
																					</asp:DropDownList>
																				</td>
																				<td>
																					<span id="lblCustomer" class="clsLabel">Customer</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtCustomer" runat="server"
																						CssClass="clsTextBoxTagSearch" MaxLength="100">
																					</asp:TextBox>
																				</td>
																			</tr>
																		</table>
																	</asp:Panel>
																	<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
																		ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" 
																		ExpandControlID="cpnlAdvancedSearch"
																		CollapseControlID="cpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
																		CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																		ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																		SuppressPostBack="false" />
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
										<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True"></asp:Label>
														</td>
														<td align="right">
															<table>
																<tr>
																	<td>
																		<span id="lblAs" class="clsLabelAuto">Create Sales Invoice Against</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbSalesInvoiceType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																			DataValueField="ID" DataTextField="Name">
																		</asp:DropDownList>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:GridView ID="dgSalesInvoiceList" runat="server" AllowPaging="True" AllowSorting="True"
																AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" 
																CellPadding="5" PageSize="25" ShowHeaderWhenEmpty="True">
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle CssClass="clsdgHeader" BackColor="White"
																	ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																<PagerStyle CssClass="paging" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
																		ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="InvDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="SalesInvoiceNo" HeaderText="Sales Invoice Number" SortExpression="SalesInvoiceNo">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="SalesInvoiceType" SortExpression="SalesInvoiceType" HeaderText="Type">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<asp:BoundField DataField="VendorName" HeaderText="Customer" SortExpression="VendorName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total" SortExpression="CGrandTotal">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
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
																									CommandArgument='<%# Eval("ID") %>'
																									CommandName="EditView" class="actionICNS"
																									ImageUrl="~/images/edit.png"
																									ToolTip="Click to Edit record." />
																							</td>
																							<td>
																								<asp:ImageButton ID="DeleteRecord" runat="server"
																									CommandArgument='<%# Eval("ID") %>' CausesValidation="false"
																									CommandName="DeleteRecord" class="actionICNS  largerActionICNS"
																									ImageUrl="~/images/delete.png"
																									ToolTip="Click to Delete record." />
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
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td align="right">
															<asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Sales Invoice"
																Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnBottomPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Sales Invoice List"
																Text="Print" CausesValidation="False" Visible="false"></asp:Button>
														</td>
														<td align="right">
															<asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Sales Invoice screen."
																Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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

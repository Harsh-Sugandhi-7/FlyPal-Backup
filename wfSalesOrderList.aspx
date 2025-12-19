<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesOrderList.aspx.vb"
	Inherits="Flypal.wfSalesOrderList" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
	<title>Sales Order List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	<link id="MainStyle" type="text/css" rel="stylesheet">

	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblMain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table class="clstablelistin" id="tblInner">
							<tr>
								<td nowrap colspan="3" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblSalesOrderList" runat="server" CssClass="clsFormHeader">List of Sales Order
                                            <asp:Label ID="lblTotal" runat="server"></asp:Label></asp:Label>
											</td>
											<td align="right" colspan="3">
												<table>
													<tr>
														<td>
															<asp:Button ID="btnAddNew" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Sales Order"
																Text="Add New" CausesValidation="False"></asp:Button>
														</td>
														<td>
															<asp:Button ID="BtnPrint" runat="server" CssClass="clsbtnH clsinfoH"
																ToolTip="Click to print list of Sales Order"
																Text="Print" CausesValidation="False"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnClose" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Sales Order screen"
																Text="Close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>

								</td>
							</tr>
							<tr>
								<td colspan="3">
									<asp:ValidationSummary ID="Validationsummary" runat="server" HeaderText="Fill Up The Following Information"
										CssClass="clsValidationSummary"></asp:ValidationSummary>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<table>
										<tr>
											<td>
												<asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Width="48px" Height="10px">Search</asp:Label>
											</td>
											<td>
												<table>
													<tr>
														<td>
															<asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="170px"
																AutoPostBack="True">
																<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
																<asp:ListItem Value="1">Date</asp:ListItem>
																<asp:ListItem Value="2">Sales Order</asp:ListItem>
																<asp:ListItem Value="3">Part No.</asp:ListItem>
																<asp:ListItem Value="4">Customer</asp:ListItem>
																<asp:ListItem Value="5">Quotation</asp:ListItem>
																<asp:ListItem Value="6">Status</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<asp:Label ID="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label>
														</td>
														<td>
															<p>
																<asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																	Visible="False">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																	<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																	<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																	<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																	<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																	<asp:ListItem Value="6">Between Dates</asp:ListItem>
																</asp:DropDownList>
																<asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="160px"
																	Visible="False">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">Opened</asp:ListItem>
																	<asp:ListItem Value="2">Authorized</asp:ListItem>
																	<asp:ListItem Value="4">Canceled</asp:ListItem>
																</asp:DropDownList>
																<asp:DropDownList ID="cmbQuotationText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																	Visible="False" DataTextField="Text" DataValueField="Text">
																</asp:DropDownList>
																<asp:DropDownList ID="cmbSalesOrderText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Height="28px"
																	AutoPostBack="True" Visible="False" DataTextField="Text" DataValueField="Text">
																</asp:DropDownList>
																<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Height="25px" Visible="False" MaxLength="100"></asp:TextBox>
															</p>
														</td>
														<td>
															<asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label>
														</td>
														<td align="left">
															<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Height="25px" Visible="False" MaxLength="4"></asp:TextBox>
														</td>
														<td align="left">
															<asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxMedium" Width="75px"
																Visible="False" MaxLength="4"></asp:TextBox>
														</td>
													</tr>
												</table>
											</td>
											<td align="right">
												<table>
													<tr>
														<td>
															<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Visible="False">From Date </asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Height="25px"
																CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>

															<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>

															<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														</td>
														<td>
															<asp:Label ID="lblToDate" runat="server" CssClass="clsLabel" Visible="False">To Date </asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Height="25px"
																CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>

															<cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>

															<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
									<asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Visible="false">Select Sales Order from the list. Click On Edit Link To Modify The Selected Sales Order.Click On Delete link To Delete The Selected Sales Order.Click On Add New button To Add A New Sales Order.</asp:Label>
								</td>
								<td align="right">
									<table>
										<tr>
											<td align="right">
												<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
													ToolTip="Click to find list of Sales Order as  per searching criteria" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Sales Order as per criteria : Record(s) found</asp:Label>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="3">
									<asp:GridView ID="dgSalesOrderList" runat="server" AllowPaging="True" AllowSorting="True"
										AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25" ShowHeaderWhenEmpty="True">
										<RowStyle CssClass="clsdgItem" />
										<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
										<AlternatingRowStyle CssClass="clsdgAltItem" />

										<Columns>
											<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
											<asp:BoundField DataField="DateFormatted" HeaderText="Date">
												<HeaderStyle></HeaderStyle>
												<ItemStyle Wrap="False"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="SalesOrderTextNo" SortExpression="SalesOrderTextNo" HeaderText="Number">
												<HeaderStyle Wrap="False"></HeaderStyle>
												<ItemStyle Wrap="False"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Customer">
												<HeaderStyle></HeaderStyle>
											</asp:BoundField>
											<asp:BoundField DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
												<HeaderStyle></HeaderStyle>
											</asp:BoundField>
											<asp:BoundField DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total">
												<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status">
												<HeaderStyle></HeaderStyle>
											</asp:BoundField>
											<asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
												<HeaderStyle></HeaderStyle>
											</asp:BoundField>
											<asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
												<HeaderStyle></HeaderStyle>
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
																			CommandName="EditView"
																			CssClass="actionICNS"
																			ImageUrl="~/images/edit.png"
																			ToolTip="Click to Edit record." />
																	</td>
																	<td>
																		<asp:ImageButton ID="DeleteRecord" runat="server"
																			CommandArgument='<%# Eval("ID") %>' CausesValidation="false"
																			CommandName="DeleteRecord"
																			CssClass="actionICNS  largerActionICNS"
																			ToolTip="Click to Delete record."
																			ImageUrl="~/images/delete.png" />
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

										<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
										<PagerStyle CssClass="paging" HorizontalAlign="Right" />
									</asp:GridView>
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

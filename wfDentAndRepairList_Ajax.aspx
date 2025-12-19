<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDentAndRepairList_Ajax.aspx.vb"
	Inherits="Flypal.DentAndRepairListPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Dent & Repair List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />

	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	
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
		<table class="clstablelistout Table-MaxWidth" id="tblMain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="2">
									<table width="100%">
										<tr>
											<td class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblHeader" runat="server"
																CssClass="clsFormHeader displayBlock"
																Text="Dent & Repair List" />
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNew" runat="server"
																					CssClass="clsbtnH clsinfoH"
																					ToolTip="Add New entry"
																					Text="Add New" CausesValidation="False" />
																			</td>
																			<td>
																				<asp:Button ID="btnPrint" runat="server"
																					CssClass="clsbtnH clsinfoH"
																					ToolTip="Print List Report"
																					Text="Print" CausesValidation="False" />
																			</td>
																			<td>
																				<asp:Button ID="btnClose" runat="server"
																					CssClass="clsbtnH clsinfoH"
																					ToolTip="Close this screen."
																					Text="Close" CausesValidation="False" />
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</td>
											<td id="tdFavICN" align="center">
												<span id="spFavICN">
													<i id="favICN" runat="server"
														onclick="fnMarkFavouriteUnFavourite(this)"
														class="fa fa-star fa-spin fa-5x circle-icon"></i>
												</span>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary"
												runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Information" />
										</ContentTemplate>
									</asp:UpdatePanel>

									<script type="text/javascript">

										function showTextField() {

											var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
											var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
											var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
											var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
											var DateIndex = $get("cmbDate").selectedIndex;

											if (DateIndex == 0) {
												txtFromDateobj.style.display = 'none';
												txtToDateobj.style.display = 'none';
												lblFromDateobj.style.display = 'none';
												lblToDateobj.style.display = 'none';
											}

										}

									</script>

								</td>
							</tr>
							<tr>
								<td>
									<fieldset id="fdsSearchInfoDet" class="clsFieldSetNewStyle" style="border-width: 1px">
										<legend id="lblSearchInfoDet" style="font-weight: bold"><b>Search Criteria</b></legend>
										<table width="100%">
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
																					<asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto" Width="78px">Report Date</asp:Label>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
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
																					<asp:Label ID="lblFromDate" runat="server"
																						CssClass="clsLabelAuto" Width="78px"
																						Text="From Date" />
																				</td>
																				<td>
																					<asp:TextBox runat="server" ID="txtFromDate"
																						CssClass="clsTextBoxTagSearchDate"
																						Width="100px" CausesValidation="true"
																						ValidationGroup="a" ClientIDMode="Static"
																						onchange="ValidateDateText(this,'FromDate_watermarkextender');"
																						AutoPostBack="True" />
																					<cc2:CalendarExtender ID="txtFromDate_CalendarExtender"
																						runat="server" CssClass="cal_Theme1" Enabled="true"
																						Format="<%$AppSettings:DateFormat%>"
																						TargetControlID="txtFromDate" />
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate"
																						ID="FromDate_watermarkextender" ClientIDMode="Static"
																						runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox" />
																					<asp:CustomValidator ID="cvFromDate" runat="server"
																						CssClass="clsLabelAuto" Display="None"
																						ClientValidationFunction="BetweenDatesValidation"
																						ValidationGroup="a"
																						ErrorMessage="From Date should not be greater than To Date " />
																				</td>
																				<td align="right">&nbsp;&nbsp;
																					<asp:Label ID="lblToDate" CssClass="clsLabelAuto"
																						runat="server" Width="78px"
																						Text="To Date" />
																				</td>
																				<td>
																					<asp:TextBox runat="server" ID="txtToDate"
																						CssClass="clsTextBoxTagSearchDate"
																						Width="100px" CausesValidation="true"
																						ValidationGroup="a" ClientIDMode="Static"
																						onchange="ValidateDateText(this,'ToDate_watermarkextender');"
																						AutoPostBack="True" />
																					<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server"
																						CssClass="cal_Theme1" Enabled="true"
																						Format="<%$AppSettings:DateFormat%>"
																						TargetControlID="txtToDate" />
																					<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate"
																						ID="ToDate_watermarkextender"
																						ClientIDMode="Static" runat="server"
																						WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox" />
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
												<td valign="top">
													<asp:Panel ID="pnlAdvancedSearch" runat="server">
														<asp:UpdatePanel ID="upnlAdvanceSearch" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table width="100%">
																	<tr>
																		<td>
																			<asp:Label runat="server" ID="lblCWPNo"
																				CssClass="clsLabelAuto" Text="Dent & Repair No." />
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbDentAndRepairNo" runat="server"
																				CssClass="clsTextBoxTagSearchComboNewstyle"
																				AutoPostBack="True" DataTextField="Text"
																				DataValueField="Text" />
																		</td>
																		<td>
																			<asp:Label ID="lblNo" runat="server"
																				CssClass="clsLabelAuto" Text="No." />
																		</td>
																		<td>
																			<asp:TextBox ID="txtNo" runat="server"
																				CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
																				ClientIDMode="Static" ToolTip="Enter Number"
																				AutoPostBack="True" Text="0" />
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<asp:Label runat="server" ID="lblStatus"
																				CssClass="clsLabelAuto" Text="Status" />
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbDentAndRepairStatus" runat="server"
																				AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle">
																				<asp:ListItem Text="(ALL)" Value="0"></asp:ListItem>
																				<asp:ListItem Text="Open" Value="1"></asp:ListItem>
																				<asp:ListItem Text="Authorized" Value="2"></asp:ListItem>
																			</asp:DropDownList>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="lblRegNo"
																				CssClass="clsLabelAuto" Text="Aircraft" />
																		</td>
																		<td>
																			<asp:TextBox ID="txtRegNo" runat="server"
																				CssClass="clsTextBoxTagSearch"
																				ToolTip="Enter Reg No."
																				AutoPostBack="True" />
																		</td>
																	</tr>
																</table>
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
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader" />
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:GridView ID="gvDentAndRepairList" runat="server"
															AllowSorting="True" AutoGenerateColumns="False"
															DataKeyNames="ID" AllowPaging="true" ShowHeaderWhenEmpty="True"
															PageSize="10" CssClass="clsGridNewStyle"
															GridLines="Horizontal" CellPadding="5">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
																ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First"
																LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging"
																ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--1--%>
																<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																<%--2--%>
																<asp:BoundField DataField="ReportDateFormatted"
																	HeaderText="Report Date">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="False" />
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="TextNo" HeaderText="Dent & Repair No."
																	SortExpression="TextNo">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																	<ItemStyle Wrap="False" />
																</asp:BoundField>
																<%--4--%>
																<asp:BoundField DataField="RegNo" HeaderText="Aircraft"
																	SortExpression="RegNo">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--5--%>
																<asp:BoundField DataField="LogTextNo" HeaderText="Log"
																	SortExpression="LogTextNo">
																	<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--6--%>
																<asp:BoundField DataField="Status" HeaderText="Status"
																	SortExpression="Status">
																	<HeaderStyle />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--7--%>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center"
																	HeaderText="Action" ItemStyle-HorizontalAlign="Center">
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
																							<asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Edit this Record." CausesValidation="false"
																								CommandName="EditRec" ImageUrl="~/images/edit.png" />
																						</td>

																						<td>
																							<asp:ImageButton ID="deleteICN" CssClass="actionICNS  largerActionICNS"
																								runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Delete this Record." CommandName="DeleteRec"
																								ImageUrl="~/images/delete.png" CausesValidation="false" />
																						</td>
																						<td>
																							<asp:ImageButton ID="viewICN" CssClass="FileAttachmentICN" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="View the Attachment added."
																								Visible='<%#  Eval("IsAttachmentAdded")%>'
																								CommandName="ViewRec" ImageUrl="icons/CLIP01.ICO"
																								CausesValidation="false" />
																						</td>
																					</tr>
																				</table>
																			</div>
																		</div>
																	</ItemTemplate>
																</asp:TemplateField>
																<asp:BoundField HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn"
																	DataField="IsAttachmentAdded"
																	HeaderText="IsAttachmentAdded" />
															</Columns>
															<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
															<PagerStyle CssClass="paging" HorizontalAlign="Right" />
														</asp:GridView>
													</td>
												</tr>
												<%--Added by Harsh on 3rd September 2024 for FLYPAL-1860 Resolving Issues related to Dent & Repair Module--%>
												<tr>
													<td colspan="2" align="right">
														<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table>
																	<tr>
																		<td>
																			<asp:Button ID="hdnBtnMarkFavourite"
																				ClientIDMode="Static" runat="server"
																				Text="----" CausesValidation="False"
																				Style="display: none;" />
																			<asp:Button ID="hdnBtnRemoveFavourite"
																				ClientIDMode="Static" runat="server"
																				Text="----" CausesValidation="False"
																				Style="display: none;" />
																		</td>
																	</tr>
																</table>
															</ContentTemplate>
														</asp:UpdatePanel>
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
		<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" 
			ID="clpAdvancedSearch"
			ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch"
			ExpandControlID="cpnlAdvancedSearch"
			CollapseControlID="cpnlAdvancedSearch" Collapsed="True" 
			ImageControlID="imgMasters"
			CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
			ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
			SuppressPostBack="false" />

		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" 
									ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>

		<script type="text/javascript">

			//From Date - To Date validation
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

		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				showTextField();
			});
		</script>

		<%--Added by Harsh on 3rd September 2024 for FLYPAL-1860 Resolving Issues related to Dent & Repair Module--%>
		<script type="text/javascript">

			function fnMarkFavouriteUnFavourite(x) {
				if (x.classList.contains("fa-star")) {
					x.classList.remove("fa-star");
					x.classList.add("fa-star-o");
					x.style.color = 'black';
					x.style.border = 'black';
					$("#hdnBtnRemoveFavourite").click();
				}
				else {
					x.classList.remove("fa-star-o");
					x.classList.add("fa-star");
					x.style.color = '#fff';
					x.style.border = 'black';
					$("#hdnBtnMarkFavourite").click();
				}
			}

			function MarkAsFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
					redstar.classList.add("fa-star");
					redstar.classList.remove("fa-star-o");
					redstar.style.color = '#fff';
					redstar.style.border = 'black';

			}

			function RemoveFromFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
				redstar.classList.add("fa-star-o");
				redstar.classList.remove("fa-star");
				redstar.style.border = 'black';
			}

		</script>

	</form>
</body>
</html>

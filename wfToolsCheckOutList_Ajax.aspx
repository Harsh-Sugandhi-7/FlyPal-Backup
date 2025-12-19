<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfToolsCheckOutList_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfToolsCheckOutList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Tools CheckOut List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<%-- Ajay 08-Nov-2022--%>
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>

	<script type="text/javascript">

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
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

	</script>

</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
	<form id="frmToolsCheckoutList" method="post" runat="server">
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
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td class="clsFormHeader1Newstyle" valign="middle">
														<table width="100%">
															<tr>
																<td>
																	<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
                                                                        List Of Tools Issued
																	</asp:Label>
																</td>
																<td align="right" colspan="2">
																	<asp:Button ID="btnAddNew" runat="server"
																		CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Item"
																		Text="Add New" CausesValidation="False"></asp:Button>
																	<asp:Button ID="btnClose" runat="server"
																		CssClass="clsbtnH clsinfoH" ToolTip="Click to Close List screen"
																		Text="Close" CausesValidation="False"></asp:Button>
																</td>
															</tr>
														</table>
													</td>
													<td id="tdFavICN" align="center">
														<span id="spFavICN">
															<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
																class="fa fa-star fa-spin fa-5x circle-icon"></i>
														</span>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
										HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
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
																				<span id="lblFromDate" class="clsLabel" runat="server">From Date</span>
																			</td>
																			<td>
																				<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
																					Width="100px" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
																				</asp:TextBox>
																				<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"
																					CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
																					TargetControlID="txtFromDate"></cc2:CalendarExtender>
																				<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate"
																					ID="FromDate_watermarkextender" ClientIDMode="Static" runat="server"
																					WatermarkText="<%$AppSettings:DateFormat%>"
																					WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																			</td>
																			<td>
																				<span id="lblToDate" class="clsLabel" runat="server">To Date</span>
																			</td>
																			<td>
																				<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
																					Width="100px"
																					onchange="ValidateDateText(this,'ToDate_watermarkextender');">

																				</asp:TextBox>
																				<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server"
																					CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
																					TargetControlID="txtToDate"></cc2:CalendarExtender>
																				<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate"
																					ID="ToDate_watermarkextender" ClientIDMode="Static" runat="server"
																					WatermarkText="<%$AppSettings:DateFormat%>"
																					WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																			</td>
																			<td></td>
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
																				<span id="lblDescriptionSearch" class="clsLabel">Description</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtDescriptionSearch" runat="server" CssClass="clsTextBoxTagSearch"
																					MaxLength="100">
																				</asp:TextBox>
																			</td>
																			<td>
																				<span id="Span3" class="clsLabel">Issue No.</span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbIssueText" runat="server"
																					CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
																					DataValueField="Text" DataTextField="Text">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<asp:TextBox ID="txtIssueNo" runat="server" 
																					CssClass="clsTextBoxTagSearchSmall"
																					MaxLength="6" Width="70px">
																				</asp:TextBox>
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
																<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
																	ToolTip="Click to search as per Criteria."
																	ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td valign="top" colspan="2">
														<asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
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
																	<table width="100%">
																		<tr>
																			<td>
																				<span id="lblFromStore" class="clsLabelAuto" style="width: 100%">From Store
																				</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtFromStore" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100">
																				</asp:TextBox>
																			</td>
																			<td>
																				<span id="Span2" class="clsLabel">Issued To Employee</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtIssuedToEmployee" runat="server" AutoComplete="off"
																					ClientIDMode="Static" OnTextChanged="IssuedToEmployee" AutoPostBack="true"
																					CssClass="clsTextBoxTagSearch"
																					onChange="SetEmpIdonChange('txtIssuedToEmployee','txtIssuedToEmployee_Autocomplete')">
																				</asp:TextBox>
																				<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtIssuedToEmployee_Autocomplete"
																					runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
																					MinimumPrefixLength="0" CompletionInterval="1"
																					ServicePath="wfToolsCheckOutList_Ajax.aspx" ServiceMethod="GetEmployeeList"
																					TargetControlID="txtIssuedToEmployee" OnClientItemSelected="SetID"
																					UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
																					CompletionListItemCssClass="ac_results_li"
																					CompletionListHighlightedItemCssClass="ac_over_Main"
																					OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
																					OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
																					OnClientShowing="ClientShowing">
																				</cc2:AutoCompleteExtender>
																				<asp:HiddenField ID="hdnIssuedToEmployeeId" runat="server" ClientIDMode="Static" />
																			</td>
																			<td>
																				<span id="Span6" class="clsLabelAuto" style="width: 100%">Category </span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
																					DataTextField="Name">
																				</asp:DropDownList>
																			</td>
																		</tr>
																	</table>
																</asp:Panel>
																<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
																	ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch"
																	ExpandControlID="ClpnlAdvancedSearch"
																	CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
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
									<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
                                                            List of Tools issued as per criteria :  Record(s) found.
														</asp:Label>
													</td>
													<td align="right">
														<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table>
																	<tr>
																		<td>
																			<span id="lblIssueTo" class="clsLabelAuto">Check Out Against</span>
																		</td>
																		<td>
																			<asp:DropDownList ID="cmbCheckOutAgainst" runat="server"
																				CssClass="clsTextBoxTagSearchComboNewstyle" Width="128px">
																				<asp:ListItem Value="19">Part List</asp:ListItem>
																				<%--Check Out Against part list So TypeID=19 which was default--%>
																				<asp:ListItem Value="18">Requisition</asp:ListItem>
																				<%--Check Out Against Requisition So TypeID=18--%>
																			</asp:DropDownList>
																		</td>
																	</tr>
																</table>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:GridView ID="dgIssueList" runat="server" AllowSorting="True"
																	ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" DataKeyNames="ID"
																	CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"
																		HorizontalAlign="Left" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																	<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																	<Columns>
																		<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																		<asp:BoundField DataField="ILDateFormatted" HeaderText="Date">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="IssueNo" SortExpression="IssueNo" HeaderText="Number">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Issued To">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Issued For Aircarft">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="WorkOrderNo" SortExpression="WorkOrderNo" HeaderText="Issued For WO">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		</asp:BoundField>
																		<asp:BoundField DataField="AuthorizedByName" SortExpression="AuthorizedByName"
																			HeaderText="Authorized By ">
																			<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
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
																									<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																										CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																										ToolTip="Click to Edit record"
																										CommandName="EditView" ImageUrl="~/images/edit.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
																										CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																										ToolTip="Click to Delete record"
																										CommandName="DeleteRecord" ImageUrl="~/images/delete.png" />
																								</td>
																							</tr>
																						</table>
																					</div>
																				</div>
																			</ItemTemplate>
																		</asp:TemplateField>
																	</Columns>
																</asp:GridView>
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
								<td colspan="2" align="right">
									<asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<%--Ajay 08-Nov-2022--%>
													<td>
														<asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
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

		<!-- Ajax Loader -->
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
		<%--Autocomplete functions to set id--%>
		<script type="text/javascript">
			function SetID(source, e) {
				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}

				var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
				source.get_element().value = text;

				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtReceivedByEmployee_Autocomplete") {
					textbox = document.getElementById('hdnReceivedByEmployeeId');
				}
				if (source._id == "txtIssuedToEmployee_Autocomplete") {
					textbox = document.getElementById('hdnIssuedToEmployeeId');
				}



				textbox.value = value.toString();
			}
			//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
			function SetEmpIdonChange(cntrl, extender) {
				var cntrlName = '#' + cntrl;
				var popup = $find(extender);
				var complist = popup.get_completionList();
				var text = $(cntrlName).val().toLowerCase();
				for (var i = 0; i < complist.childNodes.length; i++) {
					var texttocompare = complist.childNodes[i].innerText.toLowerCase();
					if (text == texttocompare) {
						var val = complist.childNodes[i]._value;
						if (cntrl == "txtReceivedByEmployee") {
							var textbox = document.getElementById('hdnReceivedByEmployeeId');
						}
						if (cntrl == "txtIssuedToEmployee") {
							textbox = document.getElementById('hdnIssuedToEmployeeId');
						}
						textbox.value = val.toString();
						return;
					}

				}
				if (cntrl == "txtReceivedByEmployee") {
					var textbox = document.getElementById('hdnReceivedByEmployeeId');
				}
				if (cntrl == "txtIssuedToEmployee") {
					textbox = document.getElementById('hdnIssuedToEmployeeId');
				}
				textbox.value = '';
				return;
			}

		</script>
		<!--Ajay S 07-Nov-2022 -->
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
		<!--Ajay E -->
	</form>
</body>
</html>

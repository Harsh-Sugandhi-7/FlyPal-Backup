<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnResourceAllocationAMO.aspx.vb" Inherits="Flypal.wfnResourceAllocationAMO" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Job(s) for Resource Allocation</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body>
	<form id="formJobAllocation" runat="server">
		<div>
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
				EnablePageMethods="true">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div>
			<table id="tblMain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td class="clsFormHeader1Newstyle" colspan="2">
										<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td valign="middle">
															<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
																W.O. Job List
															</asp:Label>
														</td>
														<td align="right">
															<asp:Button ID="btnCloseTop" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Job List"
																Text="Close" CausesValidation="False"></asp:Button>
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
										<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
											ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
											ValidationGroup="a">
										</asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
											CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a">
										</asp:RequiredFieldValidator>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table1">
													<tr>
														<td>
															<asp:Label ID="lblFromDate" CssClass="clsLabelAuto" runat="server" Width="78px">From Date</asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
																CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
															</asp:TextBox>
															<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
															<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
																ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date ">
															</asp:CustomValidator>
														</td>
														<td>
															<asp:Label ID="lblToDate" CssClass="clsLabelAuto" runat="server" Width="68px">To Date </asp:Label>
														</td>
														<td colspan="5">
															<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
																CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');">
															</asp:TextBox>
															<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														</td>

														<td>
															<span>W.O.</span>
														</td>
														<td>

															<asp:TextBox ID="txtWO" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
																AutoPostBack="true" AutoComplete="off" OnTextChanged="WO_TextChanged" onChange="setWOID(this,'txtWO_AutocompleteExtender')">
															</asp:TextBox>
															<cc2:AutoCompleteExtender ID="txtWO_AutocompleteExtender" runat="server" ClientIDMode="Static"
																DelimiterCharacters="" EnableCaching="false" CompletionInterval="1" CompletionListCssClass="ac_results_Main"
																CompletionListHighlightedItemCssClass="ac_over_Main" CompletionListItemCssClass="ac_results_li"
																CompletionSetCount="20" UseContextKey="false" ContextKey="" Enabled="true" MinimumPrefixLength="0"
																ServicePath="wfnResourceAllocationAMO.aspx" ServiceMethod="GetWOListAutoComplete" TargetControlID="txtWO"
																OnClientItemSelected="setID" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
																OnClientShowing="ClientShowing">
															</cc2:AutoCompleteExtender>
														</td>

														<td>
															<span>WO. Job Type</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbWOJobType" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" DataValueField="ID"
																DataTextField="Name">
															</asp:DropDownList>
														</td>
														<td colspan="2">
															<asp:CheckBox ID="chkUnallocatedJobs" runat="server" CssClass="clsLabelAuto" TextAlign="Right" Text='"UN-ALLOCATED" JOBS'></asp:CheckBox>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td align="right" valign="top">
										<table>
											<tr>
												<td>
													<asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
																CssClass="clsSearch2btn" ToolTip="Click to find" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<asp:PlaceHolder ID="plAction" runat="server" Visible="false">
									<tr>
										<td align="right">
											<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td>
																<asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List of Job List"
																	Visible="false" Enabled="<%# mWOJobList.Count > 0  %>" Text="Print"
																	CausesValidation="False"></asp:Button>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td colspan="2" align="left">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Work Order Jobs as per criteria :  Record(s) found.</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td align="right">
													<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															&nbsp;
                                                                    <asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
															&nbsp;
                                                                <asp:DropDownList ID="cmbShowE" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="55px"
																	AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
																	<asp:ListItem Value="0">5</asp:ListItem>
																	<asp:ListItem Value="1">10</asp:ListItem>
																	<asp:ListItem Value="2">15</asp:ListItem>
																	<asp:ListItem Value="3">20</asp:ListItem>
																	<asp:ListItem Value="4">25</asp:ListItem>
																	<asp:ListItem Value="5">30</asp:ListItem>
																	<asp:ListItem Value="6">40</asp:ListItem>
																	<asp:ListItem Value="7">45</asp:ListItem>
																	<asp:ListItem Value="8">50</asp:ListItem>
																	<asp:ListItem Value="9">55</asp:ListItem>
																</asp:DropDownList>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
											<tr>
												<td align="right" colspan="2">
													<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
														<ContentTemplate>

															<asp:GridView ID="dgWOJobList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
																CellPadding="5" ForeColor="Black" ShowHeaderWhenEmpty="true" AllowPaging="True" PageSize="25"
																GridLines="Horizontal" AllowSorting="True">
																<SelectedRowStyle></SelectedRowStyle>
																<EditRowStyle></EditRowStyle>
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																<FooterStyle Wrap="False"></FooterStyle>
																<Columns>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																	<asp:BoundField DataField="TaskNo" HeaderText="Task No / Directive No" SortExpression="TaskNo">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																		<FooterStyle Wrap="False"></FooterStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="WODateFormatted" HeaderText="Date">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																		<FooterStyle Wrap="False"></FooterStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="WONumber" SortExpression="WONo" HeaderText="W.O. No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="CustomerInformation" SortExpression="CustomerInformation" HeaderText="Customer Info." HtmlEncode="false">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="True"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ModelNo" SortExpression="ModelNo" HeaderText="Model">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="WOJobDescription" SortExpression="WOJobDescription" HeaderText="Job Description">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="True"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="WOJobType" SortExpression="WOJobType" HeaderText="Job Type">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Skill" SortExpression="Skill" HeaderText="Skill">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="EmployeeNames" SortExpression="EmployeeNames" HeaderText="Resource(s)">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="True"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Zone" SortExpression="Zone" HeaderText="Zone">
																		<HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="True"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Area" SortExpression="Area" HeaderText="Area"
																		HtmlEncode="False">
																		<HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="True"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="WOJobEstimatedTime" SortExpression="WOJobEstimatedTime" HeaderText="Estimated Time">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:ButtonField Text="Allocate" HeaderText="Allocate" CommandName="Allocate" ControlStyle-ForeColor="Blue"></asp:ButtonField>
																</Columns>
																<PagerSettings FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															</asp:GridView>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:Button ID="btnBottomPrint" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to Print Job List"
																Visible="false" Enabled="<%# mWOJobList.count>0 %>"
																Text="Print" CausesValidation="False"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnClose" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Job List"
																Text="Close" CausesValidation="False" Visible="false"></asp:Button>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr style="height: 0px;">
									<td style="height: 0px;">
										<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
											<ContentTemplate>
												<asp:Button ID="hdnBtnResourceAllocation" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
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

			<%-- Autocomplete functions to set id--%>
			<asp:HiddenField runat="server" ClientIDMode="Static" ID="SelectedWOID" />
			<asp:HiddenField runat="server" ClientIDMode="Static" ID="InccorectEmployee" />
			<script type="text/javascript">
				function setID(source, e) {
					//get id from autocomplete list
					var node;
					var value = e.get_value();

					if (value) node = e.get_item();
					else {
						value = e.get_item().parentNode._value;
						node = e.get_item().parentNode;
					}
					//Set id to relevent hidden field 
					var textbox;
					if (source._id == "txtWO_AutocompleteExtender") {
						textbox = document.getElementById('SelectedWOID');

					}


					textbox.value = value;
					return;
				}
				//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
				function setWOID(source, extenderid) {
					var popup = $find(extenderid);
					var complist = popup.get_completionList();
					var text = $(source).val().toLowerCase();
					for (var i = 0; i < complist.childNodes.length; i++) {
						document.getElementById('InccorectEmployee').value = '';
						var texttocompare = complist.childNodes[i].innerText.toLowerCase();
						if (text == texttocompare) {
							var val = complist.childNodes[i]._value;

							if (extenderid == "txtWO_AutocompleteExtender") {
								textbox = document.getElementById('SelectedWOID');
							}

							textbox.value = val;

							return;
						}
					}

					if (extenderid == "txtWO_AutocompleteExtender") {
						document.getElementById('SelectedWOID').value = '';
						document.getElementById('InccorectEmployee').value = text;
					}
				}
			</script>

		</div>

		<!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->

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

		<!-- ResourceAllocation popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyResourceAllocation" Text="Resource Allocation" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlResourceAllocation" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeResourceAllocation" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupResourceAllocation" runat="server" TargetControlID="btnDummyResourceAllocation"
			PopupControlID="pnlResourceAllocation" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameResourceAllocationComplete() {
				$("#btnDummyResourceAllocation").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenResourceAllocationWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeResourceAllocation").attr("src", "wfnResourceAllocationForAMOJob.aspx?Type=pup");

					//if (!$.browser.msie) {
					$("#btnDummyResourceAllocation").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					//}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForResourceAllocation() {
				var ResourceAllocationwindow = $find("<%=mdlPopupResourceAllocation.ClientID %>");
				//close Task Card Tool popup window
				ResourceAllocationwindow.hide();
				//           release resources
				$("#IframeResourceAllocation").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnResourceAllocation").click();
			}
		</script>
		<!-- End-->

	</form>
</body>
</html>

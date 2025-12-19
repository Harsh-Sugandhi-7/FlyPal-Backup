<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTransTextSeries_Ajax.aspx.vb"
	Inherits="Flypal.wfTransTextSeries_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Transaction Text Series</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>
	<script type="text/javascript">
		function showNestedGridView(obj) {
			var nestedGridView = document.getElementById(obj);
			var imageID = document.getElementById('image' + obj);

			if (nestedGridView.style.display == "none") {
				nestedGridView.style.display = "inline";
				imageID.src = "images/minus.png";
			} else {
				nestedGridView.style.display = "none";
				imageID.src = "images/plus.png";
			}
		}
	</script>
	<style type="text/css">
		.actionICNS {
			height: 15px;
			width: 15px;
		}

		.largerActionICNS {
			height: 20px;
			width: 20px;
		}

		.actionICNSAlignment {
			margin-top: 5px;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr class="clsFormHeader1Newstyle">
									<td>
										<table width="100%">
											<tr>
												<td>
													<asp:Label ID="lblTransTextSeries" runat="server" CssClass="clsFormHeader">Transaction Text Series
													</asp:Label>
												</td>
												<td align="right">
													<asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH"
														Text="New" ToolTip="Click to Add New Transaction Series" />
													<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
														Text="Save" ToolTip="Click to Save the Transaction Series" />
													<asp:Button ID="btnClose" runat="server" CausesValidation="False"
														CssClass="clsbtnH clsinfoH" ToolTip="Click to Close" Text="Close" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvBaseType" runat="server" Display="None" ErrorMessage="Select Base Type form the List."
													ControlToValidate="cmbBaseTypeList" OnServerValidate="CustomValidate"></asp:CustomValidator>
												<asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage="Select Base Type form the List."
													ControlToValidate="cmbBaseTypeList" OnServerValidate="CustomValidate1"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvBaseType" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="Base Type Required" Display="None" ControlToValidate="cmbBaseTypeList"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="From Date Required" Display="None" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="To Date Required" Display="None" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
													Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
													ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="clsLabelAuto"
													Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
													ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
													Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlTransTextSeries" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td></td>
														<td colspan="3">
															<asp:Label ID="lblInfoText1" runat="server" CssClass="clsLabel" Font-Bold="True"
																Style="text-align: justify" Width="775px">
																[TransName] transaction you are trying to save for the date [TransDate], has no Series mentioned in the system.
															</asp:Label>
														</td>
													</tr>
													<tr>
														<td></td>
														<td colspan="3">
															<asp:Label ID="lblInfoText3" runat="server" CssClass="clsLabel" Style="text-align: justify"
																Width="775px">
																You are here to enter series for the valid date period along with starting no. for the transaction. 
																This series will be used to generate transaction number i.e. ORD-2024-25. 
																So that, you don&#39;t need to enter it everytime while creating new transaction.
															</asp:Label>
														</td>
													</tr>
													<tr>
														<td></td>
														<td colspan="3">
															<asp:Label ID="lblInfoText2" runat="server" CssClass="clsLabel" Style="text-align: justify; margin-top: 5px"
																Width="555px"></asp:Label>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<asp:Label ID="lblTransactionDate" runat="server" CssClass="clsLabelAuto">Transaction Date</asp:Label>
														</td>
														<td>
															<asp:Label ID="lblTransactionDateValue" runat="server" CssClass="clsLabelAuto" Font-Bold="True"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblName6" runat="server" CssClass="clsLabelStar">*</asp:Label>
														</td>
														<td>
															<asp:Label ID="lblBaseType" runat="server" CssClass="clsLabel"> Base Type</asp:Label>
														</td>
														<td>
															<asp:DropDownList ID="cmbBaseTypeList" runat="server" AutoPostBack="True"
																CssClass="clsTextBoxTagSearchComboNewstyle"
																DataTextField="Name" DataValueField="ID" Width="200px">
															</asp:DropDownList>
														</td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td>
															<asp:Label ID="lblDatePeriodFormat" runat="server" CssClass="clsLabel">Date Period</asp:Label>
														</td>
														<td>
															<table class="clsMenu">
																<tr>
																	<td>
																		<asp:RadioButton ID="rdbFinancialYear" runat="server" AutoPostBack="True" CssClass="clsRadioButton"
																			GroupName="Template" Text="Financial Year" />
																	</td>
																	<td>
																		<asp:RadioButton ID="rdbCalendarYear" runat="server" AutoPostBack="True" CssClass="clsRadioButton"
																			GroupName="Template" Text="Calendar Year" />
																	</td>
																	<td>
																		<asp:RadioButton ID="rdbCustom" runat="server" AutoPostBack="True" CssClass="clsRadioButton"
																			GroupName="Template" Text="Custom" />
																	</td>
																</tr>
															</table>
														</td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td align="right">
															<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
																AutoPostBack="True"></asp:TextBox>
															<cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
															<asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
																ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
														</td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td align="right">
															<asp:Label ID="lblToDate" runat="server" CssClass="clsLabel">To Date</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtToDate" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
																ClientIDMode="Static" runat="server" AutoPostBack="True"></asp:TextBox>
															<cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														</td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td>
															<asp:Label ID="lblBaseType4" runat="server" CssClass="clsLabel"> Auto Renew</asp:Label>
														</td>
														<td>
															<asp:CheckBox ID="chkAutoRenew" runat="server" CssClass="clsCheckBox" Text="YES ( Not available for Custom option )" />
														</td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td></td>
														<td></td>
														<td>
															<br />
														</td>
													</tr>
													<tr>
														<td colspan="4">
															<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Transaction Series</asp:Label>
														</td>
													</tr>
													<tr>
														<td colspan="4">
															<asp:UpdatePanel ID="upnldgTransSeriesDetails" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:GridView ID="dgTransSeriesDetails" runat="server" AutoGenerateColumns="False"
																		ShowHeaderWhenEmpty="True" ToolTip="Transaction Series list"
																		Width="100%" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<RowStyle CssClass="clsdgItem" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																		<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																		<Columns>
																			<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																			<asp:BoundField DataField="TransTypeName" HeaderText="Transaction">
																				<HeaderStyle HorizontalAlign="Left" Width="40%" />
																				<ItemStyle HorizontalAlign="Left" Width="40%" />
																			</asp:BoundField>
																			<asp:TemplateField HeaderText="Text Series (Prefix-Suffix)">
																				<ItemTemplate>
																					<asp:TextBox ID="txtPrefix" runat="server" CssClass="clsTextBoxTagSearch" Text='<%# DataBinder.Eval(Container.DataItem, "TransText") %>'
																						Width="185px"></asp:TextBox>
																					<%----%>
																					<asp:DropDownList ID="cmbSuffixList" runat="server" AutoPostBack="False" CssClass="clsTextBoxTagSearchComboNewstyle"
																						DataTextField="Suffix" DataValueField="Suffix" Width="100px">
																					</asp:DropDownList>
																					<asp:CustomValidator ID="cvPrefix" runat="server" ControlToValidate="txtPrefix" Display="None"
																						OnServerValidate="customvalidate1"></asp:CustomValidator>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Left" />
																				<ItemStyle HorizontalAlign="Left" />
																			</asp:TemplateField>
																			<asp:TemplateField HeaderText="Starting No.">
																				<ItemTemplate>
																					<asp:TextBox ID="txtStartingTransNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="8"
																						Text='<%# DataBinder.Eval(Container.DataItem, "StartingTransNo") %>' Width="50px"></asp:TextBox>
																					<asp:CustomValidator ID="cvStartingTransNo" runat="server" ControlToValidate="txtStartingTransNo"
																						Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Left" Width="5%" />
																				<ItemStyle HorizontalAlign="Center" Width="5%" />
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
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlTransSeriesGrid" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Transaction Series as per criteria : Record(s) found.</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:GridView ID="dgTransactionSeriesList" runat="server" AutoGenerateColumns="False"
																Width="100%" BorderStyle="Solid" ForeColor="#333333"
																AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
																SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
																PagerSettings-Mode="NextPreviousFirstLast" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:TemplateField>
																		<ItemTemplate>
																			<a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
																				<img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/plus.png" />
																			</a>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
																	<asp:BoundField DataField="BaseTransTypeName" HeaderText="Base Type">
																		<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DatePeriodFormat" HeaderText="Date Period">
																		<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
																		<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
																		<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																	</asp:BoundField>
																	<asp:BoundField DataField="AutoRenewFormatted" HeaderText="Auto Renew">
																		<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
																		<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																		<HeaderStyle HorizontalAlign="Center" />
																		<ItemStyle HorizontalAlign="Center" />
																		<ItemTemplate>
																			<div id="dropDownImg" class="dropdown">
																				<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																				<div id="dropdownICN-content" class="dropdownbtn-content">
																					<table id="dropdown-content" class="clsGridNew_Ajax">
																						<tr>
																							<td>
																								<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																									CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																									ToolTip="Click to Edit record" CausesValidation="False"
																									CommandName="EditRecord" ImageUrl="~/images/edit.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
																									CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									ToolTip="Click to Delete record" CausesValidation="False"
																									CommandName="DeleteRecord" ImageUrl="~/images/delete.png" />
																							</td>
																						</tr>
																					</table>
																				</div>
																			</div>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:TemplateField>
																		<ItemTemplate>
																			<tr>
																				<td colspan="100%" bgcolor="White" width="0px">
																					<div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
																						<asp:GridView ID="grdTransTextseries" runat="server" AutoGenerateColumns="False"
																							Width="95%" BorderStyle="Solid" AlternatingRowStyle-CssClass="alt"
																							RowStyle-Wrap="false" HeaderStyle-Wrap="false"
																							SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
																							CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																							<AlternatingRowStyle CssClass="clsdgAltItem" />
																							<RowStyle CssClass="clsdgItem" />
																							<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																							<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																							<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																							<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																							<Columns>
																								<asp:BoundField DataField="TransTypeName" HeaderText="Transaction">
																									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="300px" />
																									<HeaderStyle Font-Bold="true" HorizontalAlign="left" Width="300px" />
																								</asp:BoundField>
																								<asp:BoundField DataField="TransText" HeaderText="Text Series">
																									<ItemStyle HorizontalAlign="Left" Wrap="false" />
																									<HeaderStyle Font-Bold="true" HorizontalAlign="left" />
																								</asp:BoundField>
																								<asp:BoundField DataField="StartingTransNo" HeaderText="Start">
																									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="33px" />
																									<HeaderStyle Font-Bold="true" HorizontalAlign="left" Width="33px" />
																								</asp:BoundField>
																							</Columns>
																						</asp:GridView>
																					</div>
																				</td>
																			</tr>
																		</ItemTemplate>
																	</asp:TemplateField>
																</Columns>
																<SelectedRowStyle BackColor="ControlDark" />
																<AlternatingRowStyle CssClass="clsdgAltItem" />
															</asp:GridView>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td></td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
				runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
					</div>
					<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
									Height="48px" Width="48px" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		</div>
		<%--Date Validations--%>
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

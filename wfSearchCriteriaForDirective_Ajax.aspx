<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForDirective_Ajax.aspx.vb"
	Inherits="Flypal.wfSearchCriteriaForDirective_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Directive Report</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
	<link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<%-- Ajay 09-Nov-2022--%>
	<link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
	<link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
		rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
	<script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>

	<script type="text/javascript">

		function DirectiveMultiSelect() {
			$('[id*=ListDirectiveType],[id*=ListDirectiveSubType]').multiselect({
				onDropdownShow: function (event) {
					var i = 1;
				},
				enableFiltering: true,
				enableCaseInsensitiveFiltering: true,
				includeSelectAllOption: true,
				disableIfEmpty: true,
				maxHeight: 180,
				nonSelectedText: '(Select)',
				selectAllJustVisible: false,
				buttonWidth: '185px',
				buttonHeight: '120px',
				allSelectedText: 'Directives',
				nSelectedText: 'Directives'
			});
			$(".caret").css('float', 'right');
			$(".caret").css('margin', '8px 0');
		}

		function disableDirectiveSubType() {
			$('[id*=ListDirectiveSubType]').multiselect('clearSelection', true);
			$('[id*=ListDirectiveSubType]').multiselect('disable', false);
		}

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			DirectiveMultiSelect();
		});

	</script>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>

		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>

		<div>
			<table class="clstablelistout" id="tblmain" border="0">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="tblInner" class="clstablelistin" border="0">
										<tr>
											<td colspan="4">
												<table width="100%">
													<tr>
														<td class="clsFormHeader1Newstyle">
															<span id="lbltitle" class="clsFormHeader" style="width: 100%">
																Search criteria for Directive Report
															</span>
														</td>
														<td id="tdFavICN" align="center">
															<span id="spFavICN">
																<i id="FavIClk" runat="server"
																	onclick="FunctionFav(this)"
																	class="fa fa-star fa-spin fa-5x circle-icon"
																	title="Mark As Favourites">
																</i>
															</span>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:ValidationSummary ID="Validationsummary2" runat="server"
															HeaderText="Fill Up The Following Fields"
															CssClass="clsValidationSummary" ValidationGroup="valGroup1" />
														<asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto"
															ClientValidationFunction="ValidateAircraft"
															Display="None" ControlToValidate="cmbAircraft"
															ErrorMessage="Please select the Aircraft and Assembly"
															ValidationGroup="valGroup1" />
														<asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto"
															OnServerValidate="CustomValidate"
															Display="None" ControlToValidate="cmbAssembly"
															ErrorMessage="Please select the Directive"
															ValidationGroup="valGroup1" />
														<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
															Display="None" ControlToValidate="txtFromDate" ErrorMessage="As On Date Required"
															ValidationGroup="valGroup1" />
													</ContentTemplate>
												</asp:UpdatePanel>
												<!-- Client side validation for comboboxes-->
												<script type="text/javascript">
													//Aircraft
													function ValidateAircraft(source, args) {
														args.IsValid = false;
														var dd = $get("cmbAircraft");
														if (dd.selectedIndex != 0) {
															args.IsValid = true;
															return;
														}
													}
													function ValidateCheckBox(source, args) {
														args.IsValid = false;
														var IsAssemblyChecked = $("#chkIsAssemblyDirectivesRequired").prop('checked');
														var IsCompChecked = $("#chkIsCompDirectivesRequired").prop('checked');
														if (IsAssemblyChecked || IsCompChecked) {
															args.IsValid = true;
															return;
														}
													}
												</script>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table width="100%">
															<tr>
																<td colspan="3">
																	<span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
																</td>
															</tr>
															<tr>
																<td width="12px"></td>
																<td width="68px">
																	<span id="lblFromDate" class="clsLabelAuto">As On Date</span>
																</td>
																<td>
																	<asp:TextBox CssClass="clsTextBoxTagSearchDate"
																		runat="server" ID="txtFromDate"
																		onchange="ValidateDateText(this,'txtFromDate_watermarkextender');"
																		Height="25px" />
																	<cc2:CalendarExtender ID="txtFromDate_CalendarExtender"
																		runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>"
																		TargetControlID="txtFromDate" />
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate"
																		ID="txtFromDate_watermarkextender"
																		ClientIDMode="Static" runat="server"
																		WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsDateTextBox" />
																</td>
															</tr>
															<tr>
																<td colspan="3">
																	<span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblAircraftStar1" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
																</td>
																<td>
																	<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle"
																		ID="cmbAircraft" runat="server" DataTextField="RegNo"
																		DataValueField="ID" AutoPostBack="True">
																	</asp:DropDownList>
																</td>
															</tr>
															<tr>
																<td colspan="3">
																	<span id="lblStep3" class="clsLabelHeader">Step III. Selection of Assembly</span>
																</td>
															</tr>
															<tr>
																<td></td>
																<td>
																	<asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
																</td>
																<td>
																	<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong"
																		ID="cmbAssembly" runat="server" DataTextField="ModelSerialNoPostion"
																		DataValueField="ID">
																	</asp:DropDownList>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Directive</span>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblTypeStar1" class="clsLabelStar">*</span>
											</td>
											<td>
												<span id="lblType" class="clsLabelAuto">Directive</span>
											</td>
											<td>
												<asp:Panel ID="pnlDirectiveType" runat="server">
													<asp:UpdatePanel runat="server" ID="upnlDirectiveType" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:ListBox ID="ListDirectiveType" runat="server"
																ClientIDMode="Static" SelectionMode="Multiple"
																AutoPostBack="true" DataTextField="Name" DataValueField="ID" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</asp:Panel>
											</td>
											<td>
												<asp:CheckBox ID="chkIsAssemblyDirectivesRequired" runat="server" CssClass="clsCheckBox"
													ClientIDMode="Static" Checked="true" Text="Show Assembly Directives" />
											</td>
										</tr>
										<tr>
											<td>
												<span id="Span1" class="clsLabelStar">*</span>
											</td>
											<td>
												<span id="Span2" class="clsLabelAuto">Type</span>
											</td>
											<td>
												<asp:Panel ID="Panel1" runat="server">
													<asp:UpdatePanel runat="server" ID="upnlDirectiveSubType" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:ListBox ID="ListDirectiveSubType" runat="server"
																ClientIDMode="Static" SelectionMode="Multiple"
																DataTextField="CodeType" DataValueField="ID" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</asp:Panel>
											</td>
											<td>
												<asp:CheckBox ID="chkIsCompDirectivesRequired" runat="server" CssClass="clsCheckBox"
													ClientIDMode="Static" Text="Show Component Directives" />
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<span id="lblStep5" class="clsLabelHeader">Step V. Selection of Open or Closed</span>
											</td>
											<td></td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblTypeOC" class="clsLabelAuto">Type</span>
											</td>
											<td colspan="2">
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cnbAdType" runat="server">
													<asp:ListItem Value="0">All</asp:ListItem>
													<asp:ListItem Value="1">Open</asp:ListItem>
													<asp:ListItem Value="2">Closed</asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="Label1" class="clsLabelHeader">Step VI. Selection of Format</span>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblFormat" class="clsLabelAuto">Format</span>
											</td>
											<td>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat"
													runat="server" AutoPostBack="true">
													<asp:ListItem Value="0">Format 1</asp:ListItem>
													<asp:ListItem Value="1">Format 2</asp:ListItem>
													<asp:ListItem Value="2">Format 3</asp:ListItem>
												</asp:DropDownList>
											</td>
											<td>
												<table id="Table6" border="0" cellspacing="0" cellpadding="0" width="100%">
													<tr>
														<td>
															<asp:RadioButton ID="optAscending" runat="server" CssClass="clsRadioButton" Text="Ascending"
																AutoPostBack="true" Checked="True" GroupName="grOrientation" />
														</td>
														<td>
															<asp:RadioButton ID="optDescending" runat="server" CssClass="clsRadioButton" Text="Descending"
																AutoPostBack="true" GroupName="grOrientation" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblSortBy" class="clsLabelHeader">Step VII. Selection of Sorting Criteria</span>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblSort" class="clsLabelAuto">Sort By</span>
											</td>
											<td colspan="2">
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSortBy" runat="server">
													<asp:ListItem Value="0" Selected="True">Directive No.</asp:ListItem>
													<asp:ListItem Value="1">Issue Date</asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblSelectionofIssuingAuthority" class="clsLabelHeader">
													Step VIII. Selection of Issuing Authority</span>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblIssuingAuthority" class="clsLabelAuto">Issuing Authority</span>
											</td>
											<td colspan="2">
												<asp:DropDownList ID="cmbIssuingAuthority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
													DataTextField="Name"
													DataValueField="ID">
												</asp:DropDownList>
											</td>
										</tr>

										<tr>
											<td colspan="4">
												<span id="Label2" class="clsLabelHeader">Step IX. Bottom Line of Report</span>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="Label3" class="clsLabelAuto">Enter Line which you want to print at the bottom
                                            of the report.</span>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:TextBox CssClass=" clsTextBoxTagSearchMultilineNewstyle"
													ID="txtBottomLine" runat="server"
													Width="552px" MaxLength="500" TextMode="MultiLine"
													ToolTip="Enter Note">
												I hereby certify that the data specified above has been verified throughout.
												Planning Manager: __________________ License No.: __________ Date: _____________
												</asp:TextBox>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblStep6" class="clsLabelHeader">Step X. Display Report</span>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" />
																</td>
															</tr>
															<tr>
																<td>
																	<asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" />
																</td>
															</tr>
															<tr>
																<td>
																	<asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" />
																</td>
															</tr>
															<tr>
																<td>
																	<asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" />
																</td>
															</tr>
															<tr>
																<td>
																	<asp:Label ID="lblDirType" runat="server" Width="600px" CssClass="clsLabelAuto" />
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td colspan="4" align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table1" border="0" cellspacing="0">
															<tr>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
																		Text="Current Criteria" ToolTip="Click to Display Current Searching criterias."
																		CausesValidation="False" />
																</td>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" TabIndex="0"
																		Text="Export to Excel" ToolTip="Click to Export report" ValidationGroup="valGroup1"
																		Visible="<%$AppSettings:ShowExportToExcelButton%>" />
																</td>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"
																		Text="Display" ToolTip="Click to Display Report" ValidationGroup="valGroup1" />
																</td>
																<%-- 'Added by Shital on 14-Sep-2016--%>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
																		ToolTip="Click to receive Report through mail" ValidationGroup="valGroup1" />
																</td>
																<td>
																	<asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
																		ToolTip="Click to Close" CausesValidation="False" />
																</td>
																<td>
																	<%--Ajay 09-Nov-2022--%>
																	<asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
																		Style="display: none;" />
																	<asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
																		CausesValidation="False" Style="display: none;" />
																</td>
															</tr>
															<!-- Dummy panel to open modelpopup 'Added by Shital on 14-Sep-2016 -->
															<tr style="height: 0px;">
																<td style="height: 0px;" colspan="2" align="right">
																	<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
																		<ContentTemplate>
																			<asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
																				CausesValidation="False" Style="display: none;" />
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
															</tr>
															<!--End -->
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
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

		<!-- Popup For Report By Mail 14-Sep-2016-->
		<div id="ModalPopUp">

			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
					scrolling="auto" allowtransparency="true"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
				PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
				function OpenByMaiWindow() {
					try {
						$("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
						$("#btnDummyReceipt1").click();

						return false;
					} catch (e) {
						alert(e);
					}

				}
				function ParentCallBackFunctionForSendMail() {
					var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
					//close popup window
					Receiptwindow1.hide();
					//           release resources
					$("#IframeReceipt1").attr("src", "JavaScript:''");
				}
				function ParentCallBackFunctionToSendMail() {
					var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
					//close popup window
					Receiptwindow1.hide();
					//           release resources
					$("#IframeReceipt1").attr("src", "JavaScript:''");
					//call image button
					$("#hdnimgLogBtnSendMail").click();
				}
			</script>
			<!---End-->

		</div>

		<!--Ajay S 09-Nov-2022 -->
		<script type="text/javascript">
			function FunctionFav(x) {
				if (x.classList.contains("fa-star")) {
					x.classList.remove("fa-star");
					x.classList.add("fa-star-o");
					x.style.color = 'black';
					x.style.border = 'black';
					$("#hdnBtnRemoveFav").click();
				}
				else {
					x.classList.remove("fa-star-o");
					x.classList.add("fa-star");
					x.style.color = '#fff';
					x.style.border = 'black';
					$("#hdnBtnMarkFav").click();
				}
			}
			function MarkFav() {
				var redstar = document.getElementById("<%=FavIClk.ClientID%>");
				redstar.classList.add("fa-star");
				redstar.classList.remove("fa-star-o");
				redstar.style.color = '#fff';
				redstar.style.border = 'black';

			}
			function RemoveFav() {
				var redstar = document.getElementById("<%=FavIClk.ClientID%>");
				redstar.classList.add("fa-star-o");
				redstar.classList.remove("fa-star");
				redstar.style.border = 'black';
			}
		</script>

	</form>
	
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
	
</body>
</html>

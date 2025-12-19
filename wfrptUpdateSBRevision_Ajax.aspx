<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUpdateSBRevision_Ajax.aspx.vb"
	Inherits="Flypal.wfrptUpdateSBRevision_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Update Model Directive Revision No.</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">
		.maxGridWidth {
			max-width: 350px;
		}

		#tr-SearchDropdown {
			display: block;
			margin-top: 10px;
		}
	</style>
	<script type="text/javascript" id="clientEventHandlersJS">
		function openFile() {
			str = "wfFileView.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<table class="clstablelistin" id="tblInner">
								<tr>
									<td colspan="3" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span id="lbltitle" class="clsFormHeader">Update Model Directive Revision No.
													</span>
												</td>
												<td align="right">
													<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<table id="tblHeaderButtons">
																<tr>
																	<td>
																		<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to close Update Model Directive Revision No. Screen"
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
								<tr id="tr-SearchDropdown">
									<td align="left">
										<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table1">
													<tr>
														<td>
															<span id="lblSearch" class="clsLabelAuto">Search</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" onChange="ControlVisibilityForSearch();">
																<asp:ListItem Value="0">All</asp:ListItem>
																<asp:ListItem Value="1">Model</asp:ListItem>
																<asp:ListItem Value="2">ATA Code</asp:ListItem>
																<asp:ListItem Value="3">Description</asp:ListItem>
																<asp:ListItem Value="4">Reference</asp:ListItem>
																<asp:ListItem Value="5">Directive No.</asp:ListItem>
																<asp:ListItem Value="6">Directive Type</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<span id="lblFor" class="clsLabelAuto" style="display: none;">For</span>
														</td>
														<td>
															<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Style="display: none;"
																TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
															<asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" Style="display: none;"
																ToolTip="Enter value." MaxLength="4"></asp:TextBox>
															<asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																Style="display: none;" DataTextField="ModelName" Width="185px">
															</asp:DropDownList>
															<asp:DropDownList ID="cmbDirectiveType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
																Style="display: none;" DataTextField="CodeType" DataValueField="ID">
															</asp:DropDownList>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td align="right" width="950px">
										<asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table4" width="100%">
													<tr>
														<td align="right">
															<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
																ToolTip="Click to seacrh as per searching Criteria"
																ValidationGroup="1" CausesValidation="false" />
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
														<td align="left">
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
														</td>
													</tr>
													<tr>
														<td align="left">
															<asp:GridView ID="dgModelMonitorModList" runat="server" AllowSorting="True" AllowPaging="True"
																DataKeyNames="ID" AutoGenerateColumns="False" PageSize="10"
																ClientIDMode="Static" EnableViewState="false" ShowHeaderWhenEmpty="true"
																CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
																		<HeaderStyle Wrap="False" Width="125px" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="CodeNumber" SortExpression="CodeNumber" HeaderText="Code/Form No.">
																		<HeaderStyle Wrap="False" Width="125px" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="true" CssClass="maxGridWidth"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive No.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="false"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Applicability" SortExpression="Applicability" HeaderText="Applicability">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:ButtonField Text="Update" HeaderText="Update Revision No." CommandName="ChangeExpiryInfo">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:ButtonField>
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
			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
		<!-- Change Expiry Info-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyChangeExpiryInfo" Text="Dummy Change Expiry Info" />
		</div>
		<asp:Panel runat="server" ID="pnlChangeExpiryInfo" Style="display: none">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlChangeExpiryInfo">
				<ContentTemplate>
					<asp:Panel runat="server" ID="pnlExpiryInfo" Visible="false">
						<table class="clstablelistout" id="Table5">
							<tr>
								<td>
									<table class="clstablelistin" id="Table6">
										<tr>
											<td colspan="3" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td width="100px">
															<span id="lblModalPopupHeader" class="clsFormHeader">Update Info</span>
														</td>
														<td align="right" colspan="4">
															<asp:UpdatePanel ID="upnlMailTool" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"
																		ToolTip="Click to Update Model Directive Info"></asp:Button>
																	<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH"
																		Text="Send Mail" Visible="false"
																		ToolTip="Click to send Mail to if Directive is revised"></asp:Button>
																	<asp:Button ID="btnCloseChangeExpiryInfo" TabIndex="0"
																		runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
																		ToolTip="Click to close Update screen" CausesValidation="False"></asp:Button>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>

											</td>
										</tr>
										<tr>
											<td colspan="3">
												<asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="Enter New Issue Date" ControlToValidate="txtNewIssueDate" Display="None"></asp:RequiredFieldValidator>
												<asp:RequiredFieldValidator ID="rfvDNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Enter New Directive No."
													ControlToValidate="txtNewDirectiveNo" Display="None"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="New Issue Date should be greater than or equal Old Issue Date."
													ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
												<asp:CustomValidator ID="cvMobLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Note should not be greater than 1000 characters."
													Display="None" ControlToValidate="txtNewNote" ClientValidationFunction="validateName"></asp:CustomValidator>
											</td>
										</tr>
										<tr>
											<td>
												<fieldset id="fdOld" class="clsFieldSetNewStyle">
													<legend><b>Old</b></legend>
													<table>
														<tr>
															<td>
																<span id="lblStartDate" class="clsLabel">Issue Date</span>
															</td>
															<td>
																<asp:TextBox ID="txtOldIssueDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																	ReadOnly="true" BackColor="Gainsboro" runat="server" CausesValidation="true"></asp:TextBox>
																<cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOldIssueDate"></cc2:CalendarExtender>
																<cc2:TextBoxWatermarkExtender TargetControlID="txtOldIssueDate" ID="FromDate_watermarkextender"
																	ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																	WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
															</td>
														</tr>
														<tr>
															<td>
																<span id="Span1" class="clsLabel">Directive No.</span>
															</td>
															<td>
																<asp:TextBox ID="txtOldDirectiveNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="true"
																	BackColor="Gainsboro"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<span id="Label3" class="clsLabel">Old Note</span>
															</td>
															<td>
																<asp:TextBox ID="txtOldNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ReadOnly="true"
																	TextMode="MultiLine" Height="30px" BackColor="Gainsboro"></asp:TextBox>
															</td>
														</tr>
													</table>
												</fieldset>
											</td>
											<td>
												<fieldset id="fdNew" class="clsFieldSetNewStyle">
													<legend><b>New</b></legend>
													<table>
														<tr>
															<td>
																<span id="lstExpiryDate" class="clsLabel">Issue Date</span>
															</td>
															<td>
																<asp:TextBox ID="txtNewIssueDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																	onchange="ValidateDateText(this,'ExpiryDate_watermarkextender');" runat="server"
																	CausesValidation="true"></asp:TextBox>
																<cc2:CalendarExtender ID="calExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNewIssueDate"></cc2:CalendarExtender>
																<cc2:TextBoxWatermarkExtender TargetControlID="txtNewIssueDate" ID="ExpiryDate_watermarkextender"
																	ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																	WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
															</td>
														</tr>
														<tr>
															<td>
																<span id="Span2" class="clsLabel">Directive No.</span>
															</td>
															<td>
																<asp:TextBox ID="txtNewDirectiveNo" runat="server" CssClass="clsTextBoxTagSearch" ClientIDMode="Static"
																	MaxLength="150"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<span id="Label4" class="clsLabel">Note</span>
															</td>
															<td>
																<asp:TextBox ID="txtNewNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ClientIDMode="Static"
																	TextMode="MultiLine" Height="30px"></asp:TextBox>
															</td>
														</tr>
													</table>
												</fieldset>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<fieldset id="Fieldset1" class="clsFieldSetNewStyle">
													<legend><b>Attachment Details</b></legend>
													<table>
														<tr>
															<td>
																<span id="lblAttachFile" class="clsLabel">Attach File</span>
															</td>
															<td>
																<asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table border="0" cellpadding="0" cellspacing="0">
																			<tr>
																				<td>
																					<input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
																						runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" />
																				</td>
																				<td style="padding-left: 3px;">
																					<asp:Button ID="btnDelAttach" runat="server" CausesValidation="false" CssClass="clsbtnH clsinfoH1"
																						Enabled="False" Text="Remove Attachment" ToolTip="Click to Remove Attachment"
																						Width="150px" />
																				</td>
																				<td style="padding-left: 2px;">
																					<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
																						ImageUrl="icons/CLIP01.ICO" Width="20px" />
																				</td>
																			</tr>
																		</table>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</fieldset>
											</td>
										</tr>
										<!--Dummy panel to open modelpopup-->
										<tr style="height: 0px;">
											<td style="height: 0px;">
												<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
													<ContentTemplate>
														<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
															CausesValidation="False" Style="display: none;"></asp:Button>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<!--End -->
									</table>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpChangeExpiryInfo" runat="server" TargetControlID="btnDummyChangeExpiryInfo"
			PopupControlID="pnlChangeExpiryInfo" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End Change Part Expity Info -->
		<input id="gridrowindex" type="hidden" value="" />
		<input id="gridrowaction" type="hidden" value="" />
		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var resetTodaysDate = 'true';
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

			//From Date -To Date validation
			function BetweenDatesValidation(source, args) {
				args.IsValid = false;
				var fromdate = $("#txtOldIssueDate").val();
				var todate = $("#txtNewIssueDate").val();
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


			$(document).ready(function () {

				ControlVisibilityForSearch();
			});

			function ControlVisibilityForSearch() {
				var dd = $get("cmbSearch");
				switch (dd.selectedIndex) {
					case 0:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'none');
						$("#txtSearchFor").css('display', 'none');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 1:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'none');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'block');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 2:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'none');
						$("#txtCode").css('display', 'block');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 3:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'block');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 4:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'block');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 5:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'block');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'none');
						break;
					case 6:
						$("#txtSearchFor").val('');
						$("#txtCode").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'none');
						$("#txtCode").css('display', 'none');
						$("select#cmbModel").val('0');
						$("#cmbModel").css('display', 'none');
						$("#cmbDirectiveType").css('display', 'block');
						break;
				}
			}
		</script>
		<%-- Row Highlight--%>
		<script type="text/javascript">
			//event handler for end request i.e last event in client page cycle.
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
			//event handler for begin request i.e before sending request to the server
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

			var element;
			var timerId;
			var timeoutforblink;
			var hideRowHighlight = false;
			var tempval;

			function endRequestHandler(sender, args) {
				tempval = parseInt($("#gridrowindex").val()); //row number ..0 is header row..
				if (tempval) {
					$("#<%=dgModelMonitorModList.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
					if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
						var elem;
						var tempaction = $("#gridrowaction").val(); //action to be performed

						if (tempaction == "close") {
							$("#dgModelMonitorModList tr:eq(" + tempval + ")").removeClass('activerow');
							$("#gridrowaction").val('');
							return;
						}
						//change Expiry Info button ok event
						//blink Expiry columns of the row for perticular interval
						else if (tempaction == "ExpiryInfo") {
							$("#<%=dgModelMonitorModList.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
							elem = $("#<%=dgModelMonitorModList.ClientID %> tr:eq(" + tempval + ") td:eq(7),#<%=dgModelMonitorModList.ClientID %> tr:eq(" + tempval + ") td:eq(8)");
							$("#gridrowaction").val('');
						}
						else {
							return;
						}
						//blink column function
						timeoutforblink = setInterval(function () {

							if (elem.hasClass('activerow')) {
								elem.removeClass('activerow');
							}
							else {
								elem.addClass('activerow');
							}

						}, 500);
						//stop blink column
						timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
					}


				}
			}

			function BeginRequestHandler(sender, args) {
				clearTimeout(timerId);
				element = args.get_postBackElement();

				//change Expiry Info popup ok button event occur
				if (element.id == "MSGBoxCtrl_btnYes") {
					hideRowHighlight = true;
					$("#gridrowaction").val('ExpiryInfo');
				}
				//any of change popup close button event occur 
				else if (element.id == "btnCloseChangeExpiryInfo") {
					hideRowHighlight = true;
					$("#gridrowaction").val('close');
				}
				//change parttype ||change location link event occur
				//reset rowindex value if other grid event occurs
				else if (element.id == "dgModelMonitorModList") {
					if ($("#gridrowaction").val() != "gridrow") {
						$("#gridrowindex").val('');
					}
				}
				else if (element.id == "btnOk") {

				}
				else if (element.id == "MSGBoxCtrl_btnNo") {

				}
				//any other events
				else {
					$("#gridrowindex").val('');
				}
			}

			//stop blinking
			function TimeOut(val, action) {
				var tempelem;

				if (action == "ExpiryInfo") {
					tempelem = $("#<%=dgModelMonitorModList.ClientID %> tr:eq(" + val + ") td:eq(7),#<%=dgModelMonitorModList.ClientID %> tr:eq(" + val + ") td:eq(8)");
					tempelem.removeClass('activerow');
				}
				else {
					return;
				}
				$("#gridrowindex").val('');
				hideRowHighlight = false;
				clearInterval(timeoutforblink);
			}
		</script>
		<script type="text/javascript">
			$(document).ready(function () {
				$("#dgModelMonitorModList tr td a").live("click", function () {
					var temp = $(this).parent().parent()[0].rowIndex;
					$("#gridrowindex").val(temp);
					$("#gridrowaction").val('gridrow');
				});
			});
		</script>
		<script type="text/javascript">
			function validateName(source, args) {
				var ControlName = source.controltovalidate;
				switch (ControlName) {
					case 'txtNewNote':
						var Value = $get(ControlName).value.length;
						if (Value > 1000) {
							args.IsValid = false;
							return
						}
						break;
				}
			}
		</script>
		<!-- Popup For Report By Mail -->
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
				$("#hdnimgBtnSendMail").click();
			}
		</script>
		<!---End-->
		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
			PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileUploadStateComplete() {
				$("#btnDummyFileUpload").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			$(document).ready(function () {
				$("#btnSelectFile").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = 'visible';
						$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
						if (!$.browser.msie) {
							$("#btnDummyFileUpload").click();
							$get("AjaxLoader").style.visibility = 'hidden';
						}

						return false;
					} catch (e) {
						alert(e);
					}
				});
			});
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFileUpload(fileattached) {
				var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
				//close File Upload popup window
				FileUpwindow.hide();
				//Free resources
				$("#IFileUpload").attr("src", "JavaScript:''");
				if (fileattached) {
					//call hidden button to set file upload content to object
					$("#hdnBtnFileUpload").click();
				}
			}
		</script>
		<!--End -->
	</form>
</body>
</html>

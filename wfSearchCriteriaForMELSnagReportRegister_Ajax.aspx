<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForMELSnagReportRegister_Ajax.aspx.vb"
	Inherits="Flypal.SearchCriteriaForMELSnagReportRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>MEL / Snag Register Report</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server"
											Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect Register Report", "MEL / Snag Register Report") %>'>
										</asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Fields" ValidationGroup="a" />
												<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
													ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
													ValidationGroup="a" />
												<asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
													CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a" />
												<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
													ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a"
													ErrorMessage="From Date should not be greater than To Date " />
												<asp:CustomValidator ID="cvAircraft" runat="server" ControlToValidate="cmbAircraft"
													CssClass="clsLabelAuto" Display="None" ErrorMessage="Please Select the Aircraft"
													ClientValidationFunction="ValidationForAircraftSelection"
													ValidationGroup="a" />
												<script type="text/javascript">

													/*Modifed By Harsh on 23rd Feb 2024*/
													function ValidationForAircraftSelection(source, args) {
														var pp = $get("cmbAircraft");
														var IsRepeatitiveChecked = $get("chkIsRepetitive").checked;
														var client = '<%# AppSettings("ClientCode") %>';
														args.IsValid = true;
														if ((pp.selectedIndex == 0) && (IsRepeatitiveChecked == false) && (client != "Heligo")) {
															args.IsValid = false;
															return;
														}
													}
												</script>
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="chkIsRepetitive" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td colspan="5">
															<span id="lblStep1" class="clsLabelHeader">Step I. Selection of Dates</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblFromDate" class="clsLabelAuto">From</span>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
															<cc2:calendarextender id="txtFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate" />
															<cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="FromDate_watermarkextender"
																clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
																watermarkcssclass="clsDateTextBox" />
														</td>
														<td>
															<span id="lblToDate" class="clsLabelAuto">To</span>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'ToDate_watermarkextender');" />
															<cc2:calendarextender id="txtToDate_CalendarExtender1" runat="server" cssclass="cal_Theme1"
																enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtToDate" />
															<cc2:textboxwatermarkextender targetcontrolid="txtToDate" id="ToDate_watermarkextender"
																clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
																watermarkcssclass="clsDateTextBox" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlRepeatitive" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<span id="lbDefectType0" class="clsLabelHeader">Step VII. Selection For Is Repetitive</span>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:CheckBox ID="chkIsRepetitive" runat="server"
																CssClass="clsCheckBox" Text="Is Repetitive"
																ClientIDMode="Static" AutoPostBack="True" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlAircraftInfo" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td colspan="3">
															<span id="lblStep3" class="clsLabelHeader">Step II. Selection of Aircraft</span>
														</td>
													</tr>
													<tr>
														<td style="width: 9px">
															<asp:UpdatePanel runat="server" ID="upnlAircraftStar" UpdateMode="Conditional">
																<ContentTemplate>
																	<span id="lblAircraftStar1" runat="server" class="clsLabelStar">*</span>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td>
															<span id="lblAircraft" class="clsLabelAuto">Aircraft </span>
														</td>
														<td>
															<asp:UpdatePanel runat="server" ID="upnlAircraftCombo" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:DropDownList ID="cmbAircraft" runat="server" 
																		CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																		ClientIDMode="Static" DataTextField="RegNo" AutoPostBack="True" />
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td style="width: 9px">&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<span id="Label5" class="clsLabelHeader">Step III. Selection of ATA Chapter</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="Label3" class="clsLabel">ATA Chapter </span>
														</td>
														<td>
															<asp:DropDownList ID="cmbATAChapter" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle"
																DataValueField="ID" Style="width: 200px;"
																DataTextField="ATAChapter" />
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Status</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblStatus" class="clsLabelAuto">Status</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbStatus" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Open</asp:ListItem>
																<asp:ListItem Value="2">Close</asp:ListItem>
															</asp:DropDownList>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<span id="Label2" class="clsLabelHeader">Step V. Enter the keyword to search defect</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblDefect" class="clsLabelAuto">Defect</span>
														</td>
														<td>
															<asp:TextBox ID="txtDefect" runat="server" CssClass="clsTextBoxTagSearch"
																ToolTip="Enter Defect" />
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<span id="Span1" class="clsLabelHeader">Step VI. Select Yes/No for Is In Reliability</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblIsInReliability" class="clsLabelAuto">Is In Reliability</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbIsInReliability" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Yes</asp:ListItem>
																<asp:ListItem Value="2">No</asp:ListItem>
															</asp:DropDownList>
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<span id="Span4" runat="server" class="clsLabelHeader">Selection of Criteria</span>
														</td>
													</tr>
													<tr>
														<td colspan="3">
															<asp:RadioButton ID="rbAllLog" runat="server" Checked="true" CssClass="clsRadioButton"
																AutoPostBack="true" GroupName="R" Text="All Logs" />
															<asp:RadioButton ID="rbNILLog" runat="server" CssClass="clsRadioButton" GroupName="R"
																AutoPostBack="true" Text="Only Nil Log" />
															<asp:RadioButton ID="rbWithoutNilLog" runat="server" CssClass="clsRadioButton" GroupName="R"
																AutoPostBack="true" Text="Without Nil Log" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlMELCriteria" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<span id="lbDefectType" class="clsLabelHeader">Step VIII. Selection of Defect Type</span>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<table>
																<tr>
																	<td>
																		<asp:RadioButton ID="rbAllDefectType" runat="server" 
																			Checked="True" CssClass="clsRadioButton"
																			GroupName="c" Text="All" />
																	</td>
																	<td>
																		<asp:RadioButton ID="rbIsPireps" runat="server"
																			CssClass="clsRadioButton" GroupName="c"
																			Text="Pireps" />
																	</td>
																	<td>
																		<asp:RadioButton ID="rbMaintenanceDefect" runat="server"
																			CssClass="clsRadioButton"
																			GroupName="c" Text="Maintenance Defect" Width="136px" />
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:Label ID="lblStep2" class="clsLabelHeader"
																runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Step IX. Selection of ADD / Defect Type", "Step IX. Selection of MEL / Snag Type") %>' />
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:RadioButton ID="rbAll" runat="server" CssClass="clsRadioButton" GroupName="a"
																Text="All" />
															<asp:RadioButton ID="rbMajor" runat="server" CssClass="clsRadioButton" GroupName="a"
																Text="Major" />
															<asp:RadioButton ID="rbMinor" runat="server" CssClass="clsRadioButton" GroupName="a"
																Text="Minor" />
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:Label ID="Label1" class="clsLabelHeader" 
																runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Step X. Selection of ADD / Defect Part", "Step X. Selection of MEL / Snag Part") %>' />
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:RadioButton ID="rbAllSnagMEL" runat="server" CssClass="clsRadioButton" GroupName="b"
																Text="All" />
															<asp:RadioButton ID="rbSnag" runat="server" CssClass="clsRadioButton" GroupName="b"
																Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag") %>' />
															<asp:RadioButton ID="rbMEL" runat="server" CssClass="clsRadioButton" GroupName="b"
																Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL") %>' />
														</td>
													</tr>
													<tr>
														<td></td>
														<td></td>
														<td></td>
													</tr>
													<tr>
														<td></td>
														<td></td>
														<td>
															<span id="lblIncidentType" runat="server" 
																class="clsLabelHeader">
																Step XI. Selection of Incident Type</span>
														</td>
													</tr>
													<tr>
														<td></td>
														<td></td>
														<td>
															<asp:DropDownList ID="cmbIncidentType" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																DataTextField="Name" />
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<span id="lblRefDoc" runat="server" class="clsLabelHeader">
																Step XII. Selection of Reference Document </span>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<asp:CheckBox ID="chkLogNo" runat="server" Checked="True" CssClass="clsCheckBox"
																Text="Log No." />
															<asp:CheckBox ID="chkLogPageNo" runat="server" CssClass="clsCheckBox" Text="Log Page No." />
															<asp:CheckBox ID="chkFlightNo" runat="server" CssClass="clsCheckBox" Text="Flight No" />
														</td>
													</tr>
													<%--Modified by Harsh on 29th Jan 2024 Client Enhancement--%>
													<tr>
														<td>&nbsp;
														</td>
														<td>&nbsp;
														</td>
														<td>
															<span id="lblStep5" runat="server" class="clsLabelHeader"
																visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", True, False) %>'>Step XII. Selection of Format</span>
															<span id="Span2" runat="server" class="clsLabelHeader"
																visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", False, True) %>'>Step XIII. Selection of Format</span>
														</td>
													</tr>
													<asp:PlaceHolder ID="phBAFormat" runat="server" Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", True, False) %>'>
														<tr>
															<td></td>
															<td></td>
															<td>
																<asp:UpdatePanel runat="server" ID="upncmbFormat" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:DropDownList ID="cmbBA" runat="server" 
																			CssClass="clsTextBoxTagSearchComboNewstyle">
																			<asp:ListItem Value="1">Format 1</asp:ListItem>
																			<asp:ListItem Value="2">Format 2</asp:ListItem>
																		</asp:DropDownList>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phFormat" runat="server" 
														Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", False, True) %>'>
														<tr>
															<td>&nbsp;
															</td>
															<td>&nbsp;
															</td>
															<td>
																<asp:UpdatePanel runat="server" ID="upnlFormat" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:DropDownList ID="cmbFormat" runat="server" 
																			CssClass="clsTextBoxTagSearchComboNewstyle">
																			<asp:ListItem Value="1">Format 1</asp:ListItem>
																			<asp:ListItem Value="2">Format 2</asp:ListItem>
																			<asp:ListItem Value="3">Format 3</asp:ListItem>
																			<asp:ListItem Value="4">Format 4</asp:ListItem>
																		</asp:DropDownList>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</asp:PlaceHolder>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<span id="lblStep6" runat="server" class="clsLabelHeader">Step XIV. Display Report</span>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlselection1" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblDateRangeFrom" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblAircraft1" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblStatus1" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlselection2" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblDateRangeTo" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblATAChapter1" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblDefectType1" runat="server" 
																CssClass="clsLabelAuto" Visible="False" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right" colspan="2">
										<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table cellspacing="0">
													<tr>
														<td>
															<asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
																CssClass="clsbtnH clsinfoH1" TabIndex="0" Text="Current Criteria"
																ToolTip="Click to Display Current Searching criterias" />
														</td>
														<td>
															<asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH1" Text="Export to Excel"
																ToolTip="Click to Export report" ValidationGroup="a"
																Visible="<%$AppSettings:ShowExportToExcelButton%>" CausesValidation="true" />
														</td>
														<td>
															<asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" TabIndex="0"
																Text="Display" ToolTip="Click to Display Report" ValidationGroup="a" />
														</td>
														<td>
															<asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
																TabIndex="0" Text="Close"
																ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to close the search criteria for ADD / Defect Register screen", "Click to close the search criteria for MEL / Snag Register screen") %>' />
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

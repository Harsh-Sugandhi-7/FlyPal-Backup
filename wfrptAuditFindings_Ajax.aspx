<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAuditFindings_Ajax.aspx.vb"
	Inherits="Flypal.wfrptAuditFindings_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Audit Status Register</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" type="text/javascript" id="clientEventHandlersJS">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfExportToExcel.aspx";
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
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:UpdatePanel ID="upnlDetail" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<table id="tblLedgerList" class="clstablelistin">
									<tr>

										<td colspan="2" class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<span id="lblAuditScheduleList" class="clsFormHeader">Audit Findings Report</span>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
														HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
													<asp:CustomValidator ID="cvAuditNo" runat="server" CssClass="clsLabelAuto" Display="None"
														ControlToValidate="cmbAuditInfoList" OnServerValidate="CustomValidate"></asp:CustomValidator>
													<asp:RequiredFieldValidator ID="rfvtxtFromDate" runat="server" CssClass="clsLabelAuto"
														Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date required"></asp:RequiredFieldValidator>
													<asp:RequiredFieldValidator ID="rfvtxtToDate" runat="server" CssClass="clsLabelAuto"
														Display="None" ControlToValidate="txtToDate" ErrorMessage="To Date required"></asp:RequiredFieldValidator>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="66px">From Date</asp:Label>
										</td>
										<td>
											<table id="Table1" cellspacing="0">
												<tr>
													<td></td>
													<td>
														<asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
															AutoPostBack="true" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
													</td>
													<td>
														<asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtToDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
															AutoPostBack="true" runat="server" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Audit No.</asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left">
											<asp:Label ID="lblAuditNo" runat="server" CssClass="clsLabelAuto">Audit No.</asp:Label>
										</td>
										<td align="left">
											<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAuditInfoList" runat="server" DataTextField="AuditNo"
												DataValueField="ID">
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Department</asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left">
											<asp:Label ID="lblDepartment" runat="server" CssClass="clsLabelAuto">Department</asp:Label>
										</td>
										<td align="left">
											<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDepartmentList" runat="server" DataTextField="Name"
												DataValueField="ID">
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Finding Status</asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left">
											<asp:Label ID="lblFindingStatus" runat="server" CssClass="clsLabel">Finding Status</asp:Label>
										</td>
										<td align="left">
											<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFindingStatus" runat="server" DataTextField="Name"
												DataValueField="ID">
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblFindingLevel" runat="server" CssClass="clsLabelHeader">Step V. Selection of Level</asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left">
											<asp:Label ID="Label1" runat="server" CssClass="clsLabel">Finding Level</asp:Label>
										</td>
										<td align="left">
											<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFindingLevel" runat="server" DataTextField="Name"
												DataValueField="ID">
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Format</asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left">
											<asp:Label ID="lblFormat" runat="server" CssClass="clsLabel">Format</asp:Label>
										</td>
										<td align="left">
											<asp:UpdatePanel ID="upnlReport" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td>
																<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server">
																	<asp:ListItem Value="0">Format 1</asp:ListItem>
																	<asp:ListItem Value="1">Format 2</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td align="right">
																<asp:CheckBox ID="chkSummary" runat="server" CssClass="clsCheckBox" Text="Summary"
																	AutoPostBack="True" />
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VII. Display Report</asp:Label>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="left">
											<asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td align="left">
																<asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="lblAuditNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="lblDepartment1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="lblFindingStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="right">
											<asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table cellspacing="0">
														<tr>
															<td>
																<asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
																	Text="Current Criteria" ToolTip="Click to display Current Searching criterias."
																	CausesValidation="False"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnExport" runat="server" CssClass="clsbtnH" Text="Export to Excel" Visible="<%$AppSettings:ShowExportToExcelButton%>"
																	Width="140px" ToolTip="Click to Export report" Enabled="false"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Display"
																	ToolTip="Click to Display Report"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
																	ToolTip="Click to close Audit Findings Report screen" CausesValidation="False"></asp:Button>
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
		</div>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
			runat="server">
			<ProgressTemplate>
				<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
				</div>
				<div style="position: fixed; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
					<div class="ext-el-mask-msg x-mask-loading">
						<div class="clsLoad_ajax">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
								Height="48px" Width="48px" />
						</div>
					</div>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
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

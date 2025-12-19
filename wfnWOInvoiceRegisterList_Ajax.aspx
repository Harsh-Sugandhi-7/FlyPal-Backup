<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOInvoiceRegisterList_Ajax.aspx.vb"
	Inherits="Flypal.wfnWOInvoiceRegisterList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Invoice Register</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" id="clientEventHandlersJS">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
		function openFile() {
			str = "wfExportToExcel.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>

	<style type="text/css">
		.padding-top {
			padding-top: 0.5rem;
		}

		.padding-bottom{
			padding-bottom: 0.5rem;
		}

	</style>

</head>
<body>
	<form id="WOInvoiceRegisterForm" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Invoice Register</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
										HeaderText="Fill Up The Following Fields." ValidationGroup="1"></asp:ValidationSummary>
									<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
										ClientValidationFunction="BetweenDatesValidation" ValidationGroup="1"
										ErrorMessage="From Date should not be greater than To Date.">
									</asp:CustomValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1">
												<tr>
													<td colspan="4">
														<span id="lblStep1" class="clsLabelHeader">Step I. Selection of date range</span>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblFromDate" class="clsLabelAuto">From Date</span>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
															onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
													</td>
													<td align="right">
														<span id="lblToDate" class="clsLabelAuto">To Date </span>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
															onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
													</td>
												</tr>
												<tr>
													<td colspan="4" class="padding-top">
														<span id="Span3" class="clsLabelHeader">Step II. Selection of Work Order No.</span>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblCWPTextNo" class="clsLabelAuto">Work Order No.</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbCWP" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="WOText"
															DataValueField="WOText">
															<asp:ListItem Value="0">(ALL)</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<span id="lblCWPNo" class="clsLabelAuto">No.</span>
													</td>
													<td>
														<asp:TextBox ID="txtCWPNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="7"
															onchange="setattr(this);" ToolTip="Enter Number">0</asp:TextBox>
													</td>
												</tr>
												<tr>
													<td colspan="4" class="padding-top">
														<span id="Span1" class="clsLabelHeader">Step III. Selection of Customer ID</span>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblCustId" class="clsLabelAuto">Customer ID</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbCustomerList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
															DataTextField="Name" DataValueField="ID">
														</asp:DropDownList>
													</td>
													<tr>
														<td colspan="4" class="padding-top">
															<span id="Span4" class="clsLabelHeader">Step VI. Selection of Invoice Status</span>
														</td>
													</tr>
													<tr>
														<td>
															<span id="Span6" class="clsLabelAuto">Status</span>
														</td>
														<td colspan="3">
															<asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
																<asp:ListItem Selected="True" Text="(ALL)" Value="0"></asp:ListItem>
																<asp:ListItem Text="Open" Value="1"></asp:ListItem>
																<asp:ListItem Text="Authorized" Value="2"></asp:ListItem>
																<asp:ListItem Text="Cancelled" Value="4"></asp:ListItem>
															</asp:DropDownList>
														</td>
													</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td colspan="3">
														<asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
													</td>
												</tr>
												<tr>
													<td colspan="3" class="padding-bottom">
														<asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selections are as follows :</asp:Label>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
													</td>
													<td>
														<asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label ID="lblCust" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
													</td>
													<td>
														<asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel ID="upnlActionButton" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table7" border="0" cellspacing="0">
												<tr>
													<td>
														<asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
															Text="Current Criteria" ToolTip="Click to display Current Searching criterias"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" ValidationGroup="1"
															Text="Display" ToolTip="Click to Display Report." />
													</td>
													<td>
														<asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
															Text="Close" ToolTip="Click to close." />
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
					 function setattr(elem) {
						 var No = $(elem).val();
						 if ($(elem).val() == "") {
							 $(elem).val('0');
						 }
					 }
		</script>
	</form>
</body>
</html>

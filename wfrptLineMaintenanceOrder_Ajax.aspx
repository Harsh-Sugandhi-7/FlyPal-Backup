<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptLineMaintenanceOrder_Ajax.aspx.vb"
	Inherits="Flypal.wfrptLineMaintenanceOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>Service Order Register</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<contenttemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</contenttemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<span id="lbltitle" class="clsFormHeader">Service Order Register</span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlValidationsummary">
											<contenttemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Fields" ValidationGroup="a">
												</asp:ValidationSummary>
												<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
													ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" 
													ErrorMessage="From Date should not be greater than To Date ">
												</asp:CustomValidator>
												<script type="text/javascript">
													function showTextField(elem) {
														var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
														var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
														var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
														var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
															if (elem.selectedIndex == 0) {
																txtFromDateobj.style.display = 'none';
																txtToDateobj.style.display = 'none';
																lblFromDateobj.style.display = 'none';
																lblToDateobj.style.display = 'none';
															}
													}
												</script>
											</contenttemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
											<contenttemplate>
												<table width="100%">
													<td width="96px">
														<span id="lblDateRange" class="clsLabel">Date Range</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
															onchange="showTextField(this);">
															<asp:ListItem Value="0">(All)</asp:ListItem>
															<asp:ListItem Value="1">Last Week</asp:ListItem>
															<asp:ListItem Value="2">Last Month</asp:ListItem>
															<asp:ListItem Value="3">Last Quarter</asp:ListItem>
															<asp:ListItem Value="4">Last Year</asp:ListItem>
															<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
															<asp:ListItem Value="6">Between Dates</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
															onchange="ValidateDateText(this,'FromDate_watermarkextender');">
														</asp:TextBox>
														<cc2:calendarextender id="txtFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
															enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate">
														</cc2:calendarextender>
														<cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="FromDate_watermarkextender"
															clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
															watermarkcssclass="clsDateTextBox">
														</cc2:textboxwatermarkextender>
													</td>
													<td>
														<asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
															onchange="ValidateDateText(this,'ToDate_watermarkextender');">
														</asp:TextBox>
														<cc2:calendarextender id="txtToDate_CalendarExtender1" runat="server" cssclass="cal_Theme1"
															enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtToDate">
														</cc2:calendarextender>
														<cc2:textboxwatermarkextender targetcontrolid="txtToDate" id="ToDate_watermarkextender"
															clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
															watermarkcssclass="clsDateTextBox">
														</cc2:textboxwatermarkextender>
													</td>
												</table>
											</contenttemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlSupplierSelection" UpdateMode="Conditional">
											<contenttemplate>
												<table width="100%">
													<tr>
														<td colspan="2">
															<span id="lblStepII" class="clsLabelHeader">
																Step II. Selection of Supplier.
															</span>
														</td>
													</tr>
													<tr>
														<td width="96px">
															<span id="lblSupplier" class="clsLabel">Supplier</span>
														</td>
														<td>
															<asp:TextBox ID="txtSupplierList" runat="server" CssClass="clsTextBoxSearch_Ajax" />
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<span id="lblStep6" class="clsLabelHeader">
																Step III Selection of Order No.
															</span>
														</td>
													</tr>
													<tr>
														<td width="96px">
															<span id="lblOrderTextNo" class="clsLabel">Order No.</span>
														</td>
														<td>
															<asp:TextBox ID="txtOrderTextList" runat="server" 
																CssClass="clsTextBoxTagSearch" />
															<asp:TextBox ID="txtOrderNo" runat="server" 
																CssClass="clsTextBoxTagSearchSmall" MaxLength="8" />
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<span id="lblStep4" class="clsLabelHeader">
																Step IV. Selection to show only MSP Records.
															</span>
														</td>
													</tr>
													<tr>
														<td width="100%">
															<span id="lblOrderTextNo" class="clsLabel">
																Show only MSP Records
															</span>
														</td>
														<td>
															<asp:CheckBox runat="server" ID="chkShowOnlyMSPRecords" />
														</td>
													</tr>
												</table>
											</contenttemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblStep5" class="clsLabelHeader">
											Step V. Display Report.
										</span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
											<contenttemplate>
												<table>
													<tr>
														<td>
															<asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">
																Your selections are as follows :
															</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
														</td>
													</tr>
													<tr>
														<td align="left">
															<asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblShowOnlyMSPRecords" runat="server" CssClass="clsLabelAuto" Visible="false" />
														</td>
													</tr>
												</table>
											</contenttemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
											<contenttemplate>
												<table width="100%">
													<tr>
														<td align="right">
															<asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH1"
																Text="Current Criteria" ToolTip="Display Current Searching criterias" />
															<asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH1" 
																Text="Export to Excel" ToolTip="Export report to Excel" Visible="False" />
															<asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" 
																Text="Display" ToolTip="Click to Display Report" ValidationGroup="a" />
															<asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
																Text="Close" ToolTip="Close Service Order Register screen" />
														</td>
													</tr>
												</table>
											</contenttemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</div>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
			runat="server">
			<progresstemplate>
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
			</progresstemplate>
		</asp:UpdateProgress>
	</form>
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=txtOrderTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=19', {
				width: 185,
				autoFill: false,
				matchContains: true,
				mustMatch: true,
				delay: 0
			});
		});
	</script>
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=txtSupplierList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
				width: 275,
				autoFill: false,
				matchContains: true,
				mustMatch: true,
				delay: 0
			});
		});
	</script>
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
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
		function endRequestHandler() {
			var dd = document.getElementById("cmbDateRange");
			showTextField(dd);
		}
	</script>
</body>
</html>

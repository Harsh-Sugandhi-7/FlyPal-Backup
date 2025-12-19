<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmpCARegister_Ajax.aspx.vb" Inherits="Flypal.wfEmpCARegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtlkt" %>
<%@ Register TagName="MSGBox" Src="MSGBox.ascx" TagPrefix="msgBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>CA Register Report</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" src="modules/jquery/jquery-2.2.4.min.js"></script>
	<script id="clientEventHandlersJS" type="text/javascript">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
</head>
<body>
	<form id="frmCARegistrationReport" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<msgBox:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="3" class="clsFormHeader1">
										<span id="lbltitle" class="clsFormHeader">CA Register Report</span>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<span id="lblStepI" class="clsLabelHeader">Selection of As On Date</span>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblAsonDate" class="clsLabelAuto">As On Date</span>
									</td>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
													onchange="ValidateDateText(this,'wmeAsOnDate');"></asp:TextBox>
												<ajaxtlkt:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
													Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></ajaxtlkt:CalendarExtender>
												<ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="wmeAsOnDate"
													ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
												</ajaxtlkt:TextBoxWatermarkExtender>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<span id="lblStepII" class="clsLabelHeader">Selection of Employee Name</span>
									</td>
								</tr>

								<tr>
									<td>
										<span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
									</td>
									<td colspan="2">
										<asp:DropDownList ID="ddlEmployees" runat="server"
											CssClass="clsTextBoxTagSearchComboNewstyle" Width="225px"
											DataTextField="Name" DataValueField="ID">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<span id="lblStepIII" class="clsLabelHeader">Selection of CA Status
										</span>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblCAStatus" class="clsLabelAuto">CA Status</span>
									</td>
									<td>
										<asp:DropDownList ID="ddlCAStatus" runat="server"
											CssClass="clsTextBoxTagSearchComboNewstyle" Width="225px"
											DataTextField="Name" DataValueField="ID">
										</asp:DropDownList>
									</td>
								</tr>

								<tr>
									<td align="right" colspan="3">
										<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table cellspacing="0">
													<tr>
														<td>
															<asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
																Text="Display" ToolTip="Click to Display Report"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
																ToolTip="Click to close" CausesValidation="False"></asp:Button>
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
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
	</form>

	<%--Added for Date Validation --%>
	<script type="text/javascript">
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

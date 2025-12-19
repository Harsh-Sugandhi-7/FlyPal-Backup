<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMSPRegister_Ajax.aspx.vb" Inherits="Flypal.wfMSPRegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>MSP Register</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script id="clientEventHandlersJS" type="text/javascript">

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
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="4" class="clsFormHeader1">
										<span id="lbltitle" class="clsFormHeader">Maintenance Support Plan Utilization</span>
									</td>
								</tr>
								<tr>
									<td colspan="4">
										<span id="lblStepI" class="clsLabelHeader">Selection of Date Range</span>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblFromDate" class="clsLabelAuto">From Date</span>
									</td>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlFromDate" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
												<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
													Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td>
										<span id="lblToDate" class="clsLabelAuto">To Date</span>
									</td>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlToDate" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"></asp:TextBox>
												<cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
													Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="4">
										<span id="lblStepIII" class="clsLabelHeader">Selection of Applicable To</span>
									</td>
								</tr>

								<tr>
									<td>
										<span id="lblStore" class="clsLabelAuto">Applicable To</span>
									</td>
									<td colspan="3">
										<asp:DropDownList ID="cmbAssemblyList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
											DataValueField="ID" DataTextField="ModelSerialNoPostion" Width="225px">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td colspan="4">
										<span id="lblStep3" class="clsLabelHeader">Selection of Maintenance Support Plan No.</span>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblCategory" class="clsLabelAuto">MSP No.</span>
									</td>
									<td>
										<asp:DropDownList ID="cmbMSPText" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
											AutoPostBack="True" DataTextField="Text" DataValueField="Text">
										</asp:DropDownList>
									</td>
									<td colspan="2">
										<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="40px"
											MaxLength="8"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td colspan="4">
										<span id="lblStep4" class="clsLabelHeader">Selection of Maintenance Support Plan selected In</span>
									</td>
								</tr>
								<tr>
									<td>
										<span id="lblMSPIn" class="clsLabelAuto">MSP In</span>
									</td>
									<td colspan="3">
										<asp:DropDownList ID="cmbMSPIn" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
											<asp:ListItem Value="0">Order</asp:ListItem>
											<asp:ListItem Value="1">Work Order</asp:ListItem>
											<asp:ListItem Value="2">Line Maintenance Order</asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td align="right" colspan="4">
										<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table cellspacing="0">
													<tr>
														<td>
															<asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
																Text="Display" ToolTip="Click to display report"></asp:Button>
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

	</form>
</body>
</html>

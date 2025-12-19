<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTLPDetailEdit_Ajax.aspx.vb"
	Inherits="Flypal.wfTLPDetailEdit_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>TLP Details</title>
	<meta name="vs_showGrid" content="True">
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">	
		#arrowICN {
			cursor: pointer;
		}

		#dropdown-content {
			z-index: 7;
			position: relative;
		}

		.actionICNS {
			height: 15px;
			width: 15px;
		}
	</style>
	<script id="clientEventHandlersJS" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<link rel="stylesheet" type="text/css" href="Calander\css\start\jquery-ui-1.8.14.custom.css" />
	<link rel="stylesheet" type="text/css" href="Calander\css\demos.css" />
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<script language="javascript" type="text/javascript">

			var g_CurrentTextBox;
			var g_isTabPressed;

			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
			function endRequestHandler() {

				try {

					//if (g_isTabPressed == 1) {
					$get(g_CurrentTextBox).focus();
					$get(g_CurrentTextBox).select();

					g_isTabPressed = 0;
					//}


				}
				catch (Error) { }

			}


			function onTextFocus() {
				g_CurrentTextBox = event.srcElement.id;

			}

			function onkeyPressed(keycode, obj) {

				if (keycode == 9) {

					g_isTabPressed = 1;
				}

			}

		</script>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
							<table class="clsTablelistin">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">TLP Details</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td align="right">
													<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Button ID="btnAdd" runat="server"
																CssClass="clsbtnH clsinfoH" ToolTip="Click to Add new record"
																Text="Add"></asp:Button>
															<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" Visible="False"
																CausesValidation="False"></asp:Button>
															<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Back to Previous Page"
																Text="Back" CausesValidation="false"></asp:Button>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>

									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvCustom" runat="server" OnServerValidate="customvalidate1"
													Display="None"></asp:CustomValidator>
												<asp:CustomValidator ID="cvDepartureDateTime" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Departure date should be in date time format." ControlToValidate="calDeparture"></asp:CustomValidator>
												<asp:CustomValidator ID="cvArrivalDateTime" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Arrival date should be in date time format." ControlToValidate="calArrival"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="None" ErrorMessage="Log Date Required."
													ControlToValidate="calDateTime" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvAirBornTime" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Not be Nigative." ControlToValidate="txtAirBorneTime"></asp:CustomValidator>
												<asp:CustomValidator ID="cvPlace1" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Enter correct Source name." ControlToValidate="Place1"></asp:CustomValidator>
												<asp:CustomValidator ID="cvPlace2" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Enter correct Destination name." ControlToValidate="Place2"></asp:CustomValidator>
												<asp:CustomValidator ID="CvTime" runat="server" Display="None" ErrorMessage="Invalid Time Format."
													ControlToValidate=""></asp:CustomValidator>
												<asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtEmployee"
													ValidateEmptyText="true" Display="None" ErrorMessage="Enter Done By Employee"
													OnServerValidate="customvalidate"></asp:CustomValidator>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Log Details</b> </legend>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblDateTime" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Date (UTC)", "Date") %>'>Date</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="calDateTime" runat="server" CssClass="clsTextBoxTagSearchDate" BackColor="Gainsboro"
																							ReadOnly="True" Width="100px"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblFlightNo" runat="server" CssClass="clsLabelAuto">Flight No.</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Flight No."
																							Text="<%# mLogDetail.FlightNo %>" MaxLength="10"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">TLP No.</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxTagSearchSmall" BackColor="Gainsboro"
																							ReadOnly="True" ToolTip="Log Page No." Text="<%# mLog.LogPageNoFormatted %>"
																							MaxLength="9"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>
															<table width="100%">
																<tr>
																	<td valign="top">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Departure Info</b> </legend>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblPalceStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
																					</td>
																					<td>&nbsp;
																					</td>
																					<td>
																						<asp:TextBox ID="Place1" runat="server" CssClass="clsTextBoxTagSearch" Width="150px"
																							Text="<%# mLogDetail.SourceName %>"></asp:TextBox>
																						&nbsp;
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						<asp:Label ID="lblUTCDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblDepDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
																						<asp:Label ID="lblUTCDateTime" runat="server" CssClass="clsLabelAuto">UTC Date/Time</asp:Label>
																					</td>
																					<td>&nbsp;
																					</td>
																					<td>
																						<asp:TextBox runat="server" ID="calDeparture" CssClass="clsTextBoxTagSearchDate" Width="90px"
																							BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="true" onchange="ValidateDateText(this,'DateTime_watermarkextender','false');"></asp:TextBox>
																						<cc2:CalendarExtender ID="calDeparture_CalendarExtender" runat="server" CssClass="cal_Theme1"
																							Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDeparture"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calDeparture" ID="DateTime_watermarkextender"
																							Enabled="false" ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox runat="server" ID="CalUTCDateTime" CssClass="clsTextBoxTagSearchDate" Width="90px"
																							BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" CausesValidation="True"
																							onchange="ValidateDateText(this,'CalUTCDateTime_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="CalUTCDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
																							Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalUTCDateTime"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="CalUTCDateTime" ID="CalUTCDateTime_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox ID="txtDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ReadOnly="True" ToolTip="Enter Departure Time." Width="65px"></asp:TextBox>
																						<asp:TextBox ID="txtUTCDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Departure Time." Width="65px"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblTakeOffStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						<asp:Label ID="lblUTCTakeOffStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblTakeOffLocalDateTime" runat="server" CssClass="clsLabelAuto">Take Off Date/Time</asp:Label>
																						<asp:Label ID="lblUTCTakeOffDateTime" runat="server" CssClass="clsLabelAuto">UTC Take Off Date/Time</asp:Label>
																					</td>
																					<td>
																						<asp:CheckBox ID="chkTakeOff" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																							ToolTip="Check to enable Take Off Date." />
																						<asp:CheckBox ID="chkUTCTakeOff" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																							ToolTip="Check to enable Take Off Date." />
																					</td>
																					<td>
																						<asp:TextBox runat="server" ID="calTakeOffLocalDateTime" CssClass="clsTextBoxTagSearchDate"
																							Width="90px" AutoPostBack="True" CausesValidation="True" BackColor="#E0E0E0"
																							ReadOnly="true" onchange="ValidateDateText(this,'calTakeOffLocalDateTime_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="calTakeOffLocalDateTime_CalendarExtender" runat="server"
																							CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calTakeOffLocalDateTime"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calTakeOffLocalDateTime" ID="calTakeOffLocalDateTime_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox runat="server" ID="calUTCTakeOffDateTime" CssClass="clsTextBoxTagSearchDate"
																							BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
																							onchange="ValidateDateText(this,'calUTCTakeOffDateTime_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="calUTCTakeOffDateTime_CalendarExtender" runat="server"
																							CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calUTCTakeOffDateTime"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calUTCTakeOffDateTime" ID="calUTCTakeOffDateTime_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox ID="txtTakeOffLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Take Off Time." Width="65px"></asp:TextBox>
																						<asp:TextBox ID="txtUTCTakeOffTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Take Off Time." Width="65px"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																	<td valign="top">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Arrival Info</b></legend>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblPlaceStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblArrPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
																					</td>
																					<td></td>
																					<td>
																						<asp:TextBox ID="Place2" runat="server" CssClass="clsTextBoxTagSearch" Width="150px"
																							Text="<%# mLogDetail.DestinationName %>"></asp:TextBox>
																						<asp:ImageButton ID="btnAddPlaces" runat="server" CausesValidation="False" Height="22px" Visible="false"
																							ImageUrl="~/images/plus1.png" ToolTip="Click to Add new Places" Width="24px" CssClass="clsbtnH clsinfoH" />
																					</td>
																				</tr>
																				<tr>
																					<td class="style3">
																						<asp:Label ID="lblUTCDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						<asp:Label ID="lblDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblArrDate" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
																						<asp:Label ID="lblUTCArrivalDateTime" runat="server" CssClass="clsLabelAuto">UTC DateTime</asp:Label>
																					</td>
																					<td>
																						<asp:CheckBox ID="chkArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date"
																							CssClass="clsCheckBox" />
																						<asp:CheckBox ID="chkUTCArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date"
																							CssClass="clsCheckBox" />
																					</td>
																					<td>
																						<asp:TextBox runat="server" ID="calArrival" CssClass="clsTextBoxTagSearchDate" Width="90px"
																							BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" onchange="ValidateDateText(this,'calArrival_watermarkextender');"
																							CausesValidation="True" onfocus="onTextFocus();"></asp:TextBox>
																						<cc2:CalendarExtender ID="calArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
																							Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calArrival"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calArrival" ID="calArrival_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox runat="server" ID="CalUTCArrival" CssClass="clsTextBoxTagSearchDate" Width="90px"
																							BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" CausesValidation="True"
																							onchange="ValidateDateText(this,'CalUTCArrival_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="CalUTCArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
																							Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalUTCArrival"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="CalUTCArrival" ID="CalUTCArrival_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox ID="txtArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Arrival Time." Width="65px"></asp:TextBox>
																						<asp:TextBox ID="txtUTCArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Arrival Time." Width="65px"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td class="style3">
																						<asp:Label ID="lblTouchDownStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						<asp:Label ID="lblUTCTouchDownStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblTouchDownLocalDateTime" runat="server" CssClass="clsLabelAuto">Touch Down Date/Time</asp:Label>
																						<asp:Label ID="lblUTCTouchDownDateTime" runat="server" CssClass="clsLabelAuto">UTC Touch Down Date/Time</asp:Label>
																					</td>
																					<td>
																						<asp:CheckBox ID="chkTouchDown" runat="server" AutoPostBack="True" ToolTip="Check to enable Touch Down Date."
																							CssClass="clsCheckBox" />
																						<asp:CheckBox ID="chkUTCTouchDown" runat="server" AutoPostBack="True" ToolTip="Check to enable Touch Down Date."
																							CssClass="clsCheckBox" />
																					</td>
																					<td>
																						<asp:TextBox runat="server" ID="calTouchDownLocalDateTime" CssClass="clsTextBoxTagSearchDate"
																							BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
																							onchange="ValidateDateText(this,'calTouchDownLocalDateTime_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="calTouchDownLocalDateTime_CalendarExtender" runat="server"
																							CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calTouchDownLocalDateTime"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calTouchDownLocalDateTime" ID="calTouchDownLocalDateTime_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox runat="server" ID="calUTCTouchDownDateTime" CssClass="clsTextBoxTagSearchDate"
																							BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
																							onchange="ValidateDateText(this,'calUTCTouchDownDateTime_watermarkextender');"></asp:TextBox>
																						<cc2:CalendarExtender ID="calUTCTouchDownDateTime_CalendarExtender" runat="server"
																							CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calUTCTouchDownDateTime"></cc2:CalendarExtender>
																						<cc2:TextBoxWatermarkExtender TargetControlID="calUTCTouchDownDateTime" ID="calUTCTouchDownDateTime_watermarkextender"
																							ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																							WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																						<asp:TextBox ID="txtTouchDownLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Touch Down Time." Width="65px"></asp:TextBox>
																						<asp:TextBox ID="txtUTCTouchDownTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" ToolTip="Enter Touch Down Time." Width="65px"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																	<td valign="top">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Totals</b></legend>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblairfly" runat="server" CssClass="clsLabelAuto">Block Time</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtBlockTime" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLogDetail.BlockTime %>"
																							Visible="False" Width="65px"></asp:TextBox>
																						<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Hrs</asp:Label>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblAirBorneTime" runat="server" CssClass="clsLabelAuto">Air Time </asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtAirBorneTime" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							Text="<%# mLogDetail.TimeInAir %>" Visible="False" Width="65px"></asp:TextBox>
																						<asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Hrs</asp:Label>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblLandings" runat="server" CssClass="clsLabelAuto">Landings </asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtLandings" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLogDetail.Landings %>"
																							Visible="False" Width="65px"></asp:TextBox>
																						<asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">No(s)</asp:Label>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																</tr>
																<tr>
																	<td valign="top" colspan="1">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Fuel Info ( KG/LBS )</b></legend>
																			<table width="100%">
																				<tr>
																					<td>
																						<asp:Label ID="lblFuelOnDeparture" runat="server" CssClass="clsLabelAuto">Fuel On Departure</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFuelOnDeparture" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="True" Text="<%# mLogDetail.FuelOnDeparture %>"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblFuelUplifted" runat="server" CssClass="clsLabelAuto">Fuel Uplifted</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFuelUplifted" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="<%# Not mLogDetail.IsNew %>" Text="<%# mLogDetail.FuelUplifted %>"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblTotalFuelOnDeparture" runat="server" CssClass="clsLabelAuto">Total Fuel</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtTotalFuelOnDeparture" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="True" Text="<%# mLogDetail.TotalFuelOnDeparture %>"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblFuelOnArrival" runat="server" CssClass="clsLabelAuto">Fuel On Arrival</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFuelOnArrival" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="<%# Not mLogDetail.IsNew %>" Text="<%# mLogDetail.FuelOnArrival %>"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblFuelConsumption" runat="server" CssClass="clsLabelAuto">Fuel Consumption</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFuelConsumption" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="True" Text="<%# mLogDetail.FuelConsumption %>"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																	<td valign="top" colspan="1">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend style="font-weight: bold"><b>Weight Info</b></legend>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblPax" runat="server" CssClass="clsLabelAuto">Pax</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPax" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
																							Text="<%# mLogDetail.Pax %>"></asp:TextBox>
																					</td>
																					<td>
																						<div class="clsLabelAuto">
																							No(s)
																						</div>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblCargoWeight" runat="server" CssClass="clsLabelAuto">Cargo Weight</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtCargoWeight" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" Text="<%# mLogDetail.CargoWeight %>"></asp:TextBox>
																					</td>
																					<td>
																						<div class="clsLabelAuto">
																							KG/LBS
																						</div>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblTakeOffWeight" runat="server" CssClass="clsLabelAuto">TakeOff Weight</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtTakeOffWeight" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							MaxLength="10" Text="<%# mLogDetail.TakeOffWeight %>"></asp:TextBox>
																					</td>
																					<td>
																						<div class="clsLabelAuto">
																							KG/LBS
																						</div>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																	<td valign="top">
																		<asp:UpdatePanel ID="upnlEmp" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<fieldset class="clsFieldSetNewStyle">
																					<legend style="font-weight: bold"><b>PFI (Pre-Flight Inspection)</b></legend>
																					<table>
																						<tr>
																							<td>
																								<span class="clsLabelAuto">PFI Done</span>
																							</td>
																							<td>
																								<asp:CheckBox ID="chkIsPFIDone" runat="server" ToolTip="Check if PFI is done" CssClass="clsCheckBox" AutoPostBack="true"
																									OnCheckedChanged="chkIsPFI_CheckChanged" ClientIDMode="Static" Checked="<%# mLogDetail.IsPFIDone  %>" />
																							</td>
																						</tr>
																						<tr>
																							<td>
																								<span class="clsLabelAuto">PFI Done By</span>
																							</td>
																							<td>
																								<asp:TextBox ID="txtEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
																									AutoPostBack="true" OnTextChanged="txtEmployee_TextChanged" CssClass="clsTextBoxTagSearchComboNewstyle"></asp:TextBox>
																								<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtEmployee_Autocomplete" runat="server"
																									DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
																									CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtEmployee"
																									UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
																									CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
																									OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
																									OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
																								</cc2:AutoCompleteExtender>
																							</td>
																						</tr>
																					</table>
																				</fieldset>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td align="right"></td>
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
										<asp:Label ID="lblTLPGridTitle" runat="server" CssClass="clsLabelHeader">TLP Details</asp:Label>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:DataGrid ID="dgLogDetails" runat="server" AutoGenerateColumns="False"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID "></asp:BoundColumn>
														<asp:BoundColumn DataField="SrNo" HeaderText="Sr No."></asp:BoundColumn>
														<asp:BoundColumn DataField="FlightNo" SortExpression="FlightNo" HeaderText="Flight No."></asp:BoundColumn>
														<asp:BoundColumn DataField="SourceName" SortExpression="SourceName" HeaderText="From"></asp:BoundColumn>
														<asp:BoundColumn DataField="DestinationName" SortExpression="DestinationName" HeaderText="To"></asp:BoundColumn>
														<asp:BoundColumn DataField="SouLocalDateTimeFormatted" SortExpression="SouLocalDateTimeFormatted"
															HeaderText="Chocks Off">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="SouUniverseDateTimeFormatted" SortExpression="SouUniverseDateTimeFormatted"
															HeaderText="UTC Chocks Off">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DesLocalDateTimeFormatted" SortExpression="DesLocalDateTimeFormatted"
															HeaderText="Chocks On">
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="DesUniverseDateTimeFormatted" SortExpression="DesUniverseDateTimeFormatted"
															HeaderText="UTC Chocks On">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn DataField="BlockTime" HeaderText="Block Time"></asp:BoundColumn>
														<asp:BoundColumn DataField="TakeOffLocalDateTimeFormatted" SortExpression="TakeOffLocalDateTimeFormatted"
															HeaderText="Take Off">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="TakeOffUniverseDateTimeFormatted" SortExpression="TakeOffUniverseDateTimeFormatted"
															HeaderText="UTC Take Off">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TouchDownLocalDateTimeFormatted" SortExpression="TouchDownLocalDateTimeFormatted"
															HeaderText="Touch Down">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="TouchDownUniverseDateTimeFormatted" SortExpression="TouchDownUniverseDateTimeFormatted"
															HeaderText="UTC Touch Down">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:BoundColumn DataField="TimeInAir" HeaderText="Flight Time"></asp:BoundColumn>
														<asp:BoundColumn DataField="Landings" SortExpression="Landings" HeaderText="Landings"></asp:BoundColumn>
														<asp:BoundColumn DataField="FuelOnDeparture" HeaderText="Fuel Dep."></asp:BoundColumn>
														<asp:BoundColumn DataField="FuelUplifted" HeaderText="Fuel Add."></asp:BoundColumn>
														<asp:BoundColumn DataField="FuelOnArrival" HeaderText="Fuel Arr."></asp:BoundColumn>
														<asp:BoundColumn DataField="Pax" HeaderText="Pax"></asp:BoundColumn>
														<asp:BoundColumn DataField="CargoWeight" HeaderText="Cargo"></asp:BoundColumn>
														<asp:BoundColumn DataField="TakeOffWeight" HeaderText="Take Off Weight"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="PFI Done">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPFIDone") %>'
																	Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="PFIDoneByEmpNoName" SortExpression="PFIDoneByEmpNoName"
															HeaderText="PFI Done By">
															<ItemStyle Wrap="false" />
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center"
															HeaderStyle-HorizontalAlign="Center">
															<ItemTemplate>
																<div id="dropDownImg" class="dropdown">
																	<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																	<div id="dropdownICN-content" class="dropdownbtn-content">
																		<table id="dropdown-content" class="clsGridNew_Ajax">
																			<tr>
																				<td>
																					<asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
																						CommandArgument='<%# Eval("SrNo") %>'
																						ToolTip="Click to Edit record" CausesValidation="false"
																						CommandName="Edit" ImageUrl="~/images/edit.png" />
																				</td>
																				<td>
																					<asp:ImageButton ID="deleteICN" CssClass="actionICNS" runat="server"
																						CommandArgument='<%# Eval("SrNo") %>' Visible='<%# mLog.LogDetails.Count > 1 %>'
																						ToolTip="Click to Delete record" CausesValidation="false"
																						CommandName="Remove" ImageUrl="~/images/delete.png" />
																				</td>
																			</tr>
																		</table>
																	</div>
																</div>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:DataGrid>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right"></td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
			<asp:UpdateProgress ID="AjaxLoader" DynamicLayout="false" DisplayAfter="200" runat="server">
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
		<script type="text/javascript">
			var Enable = function () {
				var IsPFIDoneChecked = $get("chkIsPFIDone").checked;
				if (IsPFIDoneChecked) {
					$("[id$='txtEmployee']").attr('disabled', false);

				}
				else {
					$("[id$='txtEmployee']").attr('disabled', true);
					$("[id$='txtEmployee']").val('');

				}
			}
		</script>
	</form>
	<!--  For Arrival,Departure,Take off,Touch down Date Time   -->
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			var savedlog1;
			if ("<%=mLogDetail.IsNew%>" == "True") {
				if ("<%=mLog.IsUTC%>" == "True" && document.getElementById('chkUTCArrival').checked) {
					savedlog1 = "button";
				}
				else if ("<%=mLog.IsUTC%>" == "False" && document.getElementById('chkArrival').checked) {
					savedlog1 = "button";
				}
				else {
					savedlog1 = "";
				}
			}
			else {
				savedlog1 = "";
			}


		});
        });

	</script>
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			var savedlog1;
			if ("<%=mLogDetail.IsNew%>" == "True") {
				if ("<%=mLog.IsUTC%>" == "True" && document.getElementById('chkUTCTouchDown').checked == true) {
					savedlog1 = "button";
				}
				else if ("<%=mLog.IsUTC%>" == "False" && document.getElementById('chkTouchDown').checked == true) {
					savedlog1 = "button";
				}
				else {
					savedlog1 = "";
				}
			}
			else {
				savedlog1 = "";
			}



		}); 
		});

	</script>
	<!-- Autocomplete for Source and Destination Place   -->
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=Place1.ClientID%>,#<%=Place2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
				width: 250,
				autoFill: true,
				matchContains: true,
				delay: 0

			});
		});
	</script>
	<script type="text/javascript">
		//Date validations
		function ValidateDateText(elem, extenderid) {

			var datevalue = $(elem).val();
			var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

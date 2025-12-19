<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogSOP_Ajax.aspx.vb"
	Inherits="Flypal.wfLogSOP_Ajax" %>

<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Log Details</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

	<script type="text/javascript" src="bootstrap/jquery-1.8.3.min.js"></script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>

	<script language="JavaScript" type="text/javascript">

		/*Fuel Oil*/
		function autoResizeFuelOil() {
			var newheight;
			var newwidth;

			if (document.getElementById) {
				newheight = document.getElementById('IframeFuelOil').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeFuelOil').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeFuelOil').height = (newheight + 30) + "px";
			document.getElementById('IframeFuelOil').width = (newwidth) + "px";
			document.getElementById('tbpnlFuelOil').height = (newheight) + "px";
			document.getElementById('tbpnlFuelOil').width = (newwidth) + "px";

			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


		}

		function CallFuelOil() {
			document.getElementById('IframeFuelOil').src = 'wfLogFuelOil_Ajax.aspx?Type=pup';
		}

		/*Snag Reporting*/
		function autoResizeSnagReporting() {
			var newheight;
			var newwidth;

			if (document.getElementById) {
				newheight = document.getElementById('IframeSnagReporting').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeSnagReporting').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeSnagReporting').height = (newheight + 60) + "px";
			document.getElementById('IframeSnagReporting').width = (newwidth) + "px";
			document.getElementById('tbpnlSnagReporting').height = (newheight) + "px";
			document.getElementById('tbpnlSnagReporting').width = (newwidth) + "px";

			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


		}

		function CallSnagReporting() {
			document.getElementById('IframeSnagReporting').src = 'wfLogDefectActionList_Ajax.aspx?Type=pup';
		}

		/*Parameter*/
		function autoResizeParameterList() {
			var newheight;
			var newwidth;

			if (document.getElementById) {
				newheight = document.getElementById('IframeParameterList').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeParameterList').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeParameterList').height = (newheight + 2) + "px";
			document.getElementById('IframeParameterList').width = (newwidth) + "px";
			document.getElementById('tbpnlParameterList').height = (newheight) + "px";
			document.getElementById('tbpnlParameterList').width = (newwidth) + "px";

			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


		}

		function CallParameterList() {
			document.getElementById('IframeParameterList').src = 'wfLogParameterList_Ajax.aspx?Type=pup';
		}

		/*FlightCrew*/
		function autoResizeFlightCrewList() {
			var newheight;
			var newwidth;

			if (document.getElementById) {
				newheight = document.getElementById('IframeFlightCrewList').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeFlightCrewList').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeFlightCrewList').height = (newheight + 2) + "px";
			document.getElementById('IframeFlightCrewList').width = (newwidth) + "px";
			document.getElementById('tbpnlFlightCrewList').height = (newheight) + "px";
			document.getElementById('tbpnlFlightCrewList').width = (newwidth) + "px";

			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";

		}

		function CallFlightCrewList() {
			document.getElementById('IframeFlightCrewList').src = 'wfLogFlightCrew_Ajax.aspx?Type=pup';
		}

		/*Maint Activity*/
		function autoResizeMaintActivity() {
			var newheight;
			var newwidth;

			if (document.getElementById) {
				newheight = document.getElementById('IframeMaintActivity').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeMaintActivity').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeMaintActivity').height = (newheight + 2) + "px";
			document.getElementById('IframeMaintActivity').width = (newwidth) + "px";
			document.getElementById('tbpnlMaintActivity').height = (newheight) + "px";
			document.getElementById('tbpnlMaintActivity').width = (newwidth) + "px";

			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";


		}

		function CallMaintActivity() {
			document.getElementById('IframeMaintActivity').src = 'wfLogMaintenanceActivity_Ajax.aspx?Type=pup';
		}

		function autoResizeDeferredDiscrepancy() {
			var newheight;
			var newwidth;
			if (document.getElementById) {
				newheight = document.getElementById('IframeDeferredDiscrepancy').contentWindow.document.body.scrollHeight;
				newwidth = document.getElementById('IframeDeferredDiscrepancy').contentWindow.document.body.scrollWidth;
			}
			document.getElementById('IframeDeferredDiscrepancy').height = (newheight + 30) + "px";
			document.getElementById('IframeDeferredDiscrepancy').width = (newwidth + 50) + "px";
			document.getElementById('tbpnlDeferredDiscrepancies').height = (newheight + 10) + "px";
			document.getElementById('tbpnlDeferredDiscrepancies').width = (newwidth + 10) + "px";
			document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
			document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";
		}

		function callDeferredDiscrepancies() {
			document.getElementById('IframeDeferredDiscrepancy').src = 'wfDiscrepancyCorrectiveActionListFromLog.aspx?Type=pup&Troubleshoot=1';
		}

		function autoResizeDiscrepancyReporting() {

			console.log('autoResizeDiscrepancyReporting() called');

			var newheight;
			var newwidth;

			try {

				if (document.getElementById) {

					newheight = document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollHeight;
					newwidth = document.getElementById('IframeDiscrepancyReporting').contentWindow.document.body.scrollWidth;

				}

				document.getElementById('IframeDiscrepancyReporting').height = (newheight + 2) + "px";
				document.getElementById('IframeDiscrepancyReporting').width = (newwidth + 50) + "px";
				document.getElementById('tbpnlDiscrepancyReporting').height = (newheight) + "px";
				document.getElementById('tbpnlDiscrepancyReporting').width = (newwidth) + "px";

				document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
				document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";

				console.log("autoResizeDiscrepancyReporting() ended");

			} catch (e) {
				console.error("Error ocuured in autoResizeDiscrepancyReporting(). Refer the Error " + e);
				alert(e);
			}

		}

		function callDiscrepancyReporting() {

			try {

				console.log("callDiscrepancyReporting() started");

				document.getElementById('IframeDiscrepancyReporting').src = 'wfDiscrepancyCorrectiveActionListFromLog.aspx?Type=pup&Troubleshoot=0';

			} catch (e) {
				console.error("Error ocuured in callDiscrepancyReporting(). Refer the Error " + e);
				alert(e);
			}

		}

		function autoResizeCabinDefectList() {

			var newheight;
			var newwidth;

			try {

				console.log("autoResizeCabinDefectList() started");
				if (document.getElementById) {

					newheight = document.getElementById('IframeCabinDefect').contentWindow.document.body.scrollHeight;
					newwidth = document.getElementById('IframeCabinDefect').contentWindow.document.body.scrollWidth;

				}

				document.getElementById('IframeCabinDefect').height = (newheight + 2) + "px";
				document.getElementById('IframeCabinDefect').width = (newwidth) + "px";

				document.getElementById('tbpnlCabinDefect').height = (newheight) + "px";
				document.getElementById('tbpnlCabinDefect').width = (newwidth) + "px";

				document.getElementById('tabLogDetailsContainer').height = (newheight) + "px";
				document.getElementById('tabLogDetailsContainer').width = (newwidth) + "px";

				console.log("autoResizeCabinDefectList() endded");

			} catch (e) {
				console.error("Error ocuured in autoResizeCabinDefectList(). Refer the Error " + e);
				alert(e);
			}

		}

		function ShowCabinDefectList() {

			try {

				console.log("ShowCabinDefectList() started");
				var TransTypeID = 116;

				$("#IframeCabinDefect").attr("src", "wfDiscrepancyCorrectiveActionListFromLog.aspx?Type=pup&Troubleshoot=0&TransTypeID=" + TransTypeID);

			} catch (e) {
				console.error("Error ocuured in ShowCabinDefectList(). Refer the Error " + e);
				alert(e);
			}

		}

	</script>

</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
	<form id="form1" runat="server" enctype="multipart/form-data" method="post">

		<%--AJAX- ScriptManager Added--%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>

		<%--AJAX- New function added as Focus gets Lost when we use tabs in Grid--%>
		<script language="javascript" type="text/javascript">

			var g_CurrentTextBox;
			var g_isTabPressed;

			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
			function endRequestHandler() {

				try {

					$get(g_CurrentTextBox).focus();
					$get(g_CurrentTextBox).select();

					g_isTabPressed = 0;

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

		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>

		<div>

			<asp:UpdatePanel ID="upnlTabsNew" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table class="clstablelistout Table-MaxWidth" id="tblMain">
						<tr>
							<td>
								<cc2:TabContainer ID="tabLogDetailsContainer" runat="server" class="clstablelistin"
									AutoPostBack="true">

									<cc2:TabPanel ID="tabLogDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">

										<HeaderTemplate>
											<asp:Label runat="server" Text="Log Details" ID="lblHeaderLogDetails" />
										</HeaderTemplate>

										<ContentTemplate>
											<table width="100%">
												<tr>
													<td class="clsFormHeader1Newstyle">
														<table width="100%">
															<tr>

																<td>
																	<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:Label ID="lblTitle" runat="server"
																				CssClass="clsFormHeader">Log Details</asp:Label>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td align="right">
																	<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<table id="Table7" border="0" cellspacing="0">
																				<tr>
																					<td>
																						<asp:Button ID="btnAddNew" runat="server"
																							CssClass="clsbtnH clsinfoH"
																							ToolTip="Click to Save the Log and add New Log"
																							Text="Save &amp; New" />
																					</td>
																					<td>
																						<asp:Button ID="btnSave" runat="server"
																							CssClass="clsbtnH clsinfoH"
																							ToolTip="Click to Save the Record"
																							Text="Save" />
																					</td>
																					<td>
																						<asp:Button ID="btnPrint" runat="server"
																							CssClass="clsbtnH clsinfoH"
																							CausesValidation="False"
																							Text="Print" Visible="False" />
																					</td>
																					<td>
																						<asp:Button ID="btnBack" runat="server"
																							CssClass="clsbtnH clsinfoH"
																							ToolTip="Back to Previous Page"
																							Text="Back" />
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
												<tr>
													<td>
														<asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table width="100%" style="display: none;">
																	<tr>
																		<td>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblLogDetails" runat="server"
																							CssClass="clsLabelButton"
																							ToolTip="Log details">Log details</asp:Label>
																					</td>
																					<td>
																						<asp:Button ID="btnFuelOil" runat="server"
																							CssClass="clsButtonLong_Ajax"
																							CausesValidation="False"
																							Text="Fuel Oil" />
																					</td>
																					<td>
																						<asp:Button ID="btnDefectActionList" runat="server"
																							CssClass="clsButtonLong_Ajax"
																							Visible='<%#IIf(AppSettings("ShowNewDiscrepancyFlow") = "True", False, True) %>'
																							CausesValidation="False"
																							Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Reporting", "Snag Reporting") %>' />
																					</td>
																					<td>
																						<asp:Button ID="btnParameterList" runat="server"
																							CssClass="clsButtonLong_Ajax"
																							CausesValidation="False"
																							Text="Parameter List" />
																					</td>
																					<td>
																						<asp:Button ID="btnLogPax" runat="server"
																							CssClass="clsButtonLong_Ajax" CausesValidation="False"
																							Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>'
																							Text="Passenger Log" />
																					</td>
																					<td>
																						<asp:Button ID="btnHobbsOffset" runat="server"
																							CssClass="clsButtonLong_Ajax"
																							CausesValidation="False"
																							Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>'
																							Text="Hobbs Offset" />
																					</td>
																					<td>
																						<asp:Button ID="btnFlightCrew" runat="server"
																							CssClass="clsButtonLong_Ajax"
																							CausesValidation="False"
																							Text="Flight Crew" />
																					</td>
																					<td>
																						<asp:Button Style="z-index: 0" ID="btnMaintenanceAcitvity"
																							runat="server" CssClass="clsButtonLong_Ajax"
																							CausesValidation="False" Text="Maintenance Activity" />
																					</td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																</table>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td>
														<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Panel runat="server">
																	<asp:ValidationSummary ID="Validationsummary2"
																		CssClass="clsValidationSummary" runat="server"
																		HeaderText="Fill Up The Following Fields" />
																	<asp:CustomValidator ID="cvRemark" runat="server"
																		ErrorMessage="Remark Can't be greater than 200 chars"
																		ControlToValidate="txtRemark" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvAirFrame" runat="server" Display="None"
																		OnServerValidate="CustomValidation" />
																	<asp:CustomValidator ID="cvGroundRunTime" runat="server"
																		ErrorMessage="Departure date should be in date time format."
																		ControlToValidate="txtGroundRunTime" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvAirBornTime" runat="server"
																		ErrorMessage="Cannot be Nigative."
																		ControlToValidate="txtAirBorneTime" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvPilot1" runat="server"
																		ErrorMessage="Enter correct Pilot1 name."
																		ControlToValidate="Pilot1" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvPilot2" runat="server"
																		ErrorMessage="Enter correct Pilot2 name."
																		ControlToValidate="Pilot2" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvPlace1" runat="server"
																		ErrorMessage="Enter correct Source name."
																		ControlToValidate="Place1" Display="None"
																		OnServerValidate="CustomValidate" />
																	<asp:CustomValidator ID="cvPlace2" runat="server"
																		ErrorMessage="Enter correct Destination name."
																		ControlToValidate="Place2" Display="None"
																		OnServerValidate="CustomValidate" />
																	<%--Sankalp 19-Aug-25--%>
																	<asp:CustomValidator ID="cvClassification" runat="server"
																		ErrorMessage="Select Classification"
																		ControlToValidate="cmbFlightLogClassification"
																		Display="None" OnServerValidate="customvalidate" />
																</asp:Panel>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td>
														<asp:UpdatePanel ID="upnlLogDetails" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle">
																	<legend id="lgLogDetails" runat="server">
																		<div style="width: 100%">
																			<asp:Label ID="lblLogDetail" runat="server" Font-Bold="true"
																				CssClass="clsLabelHeader" Text="Log Details" />
																		</div>
																	</legend>
																	<table width="100%">
																		<tr>
																			<td valign="top">
																				<table width="100%">
																					<tr>
																						<td style="width: 2%;">
																							<asp:Label ID="lblCalDate" runat="server"
																								CssClass="clsLabelStar" Text="*" />
																						</td>
																						<td style="width: 25%;">
																							<asp:Label ID="lblDateTime" runat="server"
																								CssClass="clsLabelAuto"
																								Text='<%# IIf(mMachine.IsUTC = True, "Date (UTC)", "Date") %>' />
																						</td>
																						<td>
																							<asp:TextBox runat="server" ID="calDateTime"
																								CssClass="clsTextBoxTagSearchDate"
																								AutoPostBack="true"
																								onchange="ValidateDateText(this,'calDateTime_watermarkextender');" />
																							<cc2:CalendarExtender ID="calDateTime_CalendarExtender"
																								runat="server" CssClass="cal_Theme1"
																								Enabled="true" Format="<%$AppSettings:DateFormat%>"
																								TargetControlID="calDateTime" />
																							<cc2:TextBoxWatermarkExtender TargetControlID="calDateTime"
																								ID="calDateTime_watermarkextender"
																								ClientIDMode="Static" runat="server"
																								WatermarkText="<%$AppSettings:DateFormat%>"
																								WatermarkCssClass="clsDateTextBox" />
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																		<tr>
																			<td valign="top" width="50%">
																				<table width="100%">
																					<tr>
																						<td></td>
																						<td>
																							<asp:Label ID="lblLogNo" runat="server"
																								CssClass="clsLabelAuto" Text="Log No." />
																						</td>
																						<td colspan="3">
																							<asp:TextBox ID="txtLogText" runat="server"
																								BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																								ReadOnly="True" Text="<%# mLog.LogText %>"
																								ToolTip="Log Number" />
																							<asp:TextBox ID="txtLogNo" runat="server"
																								BackColor="#E0E0E0"
																								CssClass="clsTextBoxTagSearchSmall"
																								ReadOnly="True" Text="<%# mLog.LogNo %>" />
																						</td>
																					</tr>
																					<asp:PlaceHolder ID="SingleAttachment" runat="server"
																						Visible='<%#IIf(AppSettings("ClientCode") = "Heligo" Or
                                                                                                    AppSettings("ClientCode") = "UHPL" Or
                                                                                                    AppSettings("ClientCode") = "APFT" Or
                                                                                                    AppSettings("ClientCode") = "AAP", False, True) %>'>
																						<tr>
																							<td>
																								<asp:Label ID="lblPilotStar1" runat="server"
																									CssClass="clsLabelStar"
																									Visible="<%#Not mLog.IsHobbs %>">*</asp:Label>
																							</td>
																							<td>
																								<asp:Label ID="lblPilotComm" runat="server"
																									CssClass="clsLabelAuto">Pilot in Command</asp:Label>
																							</td>
																							<td colspan="3">
																								<asp:TextBox ID="Pilot1" runat="server"
																									CssClass="clsTextBoxTagSearch autocomplete"
																									Enabled='<%#IIf(AppSettings("ClientCode") = "7AR", False, True) %>'
																									Text="<%# mLog.Pilot1Name %>" Width="268px" />

																								<asp:TextBox ID="txtPilot1" runat="server"
																									BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																									ReadOnly="True" Text="<%# mLog.Pilot1Name %>"
																									ToolTip="Pilot #1 Name" Visible="False"
																									Width="250px" />

																								<asp:ImageButton ID="imgbtnPilot1" runat="server"
																									CausesValidation="False" CssClass="clsButtonImg"
																									ImageUrl="ICONS/ADD.ICO" ToolTip="Select Pilot #1"
																									Visible="False" />
																							</td>
																						</tr>
																						<tr>
																							<td>
																								<asp:ImageButton ID="ImageButton2" runat="server"
																									ImageUrl="icons/CLIP01.ICO" Visible="False"
																									Width="24px" CssClass="clsButtonImg_Ajax" />
																							</td>
																							<td>
																								<span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
																							</td>
																							<td colspan="3">
																								<asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<table border="0" cellpadding="0" cellspacing="0">
																											<tr>
																												<td>
																													<input type="button" id="btnSelectFile"
																														value="Select File" style="width: 120px;"
																														runat="server" class="clsbtnH clsinfoH1"
																														causesvalidation="False" tabindex="6" />
																												</td>
																												<td style="padding-left: 10px;">
																													<asp:Button ID="btnDelAttch" runat="server"
																														CssClass="clsbtnH clsinfoH1"
																														ToolTip="Click to Remove Attachment"
																														Text="Remove Attachment" Enabled="False"
																														Width="145px" />
																												</td>
																												<td style="padding-left: 2px;">
																													<asp:ImageButton ID="ImageButton1" runat="server"
																														CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
																														Height="20px" Width="20px" />
																												</td>
																											</tr>
																										</table>
																									</ContentTemplate>
																								</asp:UpdatePanel>
																							</td>
																						</tr>
																					</asp:PlaceHolder>
																					<asp:PlaceHolder ID="phEngineDerate" runat="server"
																						Visible='<%#IIf(CBool(AppSettings("ShowEngineDerateOptions")), True, False) %>'>
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblEngineDerate" runat="server"
																									CssClass="clsLabelAuto" Text="Engine Derate" />
																							</td>
																							<td colspan="3">
																								<asp:DropDownList ID="ddlEngineDerate" runat="server"
																									CssClass="clsTextBoxTagSearchComboNewstyle"
																									DataTextField="Name" DataValueField="ID" />
																							</td>
																						</tr>
																					</asp:PlaceHolder>
																				</table>
																			</td>
																			<td valign="top" width="50%">
																				<table width="100%">
																					<tr>
																						<td></td>
																						<td>
																							<asp:Label ID="lblLogPageNo" runat="server"
																								CssClass="clsLabelAuto" Text="Page No." />
																						</td>
																						<td colspan="3">
																							<asp:TextBox ID="txtLogPageNo" runat="server"
																								CssClass="clsTextBoxTagSearchSmall" MaxLength="9"
																								Text="<%# mLog.LogPageNoFormatted %>"
																								ToolTip="Enter Log Page No." TabIndex="2" />
																							<asp:Label ID="lblFlightNo" runat="server"
																								CssClass="clsLabelAuto">Flight No.</asp:Label>
																							<asp:TextBox ID="txtFlightNo" runat="server"
																								CssClass="clsTextBoxTagSearchSmall" MaxLength="10"
																								Text="<%# mLog.FlightNo %>" ToolTip="Enter Flight No."
																								TabIndex="3" />
																						</td>
																					</tr>
																					<asp:PlaceHolder ID="PlaceHolder1" runat="server"
																						Visible='<%#IIf(AppSettings("ClientCode") = "Heligo" Or
                                                                                                    AppSettings("ClientCode") = "UHPL" Or
                                                                                                    AppSettings("ClientCode") = "APFT" Or
                                                                                                    AppSettings("ClientCode") = "AAP", False, True) %>'>
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblCo" runat="server"
																									CssClass="clsLabelAuto" Text="Co-Pilot" />
																							</td>
																							<td colspan="3">
																								<asp:TextBox ID="Pilot2" runat="server"
																									Enabled='<%#IIf(AppSettings("ClientCode") = "7AR", False, True) %>'
																									Text="<%# mLog.Pilot2Name %>" Width="255px"
																									ToolTip="Pilot #2 Name" TabIndex="5"
																									CssClass="clsTextBoxTagSearch " />
																								<asp:ImageButton ID="btnAddPilots" runat="server"
																									CausesValidation="False" Height="20px"
																									ImageUrl="~/images/plus1.png"
																									ToolTip="Click to Add new pilot" Width="24px" />
																								<asp:ImageButton ID="imgbtnPilot2" runat="server"
																									CausesValidation="False" CssClass="clsButtonImg_Ajax"
																									ImageUrl="ICONS/ADD.ICO" ToolTip="Select Pilot #2 Name"
																									Visible="False" />
																							</td>
																						</tr>
																					</asp:PlaceHolder>
																					<tr>
																						<td></td>
																						<td>
																							<%-- Sankalp 19-Aug-2025 --%>
																							<asp:Label ID="lblClassificationStar" runat="server"
																								CssClass="clsLabelStar"
																								Visible='<%#IIf(AppSettings("ClientCode") = "AFC", True, False) %>'
																								Text="*" />
																							<asp:Label ID="lblFlightLogClassification"
																								runat="server"
																								CssClass="clsLabelAuto"
																								Text="Classification" />
																						</td>
																						<td colspan="3">
																							<asp:DropDownList ID="cmbFlightLogClassification"
																								runat="server"
																								CssClass="clsTextBoxTagSearchCombo"
																								DataTextField="Name" DataValueField="ID"
																								Width="258px" TabIndex="8" />
																							<asp:ImageButton ID="btnFlightLogClassifications"
																								runat="server" CausesValidation="False"
																								Height="20px" ImageUrl="~/images/plus1.png"
																								ToolTip="Add new Classification"
																								Width="24px" />
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																	</table>
																</fieldset>
																<input type="hidden" id="LogObjValue" runat="server" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%#IIf(AppSettings("ClientCode") = "Heligo" Or
                                                                                                                AppSettings("ClientCode") = "UHPL" Or
                                                                                                                AppSettings("ClientCode") = "APFT" Or
                                                                                                                AppSettings("ClientCode") = "AAP", False, True) %>'>
													<tr>
														<td valign="top">
															<asp:UpdatePanel ID="upnlFlightDetails" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table width="100%">
																		<tr>
																			<td valign="top" style="width: 50%">
																				<fieldset class="clsFieldSetNewStyle">
																					<legend id="Legend3" runat="server" style="font-weight: bold">Departure</legend>
																					<table width="100%">
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
																							</td>
																							<td colspan="3">
																								<table width="100%">
																									<tr>
																										<td style="width: 20px;"></td>
																										<td>
																											<asp:TextBox ID="Place1" runat="server"
																												CssClass="clsTextBoxTagSearch" Width="268px"
																												Text="<%# mLog.SourceName %>" TabIndex="9" />
																										</td>
																										<td>
																											<asp:ImageButton ID="imgbtnDepPlace" runat="server"
																												CausesValidation="False" CssClass="clsButtonImg_Ajax"
																												Enabled="<%# mLog.IsNew %>" ImageUrl="ICONS/ADD.ICO"
																												ToolTip="Select Place" Visible="False" />
																										</td>
																									</tr>
																								</table>
																							</td>
																						</tr>
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblDepDateTime" runat="server"
																									CssClass="clsLabelAuto">Date / Time</asp:Label>
																								<asp:Label ID="lblUTCDateTime" runat="server"
																									CssClass="clsLabelAuto">UTC Date / Time</asp:Label>
																							</td>
																							<td>&nbsp;&nbsp;&nbsp;
                                                                                                &nbsp;&nbsp;&nbsp;
                                                                                            <asp:TextBox runat="server" ID="calDeparture"
																								CssClass="clsTextBoxTagSearch" Width="100px"
																								BackColor="#E0E0E0" ReadOnly="true"
																								AutoPostBack="true" CausesValidation="True"
																								onchange="ValidateDateText(this,'calDeparture_watermarkextender');" />
																								<cc2:CalendarExtender ID="calDeparture_CalendarExtender"
																									runat="server" CssClass="cal_Theme1" Enabled="false"
																									Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="calDeparture" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="calDeparture"
																									ID="calDeparture_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox runat="server" ID="CalUTCDateTime"
																									CssClass="clsTextBoxTagSearch" Width="100px"
																									BackColor="#E0E0E0" ReadOnly="true"
																									AutoPostBack="True" CausesValidation="True"
																									onchange="ValidateDateText(this,'CalUTCDateTime_watermarkextender');" />
																								<cc2:CalendarExtender ID="CalUTCDateTime_CalendarExtender"
																									runat="server" CssClass="cal_Theme1"
																									Enabled="false" Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="CalUTCDateTime" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="CalUTCDateTime"
																									ID="CalUTCDateTime_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox ID="txtDepartureTime" runat="server"
																									AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																									Width="70px" MaxLength="10" ReadOnly="True"
																									ToolTip="Enter Departure Time." onfocus="onTextFocus();" />
																								<asp:TextBox ID="txtUTCDepartureTime"
																									runat="server" AutoPostBack="True"
																									CssClass="clsTextBoxTagSearchSmall"
																									Width="70px" TabIndex="11"
																									MaxLength="10" ToolTip="Enter UTC Departure Time." />
																								<cc2:MaskedEditExtender ID="txtDepartureTimeMaskedEditExtender"
																									TargetControlID="txtDepartureTime"
																									AutoComplete="true" Mask="99:99"
																									MaskType="Time" CultureName="en-us"
																									MessageValidatorTip="true"
																									runat="server" />
																								<cc2:MaskedEditExtender
																									ID="txtUTCDepartureTimeMaskedEditExtender"
																									TargetControlID="txtUTCDepartureTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time"
																									CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																							</td>
																							<td>
																								<asp:Label ID="lblDepDayLightTime" runat="server"
																									CssClass="clsLabelAuto" Visible="False">D / L Time</asp:Label>
																							</td>
																							<td>
																								<asp:DropDownList ID="cmbDepartureDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
																									DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.SouDayLightTime %>"
																									Visible="False">
																									<asp:ListItem Selected="True" Value="-12:00">-12:00</asp:ListItem>
																									<asp:ListItem Value="-11:45">-11:45</asp:ListItem>
																									<asp:ListItem Value="-11:30">-11:30</asp:ListItem>
																									<asp:ListItem Value="-11:15">-11:15</asp:ListItem>
																									<asp:ListItem Value="-11:00">-11:00</asp:ListItem>
																									<asp:ListItem Value="-10:45">-10:45</asp:ListItem>
																									<asp:ListItem Value="-10:30">-10:30</asp:ListItem>
																									<asp:ListItem Value="-10:15">-10:15</asp:ListItem>
																									<asp:ListItem Value="-10:00">-10:00</asp:ListItem>
																									<asp:ListItem Value="-09:45">-09:45</asp:ListItem>
																									<asp:ListItem Value="-09:30">-09:30</asp:ListItem>
																									<asp:ListItem Value="-09:15">-09:15</asp:ListItem>
																									<asp:ListItem Value="-09:00">-09:00</asp:ListItem>
																									<asp:ListItem Value="-08:45">-08:45</asp:ListItem>
																									<asp:ListItem Value="-08:30">-08:30</asp:ListItem>
																									<asp:ListItem Value="-08:15">-08:15</asp:ListItem>
																									<asp:ListItem Value="-08:00">-08:00</asp:ListItem>
																									<asp:ListItem Value="-07:45">-07:45</asp:ListItem>
																									<asp:ListItem Value="-07:30">-07:30</asp:ListItem>
																									<asp:ListItem Value="-07:15">-07:15</asp:ListItem>
																									<asp:ListItem Value="-07:00">-07:00</asp:ListItem>
																									<asp:ListItem Value="-06:45">-06:45</asp:ListItem>
																									<asp:ListItem Value="-06:30">-06:30</asp:ListItem>
																									<asp:ListItem Value="-06:15">-06:15</asp:ListItem>
																									<asp:ListItem Value="-06:00">-06:00</asp:ListItem>
																									<asp:ListItem Value="-05:45">-05:45</asp:ListItem>
																									<asp:ListItem Value="-05:30">-05:30</asp:ListItem>
																									<asp:ListItem Value="-05:15">-05:15</asp:ListItem>
																									<asp:ListItem Value="-05:00">-05:00</asp:ListItem>
																									<asp:ListItem Value="-04:45">-04:45</asp:ListItem>
																									<asp:ListItem Value="-04:30">-04:30</asp:ListItem>
																									<asp:ListItem Value="-04:15">-04:15</asp:ListItem>
																									<asp:ListItem Value="-04:00">-04:00</asp:ListItem>
																									<asp:ListItem Value="-03:45">-03:45</asp:ListItem>
																									<asp:ListItem Value="-03:30">-03:30</asp:ListItem>
																									<asp:ListItem Value="-03:15">-03:15</asp:ListItem>
																									<asp:ListItem Value="-03:00">-03:00</asp:ListItem>
																									<asp:ListItem Value="-02:45">-02:45</asp:ListItem>
																									<asp:ListItem Value="-02:30">-02:30</asp:ListItem>
																									<asp:ListItem Value="-02:15">-02:15</asp:ListItem>
																									<asp:ListItem Value="-02:00">-02:00</asp:ListItem>
																									<asp:ListItem Value="-01:45">-01:45</asp:ListItem>
																									<asp:ListItem Value="-01:30">-01:30</asp:ListItem>
																									<asp:ListItem Value="-01:15">-01:15</asp:ListItem>
																									<asp:ListItem Value="-01:00">-01:00</asp:ListItem>
																									<asp:ListItem Value="-00:45">-00:45</asp:ListItem>
																									<asp:ListItem Value="-00:30">-00:30</asp:ListItem>
																									<asp:ListItem Value="-00:15">-00:15</asp:ListItem>
																									<asp:ListItem Value="+00:00">+00:00</asp:ListItem>
																									<asp:ListItem Value="+00:15">+00:15</asp:ListItem>
																									<asp:ListItem Value="+00:30">+00:30</asp:ListItem>
																									<asp:ListItem Value="+00:45">+00:45</asp:ListItem>
																									<asp:ListItem Value="+01:00">+01:00</asp:ListItem>
																									<asp:ListItem Value="+01:15">+01:15</asp:ListItem>
																									<asp:ListItem Value="+01:30">+01:30</asp:ListItem>
																									<asp:ListItem Value="+01:45">+01:45</asp:ListItem>
																									<asp:ListItem Value="+02:00">+02:00</asp:ListItem>
																									<asp:ListItem Value="+02:15">+02:15</asp:ListItem>
																									<asp:ListItem Value="+02:30">+02:30</asp:ListItem>
																									<asp:ListItem Value="+02:45">+02:45</asp:ListItem>
																									<asp:ListItem Value="+03:00">+03:00</asp:ListItem>
																									<asp:ListItem Value="+03:15">+03:15</asp:ListItem>
																									<asp:ListItem Value="+03:30">+03:30</asp:ListItem>
																									<asp:ListItem Value="+03:45">+03:45</asp:ListItem>
																									<asp:ListItem Value="+04:00">+04:00</asp:ListItem>
																									<asp:ListItem Value="+04:15">+04:15</asp:ListItem>
																									<asp:ListItem Value="+04:30">+04:30</asp:ListItem>
																									<asp:ListItem Value="+04:45">+04:45</asp:ListItem>
																									<asp:ListItem Value="+05:00">+05:00</asp:ListItem>
																									<asp:ListItem Value="+05:15">+05:15</asp:ListItem>
																									<asp:ListItem Value="+05:30">+05:30</asp:ListItem>
																									<asp:ListItem Value="+05:45">+05:45</asp:ListItem>
																									<asp:ListItem Value="+06:00">+06:00</asp:ListItem>
																									<asp:ListItem Value="+06:15">+06:15</asp:ListItem>
																									<asp:ListItem Value="+06:30">+06:30</asp:ListItem>
																									<asp:ListItem Value="+06:45">+06:45</asp:ListItem>
																									<asp:ListItem Value="+07:00">+07:00</asp:ListItem>
																									<asp:ListItem Value="+07:15">+07:15</asp:ListItem>
																									<asp:ListItem Value="+07:30">+07:30</asp:ListItem>
																									<asp:ListItem Value="+07:45">+07:45</asp:ListItem>
																									<asp:ListItem Value="+08:00">+08:00</asp:ListItem>
																									<asp:ListItem Value="+08:15">+08:15</asp:ListItem>
																									<asp:ListItem Value="+08:30">+08:30</asp:ListItem>
																									<asp:ListItem Value="+08:45">+08:45</asp:ListItem>
																									<asp:ListItem Value="+09:00">+09:00</asp:ListItem>
																									<asp:ListItem Value="+09:15">+09:15</asp:ListItem>
																									<asp:ListItem Value="+09:30">+09:30</asp:ListItem>
																									<asp:ListItem Value="+09:45">+09:45</asp:ListItem>
																									<asp:ListItem Value="+10:00">+10:00</asp:ListItem>
																									<asp:ListItem Value="+10:15">+10:15</asp:ListItem>
																									<asp:ListItem Value="+10:30">+10:30</asp:ListItem>
																									<asp:ListItem Value="+10:45">+10:45</asp:ListItem>
																									<asp:ListItem Value="+11:00">+11:00</asp:ListItem>
																									<asp:ListItem Value="+11:15">+11:15</asp:ListItem>
																									<asp:ListItem Value="+11:30">+11:30</asp:ListItem>
																									<asp:ListItem Value="+11:45">+11:45</asp:ListItem>
																									<asp:ListItem Value="+12:00">+12:00</asp:ListItem>
																								</asp:DropDownList>
																							</td>
																						</tr>
																						<tr>
																							<td>&nbsp;
																							</td>
																							<td>
																								<asp:Label ID="lblTakeOffLocalDateTime" runat="server"
																									CssClass="clsLabelAuto" Text="Take Off Date / Time" />
																								<asp:Label ID="lblUTCTakeOffDateTime" runat="server"
																									CssClass="clsLabelAuto" Text="UTC Take Off Date / Time" />
																							</td>
																							<td colspan="3">
																								<asp:CheckBox ID="chkTakeOff" runat="server" AutoPostBack="True"
																									ToolTip="Check to enable Take Off Date" />
																								<asp:TextBox runat="server" ID="calTakeOffLocalDateTime"
																									CssClass="clsTextBoxTagSearch"
																									BackColor="#E0E0E0" ReadOnly="true" Width="100px"
																									AutoPostBack="True" CausesValidation="True"
																									onchange="ValidateDateText(this,'calTakeOffLocalDateTime_watermarkextender');" />
																								<cc2:CalendarExtender ID="calTakeOffLocalDateTime_CalendarExtender"
																									runat="server" CssClass="cal_Theme1" Enabled="false"
																									Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="calTakeOffLocalDateTime" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="calTakeOffLocalDateTime"
																									ID="calTakeOffLocalDateTime_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox runat="server" ID="calUTCTakeOffDateTime"
																									CssClass="clsTextBoxTagSearch"
																									BackColor="#E0E0E0" ReadOnly="true" Width="100px"
																									AutoPostBack="True" CausesValidation="True"
																									onchange="ValidateDateText(this,'calUTCTakeOffDateTime_watermarkextender');" />
																								<cc2:CalendarExtender ID="calUTCTakeOffDateTime_CalendarExtender"
																									runat="server" CssClass="cal_Theme1" Enabled="false"
																									Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="calUTCTakeOffDateTime" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="calUTCTakeOffDateTime"
																									ID="calUTCTakeOffDateTime_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox ID="txtTakeOffLocalTime"
																									runat="server" AutoPostBack="True"
																									CssClass="clsTextBoxTagSearchSmall"
																									Width="70px" TabIndex="14"
																									MaxLength="10" ToolTip="Enter Take Off Time." />
																								<asp:TextBox ID="txtUTCTakeOffTime" runat="server"
																									AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																									Width="70px" TabIndex="12"
																									MaxLength="10" ToolTip="Enter UTC Take Off Time." />
																								<cc2:MaskedEditExtender ID="txtTakeOffLocalTimeMaskededitextender"
																									TargetControlID="txtTakeOffLocalTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time"
																									CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																								<cc2:MaskedEditExtender ID="txtUTCTakeOffTimeMaskededitextender"
																									TargetControlID="txtUTCTakeOffTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time"
																									CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																							</td>
																						</tr>
																					</table>
																				</fieldset>
																			</td>
																			<td valign="top" style="width: 50%">
																				<fieldset class="clsFieldSetNewStyle">
																					<legend id="Legend1" runat="server" style="font-weight: bold">Arrival</legend>
																					<table width="100%">
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblArrPlace" runat="server"
																									CssClass="clsLabelAuto">Place</asp:Label>
																							</td>
																							<td colspan="3">
																								<table width="100%">
																									<tr>
																										<td style="width: 20px;"></td>
																										<td colspan="3">
																											<asp:TextBox ID="Place2" runat="server"
																												CssClass="clsTextBoxTagSearch"
																												Text="<%# mLog.DestinationName %>"
																												Width="268px" TabIndex="10" />
																											<asp:ImageButton ID="btnAddPlace"
																												runat="server" CausesValidation="False"
																												Height="20px" ImageUrl="~/images/plus1.png"
																												ToolTip="Click to Add new Place" Width="24px" />
																											<asp:ImageButton ID="imgbtnArrPlace" runat="server"
																												CausesValidation="False" CssClass="clsButtonImg_Ajax"
																												Enabled="<%# mLog.IsNew %>" ImageUrl="ICONS/ADD.ICO"
																												ToolTip="Select Place" Visible="False" />
																										</td>
																									</tr>
																								</table>
																							</td>
																						</tr>
																						<tr>
																							<td></td>
																							<td>
																								<asp:Label ID="lblArrDate" runat="server"
																									CssClass="clsLabelAuto">Date / Time</asp:Label>
																								<asp:Label ID="lblUTCArrivalDateTime"
																									runat="server" CssClass="clsLabelAuto">UTC DateTime</asp:Label>
																							</td>
																							<td>
																								<asp:CheckBox ID="chkArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date" />
																								<asp:TextBox runat="server" ID="calArrival" CssClass="clsTextBoxTagSearch" Width="100px"
																									BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" onchange="ValidateDateText(this,'calArrival_watermarkextender');"
																									CausesValidation="True" />
																								<cc2:CalendarExtender ID="calArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
																									Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calArrival" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="calArrival" ID="calArrival_watermarkextender"
																									ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox runat="server" ID="CalUTCArrival" CssClass="clsTextBoxTagSearch" Width="100px"
																									BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="false" CausesValidation="True"
																									onchange="ValidateDateText(this,'CalUTCArrival_watermarkextender');" />
																								<cc2:CalendarExtender ID="CalUTCArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
																									Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalUTCArrival" />
																								<cc2:TextBoxWatermarkExtender TargetControlID="CalUTCArrival" ID="CalUTCArrival_watermarkextender"
																									ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox ID="txtArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall" Width="70px"
																									MaxLength="10" ToolTip="Enter Arrival Time." />
																								<asp:TextBox ID="txtUTCArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall" Width="70px" TabIndex="14"
																									MaxLength="10" ToolTip="Enter UTC Arrival Time." />
																								<cc2:MaskedEditExtender ID="txtArrivalTimeMaskedEditExtender" TargetControlID="txtArrivalTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time" CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																								<cc2:MaskedEditExtender ID="txtUTCArrivalTimeMaskedEditExtender" TargetControlID="txtUTCArrivalTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time" CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																							</td>
																							<td>
																								<asp:Label ID="lblArrDayLightTime" runat="server" CssClass="clsLabelAuto" Visible="False">D/L Time</asp:Label>
																							</td>
																							<td>
																								<asp:DropDownList ID="cmbArrivalDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
																									DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.DesDayLightTime %>"
																									Visible="False">
																									<asp:ListItem Selected="True" Value="-12:00">-12:00</asp:ListItem>
																									<asp:ListItem Value="-11:45">-11:45</asp:ListItem>
																									<asp:ListItem Value="-11:30">-11:30</asp:ListItem>
																									<asp:ListItem Value="-11:15">-11:15</asp:ListItem>
																									<asp:ListItem Value="-11:00">-11:00</asp:ListItem>
																									<asp:ListItem Value="-10:45">-10:45</asp:ListItem>
																									<asp:ListItem Value="-10:30">-10:30</asp:ListItem>
																									<asp:ListItem Value="-10:15">-10:15</asp:ListItem>
																									<asp:ListItem Value="-10:00">-10:00</asp:ListItem>
																									<asp:ListItem Value="-09:45">-09:45</asp:ListItem>
																									<asp:ListItem Value="-09:30">-09:30</asp:ListItem>
																									<asp:ListItem Value="-09:15">-09:15</asp:ListItem>
																									<asp:ListItem Value="-09:00">-09:00</asp:ListItem>
																									<asp:ListItem Value="-08:45">-08:45</asp:ListItem>
																									<asp:ListItem Value="-08:30">-08:30</asp:ListItem>
																									<asp:ListItem Value="-08:15">-08:15</asp:ListItem>
																									<asp:ListItem Value="-08:00">-08:00</asp:ListItem>
																									<asp:ListItem Value="-07:45">-07:45</asp:ListItem>
																									<asp:ListItem Value="-07:30">-07:30</asp:ListItem>
																									<asp:ListItem Value="-07:15">-07:15</asp:ListItem>
																									<asp:ListItem Value="-07:00">-07:00</asp:ListItem>
																									<asp:ListItem Value="-06:45">-06:45</asp:ListItem>
																									<asp:ListItem Value="-06:30">-06:30</asp:ListItem>
																									<asp:ListItem Value="-06:15">-06:15</asp:ListItem>
																									<asp:ListItem Value="-06:00">-06:00</asp:ListItem>
																									<asp:ListItem Value="-05:45">-05:45</asp:ListItem>
																									<asp:ListItem Value="-05:30">-05:30</asp:ListItem>
																									<asp:ListItem Value="-05:15">-05:15</asp:ListItem>
																									<asp:ListItem Value="-05:00">-05:00</asp:ListItem>
																									<asp:ListItem Value="-04:45">-04:45</asp:ListItem>
																									<asp:ListItem Value="-04:30">-04:30</asp:ListItem>
																									<asp:ListItem Value="-04:15">-04:15</asp:ListItem>
																									<asp:ListItem Value="-04:00">-04:00</asp:ListItem>
																									<asp:ListItem Value="-03:45">-03:45</asp:ListItem>
																									<asp:ListItem Value="-03:30">-03:30</asp:ListItem>
																									<asp:ListItem Value="-03:15">-03:15</asp:ListItem>
																									<asp:ListItem Value="-03:00">-03:00</asp:ListItem>
																									<asp:ListItem Value="-02:45">-02:45</asp:ListItem>
																									<asp:ListItem Value="-02:30">-02:30</asp:ListItem>
																									<asp:ListItem Value="-02:15">-02:15</asp:ListItem>
																									<asp:ListItem Value="-02:00">-02:00</asp:ListItem>
																									<asp:ListItem Value="-01:45">-01:45</asp:ListItem>
																									<asp:ListItem Value="-01:30">-01:30</asp:ListItem>
																									<asp:ListItem Value="-01:15">-01:15</asp:ListItem>
																									<asp:ListItem Value="-01:00">-01:00</asp:ListItem>
																									<asp:ListItem Value="-00:45">-00:45</asp:ListItem>
																									<asp:ListItem Value="-00:30">-00:30</asp:ListItem>
																									<asp:ListItem Value="-00:15">-00:15</asp:ListItem>
																									<asp:ListItem Value="+00:00">+00:00</asp:ListItem>
																									<asp:ListItem Value="+00:15">+00:15</asp:ListItem>
																									<asp:ListItem Value="+00:30">+00:30</asp:ListItem>
																									<asp:ListItem Value="+00:45">+00:45</asp:ListItem>
																									<asp:ListItem Value="+01:00">+01:00</asp:ListItem>
																									<asp:ListItem Value="+01:15">+01:15</asp:ListItem>
																									<asp:ListItem Value="+01:30">+01:30</asp:ListItem>
																									<asp:ListItem Value="+01:45">+01:45</asp:ListItem>
																									<asp:ListItem Value="+02:00">+02:00</asp:ListItem>
																									<asp:ListItem Value="+02:15">+02:15</asp:ListItem>
																									<asp:ListItem Value="+02:30">+02:30</asp:ListItem>
																									<asp:ListItem Value="+02:45">+02:45</asp:ListItem>
																									<asp:ListItem Value="+03:00">+03:00</asp:ListItem>
																									<asp:ListItem Value="+03:15">+03:15</asp:ListItem>
																									<asp:ListItem Value="+03:30">+03:30</asp:ListItem>
																									<asp:ListItem Value="+03:45">+03:45</asp:ListItem>
																									<asp:ListItem Value="+04:00">+04:00</asp:ListItem>
																									<asp:ListItem Value="+04:15">+04:15</asp:ListItem>
																									<asp:ListItem Value="+04:30">+04:30</asp:ListItem>
																									<asp:ListItem Value="+04:45">+04:45</asp:ListItem>
																									<asp:ListItem Value="+05:00">+05:00</asp:ListItem>
																									<asp:ListItem Value="+05:15">+05:15</asp:ListItem>
																									<asp:ListItem Value="+05:30">+05:30</asp:ListItem>
																									<asp:ListItem Value="+05:45">+05:45</asp:ListItem>
																									<asp:ListItem Value="+06:00">+06:00</asp:ListItem>
																									<asp:ListItem Value="+06:15">+06:15</asp:ListItem>
																									<asp:ListItem Value="+06:30">+06:30</asp:ListItem>
																									<asp:ListItem Value="+06:45">+06:45</asp:ListItem>
																									<asp:ListItem Value="+07:00">+07:00</asp:ListItem>
																									<asp:ListItem Value="+07:15">+07:15</asp:ListItem>
																									<asp:ListItem Value="+07:30">+07:30</asp:ListItem>
																									<asp:ListItem Value="+07:45">+07:45</asp:ListItem>
																									<asp:ListItem Value="+08:00">+08:00</asp:ListItem>
																									<asp:ListItem Value="+08:15">+08:15</asp:ListItem>
																									<asp:ListItem Value="+08:30">+08:30</asp:ListItem>
																									<asp:ListItem Value="+08:45">+08:45</asp:ListItem>
																									<asp:ListItem Value="+09:00">+09:00</asp:ListItem>
																									<asp:ListItem Value="+09:15">+09:15</asp:ListItem>
																									<asp:ListItem Value="+09:30">+09:30</asp:ListItem>
																									<asp:ListItem Value="+09:45">+09:45</asp:ListItem>
																									<asp:ListItem Value="+10:00">+10:00</asp:ListItem>
																									<asp:ListItem Value="+10:15">+10:15</asp:ListItem>
																									<asp:ListItem Value="+10:30">+10:30</asp:ListItem>
																									<asp:ListItem Value="+10:45">+10:45</asp:ListItem>
																									<asp:ListItem Value="+11:00">+11:00</asp:ListItem>
																									<asp:ListItem Value="+11:15">+11:15</asp:ListItem>
																									<asp:ListItem Value="+11:30">+11:30</asp:ListItem>
																									<asp:ListItem Value="+11:45">+11:45</asp:ListItem>
																									<asp:ListItem Value="+12:00">+12:00</asp:ListItem>
																								</asp:DropDownList>
																							</td>
																						</tr>
																						<tr>
																							<td>&nbsp;
																							</td>
																							<td>
																								<asp:Label ID="lblTouchDownLocalDateTime" runat="server"
																									CssClass="clsLabelAuto" Text="Touch Down Date / Time" />
																								<asp:Label ID="lblUTCTouchDownDateTime" runat="server"
																									CssClass="clsLabelAuto" Text="UTC Touch Down Date / Time" />
																							</td>
																							<td colspan="3">
																								<asp:CheckBox ID="chkTouchDown" runat="server" AutoPostBack="True"
																									ToolTip="Check to enable Touch Down Date." />
																								<asp:TextBox runat="server" ID="calTouchDownLocalDateTime"
																									CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0"
																									ReadOnly="true" Width="100px" AutoPostBack="True" CausesValidation="True"
																									onchange="ValidateDateText(this,'calTouchDownLocalDateTime_watermarkextender');" />
																								<cc2:CalendarExtender ID="calTouchDownLocalDateTime_CalendarExtender" runat="server"
																									CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="calTouchDownLocalDateTime" />
																								<cc2:TextBoxWatermarkExtender
																									TargetControlID="calTouchDownLocalDateTime"
																									ID="calTouchDownLocalDateTime_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox runat="server" ID="calUTCTouchDownDateTime"
																									CssClass="clsTextBoxTagSearch"
																									BackColor="#E0E0E0" ReadOnly="true"
																									Width="100px" AutoPostBack="True"
																									CausesValidation="True"
																									onchange="ValidateDateText(this,'calUTCTouchDownDateTime_watermarkextender');" />
																								<cc2:CalendarExtender ID="calUTCTouchDownDateTime_CalendarExtender"
																									runat="server" CssClass="cal_Theme1" Enabled="false"
																									Format="<%$AppSettings:DateFormat%>"
																									TargetControlID="calUTCTouchDownDateTime" />
																								<cc2:TextBoxWatermarkExtender
																									TargetControlID="calUTCTouchDownDateTime"
																									ID="calUTCTouchDownDateTime_watermarkextender"
																									ClientIDMode="Static" runat="server"
																									WatermarkText="<%$AppSettings:DateFormat%>"
																									WatermarkCssClass="clsDateTextBox" />
																								<asp:TextBox ID="txtTouchDownLocalTime" runat="server" Width="70px"
																									AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																									MaxLength="10" ToolTip="Enter Touch Down Time." />
																								<asp:TextBox ID="txtUTCTouchDownTime" runat="server"
																									AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																									Width="70px" TabIndex="13" MaxLength="10"
																									ToolTip="Enter UTC Touch Down Time." />
																								<cc2:MaskedEditExtender
																									ID="txtTouchDownLocalTimeMaskedEditExtender"
																									TargetControlID="txtTouchDownLocalTime"
																									AutoComplete="true" Mask="99:99"
																									MaskType="Time" CultureName="en-us"
																									MessageValidatorTip="true"
																									runat="server" />
																								<cc2:MaskedEditExtender
																									ID="txtUTCTouchDownTimeMaskedEditExtender"
																									TargetControlID="txtUTCTouchDownTime"
																									AutoComplete="true" Mask="99:99" MaskType="Time"
																									CultureName="en-us" MessageValidatorTip="true"
																									runat="server" />
																							</td>
																						</tr>
																					</table>
																				</fieldset>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</asp:PlaceHolder>
												<tr>
													<td>
														<asp:UpdatePanel ID="upnlFlightSummary" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle">
																	<legend id="lblLegAir" runat="server" style="font-weight: bold">Aircraft Flying Hours as per Flight Log book or HOBBS
																	</legend>
																	<table width="100%">
																		<tr>
																			<td>
																				<table width="100%">
																					<tr>
																						<td>
																							<asp:Panel ID="pnlHours" runat="server" CssClass="clsPanel1" Visible="False">
																								<table width="100%">
																									<tr>
																										<td>
																											<asp:Label ID="lblairfly" runat="server"
																												CssClass="clsLabelAuto" Text="Block Time" />
																											<asp:TextBox ID="txtBlockTime" runat="server"
																												BackColor="Gainsboro"
																												CssClass="clsTextBoxTagSearchSmall" Width="70px"
																												Enabled='<%#IIf(AppSettings("SetBlockTime") = "True", True, False) %>'
																												Text="<%# mLog.DiffTime %>" Visible="False" />
																										</td>
																										<td>
																											<asp:Label ID="lblAirBorneTime" runat="server"
																												CssClass="clsLabelAuto" Text="Airborne Time" />
																											<asp:TextBox ID="txtAirBorneTime" runat="server"
																												CssClass="clsTextBoxTagSearchSmall" Width="70px" TabIndex="15"
																												AutoPostBack="true"
																												ReadOnly="<%# mLog.ShowTimeTextBoxes Or Not mLog.IsNew %>"
																												Text="<%# mLog.TimeInAir %>" Visible="False"
																												onfocus="onTextFocus();" />
																										</td>
																										<td>
																											<asp:Label ID="lblGroundRunTime" runat="server"
																												CssClass="clsLabelAuto" Text="Ground Run Time" />
																											<asp:TextBox ID="txtGroundRunTime" runat="server"
																												CssClass="clsTextBoxTagSearchSmall"
																												Width="70px" TabIndex="16"
																												ReadOnly="<%# mLog.ShowTimeOnGround Or Not mLog.IsNew %>"
																												Text="<%# mLog.TimeOnGround %>"
																												Enabled='<%# iif(AppSettings("SetBlockTime") = "True", False, True) %>'
																												Visible="False" onfocus="onTextFocus();" AutoPostBack="True" />
																										</td>
																										<td>
																											<asp:Label ID="lblPercentTimeOnGround" runat="server"
																												CssClass="clsLabelAuto" Text="% Ground Run Time" />
																											<asp:TextBox ID="txtPercentTimeOnGround" runat="server"
																												CssClass="clsTextBoxTagSearchSmall"
																												Width="70px" TabIndex="17"
																												ReadOnly="<%# Not mLog.IsNew %>"
																												Text="<%# mLog.PercentTimeOnGround %>" Visible="False"
																												AutoPostBack="True" onfocus="onTextFocus();" />
																										</td>
																									</tr>
																								</table>
																							</asp:Panel>
																						</td>
																					</tr>
																					<asp:PlaceHolder ID="plDecimal" runat="server" Visible="False">
																						<tr>
																							<td>
																								<asp:Panel ID="pnlDecimal" runat="server"
																									CssClass="clsPanel1" Visible="False">
																									<table width="100%">
																										<tr>
																											<td>
																												<table width="100%">
																													<tr>
																														<td>
																															<asp:Label ID="lblHobbsread" runat="server"
																																CssClass="clsLabelAuto"
																																Text="OBBS READING :" />
																														</td>
																														<td>
																															<asp:Label ID="Label1" runat="server"
																																CssClass="clsLabelAuto"
																																Text="Previous Value :" />
																														</td>
																														<td>
																															<asp:Label ID="lblHobbsPrevVal" runat="server"
																																CssClass="clsLabelauto" Text="Offset" />
																														</td>
																														<td>
																															<asp:TextBox ID="txtPrevHobbsOffset" runat="server"
																																BackColor="#E0E0E0"
																																CssClass="clsTextBoxTagSearchSmall"
																																Width="70px" ReadOnly="True"
																																Text="<%# mLog.PrevHobbsOffsetValue %>"
																																Visible="False" />
																														</td>
																														<td>
																															<asp:Label ID="lblHobbsCurrentReading" runat="server"
																																CssClass="clsLabelauto" Text="Reading" />
																														</td>
																														<td>
																															<asp:TextBox ID="txtPrevHobbsValue" runat="server"
																																BackColor="#E0E0E0"
																																CssClass="clsTextBoxTagSearchSmall"
																																Width="70px" ReadOnly="True"
																																Text="<%# mLog.PrevHobbsValue %>"
																																Visible="False" />
																														</td>
																													</tr>
																												</table>
																											</td>
																											<td>
																												<table width="100%">
																													<tr>
																														<td>
																															<asp:Label ID="Label2" runat="server"
																																CssClass="clsLabelAuto"
																																Text="Current Value :" />
																														</td>
																														<td>
																															<asp:Label ID="lblOffsetPreVal" runat="server"
																																CssClass="clsLabelauto" Text="Offset" />
																														</td>
																														<td>
																															<asp:TextBox ID="txtCurrentHobbsOffset"
																																runat="server" BackColor="#E0E0E0"
																																CssClass="clsTextBoxTagSearchSmall"
																																Width="70px" ReadOnly="True"
																																Text="<%# mLog.CurrentHobbsOffsetValue %>"
																																Visible="False" />
																														</td>
																														<td>
																															<asp:Label ID="lblOffsetCurrentVal"
																																runat="server"
																																CssClass="clsLabelauto" Text="Reading" />
																														</td>
																														<td>
																															<asp:TextBox ID="txtCurrentHobbsValue"
																																runat="server"
																																CssClass="clsTextBoxTagSearchSmall"
																																Width="70px" AutoPostBack="true"
																																Text="<%# mLog.CurrentHobbsValue %>"
																																Visible="False" />
																														</td>
																													</tr>
																												</table>
																											</td>
																										</tr>
																									</table>
																								</asp:Panel>
																							</td>
																						</tr>
																					</asp:PlaceHolder>
																				</table>
																			</td>
																			<td rowspan="2" align="right">
																				<table width="100%">
																					<tr>
																						<td>
																							<asp:Label ID="lblTotalTime" runat="server"
																								CssClass="clsLabelAuto" Text="Total Time" />
																							<asp:TextBox ID="txtTotalTime" runat="server"
																								CssClass="clsTextBoxTagSearchSmall"
																								Width="70px" Text="<%# mLog.TotalTime %>"
																								ReadOnly="True" BackColor="#E0E0E0" Enabled="False" />
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																	</table>
																</fieldset>
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
														<asp:UpdatePanel ID="upnlAirframeDetail" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle" runat="server" id="fldAirframePeriods">
																	<legend id="lgAirframePeriods" runat="server">
																		<table width="100%">
																			<tr>
																				<td>
																					<asp:Label ID="lblAirframePeriod" runat="server"
																						Font-Bold="true"
																						CssClass="clsLabelHeader" Text="Airframe Periods" />
																				</td>
																				<td align="right">
																					<asp:LinkButton ID="lnkAllAssembly" runat="server"
																						CssClass="clsLinkButton" Font-Italic="True"
																						Font-Size="9pt" ToolTip="View all Assemblies."
																						ClientIDMode="Static"
																						Visible="<%# (mLog.IsShowAssemblyRequired) %>"
																						Text="View all Assemblies" />
																				</td>
																			</tr>
																		</table>
																	</legend>
																	<asp:GridView ID="gvAFPeriods" runat="server"
																		AutoGenerateColumns="False" Width="100%"
																		CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																		CssClass="clsGridNewStyle"
																		AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false"
																		HeaderStyle-Wrap="false"
																		SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True"
																		PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
																		<RowStyle CssClass="clsdgItem" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																			Font-Bold="True" ForeColor="black" />
																		<SelectedRowStyle BackColor="ControlDark" />
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<Columns>
																			<%--0--%>
																			<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																			<%--1--%>
																			<asp:BoundField DataField="ModelName" HeaderText="Model">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="150px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																			</asp:BoundField>
																			<%--2--%>
																			<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="100px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
																			</asp:BoundField>
																			<%--3--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameHours" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="18"
																						ReadOnly="<%# Not mLog.IsNew %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																						ToolTip="Enter the Hours." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--4--%>
																			<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--5--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameLandings" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>'
																						ToolTip="Enter the Landing." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeypress="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" TabIndex="19">  
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--6--%>
																			<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--7--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="20"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>'
																						ToolTip="Enter Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeypress="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">  
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--8--%>
																			<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--9--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameStarts" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="21" Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>'
																						ToolTip="Enter Start Time." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">  
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%-- 10--%>
																			<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--11--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameNGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="22"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>'
																						ToolTip="Enter NG Cycles" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--12--%>
																			<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--13--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameNFCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="23"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>'
																						ToolTip="Enter NF Cycles" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--14--%>
																			<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--15--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameRins" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="24"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>'
																						ToolTip="Enter RINS" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--16--%>
																			<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--17--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameBleeds" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="25"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>'
																						ToolTip="Enter Bleeds" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--18--%>
																			<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--19--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameImpellerCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="26"
																						Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																						ToolTip="Enter Impeller Cycles" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--20--%>
																			<asp:BoundField DataField="FinalImpellerCycles"
																				HeaderText="Final Impeller Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--21--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFrameCTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="27"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>'
																						ToolTip="Enter CT Cycles" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--22--%>
																			<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false"
																					 />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--23--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirFramePTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="28"
																						Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>'
																						ToolTip="Enter PT Cycles" AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--24--%>
																			<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--25--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeGeneratorMods" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																						ToolTip="Enter the Generator Mods." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--26--%>
																			<asp:BoundField DataField="FinalGeneratorMods"
																				HeaderText="Final Generator Mods">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--27--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NR">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeNRCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NRCycles") %>'
																						ToolTip="Enter the NR Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--28--%>
																			<asp:BoundField DataField="FinalNRCycles" HeaderText="Final NR">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--29--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="LCY">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeLandingCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "LandingCycles") %>'
																						ToolTip="Enter the Landing Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--30--%>
																			<asp:BoundField DataField="FinalLandingCycles" HeaderText="Final LCY">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--31--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="LGCY">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeLandingGearCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "LandingGearCycles") %>'
																						ToolTip="Enter the Landing Gear Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--32--%>
																			<asp:BoundField DataField="FinalLandingGearCycles" HeaderText="Final LGCY">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--33--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="OLHC">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeOverSpeedLHMLGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "OverSpeedLHMLGCycles") %>'
																						ToolTip="Enter the Over Speed LH MLG Cycles."
																						AutoPostBack="true" OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--34--%>
																			<asp:BoundField DataField="FinalOverSpeedLHMLGCycles" HeaderText="Final OLHC">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--35--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="ORHC">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeOverSpeedRHMLGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "OverSpeedRHMLGCycles") %>'
																						ToolTip="Enter the Over Speed RH MLG Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--36--%>
																			<asp:BoundField DataField="FinalOverSpeedRHMLGCycles" HeaderText="Final ORHC">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--37--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="ONCY">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeOverSpeedNLGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "OverSpeedRHMLGCycles") %>'
																						ToolTip="Enter the Over Speed NLG Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--38--%>
																			<asp:BoundField DataField="FinalOverSpeedNLGCycles" HeaderText="Final ONCY">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--39--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="TCY">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeMGBTorqueCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "OverSpeedRHMLGCycles") %>'
																						ToolTip="Enter the MGB Torque Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--40--%>
																			<asp:BoundField DataField="FinalMGBTorqueCycles" HeaderText="Final TCY">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--41--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RBCY">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAirframeRotorBrakeCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RotorBrakeCycles") %>'
																						ToolTip="Enter the Rotor Brake Cycles." AutoPostBack="true"
																						OnTextChanged="AirFrameTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--42--%>
																			<asp:BoundField DataField="FinalRotorBrakeCycles" HeaderText="Final RBCY">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--43--%>
																			<asp:BoundField HeaderText="" />
																		</Columns>
																		<SelectedRowStyle BackColor="ControlDark" />
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																	</asp:GridView>
																</fieldset>
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
														<asp:UpdatePanel ID="upnlEngineDetail" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle" runat="server" id="fldEnginePeriods">
																	<legend id="lgEnginePeriods" runat="server">
																		<div style="width: 100%">
																			<asp:Label ID="lblEnginePeriod" runat="server"
																				Font-Bold="true"
																				CssClass="clsLabelHeader" Text="Engine Periods" />
																		</div>
																	</legend>
																	<asp:GridView ID="gvEnginePeriods" runat="server"
																		AutoGenerateColumns="False" Width="100%"
																		CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																		CssClass="clsGridNewStyle"
																		AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false"
																		HeaderStyle-Wrap="false"
																		SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True"
																		PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
																		<RowStyle CssClass="clsdgItem" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																			Font-Bold="True" ForeColor="black" />
																		<SelectedRowStyle BackColor="ControlDark" />
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<Columns>
																			<%--0--%>
																			<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																			<%--1--%>
																			<asp:BoundField DataField="ModelName" HeaderText="Model">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="150px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																			</asp:BoundField>
																			<%--2--%>
																			<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="100px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
																			</asp:BoundField>
																			<%--3--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineHours" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="30" ReadOnly="<%# Not mLog.IsNew %>"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																						ToolTip="Enter the Hours." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--4--%>
																			<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--5--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineLandings" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="31"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>'
																						ToolTip="Enter the Landing." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--6--%>
																			<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--7--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="32"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>'
																						ToolTip="Enter Cycles." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">    
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--8--%>
																			<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--9--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineStarts" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="33"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>'
																						ToolTip="Enter Start Time." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--10--%>
																			<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--11--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineNGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="34"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>'
																						ToolTip="Enter NG Cycles" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--12--%>
																			<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--13--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineNFCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="35"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>'
																						ToolTip="Enter NF Cycles" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--14--%>
																			<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--15--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineRins" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="36"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>'
																						ToolTip="Enter RINS" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--16--%>
																			<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--17--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Contingency Factor">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineCFactors" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="37" Width="97%"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CFactor") %>'
																						ToolTip="Enter Contingency Factor." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--18--%>
																			<asp:BoundField DataField="FinalCFactor" HeaderText="Final Contingency Factor">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--19--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineBleeds" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="38"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>'
																						ToolTip="Enter Bleeds" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--20--%>
																			<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--21--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineImpellerCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="39"
																						Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																						ToolTip="Enter Impeller Cycles" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--22--%>
																			<asp:BoundField DataField="FinalImpellerCycles"
																				HeaderText="Final Impeller Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--23--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineCTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="40"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>'
																						ToolTip="Enter CT Cycles" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--24--%>
																			<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--25--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEnginePTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="41"
																						Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>'
																						ToolTip="Enter PT Cycles" AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();"> 
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--26--%>
																			<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--27--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineGeneratorMods" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="42"
																						Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																						ToolTip="Enter the Generator Mods." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--28--%>
																			<asp:BoundField DataField="FinalGeneratorMods"
																				HeaderText="Final Generator Mods">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--29--%>
																			<asp:TemplateField HeaderText="Rapid Take Off">
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineRapidTakeOffFactor" runat="server"
																						ToolTip="Enter Rapid Take Off." TabIndex="43"
																						CssClass="clsTextBoxTagPeriodValues"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RapidTakeOffFactor") %>'
																						AutoPostBack="true" Width="97%"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																			</asp:TemplateField>
																			<%--30--%>
																			<asp:BoundField DataField="FinalRapidTakeOffFactor"
																				HeaderText="Final Rapid Take Off">
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:BoundField>
																			<%--31--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="N1 Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineN1Cycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "N1Cycles") %>'
																						ToolTip="Enter the NR Cycles." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--32--%>
																			<asp:BoundField DataField="FinalN1Cycles" HeaderText="Final N1 Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																			<%--33--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="N2 Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtEngineN2Cycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="29"
																						Text='<%# DataBinder.Eval(Container.DataItem, "N1Cycles") %>'
																						ToolTip="Enter the N2 Cycles." AutoPostBack="true"
																						OnTextChanged="EngineTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">   
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right"  />
																				<ItemStyle HorizontalAlign="Right"  />
																			</asp:TemplateField>
																			<%--34--%>
																			<asp:BoundField DataField="FinalN2Cycles"
																				HeaderText="Final N2 Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false"  />
																				<ItemStyle HorizontalAlign="Right" Wrap="false"  />
																			</asp:BoundField>
																		</Columns>
																	</asp:GridView>
																</fieldset>
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
														<asp:UpdatePanel ID="upnlAPUDetail" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle" runat="server" id="fldAPUPeriods">
																	<legend id="lgAPUPeriods" runat="server">
																		<div style="width: 100%">
																			<asp:Label ID="lblAPUPeriod" runat="server" Font-Bold="true"
																				CssClass="clsLabelHeader" Text="APU Periods" />
																		</div>
																	</legend>
																	<asp:GridView ID="gvAPUPeriods" runat="server"
																		AutoGenerateColumns="False" Width="100%"
																		CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																		CssClass="clsGridNewStyle"
																		AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false"
																		HeaderStyle-Wrap="false"
																		SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True"
																		PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
																		<RowStyle CssClass="clsdgItem" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																			Font-Bold="True" ForeColor="black" />
																		<SelectedRowStyle BackColor="ControlDark" />
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<Columns>
																			<%--0--%>
																			<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																			<%--1--%>
																			<asp:BoundField DataField="ModelName" HeaderText="Model">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="150px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																			</asp:BoundField>
																			<%--2--%>
																			<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="100px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
																			</asp:BoundField>
																			<%--3--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUHours" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="44"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																						ToolTip="Enter the Hours." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--4--%>
																			<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--5--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPULandings" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="45"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>'
																						ToolTip="Enter the Landing." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--6--%>
																			<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--7--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="46"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>'
																						ToolTip="Enter Cycles." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--8--%>
																			<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--9--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUStarts" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="47"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>'
																						ToolTip="Enter Start Time." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--10--%>
																			<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--11--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUNGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="48"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>'
																						ToolTip="Enter NG Cycles"
																						AutoPostBack="true" OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--12--%>
																			<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--13--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUNFCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="49"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>'
																						ToolTip="Enter NF Cycles" AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--14--%>
																			<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--15--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPURins" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>' TabIndex="50"
																						ToolTip="Enter RINS." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--16--%>
																			<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--17--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUBleeds" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="51"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>'
																						ToolTip="Enter Bleeds"
																						AutoPostBack="true" OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--18--%>
																			<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--19--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUImpellerCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="52"
																						Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																						ToolTip="Enter Impeller Cycles" AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--20--%>
																			<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--21--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUCTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="53"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>'
																						ToolTip="Enter CT Cycles" AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--22--%>
																			<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--23--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUPTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="54"
																						Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>'
																						ToolTip="Enter PT Cycles" AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--24--%>
																			<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--25--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
																				<ItemTemplate>
																					<asp:TextBox ID="txtAPUGeneratorMods" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="55"
																						Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																						ToolTip="Enter the Generator Mods." AutoPostBack="true"
																						OnTextChanged="APUTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();">
																					</asp:TextBox>
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--26--%>
																			<asp:BoundField DataField="FinalGeneratorMods"
																				HeaderText="Final Generator Mods">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--27--%>
																			<asp:BoundField HeaderText="" />
																		</Columns>
																	</asp:GridView>
																</fieldset>
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
														<asp:UpdatePanel ID="upnlCGBDetail" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset class="clsFieldSetNewStyle" runat="server" id="fldCGBPeriods">
																	<legend id="lgCGBPeriods" runat="server">
																		<div style="width: 100%">
																			<asp:Label ID="lblCGBPeriod" runat="server" Font-Bold="true"
																				CssClass="clsLabelHeader" Text="CGB Periods" />
																		</div>
																	</legend>
																	<asp:GridView ID="gvCGBPeriods" runat="server"
																		AutoGenerateColumns="False" Width="100%"
																		CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																		CssClass="clsGridNewStyle"
																		AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false"
																		HeaderStyle-Wrap="false"
																		SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True"
																		PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
																		<RowStyle CssClass="clsdgItem" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																			Font-Bold="True" ForeColor="black" />
																		<SelectedRowStyle BackColor="ControlDark" />
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<Columns>
																			<%--0--%>
																			<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
																			<%--1--%>
																			<asp:BoundField DataField="ModelName" HeaderText="Model">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="150px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
																			</asp:BoundField>
																			<%--2--%>
																			<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
																					Wrap="false" Width="100px" />
																				<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
																			</asp:BoundField>
																			<%--3--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBHours" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="56"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																						ToolTip="Enter the Hours." AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--4--%>
																			<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--5--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="Landings">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBLandings" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="57"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>'
																						ToolTip="Enter the Landing." AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--6--%>
																			<asp:BoundField DataField="FinalLandings"
																				HeaderText="Final Landings">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--7--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="58"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>'
																						ToolTip="Enter Cycles." AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--8--%>
																			<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--9--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="Starts">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBStarts" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="59"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>'
																						ToolTip="Enter Start Time." AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--10--%>
																			<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--11--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBNGCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="60"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>'
																						ToolTip="Enter NG Cycles" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--12--%>
																			<asp:BoundField DataField="FinalNGCycles"
																				HeaderText="Final NG Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--13--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="NF Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBNFCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="61"
																						Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>'
																						ToolTip="Enter NF Cycles" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--14--%>
																			<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--15--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBRINS" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>' TabIndex="62"
																						ToolTip="Enter RINS" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--16--%>
																			<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--17--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="Bleeds">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBBleeds" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="63"
																						Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>'
																						ToolTip="Enter Bleeds" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--18--%>
																			<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--19--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true"
																				HeaderText="Impeller Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBImpellerCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues"
																						TabIndex="64"
																						Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																						ToolTip="Enter Impeller Cycles" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--20--%>
																			<asp:BoundField DataField="FinalImpellerCycles"
																				HeaderText="Final Impeller Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--21--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBCTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="65"
																						Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>'
																						ToolTip="Enter CT Cycles" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--22--%>
																			<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--23--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBPTCycles" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="66"
																						Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>'
																						ToolTip="Enter PT Cycles" AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--24--%>
																			<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--25--%>
																			<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
																				<ItemTemplate>
																					<asp:TextBox ID="txtCGBGeneratorMods" runat="server"
																						CssClass="clsTextBoxTagPeriodValues" TabIndex="67"
																						Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																						ToolTip="Enter the Generator Mods." AutoPostBack="true"
																						OnTextChanged="CGBTextChanged"
																						onkeydown="onkeyPressed(window.event.keyCode,this);"
																						onfocus="onTextFocus();" />
																				</ItemTemplate>
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:TemplateField>
																			<%--26--%>
																			<asp:BoundField DataField="FinalGeneratorMods"
																				HeaderText="Final Generator Mods">
																				<HeaderStyle Font-Bold="true" HorizontalAlign="Right"
																					Wrap="false" />
																				<ItemStyle HorizontalAlign="Right" Wrap="false" />
																			</asp:BoundField>
																			<%--27--%>
																			<asp:BoundField HeaderText="" />
																		</Columns>
																	</asp:GridView>
																</fieldset>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td>
														<table width="100%">
															<tr>
																<td style="width: 50%" valign="top">
																	<asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
																			<br />
																			<asp:TextBox ID="txtRemark" runat="server"
																				CssClass="clsTextBoxTagSearchMultilineNewStyleLong" TabIndex="68"
																				MaxLength="500" Text="<%# mLog.Remark %>"
																				TextMode="MultiLine" ToolTip="Enter Remark"
																				onfocus="onTextFocus();" />
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td style="width: 50%">
																	<asp:PlaceHolder ID="attachMultiple" runat="server" Visible='<%#IIf(AppSettings("ClientCode") = "Heligo" Or
                                                                                                                                    AppSettings("ClientCode") = "UHPL" Or
                                                                                                                                    AppSettings("ClientCode") = "APFT" Or
                                                                                                                                    AppSettings("ClientCode") = "AAP", True, False) %>'>
																		<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																			<legend><b>File Attachments</b></legend>
																			<asp:UpdatePanel ID="upnlLogAttachment" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table width="100%">
																						<tr>
																							<td style="height: 15px">
																								<asp:UpdatePanel ID="upnldgLogAttachment" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<asp:GridView ID="dgLogAttachment" ToolTip="List of File Attachment(s)" runat="server"
																											DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
																											AllowPaging="False" AutoGenerateColumns="false" CellPadding="5"
																											ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle">
																											<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
																											<RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
																											<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																												Font-Bold="True" ForeColor="black" />
																											<Columns>
																												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																												<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID" />
																												<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																													<HeaderStyle HorizontalAlign="Left" Width="10px" />
																												</asp:BoundField>
																												<asp:BoundField Visible="False" DataField="FileName"
																													SortExpression="FileName" HeaderText="File Name">
																													<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																												</asp:BoundField>
																												<asp:TemplateField HeaderText="File Name">
																													<HeaderStyle Width="200px" HorizontalAlign="Left" />
																													<ItemTemplate>
																														<asp:TextBox ID="txtFileName" runat="server"
																															CssClass="clsTextBox3_Ajax" MaxLength="100"
																															ClientIDMode="Static"
																															ToolTip="Enter File Name To Be Attached"
																															Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
																															Width="350px" DESIGNTIMEDRAGDROP="767" />
																													</ItemTemplate>
																												</asp:TemplateField>
																												<asp:TemplateField ItemStyle-HorizontalAlign="Center"
																													HeaderText="View" HeaderStyle-HorizontalAlign="Center">
																													<ItemTemplate>
																														<asp:ImageButton ID="View" runat="server"
																															CommandArgument='<%# Eval("SrNo") %>'
																															CommandName="View"
																															Style="height: 20px; width: 13px"
																															ImageUrl="icons/CLIP01.ICO" />
																													</ItemTemplate>
																													<HeaderStyle HorizontalAlign="Center" />
																													<ItemStyle HorizontalAlign="Center" />
																												</asp:TemplateField>
																												<asp:TemplateField ItemStyle-HorizontalAlign="Center"
																													HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
																													<ItemTemplate>
																														<asp:ImageButton ID="Remove" runat="server"
																															CommandArgument='<%# Eval("SrNo") %>'
																															CommandName="Remove"
																															Style="height: 20px; width: 20px"
																															ImageUrl="~/images/delete.png"
																															CausesValidation="false" />
																													</ItemTemplate>
																													<HeaderStyle HorizontalAlign="Center" />
																													<ItemStyle HorizontalAlign="Center" />
																												</asp:TemplateField>
																											</Columns>
																										</asp:GridView>
																									</ContentTemplate>
																								</asp:UpdatePanel>
																							</td>
																							<td valign="top">
																								<asp:ImageButton ID="btnSelectFiles" runat="server"
																									ImageUrl="~/images/plus1.png"
																									Height="22px" Width="24px"
																									ToolTip="Add New Attachment"
																									CausesValidation="false" />
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</fieldset>

																	</asp:PlaceHolder>
																</td>
															</tr>
														</table>

													</td>
												</tr>
												<tr style="height: 0px;">
													<td>
														<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
															<ContentTemplate>
																<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static"
																	runat="server" Text="----"
																	CausesValidation="False" Style="display: none;" />
																<asp:Button ID="hdnimgBtnFlightLogClassification"
																	ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;" />
																<asp:Button ID="hdnimgBtnPlace" ClientIDMode="Static"
																	runat="server" Text="----"
																	CausesValidation="False" Style="display: none;" />
																<asp:Button ID="hdnBtnDiscrepancyTroubleShoot1"
																	runat="server" CausesValidation="False" ClientIDMode="Static"
																	Style="display: none;" />
																<asp:Button ID="hdnBtnDiscrepancyDetail"
																	runat="server" CausesValidation="False" 
																	ClientIDMode="Static" Style="display: none;" />
																<asp:Button ID="hdnBtnCabinfDefectDetail"
																	runat="server" CausesValidation="False" 
																	ClientIDMode="Static" Style="display: none;"  />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</ContentTemplate>

									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlFuelOil" runat="server"
										Visible="<%# Not mLog.IsNew %>"
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Fuel Oil" ID="Label5" />
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlFuelOil" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
												<ContentTemplate>
													<iframe id="IframeFuelOil" width="100%" height="200px"
														scrolling="no" marginheight="0"
														frameborder="0" onload="autoResizeFuelOil()"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlSnagReporting" runat="server"
										Visible='<%# Not mLog.IsNew And AppSettings("ShowNewDiscrepancyFlow") = "False" %>'
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server"
												Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Reporting", "Snag Reporting") %>' ID="Label6" />
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlSnagReporting" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeSnagReporting" width="100%" height="200px"
														scrolling="no" marginheight="0"
														frameborder="0" onload="autoResizeSnagReporting()"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlParameterList" runat="server" Visible="<%# Not mLog.IsNew %>"
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Parameter List" ID="Label8" />
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlParameterList" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeParameterList" width="100%" height="200px"
														scrolling="no" marginheight="0"
														frameborder="0" onload="autoResizeParameterList()"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlFlightCrewList" runat="server" Visible="<%# 
                                        Not mLog.IsNew %>"
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Flight Crew" ID="Label9" />
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlFlightCrew" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeFlightCrewList" scrolling="no" marginheight="0"
														frameborder="0" onload="autoResizeFlightCrewList()" width="100%"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlMaintActivity" runat="server" Visible="<%# 
                                        Not mLog.IsNew %>"
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Maintenance Activity" ID="Label7" />
										</HeaderTemplate>
										<ContentTemplate>
											<asp:UpdatePanel ID="upnlMaintActivity" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeMaintActivity" scrolling="no" marginheight="0"
														frameborder="0" onload="autoResizeMaintActivity()"
														width="100%"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>
									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlDiscrepancyReporting" runat="server"
										Visible='<%# Not mLog.IsNew And AppSettings("ShowNewDiscrepancyFlow") = "True" %>'
										CssClass="clsPanel1" ClientIDMode="Static">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Discrepancy Reporting" 
												ID="lblHeaderDiscrepancyReporting" />
										</HeaderTemplate>

										<ContentTemplate>
											<asp:UpdatePanel ID="upnlDiscrepancyReporting" runat="server"
												UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeDiscrepancyReporting"
														width="100%" frameborder="0"
														scrolling="no" marginheight="0"
														onload="autoResizeDiscrepancyReporting()"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>

									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlDeferredDiscrepancies" runat="server" 
										ClientIDMode="Static"
										Visible='<%# AppSettings("ShowNewDiscrepancyFlow") = "True"  %>' 
										CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Deferred Discrepancies" 
												ID="lblHeaderDeferredDiscrepancies" />
										</HeaderTemplate>

										<ContentTemplate>
											<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeDeferredDiscrepancy" 
														marginheight="0" frameborder="0" 
														onload="autoResizeDeferredDiscrepancy();"
														width="100%" height="100%"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>

									</cc2:TabPanel>

									<cc2:TabPanel ID="tbpnlCabinDefect" runat="server"
										Visible="false"
										ClientIDMode="Static" CssClass="clsPanel1">
										<HeaderTemplate>
											<asp:Label runat="server" Text="Cabin Defect" ID="lblHeaderCabinDefect" />
										</HeaderTemplate>

										<ContentTemplate>
											<asp:UpdatePanel ID="upnlCabinDefect" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<iframe id="IframeCabinDefect"
														width="100%" frameborder="0"
														scrolling="no" marginheight="0"
														onload="autoResizeCabinDefectList();"></iframe>
												</ContentTemplate>
											</asp:UpdatePanel>
										</ContentTemplate>

									</cc2:TabPanel>

								</cc2:TabContainer>
							</td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>

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

		</div>

		<div id="InfoMessagepanel" class="clsInfoMessage1" style="display: none; z-index: 100"
			draggable="true">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlLogInfo">
				<ContentTemplate>
					<table class="zui-table zui-table-rounded" style="z-index: 100" draggable="true">
						<thead style="z-index: 100">
							<tr>
								<td colspan="4">
									<span><b>List of Logs on selected date : </b></span><span><b>
										<%= mLogListOnDate.Count%></b></span> <span><b>Record(s)</b></span>
								</td>
							</tr>
						</thead>
						<thead style="z-index: 100">
							<tr>
								<th>
									<span>Log No. &</span>
									<br />
									<span>Log Page No.</span>
								</th>
								<th>
									<span>Departure Info</span>
								</th>
								<th>
									<span>Arrival Info</span>
								</th>
								<th>
									<span>Airborne Time</span>
								</th>
							</tr>
						</thead>
						<tbody style="z-index: 100">
							<% Dim Child3 As LogInfo%>
							<% For Each Child3 In mLogListOnDate%>
							<tr>
								<td>
									<span>
										<%= Child3.LogTextNo %></span>
									<br />
									<span>
										<%= Child3.LogPageNoFormatted %></span>
								</td>
								<td>
									<% If mMachine.IsUTC Then%>
									<span>
										<%= Child3.SouUniverseDateTimeFormatted%></span>
									<% Else%>
									<span>
										<%= Child3.SouLocalDateTimeFormatted%></span>
									<%End If%>
									<% If Child3.LogTypeID = 1 Then%>
									<span>
										<br />
										<%= Child3.SouPlaceName %></span>
									<%End If%>
								</td>
								<td>
									<% If mMachine.IsUTC Then%>
									<span>
										<%= Child3.DesUniverseDateTimeFormatted%></span>
									<% Else%>
									<span>
										<%= Child3.DesLocalDateTimeFormatted%></span>
									<%End If%>
									<% If Child3.LogTypeID = 1 Then%>
									<span>
										<br />
										<%= Child3.DesPlaceName %></span>
									<%End If%>
								</td>
								<td>
									<span>
										<%= Child3.TimeInAir %></span>
								</td>
							</tr>
							<% Next%>
						</tbody>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>

		<div id="modal-blur-overlay"
			style="display: none; position: fixed; top: 0; left: 0; width: 100%; 
				   height: 100%; backdrop-filter: blur(5px); 
				   background-color: rgba(0, 0, 0, 0.5); z-index: 999;">

			<div id="pnlAllAssemblypanel" class="clsInfoMessage1" style="display: none; z-index: 100;"
				draggable="true">
				<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAllAssemblypanel">
					<ContentTemplate>
						<div style="width: 100%">
							<table style="width: 100%">
								<tr>
									<td align="right" width="55%">
										<asp:Label ID="lblAllAssemblypanelHeader" runat="server"
											CssClass="clsLabelHeader" Text="ALL Assemblies" Font-Size="Large" />
									</td>
									<td align="right" width="45%">
										<span>
											<a class="close-btn1"
												style="font-size: large; color: Black" href="#"
												onclick="CloseAllAssembliesModal();return false;">X
											</a>
										</span>
									</td>
								</tr>
							</table>
						</div>
						<div style="overflow: scroll">
							<br />
							<asp:GridView ID="gvALLAssemblies" runat="server" AutoGenerateColumns="False" Width="100%"
								CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle"
								AlternatingRowStyle-CssClass="alt"
								RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
								ShowHeaderWhenEmpty="True" PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
								<RowStyle CssClass="clsdgItem" />
								<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
								<HeaderStyle BackColor="white" CssClass="clsdgHeader"
									Font-Bold="True" ForeColor="black" />
								<SelectedRowStyle BackColor="ControlDark" />
								<AlternatingRowStyle CssClass="clsdgAltItem" />
								<Columns>
									<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
									<%--0--%>
									<asp:BoundField DataField="ModelName" HeaderText="Model">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" Width="150px" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
									</asp:BoundField>
									<%--1--%>
									<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" Width="100px" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
									</asp:BoundField>
									<%--2--%>
									<asp:BoundField DataField="AssemblyTypeCode" HeaderText="Type">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--3--%>
									<asp:BoundField DataField="Hours" HeaderText="Hours">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--4--%>
									<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--5--%>
									<asp:BoundField DataField="Landings" HeaderText="Landings">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--6--%>
									<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--7--%>
									<asp:BoundField DataField="Cycles" HeaderText="Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--8--%>
									<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--9--%>
									<asp:BoundField DataField="Starts" HeaderText="Starts">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--10--%>
									<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--11--%>
									<asp:BoundField DataField="NGCycles" HeaderText="NG Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--12--%>
									<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--13--%>
									<asp:BoundField DataField="NFCycles" HeaderText="NF Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--14--%>
									<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--15--%>
									<asp:BoundField DataField="RINS" HeaderText="RINS">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--16--%>
									<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--17--%>
									<asp:BoundField DataField="Bleeds" HeaderText="Bleeds">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--18--%>
									<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--19--%>
									<asp:BoundField DataField="ImpellerCycles" HeaderText="ImpellerCycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--20--%>
									<asp:BoundField DataField="FinalImpellerCycles" 
										HeaderText="Final Impeller Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--21--%>
									<asp:BoundField DataField="CTCycles" HeaderText="CT Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--22--%>
									<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--23--%>
									<asp:BoundField DataField="PTCycles" HeaderText="PT Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--24--%>
									<asp:BoundField DataField="FinalPTCycles" 
										HeaderText="Final PT Cycles">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--25--%>
									<asp:BoundField DataField="GeneratorMods" 
										HeaderText="Generator Mods">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--26--%>
									<asp:BoundField DataField="FinalGeneratorMods" 
										HeaderText="Final Generator Mods">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--27--%>
									<asp:BoundField DataField="NRCycles" HeaderText="NR">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--28--%>
									<asp:BoundField DataField="FinalNRCycles" HeaderText="Final NR">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--29--%>
									<asp:BoundField DataField="LandingCycles" HeaderText="LCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--30--%>
									<asp:BoundField DataField="FinalLandingCycles" 
										HeaderText="Final LCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--31--%>
									<asp:BoundField DataField="LandingGearCycles" 
										HeaderText="LGCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--32--%>
									<asp:BoundField DataField="FinalLandingGearCycles" 
										HeaderText="Final LGCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--33--%>
									<asp:BoundField DataField="OverSpeedLHMLGCycles" 
										HeaderText="OLHC">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--34--%>
									<asp:BoundField DataField="FinalOverSpeedLHMLGCycles" 
										HeaderText="Final OLHC">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--35--%>
									<asp:BoundField DataField="OverSpeedRHMLGCycles" 
										HeaderText="ORHC">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--36--%>
									<asp:BoundField DataField="FinalOverSpeedRHMLGCycles" 
										HeaderText="Final ORHC">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--37--%>
									<asp:BoundField DataField="OverSpeedRHMLGCycles" 
										HeaderText="ONCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--38--%>
									<asp:BoundField DataField="FinalOverSpeedNLGCycles"
										HeaderText="Final ONCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--39--%>
									<asp:BoundField DataField="MGBTorqueCycles" HeaderText="TCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--40--%>
									<asp:BoundField DataField="FinalMGBTorqueCycles"
										HeaderText="Final TCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--41--%>
									<asp:BoundField DataField="RotorBrakeCycles" HeaderText="RBCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
									<%--42--%>
									<asp:BoundField DataField="FinalRotorBrakeCycles" 
										HeaderText="Final RBCY">
										<HeaderStyle Font-Bold="true" HorizontalAlign="Left"
											Wrap="false" />
										<ItemStyle HorizontalAlign="Left" Wrap="false" />
									</asp:BoundField>
								</Columns>
							</asp:GridView>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>

		</div>

		<script type="text/javascript">
            function delete_cookie() {
                $.cookie('HideInfoMessagepanel', false);

            }
            function ShowLastDet() {
                $pos = $("#<%=lblDateTime.ClientID%>").position();
                var top = $pos.top;
                var left = $pos.left;
                var searchHeight = $("#<%=lblDateTime.ClientID%>").height();
                var margin = top + searchHeight;

                var height = $("#tblMain").outerHeight();
                var h = margin - height;
                if ($.cookie('HideInfoMessagepanel') == 'true') $("#InfoMessagepanel").hide();
                else {
                    $.cookie('HideInfoMessagepanel', true);
                    $("#InfoMessagepanel").css("display", "block");
                    $("#InfoMessagepanel").animate({ marginTop: h, marginLeft: left - 5 }, 100, 'swing', function () {
                        $("#InfoMessagepanel").delay(9000).fadeOut();

                    });
                }

            }
		</script>

		<script type="text/javascript">
            function delete_cookie() {
                $.cookie('HideInfoMessagepanel', null);
            }
		</script>

		<div id="modalPopUps">

			<!-- File Upload Modal Dialog-->
			<div id="FileUploadModal">

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

					function OpenFileUploadWindow() {

						try {

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");

							return false;

						} catch (e) {
							alert(e);
						}

					}

				</script>

			</div>
			<!-- End -->

			<!-- Place Popup -->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyPlace" Text="Dummy Place" ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlPlace" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
					<iframe id="iPopupPlace" frameborder="0" allowtransparency="true" height="100%" width="100%"
						src="JavaScript:''" scrolling="auto"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopupPlace" runat="server" TargetControlID="btnDummyPlace"
					PopupControlID="pnlPlace" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>
				<script type="text/javascript">
					function IFramePlaceComplete() {
						$("#btnDummyPlace").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}

					function OpenPlaceWindow(BackPagetmp) {
						try {
							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupPlace").attr("src", "wfPlace_Ajax.aspx?Type=Place&AddType=2&BackPage=BackPagetmp&BackPage1=wfLogSOP_Ajax.aspx&Typepup=pup");
							if (!$.browser.msie) {
								$("#btnDummyPlace").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

						} catch (e) {
							alert(e);
						}

					}
				</script>
				<script type="text/javascript">
					function ParentCallBackPlaceFunction() {
						var atawindow = $find("<%=mdlPopupPlace.ClientID %>");
						//close ata popup window
						atawindow.hide();
						$("#iPopupPlace").attr("src", "JavaScript:''");
						//call ata image button
						$("#hdnimgBtnPlace").click();
					}
				</script>

			</div>
			<!-------------------->

			<!-- FlightLogClassification Popup -->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyFlightLogClassification" Text="Dummy FlightLogClassification" ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlFlightLogClassification" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
					<iframe id="iPopupFlightLogClassification" frameborder="0" allowtransparency="true" height="100%" width="100%" 
						src="JavaScript:''" scrolling="auto"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopupFlightLogClassification" runat="server" TargetControlID="btnDummyFlightLogClassification"
					PopupControlID="pnlFlightLogClassification" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>
				<script type="text/javascript">
					function IFrameFlightLogClassificationComplete() {
						$("#btnDummyFlightLogClassification").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}

					function OpenFlightLogClassificationWindow(BackPagetmp) {
						try {
							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupFlightLogClassification").attr("src", "wfFlightLogClassification.aspx?BackPage=BackPagetmp&BackPage1=wfLogSOP_Ajax.aspx&Typepup=pup");
							if (!$.browser.msie) {
								$("#btnDummyFlightLogClassification").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

							return false;
						} catch (e) {
							alert(e);
						}

					}
				</script>
				<script type="text/javascript">
					function ParentCallBackClassificationFunction() {
						var atawindow = $find("<%=mdlPopupFlightLogClassification.ClientID %>");
						//close ata popup window
						atawindow.hide();
						$("#iPopupFlightLogClassification").attr("src", "JavaScript:''");
						//call ata image button
						$("#hdnimgBtnFlightLogClassification").click();
					}
				</script>

			</div>
			<!-------------------->

			<!-- Pilot Popup -->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyPilot" Text="Dummy Pilot" ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlPilot" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
					<iframe id="iPopupPilot" frameborder="0" allowtransparency="true" height="100%"
						width="100%" src="JavaScript:''" scrolling="auto"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopupPilot" runat="server" TargetControlID="btnDummyPilot"
					PopupControlID="pnlPilot" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>
				<script type="text/javascript">
					function IFramePilotComplete() {
						$("#btnDummyPilot").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}

					function OpenPilotWindow(BackPagetmp) {
						try {
							$get("AjaxLoader").style.visibility = "visible";
							$("#iPopupPilot").attr("src", "wfPilot.aspx?Type=Place&AddType=2&BackPage=BackPagetmp&BackPage1=wfLogSOP_Ajax.aspx&Typepup=pup");
							if (!$.browser.msie) {
								$("#btnDummyPilot").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}

							return false;
						} catch (e) {
							alert(e);
						}

					}
				</script>
				<script type="text/javascript">
					function ParentCallBackPilotFunction() {
						var atawindow = $find("<%=mdlPopupPilot.ClientID %>");
						//close ata popup window
						atawindow.hide();
						$("#iPopupPilot").attr("src", "JavaScript:''");
						//call ata image button
						$("#hdnimgBtnPilot").click();
					}

				</script>

			</div>
			<!-------------------->

			<!-- TroubleShoot Popup Window -->
			<div>

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyDiscrepancyTroubleShoot" Text="Discrepancy TroubleShoot" ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlDiscrepancyTroubleShoot" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">
					<iframe id="IframeDiscrepancyTroubleShoot" frameborder="0" height="100%" width="100%" src="JavaScript:''"
						allowtransparency="true" scrolling="auto"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopupDiscrepancyTroubleShoot" runat="server" TargetControlID="btnDummyDiscrepancyTroubleShoot"
					PopupControlID="pnlDiscrepancyTroubleShoot" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>

				<script type="text/javascript">

					function IframeDiscrepancyTroubleShootStateComplete() {
						$("#btnDummyDiscrepancyTroubleShoot").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					function OpenDiscrepancyTroubleShootWindow() {

						try {

							var TransTypeID = 115;

							console.log("TransTypeID = " + TransTypeID);

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeDiscrepancyTroubleShoot").attr("src", "wfDiscrepancyTroubleShoot.aspx?Type=pup&TransTypeID=" + TransTypeID);

							if (!$.browser.msie) {
								$("#btnDummyDiscrepancyTroubleShoot").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}

							return false;

						} catch (e) {
							console.error("Error ocuured while opening TroubleShoot Window. Refer the Error " + e);
							alert(e);
						}

					}

					function ParentCallBackFunctionForDiscrepancyTroubleShoot() {

						var DiscrepancyTroubleShootwindow = $find("<%=mdlPopupDiscrepancyTroubleShoot.ClientID %>");

						DiscrepancyTroubleShootwindow.hide();
						$("#IframeDiscrepancyTroubleShoot").attr("src", "JavaScript:''");
						$("#hdnBtnDiscrepancyTroubleShoot1").click();
						CloseChildPage();

					}

				</script>

			</div>
			<!-- End-->

			<!-- Discrepancy & Cabin Defect Detail Popup Window -->
			<div>

				<div id="DiscrepancyDetailModal">

					<div style="display: none">

						<asp:Button runat="server"
							ID="btnDummyDiscrepancyDetail"
							Text="Discrepancy Detail"
							ClientIDMode="Static" />

					</div>

					<asp:Panel runat="server" ID="pnlDiscrepancyDetail"
						ClientIDMode="Static" HorizontalAlign="Center"
						Style="height: 100%; width: 100%;">

						<iframe id="IframeDiscrepancyDetail" frameborder="0"
							height="100%" width="100%" src="JavaScript:''"
							allowtransparency="true" scrolling="auto"></iframe>

					</asp:Panel>

					<cc2:ModalPopupExtender ID="mdlPopupDiscrepancyDetail"
						runat="server" TargetControlID="btnDummyDiscrepancyDetail"
						PopupControlID="pnlDiscrepancyDetail"
						BackgroundCssClass="clsModalPopupBG">
					</cc2:ModalPopupExtender>

					<script type="text/javascript">

						function IFrameDiscrepancyDetailComplete() {

							$("#btnDummyDiscrepancyDetail").click();
							$get("AjaxLoader").style.visibility = 'hidden';

						}

						function OpenDiscrepancyDetailWindow() {

							try {

								var TransTypeID = 115;

								$get("AjaxLoader").style.visibility = 'visible';
								$("#IframeDiscrepancyDetail").attr("src", "wfDiscrepancyCorrectiveAction.aspx?Type=pup&TransTypeID=" + TransTypeID);

								if (!$.browser.msie) {
									$("#btnDummyDiscrepancyDetail").click();
									$get("AjaxLoader").style.visibility = 'hidden';
								}

								return false;

							} catch (e) {
								console.error("Error ocuured while opening Discrepancy Detail Window. Refer the Error " + e);
								alert(e);
							}

						}

						function ParentCallBackFunctionForDiscrepancyDetail() {

							try {

								var DiscrepancyDetailWindow = $find("<%=mdlPopupDiscrepancyDetail.ClientID %>");

								DiscrepancyDetailWindow.hide();
								$("#IframeDiscrepancyDetail").attr("src", "JavaScript:''");
								$("#hdnBtnDiscrepancyDetail").click();

								CloseChildPage();

							} catch (e) {
								console.error("Error ocuured in ParentCallBackForDiscrepancyDetail(). Refer the Error " + e);
								alert(e);
							}

						}

					</script>

				</div>

				<div id="CabinDefectDetailModal">

					<div style="display: none">

						<asp:Button runat="server"
							ID="dummyBtnCabinDefectDetail"
							Text="Cabin Defect Detail"
							ClientIDMode="Static" />
					</div>

					<asp:Panel runat="server" ID="pnlCabinDefectDetail"
						ClientIDMode="Static" HorizontalAlign="Center"
						Style="height: 100%; width: 100%;">ss

						<iframe id="IframeCabinDefectDetail" frameborder="0"
							height="100%" width="100%" src="JavaScript:''"
							allowtransparency="true" scrolling="auto"></iframe>

					</asp:Panel>

					<cc2:ModalPopupExtender ID="ModalPopupExtenderCabinDefectDetail" 
						runat="server" TargetControlID="dummyBtnCabinDefectDetail"
						PopupControlID="pnlCabinDefectDetail"
						BackgroundCssClass="clsModalPopupBG" />

					<script type="text/javascript">

						function IFrameCabinDefectDetailComplete() {

							$("#dummyBtnCabinDefectDetail").click();
							$get("AjaxLoader").style.visibility = 'hidden';

						}

						function OpenCabinDefectDetailWindow() {

							try {

								console.log("OpenCabinDefectDetailWindow() called from Discrepancy Detail page");
								var TransTypeID = 116;

								$get("AjaxLoader").style.visibility = 'visible';
								$("#IframeCabinDefectDetail").attr("src", "wfDiscrepancyCorrectiveAction.aspx?Type=pup&TransTypeID=" + TransTypeID);

								if (!$.browser.msie) {
									$("#dummyBtnCabinDefectDetail").click();
									$get("AjaxLoader").style.visibility = 'hidden';
								}

								console.log("OpenCabinDefectDetailWindow() ended at Discrepancy Detail page");

								return false;

							} catch (e) {
								console.error("Error ocuured while opening Cabin Defect Window. Refer the Error " + e);
								alert(e);
							}

						}

						function ParentCallBackForCabinDefectDetail() {

							try {

								var CabinDefectDetailWindow = $find("<%=ModalPopupExtenderCabinDefectDetail.ClientID %>");

								CabinDefectDetailWindow.hide();
								$("#IframeCabinDefectDetail").attr("src", "JavaScript:''");
								$("#hdnBtnCabinfDefectDetail").click();

								CloseChildPage();

							} catch (e) {
								console.error("Error ocuured in ParentCallBackForCabinDefectDetail(). Refer the Error " + e);
								alert(e);
							}

						}

					</script>

				</div>

			</div>
			<!-- End-->

		</div>

	</form>

	<%--Show All Assemblies--%>
	<script type="text/javascript" id="ALLAssemblies">

		function ShowAllAssemblies() {

			try {

				console.log("Entered function ShowAllAssemblies");

				var popupPanel = $("#pnlAllAssemblypanel");

				var viewportWidth = $(window).width();
				var viewportHeight = $(window).height();

				var modalMaxWidth = viewportWidth * 0.9;
				var modalMaxHeight = viewportHeight * 0.9;

				$("#modal-blur-overlay").css("display", "block");

				popupPanel.css({
					"display": "block",
					"position": "fixed",
					"top": "50%",
					"left": "50%",
					"transform": "translate(-50%, -50%)",
					"width": modalMaxWidth + "px",
					"height": "auto",
					"max-height": modalMaxHeight + "px",
					"overflow": "hidden"
				});

				var headerHeight = popupPanel.find("table:first").outerHeight(true);
				var padding = 40;
				var scrollableHeight = modalMaxHeight - headerHeight - padding;

				$("#gridScrollContainer").css({
					"max-height": scrollableHeight + "px",
					"overflow": "auto"
				});

				console.log("function ShowAllAssemblies ended");

			} catch (e) {
				console.error("An error occurred in ShowAllAssemblies():", e);
			}

		}

		function CloseAllAssembliesModal() {

			try {

				console.log("Entered function CloseAllAssembliesModal");

				$("#pnlAllAssemblypanel").hide();
				$("#modal-blur-overlay").hide();

				console.log("function CloseAllAssembliesModal ended");

			} catch (e) {
				console.error("An error occurred in CloseAllAssembliesModal():", e);
			}

		}

	</script>

	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

			try {

				console.log("pageLoad() of LogSOP called");

				var str = "<%=System.Configuration.ConfigurationManager.AppSettings("TimeFormatLOG").ToString()%>";
				var bool1;
				var savedlog1;

				//AJAX- Hidden Field value used here
				if (document.getElementById("LogObjValue").value == "True") {
					savedlog1 = "button";
				}
				else {
					savedlog1 = "";
				}

				if (str.search("TT") === -1 && str.search("tt") === -1) {
					bool1 = false;
				}
				else {
					bool1 = true;
				}

			} catch (e) {
				console.error("Error ocuured in Page Load from JS. Refer the Error " + e);
				alert(e);
			}

		});

	</script>

	<!-- Autocomplete for Source and Destination Place, Pilot 1 & Pilot 2  -->
	<script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $("#<%=Place1.ClientID%>,#<%=Place2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
                width: 200,
                autoFill: true,
                matchContains: true,
                delay: 0


            });

        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $("#<%=Pilot1.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
                autoFill: true,
                width: 252,
                mustMatch: true,
                matchContains: true,
                delay: 0
            });

        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $("#<%=Pilot2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
                autoFill: true,
                width: 256,
                mustMatch: true,
                matchContains: true,
                delay: 0
            });

        });

	</script>

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

                console.log("onSuccess function started");

                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);

                console.log("onSuccess function finished");

            }

            function onError(result) {

                console.log("onError function started");

                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');

                console.log("onError function finished");

            }
            function OnBeforeSend() {
                console.log("OnBeforeSend function started");
                $(elem).addClass('ac_loading');
                console.log("OnBeforeSend function finished");
            }

        }

	</script>

	<script type="text/javascript">

		function ParentCallBackFunctionForLogFuelOil() {
			CloseChildPage();
		}
		function ParentCallBackFunctionForLogParameter() {
			CloseChildPage();
		}
		function ParentCallBackFunctionForLogCrew() {
			CloseChildPage();
		}
		function ParentCallBackFunctionForLogMaintenanceActivity() {
			CloseChildPage();
		}
		function ParentCallBackFunctionForLogDefectAction() {
			CloseChildPage();
		}

	</script>

	<script type="text/javascript">

		function CloseLastDet() {

			try {

				$("#InfoMessagepanel").delay(9000).fadeOut();

			} catch (e) {
				console.error("Error ocuured in CloseLastDet(). Refer the Error " + e);
				alert(e);
			}

		}

		function CloseChildPage() {

			try {

				$find('<%=tabLogDetailsContainer.ClientID%>').set_activeTabIndex(0);

			} catch (e) {
				console.error("Error ocuured in CloseChildPage(). Refer the Error " + e);
				alert(e);
			}

		}

	</script>

</body>
</html>

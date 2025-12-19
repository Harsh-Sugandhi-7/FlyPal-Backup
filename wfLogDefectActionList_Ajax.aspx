<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogDefectActionList_Ajax.aspx.vb"
	Inherits="Flypal.wfLogDefectActionList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>Snag / Defect Reporting Detail</title>
	<meta http-equiv="x-ua-compatible" content="IE=9" />
	<meta name="vs_showGrid" content="True" />
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
	<meta name="vs_defaultClientScript" content="JavaScript" />
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	<link id="MainStyle" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" type="text/css" href="popup.css" />
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="AlertMessage.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js">
	</script>
	<script language="javascript" type="text/javascript" src="DATEFUNCTIONS.js">
	</script>
	<script id="clientEventHandlersJS" type="text/javascript" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>
	<style type="text/css">
		.clsCursorStyle {
			cursor: pointer;
		}
	</style>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="LogDefectActionListForm" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblMain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
							<table id="tblinner" class="clsTablelistin" cellpadding="0">
								<tbody>
									<tr>
										<td class="clsFormHeader1Newstyle">
											<table width="100%">

												<tr>
													<td>
														<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
                                                                    Log Details
																</asp:Label>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="right">
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
																		ToolTip="Click to save record in Log Defect List"></asp:Button>
																</td>
																<td>
																	<asp:UpdatePanel ID="upnlMailTool" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<placeholder id="phSendMail" runat="server" visible="<%#Not mMELSnagCorrectiveAction.IsNew %>">

																				<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Send Mail"
																					ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to send Mail to if ADD is added", "Click to send Mail to if MEL is added") %>'></asp:Button>

																			</placeholder>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td>
																	<asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" Visible="true"
																		CausesValidation="False" ToolTip="Click to go back to previous page"></asp:Button>
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<asp:PlaceHolder runat="server" Visible="false">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table1" border="0">
															<tr>
																<td>
																	<asp:Button ID="btnLogDetails" runat="server" CssClass="clsButtonLong_Ajax" Text="Log Details"
																		CausesValidation="False"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnFuelOil" runat="server" CssClass="clsButtonLong_Ajax" Text="Fuel Oil"
																		CausesValidation="False"></asp:Button>
																</td>
																<td>
																	<asp:Label ID="lblSnagReport" runat="server" CssClass="clsLabelButton" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Reporting", "Snag Reporting") %>'
																		ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag") %>'></asp:Label>
																</td>
																<td>
																	<asp:Button ID="btnParameterList" runat="server" CssClass="clsButtonLong_Ajax" Text="Parameter List"
																		CausesValidation="False" ToolTip="Parameter List"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnLogPax" runat="server" CssClass="clsButtonLong_Ajax" Text="Passenger Log"
																		Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' CausesValidation="False"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnHobbsOffset" runat="server" CssClass="clsButtonLong_Ajax" Text="Hobbs Offset"
																		Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>' CausesValidation="False"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnFlightCrew" runat="server" CssClass="clsButtonLong_Ajax" Text="Flight Crew"
																		CausesValidation="False"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnMaintenanceAcitvity" runat="server" CssClass="clsButtonLong_Ajax"
																		Text="Maintenance Activity" CausesValidation="False"></asp:Button>
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
											<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:ValidationSummary ID="vsDiscrepencyDetails" CssClass="clsValidationSummary" runat="server"
														HeaderText="Fill Up The Following Fields." ValidationGroup="a">
													</asp:ValidationSummary>
													<asp:CustomValidator ID="cvDefectList" runat="server" ControlToValidate="txtDefect"
														OnServerValidate="customvalidate" Display="None" CssClass="clslabelauto" ValidationGroup="a">
													</asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtDefect"
														OnServerValidate="customvalidate1" Display="None" CssClass="clslabelauto" ValidationGroup="a">
													</asp:CustomValidator>
													<asp:CustomValidator ID="cvDiscrepancyDetails" runat="server" CssClass="clsLabelAuto" 
														ValidationGroup="a" ControlToValidate="txtDefectReportNo" Display="None" ErrorMessage="">
													</asp:CustomValidator>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td>
											<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="Table8" border="0" width="100%">
														<tr>
															<td colspan="2">
																<fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -5px">
																	<legend id="Legend3" runat="server" style="font-weight: bold">MEL / Snag Details</legend>
																	<table width="100%">

																		<tr>
																			<td>
																				<asp:Label ID="lblDatePlaceofoccuranceStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																			</td>
																			<td colspan="4">
																				<asp:Label ID="lblDefectReportNo" runat="server" CssClass="clsLabelAuto">Defect No.</asp:Label>
																			</td>
																			<td colspan="3">

																				<asp:TextBox ID="txtDefectReportNo" runat="server" CssClass="clsTextBoxTagSearch" Width="130px" Text="<%# mMELSnagCorrectiveAction.DefectReportNo %>"
																					ToolTip="Enter Defect Text" MaxLength="25"></asp:TextBox>
																				<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mMELSnagCorrectiveAction.No %>"
																					ToolTip="Enter Defect No." MaxLength="4"></asp:TextBox>
																			</td>
																			<td>
																				<asp:Label ID="Label4" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
																			</td>
																			<td colspan="1">
																				<asp:Label ID="lblDatePlaceofoccurance" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Date of occurrence (UTC)", "Date of occurrence") %>'>Date of occurrence</asp:Label>
																			</td>
																			<td>
																				<asp:TextBox ID="txtDateofoccurrence" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
																					AutoPostBack="True" onchange="ValidateDateText(this,'txtDateofoccurrence_watermarkextender');"></asp:TextBox>
																				<cc2:CalendarExtender ID="txtDateofoccurrence_CalendarExtender" runat="server" CssClass="cal_Theme1"
																					Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDateofoccurrence"></cc2:CalendarExtender>
																				<cc2:TextBoxWatermarkExtender TargetControlID="txtDateofoccurrence" ID="txtDateofoccurrence_watermarkextender"
																					ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																					WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																			</td>
																			<td>
																				<asp:Label ID="lblStar1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
																			</td>
																			<td>
																				<asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
																			</td>
																			<td>
																				<asp:TextBox ID="txtLogNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mMELSnagCorrectiveAction.LogNo %>"
																					BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
																			</td>
																		</tr>
																		<tr>
																			<td colspan="14">
																				<asp:UpdatePanel ID="upnlSnagType" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<table id="Table2" border="0" width="100%">
																							<tr>
																								<td style="width: 33%;">
																									<fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
																										<legend><b>
																											<asp:Label ID="lblMELSnag" runat="server" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","ADD/Defect Type","MEL/Snag Type") %>'></asp:Label></b></legend>
																										<table id="Table6">
																											<tr>
																												<td>
																													<asp:RadioButton ID="rbMajor" runat="server" CssClass="clsRadioButton" Width="60px"
																														Text="Major" Checked="<%# mMELSnagCorrectiveAction.IsMajor %>" GroupName="a"></asp:RadioButton>
																												</td>
																												<td>&nbsp;&nbsp;&nbsp;&nbsp;
																												</td>
																												<td>
																													<asp:RadioButton ID="rbMinor" runat="server" CssClass="clsRadioButton" Width="60px"
																														Text="Minor" Checked="<%# mMELSnagCorrectiveAction.IsMinor %>" GroupName="a"></asp:RadioButton>
																												</td>
																											</tr>
																										</table>
																									</fieldset>
																								</td>
																								<td style="width: 33%;">
																									<fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
																										<legend><b>Defect Type</b></legend>
																										<table id="Table7">
																											<tr>
																												<td>
																													<asp:RadioButton ID="rbPireps" runat="server" CssClass="clsRadioButton" Text="Pireps"
																														Width="60px" Checked="<%# mMELSnagCorrectiveAction.IsPireps %>" GroupName="b"></asp:RadioButton>
																												</td>
																												<td>&nbsp;&nbsp;&nbsp;&nbsp;
																												</td>
																												<td>
																													<asp:RadioButton ID="rbMaintenanceDefect" runat="server" CssClass="clsRadioButton"
																														Width="136px" Text="Maintenance Defect" Checked="<%# mMELSnagCorrectiveAction.IsMaintenanceDefect %>"
																														GroupName="b"></asp:RadioButton>
																												</td>
																												<%-- <td>
                                                                                                                    <asp:RadioButton ID="rbCabinDefect" runat="server" CssClass="clsRadioButton"
                                                                                                                        Width="136px" Text="Cabin Defect"
                                                                                                                        GroupName="b"></asp:RadioButton>
                                                                                                                </td>--%>
																											</tr>
																										</table>
																									</fieldset>
																								</td>
																								<td style="width: 33%;">
																									<fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
																										<legend><b>Reliability</b></legend>
																										<table id="Table4">
																											<tr>
																												<td>
																													<asp:CheckBox ID="chkIsInReliability" runat="server" CssClass="clsCheckBox" Text="Consider In Reliability"
																														Width="152px" Checked="<%# mMELSnagCorrectiveAction.IsInReliability %>"></asp:CheckBox>
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
																		<tr>
																			<td></td>
																			<td colspan="4">
																				<asp:Label ID="lblSector" runat="server" CssClass="clsLabelAuto">Sector / Place</asp:Label>
																			</td>
																			<td colspan="3">
																				<asp:TextBox ID="txtSector" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mMELSnagCorrectiveAction.Sector %>"
																					ToolTip="Enter Sector/Place" MaxLength="50" DESIGNTIMEDRAGDROP="110"></asp:TextBox>
																			</td>
																			<td></td>
																			<td>
																				<asp:Label ID="Label7" runat="server" CssClass="clsLabelAuto" Width="125px" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect reported by", "Snag reported by") %>'></asp:Label>
																			</td>
																			<td colspan="2">
																				<asp:TextBox ID="txtSnagReportedBy" runat="server" CssClass="clsTextBoxTagSearchCombo" Text="<%# mMELSnagCorrectiveAction.SnagReportedBy %>"
																					ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Enter name of Defect reporter", "Enter name of Snag reporter") %>'
																					MaxLength="50"></asp:TextBox>
																			</td>
																			<td>
																				<asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">(e.g. Pilot/AME/Passenger)</asp:Label>

																			</td>
																		</tr>
																		<tr>
																			<td></td>
																			<td colspan="4">
																				<asp:Label ID="lblAircraftHrsLandingsTSNsincelastmajorcheck" Width="165px" runat="server" CssClass="clsLabelAuto">Aircraft Hrs. /Landings (TSN since last major check)</asp:Label>
																			</td>
																			<td colspan="3">
																				<asp:TextBox ID="txtLastMajorCheck" runat="server" CssClass="clsTextBoxTagSearchCombo" Text="<%# mMELSnagCorrectiveAction.LastMajorCheckHour %>"
																					ToolTip="Enter Aircraft Hrs./Landings" MaxLength="50"></asp:TextBox>
																			</td>

																			<td></td>
																			<td>
																				<asp:Label ID="lblReportedBy" runat="server" Width="165px" CssClass="clsLabelAuto">Name of Pilot /AME & License No./Observed By</asp:Label>
																			</td>
																			<td colspan="5">
																				<asp:TextBox ID="txtReportedBy" runat="server" CssClass="clsTextBoxTagSearchCombo" Text="<%# mMELSnagCorrectiveAction.ReportedBy %>"
																					ToolTip="Enter name of Pilot" MaxLength="150"></asp:TextBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="lblParameterStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
																			</td>
																			<td colspan="4">
																				<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Defect</asp:Label>
																			</td>
																			<td colspan="3">
																				<asp:TextBox ID="txtDefect" runat="server" CssClass="clsTextBoxTagSearchLong1" Text="<%# mMELSnagCorrectiveAction.Defect %>"
																					Width="95%" ToolTip="Enter Defect Description" MaxLength="1000" TextMode="MultiLine">
																				</asp:TextBox>
																			</td>
																			<td></td>
																			<td>
																				<span id="lblDefectOn" class="clsLabelAuto">Defect On</span>
																			</td>
																			<td colspan="5">
																				<asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchCombo" DataTextField="ModelSerialNoPostion"
																					DataValueField="AssemblyStatusID">
																				</asp:DropDownList>
																			</td>
																		</tr>
																	</table>
															</td>
														</tr>
														<tr>
															<td colspan="2">
																<cc2:TabContainer ID="tabLogDetailsMELContainer" runat="server" class="clstablelistin"
																	AutoPostBack="false">
																	<cc2:TabPanel ID="tbpnlFuelOil" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
																		<HeaderTemplate>
																			<asp:Label runat="server" Text="Verification Details" ID="Label6"></asp:Label>
																		</HeaderTemplate>
																		<ContentTemplate>
																			<asp:UpdatePanel ID="upnlMMELDetails" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -7px">
																						<legend id="Legend1" runat="server" style="font-weight: bold">
																							<table width="100%">
																								<tr>
																									<td>
																										<span class="clsLabelHeader">Minimum Equipment Detail</span>
																									</td>
																									<td>
																										<asp:CheckBox ID="chkShowMEL" runat="server" CssClass="clsCheckBox" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD (check if ADD)", "MEL (check if MEL)") %>'
																											Checked="<%# mMELSnagCorrectiveAction.IsMEL %>" AutoPostBack="True"></asp:CheckBox>
																									</td>
																								</tr>
																							</table>
																						</legend>
																						<table width="80%" style="margin-top: -7px">
																							<tr>
																								<td></td>
																								<td colspan="8">
																									<asp:UpdatePanel ID="upnlErrors1" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:ValidationSummary ID="vsVerificationDetails" CssClass="clsValidationSummary" runat="server"
																												ValidationGroup="1" HeaderText="Fill Up The Following Fields.">
																											</asp:ValidationSummary>
																											<asp:CustomValidator ID="cvFrequencyInHours" runat="server" ControlToValidate="txtFrequencyInHours"
																												ValidationGroup="1" ErrorMessage="Please select MEL Category"
																												OnServerValidate="customvalidate" Display="None" CssClass="clslabelauto">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvFrequencyInDay" runat="server" ControlToValidate="txtFrequencyInDay"
																												ValidationGroup="1" CssClass="clslabelauto"
																												ErrorMessage="Please select the Due Date" OnServerValidate="customvalidate" Display="None">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvOccDate" runat="server" ControlToValidate="txtDateofoccurrence"
																												OnServerValidate="customvalidate" Display="None" CssClass="clslabelauto" ValidationGroup="1">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvDueDate" runat="server" ControlToValidate="txtDueDate" 
																												ValidationGroup="1" OnServerValidate="customvalidate" Display="None" 
																												CssClass="clslabelauto">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvComponent" runat="server" ControlToValidate="cmbMELCategory" 
																												ValidationGroup="1" ErrorMessage="" OnServerValidate="customvalidate" 
																												Display="None" CssClass="clslabelauto">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvEx" runat="server" Display="None"
																												ControlToValidate="txtExtensionInDays" ValidationGroup="1"
																												OnServerValidate="customvalidate" CssClass="clslabelauto">
																											</asp:CustomValidator>
																											<asp:CustomValidator ID="cvVerificationDetails" runat="server"
																												CssClass="clsLabelAuto" ValidationGroup="1"
																												ControlToValidate="cmbATAChapter" Display="None" ErrorMessage="">
																											</asp:CustomValidator>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																							</tr>
																							<tr>
																								<td></td>
																								<td colspan="4">
																									<asp:Label ID="Label8" runat="server" CssClass="clsLabelAuto">Please select part, if not present then type in Part No. field.</asp:Label>
																								</td>
																								<td align="right" colspan="4">
																									<asp:LinkButton ID="lnlMELDetail" runat="server" CssClass="clsHyperlink1" ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to view ADD details", "Click to view MEL details") %>'
																										Text='<%# iif(AppSettings("MELSnagNomenclature") = "True", "View ADD Detail", "View MEL Detail") %>'
																										Visible="<%#  not mMELSnagCorrectiveAction.MELID.Equals(Guid.Empty)%>"></asp:LinkButton>
																								</td>
																							</tr>
																							<tr>
																								<td style="width: 9px;"></td>
																								<td style="width: 127px;">
																									<asp:Label ID="Label9" runat="server" CssClass="clsLabelAuto">Component</asp:Label>
																								</td>
																								<td style="width: 360px;">
																									<asp:UpdatePanel ID="upnlShowMEL" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
																												SelectedValue="<%# mMELSnagCorrectiveAction.PartID %>" DataValueField="CompID"
																												DataTextField="PartNoSerialNo">
																											</asp:DropDownList>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td>
																									<asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlPartNo" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Text="<%# mMELSnagCorrectiveAction.PartNo %>"
																												ToolTip="Part No." MaxLength="50"></asp:TextBox>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td colspan="2">
																									<asp:Label ID="Label10" runat="server" CssClass="clsLabelAuto">Serial No</asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlSerialNo" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Text="<%# mMELSnagCorrectiveAction.PartSerialNo %>"
																												ToolTip="Serial No." MaxLength="50"></asp:TextBox>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td>
																									<asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlDesc" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Width="230px" Text="<%# mMELSnagCorrectiveAction.Description %>"
																												ToolTip="Description" MaxLength="50"></asp:TextBox>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>

																							</tr>
																							<tr>
																								<td></td>
																								<td>
																									<asp:Label Width="100px" ID="lblATAChapter" runat="server" CssClass="clsLabelAuto">ATA Chapter</asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="True"
																												SelectedValue="<%# mMELSnagCorrectiveAction.ATAChapterID %>" DataValueField="ID"
																												DataTextField="ATAChapter">
																											</asp:DropDownList>
																											<asp:CustomValidator ID="cvATA" runat="server" CssClass="clslabelauto" Display="None" ValidationGroup="1"
																												OnServerValidate="customvalidate" ControlToValidate="cmbATAChapter"></asp:CustomValidator>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td>
																									<asp:Label ID="lblSubATAChapter" runat="server" CssClass="clsLabelAuto" Width="100px">Sub-ATA Chapter</asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
																												DataTextField="SubATAChapter">
																											</asp:DropDownList>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td colspan="2">
																									<asp:Label ID="lblMELCategory" Width="100px" runat="server" CssClass="clsLabelAuto" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category", "MEL Category") %>'></asp:Label>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlMELCategory" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:DropDownList ID="cmbMELCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
																												DataValueField="ID" DataTextField="Name" AutoPostBack="True" SelectedValue="<%# mMELSnagCorrectiveAction.MELCategoryID %>">
																											</asp:DropDownList>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td>
																									<asp:Label ID="lblHrsofComp" Width="100px" runat="server" CssClass="clsLabelAuto">Hrs. of Comp</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtHrsofComp" runat="server" CssClass="clsTextBoxTagSearch" Width="230px" Text="<%# mMELSnagCorrectiveAction.ComponentHour %>"
																										ToolTip="Enter Component Hours" MaxLength="50" DESIGNTIMEDRAGDROP="185"></asp:TextBox>
																								</td>
																								<td></td>
																							</tr>
																							<tr>
																								<td></td>
																								<td>
																									<asp:Label ID="lblFrequency" runat="server" CssClass="clsLabelAuto">Frequency</asp:Label>
																								</td>
																								<td colspan="4">
																									<asp:UpdatePanel ID="upnlFreq" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:TextBox ID="txtFrequencyInDay" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
																												Text="<%# mMELSnagCorrectiveAction.FrequencyInDays %>" ToolTip="Enter Frequency In Days"
																												AutoPostBack="True" MaxLength="4" Enabled="False"></asp:TextBox>
																											<asp:Label ID="lblDays" runat="server" CssClass="clsLabelAuto">In Days</asp:Label>
																											<asp:TextBox ID="txtFrequencyInHours" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
																												Text="<%# mMELSnagCorrectiveAction.FrequencyInHours %>" ToolTip="Enter Frequency In Hours"
																												AutoPostBack="True" MaxLength="5" Enabled="False"></asp:TextBox>
																											<asp:Label ID="lblHours" runat="server" CssClass="clsLabelAuto">Hours</asp:Label>
																											<asp:CheckBox ID="chkIsInHours" runat="server" CssClass="clsCheckBox" Text="(Select if Freq. in Hours)"
																												Checked="<%# mMELSnagCorrectiveAction.IsHours %>" AutoPostBack="True" Enabled="False"></asp:CheckBox>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td colspan="2">
																									<asp:Label ID="lblDueDate" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Due Date (UTC)", "Due Date") %>'>Due Date</asp:Label>
																								</td>
																								<td>
																									<%--<uc1:SICalendar  ID="txtDueDate" runat="server"></uc1:SICalendar>--%>
																									<asp:UpdatePanel ID="upnlDueDate" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:TextBox ID="txtDueDate" runat="server" CssClass="clsTextBoxTagSearchDate" AutoPostBack="true"
																												onchange="ValidateDateText(this,'txtDueDate_watermarkextender');"></asp:TextBox>
																											<cc2:CalendarExtender ID="CalExt_txtDueDate" runat="server" Enabled="True" TargetControlID="txtDueDate"
																												CssClass="cal_Theme1" Format="<%$AppSettings:DateFormat%>"></cc2:CalendarExtender>
																											<cc2:TextBoxWatermarkExtender TargetControlID="txtDueDate" ID="txtDueDate_watermarkextender"
																												ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																												WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td colspan="1">
																									<asp:Label ID="lblIncidentType" runat="server" CssClass="clsLabelAuto">Incident Type</asp:Label>
																								</td>
																								<td>
																									<asp:DropDownList ID="cmbIncidentType" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="130px"
																										DataValueField="ID" DataTextField="Name" SelectedValue="<%# mMELSnagCorrectiveAction.IncidentTypeID %>">
																									</asp:DropDownList>
																								</td>
																							</tr>
																							<!--/////////////////////////////////////////////////////////////// -->
																							<tr>
																								<td></td>
																								<td>
																									<span id="lblExtensionApplied" class="clsLabelAuto">Extension</span>
																								</td>
																								<td>
																									<asp:UpdatePanel ID="upnlExtension" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:CheckBox ID="chkExtensionApplied" runat="server" CssClass="clsCheckBox" Checked="<%# mMELSnagCorrectiveAction.ExtensionApplied %>"
																												AutoPostBack="True" Enabled="False"></asp:CheckBox>
																											<asp:TextBox ID="txtExtensionInDays" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
																												Text="<%# mMELSnagCorrectiveAction.ExtensionInDays %>" ToolTip="Enter Frequency In Days"
																												AutoPostBack="True" MaxLength="4" Enabled="False"></asp:TextBox>
																											<span id="lblInDays" class="clsLabelAuto">In Days</span>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																								<td></td>
																								<td>
																									<span id="lblExtensionApprovalNo" runat="server" cssclass="clsLabelAuto">Approval Details</span>
																								</td>
																								<td>
																									<asp:TextBox ID="txtExtensionApprovalNo" runat="server" CssClass="clsTextBoxTagSearch"
																										Text="<%# mMELSnagCorrectiveAction.ExtensionApprovalNo %>" ToolTip="Enter Extension Approval No"
																										MaxLength="100" Enabled="False"></asp:TextBox>
																								</td>
																								<td>&nbsp;
																								</td>
																							</tr>
																						</table>
																					</fieldset>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</ContentTemplate>
																	</cc2:TabPanel>

																	<cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
																		<HeaderTemplate>
																			<asp:Label runat="server" Text="Rectification Details" ID="Label11"></asp:Label>
																		</HeaderTemplate>
																		<ContentTemplate>
																			<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																				<table width="40%">
																					<tr>
																						<td colspan="2">
																							<asp:UpdatePanel ID="upnlErrorList2" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<asp:ValidationSummary ID="vsRectificationDetails" CssClass="clsValidationSummary" runat="server"
																										ValidationGroup="2" HeaderText="Fill Up The Following Fields">
																									</asp:ValidationSummary>
																									<asp:CustomValidator ID="cvAction" runat="server" ControlToValidate="txtAction"
																										OnServerValidate="customvalidate" ValidationGroup="2" Display="None" CssClass="clslabelauto">
																									</asp:CustomValidator>
																									<asp:CustomValidator ID="cvRectifiedLogNo" runat="server" ControlToValidate="cmbRectifiedLogNo"
																										ValidationGroup="2" OnServerValidate="customvalidate2" Display="None" CssClass="clslabelauto">
																									</asp:CustomValidator>
																									<asp:CustomValidator ID="cvRectifiedDate" runat="server" ControlToValidate="cmbRectifiedLogNo"
																										ValidationGroup="2" OnServerValidate="customvalidate2" Display="None" CssClass="clslabelauto">
																									</asp:CustomValidator>
																									<asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtAction"
																										ValidationGroup="2" Display="None" CssClass="clslabelauto" OnServerValidate="customvalidate2">
																									</asp:CustomValidator>
																									<asp:CustomValidator ID="cvRectificationDetails" runat="server"
																										CssClass="clsLabelAuto" ValidationGroup="2"
																										ControlToValidate="txtRectifiedDate" Display="None" ErrorMessage="">
																									</asp:CustomValidator>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<asp:CheckBox ID="chkClose" runat="server" CssClass="clsCheckBox" Text="" Checked="<%# mMELSnagCorrectiveAction.InvestigationStatus %>"
																										AutoPostBack="True"></asp:CheckBox>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</td>
																						<td>
																							<asp:Label ID="lblStatus" runat="server" CssClass="clsLabel">Investigation Status (Closed)</asp:Label>
																						</td>
																					</tr>

																				</table>
																				<asp:PlaceHolder runat="server" ID="plhRectification">
																					<table style="margin-top: -5px">
																						<tr>
																							<td style="width: 9px;"></td>
																							<td colspan="2">
																								<asp:Label ID="lblIsRepetitive" runat="server" CssClass="clsLabelAuto">Is Repetitive</asp:Label>
																							</td>
																							<td>
																								<asp:CheckBox ID="chkIsRepetitive" runat="server" CssClass="clsCheckBox" Checked="<%# mMELSnagCorrectiveAction.IsRepetitive %>"></asp:CheckBox>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label ID="lblRectifiedDate" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Rectified Date (UTC)", "Rectified Date") %>'>Rectified Date</asp:Label>
																							</td>
																							<td>
																								<asp:UpdatePanel ID="upnlRectifiedDate" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<asp:TextBox ID="txtRectifiedDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
																											AutoPostBack="true" onchange="ValidateDateText(this,'txtRectifiedDate_watermarkextender');"></asp:TextBox>
																										<cc2:CalendarExtender ID="CalExt_txtRectifiedDate" runat="server" Enabled="True"
																											TargetControlID="txtRectifiedDate" CssClass="cal_Theme1" Format="<%$AppSettings:DateFormat%>"></cc2:CalendarExtender>
																										<cc2:TextBoxWatermarkExtender TargetControlID="txtRectifiedDate" ID="txtRectifiedDate_watermarkextender"
																											ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																											WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																									</ContentTemplate>
																								</asp:UpdatePanel>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label ID="lblRectifiedLogNo" runat="server" CssClass="clsLabelAuto">Rectified Log No.</asp:Label>
																							</td>
																							<td>
																								<asp:UpdatePanel ID="upnlRectifiedCombo" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<asp:DropDownList ID="cmbRectifiedLogNo" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																											AutoPostBack="True" DataValueField="LogID" DataTextField="LogNoLogPageNo" Enabled="False">
																										</asp:DropDownList>
																									</ContentTemplate>
																								</asp:UpdatePanel>
																							</td>
																						</tr>

																						<tr>
																							<td></td>
																							<td colspan="2">
																								<asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">Action</asp:Label>
																							</td>
																							<td>
																								<asp:TextBox ID="txtAction" runat="server" CssClass="clsTextBoxTagSearchLong1" Text="<%# mMELSnagCorrectiveAction.Action %>"
																									Width="275px" ToolTip="Enter Action" MaxLength="1000" TextMode="MultiLine">
																								</asp:TextBox>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label Width="100px" ID="lblAction" runat="server" CssClass="clsLabelAuto">Cause of defect</asp:Label>
																							</td>
																							<td>
																								<asp:TextBox ID="txtCauseofDefect" runat="server" CssClass="clsTextBoxTagSearchLong1"
																									Width="275px" Text="<%# mMELSnagCorrectiveAction.CauseOfDefect %>" ToolTip="Enter causes of Defect"
																									MaxLength="1000" TextMode="MultiLine">
																								</asp:TextBox>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label ID="lblPreventiveMeasuresTaken" runat="server" CssClass="clsLabelAuto"
																									DESIGNTIMEDRAGDROP="872" Width="120px">Preventive Measures Taken</asp:Label>
																							</td>
																							<td>
																								<asp:TextBox ID="txtPreventiveMeasuresTaken" runat="server" CssClass="clsTextBoxTagSearchLong1"
																									Width="275px" Text="<%# mMELSnagCorrectiveAction.PreventionTaken %>" ToolTip="Enter preventive measures to be taken"
																									MaxLength="1000" TextMode="MultiLine">
																								</asp:TextBox>
																							</td>
																						</tr>
																						<tr>

																							<td></td>
																							<td colspan="2">
																								<asp:Label Width="145px" ID="Label3" runat="server" CssClass="clsLabelAuto" DESIGNTIMEDRAGDROP="876">Action taken against eng. staff</asp:Label>
																							</td>
																							<td>
																								<asp:TextBox ID="txtActionTakenAganistEngStaff" runat="server" CssClass="clsTextBoxTagSearch1"
																									Text="<%# mMELSnagCorrectiveAction.ActionAgainstStaff %>" ToolTip="Enter actions against Staff"
																									MaxLength="50">
																								</asp:TextBox>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label ID="lblRectificationSector" runat="server" CssClass="clsLabelAuto">Sector / Place</asp:Label>
																							</td>
																							<td>
																								<asp:TextBox ID="txtRectificationSector" runat="server" CssClass="clsTextBoxTagSearch1"
																									Text="<%# mMELSnagCorrectiveAction.RectifiedStation %>" ToolTip="Enter Sector/Place"
																									MaxLength="50">
																								</asp:TextBox>
																							</td>
																							<td></td>
																							<td>
																								<asp:Label ID="lblRectificationMechanic" runat="server" CssClass="clsLabelAuto">Mechanic/Rectification By</asp:Label>
																							</td>
																							<td>
																								<asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<table>
																											<tr>
																												<td>
																													<asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter name of Mechanic"
																														AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNo_TextChanged"
																														AutoPostBack="true" MaxLength="200"></asp:TextBox>
																													<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
																														DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
																														CompletionInterval="1" ServicePath="wfLogDefectActionList_Ajax.aspx" ServiceMethod="GetLicenceList"
																														TargetControlID="txtLicenceNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
																														CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
																														OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
																														OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
																													</cc2:AutoCompleteExtender>
																												</td>
																												<td>
																													<asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
																														Height="22px" Width="24px" ToolTip="Click to add multiple Mechanics" CausesValidation="true" />
																												</td>
																											</tr>
																											<tr>
																												<td colspan="2">
																													<asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 %>"
																														ToolTip="<%# mMELSnagCorrectiveAction.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
																												</td>
																											</tr>
																										</table>
																									</ContentTemplate>
																								</asp:UpdatePanel>
																							</td>
																						</tr>
																						<tr>
																							<td></td>
																							<td colspan="2">
																								<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
																							</td>
																							<td colspan="4">
																								<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchLong" Text="<%# mMELSnagCorrectiveAction.Remark %>"
																									ToolTip="Enter Remark" MaxLength="500" Height="46px" Width="40%">
																								</asp:TextBox>
																							</td>
																						</tr>
																					</table>
																				</asp:PlaceHolder>
																			</fieldset>
																		</ContentTemplate>
																	</cc2:TabPanel>

																	<cc2:TabPanel ID="TabPanel2" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
																		<HeaderTemplate>
																			<asp:Label runat="server" Text="File Attachment" ID="Label12"></asp:Label>
																		</HeaderTemplate>
																		<ContentTemplate>
																			<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																				<%--   <legend id="Legend4" runat="server" style="font-weight: bold">File Attachment</legend>--%>
																				<table width="100%">
																					<tr>
																						<td style="width: 9px;"></td>
																						<td style="width: 127px;">
																							<span id="lblAttachFile" class="clsLabel">Attach File</span>
																						</td>
																						<td>
																							<table id="Table12" border="0">
																								<tr>
																									<td>
																										<asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
																											<ContentTemplate>
																												<table border="0" cellpadding="0" cellspacing="0">
																													<tr>
																														<td>
																															<input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
																																class="clsbtnH clsinfoH1" />
																														</td>
																														<td style="padding-left: 3px;">
																															<asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
																																Text="Remove Attachment" Enabled="False"></asp:Button>
																														</td>
																														<td style="padding-left: 2px;">
																															<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
																																Height="20px" Width="24px"></asp:ImageButton>
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
																				</table>
																			</fieldset>
																		</ContentTemplate>
																	</cc2:TabPanel>
																</cc2:TabContainer>
															</td>
														</tr>
														<tr>
															<td>
																<br />
															</td>
														</tr>
														<tr>
															<td colspan="2">
																<asp:Label ID="lblListInfo" runat="server" CssClass="clsLabelHeader">Log defect list</asp:Label>
															</td>

														</tr>
														<tr>
															<td colspan="1">
																<asp:DataGrid ID="dgLogDefectActions" runat="server" AutoGenerateColumns="False" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
																	CssClass="clsGridNewStyle" Width="97%">
																	<AlternatingItemStyle CssClass="clsdgAltItem" />
																	<ItemStyle CssClass="clsdgItem" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																	<Columns>
																		<asp:BoundColumn DataField="ID" HeaderText="ID " Visible="False"></asp:BoundColumn>
																		<asp:BoundColumn DataField="DefectNo" HeaderText="Defect No." ItemStyle-Wrap="false"></asp:BoundColumn>
																		<asp:BoundColumn DataField="DateOfOccurrenceFormatted" HeaderText="Date Of Occurrence">
																			<ItemStyle Wrap="False"></ItemStyle>
																		</asp:BoundColumn>
																		<asp:BoundColumn DataField="Defect" HeaderText="Defect">
																			<ItemStyle Wrap="true" />
																		</asp:BoundColumn>
																		<asp:BoundColumn DataField="MajorMinorTag" HeaderText="Major/Minor"></asp:BoundColumn>
																		<asp:BoundColumn DataField="InvestigationStatusTag" HeaderStyle-Wrap="true" HeaderText="Investigation Status"></asp:BoundColumn>
																		<asp:BoundColumn DataField="Action" HeaderText="Action"></asp:BoundColumn>
																		<asp:BoundColumn DataField="MELTag" HeaderText="Is MEL">
																			<ItemStyle Wrap="False" />
																		</asp:BoundColumn>
																		<asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																			<%--8--%>
																			<ItemTemplate>
																				<div class="dropdown">
																					<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																						Style="cursor: pointer;" />
																					<div class="dropdownbtn-content">
																						<table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
																							<tr>
																								<td>
																									<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, DataGridItem).ItemIndex %>' ToolTip="Click to Edit"
																										CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, DataGridItem).ItemIndex %>' ToolTip="Click to Delete"
																										CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="PrintRecord" runat="server" CommandArgument='<%# CType(Container, DataGridItem).ItemIndex %>' ToolTip="Click to Print"
																										CommandName="PrintRec" Style="height: 20px; width: 20px" ImageUrl="~/images/print.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, DataGridItem).ItemIndex %>' ToolTip="Click to View Attachment"
																										CommandName="ViewRec" Style="height: 20px; width: 17px" ImageUrl="icons/CLIP01.ICO"
																										Visible='<%#  Eval("IsAttachmentAdded")%>' />
																								</td>
																								<td>
																									<asp:ImageButton ID="CreateWO" runat="server" CommandArgument='<%# CType(Container, DataGridItem).ItemIndex %>' ToolTip="Work Order"
																										CommandName="CreateWORec" Style="height: 20px; width: 17px" ImageUrl="~/images/TaskCard.png"
																										Visible='<%# Not Eval("InvestigationStatus")%>' />
																								</td>
																							</tr>

																						</table>
																					</div>

																				</div>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
																		</asp:TemplateColumn>
																		<%-- <asp:ButtonColumn CommandName="Edit" HeaderText="Edit/View" Text="Edit/View"></asp:ButtonColumn>
                                                                        <asp:ButtonColumn CommandName="Delete" HeaderText="Delete" Text="Delete">
                                                                            <ItemStyle Width="75px" />
                                                                        </asp:ButtonColumn>
                                                                        <asp:ButtonColumn CommandName="Print" HeaderText="Print" Text="Print"></asp:ButtonColumn>--%>
																		<%--  <asp:TemplateColumn HeaderText="Attach">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Attach"
                                                                                Text="View">View</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                         
                                                                        <asp:ButtonColumn CommandName="Attach" HeaderText="View" Text="View">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonColumn>--%>
																		<%--10--%>
																		<asp:ButtonColumn DataTextField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"
																			HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:ButtonColumn>
																		<%--           <asp:BoundColumn DataField="ImageSize" HeaderText="Size" Visible="False"></asp:BoundColumn> --%>
																	</Columns>
																	<PagerStyle HorizontalAlign="Right" NextPageText="Next" PrevPageText="Prev" />
																</asp:DataGrid>
															</td>
														</tr>
														<tr>
															<td colspan="2" align="right">
																<table>
																	<tr>
																		<td align="right">
																			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
																				<ContentTemplate>
																					<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="Add"
																						CausesValidation="False" Style="display: none;"></asp:Button>
																					<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
																						CausesValidation="False" Style="display: none;"></asp:Button>
																					<asp:Button ID="hdnBtnAddWODetail" ClientIDMode="Static" runat="server" Text="Add"
																						CausesValidation="False" Style="display: none;"></asp:Button>

																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>

																	</tr>
																</table>
															</td>
														</tr>
														<tr style="height: 0px;">
															<td style="height: 0px;">
																<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
																	<ContentTemplate>
																		<td>
																			<asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
																				CausesValidation="False" Style="display: none;"></asp:Button>
																			<asp:Button ID="hdnimgBtnMELMasterChapter" ClientIDMode="Static" runat="server" Text="----"
																				CausesValidation="False" Style="display: none;"></asp:Button>
																			<asp:Button ID="hdnBtnMELDetail" ClientIDMode="Static" runat="server" Text="----"
																				CausesValidation="False" Style="display: none;"></asp:Button>
																		</td>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</tbody>
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
		<!-- Alert Message -->
		<a class="poplight" href="#?w=400" rel="popup_name"></a>
		<div id="popup_name" class="popup_block" align="center">
			<div style="width: 400px; height: auto" align="left">
				<table border="0" cellpadding="0">
					<tr>
						<td valign="middle" width="12%" align="left">
							<img src="images\alert_icon.png" width="40" height="40">
						</td>
						<td>
							<table>
								<tr>
									<td>
										<asp:Label ID="lblAlertTitle" runat="server" CssClass="clsTitleAlertLabel"></asp:Label>
									</td>
								</tr>
								<tr>
									<td>
										<asp:Label ID="lblAlertMessage" runat="server" CssClass="clsAlertLabel" Width="100%">
										</asp:Label>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
		</div>
		<!-- End-->
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				$("#<%=txtReportedBy.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
					width: 370,
					autoFill: false,
					matchContains: true,
					max: 30,
					delay: 0
				});
				$("#<%=txtActionTakenAganistEngStaff.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
					width: 370,
					autoFill: false,
					mustMatch: false,
					matchContains: true,
					max: 30,
					delay: 0
				});
			});
		</script>
		<%--Date Validations--%>
		<script type="text/javascript">

			//From Date -To Date validation
			function BetweenDatesValidation(source, args) {

				args.IsValid = false;
				var fromdate = $("#txtDateofoccurrence").val();
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
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForLogDefectAction();
				return false;
			}
		</script>
		<div>
			<%--UPDATEPANEL --%>
			<script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
				$(document).ready(function () {
					SetPageLayout();
					if ($.browser.msie) {
						parent.IFrameLogDefectActionStateComplete();
					}


				});
        <% End if %>
				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
				function endRequestHandler() {
					SetPageLayout();

				}

				function SetPageLayout() {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
					ReSetPageLayout();
					onResize();//for Top bottom link
                <% End if %>
				}
				function ReSetPageLayout() {
					$("body,html").css({ 'background-color': 'transparent' });
					var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
					var windowheight = $(window).height();
					if (tempMargtop >= windowheight) {
						$("body #tblmain:eq(0)").css({ 'margin': 'auto' });
					}
					else {
						var margintop = (windowheight / 2) - (tempMargtop / 2);
						$("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
					}

				}
			</script>
		</div>
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
						//                        $("#IFileUpload").ready(function () {
						//                            $("#btnDummyFileUpload").click();
						//                            $get("AjaxLoader").style.visibility = 'hidden';
						//                        });
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
		<!-- End -->
		<!-- MELMaster Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyMELMaster" Text="Dummy MELMaster" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupMELMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupMELMaster" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupMELMaster" runat="server" TargetControlID="btnDummyMELMaster"
			PopupControlID="pnlPopupMELMaster" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameMELMasterStateComplete() {
				$("#btnDummyMELMaster").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenMELMasterWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupMELMaster").attr("src", "wfMELSelectList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyMELMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunction() {
				var MELMasterwindow = $find("<%=mdlPopupMELMaster.ClientID %>");
				//close MELMaster popup window
				MELMasterwindow.hide();
				$("#iPopupMELMaster").attr("src", "JavaScript:''");
				//call MELMaster image button
				$("#hdnimgBtnMELMasterChapter").click();
			}
		</script>
		<!-- End -->
		<!-- MEL Detail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyMELDetail" Text="MEL Detail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlMELDetail" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeMELDetail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupMELDetail" runat="server" TargetControlID="btnDummyMELDetail"
			PopupControlID="pnlMELDetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameMELDetailStateComplete() {
				$("#btnDummyMELDetail").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenMELDetail() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeMELDetail").attr("src", "wfMELDetail_Ajax.aspx?Type=pup&OpenFrom=Snag");

					if (!$.browser.msie) {
						$("#btnDummyMELDetail").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForMELDetail() {
				var MELDetailwindow = $find("<%=mdlPopupMELDetail.ClientID %>");
				//close MEL Detail popup window
				MELDetailwindow.hide();
				//           release resources
				$("#IframeMELDetail").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnMELDetail").click();
			}
		</script>
		<!-- End-->
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
		<!-- Assembly Insp Maintenance Done By Employee Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
		</div>
		<asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
			PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameMaintDoneByStateComplete() {
				$("#btnDummyMaintDoneBy").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}


			function AddEmployeeLicNo() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=11"); //11=MelSnag

					if (!$.browser.msie) {
						$("#btnDummyMaintDoneBy").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}


			}

		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForMaintDoneBy() {
				var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
				//close Ass Insp Maint Done By Emp popup window
				MaintDoneBywindow.hide();
				//Free resources
				$("#IMaintDoneBy").attr("src", "JavaScript:''");
				$("#hdnBtnMaintDoneBy").click();

			}
		</script>
		<!-- End -->
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
			function CallParentFunction() {

				window.parent.autoResizeSnagReporting();
			}
		</script>
		<!---End-->

		<!-- WO Detail Popup Window Added By Saylee-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWODetail" Text="Dummy WODetail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupWODetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupWODetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupWODetail" runat="server" TargetControlID="btnDummyWODetail"
			PopupControlID="pnlPopupWODetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameWODetailStateComplete() {
				$("#btnDummyWODetail").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenToAddWODetail() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupWODetail").attr("src", "wfnWODetail_AJAX.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyWODetail").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForWODetail() {
				var WODetailWindow = $find("<%=mdlPopupWODetail.ClientID %>");
				//close WODetail popup window
				WODetailWindow.hide();
				$("#iPopupWODetail").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnBtnAddWODetail").click();
			}
		</script>
		<!-- End-->
	</form>
</body>
</html>

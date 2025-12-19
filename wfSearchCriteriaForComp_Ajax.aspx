<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForComp_Ajax.aspx.vb"
	Inherits="Flypal.wfSearchCriteriaForComp_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Component Status</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script id="clientEventHandlersJS" type="text/javascript">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfExportToExcel.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<style type="text/css">

		#divCollapsiblePnl {
			float: left;
			vertical-align: middle;
			width: 100%;
			cursor: pointer;
		}

		#lblTypeSelection {
			vertical-align: middle;
			margin-left: 2px;
			width: 100%;
		}

	</style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
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
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="5" class="clsFormHeader1">
										<span id="lbltitle" class="clsFormHeader">Search criteria for Component Status</span>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
											CssClass="clsValidationSummary"></asp:ValidationSummary>
										<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
											Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtAsOnDate"
											ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
											Display="None" ControlToValidate="txtAsOnDate" ErrorMessage="As On Date Required"></asp:RequiredFieldValidator>
										<asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
											ControlToValidate="cmbAircraft" ErrorMessage="Select the Aircraft" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<span id="lblStep1" class="clsLabelHeader">Selection of As On Date</span>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td>
															<table id="Table2" border="0" cellspacing="0" cellpadding="0">
																<tr>
																	<td></td>
																	<td width="80px">
																		<span id="lblFromDate" class="clsLabelAuto">As On Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtAsOnDate" runat="server" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
																			CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender')"></asp:TextBox>
																		<cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender ClientIDMode="static" TargetControlID="txtAsOnDate"
																			ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																	<td></td>
																	<td></td>
																</tr>
																<tr>
																	<td colspan="5" align="left" style="height: 22px">
																		<span id="lblStep2" class="clsLabelHeader">Selection of Aircraft</span>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<span id="lblAircraftStar1" class="clsLabelStar">*</span>
																	</td>
																	<td align="left">
																		<span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
																	</td>
																	<td align="left">
																		<asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																			DataTextField="RegNo" DataValueField="ID">
																		</asp:DropDownList>
																	</td>
																	<td align="left"></td>
																	<td align="left"></td>
																</tr>
																<tr>
																	<td colspan="5" align="left" style="height: 22px">
																		<span id="lblStep3" class="clsLabelHeader">Selection of Assembly</span>
																	</td>
																</tr>
																<tr>
																	<td align="left"></td>
																	<td align="left">
																		<span id="lblAssembly" class="clsLabelAuto">Assembly</span>
																	</td>
																	<td colspan="3" align="left">
																		<asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="ModelSerialNoPostion"
																			DataValueField="ID" AutoPostBack="True">
																		</asp:DropDownList>
																	</td>
																</tr>
																<tr>
																	<td colspan="3">
																		<span id="lblStep4" class="clsLabelHeader">Selection of ATA</span>
																	</td>
																</tr>
																<tr>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="lblATAChapter" class="clsLabel">ATA Chapter</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
																			DataValueField="ID" DataTextField="ATAChapter">
																		</asp:DropDownList>
																	</td>
																</tr>
																<tr>
																	<td colspan="5" align="left">
																		<span id="Label2" class="clsLabelHeader">Selection of Component</span>
																	</td>
																</tr>
																<tr>
																	<td align="left"></td>
																	<td align="left">
																		<span id="lblComponent" class="clsLabelAuto">Part No.</span>
																	</td>
																	<td align="left">
																		<asp:DropDownList ID="cmbComponent" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID"
																			DataTextField="Name" AutoPostBack="True">
																		</asp:DropDownList>
																	</td>
																	<td style="margin-left: 3px;">
																		<span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
																	</td>
																	<td align="left">
																		<asp:DropDownList ID="cmbSerialNo" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="CompID"
																			DataTextField="SerialNo">
																		</asp:DropDownList>
																	</td>
																	<td align="left"></td>
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
									<td colspan="5" align="left">
										<span id="lblStep5" class="clsLabelHeader">Select Type of Report</span>
									</td>
								</tr>
								<tr>

									<td colspan="5" align="left">
										<table id="Table3" border="0" cellspacing="0" cellpadding="0" width="100%">
											<tbody>
												<tr>
													<td align="left">
														<asp:RadioButton ID="optHardTimeStatus" runat="server" CssClass="clsRadioButton"
															onclick="ControlVisibilityForRadioBtns(this,'H')" ClientIDMode="Static" Text="Hard Time Components"
															Checked="True" GroupName="grOrientation"></asp:RadioButton>
													</td>
													<td></td>
													<td align="left">
														<asp:RadioButton ID="optCompStatus" runat="server" CssClass="clsRadioButton" ClientIDMode="Static"
															onclick="ControlVisibilityForRadioBtns(this,'C')" Text="All Components" GroupName="grOrientation"></asp:RadioButton>
													</td>
													<td style="width: 2px"></td>
													<td align="left">
														<asp:RadioButton ID="optSerializedComp" runat="server" CssClass="clsRadioButton"
															onclick="ControlVisibilityForRadioBtns(this,'S')" ClientIDMode="Static" Text="Serialized Components"
															GroupName="grOrientation"></asp:RadioButton>
													</td>
													<td></td>
													<td>
														<asp:RadioButton ID="optOCStatus" runat="server" CssClass="clsRadioButton" onclick="ControlVisibilityForRadioBtns(this,'O')"
															ClientIDMode="Static" Text="OC Components" GroupName="grOrientation"></asp:RadioButton>
													</td>
													<td></td>
													<td>
														<asp:RadioButton ID="optNavCompStatus" runat="server" CssClass="clsRadioButton" onclick="ControlVisibilityForRadioBtns(this,'O')"
															Visible='<%#IIf(AppSettings("ClientCode") = "KamAir", True, False) %>' ClientIDMode="Static"
															Text="Navigation Components" GroupName="grOrientation"></asp:RadioButton>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<span id="lblSortType" class="clsLabelHeader" style="display: none;">Selection
                                        of Sort Type</span> <span id="lblDueValue" class="clsLabelHeader">Selection
                                            of Assembly/Airframe Due Value</span>
									</td>
								</tr>
								<tr>

									<td colspan="5">
										<table>
											<tr>
												<td align="left" width="80px">
													<span id="lblSortBy" class="clsLabelAuto" style="display: none;">Sort Type</span>
												</td>
												<td colspan="3" align="left">
													<asp:DropDownList ID="cmbSortBy" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
														Style="display: none;">
														<asp:ListItem Value="0">Part No.</asp:ListItem>
														<asp:ListItem Value="1">Description</asp:ListItem>
													</asp:DropDownList>
												</td>
											</tr>
										</table>
										<asp:Panel ID="pnlDueButtons" runat="server">
											<table>
												<tr>
													<td>
														<asp:RadioButton ID="rdbAssemblyDue" runat="server" GroupName="a" CssClass="clsRadioButton"
															Text="Assembly Due Value" Checked="True" />
													</td>
													<td></td>
													<td>
														<asp:RadioButton ID="rdbAirframeDue" runat="server" GroupName="a" CssClass="clsRadioButton"
															Text="Airframe Due Value" />
													</td>
												</tr>
											</table>
										</asp:Panel>
									</td>
								</tr>
								<asp:PlaceHolder ID="phFormat" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
									<tr>
										<td colspan="5" align="left">
											<span id="lblFormatHeader" class="clsLabelHeader">Select Format of Report</span>
										</td>
									</tr>
									<tr>
										<td align="left"></td>
										<td colspan="4">
											<table>
												<tr>
													<td align="left">
														<span id="lblFormat" class="clsLabelAuto">Format</span>
													</td>
													<td align="left">
														<asp:DropDownList ID="cmbFormat" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
															onChange="ControlVisibilityForPerDayLimitByFormat();">
															<asp:ListItem Value="Format 1">Format 1</asp:ListItem>
															<asp:ListItem Value="Format 2">Format 2</asp:ListItem>
														</asp:DropDownList>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td colspan="5" align="left">
										<span id="lblType" class="clsLabelHeader">Selection of Type</span>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<table id="tlbTypeList" border="0" width="100%">
											<tr>
												<td width="225px">
													<table border="0" cellpadding="0" cellspacing="0" width="100%">
														<tr class="clsCollapsePnl">
															<td width="25px" style="border: 1px solid #ccc; border-right: none;">
																<asp:CheckBox Text="" ID="chkService" runat="server" onclick="ControlvisibilityForCheckboxlist(this,'chkListServiceType')" />
															</td>
															<td width="100%" style="border: 1px solid #ccc; border-left: none;">
																<asp:Panel ID="ClpnlService" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
																	<div id="divCollapsiblePnl">
																		<div style="float: left; vertical-align: middle;">
																			<span style="vertical-align: middle; margin-left: 2px;" id="lblTypeSelection" class="clsLabelHeader"><%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %></span>
																		</div>
																		<div style="float: right; vertical-align: middle; margin-right: 5px;">
																			<image id="imgService" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																		</div>
																	</div>
																</asp:Panel>
															</td>
														</tr>
													</table>
												</td>
												<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>

													<td width="225px">
														<table border="0" cellpadding="0" cellspacing="0" width="100%">
															<tr class="clsCollapsePnl">
																<td width="25px" style="border: 1px solid #ccc; border-right: none;">
																	<asp:CheckBox Text="" ID="chkInspection" runat="server" onclick="ControlvisibilityForCheckboxlist(this,'chkListInspectionType')" />
																</td>
																<td width="100%" style="border: 1px solid #ccc; border-left: none;">
																	<asp:Panel ID="ClpnlInspection" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
																		<div>
																			<div style="float: left; vertical-align: middle;">
																				<span style="vertical-align: middle; margin-left: 2px;" id="Span2" class="clsLabelHeader">Inspection</span>
																			</div>
																			<div style="float: right; vertical-align: middle; margin-right: 5px;">
																				<image id="imgInspection" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																			</div>
																		</div>
																	</asp:Panel>
																</td>
															</tr>
														</table>
													</td>
												</asp:PlaceHolder>
											</tr>
											<tr>
												<td width="242px" valign="top">
													<asp:Panel ID="pnlServiceType" runat="server" Style="max-height: 122px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
														<table id="Table5" cellpadding="0" cellspacing="0" border="0" width="100%" height="122px">
															<tr>
																<td valign="top">
																	<asp:CheckBoxList ID="chkListServiceType" ClientIDMode="Static" runat="server" CssClass="clsComboBox2_Ajax"
																		Style="padding-right: 12px; width: 100%;" DataTextField="CodeType" DataValueField="ID">
																	</asp:CheckBoxList>
																</td>
															</tr>
														</table>
													</asp:Panel>
												</td>
												<asp:PlaceHolder ID="phInspection" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
													<td width="242px" valign="top">
														<asp:Panel ID="pnlInspectionType" runat="server" Style="max-height: 122px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
															<table id="Table6" cellpadding="0" cellspacing="0" border="0" width="100%" height="122px">
																<tr>
																	<td valign="top">
																		<asp:CheckBoxList Style="width: 100%;" ClientIDMode="Static" ID="chkListInspectionType"
																			runat="server" CssClass="clsComboBox2_Ajax" DataTextField="CodeType" DataValueField="ID">
																		</asp:CheckBoxList>
																	</td>
																</tr>
															</table>
														</asp:Panel>
													</td>
												</asp:PlaceHolder>
											</tr>
										</table>
										<cc2:CollapsiblePanelExtender BehaviorID="clpServiceBehaviour" ID="clpServiceType"
											ClientIDMode="Static" runat="Server" TargetControlID="pnlServiceType" ExpandControlID="ClpnlService"
											CollapseControlID="ClpnlService" Collapsed="True" ImageControlID="imgService"
											ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
											CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
										<cc2:CollapsiblePanelExtender BehaviorID="clpInspectionBehaviour" ID="clpInspectionType"
											ClientIDMode="Static" runat="Server" TargetControlID="pnlInspectionType" ExpandControlID="ClpnlInspection"
											CollapseControlID="ClpnlInspection" Collapsed="True" ImageControlID="imgInspection"
											ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
											CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
									</td>
								</tr>
								<asp:PlaceHolder ID="phEstimatedFlyingHours" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
									<tr>
										<td colspan="5">
											<asp:UpdatePanel ID="upnlEstimatedFlyingHors" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td colspan="5">
																<asp:Label ID="Label7" runat="server" CssClass="clsLabelHeader" Style="display: none;">Estimated Flying Hours</asp:Label>
															</td>
														</tr>
														<tr>
															<td colspan="5">
																<asp:GridView ID="gdPerDayLimit" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal"
																	Style="display: none;">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																	<Columns>
																		<asp:BoundField DataField="PeriodID" HeaderText="PeriodID" Visible="False"></asp:BoundField>
																		<asp:BoundField DataField="PeriodName" HeaderText="Period">
																			<HeaderStyle HorizontalAlign="Left" />
																		</asp:BoundField>
																		<asp:TemplateField HeaderText="Limit" HeaderStyle-HorizontalAlign="Left">
																			<ItemTemplate>
																				<asp:TextBox ID="txtLimitPerDay" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
																				</asp:TextBox>
																			</ItemTemplate>
																		</asp:TemplateField>
																	</Columns>
																</asp:GridView>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="5" align="left">
											<span id="lblCMPRefHeader" class="clsLabelHeader">Enter CMP Reference</span>
										</td>
									</tr>
									<tr>
										<td align="left"></td>
										<td align="left">
											<span id="lblCMPREfLine" class="clsLabelAuto">CMP Reference</span>
										</td>
										<td colspan="3" align="left">
											<asp:TextBox ID="txtCMPRef" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Note" Width="370px"
												MaxLength="500"></asp:TextBox>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td colspan="5" align="left">
										<span id="lblBottomLineHeader" class="clsLabelHeader">Bottom Line of Report</span>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<span id="Label3" class="clsLabelAuto">Enter Line which you want to print at the bottom
                                        of the report.</span>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultilineDefectActionAuto"
											Width="552px" ToolTip="Enter Note" TextMode="MultiLine" MaxLength="500">I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________</asp:TextBox>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<span id="lblDisplayReport" class="clsLabelHeader">Display Report</span>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="left">
										<span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
											<ContentTemplate>
												<table border="0" cellpadding="0" cellspacing="0" width="100%">
													<tr>
														<td align="left" width="0px"></td>
														<td colspan="2" align="left">
															<asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>&nbsp;
														</td>
														<td colspan="2" align="left">
															<asp:Label ID="lblReportType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
														</td>
													</tr>
													<tr>
														<td align="left"></td>
														<td colspan="2" align="left">
															<asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto"></asp:Label>
														</td>
														<td colspan="2" align="left"></td>
													</tr>
													<tr>
														<td align="left" width="0px"></td>
														<td colspan="2" align="left">
															<asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
														</td>
														<td colspan="2" align="left">
															<asp:Label ID="lblComponent1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>
															<asp:Label ID="lblATAChapter1" runat="server" CssClass="clsLabelAuto"></asp:Label>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="5" align="right">
										<asp:Panel ID="pnlButton" runat="server">
											<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
												<ContentTemplate>
													<table cellspacing="0">
														<tr>
															<td>
																<asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
																	Text="Current Criteria" ToolTip="Click to display current searching criterias"
																	CausesValidation="False"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
																	TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report" Width="140px"
																	Visible="<%$AppSettings:ShowExportToExcelButton%>" />
															</td>
															<td>
																<asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH"
																	Text="Display" ToolTip="Click to display report"></asp:Button>
															</td>
															<%-- 'Added by Shital on 14-Sep-2016--%>
															<td>
																<asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" Text="Report By Mail"
																	ToolTip="Click to receive Report through mail" Width="140px" />
															</td>
															<td>
																<asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
																	ToolTip="Click to Close Search criteria for Component Status screen" CausesValidation="False"></asp:Button>
															</td>
														</tr>
														<!-- Dummy panel to open modelpopup 'Added by Shital on 14-Sep-2016 -->
														<tr style="height: 0px;">
															<td style="height: 0px;" colspan="2" align="right">
																<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
																	<ContentTemplate>
																		<asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
																			CausesValidation="False" Style="display: none;"></asp:Button>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<!--End -->
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</asp:Panel>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
			<asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnReportType" />
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
		</div>
		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'true' };
				$.ajax({
					type: "POST",
					url: "DateValidationHandler.ashx",
					//        contentType: "application/json",
					cache: false,
					data: params,
					async: false,
					beforeSend: OnBeforeSend,
					//                beforeSend: function (xhr, settings) {
					//                    $("[id$=processing]").dialog();
					//                },
					success: onSuccess,
					error: onError
				});

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

			//Service/inspection list checking
			function ControlvisibilityForCheckboxlist(elem, childid) {
				//if selected then enable and select checkboxlist else uncheck and disable list
				var status = $(elem).attr('checked');
				if (status == "checked") {
					$('#' + childid).removeAttr('disabled');
				}
				else {
					$('#' + childid).attr('disabled', 'disabled');
				}

				$('#' + childid).find(":checkbox").each(function () {
					if (status == "checked") {
						$(this).attr("checked", status);
						$(this).removeAttr('disabled');
					}
					else {
						$(this).removeAttr("checked");
						$(this).attr('disabled', 'disabled');
					}
				});
			}

			//Control visibility for Format
			function ControlVisibilityForPerDayLimitByFormat() {

				var Index = $get("cmbFormat").selectedIndex;
				if (Index == 1) {
					$("#lblBottomLineHeader").text("Bottom Line of Report");
					$("#lblDisplayReport").text("Display Report");
					$("#Label7").css('display', 'block');
					$("#Label7").text("Estimated Flying Hours");
					$("#lblCMPRefHeader").text("CMP Reference");
					$("#gdPerDayLimit").css('display', 'block');
				}
				else {
					$("#lblCMPRefHeader").text("CMP Reference");
					$("#lblBottomLineHeader").text("Bottom Line of Report");
					$("#lblDisplayReport").text("Display Report");
					$("#Label7").css('display', 'none');
					$("#gdPerDayLimit").css('display', 'none');
				}
			}
			//Control visibility for radio buttons

			function ControlVisibilityForRadioBtns(elem, type) {
				switch (type) {
					case 'H': //Hard Time Components
						var status = $(elem).attr('checked');
						if (status) {
							$("#lblBottomLineHeader").text("Bottom Line of Report");
							$("#lblDisplayReport").text("Display Report");
							$("#lblFormatHeader").css('display', 'block');
							$("#lblFormat").css('display', 'block');
							$("#cmbFormat").css('display', 'block');
							$("#lblType").css('display', 'block');
							$("#lblSortType").css('display', 'none');
							$("#lblSortBy").css('display', 'none');
							$("#cmbSortBy").css('display', 'none');
							$("#tlbTypeList").css('display', 'block');
							$("#lblDueValue").css('display', 'block');
							$("#pnlDueButtons").css('display', 'block');

							$("#lblCMPRefHeader").css('display', 'block');
							$("#lblCMPREfLine").css('display', 'block');
							$("#txtCMPRef").css('display', 'block');

							ControlVisibilityForPerDayLimitByFormat();
							break;
						}
					case 'C': //All componentes

						var status = $(elem).attr('checked');
						if (status) {
							$("#lblFormatHeader").css('display', 'none');
							$("#lblFormat").css('display', 'none');
							$("#cmbFormat").css('display', 'none');
							$("#lblType").css('display', 'none');
							$("#lblSortType").css('display', 'none');
							$("#lblSortBy").css('display', 'none');
							$("#cmbSortBy").css('display', 'none');
							$("#tlbTypeList").css('display', 'none');
							$("#lblDueValue").text("Selection of Assembly/Airframe Due Value ");
							$("#lblBottomLineHeader").text("Bottom Line of Report");
							$("#lblDisplayReport").text("Display Report");
							$("#Label7").css('display', 'none');
							$("#gdPerDayLimit").css('display', 'none');
							$("#lblDueValue").css('display', 'block');
							$("#pnlDueButtons").css('display', 'block');

							$("#lblCMPRefHeader").css('display', 'none');
							$("#lblCMPREfLine").css('display', 'none');
							$("#txtCMPRef").css('display', 'none');

							break;
						}

					case 'S': //Serialized componentes
						var status = $(elem).attr('checked');
						if (status) {
							$("#lblFormatHeader").css('display', 'none');
							$("#lblFormat").css('display', 'none');
							$("#cmbFormat").css('display', 'none');
							$("#lblType").css('display', 'none');
							$("#lblSortType").css('display', 'block');
							$("#lblSortBy").css('display', 'block');
							$("#cmbSortBy").css('display', 'block');
							$("#tlbTypeList").css('display', 'none');
							$("#lblBottomLineHeader").text("Bottom Line of Report");
							$("#lblDisplayReport").text("Display Report");
							$("#Label7").css('display', 'none');
							$("#gdPerDayLimit").css('display', 'none');
							$("#lblDueValue").css('display', 'none');
							$("#pnlDueButtons").css('display', 'none');
							$("#lblCMPRefHeader").css('display', 'none');
							$("#lblCMPREfLine").css('display', 'none');
							$("#txtCMPRef").css('display', 'none');
							break;
						}

					case 'O': //OC components
						var status = $(elem).attr('checked');
						if (status) {
							$("#pnlDueButtons").css('display', 'block');
							$("#lblDueValue").css('display', 'block');
							$("#lblFormatHeader").css('display', 'none');
							$("#lblFormat").css('display', 'none');
							$("#cmbFormat").css('display', 'none');
							$("#lblType").css('display', 'none');
							$("#tlbTypeList").css('display', 'none');
							$("#lblCMPRefHeader").css('display', 'none');
							$("#lblCMPREfLine").css('display', 'none');
							$("#txtCMPRef").css('display', 'none');
							$("#lblBottomLineHeader").text("Bottom Line of Report");
							$("#lblDisplayReport").text("Display Report");
							$("#lblSortType").css('display', 'none');
							$("#lblSortBy").css('display', 'none');
							$("#cmbSortBy").css('display', 'none');
							break;
						}
				}

			}

			//Aircraft validation
			function ValidateAircraft(source, args) {
				args.IsValid = false;
				var dd = $get("cmbAircraft");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;

				}

			}
		</script>
		<!-- Popup For Report By Mail 14-Sep-2016 -->
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
				$("#hdnimgLogBtnSendMail").click();
			}
		</script>
		<!---End-->
	</form>
</body>
</html>

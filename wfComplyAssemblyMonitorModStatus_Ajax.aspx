<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyAssemblyMonitorModStatus_Ajax.aspx.vb"
	Inherits="Flypal.wfComplyAssemblyMonitorModStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Comply Assembly Directives Status</title>
		
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" type="text/css" href="popup.css" />	
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AlertMessage1.1.js"></script>
	<script type="text/javascript">
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>

	<style type="text/css">
		.clsCursorStyle {
			cursor: pointer;
		}
	</style>

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<%--<div>--%>
		<table border="0" id="tblMain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblinner" class="clstablelistin">
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Comply Assembly Directive Status</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
												ErrorMessage="Remark too long." Display="None" ControlToValidate="txtRemark"></asp:CustomValidator>
											<asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Enter correct License No"
												ControlToValidate="txtLicenceNo" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvCurrentValue" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate1"
												Display="None" ControlToValidate="txtLicenceNo"></asp:CustomValidator>
											<asp:CustomValidator ID="cvExtensionValue" runat="server" OnServerValidate="CustomValidate1"
												Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
										<legend id="Legend1" runat="server"><b>Monitoring Details</b></legend>
										<table border="0" id="Table3" class="clsTable1" cellpadding="0" width="100%">
											<tr>
												<td style="width: 10px;">
													<span id="L1" class="clsLabel"></span>
												</td>
												<td style="width: 115px;">
													<span id="lblMonitorModType" class="clsLabelAuto">Directive Type </span>
												</td>
												<td style="padding-left: 2px;">
													<asp:TextBox ID="txtModelMonitorModTypeName" runat="server" CssClass="clsTextBox_Ajax"
														BackColor="#E0E0E0" Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.ModelMonitorModTypeName %>"
														ReadOnly="True" ToolTip="Directive Type">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="lblMonitorType" class="clsLabel">Monitor Type </span>
												</td>
												<td style="padding-left: 2px;">
													<asp:TextBox ID="txtMonitorType" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
														Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeName %>" ReadOnly="True"
														ToolTip="Monitor Type">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="lblATAChapter" class="clsLabel">ATA Chapter </span>
												</td>
												<td style="padding-left: 2px;">
													<asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
														Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.ATAChapter %>" ReadOnly="True"
														ToolTip="ATA Chapter">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="lblReference" class="clsLabel">Reference </span>
												</td>
												<td style="padding-left: 2px;">
													<asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
														Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.Reference %>" ReadOnly="True"
														ToolTip="Reference">
													</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<asp:Label ID="lblModNumber" runat="server" CssClass="clsLabel">Directive Number</asp:Label>
												</td>
												<td align="left">
													<asp:TextBox ID="txtModNumber" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Directive Number"
														ReadOnly="True" BackColor="#E0E0E0" Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.Number %>"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="lblDescription" class="clsLabel">Description </span>
												</td>
												<td style="padding-left: 2px;">
													<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
														Width="250px" BackColor="#E0E0E0" Text="<%# mAssemblyMonitorModStatus.ModelMonitorMod.Description %>"
														ReadOnly="True" ToolTip="Description" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
												</td>
											</tr>
										</table>
									</fieldset>
								</td>
								<td valign="top">
									<asp:UpdatePanel ID="upnlCurrentValueGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
												<legend id="Legend2" runat="server"><b>Elapsed and Remaining Values</b></legend>
												<table>
													<tr>
														<td>
															<asp:GridView ID="dgCurrentValue" runat="server" CssClass="clsGridLog" PageSize="3"
																ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle CssClass="clsdgHeader" />
																<Columns>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="Id"></asp:BoundField>
																	<asp:BoundField DataField="PeriodUnitName" HeaderText="Period" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ElapsedValueFormatted" HeaderText="Elapsed" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<asp:BoundField DataField="RemainingValueFormatted" HeaderText="Remaining" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">Please Note: Elapsed and Remaining Values for
                                                                    Days/Months/Years will be in Days</asp:Label>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td></td>
								<td align="left"></td>
							</tr>
							<tr>
								<td valign="top">
									<asp:UpdatePanel ID="upnlMonitoringStatusDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
												<legend id="Legend3" runat="server"><b>Done On Details</b></legend>
												<table border="0" id="Table2" cellpadding="0" width="100%">
													<tr>
														<td colspan="3" align="right">
															<asp:UpdatePanel ID="upnlSelectLog" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton_Ajax" Text="Select Log"
																					ToolTip="Click to open Select Log screen"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td style="width: 10px;">
															<span id="Label4" class="clsLabelStar">*</span>
														</td>
														<td style="width: 115px;">
															<span id="lblDoneOn" class="clsLabel">Done On </span>
														</td>
														<td style="padding-left: 2px;">
															<table border="0" id="Table9" cellpadding="0">
																<tr>
																	<td>
																		<asp:TextBox runat="server" ID="txtDoneOnDate" CssClass="clsTextBox_Ajax" Width="100px"
																			AutoPostBack="true" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender','true');"></asp:TextBox>
																		<cc2:calendarextender id="txtDoneOnDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																			enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtDoneOnDate">
																		</cc2:calendarextender>
																		<cc2:textboxwatermarkextender targetcontrolid="txtDoneOnDate" id="DoneOnDate_watermarkextender"
																			clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>">
																		</cc2:textboxwatermarkextender>
																	</td>
																	<td></td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblWorkOrNo" class="clsLabel">Work Order No. </span>
														</td>
														<td style="padding-left: 2px;">
															<asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mAssemblyMonitorModStatus.DoneWoNo %>"
																ToolTip="Enter Work Order Number" MaxLength="100">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblLicenceNo" class="clsLabelAuto">License No.</span>
														</td>
														<td>
															<asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
																					AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNo_TextChanged"
																					AutoPostBack="true" MaxLength="200"></asp:TextBox>
																				<cc2:autocompleteextender clientidmode="Static" id="txtLicenceNo_Autocomplete" runat="server"
																					delimitercharacters="" enabled="True" completionsetcount="20" minimumprefixlength="0"
																					completioninterval="1" servicepath="wfComplyAssemblyMonitorModStatus_Ajax.aspx"
																					servicemethod="GetLicenseNoList" targetcontrolid="txtLicenceNo" usecontextkey="False"
																					contextkey="" completionlistcssclass="ac_results_Main" completionlistitemcssclass="ac_results_li"
																					completionlisthighlighteditemcssclass="ac_over_Main" onclientpopulated="ClientPopulated"
																					onclientpopulating="ClientPopulating" onclienthiding="ClientHiding" onclientshown="ClientHiding"
																					onclientshowing="ClientShowing">
																				</cc2:autocompleteextender>
																			</td>
																			<td>
																				<asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
																					Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
																			</td>
																		</tr>
																		<tr>
																			<td colspan="2">
																				<asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 %>"
																					ToolTip="<%# mAssemblyMonitorModStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblPlace" class="clsLabelAuto">Place</span>
														</td>
														<td style="padding-left: 2px;">
															<asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mAssemblyMonitorModStatus.Place %>"
																ToolTip="Enter Place" MaxLength="25">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblRequiredmanHours" class="clsLabelAuto">Actual Man Hours</span>
														</td>
														<td style="padding-left: 2px;">
															<asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxSmall_Ajax"
																ToolTip="Enter Actual Man Hours" Text="<%# mAssemblyMonitorModStatus.TotalReqManHrs1 %>"
																Enabled="<%# mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count <= 1 %>"
																OnTextChanged="txtRequiredManHours_TextChanged" AutoPostBack="true" MaxLength="8">
															</asp:TextBox>
															<asp:Label ID="lblEstdManHours" runat="server" CssClass="clsLabelHeader" ToolTip="Estd. Man Hours">
															</asp:Label>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblMethodOfCompliance" class="clsLabel">
																Method Of Compliance
															</span>
														</td>
														<td style="padding-left: 2px;">
															<asp:TextBox ID="txtMethodOfCompliance" runat="server"
																CssClass="clsTextBoxMultiLine1_Ajax" Width="250px"
																Text="<%# mAssemblyMonitorModStatus.MethodOfCompliance %>"
																ToolTip="Enter Method Of Compliance" TextMode="MultiLine">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblRemark" class="clsLabel">Remark </span>
														</td>
														<td style="padding-left: 2px;">
															<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" Width="250px"
																Text="<%# mAssemblyMonitorModStatus.DoneRemark %>" ToolTip="Enter Remark" TextMode="MultiLine">
															</asp:TextBox>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td valign="top">
									<asp:UpdatePanel ID="upnlDoneOnValueGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset4" class="clsFieldSet" style="border-width: 1px;">
												<legend id="lblAssemblyValues" runat="server" style="font-weight: bold;">Airframe Values</legend>
												<table>
													<tr>
														<td>
															<asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGridLog" PageSize="3"
																AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle CssClass="clsdgHeader" />
																<Columns>
																	<asp:BoundField DataField="PeriodUnitNameForDate" HeaderText="Period">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderText="Done On" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
																		<ItemStyle Wrap="False"></ItemStyle>
																		<ItemTemplate>
																			<asp:TextBox ID="txtCurrentValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
																				AutoPostBack="true" OnTextChanged="txtCurrentValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentValueFormatted") %>'
																				ClientIDMode="Static">
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:TemplateField HeaderText="Extension" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
																		<ItemTemplate>
																			<asp:TextBox ID="txtExtensionValue" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
																				AutoPostBack="true" OnTextChanged="txtExtensionValue_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem,"ExtensionValueFormatted") %>'
																				ToolTip="Enter the Extension Value.">
																			</asp:TextBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																	<asp:BoundField DataField="AssemblyDueOnValueFormattedByAirFrame" HeaderText="Due At Airframe">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" />
																	</asp:BoundField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblNote1" runat="server" CssClass="clsLabelHeader" Width="505px">Please Note: Started On/Current Values/Due
                                                                    On values for Days/Months/Years will be in Dates. Extension Value for Calendar period
                                                                    should be entered in Days only.</asp:Label>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<asp:UpdatePanel ID="upnlDocumentDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset5" class="clsFieldSet" style="border-width: 1px;">
												<legend id="Legend5"><b>Document Details</b></legend>
												<table id="Table5" border="0" cellpadding="0" width="100%">
													<tr>
														<td style="width: 10px;"></td>
														<td style="width: 115px;">
															<span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
														</td>
														<td style="padding-left: 3px;">
															<asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
																Text="<%# mAssemblyMonitorModStatus.RevisionNo %>" ToolTip="Enter Revision No."></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblPageNo" class="clsLabel">Page No.</span>
														</td>
														<td style="padding-left: 3px;">
															<asp:TextBox ID="txtPageNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
																Text="<%# mAssemblyMonitorModStatus.PageNo %>" ToolTip="Enter Page No.">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblBookNo" class="clsLabel">Book No.</span>
														</td>
														<td style="padding-left: 3px;">
															<asp:TextBox ID="txtBookNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
																Text="<%# mAssemblyMonitorModStatus.BookNo %>" ToolTip="Enter Book No.">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
															<span id="lblSourceDoc" class="clsLabel">Source Doc</span>
														</td>
														<td style="padding-left: 3px;">
															<asp:TextBox ID="txtSourceDoc" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
																MaxLength="500" Text="<%# mAssemblyMonitorModStatus.SourceDoc %>" TextMode="MultiLine"
																ToolTip="Enter Source Doc." Width="250px"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td></td>
														<td>
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
																							<input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
																								runat="server" class="clsButton_Ajax" />
																						</td>
																						<td style="padding-left: 3px;">
																							<asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
																								Text="Remove Attachment" Enabled="False" Width="120px"></asp:Button>
																						</td>
																						<td style="padding-left: 2px;">
																							<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
																								Height="20px" Width="20px"></asp:ImageButton>
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
									</asp:UpdatePanel>
								</td>
								<td valign="top">
									<asp:UpdatePanel ID="upnlExtensionDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="fdsExtensionDetails" class="clsFieldSet" style="border-width: 1px;">
												<legend id="Legend4"><b>Extension Details</b></legend>
												<table id="Table51" border="0" cellpadding="0" width="100%">
													<tr>
														<td>
															<span id="lblExtensionDate" class="clsLabelAuto" visible="<%# Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3 %>">Extension Date</span>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtExtensionDate" CssClass="clsTextBox_Ajax" Width="100px"
																onchange="ValidateDateText(this,'ExtensionDate_watermarkextender','false');"></asp:TextBox>
															<cc2:calendarextender id="txtExtensionDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtExtensionDate">
															</cc2:calendarextender>
															<cc2:textboxwatermarkextender targetcontrolid="txtExtensionDate" id="ExtensionDate_watermarkextender"
																clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>">
															</cc2:textboxwatermarkextender>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblApprovalRemark" class="clsLabelAuto" visible="<%# Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3 %>">Approval Remark</span>
														</td>
														<td>
															<asp:TextBox ID="txtApprovalRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
																MaxLength="500" Text="<%# mAssemblyMonitorModStatus.ApprovalRemark %>" TextMode="MultiLine"
																Visible="<%# Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3%>"
																ToolTip="Enter Approval Remark">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:UpdatePanel ID="upnlRevisedDetails" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset id="fdsRevisedDetails" class="clsFieldSet" style="border-width: 1px;">
																		<legend id="Legend6"><b>Revised Details</b></legend>
																		<table>
																			<tr>
																				<td colspan="2">
																					<asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Text="Applicable"
																						ToolTip="Check to apply applicable" Checked="<%# mAssemblyMonitorModStatus.IsApplicable %>"></asp:CheckBox>
																					<span class="clsLabelHeader">(Un-check if not required to be monitored from now onwards..)</span>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="Span1" class="clsLabel">Do you want to Revise this Activity?</span>
																				</td>
																				<td>
																					<asp:Button ID="btnRevise" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Revise Assembly Inspection"
																						Text="Yes"></asp:Button>
																				</td>
																			</tr>
																		</table>
																	</fieldset>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlLinkMaintenance" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="pnlInner" runat="server" Width="100%" Visible="false" CssClass="clsCalPanel">
												<table id="tbllinkMaint" class="clsTablelistin" border="0" cellspacing="3" cellpadding="0">
													<tr>
														<td>
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:GridView ID="dgMultiComplianceList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
																ShowHeaderWhenEmpty="true" PageSize="3" AllowSorting="True">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle CssClass="clsdgHeader" />
																<Columns>
																	<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
																		<ItemTemplate>
																			<asp:CheckBox ID="chkSelect" runat="server" CssClass="clsLabelAuto" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField DataField="MaintenanceActivityName" SortExpression="MaintenanceActivityName"
																		HeaderText="Maintenance Activity">
																		<HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="MaintenanceActionName" SortExpression="MaintenanceActionName"
																		HeaderText="Action">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="MonitorType" SortExpression="MonitorType" HeaderText="Monitor Info">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Type">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="MaintenanceOn" SortExpression="MaintenanceOn"
																		HeaderText="Maintenance On">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
																		HeaderText="Aircraft Info">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
																		HeaderText="Assembly Type">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
																		HeaderText="Assembly Info">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
																		<HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="DirectiveNumber" SortExpression="DirectiveNumber"
																		HeaderText="Directive Number">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On" HtmlEncode="false">
																		<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnWONO" SortExpression="DoneOnWONO" HeaderText="Work Order No.">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="InstalledOnFormatted" HeaderText="Installed On">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="RemovedOnFormatted" HeaderText="Removed On"></asp:BoundField>
																	<asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValue" SortExpression="FrequencyValue" HeaderText="Frequency"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="RemovalReasonName" SortExpression="RemovalReasonName"
																		HeaderText="Removal Reason">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField Visible="False" DataField="DoneRemark" SortExpression="DoneRemark"
																		HeaderText="Comply Remark">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnValue" SortExpression="DoneOnValue" HeaderText="Done On Value"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="CurrentValue" SortExpression="CurrentValue" HeaderText="Current"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ElapsedValue" SortExpression="ElapsedValue" HeaderText="Elapsed"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExtensionValue" SortExpression="ExtensionValue" HeaderText="Extension"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DueOnValue" SortExpression="DueOnValue" HeaderText="Due At"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="RemainingValue" SortExpression="RemainingValue" HeaderText="Remaining"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:ButtonField Visible="False" Text="Remove" HeaderText="Remove" CommandName="Remove"
																		HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
												</table>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>&nbsp;&nbsp;
									<asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton" 
										Enabled="<%# Not mAssemblyMonitorModStatus.IsNew %>"
										Font-Italic="true" Font-Size="8pt">View Log Book Entry</asp:LinkButton>
									&nbsp;
								</td>
								<td align="right">
									<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table border="0" id="Table1" cellspacing="0">
												<tr>
													<td>
														<asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save Assembly Directive"></asp:Button>
													</td>
													<td>
														<asp:UpdatePanel ID="upnlMailTool" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Button ID="btnSendMail" runat="server" CssClass="clsButton_Ajax" Text="Send Mail"
																	ToolTip="Click to send Mail to if Directive is revised"></asp:Button>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td>
														<asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to print Assembly Directive"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to previous page"
															CausesValidation="False"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<!--Dummy panel to open modelpopup-->
							<tr style="height: 0px;">
								<td style="height: 0px;">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
										<ContentTemplate>
											<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;"></asp:Button>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<!--End -->
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>

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


		<asp:HiddenField ID="hdnLicenceNo" runat="server" ClientIDMode="Static" />
		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupFileUpload" runat="server" targetcontrolid="btnDummyFileUpload"
			popupcontrolid="pnlFileUpload" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>

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
		</script>
		<!-- End -->


		<!-- SelectSelectLog popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySelectLog" Text="Select Log" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupSelectLog" runat="server" targetcontrolid="btnDummySelectLog"
			popupcontrolid="pnlSelectLog" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>

		<script type="text/javascript">

			function IFrameSelectLogStateComplete() {
				$("#btnDummySelectLog").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenSelectLogWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeSelectLog").attr("src", "wfSelectLog_Ajax.aspx?Type=pup");
					$("#btnDummySelectLog").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForSelectLog() {
				var SelectLogwindow = $find("<%=mdlPopupSelectLog.ClientID %>");
					//close Task Card Tool popup window
					SelectLogwindow.hide();
					//           release resources
					$("#IframeSelectLog").attr("src", "JavaScript:''");
					//call image button
					$("#hdnBtnSelectLog").click();
			}

		</script>
		<!-- End-->


		<!-- Assembly Mod Maintenance Done By Employee Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
		</div>
		<asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupMaintDoneBy" runat="server" targetcontrolid="btnDummyMaintDoneBy"
			popupcontrolid="pnlMaintDoneBy" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>

		<script type="text/javascript">
			function IFrameMaintDoneByStateComplete() {
				$("#btnDummyMaintDoneBy").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}


			function AddEmployeeLicNo() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=7");

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
				//close Ass Mod Maint Done By Emp popup window
				MaintDoneBywindow.hide();
				//Free resources
				$("#IMaintDoneBy").attr("src", "JavaScript:''");
				$("#hdnBtnMaintDoneBy").click();

			}
		</script>
		<!-- End -->


		<!--Model Mod Master Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelModMaster" Text="Model Mod Master" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelModMaster" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelModMaster" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupModelModMaster" runat="server" targetcontrolid="btnDummyModelModMaster"
			popupcontrolid="pnlModelModMaster" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		
		<script type="text/javascript">
			function IFrameModelModMasterStateComplete() {
				$("#btnDummyModelModMaster").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelModMasterWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelModMaster").attr("src", "wfModelMonitorMod_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

					if (!$.browser.msie) {
						$("#btnDummyModelModMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForModelModMaster() {
				var ModelModMasterwindow = $find("<%=mdlPopupModelModMaster.ClientID %>");
				//close Model Mod Master popup window
				ModelModMasterwindow.hide();
				//           release resources
				$("#IframeModelModMaster").attr("src", "JavaScript:''");
				//call Model Mod Master image button
				$("#hdnBtnModelModMaster").click();
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


		<!-- Popup For Report By Mail -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupReceipt1" runat="server" targetcontrolid="btnDummyReceipt1"
			popupcontrolid="pnlReceipt1" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		
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
		</script>
		<!---End-->


		<%--Date Validations--%>
		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid, TobeReset) {

				var datevalue = $(elem).val();
				var resetTodaysDate = TobeReset;
				var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
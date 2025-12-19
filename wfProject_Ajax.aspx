<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfProject_Ajax.aspx.vb"
	Inherits="Flypal.ProjectDetails" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Project Detail</title>
	<link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>

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
		<script type="text/javascript">

			window.onload = blinkProjectStatus;

			function blinkProjectStatus() {

				var e = document.getElementById("<%=lblProjectStatus.ClientID%>");
				e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
				setTimeout("blinkProjectStatus();", 750);
			}

		</script>

		<table class="clstablelistout" id="Table-MaxWidth">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblInner" class="clstablelistin">
							<tr id="ActionButtons">
								<td class="clsFormHeader1Newstyle" colspan="2">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" 
															CssClass="clsFormHeader" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnPrint" runat="server"
																		Text="Print" class="clsbtnH clsinfoH"
																		ToolTip="Print the Project Detail Report."
																		Visible="<%# (Not Project.IsNew) %>" />

																	<asp:Button ID="btnSubmit" runat="server"
																		Text="Submit" class="clsbtnH clsinfoH"
																		ToolTip="Submit the Project."
																		Visible="<%# (Not Project.IsNew) AndAlso (Project.StatusID = 1) %>" />

																	<asp:Button ID="btnComplete" runat="server"
																		Text="Complete" class="clsbtnH clsinfoH"
																		ToolTip="Compelete the Project."
																		CausesValidation="true" />

																	<asp:Button ID="btnSave" runat="server"
																		Text="Save" class="clsbtnH clsinfoH"
																		ToolTip="Save Project Details." ValidationGroup="ProjectDetails" />

																	<asp:Button ID="btnClose" runat="server"
																		Text="Close" class="clsbtnH clsinfoH"
																		ToolTip="Close Project Detail screen." />
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
							<tr id="ValidationSummary">
								<td>
									<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="ProjectDetailsValidationsummary" 
												runat="server" CssClass="clsValidationSummary"
												ValidationGroup="ProjectDetails"
												HeaderText="Fill Up The Following Fields." />
											<asp:CustomValidator ID="CustValidator" runat="server"
												OnServerValidate="CustomValidation"
												ValidationGroup="ProjectDetails" 
												ErrorMessage="Project Date is Required."
												ControlToValidate="txtProjectDate" 
												Display="None" CssClass="clsValidationSummary" />
											<asp:RequiredFieldValidator
												ID="rfvDate" runat="server" Display="None" 
												ErrorMessage="Date Required."
												ValidationGroup="ProjectDetails" 
												ControlToValidate="txtProjectDate" 
												CssClass="clsValidationSummary" />
											<asp:RequiredFieldValidator
												ID="rfvPlanName" runat="server" Display="None" 
												ErrorMessage="Project Text Required."
												ValidationGroup="ProjectDetails" 
												ControlToValidate="txtProjectText" 
												CssClass="clsValidationSummary" />
											<asp:CustomValidator ID="cvEmployee"
												runat="server" ValidationGroup="ProjectDetails" Display="None"
												ControlToValidate="cmbEmployee" 
												ErrorMessage="Please Select Employee." 
												CssClass="clsValidationSummary" />
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td align="right">
									<asp:UpdatePanel runat="server" ID="upnlProjectStatus" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblProjectStatus" runat="server"
												Text="<%# Project.StatusName %>"
												CssClass="clsLabelHeader" Font-Size="Small">
											</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr id="ProjectDetails">
								<td valign="top">
									<asp:UpdatePanel runat="server" ID="upnlProjectDetails" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="pnlProjectDetails" runat="server" CssClass="clsPanel1">
												<table width="100%">
													<tr>
														<td valign="top">
															<fieldset id="fdsProjectDetails" class="clsFieldSetNewStyle">
																<legend id="ledProjectDetails" class="clsFieldSet1">
																	<b>
																		<asp:Label ID="lblProjectDetailHeader" runat="server" 
																			CssClass="clsLabelHeader" Text="Details" />
																	</b>
																</legend>
																<table width="100%">
																	<tr>
																		<td>
																			<span id="lblStarDate" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<span id="lblDate" class="clsLabel">Date</span>
																		</td>
																		<td>
																			<asp:TextBox runat="server" ID="txtProjectDate"
																				CssClass="clsTextBoxTagDateSearch" Width="100px"
																				AutoComplete="off" Text=""
																				onchange="ValidateDateText(this,'txtProjectDate_watermarkextender');" />
																			<cc2:CalendarExtender ID="txtProjectDate_CalendarExtender"
																				runat="server" CssClass="cal_Theme1"
																				Enabled="true" Format="<%$AppSettings:DateFormat%>"
																				TargetControlID="txtProjectDate" />
																			<cc2:TextBoxWatermarkExtender TargetControlID="txtProjectDate"
																				ID="txtProjectDate_watermarkextender"
																				ClientIDMode="Static" runat="server"
																				WatermarkText="<%$AppSettings:DateFormat%>" />
																		</td>
																		<td>
																			<span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
																		</td>
																		<td>

																			<span id="lblNo" class="clsLabel">No.</span>
																		</td>
																		<td>
																			<asp:TextBox ID="txtProjectText" runat="server"
																				CssClass="clsTextBoxTagSearch" MaxLength="50"
																				Text="<%# Project.Text %>" ToolTip="Enter No."
																				Width="130px" />
																			<cc2:AutoCompleteExtender ID="txtProjectText_Autocomplete" runat="server"
																				ClientIDMode="Static" CompletionInterval="1" CompletionSetCount="20"
																				DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
																				ServiceMethod="GetDistinctTextListAutoComplete" ServicePath="wfProject_Ajax.aspx"
																				TargetControlID="txtProjectText" UseContextKey="False">
																			</cc2:AutoCompleteExtender>

																			<asp:TextBox ID="txtProjectNo" runat="server"
																				CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																				MaxLength="8" Text="<%# Project.No %>" Enabled="false" />
																		</td>
																		<td></td>
																		<td></td>
																		<td></td>
																	</tr>
																	<tr>
																		<td>
																			<asp:Label ID="llblRegNoStar" class="clsLabelStar" runat="server"
																				Visible='<%#IIf(Project.TransTypeID = 101, True, False) %>' 
																				Text="*" />
																		<td>
																			<asp:Label ID="lblRegNo" runat="server"
																				CssClass="clsLabel" Text="Reg. No." />
																		</td>
																		<td>
																			<asp:UpdatePanel runat="server" ID="upnlAircraftList" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phRegNoTextbox" runat="server"
																						Visible='<%#IIf(Project.TransTypeID = 104, True, False) %>'>
																						<asp:TextBox ID="txtRegNo" TabIndex="26" runat="server" 
																							CssClass="clsTextBoxTagSearch"
																							Text="<%# Project.RegNo %>" ToolTip="Enter Reg. No." 
																							autocomplete="off" />
																						<cc2:AutoCompleteExtender ClientIDMode="Static"
																							ID="AutoCompleteExtender1"
																							runat="server" DelimiterCharacters="" Enabled="True"
																							MinimumPrefixLength="0"
																							CompletionInterval="1000" ServicePath="wfProject_Ajax.aspx"
																							ServiceMethod="GetRegTextList" TargetControlID="txtRegNo" 
																							UseContextKey="True"
																							ContextKey="" CompletionListCssClass="ac_results_Main"
																							CompletionListItemCssClass="ac_results_li"
																							CompletionListHighlightedItemCssClass="ac_over_Main"
																							OnClientPopulated="ClientPopulated" 
																							OnClientPopulating="ClientPopulating"
																							OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
																							OnClientShowing="ClientShowing">
																						</cc2:AutoCompleteExtender>
																					</asp:PlaceHolder>
																					<asp:PlaceHolder ID="phRegNoDropdown" runat="server"
																						Visible='<%#IIf(Project.TransTypeID = 101,
																											  True,
																											  False) %>'>
																						<asp:DropDownList ID="cmbAircraftList" runat="server"
																							CssClass="clsTextBoxTagSearchComboSmall" Visible="false"
																							AutoPostBack="True" DataValueField="ID" DataTextField="RegNo"
																							SelectedValue="<%# Project.MachineID %>" />
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<span id="lblModelStar" class="clsLabelStar">*</span>
																		</td>
																		<td>
																			<asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model No.</asp:Label>
																		</td>
																		<td>
																			<asp:UpdatePanel runat="server" ID="upnlModelNo" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:TextBox ID="txtModelNo" runat="server"
																						CssClass="clsTextBoxTagSearch"
																						Text="<%# Project.ModelName %>"
																						AutoPostBack="true"
																						ToolTip="Enter Model No." />
																					<cc2:AutoCompleteExtender runat="server"
																						ID="txtModelList_AutoCompleteExtender"
																						TargetControlID="txtModelNo"
																						ServiceMethod="GetModelNameList"
																						MinimumPrefixLength="0" EnableCaching="true"
																						CompletionSetCount="20"
																						CompletionInterval="1000" UseContextKey="True"
																						CompletionListCssClass="ac_results_Main"
																						CompletionListItemCssClass="ac_results_li"
																						CompletionListHighlightedItemCssClass="ac_over_Main"
																						OnClientPopulated="ClientPopulated"
																						OnClientPopulating="ClientPopulating"
																						OnClientHiding="ClientHiding"
																						OnClientShown="ClientHiding"
																						OnClientShowing="ClientShowing"
																						ServicePath="wfProject_Ajax.aspx" />
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<asp:Label ID="lblSerialNoStar" runat="server"
																				CssClass="clsLabelStar">*</asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="lblSerialNo" runat="server"
																				CssClass="clsLabelAuto">Serial No.</asp:Label>
																		</td>
																		<td>
																			<asp:UpdatePanel runat="server" ID="upnlSerialNo" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:TextBox ID="txtSerialNo" runat="server"
																						CssClass="clsTextBoxTagSearch"
																						Text="<%# Project.SerialNo %>"
																						ToolTip="Enter Serial No.">
																					</asp:TextBox>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																	<asp:PlaceHolder ID="phCustomer" runat="server" 
																		Visible='<%#IIf(Project.TransTypeID = 104, True, False) %>'>
																		<tr>
																			<td>
																				<asp:Label ID="lblCustomerStar" 
																					CssClass="clsLabelStar" runat="server"
																					Visible='<%#IIf(Project.TransTypeID = 104, True, False) %>'>*</asp:Label>
																			</td>
																			<td>
																				<asp:Label ID="lblCustomer" runat="server" 
																					CssClass="clsLabelAuto">Customer</asp:Label>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbCustomer" runat="server"
																					CssClass="clsTextBoxTagSearchComboNewstyle"
																					SelectedValue="<%# Project.CustomerID %>"
																					DataTextField="Name" DataValueField="ID"
																					Width="225px" />
																			</td>
																			<td>
																				<span id="lblCustomerContractStar" class="clsLabelStar"
																					runat="server" visible="false">*</span>
																			</td>
																			<td colspan="2">
																				<asp:UpdatePanel runat="server" ID="upnlCustomerContract" UpdateMode="Conditional">
																					<ContentTemplate>
																						<table>
																							<tr>
																								<td>
																									<asp:CheckBox ID="chkCustomerContract" 
																										runat="server" CssClass="clsCheckBox"
																										Text="Contracted Customer?" TextAlign="Left"
																										Checked="<%# Project.IsCustomerContract %>"
																										AutoPostBack="true" />
																								</td>
																								<td>
																									<asp:Label ID="lblCustomerContractNo" 
																										runat="server"
																										Text="<%# Project.CustomerContractNo  %>" 
																										CssClass="clsLabelHeader" />
																								</td>
																							</tr>
																						</table>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</td>
																		</tr>
																	</asp:PlaceHolder>
																	<tr>
																		<td><span id="lblDescriptionStar" class="clsLabelStar">*</span></td>
																		<td>
																			<asp:Label ID="lblDescription" runat="server" 
																				CssClass="clsLabelAuto" Text="Description" />
																		</td>
																		<td colspan="7">
																			<asp:TextBox ID="txtDescription" runat="server"
																				CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																				MaxLength="1000" Width="815px"
																				Text="<%# Project.Description %>"
																				TextMode="MultiLine"
																				Enabled="<%# Project.StatusID <> 3 %>" />
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblPLanStartDate" runat="server"
																				CssClass="clsLabelAuto" Text="Plan Start Date" />
																		</td>
																		<td>
																			<asp:TextBox runat="server" ID="txtPlanStartDate"
																				CssClass="clsTextBoxTagSearch" AutoComplete="off"
																				onchange="ValidateDateText(this,'txtPlanStartDate_WatermarkExtender');"
																				Width="100px" Enabled="<%# (Project.StatusID = 10) %>" />
																			<cc2:CalendarExtender ID="txtPlanStartDate_CalendarExtender"
																				runat="server" CssClass="cal_Theme1"
																				Enabled="true" Format="<%$AppSettings:DateFormat%>"
																				TargetControlID="txtPlanStartDate" />
																			<cc2:TextBoxWatermarkExtender TargetControlID="txtPlanStartDate"
																				ID="txtPlanStartDate_WatermarkExtender"
																				ClientIDMode="Static" runat="server"
																				WatermarkText="<%$AppSettings:DateFormat%>" />
																		</td>
																		<td></td>
																		<td>
																			<asp:Label ID="lblPlanEndDate" runat="server"
																				CssClass="clsLabelAuto" Text="Plan End Date" />
																		</td>
																		<td>
																			<asp:TextBox runat="server" ID="txtPlanEndDate" CssClass="clsTextBoxTagSearch"
																				AutoComplete="off" onchange="ValidateDateText(this,'txtPlanEndDate_WatermarkExtender');"
																				Width="100px" Enabled="<%# (Project.StatusID = 10) %>" />
																			<cc2:CalendarExtender ID="txtPlanEndDate_CalendarExtender" runat="server"
																				CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
																				TargetControlID="txtPlanEndDate" />
																			<cc2:TextBoxWatermarkExtender TargetControlID="txtPlanEndDate"
																				ID="txtPlanEndDate_WatermarkExtender" ClientIDMode="Static"
																				runat="server" WatermarkText="<%$AppSettings:DateFormat%>" />
																		</td>
																		<asp:PlaceHolder ID="phServiceProvider" runat="server" 
																			Visible="<%# Project.TransTypeID = 101 %>">
																			<td>
																				<span id="lblServiceProviderStar" class="clsLabelStar">*</span>
																			</td>
																			<td>
																				<asp:Label ID="lblServiceProvider" runat="server" 
																					CssClass="clsLabelAuto" Text="Service Provider" />
																			</td>
																			<td>
																				<asp:DropDownList ID="DD_ServiceProvider" runat="server"
																					CssClass="clsTextBoxTagSearchComboSmall"
																					AutoPostBack="true" Width="200px"
																					DataTextField="Name" DataValueField="ID"
																					ToolTip="Service Provider"
																					SelectedValue="<%# Project.ServiceProviderID %>" />
																			</td>
																		</asp:PlaceHolder>
																	</tr>
																</table>
															</fieldset>
														</td>
													</tr>
													<asp:PlaceHolder ID="phReceiving" runat="server" Visible='<%#IIf(Project.TransTypeID = 104, True, False) %>'>
														<tr>
															<td valign="top">
																<fieldset id="fdsEmpOtherDetails" class="clsFieldSetNewStyle">
																	<legend id="ledEmpOtherDetails" class="clsLabelHeader">Receiving Details</legend>
																	<table width="100%">
																		<tr>
																			<td></td>
																			<td style="width: 115px;">
																				<span id="lblFromDate" class="clsLabelAuto">Date</span>
																			</td>
																			<td style="width: 300px;">
																				<asp:TextBox runat="server" ID="txtReceivingDate"
																					CssClass="clsTextBoxTagDateSearch"
																					Width="100px" AutoComplete="off"
																					Text="<%# Project.ReceivingDateFormatted %>"
																					onchange="ValidateDateText(this,'txtReceivingDate_watermarkextender');"
																					Enabled="<%# Project.StatusID <> 3 %>" />
																				<cc2:CalendarExtender ID="txtReceivingDate_CalendarExtender"
																					runat="server" CssClass="cal_Theme1" Enabled="true"
																					Format="<%$AppSettings:DateFormat%>"
																					TargetControlID="txtReceivingDate" />
																				<cc2:TextBoxWatermarkExtender TargetControlID="txtReceivingDate"
																					ID="txtReceivingDate_watermarkextender"
																					ClientIDMode="Static" runat="server"
																					WatermarkText="<%$AppSettings:DateFormat%>" />
																			</td>
																			<td></td>
																			<td style="width: 110px;">
																				<asp:Label ID="lblEmployee" runat="server"
																					CssClass="clsLabelAuto">Person</asp:Label>
																			</td>
																			<td style="width: 260px;">
																				<asp:DropDownList ID="cmbEmployee" runat="server"
																					CssClass="clsTextBoxTagSearchComboNewstyle"
																					SelectedValue="<%# Project.ReceivingPersonID %>"
																					DataTextField="EmpNoName" DataValueField="ID"
																					Width="225px" AutoPostBack="true"
																					Enabled="<%# Project.StatusID <> 3 %>" />
																			</td>
																			<td></td>
																			<td style="width: 70px;">
																				<span id="lblToDate" class="clsLabelAuto">Inspection Date</span>
																			</td>
																			<td>
																				<asp:TextBox runat="server" ID="txtInspectionDate"
																					CssClass="clsTextBoxTagDateSearch"
																					Width="100px" AutoComplete="off"
																					Text="<%# Project.InspectionDateFormatted %>"
																					onchange="ValidateDateText(this,'txtInspectionDate_watermarkextender');"
																					Enabled="<%# Project.StatusID <> 3 %>" />
																				<cc2:CalendarExtender ID="txtInspectionDate_CalendarExtender"
																					runat="server" CssClass="cal_Theme1" Enabled="true"
																					Format="<%$AppSettings:DateFormat%>"
																					TargetControlID="txtInspectionDate" />
																				<cc2:TextBoxWatermarkExtender TargetControlID="txtInspectionDate"
																					ID="txtInspectionDate_watermarkextender" ClientIDMode="Static"
																					runat="server" WatermarkText="<%$AppSettings:DateFormat%>" />
																			</td>
																		</tr>
																	</table>
																</fieldset>
															</td>
														</tr>
													</asp:PlaceHolder>
													<tr>
														<td valign="top">
															<fieldset id="fdsAttachmentDetails" class="clsFieldSetNewStyle">
																<table>
																	<tr>
																		<td>
																			<span id="lblAttachFile" class="clsLabel">Attach File</span>
																		</td>
																		<td>
																			<asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table>
																						<tr>
																							<td>
																								<input type="button" id="btnSelectFile"
																									value="Select File"
																									runat="server" class="clsbtnH" />
																							</td>
																							<td>
																								<asp:Button ID="btnDelAttach" runat="server"
																									CssClass="clsbtnH"
																									Enabled="False" Text="Remove Attachment"
																									ToolTip="Click to Remove Attachment" />
																							</td>
																							<td>
																								<asp:ImageButton ID="btnViewAttachment"
																									runat="server"
																									CausesValidation="False" 
																									CssClass="FileAttachmentICN"
																									ImageUrl="icons/CLIP01.ICO" />

																								<asp:Button ID="hdnBtnFileUpload" runat="server"
																									CausesValidation="False" ClientIDMode="Static"
																									Style="display: none;" />
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<asp:Label ID="lblRemark" runat="server"
																				CssClass="clsLabelAuto" Text="Remark" />
																		</td>
																		<td>
																			<asp:TextBox ID="txtRemark" runat="server"
																				CssClass="clsTextBoxTagSearchMultilineNewstyle2"
																				MaxLength="1000" Text="<%# Project.Remark %>"
																				ToolTip="Enter Remark" TextMode="MultiLine"
																				Width="567px" />
																		</td>
																	</tr>
																</table>
															</fieldset>
														</td>
													</tr>
												</table>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<asp:PlaceHolder ID="phCurrentValues" runat="server"
									Visible="<%# Project.TransTypeID = 101 %>">
									<td valign="top">
										<asp:UpdatePanel ID="upnlAirframePeriods" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset class="clsFieldSetNewStyle">
													<legend class="clsFieldSet1">
														<b>
															<asp:Label ID="lblCurrentValue" runat="server" CssClass="clsLabelHeader">
															Current Values
															</asp:Label>
														</b>
													</legend>
													<table id="tblAirframePeriods" border="0" cellspacing="1" cellpadding="1" width="100%">
														<tr>
															<td valign="top" align="left">
																<asp:GridView ID="GV_CurrentPeriodValue" runat="server"
																	CssClass="clsGridNewStyle" ToolTip="Aircraft Periods."
																	PageSize="3" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																	GridLines="Horizontal" CellPadding="5">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<HeaderStyle BackColor="White"
																		ForeColor="Black" Font-Bold="True" />
																	<PagerStyle HorizontalAlign="Right" BorderStyle="Solid" />
																	<PagerSettings NextPageText="Next" PreviousPageText="Prev" />
																	<Columns>
																		<%--0--%>
																		<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																		<%--1--%>
																		<asp:BoundField DataField="PeriodName" HeaderText="Periods">
																			<HeaderStyle Wrap="False" Width="100px"
																				HorizontalAlign="Left" VerticalAlign="Middle" />
																			<ItemStyle Wrap="False" Width="100px"
																				HorizontalAlign="Left" VerticalAlign="Middle" />
																		</asp:BoundField>
																		<%--2--%>
																		<asp:BoundField DataField="AssemblyCurrentValueTextFormatted"
																			HeaderText="Values">
																			<HeaderStyle Wrap="False" HorizontalAlign="Right"
																				VerticalAlign="Middle" />
																			<ItemStyle Wrap="False" HorizontalAlign="Right"
																				VerticalAlign="Middle" />
																		</asp:BoundField>
																	</Columns>
																</asp:GridView>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</asp:PlaceHolder>
							</tr>
							<tr id="WOList">
								<td colspan="2">
									<asp:UpdatePanel runat="server" ID="upnlProjectDetail" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="pnlCompanyDetails" runat="server" CssClass="clsPanel1">
												<fieldset id="fdsEmpAuthorizationDetails" class="clsFieldSetNewStyle" runat="server">
													<legend id="ledEmpAuthorizationDetails">
														<table>
															<tr>
																<td>
																	<span id="lblCompanyAuthorizationDetailsAdd"
																		class="clsLabelHeader">Work Order:</span>
																</td>
																<td>
																	<asp:DropDownList ID="cmbWorkOrderType" runat="server"
																		CssClass="clsTextBoxTagSearchComboNewstyle"
																		AutoPostBack="true" />
																</td>
																<td>
																	<asp:Label runat="server" ID="lblWODate" 
																		CssClass="clsLabelAuto" Text="W.O. Date" />
																</td>
																<td>
																	<asp:TextBox runat="server" ID="txtWODate"
																		CssClass="clsTextBoxTagDateSearch" 
																		Width="100px" AutoComplete="off" 
																		onchange="ValidateDateText(this,'txtWODate_TextBoxWatermarkExtender');" />
																	<cc2:CalendarExtender ID="txtWODate_CalendarExtender"
																		runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>"
																		TargetControlID="txtWODate" />
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtWODate"
																		ID="txtWODate_TextBoxWatermarkExtender"
																		ClientIDMode="Static" runat="server"
																		WatermarkText="<%$AppSettings:DateFormat%>" />
																</td>
																<td>
																	<asp:ImageButton ID="btnAddWO" runat="server"
																		CausesValidation="true" CssClass="addRecordICN"
																		Enabled="<%# Project.StatusID <> 3 %>"
																		ImageUrl="~/images/plus1.png"
																		ToolTip="Add Work Order Details."
																		Height="22px" Width="24px" />
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">
														<tr>
															<td>
																<br />
															</td>
														</tr>
														<tr>
															<td>
																<asp:GridView ID="dgWOList" runat="server" CssClass="clsGridNewStyle"
																	ShowHeaderWhenEmpty="True" DataKeyNames="ID"
																	AutoGenerateColumns="False" CellPadding="10"
																	ForeColor="Black" GridLines="Horizontal">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																		Font-Bold="True" ForeColor="black" />
																	<PagerSettings FirstPageText="First" LastPageText="Last" />
																	<PagerStyle BackColor="White" CssClass="paging"
																		ForeColor="Black" HorizontalAlign="Right" />
																	<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
																	<SortedAscendingCellStyle BackColor="#F7F7F7" />
																	<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
																	<SortedDescendingCellStyle BackColor="#E5E5E5" />
																	<SortedDescendingHeaderStyle BackColor="#242121" />
																	<Columns>
																		<%--0--%>
																		<asp:BoundField DataField="ID" HeaderText="ID" Visible="false">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="true" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--1--%>
																		<asp:BoundField DataField="WODateFormatted" HeaderText="Date">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="true" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--2--%>
																		<asp:BoundField DataField="WONumber" HeaderText="Work-Order No">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="true" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--3--%>
																		<asp:BoundField DataField="TransTypeNameOnProject"
																			HeaderText="Work-Order Type"
																			SortExpression="TransTypeNameOnProject"
																			ItemStyle-Font-Bold="true">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="False" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--7--%>
																		<asp:BoundField DataField="WOBy" HeaderText="Created By"
																			SortExpression="WOBy">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="False" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--8--%>
																		<asp:BoundField DataField="AuthorizedBy" HeaderText="Submitted By"
																			SortExpression="AuthorizedBy">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="False" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--9--%>
																		<asp:BoundField DataField="WOStatus"
																			HeaderText="Work-Order Status">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="false" />
																			<ItemStyle Wrap="False" />
																		</asp:BoundField>
																		<%--10--%>
																		<asp:BoundField DataField="WOCloseDateFormatted"
																			HeaderText="Closing Date">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="False" />
																			<ItemStyle Wrap="False" />
																			<FooterStyle Wrap="False" />
																		</asp:BoundField>
																		<%--11--%>
																		<asp:BoundField DataField="ClosedBy"
																			HeaderText="Closed By"
																			SortExpression="ClosedBy">
																			<HeaderStyle ForeColor="Black"
																				HorizontalAlign="Left" Wrap="False" />
																		</asp:BoundField>
																		<%--12--%>
																		<asp:TemplateField HeaderText="Job Details"
																			ItemStyle-VerticalAlign="Middle"
																			HeaderStyle-HorizontalAlign="Center"
																			ItemStyle-HorizontalAlign="Center">
																			<ItemTemplate>
																				<asp:LinkButton ID="lnkJobDetails"
																					runat="server" Text='<%# Eval("WOJobCount") %>'
																					CausesValidation="false" CommandName="JobDetails" />
																			</ItemTemplate>
																		</asp:TemplateField>
																		<%--13--%>
																		<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
																			ItemStyle-HorizontalAlign="Center">
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
																			<ItemTemplate>
																				<div id="dropDownImg" class="dropdown">
																					<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
																						CssClass="clsActionbtn" />
																					<div id="dropdownICN-content" class="dropdownbtn-content">
																						<table id="dropdown-content" class="clsGridNew_Ajax">
																							<tr>
																								<td>
																									<asp:ImageButton ID="EditView" runat="server"
																										CommandArgument='<%# Eval("ID") %>'
																										CommandName="EditRec"
																										ImageUrl="~/images/edit.png" CssClass="actionICNS"
																										ToolTip="Edit current record." />
																								</td>
																								<td>
																									<asp:ImageButton ID="deleteICN" CssClass="largerActionICNS"
																										runat="server" CommandArgument='<%# Eval("ID") %>'
																										ToolTip="Delete current record."
																										Enabled="<%# Project.StatusID <> 3 %>"
																										CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="View" runat="server"
																										CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																										CommandName="ViewRec" ImageUrl="icons/CLIP01.ICO"
																										CssClass="FileAttachmentICN"
																										Visible='<%#  Eval("IsAttachmentAdded")%>'
																										ToolTip="View the attachment added." />
																								</td>
																							</tr>
																						</table>
																					</div>
																				</div>
																			</ItemTemplate>
																		</asp:TemplateField>
																		<%--14--%>
																		<asp:TemplateField HeaderText="Job Completion" ItemStyle-Width="110px">
																			<ItemTemplate>
																				<div class="progress progress-striped active">
																					<div id="prgbar" runat="server" aria-valuemax="100"
																						aria-valuemin="0" aria-valuenow="33"
																						class="progress-bar progress-bar-striped bg-success"
																						role="progressbar" style="width: 50%">
																						<span id="lblPercentage" runat="server"></span>
																					</div>
																				</div>
																			</ItemTemplate>
																		</asp:TemplateField>
																		<%--15--%>
																		<asp:BoundField DataField="TaskCompletionPercentage"
																			HeaderStyle-CssClass="hideGridColumn"
																			HeaderText="TaskCompletionPercentage"
																			ItemStyle-CssClass="hideGridColumn" />
																		<%--16--%>
																		<asp:BoundField DataField="IsAttachmentAdded"
																			HeaderStyle-CssClass="hideGridColumn"
																			HeaderText="IsAttachmentAdded"
																			ItemStyle-CssClass="hideGridColumn" />
																		<%--17--%>
																		<asp:BoundField DataField="TransTypeID"
																			HeaderStyle-CssClass="hideGridColumn"
																			HeaderText="TransTypeID"
																			ItemStyle-CssClass="hideGridColumn" />
																		<%--18--%>
																		<asp:BoundField DataField="WOJobTypeID"
																			HeaderStyle-CssClass="hideGridColumn"
																			HeaderText="WOJobTypeID"
																			ItemStyle-CssClass="hideGridColumn" />
																	</Columns>
																</asp:GridView>
															</td>
														</tr>
														<tr>
															<td>
																<table style="background-color: white" width="100%">
																	<tr>
																		<td align="left">
																			<table>
																				<tr>
																					<td valign="top">
																						<asp:Label ID="lblAMPTask" runat="server" CssClass="clsColorLabel"
																							BackColor="#ADD8E6" Height="20px" Width="20px"
																							ForeColor="#ADD8E6" BorderColor="black" />
																					</td>
																					<td valign="middle">
																						<asp:Label ID="lblAMPTasklabel" runat="server"
																							CssClass="clsLabel" Style="margin-top: 5px"
																							Text='<%# IIf(Project.TransTypeID = 101, "AMP Task", "AMO Task")%>' />
																					</td>
																				</tr>
																			</table>
																		</td>
																		<td align="left">
																			<table>
																				<tr>
																					<td valign="top">
																						<asp:Label ID="lblUnSchedule" runat="server" CssClass="clsColorLabel"
																							BackColor="#ffff90" Height="20px" Width="20px"
																							ForeColor="#ffff90" BorderColor="black" />
																					</td>
																					<td valign="middle">
																						<asp:Label ID="lblUnSchedulelabel" runat="server" class="clsLabel"
																							Style="margin-top: 5px"
																							Text='<%# IIf(Project.TransTypeID = 101,
																										"Un-Scheduled",
																										"Customer WO")%>' />
																					</td>
																				</tr>
																			</table>
																		</td>
																		<asp:PlaceHolder Visible='<%#IIf(AppSettings("IsEngineeringWORequired") = "True", True, False) %>'
																			runat="server" ID="phIsEngineeringWORequired">
																			<td align="left">
																				<table>
																					<tr>
																						<td valign="top">
																							<asp:Label ID="lblADSB" runat="server" CssClass="clsColorLabel"
																								BackColor="#F08080" Height="20px" Width="20px"
																								ForeColor="#F08080" BorderColor="black" />
																						</td>
																						<td valign="middle">
																							<span id="lblADSBSpan" class="clsLabel"
																								style="margin-top: 5px">AD / SB </span>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</asp:PlaceHolder>
																		<asp:PlaceHolder Visible='<%#IIf(AppSettings("ShowNewDiscrepancyFlow") = "True", False, True) %>'
																			runat="server" ID="phMEL">
																			<td align="left">
																				<table>
																					<tr>
																						<td valign="top">
																							<asp:Label ID="lblMELSnag" runat="server" CssClass="clsColorLabel"
																								BackColor="#AFEEEE" Height="20px" Width="20px"
																								ForeColor="#AFEEEE" BorderColor="black" />
																						</td>
																						<td valign="middle">
																							<asp:Label ID="lblMELSnaglabel" runat="server" class="clsLabel"
																								Style="margin-top: 5px"
																								Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True",
																									  "Defect / ADD",
																									  "Snag / MEL") %>' />
																						</td>
																					</tr>
																				</table>
																			</td>
																		</asp:PlaceHolder>
																		<asp:PlaceHolder Visible='<%#IIf(AppSettings("ShowNewDiscrepancyFlow") = "True", True, False) %>'
																			runat="server" ID="phDis">
																			<td align="left">
																				<table>
																					<tr>
																						<td valign="top">
																							<asp:Label ID="lblDiscrepancy" runat="server"
																								CssClass="clsColorLabel"
																								BackColor="#D3D3D3" Height="20px" Width="20px"
																								ForeColor="#D3D3D3" BorderColor="black" />
																						</td>
																						<td valign="middle">
																							<span id="lblDiscrepancySpan" class="clsLabel"
																								style="margin-top: 5px">Discrepancies </span>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</asp:PlaceHolder>
																		<asp:PlaceHolder Visible='<%#IIf(Project.TransTypeID = 101, True, False) %>'
																			runat="server" ID="phConcessionTask">
																			<td>
																				<table>
																					<tr>
																						<td valign="top">
																							<asp:Label ID="lblConcessionTaskColor" runat="server" CssClass="clsColorLabel"
																								BackColor="#90EE90" Height="20px" Width="20px"
																								ForeColor="#90EE90" BorderColor="black" />
																						</td>
																						<td valign="middle">
																							<asp:Label ID="lblConcessionTask" runat="server"
																								CssClass="clsLabel" Style="margin-top: 5px"
																								Text="Concession Task" />
																						</td>
																					</tr>
																				</table>
																			</td>
																		</asp:PlaceHolder>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</fieldset>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<!--Dummy panel to open modelpopup-->
							<tr id="HiddenButtons" style="height: 0px;">
								<td style="height: 0px;" colspan="2">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
										<ContentTemplate>
											<asp:Button ID="hdnBtnAddWODetail" ClientIDMode="Static" runat="server" Text="Add"
												CausesValidation="False" Style="display: none;"></asp:Button>
											<asp:Button ID="hdnBtnCustomerContractSelection" ClientIDMode="Static" runat="server"
												Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
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

		<div id="ModalPopUps">

			<!-- File Upload Modal Dialog-->
			<div id="FileUpload">

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
					function OpenFileUploadWindow() {
						try {
							$get("AjaxLoader").style.visibility = 'visible';
							$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
							$("#btnDummyFileUpload").click();
							$get("AjaxLoader").style.visibility = "hidden";
							return false;
						} catch (e) {
							alert(e);
						}
					}
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

			</div>
			<!-- End -->

			<!--WO Detail Popup Window-->
			<div id="WODetail">

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
					function OpenWODetail() {
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

			</div>
			<!-- End-->

			<!-- Popup For CustomerContractSelection -->
			<div id="CustomerContractSelection">

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyCustomerContractSelection" Text="CustomerContractSelection"
						ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlCustomerContractSelection" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">
					<iframe id="IframeCustomerContractSelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
						scrolling="auto" allowtransparency="true"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopupCustomerContractSelection" runat="server" TargetControlID="btnDummyCustomerContractSelection"
					PopupControlID="pnlCustomerContractSelection" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>
				<script type="text/javascript">
					function OpenCustomerContractSelectionWindow() {
						try {
							$("#IframeCustomerContractSelection").attr("src", "wfCustomerContractSelection_Ajax.aspx?Type=FromProejct");
							$("#btnDummyCustomerContractSelection").click();
							return false;
						} catch (e) {
							alert(e);
						}
					}
					function ParentCallBackFunctionForCustomerContractSelection() {
						var CustomerContractSelectionwindow = $find("<%=mdlPopupCustomerContractSelection.ClientID %>");
						//close popup window
						CustomerContractSelectionwindow.hide();
						//           release resources
						$("#IframeCustomerContractSelection").attr("src", "JavaScript:''");
						//call image button
						$("#hdnBtnCustomerContractSelection").click();
					}
				</script>

			</div>
			<!---End-->

			<!--WorkOrderAttach Popup Window -->
			<div id="WorkOrderAttach">

				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
						ClientIDMode="Static" />
				</div>
				<asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
					Style="height: 100%; width: 100%;">
					<iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
						width="100%" src="JavaScript:''" scrolling="auto"></iframe>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
					PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
				</cc2:ModalPopupExtender>
				<script type="text/javascript">
					function IFrameAttachStateComplete() {
						$("#btnDummyAttach").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					function OpenAttachWindow() {
						try {

							$get("AjaxLoader").style.visibility = 'visible';
							$("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

							if (!$.browser.msie) {
								$("#btnDummyAttach").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							}
							return false;
						} catch (e) {
							alert(e);
						}
					}
					function ParentCallBackFunctionForAttach() {
						var Attachwindow = $find("<%=mdlAttach.ClientID %>");
						//close popup window
						Attachwindow.hide();
						//release resources
						$("#IframeAttach").attr("src", "JavaScript:''");
						//call button click
						$("#hdnBtnAttach").click();
					}
				</script>

			</div>
			<!-- End-->

			<%--WO Job Details Popup Window--%>
			<div id="WOJobDetails">

				<div style="display: none">

					<asp:Button runat="server" ID="btnDummyWOJobDetails"
						Text="WOJobDetails" CausesValidation="false"
						ClientIDMode="Static" />

				</div>
				<asp:Panel runat="server" ID="pnlWOJobDetails" HorizontalAlign="Center">
					<asp:UpdatePanel runat="server" ID="upnlWOJobDetails" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="clstablelistout" id="tblWOJobDetails">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:Label ID="lblWOJobDetailsHeader" runat="server"
														CssClass="clsFormHeader" Text="Job Details" />
												</td>
												<td align="right">
													<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Button ID="btnCloseWOJobDetails" runat="server"
																CssClass="clsbtnH clsinfoH"
																ToolTip="Close Job Detail Pop Up."
																Text="Close" CausesValidation="False" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlGV_WOJobDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:GridView ID="GV_WOJobDetails" runat="server" CssClass="clsGridNewStyle"
													CellPadding="10" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
													ForeColor="Black" GridLines="Horizontal" AllowPaging="true" PageSize="3">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader"
														Font-Bold="True" ForeColor="black" />
													<PagerSettings FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging"
														ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<%--0--%>
														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
														<%--1--%>
														<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
															<HeaderStyle HorizontalAlign="Left" Width="10px" />
														</asp:BoundField>
														<%--2--%>
														<asp:BoundField DataField="TaskCardNo" HeaderText="Task No.">
															<HeaderStyle HorizontalAlign="Left" Width="10px" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--3--%>
														<asp:BoundField DataField="WOJobDescription"
															HeaderText="Description" HtmlEncode="false">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="true" CssClass="clsTextOnNewLine" />
														</asp:BoundField>
														<%--4--%>
														<asp:BoundField DataField="WOJobAction" HeaderText="Action">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<%--5--%>
														<asp:BoundField DataField="DueAsOfGrid"
															HeaderText="Due As Of" HtmlEncode="false">
															<ItemStyle Wrap="False" CssClass="clsTextOnNewLine"
																Width="80px" />
															<HeaderStyle HorizontalAlign="Left" Width="80px" />
														</asp:BoundField>
														<%--6--%>
														<asp:BoundField DataField="WOJobEstimatedTime"
															HeaderText="Est. Man Hr">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--7--%>
														<asp:BoundField DataField="WOJobStartDateFormatted"
															HeaderText="Start Date">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--8--%>
														<asp:BoundField DataField="WOJobCloseDateFormatted"
															HeaderText="Close Date">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--9--%>
														<asp:BoundField DataField="WOJobActualTime"
															HeaderText="Actual Man Hr.">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<%--10--%>
														<asp:BoundField DataField="WOJobTypeName"
															HeaderText="Job Type">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<%--11--%>
														<asp:BoundField DataField="WOJobStatusName"
															HeaderText="Status">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="false" />
														</asp:BoundField>
													</Columns>
												</asp:GridView>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>
				<cc2:ModalPopupExtender ID="mdlPopUpWOJobDetails" runat="server" BackgroundCssClass="clsModalPopupBG"
					TargetControlID="btnDummyWOJobDetails" PopupControlID="pnlWOJobDetails" />

			</div>
			<%--End--%>
		</div>

		<div id="Scripts">

			<%--Set page layout when open as popup aspx page--%>
			<script type="text/javascript">

				<% Dim Ppen As String = Request.QueryString("Type") %>

				<% If Not Ppen Is Nothing AndAlso Ppen = "pup" Then %> 

				$(document).ready(function () {
					SetPageLayout();
					if ($.browser.msie) {
						parent.IFrameEmpCAAuthorizationDetailsStateComplete();
					}
				});

				<% End if %>

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

				function endRequestHandler() {
					SetPageLayout();
				}

				function SetPageLayout() {

					<% Dim OpenAs As String = Request.QueryString("Type") %>

					<% If Not OpenAs Is Nothing AndAlso OpenAs = "pup" Then %>

					ReSetPageLayout();
					onResize();//for Top bottom link

					<% End if %>

				}

				function ReSetPageLayout() {

					$("body,html").css({ 'background-color': 'transparent' });
					var tempMargtop = $("body #Table-MaxWidth:eq(0)").outerHeight();
					var windowheight = $(window).height();

					if (tempMargtop >= windowheight) {
						$("body #Table-MaxWidth:eq(0)").css({ 'margin': 'auto' });
					}
					else {
						var margintop = (windowheight / 2) - (tempMargtop / 2);
						console.log("Margin Top " + margin);
						$("body #Table-MaxWidth:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
					}

				}

			</script>
			<%--End--%>

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
		</div>

	</form>
</body>
</html>

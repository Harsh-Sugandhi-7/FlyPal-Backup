<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSpareCompStatus.aspx.vb"
	Inherits="Flypal.wfSpareCompStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Component Status Details</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link href="js/semantic.css" rel="stylesheet" type="text/css" />
	<link id="MainStyle" type="text/css" rel="stylesheet" href="styles.css" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<link rel="stylesheet" type="text/css" href="popup.css" />
	<script type="text/javascript" src="AlertMessage1.1.js"></script>
	<script type="text/javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<script src="jquery.tooltip.min.js" type="text/javascript"></script>
	<style type="text/css">
		.clsCursorStyle {
			cursor: pointer;
		}
	</style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			OnAsyncPostBackError="ScriptManager1_AsyncPostBackError">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table border="0" class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table width="100%" class="clstitle1">
								<tr>
									<td>
										<asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Component Status Details</asp:Label>
									</td>
									<td align="right" style="padding: 0px;">
										<asp:ImageButton runat="server" ID="imgHome"
											ImageUrl="~/images/Home_Button.png"
											ToolTip="Return to Machine Detail Page"
											CssClass="HomeICN" Visible="false"
											CausesValidation="false" />
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td>
					<asp:UpdatePanel ID="upnlContainer" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<cc2:TabContainer ID="tabContainer" runat="server" AutoPostBack="true" ActiveTabIndex="3">
								<cc2:TabPanel ID="tbPnlComponentDetails" runat="server" CssClass="clsPanel1">
									<HeaderTemplate>
										Component details
									</HeaderTemplate>
									<ContentTemplate>
										<asp:UpdatePanel ID="upnlComponentDetails" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table border="0" class="clsTablelistin" id="tblinner" cellspacing="0" cellpadding="0">
													<tr>
														<td colspan="2">
															<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
																		HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
																	<asp:CustomValidator ID="cvInstalledValue" runat="server" Display="None" OnServerValidate="customvalidate1"
																		CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:CustomValidator ID="cvAssemblyInstallationValue" runat="server" Display="None"
																		OnServerValidate="customvalidate1" CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Max length of Remark should be 200."
																		ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"
																		CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" Display="None"
																		ControlToValidate="txtLicenceNo" ErrorMessage="Enter correct License No" OnServerValidate="customvalidate"></asp:CustomValidator>
																	<asp:CustomValidator ID="cvCurrentValue" runat="server" Display="None" OnServerValidate="customvalidate1"
																		CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description required."
																		ControlToValidate="txtDescription" Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
																	<asp:CustomValidator ID="cvDescription" runat="server" ErrorMessage="Description can not be more than 200 chars."
																		ControlToValidate="txtDescription" Display="None" OnServerValidate="customvalidate"
																		CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:CustomValidator ID="cvPartNo" runat="server" ErrorMessage="Part No Required."
																		ControlToValidate="cmbPartNo" Display="None" OnServerValidate="customvalidate"
																		CssClass="clsLabelAuto"></asp:CustomValidator>
																	<asp:RequiredFieldValidator ID="rfvATAChapter" runat="server" CssClass="clsLabelAuto"
																		ErrorMessage="ATA Chapter Required" ControlToValidate="cmbATAChapter" Display="None"></asp:RequiredFieldValidator>
																	<asp:CustomValidator ID="cvATAChapter" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select ATA Chapter From List."
																		ControlToValidate="cmbATAChapter" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:UpdatePanel ID="upnlPartInfo" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset id="Fieldset4" class="clsFieldSet" style="border-width: 1px;">
																		<legend id="lblCompDetailsCaption" style="font-weight: bold;">Part and Serial No. of
                                                                        Component</legend>
																		<table>
																			<tr>
																				<td style="width: 8px;"></td>
																				<td style="width: 115px;">
																					<span id="lblCode" class="clsLabelAuto">Code</span>
																				</td>
																				<td></td>
																				<td>
																					<asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxMedium_Ajax" ToolTip="Enter Code"
																						ClientIDMode="Static" Text="<%# mCompStatus.Comp.Code %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<span id="lblStarATA" class="clsLabelStar">*</span>
																				</td>
																				<td>
																					<span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
																				</td>
																				<td></td>
																				<td>
																					<asp:UpdatePanel ID="upnlATAMaster" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<table border="0" id="Table8" cellspacing="0">
																								<tr>
																									<td>
																										<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox2_Ajax"
																											SelectedValue="<%# mCompStatus.ATAID %>" DataValueField="ID" DataTextField="ATAChapter"
																											Width="229px">
																										</asp:DropDownList>
																									</td>
																									<td>
																										<asp:ImageButton ID="ImgBtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
																											Height="22px" Width="24px" ToolTip="Click to Add New ATA Chapter" CausesValidation="False"></asp:ImageButton>
																									</td>
																								</tr>
																							</table>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																				</td>
																			</tr>
																			<tr>
																				<td style="width: 8px;">
																					<span id="lblStarPartNo" class="clsLabelStar">*</span>
																				</td>
																				<td>
																					<asp:UpdatePanel ID="upnlbtnPartNo" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<asp:Button ID="btnPartNo" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add the new Part"
																								Text="Part No" CausesValidation="False"></asp:Button>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																				</td>
																				<td></td>
																				<td>
																					<asp:UpdatePanel ID="upnlPartNoModelDetails" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<table>
																								<tr>
																									<td>
																										<asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsComboBox2_Ajax" SelectedValue="<%# IIf(mPartlist.Contains(mCompStatus.Comp.PartName), mCompStatus.Comp.PartID, Guid.Empty) %>"
																											DataValueField="ID" DataTextField="Name" AutoPostBack="True" Enabled="<%# mCompStatus.IsPartEnabled %>"
																											Width="223px">
																										</asp:DropDownList>
																										<asp:TextBox ID="txtPartDescription" autocomplete="off" runat="server" CssClass="clsTextBox1_Ajax"
																											Text="<%# mcompstatus.Comp.PartName %>" Width="220px" AutoPostBack="True"></asp:TextBox>
																										<cc2:AutoCompleteExtender ID="txtPartDescription_Autocomplete" runat="server" ClientIDMode="Static"
																											CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
																											CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
																											Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetPartNoDescriptionList"
																											ServicePath="wfSpareCompStatus.aspx" TargetControlID="txtPartDescription" UseContextKey="True"
																											ContextKey="">
																										</cc2:AutoCompleteExtender>
																									</td>
																									<td>
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
																					<span id="lblDescription" class="clsLabelAuto">Description</span>
																				</td>
																				<td></td>
																				<td align="left">
																					<asp:UpdatePanel ID="upnlPartNoDetails" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
																								ToolTip="Enter Description" Text="<%# mCompStatus.Comp.Description %>" ReadOnly="True"
																								BackColor="#E0E0E0" Width="226px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
																							</td>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																			</tr>
																			<tr>
																				<td></td>
																				<td>
																					<span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
																				</td>
																				<td></td>
																				<td align="left">
																					<asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox2_Ajax" ToolTip="Enter Serial Number"
																						Text="<%# mCompStatus.Comp.SerialNo %>" MaxLength="50" Width="225px"></asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td></td>
																				<td>
																					<span id="lblManufacturer" class="clsLabelAuto">Manufacturer</span>
																				</td>
																				<td></td>
																				<td align="left">
																					<asp:UpdatePanel ID="upnlManufacturerMaster" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<table id="Table6" cellspacing="0">
																								<tr>
																									<td>
																										<asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsComboBox2_Ajax"
																											DataTextField="Name" DataValueField="ID" SelectedValue="<%# mCompStatus.ManufacturerID %>"
																											Width="229px">
																										</asp:DropDownList>
																									</td>
																									<td>
																										<asp:ImageButton ID="imgbtnModel1" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																											Width="24px" ToolTip="Click to add New Manufacturer" CausesValidation="False"></asp:ImageButton>
																									</td>
																								</tr>
																							</table>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																				</td>
																			</tr>
																		
																		</table>
																	</fieldset>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td valign="top">
															<asp:UpdatePanel ID="upnlTSNValues" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset id="Fieldset3" class="clsFieldSet" style="border-width: 1px;">
																		<legend id="lblModuleTSNCaption" runat="server" style="font-weight: bold;">Since New
                                                                        Values as on []</legend>
																		<table cellspacing="0">
																			<tr>
																				<td valign="top">
																					<asp:GridView ID="dgCurrentCompValue" runat="server" CssClass="clsGridLog" AutoGenerateColumns="False"
																						ShowHeaderWhenEmpty="true">
																						<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																						<RowStyle CssClass="clsdgItem"></RowStyle>
																						<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
																						<Columns>
																							<asp:BoundField Visible="False" DataField="PeriodID" HeaderText="PeriodID"></asp:BoundField>
																							<asp:BoundField DataField="PeriodName" HeaderText="Period">
																								<HeaderStyle HorizontalAlign="Left" />
																							</asp:BoundField>
																							<asp:TemplateField HeaderText="Component" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
																								<ItemTemplate>
																									<asp:TextBox ID="txtCurrentCompValue" runat="server" ToolTip="Enter Current Values for Component"
																										CausesValidation="false" AutoPostBack="true" OnTextChanged="txtCurrentCompValue_TextChanged"
																										CssClass="clsTextBoxRightAlignSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"CompCurrentValueFormatted") %>'
																										MaxLength="50">
																									</asp:TextBox>
																								</ItemTemplate>
																							</asp:TemplateField>
																							<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec">
																								<HeaderStyle HorizontalAlign="Left" />
																							</asp:ButtonField>
																						</Columns>
																					</asp:GridView>
																				</td>
																				<td valign="top">
																					<asp:ImageButton ID="btnAddPeriod" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																						Width="24px" ToolTip="Click to Add New Period" CausesValidation="False"></asp:ImageButton>
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
															<asp:UpdatePanel ID="upnlInstallationInfo" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
																		<legend id="lblInstallationCaption" style="font-weight: bold;">Information of Component</legend>
																		<table>
																			<tr>
																				<td></td>
																				<td>
																					<span id="lblRemark" class="clsLabelAuto">Remark</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" Width="223px"
																						ToolTip="Enter Remark" Text="<%# mCompStatus.InstallationRemark %>" MaxLength="1000"
																						TextMode="MultiLine"></asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td></td>
																				<td>
																					<span id="lblDoneByAgency" class="clsLabelAuto">Done By Agency</span>
																				</td>
																				<td>
																					<asp:TextBox ID="txtInstDoneBy" runat="server" ToolTip="Enter Done By Agency Name"
																						CssClass="clsTextBox2_Ajax" Text="<%# mCompStatus.InstDoneBy %>" MaxLength="100"
																						Width="225px"></asp:TextBox>
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
																										<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
																											DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
																											CompletionInterval="1" ServicePath="wfComplyAssemblyMonitorInspStatus_Ajax.aspx"
																											ServiceMethod="GetLicenseNoList" TargetControlID="txtLicenceNo" OnClientItemSelected="SetLicenceNo"
																											UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
																											CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
																											OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
																											OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
																										</cc2:AutoCompleteExtender>
																									</td>
																									<td>
																										<asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
																											Height="22px" Width="24px" ToolTip="Click to Add New Licence No." CausesValidation="true" />
																									</td>
																								</tr>
																								<tr>
																									<td colspan="2">
																										<asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mCompStatus.MaintenanceDoneByEmployees.Count > 1 %>"
																											ToolTip="<%# mCompStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
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
																				<td>
																					<asp:TextBox ID="txtPlace" runat="server" ToolTip="Enter Place" CssClass="clsTextBox2_Ajax"
																						Text="<%# mCompStatus.InstPlace %>" MaxLength="25" Width="225px"></asp:TextBox>
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
															<asp:UpdatePanel ID="upnlDocumentDetails" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset id="Fieldset5" class="clsFieldSet" style="border-width: 1px;">
																		<legend id="lblDocumentationValueCaption" runat="server" style="font-weight: bold;">
																			<b>Document Details</b></legend>
																		<table id="Table10" border="0" cellpadding="0" width="100%">
																			<tr>
																				<td></td>
																				<td>
																					<span id="lblAttachFile" class="clsLabel">Attach File</span>
																				</td>
																				<td style="padding-left: 3px">
																					<asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<table border="0" cellpadding="0" cellspacing="0">
																								<tr>
																									<td>
																										<input type="button" id="btnSelectFile" runat="server" value="Select File" style="width: 110px;"
																											clientidmode="Static" class="clsButton_Ajax" />
																									</td>
																									<td style="padding-left: 3px;">
																										<asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
																											Text="Remove Attachment" Enabled="False" Width="120px"></asp:Button>
																									</td>
																									<td style="padding-left: 2px;">
																										<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" ImageUrl="icons/CLIP01.ICO"
																											Height="24px" Width="15px"></asp:ImageButton>
																									</td>
																								</tr>
																							</table>
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
														<td align="right" colspan="2">
															<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table border="0" id="Table1" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnSave" runat="server" 
																					CssClass="clsButton_Ajax" 
																					ToolTip="Click to Save the page."
																					Text="Save"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrint" runat="server"
																					CssClass="clsButton_Ajax" 
																					ToolTip="Click to Print the page."
																					Text="Print"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnBack" runat="server" 
																					CssClass="clsButton_Ajax" 
																					ToolTip="Back to Previous Page"
																					Text="Back" CausesValidation="false"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td align="right">
															<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
																		CausesValidation="false" Style="display: none;"></asp:Button>
																	<asp:Button ID="hdnBtnManufacturer" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnBtnPart" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnAddPeriod" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnBtnSeriviceMasterList" runat="server" CausesValidation="false"
																		ClientIDMode="Static" Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnBtnInspMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnimgBtnATAChapter" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnBtnModMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
																		Style="display: none;" Text="Add" />
																	<asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
																		CausesValidation="False" Style="display: none;"></asp:Button>
																	<asp:Button ID="hdnThrustValue" ClientIDMode="Static" runat="server" Text="----"
																		CausesValidation="False" Style="display: none;"></asp:Button>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</ContentTemplate>
								</cc2:TabPanel>
								<cc2:TabPanel ID="tbPnlService" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
									<HeaderTemplate>
										Service List
									</HeaderTemplate>
									<ContentTemplate>
										<asp:UpdatePanel ID="upnlService" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table2" class="clsTablelistin">
													<tr>
														<td colspan="2">
															<asp:Label ID="lblInfoService" runat="server" CssClass="clsLabelAuto">List of all the Services on the Component as of Date: [As of Date]. All the values of all the Services will be as of Date: [As on Date].</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<span id="lblSearchService" class="clsLabelAuto">Search</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbLookInService" runat="server" CssClass="clsComboBox_Ajax"
																			AutoPostBack="True">
																			<asp:ListItem Value="0">All</asp:ListItem>
																			<asp:ListItem Value="1">ATA Code</asp:ListItem>
																			<asp:ListItem Value="2">Service Type</asp:ListItem>
																			<asp:ListItem Value="3">Work Order No.</asp:ListItem>
																			<asp:ListItem Value="4">Show In C of A</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																	<td>
																		<asp:Label ID="lblForService" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtForService" runat="server" ToolTip="Enter value." CssClass="clsTextBox_Ajax"
																			Visible="False" MaxLength="25"></asp:TextBox>
																		<asp:TextBox ID="txtCodeService" runat="server" ToolTip="Enter value." CssClass="clsTextBox_Ajax"
																			ClientIDMode="Static" Visible="False" MaxLength="5"></asp:TextBox>
																		<asp:DropDownList ID="cmbSearchForService" runat="server" CssClass="clsComboBoxDouble_Ajax"
																			DataTextField="CodeType" DataValueField="ID">
																		</asp:DropDownList>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right">
															<table>
																<tr>
																	<td align="right">
																		<asp:Button ID="btnFindNowService" runat="server" 
																			ToolTip="Click to Find the list as per searching criteria."
																			CssClass="clsButton_Ajax" CausesValidation="False"
																			Text="Find Now"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblCountService" runat="server" CssClass="clsLabelHeader"></asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlActionBtnServiceTop" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table4" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewTopService" TabIndex="0" runat="server" 
																					ToolTip="Add New Component Service"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintTopService" TabIndex="0" runat="server" 
																					ToolTip="Print Service List page."
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseTopService" runat="server" Visible="false" 
																					ToolTip="Back to Previous Page"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:GridView ID="dgMonitorServiceStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
																ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" PageSize="3">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="PartMonitorServiceID" HeaderText="PartMonitorServiceID"></asp:BoundField>
																	<asp:TemplateField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
																		<ItemStyle HorizontalAlign="Center" />
																		<ItemTemplate>
																			<div class="clstooltip" style="display: none;">
																				<b>Monitor Info:</b>&nbsp;
                                                                            <%# Eval("ServiceTypeDet")%>
																			</div>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ServiceTypeCode" SortExpression="ServiceTypeCode" HeaderText="Monitor Type">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Code_Desc" SortExpression="Code_Desc" HeaderText="Code/Form No./Description"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Right" Wrap="false" />
																	</asp:BoundField>
																	<asp:BoundField DataField="WorkOrderNo" HeaderText="WO. No.">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On " HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" Wrap="false" />
																	</asp:BoundField>
																	<asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" Wrap="false" />
																	</asp:BoundField>
																	<asp:BoundField DataField="Remark" HeaderText="Remark">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderText="Is Applicable" HeaderStyle-HorizontalAlign="Left"
																		HeaderStyle-Wrap="false">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
																				Enabled="False"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster"
																		HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="View" HeaderText="View" CommandName="View" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td colspan="2" align="right">
															<asp:UpdatePanel ID="upnlActionBtnService" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table5" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewService" runat="server" ToolTip="Add New Component Service"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintService" runat="server" ToolTip="Print Service List page."
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseService" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
																					CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</ContentTemplate>
								</cc2:TabPanel>
								<cc2:TabPanel ID="tbPnlInspection" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
									<HeaderTemplate>
										Inspection List
									</HeaderTemplate>
									<ContentTemplate>
										<asp:UpdatePanel ID="upnlInspection" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table3" class="clsTablelistin">
													<tr>
														<td colspan="2">
															<asp:Label ID="lblInfoInspection" runat="server"
																CssClass="clsLabelAuto">List of all the Inspections on the Component as of Date: 
																[As of Date]. All the values of all the Inspections will be as of Date: [As on Date]</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<span id="lblSearchInspection" class="clsLabelAuto">Search</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbLookInInspection" runat="server" CssClass="clsComboBox_Ajax"
																			AutoPostBack="True">
																			<asp:ListItem Value="0">All</asp:ListItem>
																			<asp:ListItem Value="1">ATA Code</asp:ListItem>
																			<asp:ListItem Value="2">Insp Type</asp:ListItem>
																			<asp:ListItem Value="3">Work Order No.</asp:ListItem>
																			<asp:ListItem Value="4">Show In C of A</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																	<td>
																		<asp:Label ID="lblForInspection" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtForInspection" runat="server" ToolTip="Enter value." CssClass="clsTextBox_Ajax"
																			Visible="False" MaxLength="25"></asp:TextBox>
																		<asp:TextBox ID="txtCodeInspection" runat="server" ToolTip="Enter value." CssClass="clsTextBox_Ajax"
																			Visible="False" MaxLength="5"></asp:TextBox>
																		<asp:DropDownList ID="cmbSearchForInspection" runat="server" CssClass="clsComboBoxDouble_Ajax"
																			DataValueField="ID" DataTextField="CodeType">
																		</asp:DropDownList>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right">
															<table>
																<tr>
																	<td align="right">
																		<asp:Button ID="btnFindNowInspection" runat="server" ToolTip="Click to Find the list as per searching criteria."
																			Text="Find Now" CausesValidation="False" CssClass="clsButton_Ajax"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblCountInspection" runat="server" CssClass="clsLabelHeader"></asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlActionBtnInspectionTop" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table9" border="0" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewTopInspection" runat="server" ToolTip="Add New Component Inspection"
																					Text="Add New" CausesValidation="False" CssClass="clsButton_Ajax"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintTopInspection" runat="server" ToolTip="Print Inspection List page."
																					Text="Print" CausesValidation="False" CssClass="clsButton_Ajax"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseTopInsp" runat="server" Visible="false" ToolTip="Back to Previous Page"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:GridView ID="dgMonitorInspStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
																ShowHeaderWhenEmpty="true" PageSize="3" AutoGenerateColumns="False">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="PartMonitorInspID" HeaderText="PartMonitorInspID"></asp:BoundField>
																	<asp:TemplateField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
																		<ItemStyle HorizontalAlign="Center" />
																		<ItemTemplate>
																			<div class="clstooltip" style="display: none;">
																				<b>Monitor Info:</b>&nbsp;
                                                                            <%# Eval("InspTypeDet")%>
																			</div>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="InspTypeCode" SortExpression="InspType" HeaderText="Monitor Type">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Code_Desc" SortExpression="Code_Desc" HeaderText="Code/Form No./Description"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="WorkOrderNo" HeaderText="WO. No.">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Remark" HeaderText="Remark">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderText="Is Applicable" HeaderStyle-HorizontalAlign="Left"
																		HeaderStyle-Wrap="false">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
																				Enabled="False"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster"
																		HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="View" HeaderText="View" CommandName="View" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td colspan="2" align="right">
															<asp:UpdatePanel ID="upnlActionBtnInspection" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table11" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewInspection" runat="server" ToolTip="Add New Component Inspection"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Add New"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintInspection" runat="server" ToolTip="Print Inspection List page."
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Print"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseInsp" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
																					CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</ContentTemplate>
								</cc2:TabPanel>
								<cc2:TabPanel ID="tbPnlModification" runat="server" CssClass="clsPanel1" Visible="<%# Not mCompStatus.IsNew %>">
									<HeaderTemplate>
										Modification List
									</HeaderTemplate>
									<ContentTemplate>
										<asp:UpdatePanel ID="upnlModification" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table7" class="clsTablelistin">
													<tr>
														<td colspan="2">
															<asp:Label ID="lblInfoModification" runat="server" CssClass="clsLabelAuto">List of all the Modifications on the Component as of Date: [As of Date]. All the values of all the Modifications will be as of Date: [As on Date]</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<span id="lblSearchModification" class="clsLabelAuto">Search</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbLookInModification" runat="server" CssClass="clsComboBox_Ajax"
																			AutoPostBack="True">
																			<asp:ListItem Value="0">All</asp:ListItem>
																			<asp:ListItem Value="1">ATA Code</asp:ListItem>
																			<asp:ListItem Value="2">Mod No.</asp:ListItem>
																			<asp:ListItem Value="3">Mod Type</asp:ListItem>
																			<asp:ListItem Value="4">Work Order No.</asp:ListItem>
																			<asp:ListItem Value="5">Show In C of A</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																	<td>
																		<asp:Label ID="lblForModification" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtForModification" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
																			Visible="False" MaxLength="25"></asp:TextBox>
																		<asp:TextBox ID="txtCodeModification" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter value."
																			Visible="False" MaxLength="5"></asp:TextBox>
																		<asp:DropDownList ID="cmbSearchForModification" runat="server" CssClass="clsComboBoxDouble_Ajax"
																			DataTextField="CodeType" DataValueField="ID">
																		</asp:DropDownList>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right">
															<asp:Button ID="btnFindNowModification" runat="server" Text="Find Now" CausesValidation="False"
																CssClass="clsButton_Ajax" ToolTip="Click to Find the list as per searching criteria."></asp:Button>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblCountModification" runat="server" CssClass="clsLabelHeader"></asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlActionBtnModificationTop" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table12" border="0" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewTopModification" runat="server" Text="Add New" CausesValidation="False"
																					CssClass="clsButton_Ajax" ToolTip="Add New Component Modification"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintTopModification" runat="server" Text="Print" CausesValidation="False"
																					CssClass="clsButton_Ajax" ToolTip="Print Modification List"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseTopMod" runat="server" Visible="false" ToolTip="Back to Previous Page"
																					CssClass="clsButton_Ajax" CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:GridView ID="dgMonitorModStatusList" runat="server" CssClass="clsGrid" AllowSorting="True"
																ShowHeaderWhenEmpty="true" PageSize="3" AutoGenerateColumns="False">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
																		DataField="PartMonitorModID" HeaderText="PartMonitorModID"></asp:BoundField>
																	<asp:TemplateField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
																		<ItemStyle HorizontalAlign="Center" />
																		<ItemTemplate>
																			<div class="clstooltip" style="display: none;">
																				<b>Monitor Info:</b>&nbsp;
                                                                            <%# Eval("ModTypeDet")%>
																			</div>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:BoundField DataField="ModNumber" SortExpression="ModNumber" HeaderText="Modification Number">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ModTypeCode" SortExpression="ModTypeCode" HeaderText="Monitor Type">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Code_Desc" SortExpression="Code_Desc" HeaderText="Code/Form No./Description"
																		HtmlEncode="false">
																		<HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="WorkOrderNo" HeaderText="WO. No.">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On " HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExtensionValueFormatted" HeaderText="Extension" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At." HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="Remark" HeaderText="Remark">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:TemplateField HeaderText="Is Applicable" HeaderStyle-HorizontalAlign="Left"
																		HeaderStyle-Wrap="false">
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
																				Enabled="False"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Edit Master" HeaderText="Edit Master" CommandName="EditMaster"
																		HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:ButtonField Text="View" HeaderText="View" CommandName="View" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td colspan="2" align="right">
															<asp:UpdatePanel ID="upnlActionBtnModification" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table13" border="0" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAddNewModification" runat="server" Text="Add New" CausesValidation="False"
																					CssClass="clsButton_Ajax" ToolTip="Add New Component Modification"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnPrintModification" TabIndex="0" runat="server" Text="Print" CausesValidation="False"
																					CssClass="clsButton_Ajax" ToolTip="Print Modification List"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnCloseMod" runat="server" ToolTip="Back to Previous Page" CssClass="clsButton_Ajax"
																					CausesValidation="False" Text="Back"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</ContentTemplate>
								</cc2:TabPanel>
							</cc2:TabContainer>
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
		</table>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
		<script language="javascript" type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				$(".clstooltip").closest("tr").mousemove(function (event) {
					$(this).find(".clstooltip").css({
						"left": event.pageX + 1,
						"top": event.pageY + 1
					}).show();
				}).mouseout(function () { $(this).find(".clstooltip").hide(); });;
			});
		</script>
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
		<!-- Manufacturer Master popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyManufacturer" Text="TaskCard Tool" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeManufacturer" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
			PopupControlID="pnlManufacturer" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameManufacturerStateComplete() {
				$("#btnDummyManufacturer").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenManufacturerWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyManufacturer").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForManufacturer() {
				var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
				//close Task Card Tool popup window
				Manufacturerwindow.hide();
				//           release resources
				$("#IframeManufacturer").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnManufacturer").click();
			}
		</script>
		<!-- End-->
		<!-- Part Master Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyPart" Text="Dummy Part" ClientIDMode="Static"
				CausesValidation="false"></asp:Button>
		</div>
		<asp:Panel runat="server" ID="pnlPart" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframePart" frameborder="0" height="100%" allowtransparency="true" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupPart" runat="server" TargetControlID="btnDummyPart"
			PopupControlID="pnlPart" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFramePartStateComplete() {
				$("#btnDummyPart").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenPartWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframePart").attr("src", "wfPart_AJAX.aspx?Type=pup");
					// $("#IframePart").load(function () {
					//                    var doc = IframePart.window;
					//                    IframePart.SetPageLayout();

					if (!$.browser.msie) {
						$("#btnDummyPart").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}


					//});


					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForPart() {
				var Partwindow = $find("<%=mdlPopupPart.ClientID %>");
				//close Part popup window
				Partwindow.hide();
				//           release resources
				$("#IframePart").attr("src", "JavaScript:''");
				//call Part image button
				$("#hdnBtnPart").click();
			}
		</script>
		<!-- End-->
		<!-- Period Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlAddPeriod" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeAddPeriod" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupTaskCardStep" runat="server" TargetControlID="btnDummyAddPeriod"
			PopupControlID="pnlAddPeriod" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameStateComplete() {
				$("#btnDummyAddPeriod").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenAddPeriodWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeAddPeriod").attr("src", "wfSelectPeriod_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyAddPeriod").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForAddPeriod() {
				var TaskCardStepwindow = $find("<%=mdlPopupTaskCardStep.ClientID %>");
				//close Task Card Step popup window
				TaskCardStepwindow.hide();
				//           release resources
				$("#IframeAddPeriod").attr("src", "JavaScript:''");
				//call image button
				$("#hdnAddPeriod").click();
			}
		</script>
		<!-- End-->
		<!-- ATA Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupATA" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupATA" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupATA" runat="server" TargetControlID="btnDummyATA"
			PopupControlID="pnlPopupATA" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameATAStateComplete() {
				$("#btnDummyATA").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenATAWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyATA").click();
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
				var atawindow = $find("<%=mdlPopupATA.ClientID %>");
				//close ata popup window
				atawindow.hide();
				$("#iPopupATA").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnATAChapter").click();
			}
		</script>
		<!-- End-->
		<!-- Serivice Master Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySeriviceMaster" Text="Dummy SeriviceMaster"
				ClientIDMode="Static"></asp:Button>
		</div>
		<asp:Panel runat="server" ID="pnlSeriviceMaster" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeSeriviceMaster" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupSeriviceMaster" runat="server" TargetControlID="btnDummySeriviceMaster"
			PopupControlID="pnlSeriviceMaster" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameServiceMasterStateComplete() {
				$("#btnDummySeriviceMaster").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenSeriviceMasterWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					//var GChildPage2 = window.location.search.split('GChildPage2')[1].split('=')[1];
					$("#IframeSeriviceMaster").attr("src", "wfPartMonitorService_AJAX.aspx?Type=pup&GChildPage4=wfSpareCompStatus.aspx");
					// $("#IframeSeriviceMaster").load(function () {
					//                    var doc = IframeSeriviceMaster.window;
					//                    IframeSeriviceMaster.SetPageLayout();

					if (!$.browser.msie) {
						$("#btnDummySeriviceMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}


					//});


					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForServiceMaster() {
				var SeriviceMasterwindow = $find("<%=mdlPopupSeriviceMaster.ClientID %>");
				//close SeriviceMaster popup window
				SeriviceMasterwindow.hide();
				//           release resources
				$("#IframeSeriviceMaster").attr("src", "JavaScript:''");
				//call SeriviceMaster image button
				$("#hdnBtnSeriviceMaster").click();
			}
		</script>
		<!-- End-->

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


		<!-- Insp Master Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyInspMaster" Text="Dummy InspMaster" ClientIDMode="Static"></asp:Button>
		</div>
		<asp:Panel runat="server" ID="pnlInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeInspMaster" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupInspMaster" runat="server" TargetControlID="btnDummyInspMaster"
			PopupControlID="pnlInspMaster" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameInspMasterStateComplete() {
				$("#btnDummyInspMaster").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenInspMasterWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					// var GChildPage2 = window.location.search.split('GChildPage2')[1].split('=')[1];
					$("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage4=wfSpareCompStatus.aspx");
					// $("#IframeInspMaster").load(function () {
					//                    var doc = IframeInspMaster.window;
					//                    IframeInspMaster.SetPageLayout();

					if (!$.browser.msie) {
						$("#btnDummyInspMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}


					//});


					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForInspMaster() {
				var InspMasterwindow = $find("<%=mdlPopupInspMaster.ClientID %>");
				//close InspMaster popup window
				InspMasterwindow.hide();
				//           release resources
				$("#IframeInspMaster").attr("src", "JavaScript:''");
				//call InspMaster image button
				$("#hdnBtnInspMaster").click();
			}
		</script>
		<!-- End-->
		<!-- ModMaster Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModMaster" Text="Dummy ModMaster" ClientIDMode="Static"></asp:Button>
		</div>
		<asp:Panel runat="server" ID="pnlModMaster" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModMaster" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModMaster" runat="server" TargetControlID="btnDummyModMaster"
			PopupControlID="pnlModMaster" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModMasterStateComplete() {
				$("#btnDummyModMaster").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModMasterWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					//var GChildPage2 = window.location.search.split('GChildPage2')[1].split('=')[1];
					$("#IframeModMaster").attr("src", "wfPartMonitorMod_AJAX.aspx?Type=pup&GChildPage4=wfSpareCompStatus.aspx");
					// $("#IframeModMaster").load(function () {
					//                    var doc = IframeModMaster.window;
					//                    IframeModMaster.SetPageLayout();

					if (!$.browser.msie) {
						$("#btnDummyModMaster").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}


					//});


					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForModMaster() {
				var ModMasterwindow = $find("<%=mdlPopupModMaster.ClientID %>");
				//close ModMaster popup window
				ModMasterwindow.hide();
				//           release resources
				$("#IframeModMaster").attr("src", "JavaScript:''");
				//call ModMaster image button
				$("#hdnBtnModMaster").click();
			}
		</script>
		<!-- End-->
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
					$("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=3");

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
		<script type="text/javascript">
			function SetLicenceNo(source, e) {
				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}

				var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
				source.get_element().value = text;

				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtLicenceNo_Autocomplete") {
					textbox = document.getElementById('hdnLicenceNo');
				}


				textbox.value = value.toString();
			}
			//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
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
	</form>
	<script src="js/jquery.js" type="text/javascript"></script>
	<script src="js/jquery-1.8.3.js" type="text/javascript"></script>
	<script type="text/javascript" src="Notification/jQuery/ui.core.js"></script>
	<script type="text/javascript" src="Notification/jQuery/ui.notificationmsg.js"></script>
	<script src="bootstrap/bootstrap-toggle.min.js" type="text/javascript"></script>
	<script src="js/semantic.js" type="text/javascript"></script>

</body>
</html>

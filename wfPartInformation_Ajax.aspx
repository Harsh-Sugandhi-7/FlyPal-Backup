<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartInformation_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfPartInformation_Ajax" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Import Namespace="Flypal.CategoryList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Part Information</title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" id="clientEventHandlersJS">
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
	</script>
	<style type="text/css">
		.clsTextBoxRightAlignSmall_Ajax {
		}
	</style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<asp:Panel runat="server" ID="pnlPartInformation">
				<table class="clstablelistout" id="tblMain">
					<tr>
						<td>
							<asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
								<table id="tblinner" class="clsTablelistin">
									<tr>
										<td class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Part Information [New]</asp:Label>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>

													<td align="right">
														<asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
															<ContentTemplate>
																<table id="Table1">
																	<tr>
																		<td>
																			<asp:Button ID="btnSave" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
																				ToolTip="Click to save the Part &amp; refresh the screen" Text="Save"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnSaveNew" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
																				Visible="false" ToolTip="Click to save the Part &amp; refresh the screen" Text="Save &amp; New"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnSaveClose" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
																				Visible="false" ToolTip="Click to save the Part &amp; close the screen" Text="Save &amp; Close"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
																				Text="Close" CausesValidation="False"></asp:Button>
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
											<asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
														ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
													<asp:RequiredFieldValidator ID="rfvPartNo" runat="server" CssClass="clslabelAuto"
														ValidationGroup="1" Display="None" ErrorMessage="Part Number Required" ControlToValidate="txtPartNo"></asp:RequiredFieldValidator>
													<asp:RequiredFieldValidator ValidationGroup="1" ID="rfvDescription" runat="server"
														CssClass="clsLabel" Display="None" ErrorMessage="Description Required" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
													<asp:CustomValidator ValidationGroup="1" ID="cvDescription" runat="server" Display="None"
														ErrorMessage="Part description must not be greater than 100 characters." ControlToValidate="txtDescription"
														OnServerValidate="customvalidate" CssClass="clsLabel" />

													<asp:CustomValidator ValidationGroup="1" ID="CvLocation" runat="server" Display="None"
														ErrorMessage="Location Required." ControlToValidate="txtLocation"
														OnServerValidate="customvalidate" CssClass="clsLabel" />

													<asp:CustomValidator ValidationGroup="1" ID="cvNote" runat="server" Display="None"
														ErrorMessage="Note field  must not be greater than 100 characters." ControlToValidate="txtNote"
														OnServerValidate="customvalidate" CssClass="clsLabel" />
													<asp:CustomValidator ValidationGroup="1" ID="cvApproxRate" runat="server" Display="None"
														ErrorMessage="Approximate Rate can't be negative." ControlToValidate="txtApproxRate"
														OnServerValidate="customvalidate" CssClass="clsLabel" />
													<asp:CustomValidator ID="cvFolio" ValidationGroup="1" runat="server" Display="None"
														ErrorMessage="Folio No. can't be negative." ControlToValidate="txtFolio" OnServerValidate="customvalidate"
														CssClass="clsLabel" />
													<asp:CustomValidator ValidationGroup="1" ID="cvBenchCheckMth" runat="server" Display="None"
														ErrorMessage="a" ControlToValidate="txtBenchmarkMonths" OnServerValidate="customvalidate"
														CssClass="clsLabel" />
													<asp:CustomValidator ID="cvQty1" runat="server" Display="None" ValidationGroup="1"
														ValidateEmptyText="true" ControlToValidate="txtMinStockLevel" ErrorMessage="Aircraft Required"
														OnServerValidate="customvalidate" />
													<asp:CustomValidator ID="csvItemAppli" runat="server" Display="None" ValidationGroup="1"
														ValidateEmptyText="true" ControlToValidate="txtAMMCMMReference" ErrorMessage="As item is Rotable add applicability."
														OnServerValidate="customvalidate" />
													<asp:CustomValidator ID="cvIPCRef" runat="server" Display="None" ValidationGroup="1"
														ValidateEmptyText="true" ControlToValidate="txtIPCReference" ErrorMessage="IPC Reference Required."
														OnServerValidate="customvalidate" />
													<%--
                                                       <asp:CustomValidator ValidationGroup="1" ID="Cust" runat="server" Display="None"
                                                    ErrorMessage="Select Service Inspections" ControlToValidate="chkServicedInspected"
                                                    OnServerValidate="customvalidate" CssClass="clsLabel" />--%>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td>
											<asp:UpdatePanel runat="server" ID="upnlTabs" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="Table2" border="0" cellspacing="0">
														<tr>
															<td>
																<asp:Label ID="lblPartInformation" runat="server" CssClass="clsLabelButton1" ToolTip="Current page of Aircraft Status Detail">Part Information</asp:Label>
															</td>
															<td>
																<asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to open the Alternate Part List"
																	Text="Alternate Part" EnableViewState="False"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnApplicability" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to open the Applicability List"
																	Text="Applicability" EnableViewState="False"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnOpeningStock" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to open the Opening Stock List"
																	Text="Opening Stock" EnableViewState="False"></asp:Button>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<!--update panel details -->
									<tr>
										<td>
											<asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
												<ContentTemplate>
													<div style="width: 100%;">
														<table style="width: 100%;" border="0">
															<tr>
																<td colspan="6">
																	<span id="lblGeneralInfo" class="clsLabelHeader">General Information</span>
																</td>
															</tr>
															<tr>
																<td style="width: 0px;">
																	<span id="lblPartNo1" class="clsLabelStar">*</span>
																</td>
																<td style="width: 180px;">
																	<span id="lblPartNo" class="clsLabel">Part No.</span>
																</td>
																<td align="left" style="width: 300px;">
																	<asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part No."
																		Text="<%# mItem.Name %>" MaxLength="50">
																	</asp:TextBox>
																</td>
																<td align="right" style="display: none">
																	<span id="lblNomenclature" class="clsLabelStar">*</span>
																</td>
																<td align="left" style="display: none;">
																	<span id="lblNomeclature" class="clsLabel">Nomenclature</span>
																</td>
																<td style="width: 231px; display: none">
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td align="left">
																				<asp:DropDownList ID="cmbNomenclature" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					DataValueField="ID" DataTextField="Name" onChange="setComboBoxValue(this,'N')">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<asp:Button ID="imgbtnNomenclature" runat="server" CssClass="clsButtonGrid_Ajax"
																					ToolTip="Click to Add New Nomenclature" Text="..." CausesValidation="False"></asp:Button>
																			</td>
																		</tr>
																	</table>
																	<%-- <asp:CustomValidator ID="cvNomenclature" ValidationGroup="1" runat="server" Display="None"
                                                                    ErrorMessage="Select Nomenclature from the list." ControlToValidate="cmbNomenclature"
                                                                    ClientValidationFunction="ValidateNomenclature" />--%>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblName1" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="lblDescription" class="clsLabel">Description </span>
																</td>
																<td colspan="4">
																	<asp:TextBox ID="txtDescription" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearch"
																		ToolTip="Enter Description" Text="<%# mItem.Description %>" MaxLength="100" TextMode="MultiLine"
																		Width="617px" Rows="3"></asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblUnit1" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="lblUnit" class="clsLabel">Unit </span>
																</td>
																<td>
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:DropDownList ID="cmbUnit" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					DataValueField="ID" DataTextField="Name" onChange="setComboBoxValue(this,'U')"
																					Enabled="<%# mItem.AllowSerialize and mItem.AlternatePartNos.Count = 0 and mItem.CountForItemUseInOrderAndReq = 0 %>">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<%--<asp:Button ID="imgbtnUnit" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Unit"
                                                                                Text="..." CausesValidation="False"></asp:Button>--%>

																				<asp:ImageButton ID="imgbtnUnit" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					ToolTip="Click to Add New Unit" CausesValidation="False"></asp:ImageButton>

																			</td>
																	</table>
																	<asp:CustomValidator ValidationGroup="1" ID="cvUnit" runat="server" Display="None"
																		ErrorMessage="Select part unit from the list." ControlToValidate="cmbUnit" ClientValidationFunction="ValidateUnit" />
																</td>
																<td align="right">
																	<span id="lblCategory1" class="clsLabelStar">*</span>
																</td>
																<td align="left">
																	<span id="lblCategory" class="clsLabel">Category </span>
																</td>
																<td>
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:DropDownList ID="cmbCategory" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					DataValueField="ID" DataTextField="Name" onChange="setComboBoxValue(this,'C')"
																					Enabled="<%# mItem.AlternatePartNos.Count=0 %>">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<%--<asp:Button ID="imgbtnCategory" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Category"
                                                                                Text="..." CausesValidation="False"></asp:Button>--%>

																				<asp:ImageButton ID="imgbtnCategory" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					ToolTip="Click to Add New Category" CausesValidation="False"></asp:ImageButton>

																			</td>
																		</tr>
																	</table>
																	<asp:CustomValidator ValidationGroup="1" ID="cvCategory" runat="server" Display="None"
																		ErrorMessage="Select Category from the list." ControlToValidate="cmbCategory"
																		ClientValidationFunction="ValidateCategory" />
																</td>
															</tr>
															<tr>
																<td></td>
																<td>
																	<span id="Span8" class="clsLabel">Serial No. Required</span>
																</td>
																<td>
																	<asp:CheckBox ID="chkSerialisedStatus" runat="server" CssClass="clsLabelAuto" Enabled="<%# mItem.AllowSerialize and mItem.AlternatePartNos.Count = 0 %>"
																		Checked="<%# mItem.SerialisedStatus %>" TextAlign="Left"></asp:CheckBox>
																</td>
																<td align="right"></td>
																<td>
																	<span id="lblLocation" class="clsLabel">Location </span>
																</td>
																<td>
																	<asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Location"
																		Text="<%# mItem.Location %>" MaxLength="50">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td style="width: 0px;">
																	<span id="lblABCType1" class="clsLabelStar" style="display: none;">* </span>
																</td>
																<td style="width: 180px;">
																	<span id="lblABCType" class="clsLabel">ABC Type</span>
																</td>
																<td align="left" style="width: 300px;">
																	<asp:DropDownList ID="cmbABCType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																		ClientIDMode="Static" DataTextField="Name" onChange="setComboBoxValue(this,'ABC')">
																	</asp:DropDownList>
																</td>
																<td></td>
																<td>
																	<span id="lblFolioNo" class="clsLabel">Folio No.</span>
																</td>
																<td style="width: 231px;">
																	<asp:TextBox ID="txtFolio" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Folio No."
																		Text="<%# mItem.Folio %>" MaxLength="8">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblStarATA" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="lblAltType" class="clsLabel">Part Type </span>
																</td>
																<td>
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td></td>
																			<td></td>
																		</tr>
																	</table>
																	<asp:DropDownList ID="cmbAltType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																		ClientIDMode="Static" DataTextField="Name" onChange="setComboBoxValue(this,'P')">
																	</asp:DropDownList>
																	<asp:CustomValidator ValidationGroup="1" ID="cvAltType" runat="server" Display="None"
																		ErrorMessage="Select Part Type." ControlToValidate="cmbAltType" />
																</td>
																<td align="right"></td>
																<td style="width: 120px;">
																	<span id="lblATAChapter" class="clsLabel">ATA Chapter </span>
																</td>
																<td valign="top">
																	<table id="Table8" border="0" cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:DropDownList ID="cmbATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																					DataTextField="ATAChapter" onChange="setComboBoxValue(this,'ATA')">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<%--<asp:Button ID="imgBtnATAChapter" ClientIDMode="Static" runat="server" CssClass="clsButtonGrid_Ajax"
                                                                                ToolTip=" Add New ATA Chapter  to the list" Text="..." CausesValidation="False"></asp:Button>--%>

																				<asp:ImageButton ID="imgBtnATAChapter" runat="server" ClientIDMode="Static" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					ToolTip=" Add New ATA Chapter  to the list " CausesValidation="False"></asp:ImageButton>

																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="IPCReferenceStar" class="clsLabelStar" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "STR", True, False) %>'>*</span>
																</td>
																<td>
																	<span id="Span2" class="clsLabel">IPC Reference</span>
																</td>
																<td>
																	<asp:TextBox ID="txtIPCReference" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter IPC Reference"
																		Text="<%# mItem.IPCReference %>" MaxLength="50">
																	</asp:TextBox>
																</td>
																<td align="right"></td>
																<td style="width: 120px;">
																	<span id="lblBinCardNumber" class="clsLabel">Bin Card No.</span>
																</td>
																<td valign="top">
																	<asp:TextBox ID="txtBinCardNumber" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Card Number"
																		Text="<%# mItem.BinCardNumber %>" MaxLength="50">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="Span1" class="clsLabelStar"></span>
																</td>
																<td>
																	<span id="Span4" class="clsLabel">Item Tag</span>
																</td>
																<td>
																	<asp:DropDownList ID="cmbItemTag" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																		ClientIDMode="Static" DataTextField="Name" onChange="setComboBoxValue(this,'IT')">
																	</asp:DropDownList>
																</td>
																<td align="right"></td>
																<td style="width: 120px;">
																	<span id="lblApproxRate" class="clsLabel">Approx. Rate </span>
																</td>
																<td valign="top">
																	<asp:TextBox ID="txtApproxRate" runat="server" CssClass="clsTextBoxTagSearch"
																		ToolTip="Enter Approximate Rate" Text="<%# mItem.Rate %>" MaxLength="12">
																	</asp:TextBox>
																</td>
															</tr>
															<tr>
																<td></td>
																<td>
																	<span id="lblManufacturer" class="clsLabel">Manufacturer </span>
																</td>
																<td>
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					DataValueField="ID" DataTextField="Name" onChange="setComboBoxValue(this,'Manufacturer')">
																				</asp:DropDownList>
																			</td>
																			<td>
																				<%--      <asp:Button ID="imgbtnManufacturer" runat="server" CssClass="clsButtonGrid_Ajax"
                                                                                ToolTip="Click to Add New Manufacturer" Text="..." CausesValidation="False"></asp:Button>--%>


																				<asp:ImageButton ID="imgbtnManufacturer" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					ToolTip="Click to Add New Manufacturer" CausesValidation="False"></asp:ImageButton>

																			</td>
																		</tr>
																	</table>
																</td>
																<td align="right"></td>
																<td style="width: 120px;">
																	<span id="lblKit" class="clsLabel">Kit </span>
																</td>
																<td valign="top">
																	<table id="Table7" border="0" cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:CheckBox ID="chkStatusKit" runat="server" CssClass="clsCheckBox" Enabled="<%# Not mItem.IsNew %>"
																					Checked="<%# mItem.StatusKit %>"></asp:CheckBox>
																			</td>
																			<td>
																				<%--<asp:Button ID="imgbtnKit" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Kit"
                                                                                Text="..." CausesValidation="False" Enabled="<%# Not mItem.IsNew %>"></asp:Button>--%>


																				<asp:ImageButton ID="imgbtnKit" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																					Enabled="<%# Not mItem.IsNew %>" ToolTip="Click to Add New Kit" CausesValidation="False"></asp:ImageButton>

																				<%--Enabled="<%# Not mItem.IsNew %>"--%>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
															<tr>
																<td></td>
																<td>
																	<span id="lblHSNACS" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("HSNACSCodeVisibleInPartMaster") = "True", True, False) %>'>HSN/ACS </span>
																</td>
																<td>
																	<table cellspacing="0" cellpadding="0">
																		<tr>
																			<td>
																				<asp:DropDownList ID="cmbHSNACSList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																					DataTextField="Code" onChange="setComboBoxValue(this,'HSNACS')" Enabled="<%# mItem.UsedInCount=0 %>"
																					Visible='<%# IIf(AppSettings("HSNACSCodeVisibleInPartMaster") = "True", True, False) %>'>
																				</asp:DropDownList>
																			</td>
																			<td></td>
																		</tr>
																	</table>
																</td>
																<td></td>
																<td>
																	<span id="Span15" class="clsLabel" runat="server" visible="false">Essential Catagory
																	</span>
																</td>
																<td>
																	<asp:DropDownList ID="cmbEssentialCatagory" Visible="false" runat="server" AutoPostBack="true"
																		CssClass="clsTextBoxTagSearchComboNewstyle">
																		<asp:ListItem Text="Go" Value="0"> Go</asp:ListItem>
																		<asp:ListItem Text="No Go" Value="1">No Go</asp:ListItem>
																		<asp:ListItem Text="Go If" Value="2">Go If</asp:ListItem>
																	</asp:DropDownList>
																</td>
															</tr>
														</table>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<!--Start pane Expiry / Benchmark /Calibration info -->
									<tr class="clsCollapsePanel">
										<td style="width: 100%">
											<asp:Panel ID="ClpnlExpiryBenchcheckCalibrationInformation" runat="server" Style="border: none; width: 100%">
												<div>
													<div style="float: left; vertical-align: middle;">
														<table width="100%">
															<tr>
																<td>
																	<span style="vertical-align: middle; font-weight: bold; margin-left: 2px; width: 100%"
																		id="Span3" class="clsLabel">Tooling Details</span>
																</td>
																<td align="right">
																	<div style="float: right; vertical-align: middle; margin-right: 5px;">
																		<image id="imgMasters2" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																	</div>
																</td>
															</tr>
														</table>
													</div>
												</div>
											</asp:Panel>
										</td>
									</tr>
									<!--update pane Expiry / Benchmark /Calibration info -->
									<tr>
										<td>
											<asp:UpdatePanel runat="server" ID="upnlExpBencCal" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Panel ID="pnlExpiryBenchcheckCalibrationInformation" runat="server" Style="max-height: 300px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
														<div style="width: 100%;">
															<table style="width: 100%;" border="0">
																<tr>
																	<td style="width: 0px;"></td>
																	<td style="width: 180px;">
																		<span id="Span5" class="clsLabelAuto">For Calibration </span>
																	</td>
																	<td align="left" style="width: 300px;">
																		<asp:CheckBox ID="chkStatusGroundEquipment" runat="server" AutoPostBack="true" Checked="<%# mItem.StatusEquipment %>"
																			CssClass="clsRadioButton" GroupName="a" Text="Gro. Equi./Calibration" />
																	</td>
																	<td></td>
																	<td>
																		<span id="Span11" class="clsLabelAuto">Interval </span>
																	</td>
																	<td style="width: 231px;">
																		<table id="Table18" border="0" cellspacing="0" cellpadding="0">
																			<tr>
																				<td>
																					<span id="lblBenchchecked" class="clsLabelAuto">
																						<asp:TextBox ID="txtBenchmarkMonths" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																							Enabled="<%# mItem.CalibrationCount = 0 %>" MaxLength="4" Text="<%# mItem.BenchmarkMonths %>"
																							Width="38px"></asp:TextBox>
																					</span>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbCalibrationPeriodIn" runat="server" ClientIDMode="Static"
																						CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Enabled="<%# mItem.CalibrationCount = 0 %>"
																						onChange="setComboBoxValue(this,'CP')" Width="80px">
																					</asp:DropDownList>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td style="width: 0px;"></td>
																	<td style="width: 180px;">
																		<span id="Span12" class="clsLabelAuto" style="display: none">For Condition Check
																		</span>
																	</td>
																	<td align="left" style="width: 300px;">
																		<asp:CheckBox ID="chkConditionCheck" runat="server" AutoPostBack="true" Checked="<%# mItem.IsConditionCheck %>"
																			CssClass="clsRadioButton" Enabled="<%# mItem.CalibrationCount = 0 %>" GroupName="a"
																			Text="Condition Check" Visible="false" />
																	</td>
																	<td></td>
																	<td>
																		<span id="Span9" class="clsLabelAuto" style="display: none">Interval </span>
																	</td>
																	<td style="width: 231px;">
																		<table id="Table19" border="0" cellspacing="0" cellpadding="0">
																			<tr>
																				<td>
																					<asp:TextBox ID="txtConditionCheckInterval" runat="server" ClientIDMode="Static"
																						Checked="<%# mItem.StatusEquipment %>" CssClass="clsTextBoxRightAlignSmall_Ajax"
																						Enabled="<%# mItem.CalibrationCount = 0 %>" MaxLength="4" Text="<%# mItem.ConditionCheckInterval %>"
																						Width="38px" Visible="false"></asp:TextBox>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbConditionCheckIntervalIn" runat="server" ClientIDMode="Static"
																						CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Enabled="<%# mItem.CalibrationCount = 0 %>"
																						onChange="setComboBoxValue(this,'CCIn')" Width="80px" Visible="false">
																					</asp:DropDownList>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td style="width: 0px;"></td>
																	<td style="width: 180px;">
																		<span id="Span13" class="clsLabelAuto">Equipment Maintenance </span>
																	</td>
																	<td align="left" style="width: 300px;">
																		<asp:CheckBox ID="chkServicedInspected" runat="server" AutoPostBack="true" CssClass="clsRadioButton"
																			Checked="<%# mItem.IsServicedInspected Or mItem.IsConditionCheck %>" GroupName="a"
																			Text="Condition/Service/Inspection" />&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="imgbtnServiceInspections" runat="server" ImageUrl="~/images/plus1.png"
																		Height="22px" Width="24px" ToolTip="Click to Add New Service Inspections" Visible="<%# mItem.IsServicedInspected Or mItem.IsConditionCheck %>"
																		CausesValidation="true"></asp:ImageButton>
																	</td>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="Span6" class="clsLabelAuto" style="display: none">Interval </span>
																	</td>
																	<td style="width: 231px;">
																		<table id="Table5" border="0" cellspacing="0" cellpadding="0">
																			<tr>
																				<td>
																					<asp:TextBox ID="txtServicedInspected" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
																						Enabled="<%# mItem.CalibrationCount = 0 %>" MaxLength="4" Text="<%# mItem.ServicedInspectedInterval %>"
																						Width="38px" Visible="false"></asp:TextBox>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbServicedInspectedInterval" runat="server" ClientIDMode="Static"
																						CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Enabled="<%# mItem.CalibrationCount = 0 %>"
																						onChange="setComboBoxValue(this,'SIIn')" Width="80px" Visible="false">
																					</asp:DropDownList>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblToolType" class="clsLabel">Tool Type </span>
																	</td>
																	<td style="width: 200px;">
																		<asp:DropDownList ID="cmbToolType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																			ClientIDMode="Static" DataTextField="Name" onChange="setComboBoxValue(this,'TT')"
																			Width="100px">
																		</asp:DropDownList>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="lblMaxUses" class="clsLabel">Max Uses </span>
																	</td>
																	<td valign="top">
																		<table id="Table4" border="0" cellspacing="0" cellpadding="0">
																			<tr>
																				<td></td>
																				<td>
																					<asp:TextBox ID="txtMaxUses" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																						ToolTip="Enter Max Uses" Text="<%# mItem.MaxNoOfUses %>" MaxLength="5">
																					</asp:TextBox>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblCalibrationStandard" class="clsLabelAuto">Calibration Standard</span>
																	</td>
																	<td colspan="4">
																		<asp:TextBox ID="txtCalibrationStandard" runat="server" CssClass="clsTextBoxTagSearch"
																			ToolTip="Enter Calibration Standard" Text="<%# mItem.CalibrationStandard %>"
																			Width="617px" MaxLength="200" Rows="3">
																		</asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="Label1" class="clsLabelAuto">Range (Specification)</span>
																	</td>
																	<td colspan="4">
																		<asp:TextBox ID="txtSpecification" runat="server" CssClass="clsTextBoxTagSearch"
																			Width="617px" ToolTip="Enter Note" Text="<%# mItem.Specification %>" MaxLength="150" Rows="3">
																		</asp:TextBox>
																	</td>
																</tr>
															</table>
														</div>
													</asp:Panel>
													<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
														ClientIDMode="Static" runat="Server" TargetControlID="pnlExpiryBenchcheckCalibrationInformation"
														ExpandControlID="ClpnlExpiryBenchcheckCalibrationInformation" CollapseControlID="ClpnlExpiryBenchcheckCalibrationInformation"
														Collapsed="True" ImageControlID="imgMasters2" CollapsedSize="0" ExpandedText="(Hide Details...)"
														CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
														CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<!-- End  -->
									<!--End update panel details -->
									<!--Start Additional Information -->
									<tr class="clsCollapsePanel">
										<td style="width: 100%">
											<asp:Panel ID="ClpnlAdditionalInformation" runat="server" Style="border: none; width: 100%">
												<div>
													<div style="float: left; vertical-align: middle;">
														<table width="100%">
															<tr>
																<td>
																	<span style="vertical-align: middle; font-weight: bold; margin-left: 2px; width: 100%"
																		id="lblMastersSelection" class="clsLabel">Additional Information</span>
																</td>
																<td align="right">
																	<div style="float: right; vertical-align: middle; margin-right: 5px;">
																		<image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																	</div>
																</td>
															</tr>
														</table>
													</div>
												</div>
											</asp:Panel>
										</td>
									</tr>
									<!--update panel Additional Information -->
									<tr>
										<td>
											<asp:UpdatePanel runat="server" ID="upnlAdditionalInformation" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Panel ID="pnlAdditionalInformation" runat="server" Style="max-height: 300px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
														<div style="width: 100%;">
															<table style="width: 100%;" border="0">
																<tr>
																	<td style="width: 0px;"></td>
																	<td style="width: 180px;">
																		<span id="lblExpire" class="clsLabelAuto">Expiry Period In Months</span>
																	</td>
																	<td align="left" style="width: 300px;">
																		<asp:TextBox ID="txtExpiryMonths" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																			ToolTip="Enter Expiry Month(s)" Text="<%# mItem.ExpiryMonths %>" MaxLength="5"
																			AutoPostBack="True">
																		</asp:TextBox>
																	</td>
																	<td></td>
																	<td>
																		<span id="lblquarter" style="margin-left: 3px;" class="clsLabelAuto">In Quarters
																		</span>
																	</td>
																	<td style="width: 231px;">
																		<asp:TextBox ID="txtExpiryQuaters" runat="server" 
																			CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																			ToolTip="Enter Expiry Quarter" 
																			Text="<%# mItem.ExpiryQuaters %>" MaxLength="5"
																			AutoPostBack="True">
																		</asp:TextBox>
																		<asp:CustomValidator ID="cvExpQrts" ValidationGroup="1"
																			runat="server" Display="None"
																			ControlToValidate="txtExpiryQuaters"
																			OnServerValidate="customvalidate" />
																		<asp:CheckBox ID="chkIsExpiryItem" runat="server" 
																			Checked="<%# mItem.IsExpiryItem %>"
																			Text="Expiry Item" CssClass="clsLabelAuto" 
																			TextAlign="Left" />
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblStorageLife" class="clsLabelAuto">Storage Life In Month </span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtStorageLife" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																			ClientIDMode="Static" ToolTip="Enter Storage Life in Month(s)" Text="<%# mItem.StorageLife %>"
																			MaxLength="5">
																		</asp:TextBox>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="lblAMMCMMRef" class="clsLabelAuto">AMM/CMM Reference</span>
																	</td>
																	<td valign="top">
																		<asp:TextBox ID="txtAMMCMMReference" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter AMM/CMM Reference"
																			Text="<%# mItem.AMMCMMReference %>" MaxLength="200" Width="225px" TextMode="MultiLine">
																		</asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblMinStockLevel" class="clsLabel">Min Level</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtMinStockLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
																			Style="text-align: right" ToolTip="Enter Min Stock Level" Text="<%# mItem.MinStockLevel %>" MaxLength="8">
																		</asp:TextBox>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="lblMaxStockLevel" class="clsLabel">Max Level</span>
																	</td>
																	<td valign="top">
																		<asp:TextBox ID="txtMaxStockLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
																			Style="text-align: right" ToolTip="Enter Max Stock Level" Text="<%# mItem.MaxStockLevel %>" MaxLength="8">
																		</asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblIsConsiderForReOrder" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS", False, True) %>'>Re-Order Level</span>
																	</td>
																	<td>
																		<asp:CheckBox ID="chkIsConsiderForReOrder" runat="server" CssClass="clsLabelAuto"
																			Visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS", False, True) %>'
																			ClientIDMode="Static" AutoPostBack="true" Checked="<%# mItem.IsConsiderForReOrder %>"
																			TextAlign="right"></asp:CheckBox>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="lblReOrder" class="clsLabel">Re-Ord. Level</span>
																	</td>
																	<td valign="top">
																		<asp:TextBox ID="txtReOrderLevel" runat="server" CssClass="clsTextBoxTagSearchSmall"
																			Style="text-align: right" Enabled="false" ToolTip="Enter Re Order Level" Text="<%# mItem.MinReOrderLevel %>"
																			MaxLength="8">
																		</asp:TextBox>
																	</td>
																</tr>
																<%-- Added by Shital on 05-Mar-2021--%>
																<tr>
																	<td></td>
																	<td>
																		<span id="Span14" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "STR", True, False) %>'>Re-Order Qty.</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtReOrderQty" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
																			ToolTip="Enter Re Order Level" Text="<%# mItem.ReOrderQty %>" Visible='<%# IIf(AppSettings("ClientCode") = "STR", True, False) %>'
																			MaxLength="8">
																		</asp:TextBox>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;"></td>
																	<td valign="top"></td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="Span10" class="clsLabel">Show in Valuation Report </span>
																	</td>
																	<td>
																		<asp:CheckBox ID="chkValuationStatus" runat="server" Checked="<%# mItem.ValuationStatus %>"
																			CssClass="clsLabelAuto" TextAlign="right" />
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="Span7" class="clsLabel">One Time Purchase </span>
																	</td>
																	<td valign="top">
																		<asp:CheckBox ID="chkIsOneTimePurchase" runat="server" CssClass="clsLabelAuto" Checked="<%# mItem.IsOneTimePurchase %>"
																			TextAlign="right"></asp:CheckBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblA" class="clsLabel">Airworthiness check Required </span>
																	</td>
																	<td>
																		<asp:CheckBox ID="chkAirworthiness" runat="server" Checked="<%# mItem.IsAirworthiCheck %>"
																			CssClass="clsLabelAuto" TextAlign="right" />
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;">
																		<span id="lblStockStatus" class="clsLabelAuto">Show in Stock Report</span>
																	</td>
																	<td valign="top">
																		<asp:CheckBox ID="chkStockStatus" runat="server" Checked="<%# mItem.StockStatus %>"
																			CssClass="clsLabelAuto" TextAlign="right" />
																	</td>
																</tr>
																<tr>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="lblNot" class="clsLabel">Part Not In Use</span>
																	</td>
																	<td>
																		<asp:CheckBox ID="chkNotInUse1" ClientIDMode="Static" runat="server" CssClass="clsLabelAuto"
																			AutoPostBack="True" Checked="<%# mItem.NotInUse %>" TextAlign="right" Width="168px"></asp:CheckBox>
																	</td>
																	<td align="right">&nbsp;
																	</td>
																	<td style="width: 120px;">
																		<span id="lblNotInUseDate1" class="clsLabel">Not In Use Date</span>
																	</td>
																	<td valign="top">
																		<asp:TextBox ID="txtNotInUseDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																			onchange="ValidateDateText(this,'calNotInUseDate_CalendarExtender')" runat="server"></asp:TextBox>
																		<cc2:CalendarExtender ID="calNotInUseDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNotInUseDate"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="txtNotInUseDate" ID="Calender_watermarkextender"
																			runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																		<asp:CustomValidator ValidationGroup="1" ID="cvNotInUseDate" OnServerValidate="CustomValidate"
																			runat="server" ControlToValidate="txtNotInUseDate" Display="None" ErrorMessage="Enter Not In Use Date."
																			ClientValidationFunction="ValidateNotInUseDate" />
																	</td>
																</tr>
																<tr>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="lblNote" class="clsLabel">Note</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="500"
																			Width="280px" Rows="3" Text="<%# mItem.Note %>" TextMode="MultiLine" ToolTip="Enter Note">
																		</asp:TextBox>
																	</td>
																	<td align="right">&nbsp;
																	</td>
																	<td style="width: 120px;">
																		<span id="lblLifeComponent" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True , False) %>'>Life Component</span>
																	</td>
																	<td valign="top">
																		<asp:CheckBox ID="ChkLifeComponent" runat="server" Checked="<%# mItem.LifeComponent %>"
																			Visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS", True, False) %>'
																			CssClass="clsLabelAuto" TextAlign="right" />
																	</td>
																</tr>
																<tr>
																	<td>&nbsp;
																	</td>
																	<td>
																		<span id="lblMake" class="clsLabelAuto">Make (Model)</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtMake" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Note"
																			Width="280px" Text="<%# mItem.Make %>" MaxLength="100" Rows="3">
																		</asp:TextBox>
																	</td>
																	<td align="right">&nbsp;
																	</td>
																	<td style="width: 120px;">
																		<span id="lblContractedVendor" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True , False) %>'>Contracted Vendor</span>
																	</td>
																	<td valign="top">
																		<asp:DropDownList ID="cmbContractedVendor" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																			Visible='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS",True , False) %>'
																			DataValueField="ID" ClientIDMode="Static" DataTextField="Name" onChange="setComboBoxValue(this,'CV')">
																		</asp:DropDownList>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblManuallyUpdated" class="clsLabel" runat="server" visible='<%# IIf(AppSettings("ClientCode") = "BA", True, False) %>'>Manually Updated</span>
																	</td>
																	<td>
																		<asp:CheckBox ID="chkManuallyUpdated" ClientIDMode="Static" runat="server" CssClass="clsLabelAuto"
																			Visible='<%# IIf(AppSettings("ClientCode") = "BA", True, False) %>' Checked="<%# mItem.ManuallyUpdated %>"
																			TextAlign="right" Width="168px"></asp:CheckBox>
																	</td>
																	<td align="right"></td>
																	<td style="width: 120px;"></td>
																	<td valign="top"></td>
																</tr>
																<tr>
																	<td colspan="6" valign="top">
																		<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																			<legend class="clsFieldSet1"><b>File Attachments</b></legend>
																			<asp:UpdatePanel ID="upnlItemAttachment" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table>
																						<tr>
																							<td style="height: 15px">
																								<asp:UpdatePanel ID="upnldgItemAttachment" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<asp:GridView ID="dgItemAttachment" ToolTip="List of File Attachment(s)" runat="server"
																											CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
																											AllowPaging="False" AutoGenerateColumns="false">
																											<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																											<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																											<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																											<Columns>
																												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																												<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																												<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																													<HeaderStyle Width="10px"></HeaderStyle>
																												</asp:BoundField>
																												<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																												</asp:BoundField>
																												<asp:TemplateField HeaderText="File Name">
																													<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																													<ItemTemplate>
																														<asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
																															ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
																															Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
																													</ItemTemplate>
																												</asp:TemplateField>
																												<%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                                        CausesValidation="false" />
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                            </asp:TemplateField>--%>


																												<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																													<ItemTemplate>
																														<%-- <span id="button">Login</span>--%>
																														<div class="dropdown">
																															<div id="divd" class="dropdownbtn-content" runat="server">
																																<table id="T1" class="clsGridNew_Ajax">
																																	<tr>

																																		<td>
																																			<asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>'
																																				CommandName="View" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />

																																		</td>

																																		<td>
																																			<asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>' CausesValidation="false"
																																				CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																																		</td>



																																	</tr>
																																</table>
																															</div>
																															<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																																Style="cursor: pointer" />
																														</div>
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
																								<asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
																									Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false"></asp:ImageButton>
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</fieldset>
																	</td>
																</tr>
															</table>
														</div>
													</asp:Panel>
													<cc2:CollapsiblePanelExtender BehaviorID="clpBehaviour" ID="clp" ClientIDMode="Static"
														runat="Server" TargetControlID="pnlAdditionalInformation" ExpandControlID="ClpnlAdditionalInformation"
														CollapseControlID="ClpnlAdditionalInformation" Collapsed="True" ImageControlID="imgMasters"
														CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
														ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
														SuppressPostBack="false" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<!--End update panel Additional Information -->
									<%--<tr class="clsCollapsePanel">
                                    <td style="width: 100%">
                                        <asp:Panel ID="ClpnlStatusInformation" runat="server" Style="border: none; width: 100%">
                                            <div>
                                                <div style="float: left; vertical-align: middle;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span style="vertical-align: middle; font-weight: bold; margin-left: 2px; width: 100%"
                                                                    id="Span6" class="clsLabel">Not In Use Information</span>
                                                            </td>
                                                            <td align="right">
                                                                <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                    <image id="imgMasters1" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>--%>
									<%-- <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlNotInUse" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="width: 100%;">
                                                    <table style="width: 100%;" border="0">
                                                        <tr>
                                                            <td style="width: 100%" colspan="6" valign="top">
                                                                <asp:Panel ID="pnlStatusInformation" runat="server" Style="max-height: 200px; overflow-y: auto;
                                                                    overflow: auto; overflow-x: hidden;">
                                                                    <table style="width: 100%;" border="0">
                                                                        <tr>
                                                                            <td style="width: 0px;">
                                                                            </td>
                                                                            <td style="width: 180px;">
                                                                            </td>
                                                                            <td colspan="4">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                           
                                                                                        </td>
                                                                                        <td>
                                                                                            
                                                                                        </td>
                                                                                        <td>
                                                                                          
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <cc2:CollapsiblePanelExtender BehaviorID="clp" ID="clpStatusInformation" ClientIDMode="Static"
                                                                    runat="Server" TargetControlID="pnlStatusInformation" ExpandControlID="ClpnlStatusInformation"
                                                                    CollapseControlID="ClpnlStatusInformation" Collapsed="True" ImageControlID="imgMasters1"
                                                                    CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                                    ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                    SuppressPostBack="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>--%>
									<!--update panel Not in use -->
									<!--End -->
									<!--Dummy panel to open modelpopup for category/nomenclature-->
									<tr style="height: 0px;">
										<td style="height: 0px;">
											<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
												<ContentTemplate>
													<asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="..."
														CausesValidation="False" Style="display: none;"></asp:Button>
													<asp:Button ID="hdnimgbtnKit" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
														Style="display: none;"></asp:Button>
													<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
														CausesValidation="False" Style="display: none;"></asp:Button>
													<asp:Button ID="hdnimgBtnManufacturerChapter" ClientIDMode="Static" runat="server"
														Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<!--End -->
									<!--update pane action buttons -->
									<%--<tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <table id="Table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSave" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                ToolTip="Click to save the Part &amp; refresh the screen" Text="Save"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSaveNew" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Visible="false" ToolTip="Click to save the Part &amp; refresh the screen" Text="Save &amp; New">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSaveClose" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Visible="false" ToolTip="Click to save the Part &amp; close the screen" Text="Save &amp; Close">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>--%>
									<!--End -->
								</table>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</asp:Panel>
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
			<!-- File Upload Modal Dialog-->
			<%-- <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
            width: 100%;">
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
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
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
        </script>--%>
			<!-- End -->
		</div>
		<!-- hidden fields to set combobox selected values at client side -->
		<asp:HiddenField ID="NomenclatureValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="UnitValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="UnitNameValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="CategoryValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="ABCTypeValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="PartTypeValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="ATAValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnCalibrationPeriodIn" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnItemTag" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnConditionCheckIntervalIn" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnToolType" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="ManufacturerValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="HSNACSValue" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnServicedInspected" runat="server" ClientIDMode="Static" />
		<asp:HiddenField ID="hdnContractedVendor" runat="server" ClientIDMode="Static" />
		<!-- End-->
		<!-- javascript function to set combobox selected value to appropriate hidden field for Part Information-->
		<script type="text/javascript">
			function setComboBoxValue(elem, combo) {
				switch (combo) {
					//Nomenclature                                                                                                                                                                                                                                              
					case 'N':
						var id = $(":selected", elem).val();
						var text = $(":selected", elem).text();
						//set id to hidden field
						$("#NomenclatureValue").val(id);
						//set text of combo to the description text box
						$("#txtDescription").val(text);
						break;
					//Unit                                                                                                                                                                                                                                               
					case 'U':
						var id = $(":selected", elem).val();
						var text = $(":selected", elem).text();
						//set id to hidden field
						$("#UnitValue").val(id);
						//set text of combo to the description text box
						$("#UnitNameValue").val(text);
						break;
					//Category
					case 'C':
						var id = $(":selected", elem).val();
						var cmbtext = $(":selected", elem).text();
						var ddSupplier = document.getElementById("cmbCategory");
						//set id to hidden field
						$("#CategoryValue").val(id);
						var i = 0;
						var text = '';
                   <% For Each item1 In mCategoryList%>

                   <% If item1.PrimaryCategoryID = 2 Then%>
						text = ddSupplier[i].text;
						if (text == cmbtext) {
							//  document.getElementById("ClpnlExpiryBenchcheckCalibrationInformation").style.display = "none";
							document.getElementById("ClpnlExpiryBenchcheckCalibrationInformation").setAttribute("style", "display:block");
							break;
						};
						if (text !== cmbtext) {
							//document.getElementById("ClpnlExpiryBenchcheckCalibrationInformation").style.display = "block";
							document.getElementById("ClpnlExpiryBenchcheckCalibrationInformation").setAttribute("style", "display:none");
						};
                        <% End If%>
						i = i + 1;
                    <% Next%>

						break;
					//ABC Type                                                                                                                                                                                                                                                   
					case 'ABC':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#ABCTypeValue").val(id);
						break;
					//Part Type                                                                                                                                                                                                                                             
					case 'P':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#PartTypeValue").val(id);
						break;
					//ATA                                                                                                                                                                                                                                             
					case 'ATA':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#ATAValue").val(id);
						break;
					//Primary Category from Category window                                                                                                                                                                                                                                         
					case 'PC':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#PrimaryCategoryValue").val(id);
						break;
					//Calibration Period ID                                                                                                                                                                                                            
					case 'CP':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnCalibrationPeriodIn").val(id);
						break;
					//Item Tag ID                                                                                                                                                                                                            
					case 'IT':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnItemTag").val(id);
						break;
					//Condition Check Interval In ID                                                                                                                                                                                                              
					case 'CCIn':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnConditionCheckIntervalIn").val(id);
						break;
					//Tool Type                                                                                                                                                                                                             
					case 'TT':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnToolType").val(id);
						break;
					//Manufacturer                                                                                                                                                                                                                        
					case 'Manufacturer':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#ManufacturerValue").val(id);
						break;
					//HSNACS                                                                                                                                                                                                                         
					case 'HSNACS':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#HSNACSValue").val(id);
						break;
					//Serviced Inspected Interval In ID                                                                                                                                                                                                               
					case 'SIIn':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnServicedInspected").val(id);
						break;
					//Contracted Vendor ID                                                                                                                                                                                                               
					case 'CV':
						var id = $(":selected", elem).val();
						//set id to hidden field
						$("#hdnContractedVendor").val(id);
						break;
				}
			}
		</script>
		<!-- End-->
		<!-- Client side validation of Not In Use Date-->
		<script type="text/javascript">
			function ValidateNotInUseDate(source, args) {
				args.IsValid = true;
				if ($("#chkNotInUse1").attr("checked")) {
					var tempval = $.trim($("#txtNotInUseDate").val());
					if (!tempval) {
						args.IsValid = false;
						return;
					}
				}
			}
		</script>
		<!-- End-->
		<!-- Client side validation for comboboxes-->
		<script type="text/javascript">
			//Nomenclature
			function ValidateNomenclature(source, args) {
				args.IsValid = false;
				var dd = $get("cmbNomenclature");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
			//Unit
			function ValidateUnit(source, args) {
				args.IsValid = false;
				var dd = $get("cmbUnit");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
			//Category
			function ValidateCategory(source, args) {
				args.IsValid = false;
				var dd = $get("cmbCategory");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
			//Part Type
			function ValidatePartType(source, args) {
				args.IsValid = false;
				var dd = $get("cmbAltType");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;
				}
			}
		</script>
		<!-- End-->
		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'false' };
				$.ajax({
					type: "POST",
					url: "DateValidationHandler.ashx",
					//        contentType: "application/json",
					cache: false,
					data: params,
					async: false,
					beforeSend: OnBeforeSend,
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
		<!-- Category Popup-->
		<div style="display: none">
			<asp:Button runat="server" ID="btndummyCategory" Text="Dummy Category" />
		</div>
		<asp:Panel runat="server" ID="pnlCategory" Style="display: none">
			<div>
				<table class="clstablelistout" id="TABLE9">
					<tr>
						<td>
							<asp:Panel ID="Panel1" CssClass="clspnl1" runat="server">
								<table id="Table10" class="clsTablelistin">
									<tr>
										<td colspan="2" class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<asp:UpdatePanel ID="upnlCategoryTitle" UpdateMode="Conditional" runat="server">
															<ContentTemplate>
																<asp:Label ID="lblTitleCategory" CssClass="clsFormHeader" runat="server">Category Information [New]</asp:Label>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="right">
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="New"
																		ValidationGroup="2" CausesValidation="False" ToolTip="Click to Add New Category"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnSaveCategory" runat="server" CssClass="clsbtnH clsinfoH" ValidationGroup="2"
																		Text="Save" ToolTip="Click to Save" />
																</td>

																<td>
																	<asp:UpdatePanel ID="upnlCategoryActionBtn" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<table border="0" cellpadding="0" cellspacing="0">
																				<tr>
																					<td align="right">
																						<asp:Button ID="btnCategoryClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
																							TabIndex="0" Text="Close" ToolTip="Click to close the Category Information screen" />
																					</td>
																				</tr>
																			</table>
																		</ContentTemplate>
																		<Triggers>
																			<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="click" />
																		</Triggers>
																	</asp:UpdatePanel>
																</td>

															</tr>
														</table>
													</td>

												</tr>
											</table>

										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel ID="upnlCategoryValidations" UpdateMode="Conditional" runat="server">
												<ContentTemplate>
													<asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="2" runat="server"
														CssClass="clsValidationSummary"></asp:ValidationSummary>
													<asp:RequiredFieldValidator ValidationGroup="2" ID="rfvName" runat="server" CssClass="clsLabelAuto"
														ErrorMessage="Name Required ." ControlToValidate="txtName" Display="None">Name Required.</asp:RequiredFieldValidator>
													<asp:CustomValidator ValidationGroup="2" ID="cvGLCode" runat="server" CssClass="clsLabelAuto1"
														ControlToValidate="txtGLCode" ValidateEmptyText="true" Display="None" OnServerValidate="customvalidateForCategory" />
													<asp:CustomValidator ValidationGroup="2" ID="cvPrimCate" runat="server" ControlToValidate="cmbPrimaryCategory"
														CssClass="clsLabelAuto1" Display="None" ErrorMessage="Select Primary Category from the List."
														ClientValidationFunction="validateMdlPopupCategory" />
													</TD>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<%--  <tr>
                                    <td>
                                        <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New"
                                            ValidationGroup="2" CausesValidation="False" ToolTip="Click to Add New Category">
                                        </asp:Button>
                                    </td>
                                </tr>--%>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel ID="upnlCategoryDetails" UpdateMode="Conditional" runat="server">
												<ContentTemplate>
													<table border="0" width="100%">
														<tr>
															<td colspan="4">
																<span id="lblCategoryDetails" class="clsLabelHeader">Category Details</span>
															</td>
														</tr>
														<tr>
															<td align="right">
																<span id="Label3" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblName" class="clsLabelAuto">Name </span>
															</td>
															<td>
																<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCategory.Name %>"
																	ToolTip="Enter Name" MaxLength="50">
																</asp:TextBox>
															</td>
															<td align="right"></td>
														</tr>
														<tr>
															<td align="right">
																<asp:Label ID="lblStarGLCode" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
															</td>
															<td>
																<span id="lblGLCode" runat="server" class="clsLabelAuto">GL Code</span>
															</td>
															<td>
																<asp:TextBox ID="txtGLCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCategory.GLCode %>"
																	ToolTip="Enter GL Code" MaxLength="4">

																</asp:TextBox>
															</td>
															<td align="right"></td>
														</tr>
														<tr>
															<td align="right">
																<span id="lblPrimaryCategoryStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblPrimaryCategory" class="clsLabelAuto">Primary Category</span>
															</td>
															<td>
																<asp:DropDownList ID="cmbPrimaryCategory" runat="server" onChange="setComboBoxValue(this,'PC')"
																	CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static" DataTextField="Name" DataValueField="ID"
																	EnableViewState="false">
																</asp:DropDownList>
															</td>
															<td align="right">&nbsp;
															</td>
														</tr>
														<%--<tr>
                                                        <td colspan="3">
                                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSaveCategory" runat="server" CssClass="clsButton_Ajax" ValidationGroup="2"
                                                                Text="Save" ToolTip="Click to Save" />
                                                        </td>
                                                    </tr>--%>
													</table>
													<asp:HiddenField ID="PrimaryCategoryValue" runat="server" ClientIDMode="Static" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2" style="width: 538px; overflow-x: hidden;">
											<asp:UpdatePanel runat="server" ID="upnlCategoryGrid" UpdateMode="Conditional">
												<ContentTemplate>
													<div style="width: 100%; padding-bottom: 3px;">
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</div>
													<%--<div style="width: 520px;">
                                                    <table class="clsGrid clsdgHeader" style="width: 520px; border-collapse: collapse;
                                                        padding-right: 17px;" cellspacing="0">
                                                        <tr>
                                                            <td width="200px" class="clsdgHeader TextBreak">
                                                                <span>Category</span>
                                                            </td>
                                                            <td width="70px" class="clsdgHeader TextBreak">
                                                                <span>GL Code</span>
                                                            </td>
                                                            <td width="130px" class="clsdgHeader TextBreak">
                                                                <span>Primary Category</span>
                                                            </td>
                                                            <td width="70px" class="clsdgHeader TextBreak">
                                                                <span class="clsdgHeader">Edit/View</span>
                                                            </td>
                                                            <td width="50px" class="clsdgHeader TextBreak">
                                                                <span class="clsdgHeader">Delete</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>
													<div style="width: 100%; max-height: 250px; overflow-y: auto; overflow-x: hidden;">
														<asp:GridView ID="gdvCategory" Width="520px" ShowHeader="true" runat="server" AutoGenerateColumns="False"
															EnableViewState="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true">
															<AlternatingRowStyle CssClass="clsdgAltItem TextBreak" />
															<RowStyle CssClass="clsdgItem TextBreak" />
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
															<Columns>
																<asp:BoundField DataField="ID" HeaderText="Category ID" Visible="False"></asp:BoundField>
																<asp:BoundField DataField="Name" HeaderText="Category">
																	<ItemStyle Width="200px" />
																</asp:BoundField>
																<asp:BoundField DataField="GLCode" HeaderText="GL Code">
																	<ItemStyle Width="70px" />
																</asp:BoundField>
																<asp:BoundField DataField="PrimaryCategoryName" HeaderText="Primary Category">
																	<ItemStyle Width="130px" />
																</asp:BoundField>
																<%--<asp:ButtonField CommandName="EditCategory" HeaderText="Edit/View" Text="Edit/View">
                                                                <ItemStyle Width="70px" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField CommandName="DeleteCategory" HeaderText="Delete" Text="Delete">
                                                                <ItemStyle Width="50px" />
                                                            </asp:ButtonField>--%>

																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png"
																				runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="ViewRec" CssClass="actionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Click to Edit record"
																								CommandName="EditCategory" ImageUrl="~/images/edit.png" CausesValidation="false" />
																						</td>
																						<td>
																							<asp:ImageButton ID="DeleteRec" class="actionICNS largerActionICNS" runat="server"
																								CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																								ToolTip="Click to Delete record"
																								CommandName="DeleteCategory" ImageUrl="~/images/delete.png" CausesValidation="false" />
																						</td>
																					</tr>
																				</table>
																			</div>
																		</div>
																	</ItemTemplate>
																</asp:TemplateField>



															</Columns>
														</asp:GridView>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<%--<tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel ID="upnlCategoryActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnCategoryClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                TabIndex="0" Text="Close" ToolTip="Click to close the Category Information screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>--%>
								</table>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpCategory" runat="server" TargetControlID="btndummyCategory"
			PopupControlID="pnlCategory" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function validateMdlPopupCategory(source, args) {
				args.IsValid = false;
				var dd = $get("cmbPrimaryCategory");
				if (dd.selectedIndex != 0) {
					args.IsValid = true;
					return;

				}
			}
		</script>
		<!-- End-->
		<!-- NomenClature Popup-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyNomenclature" Text="Dummy Nomenclature" />
		</div>
		<asp:Panel runat="server" ID="pnlNomenClature" Style="display: none; max-height: 450px;">
			<div>
				<table class="clstablelistout" id="Table11">
					<tr>
						<td>
							<asp:Panel ID="Panel2" runat="server" CssClass="clspanel1">
								<table class="clstablelistin" id="TABLE12">
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlNomenTitle" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Label ID="lblNomenTitle" CssClass="clstitle1" runat="server">Nomenclature [New]</asp:Label>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlNomenValidations" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:ValidationSummary ID="ValidationSummary3" ValidationGroup="3" runat="server"
														CssClass="clsValidationSummary"></asp:ValidationSummary>
													<asp:RequiredFieldValidator ID="rfvNomenName" runat="server" ValidationGroup="3"
														CssClass="clsLabelAuto" ErrorMessage="Name Required ." ControlToValidate="txtNomenName"
														Display="None">Name Required.</asp:RequiredFieldValidator>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td>
											<span id="Label5" class="clsLabelAuto">Click To Add New Record</span>
										</td>
										<td align="right">
											<asp:Button ID="btnNomenNew" ValidationGroup="3" runat="server" CssClass="clsButton_Ajax"
												CausesValidation="False" ToolTip="Click to add new Place" Text="New"></asp:Button>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlNomenDetails" UpdateMode="Conditional">
												<ContentTemplate>
													<table border="0" width="100%">
														<tr>
															<td colspan="4">
																<span id="lblNomenclatureDetails" class="clsLabelHeader">Nomenclature Details</span>
															</td>
														</tr>
														<tr>
															<td align="right">
																<span id="Label6" class="clsLabelStar">* </span>
															</td>
															<td>
																<span id="Label7" class="clsLabel">Name</span>
															</td>
															<td>
																<asp:TextBox ID="txtNomenName" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Name"
																	Text="<%# mNomenclature.Name %>" MaxLength="200">
																</asp:TextBox>
															</td>
															<td align="right"></td>
														</tr>
														<tr>
															<td colspan="3">
																<span id="Label8" class="clsLabelAuto">Click To Save Current Record</span>
															</td>
															<td align="right">
																<asp:Button ID="btnNomenSave" ValidationGroup="3" CssClass="clsButton_Ajax" runat="server"
																	ToolTip="Click to Save Nomenclature Information" Text="Save"></asp:Button>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2" style="width: 518px; overflow-x: hidden;">
											<asp:UpdatePanel runat="server" ID="upnlNomenGrid" UpdateMode="Conditional">
												<ContentTemplate>
													<div style="width: 100%; margin-bottom: 3px;">
														<asp:Label ID="lblNomenGridRecord" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</div>
													<div style="width: 500px;">
														<table class="clsGrid clsdgHeader" style="width: 500px; border-collapse: collapse; padding-right: 17px;"
															cellspacing="0">
															<tr>
																<td width="380px" class="clsdgHeader">
																	<span>Nomenclature</span>
																</td>
																<td width="70px" class="clsdgHeader">
																	<span class="clsdgHeader">Edit/View</span>
																</td>
																<td width="50px" class="clsdgHeader">
																	<span class="clsdgHeader">Delete</span>
																</td>
															</tr>
														</table>
													</div>
													<div style="width: 100%; max-height: 250px; overflow-y: auto; overflow-x: hidden;">
														<asp:GridView ID="gdvNomenclature" runat="server" ShowHeader="false" Style="width: 500px;"
															CssClass="clsGrid" AutoGenerateColumns="False" AllowPaging="True" PageSize="20"
															EnableViewState="false">
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle CssClass="paging" HorizontalAlign="Right" />
															<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
															<RowStyle CssClass="clsdgItem"></RowStyle>
															<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="Nomenclature ID"></asp:BoundField>
																<asp:BoundField DataField="Name" HeaderText="Nomenclature">
																	<ItemStyle Width="380px" />
																</asp:BoundField>
																<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditNomen">
																	<ItemStyle Width="70px" />
																</asp:ButtonField>
																<asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteNomen">
																	<ItemStyle Width="50px" />
																</asp:ButtonField>
															</Columns>
														</asp:GridView>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<table id="Table13" width="100%" cellspacing="0" cellpadding="0" align="right" border="0">
												<tr>
													<td valign="bottom" align="right">
														<asp:UpdatePanel runat="server" ID="upnlNomenActionBtns" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Button ID="btnNomenClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
																	CausesValidation="False" ToolTip="Click to close Nomenclature screen" Text="Close"></asp:Button>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="btnNomenNew" EventName="click" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupNomenclature" runat="server" TargetControlID="btnDummyNomenclature"
			PopupControlID="pnlNomenClature" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End -->
		<!-- ATA Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static" />
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
			$(document).ready(function () {
				$("#imgBtnATAChapter").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = "visible";
						$("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");
						if (!$.browser.msie) {
							$("#btnDummyATA").click();
							$get("AjaxLoader").style.visibility = "hidden";
						}

						return false;
					} catch (e) {
						alert(e);
					}


				});
			});
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
		<!-- Unit Popup -->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyUnit" />
		</div>
		<asp:Panel runat="server" ID="pnlUnit" Style="display: none; max-height: 450px;">
			<div>
				<table class="clstablelistout" id="Table14">
					<tr>
						<td>
							<asp:Panel ID="Panel3" runat="server" CssClass="clspanel1">
								<table class="clstablelistin" id="Table15">
									<tr>
										<td colspan="2" class="clsFormHeader1Newstyle">
											<table width="100%">
												<tr>
													<td>
														<asp:UpdatePanel runat="server" ID="upnlUnitTitle" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Label ID="lblUnitTitle" CssClass="clsFormHeader" runat="server">Unit [New]</asp:Label>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="right">
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnUnitNew" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																		Text="New" ToolTip="Click to add the new Unit" CausesValidation="False"></asp:Button>
																</td>
																<td align="right">
																	<asp:Button ID="btnUnitSave" CssClass="clsbtnH clsinfoH" runat="server" ValidationGroup="4"
																		Text="Save" ToolTip="Click to Save the Unit Information"></asp:Button>
																</td>
																<td valign="top" align="right">
																	<asp:Button ID="btnUnitClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
																		ToolTip="Click to close Unit screen" CausesValidation="False"></asp:Button>
																</td>
															</tr>
														</table>

													</td>
												</tr>
											</table>

										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlUnitValidations" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="4"
														CssClass="clsValidationSummary"></asp:ValidationSummary>
													<asp:RequiredFieldValidator ValidationGroup="4" ID="rfvUnitName" runat="server" CssClass="clsLabelAuto"
														Display="None" ControlToValidate="txtUnitName" ErrorMessage="Unit Required ."></asp:RequiredFieldValidator>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<%--<tr>
                                    <td>
                                        <span id="Label9" class="clsLabelAuto">Click To Add New Record</span>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnUnitNew" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                            Text="New" ToolTip="Click to add the new Unit" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>--%>
									<tr>
										<td colspan="2">
											<span id="lblUnitDetails" class="clsLabelHeader">Unit Details</span>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlUnitDetails" UpdateMode="Conditional">
												<ContentTemplate>
													<table border="0" cellpadding="0" width="100%">
														<tr>
															<td>
																<span id="Label10" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="Label11" class="clsLabel">Unit Name</span>
															</td>
															<td>
																<asp:TextBox ID="txtUnitName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mUnit.Name %>"
																	ToolTip="Enter Unit" MaxLength="15">
																</asp:TextBox>
															</td>
															<td align="right"></td>
														</tr>
														<%-- <tr>
                                                        <td colspan="3">
                                                            <span id="Label12" class="clsLabelAuto">Click To Save Current Record</span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnUnitSave" CssClass="clsButton_Ajax" runat="server" ValidationGroup="4"
                                                                Text="Save" ToolTip="Click to Save the Unit Information"></asp:Button>
                                                        </td>
                                                    </tr>--%>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2" style="width: 518px;">
											<asp:UpdatePanel runat="server" ID="upnlUnitGrid" UpdateMode="Conditional">
												<ContentTemplate>
													<div style="width: 100%; margin-bottom: 3px;">
														<asp:Label ID="lblUnitGridTitle" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</div>
													<%-- <div style="width: 500px;">
                                                    <table class="clsGrid clsdgHeader" style="width: 500px; border-collapse: collapse;
                                                        padding-right: 17px;" cellspacing="0">
                                                        <tr>
                                                            <td width="380px" class="clsdgHeader">
                                                                <span>Unit Name</span>
                                                            </td>
                                                            <td width="70px" class="clsdgHeader">
                                                                <span class="clsdgHeader">Edit/View</span>
                                                            </td>
                                                            <td width="50px" class="clsdgHeader">
                                                                <span class="clsdgHeader">Delete</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>
													<div style="width: 100%; max-height: 250px; overflow-y: auto; overflow-x: hidden;">
														<asp:GridView EnableViewState="True" ID="gdvUnit" ShowHeader="true" runat="server"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" Style="width: 500px;" AutoGenerateColumns="False">
															<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
															<RowStyle CssClass="clsdgItem"></RowStyle>
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="UnitID"></asp:BoundField>
																<asp:BoundField DataField="Name" HeaderText="Unit Name">
																	<ItemStyle Width="380px" />
																</asp:BoundField>
																<%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditUnit">
                                                                <ItemStyle Width="70px" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteUnit">
                                                                <ItemStyle Width="50px" />
                                                            </asp:ButtonField>--%>



																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png"
																				runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="Edit" CssClass="actionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Click to Edit record"
																								CommandName="EditUnit" ImageUrl="~/images/edit.png" />
																						</td>
																						<td>
																							<asp:ImageButton ID="Delete" class="actionICNS largerActionICNS" runat="server"
																								CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																								ToolTip="Click to Delete record"
																								CommandName="DeleteUnit" ImageUrl="~/images/delete.png" />
																						</td>
																					</tr>
																				</table>
																			</div>
																		</div>
																	</ItemTemplate>
																</asp:TemplateField>


															</Columns>
														</asp:GridView>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:UpdatePanel runat="server" ID="upnlUnitActionBtns" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="Table16" cellspacing="0" cellpadding="0" align="right" border="0">
														<%--<tr>
                                                        <td valign="top" align="right">
                                                            <asp:Button ID="btnUnitClose" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                ToolTip="Click to close Unit screen" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>--%>
													</table>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="btnUnitNew" EventName="click" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupUnit" runat="server" TargetControlID="btnDummyUnit"
			PopupControlID="pnlUnit" PopupDragHandleControlID="upnlUnitTitle" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End -->
		<!-- Kit Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyKit" Text="Dummy Kit" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlKit" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeKit" frameborder="0" height="100%" width="100%" allowtransparency="true"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupKit" runat="server" TargetControlID="btnDummyKit"
			PopupControlID="pnlKit" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameKitStateComplete() {
				$("#btnDummyKit").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenKitWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeKit").attr("src", "wfKit_Ajax.aspx?Type=pup");
					// $("#IframeKit").load(function () {
					//                    var doc = IframeKit.window;
					//                    IframeKit.SetPageLayout();

					if (!$.browser.msie) {
						$("#btnDummyKit").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}


					//});


					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForKit() {
				var kitwindow = $find("<%=mdlPopupKit.ClientID %>");
				//close kit popup window
				kitwindow.hide();
				//           release resources
				$("#IframeKit").attr("src", "JavaScript:''");
				//call kit image button
				$("#hdnimgbtnKit").click();
			}
		</script>
		<!-- End-->
		<%-- hide validation summary when server event occurs--%>
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
				//Page_ClientValidate();
				// ValidationSummaryOnSubmit();
				//Page_IsValid=true;
				//            Page_ClientValidate();
				//            if (Page_IsValid) {
				//                $("#ValidationSummary1").css('display', 'none');
				//            }

				if ((typeof (Page_ClientValidate) == 'function')) {
					if (Page_ValidationActive) {
						if (!ValidatorCommonOnSubmit()) {
							return false;
						}
						else {
							$(".clsValidationSummary").css('display', 'none');
							//ValidationSummaryOnSubmit();

						}
					}
				}
			});

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				EnableDisable();
			});
			//Enable Disable TextBoxes in a Row when the Row CheckBox is checked.
			function EnableDisable() {
				var IsBAClientCode = '<%=System.Configuration.ConfigurationSettings.AppSettings("ClientCode").ToString()%>';
				if (IsBAClientCode == "BA" || IsBAClientCode == "Novo") {
					//If the CheckBox is Checked then enable the TextBoxes in thr Row.
					var IsOneTimePurchase = $("#chkIsOneTimePurchase").is(":checked");
					if (!IsOneTimePurchase) {
						$("#txtMaxStockLevel,#txtMinStockLevel").removeAttr("disabled");
					} else {
						$("#txtMaxStockLevel,#txtMinStockLevel,#txtReOrderLevel").val('0');
						$("#txtMaxStockLevel,#txtMinStockLevel").attr("disabled", "disabled");
					}
				}

			}
		</script>
		<%-- End--%>
		<script type="text/javascript">
			function Onchecked() {
				var status = $("#chkStatusKit").attr('checked');
				if (status) {
					$('#imgbtnKit').removeAttr('disabled');
				}
				else {
					$('#imgbtnKit').attr('disabled', 'disabled');

				}
			}
		</script>
		<!-- Manufacturer Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyManufacturer" Text="Dummy Manufacturer" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupManufacturer" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupManufacturer" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
			PopupControlID="pnlPopupManufacturer" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameManufacturerStateComplete() {
				$("#btnDummyManufacturer").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			$(document).ready(function () {
				$("#imgbtnManufacturer").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = "visible";
						$("#iPopupManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");
						if (!$.browser.msie) {
							$("#btnDummyManufacturer").click();
							$get("AjaxLoader").style.visibility = "hidden";
						}
						return false;
					} catch (e) {
						alert(e);
					}
				});
			});
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForManufacturer() {
				var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
				//close Manufacturer popup window
				Manufacturerwindow.hide();
				$("#iPopupManufacturer").attr("src", "JavaScript:''");
				//call Manufacturer image button
				$("#hdnimgBtnManufacturerChapter").click();
			}
		</script>
		<!-- End-->
		<!-- Service Inspactions Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyServiceInspactions" />
		</div>
		<asp:Panel runat="server" ID="pnlServiceInspactions" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IServiceInspactions" allowtransparency="true" frameborder="0" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupServiceInspactions" runat="server" TargetControlID="btnDummyServiceInspactions"
			PopupControlID="pnlServiceInspactions" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameServiceInspactionsStateComplete() {
				$("#btnDummyServiceInspactions").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}


			function AddServiceInspections() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IServiceInspactions").attr("src", "wfItemServiceInspections_Ajax.aspx?Type=pup&MaintTypeID=1");

					//  if (!$.browser.msie) {
					$("#btnDummyServiceInspactions").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					// }

					return false;
				} catch (e) {
					alert(e);
				}


			}

		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForServiceInspactions() {
				var ServiceInspactionswindow = $find("<%=mdlPopupServiceInspactions.ClientID %>");
				//close Ass Insp Maint Done By Emp popup window
				ServiceInspactionswindow.hide();
				//Free resources
				$("#IServiceInspactions").attr("src", "JavaScript:''");
				$("#hdnBtnServiceInspactions").click();

			}
		</script>
		<!-- End -->
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
			function OpenFileUploadWindow() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
					//                        $("#IFileUpload").ready(function () {
					//                            $("#btnDummyFileUpload").click();
					//                            $get("AjaxLoader").style.visibility = 'hidden';
					//                        });
					//                if (!$.browser.msie) {
					//                    $("#btnDummyFileUpload").click();
					//                    $get("AjaxLoader").style.visibility = 'hidden';
					//                }

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
		<!-- End -->
		<!-- End File Upload Modal Dialog-->
	</form>
</body>
</html>

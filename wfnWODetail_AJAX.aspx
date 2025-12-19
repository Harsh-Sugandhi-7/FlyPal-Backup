<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWODetail_AJAX.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfnWODetail_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>Detail</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />

	<link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />

	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script src="/jquery.tooltip.min.js" type="text/javascript"></script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" id="clientEventHandlersJS">

		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openFilel() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openFile() {
			str = "wfExportToExcel.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

	</script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
		<asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="5400">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<script type="text/javascript">
			window.onload = blinknow;
			function blinknow() {
				var e = document.getElementById("<%=lblStatus.ClientID%>");

				e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
				setTimeout("blinknow();", 750);
			}

		</script>
		<%--AJAX- ScriptManager Added--%>
		<table id="Table-MaxWidth" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
			width="100%">
			<tr>
				<td valign="top" colspan="3">
					<table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
						<tr>
							<td class="clsFormHeader1Newstyle">
								<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table13" valign="top" border="0" cellspacing="1" cellpadding="1" width="100%">
											<tr>
												<td>
													<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">W.O. Detail</asp:Label>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table8" valign="top" border="0" cellspacing="1" cellpadding="1" width="100%">
											<tr>
												<td>
													<asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
														CssClass="clsValidationSummary" Width="100%"></asp:ValidationSummary>
													<asp:CustomValidator ID="cvWODate" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="txtWODate" ErrorMessage="Select W.O. Date"></asp:CustomValidator>
													<asp:RequiredFieldValidator ID="rfvWODate" runat="server" CssClass="clsLabelAuto"
														Display="None" ControlToValidate="txtWODate" ErrorMessage="Select W.O. Date."></asp:RequiredFieldValidator>
													<asp:RequiredFieldValidator ID="rfvWOCreatedBy" runat="server" CssClass="clsLabelAuto"
														Display="None" ControlToValidate="txtCreatedBy" ErrorMessage="Enter the Name who created Work Order"></asp:RequiredFieldValidator>
													<asp:CustomValidator ID="cvJobType" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="cmbJobType" ErrorMessage="Please select aircraft from the list."></asp:CustomValidator>
													<asp:CustomValidator ID="cvStartDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="txtStartDate"></asp:CustomValidator>
													<asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="txtRemark"></asp:CustomValidator>
													<asp:CustomValidator ID="cvPlanDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="txtPlanDate"></asp:CustomValidator>
													<asp:CustomValidator ID="cvCloseDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="txtCloseDate"></asp:CustomValidator>
													<asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
														Display="None"></asp:CustomValidator>
													<asp:CustomValidator ID="cvCurrentValue" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtNo"
														Display="None"></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtIssueTo"></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="cmbAircraftList"
														ErrorMessage="Please select aircraft from the list."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator3" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtSerialNo"
														ValidateEmptyText="true" ErrorMessage="Enter the Serial No."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator4" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtQcDate"></asp:CustomValidator>
													<asp:CustomValidator ID="custWorkshop" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ControlToValidate="cmbWorkShopList" ErrorMessage="Please select Location from the list."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator5" runat="server" CssClass="clsLabelAuto"
														ValidateEmptyText="true" OnServerValidate="customvalidate" Display="None" ControlToValidate="cmbAssembly"
														ErrorMessage="Please select Assembly from the list."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator6" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtLicenceNo"
														ValidateEmptyText="true" ErrorMessage="Enter the Serial No."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator7" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtLicenceNo2"
														ValidateEmptyText="true" ErrorMessage="Enter the Serial No."></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator8" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtNoOfSupplementalSheets"
														ValidateEmptyText="true" ErrorMessage=""></asp:CustomValidator>
													<asp:CustomValidator ID="CustomValidator9" runat="server" CssClass="clsLabelAuto"
														OnServerValidate="customvalidate" Display="None" ControlToValidate="txtNoOfNRCs"
														ValidateEmptyText="true" ErrorMessage=""></asp:CustomValidator>
													<!-- 'ALL27072020-->
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<asp:UpdatePanel ID="upnlStatusHeader" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="Table11" valign="top" style="width: 100%;">
										<%-- Sankalp 20-11-25 --%>
										<td align="right" style="vertical-align: top;">
											<div id="createdByAndUpdatedBy" runat="server">
												<td align="left" style="vertical-align: top;">
													<table id="T3" border="0" cellspacing="0">
														<tr>
															<td>
																<table align="right">
																	<tr>
																		<td>
																			<asp:Label ID="Label1" CssClass="clsLabel" Style="font-size: xx-small; color: navy"
																				runat="server" Text='<%# " Created By: " & mnWO.CreatedBy %>'></asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="Label6" CssClass="clsLabel" Style="font-size: xx-small; color: navy"
																				runat="server" Text='<%# " On: " & mnWO.CreateDateTimeStampFormatted %>'></asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="Label8" CssClass="clsLabel" Style="font-size: xx-small; color: navy"
																				runat="server" Text='<%# " Last Updated By: " & mnWO.LastUpdatedBy %>'></asp:Label>
																		</td>
																		<td>
																			<asp:Label ID="Label17" CssClass="clsLabel" Style="font-size: xx-small; color: navy"
																				runat="server" Text='<%#" On: " & mnWO.UpdateDateTimeStampFormatted %>'></asp:Label>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</td>
											</div>
										</td>
										<%-- end --%>

										<td align="right" style="vertical-align: top;">
											<asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader"
												Text="<%# mnWO.WOStatus %>" Font-Size="Medium"></asp:Label>
										</td>

									</table>
								</ContentTemplate>
							</asp:UpdatePanel>

						</tr>
						<asp:Panel ID="UsedForAllWO" runat="server">
							<tr>
								<td colspan="3">
									<table>
										<tr>
											<td valign="top">
												<table>
													<tr>
														<td>
															<asp:UpdatePanel ID="UpnlWODet" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset class="clsFieldSetNewStyle">
																		<legend id="ldwodetail" class="clsFieldSet1" runat="server">
																			<b>W.O. Detail</b>
																		</legend>
																		<table id="tblWODet" valign="top">
																			<tr>
																				<td colspan="1">
																					<asp:Label ID="lblWODateStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																				</td>
																				<td>
																					<asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">W.O.Date</asp:Label>
																				</td>
																				<td>
																					<table>
																						<tr>
																							<td>
																								<asp:TextBox ID="txtWODate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																									ReadOnly='<%#IIf(AppSettings("ClientCode") = "IND", True, False) %>'
																									onchange="ValidateDateText(this,'txtWODate_CalendarExtender');" Width="100px" />
																								<cc2:calendarextender id="txtWODate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																									enabled='<%#IIf(AppSettings("ClientCode") = "IND", False, True) %>'
																									format="<%$AppSettings:DateFormat%>" targetcontrolid="txtWODate" />
																								<cc2:textboxwatermarkextender id="TBWE2" runat="server" targetcontrolid="txtWODate"
																									watermarktext="<%$AppSettings:DateFormat%>" watermarkcssclass="clsDateTextBox" />

																								<asp:TextBox ID="txtWOTime" runat="server"
																									AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																									Visible='<%#IIf(AppSettings("ClientCode") = "IND" Or 
                                                                                                                    AppSettings("ClientCode") = "YA" Or 
                                                                                                                    AppSettings("ClientCode") = "AFC" Or 
                                                                                                                    AppSettings("ClientCode") = "ARA" Or 
                                                                                                                    AppSettings("ClientCode") = "BAP" Or
																													AppSettings("ClientCode") = "RPS" Or 
                                                                                                                    AppSettings("ClientCode") = "GLD", True, False) %>'
																									MaxLength="10" ToolTip="Enter Time" Width="65px" />
																								<cc2:maskededitextender id="txtWOTimeMaskedEditExtender"
																									targetcontrolid="txtWOTime" runat="server"
																									autocomplete="true" mask="99:99" masktype="Time"
																									culturename="en-us" messagevalidatortip="true" />
																							</td>
																							<td>
																								<asp:CheckBox ID="chkIsCritical" runat="server"
																									CssClass="clsCheckBox"
																									Visible='<%# AppSettings("ClientCode") = "STR" Or
                                                                                                                 AppSettings("ClientCode") = "KLP" Or
                                                                                                                 AppSettings("ClientCode") = "IRM" Or
                                                                                                                 AppSettings("ClientCode") = "GEP" Or
                                                                                                                 AppSettings("ClientCode") = "MEL" Or
                                                                                                                 AppSettings("ClientCode") = "BAP" %>'
																									Checked="<%# mnWO.IsCriticalWO %>" Text="Critical Work Order" />
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<asp:Label ID="Label4" runat="server" CssClass="clsLabelStar">*</asp:Label>
																				</td>
																				<td>
																					<asp:Label ID="lblText" runat="server" CssClass="clsLabelAuto">W.O.No.</asp:Label>
																				</td>
																				<td>
																					<asp:TextBox ID="txtText" runat="server" AutoComplete="off" ClientIDMode="Static"
																						CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOText %>" ToolTip="Enter Text" Width="180px" />
																					<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Width="60px"
																						Text="<%# mnWO.WONoStr %>" ToolTip="Enter No." MaxLength="5"></asp:TextBox>
																					<cc2:autocompleteextender clientidmode="Static" id="txtText_Autocomplete" runat="server"
																						delimitercharacters="" enabled="True" minimumprefixlength="0" completioninterval="1"
																						servicepath="wfnWODetail_AJAX.aspx" servicemethod="GetTextList" targetcontrolid="txtText"
																						usecontextkey="False" contextkey="" completionlistcssclass="ac_results_Main"
																						completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
																						onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
																						onclientshown="ClientHiding" onclientshowing="ClientShowing" />
																				</td>
																			</tr>
																			<asp:PlaceHolder runat="server" ID="phWorkShop">
																				<tr>
																					<td>
																						<asp:Label ID="lblWorkShopStar" runat="server" CssClass="clsLabelStar" Height="18px"
																							Width="8px" Visible='<%#IIf(AppSettings("ClientCode") = "IND" Or
																												        AppSettings("ClientCode") = "STR", True, False) %>'>
																								*
																						</asp:Label>
																					</td>
																					<td>
																						<asp:Label ID="lblWorkShop" runat="server" CssClass="clsLabelAuto">Location</asp:Label>
																					</td>
																					<td>
																						<asp:DropDownList ID="cmbWorkShopList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																							DataValueField="ID" DataTextField="LocationWorkShop" SelectedValue="<%# mnWO.WorkShopID %>"
																							onmouseover="ddlToolTip(this);" Width="186px" />
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<tr>
																				<td>
																					<asp:Label ID="lblstarPlanDate" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					&nbsp;
																				</td>
																				<td>
																					<asp:Label ID="lblPlanDate" runat="server" CssClass="clsLabelAuto">Plan Date</asp:Label>
																				</td>
																				<td>
																					<asp:TextBox ID="txtPlanDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																						onchange="ValidateDateText(this,'txtPlanDate_CalendarExtender');" Width="100px"></asp:TextBox>
																					<cc2:calendarextender id="txtPlanDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																						enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtPlanDate">
																					</cc2:calendarextender>
																					<cc2:textboxwatermarkextender id="TBWEtxtPlanDate" runat="server" targetcontrolid="txtPlanDate"
																						watermarkcssclass="clsDateTextBox" watermarktext="<%$AppSettings:DateFormat%>">
																					</cc2:textboxwatermarkextender>
																					<asp:TextBox ID="txtPlanDateTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
																						MaxLength="10" ToolTip="Enter Time" Visible='<%#IIf(AppSettings("ClientCode") = "IND", True, False) %>'
																						Width="65px"></asp:TextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<asp:Label ID="StarCreatedBy" runat="server" CssClass="clsLabelStar">*</asp:Label>
																				</td>
																				<td>
																					<asp:Label ID="lblCreatedBy" runat="server" CssClass="clsLabelAuto" Width="80px">Created By</asp:Label>
																				</td>
																				<td>
																					<asp:TextBox ID="txtCreatedBy" runat="server"
																						CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOBy %>"
																						ToolTip="Enter Created By" MaxLength="50" Width="180px" />
																				</td>
																			</tr>
																			<asp:PlaceHolder runat="server" ID="phRemark" 
																				Visible='<%#IIf(AppSettings("ShowAMOOnlyForNewClients") = "True" Or 
																							    Session("wfProject_Ajax") = "wfProject_Ajax", 
																									IIf(AppSettings("ClientCode") = "PTW", True, False), 
																									True) %>'>
																				<tr>
																					<td>&nbsp;
																					</td>
																					<td>
																						<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle1"
																							Text="<%# mnWO.WORemark %>" ToolTip="Enter Remark" MaxLength="500" TextMode="MultiLine">
																						</asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<asp:PlaceHolder runat="server" ID="phServiceProvider" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" And 
                                                                                                                                                  (mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102), 
                                                                                                                                               True, 
                                                                                                                                               False) %>'>
																				<tr>
																					<td></td>
																					<td colspan="1">
																						<asp:Label ID="Label15" runat="server" CssClass="clsLabelAuto">Service Provider</asp:Label>
																					</td>
																					<td>
																						<asp:DropDownList ID="cmbServiceProvider" runat="server"
																							CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
																							DataTextField="Name" DataValueField="ID" Width="186px"
																							SelectedValue="<%# mnWO.ServiceProviderID %>">
																						</asp:DropDownList>

																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<asp:PlaceHolder runat="server" ID="phIssueTo" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or 
                                                                                                                                           AppSettings("ShowAMOOnlyForNewClients") = "True" Or
                                                                                                                                           Session("wfProject_Ajax") = "wfProject_Ajax", 
                                                                                                                                       False, 
                                                                                                                                       True) %>'>
																				<tr>
																					<td></td>
																					<td colspan="2">
																						<asp:Label ID="lblIssueTo" runat="server" CssClass="clsLabelAuto">
                                                                                            Issue To Third Party Maintenance Agency
																						</asp:Label>
																					</td>
																				</tr>
																				<tr>
																					<td></td>
																					<td colspan="2">
																						<asp:TextBox ID="txtIssueTo" runat="server"
																							CssClass="clsTextBoxTagSearchMultilineNewstyle2"
																							Text="<%# mnWO.IssueTo %>" TextMode="MultiLine"
																							ToolTip="Enter WO. Issue To Information"
																							MaxLength="150">
																						</asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																		</table>
																	</fieldset>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<asp:PlaceHolder ID="phlinks" runat="server">
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlLinks" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle">
																			<legend class="clsFieldSet1"><%#IIf(mnWO.TransTypeID = 88, 
																											    "Spares / Tools / Requisition Details", 
																											    "Fuel / Spares / Tools / Requisition Details") %></legend>
																			<table>
																				<tr>
																					<td align="left">
																						<asp:LinkButton ID="lnkIssuedSpares" runat="server" CssClass="clsHyperlink1" ToolTip="Click to go on Issued Spares screen"
																							Enabled="<%# Not (mnWO.StatusID = 1) %>" Width="100px">Issued Spares</asp:LinkButton>
																					</td>
																					<td>
																						<asp:LinkButton ID="lnkIssuedTools" runat="server" Width="100px" CssClass="clsHyperlink1"
																							Enabled="<%# Not (mnWO.StatusID = 1) %>" ToolTip="Click to go on Issued Tools screen">Issued Tools</asp:LinkButton>
																					</td>
																					<td>
																						<asp:LinkButton ID="lnkViewIndent" runat="server" Width="160px" CssClass="clsHyperlink1"
																							ToolTip="Click to go on Requested Item(s) screen">Requisition Items</asp:LinkButton>
																					</td>
																				</tr>
																				<tr>
																					<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%#IIf(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102, True, False) %>'>
																						<td style="height: 21px" align="left" colspan="1">
																							<asp:LinkButton ID="lnkFuelDetail" runat="server" Width="100px" CssClass="clsHyperlink1"
																								Enabled="<%# Not mnWO.IsNew %>" ToolTip="Click to go on Fuel Detail screen">Fuel Detail</asp:LinkButton>
																						</td>
																					</asp:PlaceHolder>
																					<td>
																						<asp:LinkButton ID="lnkWOParameters" runat="server" Width="100px" CssClass="clsHyperlink1"
																							Visible='<%#IIf((AppSettings("WOParametersRequired") = "True") And (Not mnWO.IsNew), True, False) %>'
																							Enabled="<%# NOT (mnWO.StatusID = 1) %>"
																							ToolTip="Click to go on WO Parameters screen">
                                                                                          WO Parameters
																						</asp:LinkButton>
																					</td>
																					<td>
																						<div class="dropdown">
																							<span id="lblWOStatusList" class="dropbtn" style="color: Blue; text-decoration: underline;"
																								runat="server" visible='<%#IIf(AppSettings("ClientCode") = "IND", True, False) %>'>WO Stages(s) &#9660;</span>
																							<div class="dropdown-content" style="z-index: 1000000">
																								<div id="myDropdown">
																									<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:GridView ID="dgWOStages" ToolTip="WO Stage(s)" runat="server" CssClass="clsGridNewStyle"
																												Width="900px" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="5"
																												AllowPaging="False" AutoGenerateColumns="false">
																												<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																												<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																												<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																												<Columns>
																													<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																													<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																													<asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
																													<asp:BoundField DataField="ApprovedRejectStatusName" HeaderText="Authorized/Approved /Reject">
																														<HeaderStyle Wrap="false" />
																													</asp:BoundField>
																													<asp:BoundField DataField="DateFormatted" HeaderText="Date">
																														<HeaderStyle Wrap="false" />
																														<ItemStyle Wrap="false" />
																													</asp:BoundField>
																													<asp:BoundField DataField="DoneBy" HeaderText="Done By">
																														<HeaderStyle Wrap="false" />
																													</asp:BoundField>
																													<asp:BoundField DataField="Remark" HeaderText="Remark">
																														<HeaderStyle Wrap="True" />
																													</asp:BoundField>
																												</Columns>
																											</asp:GridView>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</div>
																							</div>
																						</div>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</asp:PlaceHolder>
													<tr>
														<td>
															<asp:UpdatePanel ID="UpdatePanel61" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<fieldset class="clsFieldSetNewStyle" id="fldRemark" runat="server"
																		visible='<%#IIf(AppSettings("ShowNewWOFlow") = "True", True, False) And (Not mnWO.IsNew) And (mnWO.StatusID > 1) %>'>
																		<legend class="clsFieldSet1"><b>Remark</b></legend>
																		<table>
																			<asp:PlaceHolder ID="phStatusRemark" runat="server">
																				<tr>
																					<td>
																						<asp:Label ID="lblstarStatusRemark" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						&nbsp;
																					</td>
																					<td align="left">
																						<asp:Label ID="lblStatusRemark" runat="server" CssClass="clsLabelAuto">Status Remark</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtStatusRemark" runat="server" CssClass="clsTextBoxTagSearch"
																							MaxLength="500" Height="25px" TextMode="MultiLine" ToolTip="Enter Remark"></asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<asp:PlaceHolder ID="PlaceHolder5" runat="server" Visible="false">
																				<tr>
																					<td>
																						<asp:Label ID="lblstarPlanningRemark" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						&nbsp;
																					</td>
																					<td align="left">
																						<asp:Label ID="lblPlanningRemark" runat="server" Visible="<%# (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (Not mnWO.WOStatusID = 4)  %>"
																							CssClass="clsLabelAuto">Planning Remark</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPlanningRemark" runat="server" CssClass="clsTextBoxTagSearch"
																							Text="<%# mnWO.PlanningRemark %>" MaxLength="500" Height="25px" TextMode="MultiLine"
																							Visible="<%# (NOT mnWO.IsNew) And (mnWO.StatusID = 2) And (Not mnWO.WOStatusID = 4)  %>"
																							ToolTip="Enter PPC Remark"></asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<asp:PlaceHolder ID="PlaceHolder6" runat="server" Visible="false">
																				<tr>
																					<td>
																						<asp:Label ID="lblstarPPCRemark" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						&nbsp;
																					</td>
																					<td align="left">
																						<asp:Label ID="lblPPCRemark" runat="server" Visible="<%# Not mnWO.IsNew And mnWO.WOJobs.IsCompleted = True And (mnWO.StatusID = 1) And (mnWO.WOStatusID = 5)  %>"
																							CssClass="clsLabelAuto">PPC Remark</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPPCRemark" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.PPCRemark %>"
																							MaxLength="500" Height="25px" TextMode="MultiLine" Visible="<%# NOT mnWO.IsNew And mnWO.WOJobs.IsCompleted = True And mnWO.StatusID = 1 And (mnWO.WOStatusID = 5)%>"
																							ToolTip="Enter PPC Remark"></asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																			<asp:PlaceHolder ID="PlaceHolder7" runat="server" Visible="false">
																				<tr>
																					<td>
																						<asp:Label ID="lblstarCAMOUpdateRemark" runat="server" CssClass="clsLabelStar">*</asp:Label>
																						&nbsp;
																					</td>
																					<td align="left">
																						<asp:Label ID="lblCAMOUpdateRemark" runat="server" Visible="<%# Not mnWO.IsNew And mnWO.WOJobs.IsCompleted = True And (mnWO.StatusID = 1) And (mnWO.WOStatusID = 5)  %>"
																							CssClass="clsLabelAuto">CAMO Update Remark</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtCAMOUpdateRemark" runat="server" CssClass="clsTextBoxTagSearch"
																							Text="<%# mnWO.CAMOUpdateRemark %>" MaxLength="500" Height="25px" TextMode="MultiLine"
																							Visible="<%# NOT mnWO.IsNew And mnWO.WOJobs.IsCompleted = True And mnWO.StatusID = 1 And (mnWO.WOStatusID = 5)%>"
																							ToolTip="Enter PPC Remark"></asp:TextBox>
																					</td>
																				</tr>
																			</asp:PlaceHolder>
																		</table>
																	</fieldset>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<asp:PlaceHolder ID="phdoc" runat="server" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax", False, True) %>'>
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlDocumentStatusDetail" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle" id="fldDocument" runat="server">
																			<legend class="clsFieldSet1">Document Status Detail
																			</legend>
																			<table>
																				<tr>
																					<td align="left">
																						<asp:Label ID="lblFormNo" runat="server" CssClass="clsLabelAuto">Form No.</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtFormNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.FormNo %>"
																							ToolTip="Enter Form No." MaxLength="150"> </asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<asp:Label ID="lblIssueNo" runat="server" CssClass="clsLabelAuto">Issue No.</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtIssueNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.IssueNo %>"
																							ToolTip="Enter Issue No." MaxLength="150"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<asp:Label ID="lblRevisionNo" runat="server" CssClass="clsLabelAuto">Revision No.</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.RevisionNo %>"
																							ToolTip="Enter Revision No." MaxLength="150"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</asp:PlaceHolder>
												</table>
											</td>
											<td valign="top" colspan="2">
												<table>
													<tr>
														<td valign="top">
															<table>
																<tr>
																	<td>
																		<%--AJAX- New function added as Focus gets Lost when we use tabs in Grid--%>
																		<asp:UpdatePanel ID="upnlMachineDet" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<fieldset class="clsFieldSetNewStyle">
																					<legend class="clsFieldSet1"><b>
																						<asp:Label ID="lblAircraftDetailsInfo" runat="server" CssClass="clsLabelHeader"></asp:Label><!-- 'ALL27072020-->
																					</b></legend>
																					<table>
																						<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#IIf(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 92 Or mnWO.TransTypeID = 93 Or mnWO.TransTypeID = 102 or not (mnWO.MachineID=Guid.Empty), True, False) %>'>
																							<%-- ALL27072020--%>
																							<tr>
																								<td colspan="3">
																									<asp:CheckBox ID="chkMaintenance" runat="server" CssClass="clsLabelAuto" Text="Maintenance"
																										Enabled="false" Visible="false" ToolTip="Check if this is for Maintenance" AutoPostBack="true"
																										Checked="<%#IIf(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 92 Or mnWO.TransTypeID = 93 Or mnWO.TransTypeID = 102, True, False) %>"></asp:CheckBox><!-- 'ALL27072020-->
																								</td>
																							</tr>
																							<tr>
																								<td colspan="1">
																									<asp:Label ID="Label13" runat="server" CssClass="clsLabelStar">*</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
																								</td>
																								<td colspan="1">
																									<asp:DropDownList ID="cmbAircraftList" runat="server" 
																										CssClass="clsTextBoxTagSearchComboSmall" Width="190px"
																										AutoPostBack="True" DataValueField="ID" DataTextField="RegNo" 
																										SelectedValue="<%# mnWO.MachineID %>" />
																									<asp:DropDownList ID="cmbAssembly" runat="server" 
																										CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
																										DataValueField="AssemblyStatusID" DataTextField="ModelSerialNo" />
																									<asp:DropDownList ID="cmbCompList" runat="server" 
																										CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
																										DataValueField="CompStatusID" DataTextField="PartSerialNo" />
																									<!-- 'ALL27072020-->
																								</td>
																							</tr>
																						</asp:PlaceHolder>
																						<asp:PlaceHolder ID="phRegDetails" runat="server" 
																							Visible='<%#IIf(mnWO.TransTypeID = 88 Or
																											(mnWO.MachineID = Guid.Empty), True, False) %>'>
																							<tr>
																								<td></td>
																								<td>
																									<asp:Label ID="lblRegNo" runat="server" CssClass="clsLabelAuto">Reg. No.</asp:Label>
																								</td>
																								<td colspan="1">
																									<asp:TextBox ID="txtRegNo" TabIndex="26" runat="server" CssClass="clsTextBoxTagSearch"
																										Text="<%# mnWO.RegNo %>" ToolTip="Enter Reg. No." />
																									<cc2:autocompleteextender clientidmode="Static" id="AutoCompleteExtender1" runat="server"
																										delimitercharacters="" enabled="True" minimumprefixlength="0" completioninterval="1000"
																										servicepath="wfnWODetail_AJAX.aspx" servicemethod="GetRegTextList" targetcontrolid="txtRegNo"
																										usecontextkey="True" contextkey="" completionlistcssclass="ac_results_Main" completionlistitemcssclass="ac_results_li"
																										completionlisthighlighteditemcssclass="ac_over_Main" onclientpopulated="ClientPopulated"
																										onclientpopulating="ClientPopulating" onclienthiding="ClientHiding" onclientshown="ClientHiding"
																										onclientshowing="ClientShowing" />
																								</td>
																							</tr>
																						</asp:PlaceHolder>
																						<tr>
																							<td class="style1">
																								<asp:Label ID="lblModelStar" runat="server" CssClass="clsLabelStar" Height="18px" Width="8px" Visible='<%#IIf(AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax", True, False) %>'>*</asp:Label>
																							</td>
																							<td class="style1">
																								<asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto" Height="16px" Width="75px">Model No.</asp:Label>
																							</td>
																							<td colspan="1" class="style1">
																								<asp:TextBox ID="txtModelNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.ModelName %>"
																									AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged" ToolTip="Enter Model No.">
																								</asp:TextBox>
																								<cc2:autocompleteextender runat="server" id="txtModelList_AutoCompleteExtender" targetcontrolid="txtModelNo"
																									servicemethod="GetModelNameList" minimumprefixlength="0" enablecaching="true"
																									completionsetcount="20" completioninterval="1000" usecontextkey="True" completionlistcssclass="ac_results_Main"
																									completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
																									onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
																									onclientshown="ClientHiding" onclientshowing="ClientShowing">
																								</cc2:autocompleteextender>
																							</td>
																						</tr>
																						<tr>
																							<td align="right">
																								<asp:Label ID="Label2" runat="server" CssClass="clsLabelStar" Height="18px" Width="8px">*</asp:Label>
																							</td>
																							<td>
																								<asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
																							</td>
																							<td colspan="1">
																								<asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.SerialNo %>"
																									ToolTip="Enter Serial No.">
																								</asp:TextBox>
																							</td>
																						</tr>
																						<tr style="display: none">
																							<td></td>
																							<td>
																								<asp:Label ID="lblHourType" runat="server" CssClass="clsLabelAuto">Hour/Hobbs Type </asp:Label>
																							</td>
																							<td colspan="1">
																								<asp:DropDownList ID="cmbHourTypeList" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboSmall1"
																									DataTextField="PeriodUnitName" DataValueField="ID" Enabled="<%# mnWO.MachineID.Equals(Guid.Empty) %>"
																									SelectedValue="<%# mnWO.HourType %>">
																								</asp:DropDownList>
																							</td>
																						</tr>
																						<asp:PlaceHolder ID="plhCustomer" runat="server">
																							<tr>
																								<td>
																									<asp:Label ID="lblCustStar" runat="server" CssClass="clsLabelStar" Height="18px" Width="8px" Visible='<%#IIf(AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax", True, False) %>'>*</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelAuto">Customer</asp:Label>
																								</td>
																								<td>
																									<asp:DropDownList ID="cmbCustomerList" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																										DataTextField="Name" DataValueField="ID" SelectedValue="<%# mnWO.CustomerID %>">
																									</asp:DropDownList>
																								</td>
																							</tr>
																							<tr>
																								<td>&nbsp;
																								</td>
																								<td>
																									<asp:Label ID="lblCustomer0" runat="server" CssClass="clsLabelAuto">Customer</asp:Label>
																									WO # / PO #
																								</td>
																								<td>
																									<asp:TextBox ID="txtCustWO" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.CustomerWONo %>"
																										ToolTip="Enter Cust. WO"></asp:TextBox>
																								</td>
																							</tr>
																							<asp:PlaceHolder ID="plhCustApproval" runat="server">
																								<tr>
																									<td>&nbsp;
																									</td>
																									<td colspan="2">
																										<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
																											<ContentTemplate>
																												<asp:Panel ID="CustApproval" runat="server">
																													<fieldset class="clsFieldSetNewStyle">
																														<legend class="clsFieldSet1"><b>Customer Approval</b></legend>
																														<table>
																															<tr>
																																<td style="height: 24px; margin-left: 40px;" align="left">
																																	<asp:RadioButton ID="rdpYes" runat="server" CssClass="clsRadioButton" Text="Obtained"
																																		ToolTip="Check if Approved" AutoPostBack="True" Checked="<%# mnWO.IsCustApprovedObtained %>"
																																		GroupName="m"></asp:RadioButton>
																																</td>
																																<td style="height: 24px">
																																	<asp:RadioButton ID="rdpNo" runat="server" CssClass="clsRadioButton" Text="Not Obtained"
																																		ToolTip="Check if not Approved" AutoPostBack="True" Checked="<%#Not mnWO.IsCustApprovedObtained %>"
																																		GroupName="m"></asp:RadioButton>
																																</td>
																															</tr>
																															<tr>
																																<td colspan="2">
																																	<table>
																																		<tr>
																																			<td>
																																				<asp:Label runat="server" ID="lblCustBy" Visible="False">By</asp:Label>
																																			</td>
																																			<td>
																																				<asp:DropDownList ID="cmbCustApprovedByEmailWO" runat="server" CssClass="clsComboBoxMedium_Ajax">
																																					<asp:ListItem>W.O</asp:ListItem>
																																					<asp:ListItem>E-mail</asp:ListItem>
																																				</asp:DropDownList>
																																			</td>
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
																							</asp:PlaceHolder>
																							<tr>
																								<td></td>
																								<td>
																									<span id="Span4" class="clsLabelAuto" runat="server" visible='<%#IIf((AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax") And mnWO.TransTypeID = 88, True, False) %>'>Maintenance Contract</span>
																								</td>
																								<td>
																									<asp:CheckBox ID="chkFMC" runat="server" CssClass="clsCheckBox" Visible='<%#IIf((AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax") And mnWO.TransTypeID = 88, True, False) %>'
																										Checked="<%# mnWO.IsFMC %>" AutoPostBack="true" />
																									<asp:Label ID="lblCustomerContractNo" runat="server" Text="<%# mnWO.CustomerContractNo  %>" CssClass="clsLabelHeader"> </asp:Label>
																								</td>
																							</tr>
																						</asp:PlaceHolder>
																						<tr>
																							<td></td>
																							<td>
																								<span id="lblIsMSP" class="clsLabelAuto" runat="server" visible='<%#IIf(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102 or not (mnWO.MachineID=Guid.Empty) , True, False) %>'>Maintenance Support Plan</span>
																							</td>
																							<td>
																								<asp:CheckBox ID="chkIsMSP" runat="server" CssClass="clsLabelAuto" Checked="<%# mnWO.IsMSP %>"
																									Enabled="<%# mnWO.StatusID = 1 %>" TextAlign="Right" AutoPostBack="true" Visible='<%# iif(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102 or not (mnWO.MachineID=Guid.Empty), True, False) %>'></asp:CheckBox>
																								<asp:Label ID="lblContractNo" runat="server" Text="<%# mnWO.ContractNo  %>" CssClass="clsLabelHeader"> </asp:Label>
																							</td>

																						</tr>

																					</table>
																				</fieldset>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td colspan="1" valign="top">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend class="clsFieldSet1">File Attachments
																			</legend>
																			<asp:UpdatePanel ID="upnlWOAttachment" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table width="100%">
																						<tr>
																							<td>
																								<asp:UpdatePanel ID="upnldgWOAttachment" runat="server" UpdateMode="Conditional">
																									<ContentTemplate>
																										<asp:GridView ID="dgWOAttachment" ToolTip="List of File Attachment(s)" runat="server"
																											CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal" CellPadding="5"
																											AllowPaging="False" AutoGenerateColumns="false">
																											<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																											<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																											<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																											<Columns>
																												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																												<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																												<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																													<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																												</asp:BoundField>
																												<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																												</asp:BoundField>
																												<asp:TemplateField HeaderText="File Name">
																													<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																													<ItemTemplate>
																														<asp:TextBox ID="txtFileName" runat="server" 
                                                                                                                            CssClass="clsTextBoxTagSearch" MaxLength="100"
																															ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" 
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
																															Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
																													</ItemTemplate>
																												</asp:TemplateField>
																												<asp:TemplateField HeaderStyle-HorizontalAlign="Center" 
                                                                                                                    HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																													<ItemTemplate>
																														<%-- <span id="button">Login</span>--%>
																														<div class="dropdown">
																															<div class="dropdownbtn-content">
																																<table id="T1" class="clsGridNew_Ajax">
																																	<tr>

																																		<td>
																																			<asp:ImageButton ID="View" runat="server" 
                                                                                                                                                CommandArgument='<%# Eval("SrNo") %>' 
                                                                                                                                                CommandName="View"
																																				CssClass="FileAttachmentICN"
                                                                                                                                                ImageUrl="icons/CLIP01.ICO" />
																																		</td>
																																		<td>
																																			<asp:ImageButton ID="Remove" runat="server" 
                                                                                                                                                CommandArgument='<%# Eval("SrNo") %>'
																																				CommandName="Remove" CssClass="largerActionICNS" 
                                                                                                                                                ImageUrl="~/images/delete.png"
																																				CausesValidation="false" />
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
																									Height="22px" Width="24px" ToolTip="Add New Attachment"
																									CausesValidation="false"></asp:ImageButton>
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</fieldset>
																	</td>
																</tr>
															</table>
														</td>
														<td valign="top" align="left">
															<table>
																<tr>
																	<td>
																		<%--AJAX- Add UpdatePanel  --%>
																		<asp:UpdatePanel ID="upnlAirframePeriods" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<fieldset class="clsFieldSetNewStyle">
																					<legend class="clsFieldSet1"><b>
																						<asp:Label ID="lblCurrentValue" runat="server" CssClass="clsLabelHeader">Airframe Current Value</asp:Label><!-- 'ALL27072020-->
																					</b></legend>
																					<table id="Table7" border="0" cellspacing="1" cellpadding="1" width="100%">
																						<tr>
																							<td valign="top" align="left">
																								<asp:GridView ID="dgCurrentPeriodValue" runat="server" CssClass="clsGridNewStyle" ToolTip="W.O. Periods"
																									PageSize="3" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
																									<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																									<RowStyle CssClass="clsdgItem"></RowStyle>
																									<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																									<Columns>
																										<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																										<asp:BoundField DataField="PeriodName" HeaderText="Periods">
																											<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																											<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
																										</asp:BoundField>
																										<asp:TemplateField HeaderText="Value" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
																											<ItemTemplate>
																												<asp:TextBox ID="txtValue" runat="server" Width="80px"
																													CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																													Text='<%# DataBinder.Eval(Container.DataItem, "CurrentValue") %>'
																													ToolTip="Enter corresponding Period Value" />
																											</ItemTemplate>
																										</asp:TemplateField>
																										<asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRecord"></asp:ButtonField>
																									</Columns>
																									<PagerStyle HorizontalAlign="Right" BorderStyle="Solid"></PagerStyle>
																									<PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
																								</asp:GridView>
																							</td>
																							<td valign="top">
																								<asp:ImageButton ID="btnSelectPeriod" runat="server"
																									ImageUrl="~/images/plus1.png"
																									Height="22px" Width="24px"
																									ToolTip="Add New Period & Value"
																									CausesValidation="true" />
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
																		<table width="100%">
																			<tr>
																				<td></td>
																			</tr>
																			<tr>
																				<td valign="top" align="left">
																					<asp:UpdatePanel ID="upnlStartDetails" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<fieldset class="clsFieldSetNewStyle">
																								<legend class="clsFieldSet1">Start Details
																								</legend>
																								<table id="Table4" width="100%">
																									<tr>
																										<td></td>
																										<td>
																											<asp:Label ID="lblStartDate" runat="server" CssClass="clsLabelAuto">Start Date</asp:Label>
																										</td>
																										<td>
																											<asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="true"
																												CssClass="clsTextBoxTagSearch" ClientIDMode="Static" Width="100px"
																												onchange="ValidateDateText(this,'txtStartDate_CalendarExtender');" />
																											<cc2:calendarextender id="txtStartDate_CalendarExtender" runat="server"
																												cssclass="cal_Theme1" clientidmode="Static" enabled="True"
																												format="<%$AppSettings:DateFormat%>" targetcontrolid="txtStartDate" />
																											<cc2:textboxwatermarkextender id="TBWEStartDate" runat="server"
																												targetcontrolid="txtStartDate" watermarktext="<%$AppSettings:DateFormat%>"
																												watermarkcssclass="clsDateTextBox" />

																											<asp:TextBox ID="txtStartDateTime" runat="server" AutoPostBack="True"
																												Visible='<%#IIf(AppSettings("ClientCode") = "IND" Or 
                                                                                                                                AppSettings("ClientCode") = "YA" Or 
                                                                                                                                AppSettings("ClientCode") = "AFC" Or 
                                                                                                                                AppSettings("ClientCode") = "ARA" Or 
                                                                                                                                AppSettings("ClientCode") = "BAP" Or
																																AppSettings("ClientCode") = "RPS" Or 
                                                                                                                                AppSettings("ClientCode") = "GLD", True, False) %>'
																												CssClass="clsTextBoxTagSearchSmall" MaxLength="10"
																												ToolTip="Enter Time" Width="65px" />

																											<cc2:maskededitextender id="txtStartDateTimeMaskedEditExtender"
																												targetcontrolid="txtStartDateTime" runat="server"
																												autocomplete="true" mask="99:99" masktype="Time"
																												culturename="en-us" messagevalidatortip="true" />
																										</td>
																									</tr>
																									<asp:PlaceHolder ID="phLogList" runat="server" Visible='<%#IIf(mnWO.TransTypeID = 89 Or 
																																								   mnWO.TransTypeID = 102, True, False) %>'>
																										<tr>
																											<td></td>
																											<td>
																												<asp:Label ID="lblLog" runat="server" CssClass="clsLabelAuto">Tech. Log No.</asp:Label>
																											</td>
																											<td>
																												<asp:DropDownList ID="cmbLogList" runat="server"
																													CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																													DataValueField="LogID" DataTextField="LogNoLogPageNo"
																													Enabled='<%# cmbAircraftList.SelectedIndex > 0 %>'>
																												</asp:DropDownList>
																											</td>
																										</tr>
																									</asp:PlaceHolder>
																									<asp:PlaceHolder ID="phLogNo" runat="server"
																										Visible='<%#IIf(mnWO.TransTypeID = 88 And 
																														AppSettings("ShowAMOOnlyForNewClients") = "False", 
																													True, False) %>'>
																										<tr>
																											<td></td>
																											<td>
																												<asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Enter Log No.</asp:Label>
																											</td>
																											<td>
																												<asp:TextBox ID="txtLogNo" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
																													Width="140px" Text="<%# mnWO.LogNo %>" ToolTip="Enter Log No." MaxLength="99"
																													Enabled='<%# cmbAircraftList.SelectedIndex %>'></asp:TextBox>
																											</td>
																										</tr>
																									</asp:PlaceHolder>
																									<tr>
																										<td colspan="3">
																											<asp:Label ID="L1" runat="server" CssClass="clsLabelauto" Height="7px"></asp:Label>
																										</td>
																									</tr>
																								</table>
																							</fieldset>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																				</td>
																			</tr>
																			<tr style="display: none">
																				<td>
																					<fieldset class="clsFieldSetNewStyle">
																						<legend class="clsFieldSet1">
																							<b>In House / Third Party</b>
																						</legend>
																						<table>
																							<tr>
																								<td style="height: 24px; margin-left: 40px;" align="left">
																									<asp:RadioButton ID="rdbIsInHouse" runat="server"
																										CssClass="clsRadioButton" Text="Is In House"
																										ToolTip="Check if this is In House" AutoPostBack="True"
																										Checked="<%# mnWO.IsInHouse %>"
																										GroupName="a" />
																								</td>
																								<td style="height: 24px">
																									<asp:RadioButton ID="rdbIsThirdParty" runat="server" CssClass="clsRadioButton"
																										Text="Is Third Party"
																										ToolTip="Check if this is Third Party" AutoPostBack="True"
																										Checked="<%# mnWO.IsThirdParty %>" GroupName="a" />
																								</td>
																							</tr>
																						</table>
																					</fieldset>
																				</td>
																			</tr>
																			<tr>
																				<td valign="top">
																					<asp:UpdatePanel ID="upnlClosing" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<fieldset class="clsFieldSetNewStyle" id="fldClosingDet" runat="server">
																								<legend class="clsFieldSet1">Closing Details
																								</legend>
																								<table width="100%">
																									<tr>
																										<td align="left">
																											<asp:Label ID="lblCloseingDateDate" runat="server" CssClass="clsLabelAuto">Close Date</asp:Label>
																										</td>
																										<td>
																											<asp:TextBox ID="txtCloseDate" runat="server" AutoPostBack="True"
																												CssClass="clsTextBoxTagSearch" Width="100px"
																												onchange="ValidateDateText(this,'txtCloseDate_CalendarExtender');" />
																											<cc2:calendarextender id="txtCloseDate_CalendarExtender"
																												runat="server" cssclass="cal_Theme1"
																												enabled="True" format="<%$AppSettings:DateFormat%>"
																												targetcontrolid="txtCloseDate" />
																											<cc2:textboxwatermarkextender id="TextBoxWatermarkExtender1"
																												runat="server" targetcontrolid="txtCloseDate"
																												watermarktext="<%$AppSettings:DateFormat%>"
																												watermarkcssclass="clsDateTextBox" />

																											<asp:TextBox ID="txtClosedDateTime" runat="server" AutoPostBack="True"
																												Visible='<%#IIf(AppSettings("ClientCode") = "IND" Or 
                                                                                                                                AppSettings("ClientCode") = "YA" Or 
                                                                                                                                AppSettings("ClientCode") = "AFC" Or 
                                                                                                                                AppSettings("ClientCode") = "ARA" Or 
                                                                                                                                AppSettings("ClientCode") = "BAP" Or
																																AppSettings("ClientCode") = "RPS" Or 
                                                                                                                                AppSettings("ClientCode") = "GLD", True, False) %>'
																												CssClass="clsTextBoxTagSearchSmall" MaxLength="10"
																												ToolTip="Enter Time" Width="65px" />

																											<cc2:maskededitextender id="txtClosedDateTimeMaskedEditExtender"
																												targetcontrolid="txtClosedDateTime" runat="server"
																												autocomplete="true" mask="99:99" masktype="Time"
																												culturename="en-us" messagevalidatortip="true" />
																										</td>
																									</tr>
																									<asp:PlaceHolder runat="server" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or 
																																					AppSettings("ShowAMOOnlyForNewClients") = "True" Or 
																																					Session("wfProject_Ajax") = "wfProject_Ajax", False, True) %>'>
																										<tr>
																											<td align="left">
																												<asp:Label ID="lblTime" runat="server" CssClass="clsLabelAuto">Time Taken</asp:Label>
																											</td>
																											<td>
																												<asp:TextBox ID="txtActualTime" runat="server" CssClass="clsTextBoxTagSearchSmall"
																													Text="<%# mnWO.WOTotalActualTime %>" ToolTip="Actual Time"
																													BackColor="#E0E0E0" ReadOnly="True" Width="65px" />
																											</td>
																										</tr>
																									</asp:PlaceHolder>
																									<asp:PlaceHolder runat="server" Visible='<%#Not AppSettings("ClientCode") = "STR" %>'>
																										<tr>
																											<td align="left">
																												<asp:Label ID="lblCloseBy" runat="server" CssClass="clsLabelAuto">Closed By</asp:Label>
																											</td>
																											<td>
																												<asp:TextBox ID="txtClosedBy" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.ClosedBy %>"
																													ToolTip="Enter Closed By"> </asp:TextBox>
																											</td>
																										</tr>
																									</asp:PlaceHolder>
																									<tr>
																										<td colspan="2">
																											<asp:UpdatePanel ID="upnlMaintComplainceDetails" runat="server" UpdateMode="Conditional"
																												Visible='<%# AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "GEP" %>'>
																												<ContentTemplate>
																													<asp:Panel ID="pnlMaintComplainceDetails" runat="server">
																														<table>
																															<tr>
																																<td>
																																	<asp:CheckBox ID="chkSupplementalSheetAttached"
																																		runat="server" CssClass="clsCheckBox" AutoPostBack="true"
																																		Checked="<%# mnWO.IsSupplementalSheetAttached %>"
																																		Text="Supplemental Sheet Attached?" />
																																</td>
																																<td>
																																	<asp:TextBox ID="txtNoOfSupplementalSheets" runat="server"
																																		CssClass="clsTextBoxRightAlignSmall1_Ajax"
																																		MaxLength="4" BackColor="LightGray" ReadOnly="true"
																																		Text="<%# mnWO.NoOfSupplementalSheetsStr %>"
																																		ToolTip="Enter No Of Supplemental Sheets" />
																																	<asp:Label ID="lblNoOfSupplementalSheets" runat="server" CssClass="clsLabelAuto">No(s)</asp:Label>
																																</td>
																															</tr>
																															<tr>
																																<td colspan="2">
																																	<table>
																																		<tr>
																																			<td>
																																				<asp:CheckBox ID="chkNRCRaised" runat="server"
																																					CssClass="clsCheckBox" Checked="<%# mnWO.IsNRCRaised %>"
																																					AutoPostBack="true" Text="NRC Raised?" />
																																			</td>
																																			<td>
																																				<asp:TextBox ID="txtNoOfNRCs" runat="server"
																																					CssClass="clsTextBoxTagSearch" MaxLength="500"
																																					Width="184px" Text="<%# mnWO.NoOfNRCsStr %>"
																																					BackColor="LightGray" ReadOnly="true"
																																					ToolTip="Enter No Of NRCs" />
																																			</td>
																																		</tr>
																																	</table>
																																</td>
																															</tr>
																															<tr>
																																<td colspan="2">
																																	<table>
																																		<tr>
																																			<td>
																																				<asp:Label ID="lblCRSNo" runat="server" CssClass="clsLabelAuto">CRS No</asp:Label>
																																			</td>
																																			<td>
																																				<asp:TextBox ID="txtCRSNo" runat="server" CssClass="clsTextBoxTagSearch"
																																					Text="<%# mnWO.CRSNo %>" MaxLength="10"
																																					Width="184px" ToolTip="Enter CRSNo" />
																																			</td>
																																		</tr>
																																		<tr>
																																			<td>
																																				<asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Certifying Staff 1</asp:Label>
																																			</td>
																																			<td>
																																				<asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter License No."
																																					AutoComplete="off" ClientIDMode="Static" MaxLength="200"></asp:TextBox>
																																				<cc2:autocompleteextender clientidmode="Static" id="txtLicenceNo_Autocomplete" runat="server"
																																					delimitercharacters="" enabled="True" completionsetcount="20" minimumprefixlength="0"
																																					completioninterval="1" servicepath="wfnWODetail_AJAX.aspx" servicemethod="GetLicenseNoList"
																																					targetcontrolid="txtLicenceNo" usecontextkey="False" contextkey="" completionlistcssclass="ac_results_Main"
																																					completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
																																					onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
																																					onclientshown="ClientHiding" onclientshowing="ClientShowing">
																																				</cc2:autocompleteextender>
																																			</td>
																																		</tr>
																																		<tr>
																																			<td>
																																				<asp:Label ID="Label14" runat="server" CssClass="clsLabelAuto">Certifying Staff 2</asp:Label>
																																			</td>
																																			<td>
																																				<asp:TextBox ID="txtLicenceNo2" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter License No."
																																					AutoComplete="off" ClientIDMode="Static" MaxLength="200"></asp:TextBox>
																																				<cc2:autocompleteextender clientidmode="Static" id="txtLicenceNo2_Autocomplete" runat="server"
																																					delimitercharacters="" enabled="True" completionsetcount="20" minimumprefixlength="0"
																																					completioninterval="1" servicepath="wfnWODetail_AJAX.aspx" servicemethod="GetLicenseNoList"
																																					targetcontrolid="txtLicenceNo2" usecontextkey="False" contextkey="" completionlistcssclass="ac_results_Main"
																																					completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
																																					onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
																																					onclientshown="ClientHiding" onclientshowing="ClientShowing">
																																				</cc2:autocompleteextender>
																																			</td>
																																		</tr>
																																	</table>
																																</td>
																															</tr>
																															<tr>
																																<td>
																																	<asp:CheckBox ID="chkIsReInspection" runat="server" CssClass="clsCheckBox" Checked="<%# mnWO.IsReInspection %>"
																																		AutoPostBack="true" Text="Re-Inspection" />
																																</td>
																																<td>
																																	<asp:CheckBox ID="chkIsIndependentInspection" runat="server" CssClass="clsCheckBox"
																																		AutoPostBack="true" Checked="<%# mnWO.IsIndependentInspection %>" Text="Independent Inspection" />
																																</td>
																															</tr>
																														</table>
																													</asp:Panel>
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
																		</table>
																	</td>
																</tr>
																<tr>
																	<td valign="top" colspan="2">
																		<asp:Panel ID="QcApproval" runat="server" Visible="False">
																			<fieldset class="clsFieldSetNewStyle">
																				<legend class="clsFieldSet1">
																					<b>QC Approval</b>
																				</legend>
																				<asp:UpdatePanel ID="UpnlApproval" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<table>
																							<tr style="display: none">
																								<td colspan="2">
																									<asp:RadioButton Text="Approved" ID="rdbApproved" GroupName="a" runat="server" AutoPostBack="true" />
																									<asp:RadioButton Text="Not Approved" ID="rdbNotApproved" GroupName="a" runat="server"
																										AutoPostBack="true" />
																									<asp:RadioButton Text="None" ID="rdbNone" GroupName="a" runat="server" AutoPostBack="true"
																										Checked="true" />
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label ID="Label7" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtQcDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																										onchange="ValidateDateText(this,'CalendarExtender1');"></asp:TextBox>
																									<cc2:calendarextender id="CalendarExtender1" runat="server" cssclass="cal_Theme1"
																										enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtQcDate">
																									</cc2:calendarextender>
																									<cc2:textboxwatermarkextender id="TextBoxWatermarkExtender2" runat="server" targetcontrolid="txtQcDate"
																										watermarktext="<%$AppSettings:DateFormat%>" watermarkcssclass="clsDateTextBox">
																									</cc2:textboxwatermarkextender>
																									<asp:TextBox ID="txtQCDateTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
																										Visible="false" MaxLength="10" ToolTip="Enter Time" Width="65px"></asp:TextBox>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtQcRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle1" TextMode="MultiLine"
																										MaxLength="200" ToolTip="Enter Remark"></asp:TextBox>
																								</td>
																							</tr>
																						</table>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</fieldset>
																		</asp:Panel>
																	</td>
																</tr>
																<tr>
																	<td valign="top" colspan="2">
																		<asp:Panel ID="pnlBilling" runat="server" Visible="<%# (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOStatusID = 5)   %>">
																			<fieldset class="clsFieldSetNewStyle">
																				<legend class="clsFieldSet1">
																					<b>Billing Details</b>
																				</legend>
																				<asp:UpdatePanel ID="upnlBilling" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<table>
																							<tr>
																								<td colspan="3">
																									<asp:UpdatePanel ID="upnlValidationSummary3" runat="server" UpdateMode="Conditional">
																										<ContentTemplate>
																											<asp:ValidationSummary ID="Validationsummary3" runat="server" CssClass="clsValidationSummary"
																												Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="c"></asp:ValidationSummary>
																											<asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Billing Date should be greater than or equal to Work Order Date"
																												ControlToValidate="txtBillingDate" ValidationGroup="c" SetFocusOnError="true"
																												Display="None"></asp:CustomValidator>
																										</ContentTemplate>
																									</asp:UpdatePanel>
																								</td>
																							</tr>
																							<tr>
																								<td colspan="3">
																									<asp:RadioButton Text="Billing Done" ID="rdbBillingDone" GroupName="a" runat="server"
																										AutoPostBack="true" />
																									<asp:RadioButton Text="Not Required" ID="rdbBillingNotRequired" GroupName="a" runat="server"
																										AutoPostBack="true" />
																									<asp:RadioButton Text="None" ID="rdbBillingNone" GroupName="a" runat="server" Checked="true"
																										AutoPostBack="true" />
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label ID="lblBillingStar" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="Label9" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtBillingDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																										onchange="ValidateDateText(this,'txtBillingDate_CalendarExtender');"></asp:TextBox>
																									<cc2:calendarextender id="CalendarExtender2" runat="server" cssclass="cal_Theme1"
																										enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtBillingDate">
																									</cc2:calendarextender>
																									<cc2:textboxwatermarkextender id="TextBoxWatermarkExtender3" runat="server" targetcontrolid="txtBillingDate"
																										watermarktext="<%$AppSettings:DateFormat%>" watermarkcssclass="clsDateTextBox">
																									</cc2:textboxwatermarkextender>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label ID="lblBillingInvoiceNumberStar" runat="server" CssClass="clsLabelStar"
																										Visible="false">*</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="Label10" runat="server" CssClass="clsLabelAuto">Invoice No.</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
																										Text="<%# mnWO.BillingInvoiceNumber %>" ToolTip="Enter Invoice Number"></asp:TextBox>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label ID="lblBillingByStar" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="Label11" runat="server" CssClass="clsLabelAuto">Billing By</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtBillingBy" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
																										Text="<%# mnWO.BillingBy %>" ToolTip="Enter Billing By"></asp:TextBox>
																								</td>
																							</tr>
																							<tr>
																								<td></td>
																								<td>
																									<asp:Label ID="Label12" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtBillingRemark" runat="server" CssClass="clsTextBoxTagSearch"
																										Text="<%# mnWO.BillingRemark %>" TextMode="MultiLine" Height="25px" MaxLength="200"
																										ToolTip="Enter Remark"></asp:TextBox>
																								</td>
																							</tr>
																						</table>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</fieldset>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<table width="100%">
										<tr>
											<td>
												<asp:PlaceHolder ID="phJobType" runat="server">
													<table>
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlJobHeader" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="Table12">
																			<tr>
																				<td>
																					<asp:Label ID="lblWOJobs" runat="server" CssClass="clsLabelHeader">W.O. Jobs</asp:Label>
																				</td>
																			</tr>
																		</table>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
															<td align="right">
																<asp:UpdatePanel ID="upnlJobType" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="Table10" width="100%">
																			<tr>
																				<td colspan="1">
																					<span id="Label31" runat="server" class="clsLabelStar">*</span>
																				</td>
																				<td>
																					<asp:Label ID="lblJobType" runat="server" CssClass="clsLabelAuto" Height="12px" Width="57px">Job Type</asp:Label>
																				</td>
																				<td>
																					<asp:DropDownList ID="cmbJobType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:ImageButton ID="btnAddJob" runat="server" ImageUrl="~/images/plus1.png"
																						Height="22px" Width="24px" ToolTip="Click to Add Job" CausesValidation="True"></asp:ImageButton>
																				</td>
																			</tr>
																		</table>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
															<td>
																<table width="100%">
																	<tr>
																		<%--'Added by Saylee on 22-Jun-2023 , for Third Party job transferring--%>
																		<asp:PlaceHolder ID="phImportJobs" runat="server" Visible='<%# (mnWO.StatusID = 1 Or mnWO.StatusID = 2) And (mnWO.WOStatusID <> 3) And ((mnWO.TransTypeID = 88) And (AppSettings("ShowAMOOnlyForNewClients") = "True") OR (Session("wfProject_Ajax") = "wfProject_Ajax" and mnWO.MachineID.Equals(Guid.Empty))) %>'>
																			<td>
																				<asp:UpdatePanel ID="upnlImportJobsLink" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<asp:LinkButton ID="lnkImportJobs" runat="server" Width="150px" CssClass="clsHyperlink1"
																							ToolTip="Import Job(s) from Excel">Import Task(s)/Job(s)</asp:LinkButton>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</td>
																		</asp:PlaceHolder>
																		<td>
																			<asp:UpdatePanel ID="upnlCloseAll" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:LinkButton ID="lnkCloseALLJobs" runat="server" Width="180px" CssClass="clsHyperlink1"
																						ToolTip="Close All Job(s)" Visible='<%# (mnWO.StatusID = 2) And
																																(mnWO.WOJobs.IsCompleted = False Or mnWO.WONRCJobs.IsCompleted = False) And
																																(AppSettings("ShowMultipleWOJobActions").ToString.ToLower = "false")%>'>
                                                                                        Close ALL Job(s) Or NRC(s)
																					</asp:LinkButton>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</asp:PlaceHolder>
											</td>
											<td align="right" colspan="2">
												<asp:UpdatePanel ID="upnlReq" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:LinkButton ID="lnkCreateMultipleRequisitionOfTaskSpares" runat="server" Width="250px"
															CssClass="clsHyperlink1" Font-Underline="true">Task card wise Requisition(s)</asp:LinkButton>
														<asp:LinkButton ID="lnkCreateRequisition" runat="server" Width="200px" CssClass="clsHyperlink1"
															Font-Underline="true" ToolTip="Create Requisition of Job Spares Items(s)">Create Spare Requisition</asp:LinkButton>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</asp:Panel>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlGrids" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table style="width: 100%">
											<asp:Panel ID="UsedForAllWO1" runat="server">
												<tr>
													<td colspan="2">
														<fieldset class="clsFieldSetNewStyle">

															<asp:GridView ID="dgWOJobs" runat="server" CssClass="clsGridNewStyle" Width="100%" ToolTip="List of W.O. Jobs"
																DESIGNTIMEDRAGDROP="10" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																<RowStyle CssClass="clsdgItem"></RowStyle>
																<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																<Columns>
																	<%--0--%>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																	<%--1--%>
																	<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																		<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																	</asp:BoundField>
																	<%--2--%>
																	<asp:BoundField DataField="TaskCardNo" HeaderText="Task No.">
																		<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--3--%>
																	<asp:BoundField DataField="WOJobDescription" HeaderText="Description" HtmlEncode="false">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="true" CssClass="clsTextOnNewLine" />
																		<%--Modified by Harsh on 14th May 2024 for displaying each label on new line --%>
																	</asp:BoundField>
																	<%--4--%>
																	<asp:BoundField DataField="MonitorInfoType" HeaderText="Monitor Type" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--5--%>
																	<asp:BoundField DataField="WOJobAction" HeaderText="Action">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<%--6--%>
																	<asp:BoundField DataField="DueAsOfGrid" HeaderText="Due As Of" HtmlEncode="false">
																		<ItemStyle Wrap="False" CssClass="clsTextOnNewLine" Width="80px"></ItemStyle>
																		<HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>
																		<%--Modified by Harsh on 14th May 2024 for displaying each label on new line --%>
																	</asp:BoundField>
																	<%--7--%>
																	<asp:BoundField DataField="WOJobEstimatedTime" HeaderText="Est. Man Hr">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--8--%>
																	<asp:BoundField DataField="WOJobStartDateFormatted" HeaderText="Start Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--9--%>
																	<asp:BoundField DataField="WOJobCloseDateFormatted" HeaderText="Close Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<%--10--%>
																	<asp:BoundField DataField="WOJobActualTime" HeaderText="Actual Man Hr.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<%--11--%>
																	<asp:BoundField DataField="WOJobTypeName" HeaderText="Job Type">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<%--12--%>
																	<asp:BoundField DataField="WOJobStatusName" HeaderText="Status">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="false" />
																	</asp:BoundField>
																	<%--13--%>
																	<asp:TemplateField HeaderText="Task Cards">
																		<ItemTemplate>
																			<asp:LinkButton ID="lnkbtnTaskCard" runat="server" CommandName="TaskCardsRec" CausesValidation="True"></asp:LinkButton>
																		</ItemTemplate>
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																	<%--14--%>
																	<asp:TemplateField HeaderText="Designation Allocation">
																		<ItemTemplate>
																			<asp:LinkButton ID="lnkbtnDesignationAllocation" runat="server" CommandName="DesignationAllocationRec"
																				CausesValidation="True"></asp:LinkButton>
																		</ItemTemplate>
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																	<%--15--%>
																	<asp:TemplateField HeaderText="Required Spares">
																		<ItemTemplate>
																			<asp:LinkButton ID="lnkbtnSparesAddRemove" runat="server" CommandName="SparesAddRemove"
																				CausesValidation="True"></asp:LinkButton>
																		</ItemTemplate>
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																	<%--16--%>
																	<asp:TemplateField HeaderText="Inst./Rem.">
																		<ItemTemplate>
																			<asp:LinkButton ID="lnkbtnInstallationRemovalRec" runat="server" CommandName="InstallationRemovalRec"
																				CausesValidation="True"></asp:LinkButton>
																		</ItemTemplate>
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																	<%--17--%>
																	<asp:TemplateField HeaderText="NRC">
																		<ItemTemplate>
																			<asp:LinkButton ID="lnkbtnAddNRC" runat="server" CommandName="NRCRec" CausesValidation="True"></asp:LinkButton>
																		</ItemTemplate>
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>
																	<%--18--%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																		<ItemTemplate>
																			<%-- <span id="button">Login</span>--%>
																			<div class="dropdown">
																				<div id="divd" class="dropdownbtn-content" runat="server">
																					<table id="T1" class="clsGridNew_Ajax">
																						<tr>
																							<td>
																								<asp:ImageButton ID="EditView" runat="server"
																									CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									CommandName="EditRec" Style="height: 15px; width: 15px"
																									ImageUrl="~/images/edit.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="DeleteRecord" runat="server"
																									CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									CausesValidation="false" CommandName="DeleteRec"
																									Style="height: 20px; width: 20px"
																									ImageUrl="~/images/delete.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="View" runat="server"
																									CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																									CommandName="ViewRec" Style="height: 20px; width: 13px"
																									ImageUrl="icons/CLIP01.ICO"
																									Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
																	<%--21--%<%-->19--%>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" HeaderText="IsAttachmentAdded">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--22--%><%--20--%>
																	<asp:BoundField DataField="WOJobTaskCardCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobTaskCardCountForLinK">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--23--%><%--21--%>
																	<asp:BoundField DataField="WOJobDesignationAllocationCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobTaskCardCountForLinK">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--24--%><%--22--%>
																	<asp:BoundField DataField="WOJobInstallationRemovalCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobInstallationRemovalCountForLinK">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--25--%><%--23--%>
																	<asp:BoundField DataField="WOJobSparesCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobSparesCountForLinK">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--26--%><%--24--%>
																	<asp:BoundField DataField="WOJobNRCCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																		ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobNRCCountForLinK">
																		<HeaderStyle CssClass="hideGridColumn" />
																		<ItemStyle CssClass="hideGridColumn" />
																	</asp:BoundField>
																	<%--27--%><%--25--%>
																	<asp:ButtonField ItemStyle-Wrap="false" Text="Print With Task(s)" HeaderText="Print With Task(s)"
																		CommandName="PrintWithTaskCardsRec" CausesValidation="True"></asp:ButtonField>
																</Columns>
																<PagerStyle HorizontalAlign="Right" BorderStyle="Solid"></PagerStyle>
																<PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
															</asp:GridView>
														</fieldset>
													</td>
												</tr>
												<asp:PlaceHolder ID="PlaceHolder9" runat="server">
													<tr>
														<td align="left">
															<table id="Table6" border="0" cellspacing="1" cellpadding="1">
																<tr>
																	<td>
																		<br />
																	</td>
																	<td>
																		<asp:Label ID="lblRequiredToolList" runat="server" CssClass="clsLabelHeader">W.O. Tools</asp:Label>
																	</td>
																	<td align="right">
																		<asp:UpdatePanel ID="upnlAddTool" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<asp:ImageButton ID="btnAddTool" runat="server" ImageUrl="~/images/plus1.png"
																					Height="22px" Width="24px" ToolTip="Click to Add Tool" CausesValidation="True"></asp:ImageButton>

																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																	<td align="right">
																		<asp:UpdatePanel ID="upnlMailTool" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<asp:Button ID="btnSendMailTool" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
																					Visible="<%# mnWO.WOTools.Count > 0 %>" ToolTip="Click to send Mail to Store for requesting Tools."></asp:Button>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
															</table>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:LinkButton ID="lnkCreateToolsRequisition" runat="server" Width="200px" CssClass="clsHyperlink1"
																		Font-Underline="true" ToolTip="Click to create Requisition of Work Order Tool(s)">Create Tools Requisition</asp:LinkButton>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</asp:PlaceHolder>
												<!--Dummy panel to open modelpopup for FileUpload-->
												<tr style="height: 0px;">
													<td colspan="2" style="height: 0px;">
														<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
															<ContentTemplate>
																<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddJobCompDetail" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddDesignaionAllocation" ClientIDMode="Static" runat="server"
																	Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddJobSpareDetail" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddSelectNRC" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddWOTool" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnAddJobTaskDetail" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnMSPAssemblySelection" ClientIDMode="Static" runat="server"
																	Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnCustomerContractSelection" ClientIDMode="Static" runat="server"
																	Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
																<asp:Button ID="hdnBtnDigitalSignatureRequest" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<asp:PlaceHolder ID="PlaceHolder10" runat="server">
													<tr>
														<td colspan="2" valign="top">
															<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlToolGrid">
																<ContentTemplate>
																	<fieldset class="clsFieldSetNewStyle">

																		<asp:GridView ID="dgWOTools" runat="server" AutoGenerateColumns="False" Width="100%"
																			CssClass="clsGridNewStyle" ToolTip="List of W.O. Tools" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
																			<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																			<RowStyle CssClass="clsdgItem"></RowStyle>
																			<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																			<Columns>
																				<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
																				<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																					<HeaderStyle HorizontalAlign="Left" Wrap="False" Width="5px"></HeaderStyle>
																					<ItemStyle Wrap="true"></ItemStyle>
																				</asp:BoundField>
																				<asp:BoundField DataField="PartNo" HeaderText="Part No.">
																					<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																				</asp:BoundField>
																				<asp:BoundField DataField="Description" HeaderText="Description">
																					<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																				</asp:BoundField>
																				<asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
																					<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Right"></ItemStyle>
																				</asp:BoundField>
																				<asp:BoundField DataField="ToolsPendingToReturnQty" HeaderText="Pending to return Qty.">
																					<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																					<ItemStyle HorizontalAlign="Right"></ItemStyle>
																				</asp:BoundField>
																				<asp:BoundField DataField="WOToolRemark" HeaderText="Remark">
																					<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																				</asp:BoundField>
																				<%--7--%>
																				<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																					<ItemTemplate>
																						<div class="dropdown">
																							<div class="dropdownbtn-content">
																								<table id="T1" class="clsGridNew_Ajax">
																									<tr>
																										<td>
																											<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																												CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																										</td>
																										<td>
																											<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																												CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
																			<PagerStyle HorizontalAlign="Right"></PagerStyle>
																			<PagerSettings NextPageText="Next" PreviousPageText="Prev" Position="TopAndBottom"></PagerSettings>
																		</asp:GridView>

																	</fieldset>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<asp:UpdatePanel ID="upnlWONRC" runat="server" UpdateMode="Conditional" Visible='<%#IIf(mnWO.TransTypeID = 102, False, True) %>'>
																<ContentTemplate>
																	<table width="100%">
																		<tr>
																			<td>
																				<table>
																					<tr>
																						<td>
																							<br />
																						</td>
																						<td>
																							<asp:Label ID="lblAddWONRC" runat="server" CssClass="clsLabelHeader">
																								W.O. NRC's
																							</asp:Label>
																						</td>
																						<td>
																							<asp:ImageButton ID="btnAddNRC" runat="server" ImageUrl="~/images/plus1.png"
																								Height="22px" Width="24px" ToolTip="Click to Add WO NRC" CausesValidation="true" Visible="false"></asp:ImageButton>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<fieldset class="clsFieldSetNewStyle">

																					<asp:GridView ID="dgWONRC" runat="server" CssClass="clsGridNewStyle" Width="100%" AutoGenerateColumns="False"
																						ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
																						<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																						<RowStyle CssClass="clsdgItem"></RowStyle>
																						<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																						<Columns>
																							<%--0--%>
																							<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																							<%--1--%>
																							<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																								<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																							</asp:BoundField>
																							<%--2--%>
																							<asp:BoundField DataField="WOJobDescription" HeaderText="Description">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																							</asp:BoundField>
																							<%--3--%>
																							<asp:BoundField DataField="WOJobAction" HeaderText="Action">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																							</asp:BoundField>
																							<%--4--%>
																							<asp:BoundField DataField="DueAsOfGrid" HeaderText="Due As Of" HtmlEncode="false">
																								<ItemStyle Wrap="False"></ItemStyle>
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																							</asp:BoundField>
																							<%--5--%>
																							<asp:BoundField DataField="WOJobEstimatedTime" HeaderText="Est. Man Hr">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--6--%>
																							<asp:BoundField DataField="WOJobStartDateFormatted" HeaderText="Start Date">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--7--%>
																							<asp:BoundField DataField="WOJobCloseDateFormatted" HeaderText="Close Date">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--8--%>
																							<asp:BoundField DataField="WOJobActualTime" HeaderText="Actual Man Hr.">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																							</asp:BoundField>
																							<%--9--%>
																							<asp:BoundField DataField="WOJobStatusName" HeaderText="Status">
																								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																							</asp:BoundField>
																							<%--============ Ajay 19-sep-2022 Start ===============--%>
																							<%--10--%>
																							<asp:TemplateField HeaderText="Task Cards">
																								<ItemTemplate>
																									<asp:LinkButton ID="CountTaskCards" runat="server" CommandName="TaskCards" CausesValidation="True"></asp:LinkButton>
																								</ItemTemplate>
																								<ItemStyle HorizontalAlign="Center" />
																							</asp:TemplateField>
																							<%--11--%>
																							<asp:TemplateField HeaderText="Designation Allocation">
																								<ItemTemplate>
																									<asp:LinkButton ID="CountDesignationAllocation" runat="server" CommandName="DesignationAllocation"
																										CausesValidation="True"></asp:LinkButton>
																								</ItemTemplate>
																								<ItemStyle HorizontalAlign="Center" />
																							</asp:TemplateField>
																							<%--12--%>
																							<asp:TemplateField HeaderText="Required Spares">
																								<ItemTemplate>
																									<asp:LinkButton ID="CountRequiredSpares" runat="server" CommandName="RequiredSpares"
																										CausesValidation="True"></asp:LinkButton>
																								</ItemTemplate>
																								<ItemStyle HorizontalAlign="Center" />
																							</asp:TemplateField>
																							<%--13--%>
																							<asp:TemplateField HeaderText="Inst./Rem.">
																								<ItemTemplate>
																									<asp:LinkButton ID="CountInstRem" runat="server" CommandName="InstRem" CausesValidation="True"></asp:LinkButton>
																								</ItemTemplate>
																								<ItemStyle HorizontalAlign="Center" />
																							</asp:TemplateField>
																							<%--14--%>
																							<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																								<ItemTemplate>
																									<%-- <span id="button">Login</span>--%>
																									<div class="dropdown">
																										<div class="dropdownbtn-content">
																											<table id="T1" class="clsGridNew_Ajax">
																												<tr>
																													<td>
																														<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																															CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																													</td>
																													<td>
																														<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																															CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																													</td>
																													<td>
																														<asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																															CommandName="View" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
																															Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
																							<%--17--%><%--15--%>
																							<asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																								ItemStyle-CssClass="hideGridColumn" HeaderText="IsAttachmentAdded"></asp:BoundField>
																							<%--18--%><%--16--%>
																							<asp:BoundField DataField="WOJobTaskCardCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																								ItemStyle-CssClass="hideGridColumn" HeaderText="WONRCTaskCardCountForLinK">
																								<HeaderStyle CssClass="hideGridColumn" />
																								<ItemStyle CssClass="hideGridColumn" />
																							</asp:BoundField>
																							<%--19--%><%--17--%>
																							<asp:BoundField DataField="WOJobDesignationAllocationCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																								ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobTaskCardCountForLinK">
																								<HeaderStyle CssClass="hideGridColumn" />
																								<ItemStyle CssClass="hideGridColumn" />
																							</asp:BoundField>
																							<%--20--%><%--18--%>
																							<asp:BoundField DataField="WOJobInstallationRemovalCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																								ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobInstallationRemovalCountForLinK">
																								<HeaderStyle CssClass="hideGridColumn" />
																								<ItemStyle CssClass="hideGridColumn" />
																							</asp:BoundField>
																							<%--21--%><%--19--%>
																							<asp:BoundField DataField="WOJobSparesCountForLinK" HeaderStyle-CssClass="hideGridColumn"
																								ItemStyle-CssClass="hideGridColumn" HeaderText="WOJobSparesCountForLinK">
																								<HeaderStyle CssClass="hideGridColumn" />
																								<ItemStyle CssClass="hideGridColumn" />
																							</asp:BoundField>
																							<%--23--%><%--20--%>
																							<%--============ Ajay 19-sep-2022 End ===============--%>
																						</Columns>
																						<PagerStyle HorizontalAlign="Right" BorderStyle="Solid"></PagerStyle>
																						<PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
																					</asp:GridView>

																				</fieldset>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
												</asp:PlaceHolder>
												<placeholder id="phActionTaken" runat="server" visible='<%#IIf(AppSettings("ClientCode") = "HSC", True, False) And Not (mnWO.IsNew) %>'>
													<tr>
														<td colspan="2">
															<table width="100%">
																<tr>
																	<td>
																		<span id="Span1" class="clsLabelHeader">WORK REQUIRED/DEFECT</span>
																	</td>
																	<td>
																		<span id="lblAction" class="clsLabelHeader">ACTION TAKEN</span>
																	</td>

																</tr>
																<tr>
																	<td>
																		<asp:TextBox ID="txtWOWorkDone" runat="server" CssClass="clsTextBoxMultilineDefectAction" Text="<%# mnWO.WOWorkDone %>"
																			Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Enter Remark" TextMode="MultiLine"
																			Width="99%"></asp:TextBox>
																	</td>
																	<td>
																		<asp:TextBox ID="txtWOAction" runat="server" CssClass="clsTextBoxMultilineDefectAction" Text="<%# mnWO.WOAction %>"
																			Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Enter Action"
																			TextMode="MultiLine" Width="99%"></asp:TextBox>
																	</td>

																</tr>
															</table>
														</td>
													</tr>
												</placeholder>
											</asp:Panel>
											<tr>
												<td></td>
												<td align="right">
													<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpnlPrint">
														<ContentTemplate>
															<table id="Table3">
																<tr>
																	<asp:Panel ID="UsedForAllWO2" runat="server">
																		<td>
																			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel5">
																				<ContentTemplate>
																					<div class="dropdown">
																						<div class="dropdownbtn-content">
																							<table id="Table5" class="clsButton_Ajax" border="0">
																								<placeholder id="phPrintAll" runat="server" visible="false">
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintAll" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled="<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) %>" Visible="false" Text="Print All"
																												ToolTip="Click to Print All"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phPrintWithPDF" runat="server" visible='<%#IIf(AppSettings("ClientCode") = "IND" Or AppSettings("ShowMaintenanceForNewClients") = "True", IIf(AppSettings("ShowMaintenanceForNewClientsWithTaskCard") = "True", True, False), True) %>'>
																									<td>
																										<asp:Button ID="btnPrintWithPDF" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											runat="server" Text="Print with Task Attachments" ToolTip="Click to Print Additional W.O. Package with Attachments"
																											Width="200px" Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4))  %>' Visible="False"></asp:Button>
																									</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phPrintCallOutHeligo" runat="server" visible='<%#IIf(AppSettings("ClientCode") = "Heligo", True, False) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintCallOut" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												runat="server" Text="CallOut Print" ToolTip="Click to Print CallOut"
																												Width="164px" Enabled='<%#Not ((mnWO.IsNew))  %>'></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											Enabled="<%#Not (mnWO.IsNew) %>" Text="Print" ToolTip="Click to Print"></asp:Button>
																									</td>
																								</tr>
																								<placeholder id="Placeholder11" runat="server"
																									visible='<%#IIf(AppSettings("ClientCode") = "FIT", True, False) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintSWC" runat="server"
																												CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled="<%#Not (mnWO.IsNew) %>" Text="Print SWC"
																												ToolTip="Click to Print"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintWithJobAttachment" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											Enabled="<%#Not (mnWO.IsNew) %>" Text="Print with Job Attachment" ToolTip="Click to Print"></asp:Button>
																									</td>
																								</tr>
																								<placeholder id="phPrintOther" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnCRS" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												ToolTip="Click to print CRS" Text="CRS" Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) And ((mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 4 Or mnWO.WOStatusID = 7 Or mnWO.WOStatusID = 9) Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>'></asp:Button>
																										</td>
																									</tr>
																									<tr>
																										<td>
																											<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) And ((mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 4 Or mnWO.WOStatusID = 7 Or mnWO.WOStatusID = 9) Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>' Text="Send Mail" ToolTip="Click to Send Mail to Assigned Resources"></asp:Button>
																										</td>
																									</tr>
																									<tr>
																										<td>
																											<asp:Button ID="btnLogBookEntry" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) And ((mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 4 Or mnWO.WOStatusID = 7 Or mnWO.WOStatusID = 9) Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>'
																												Text="Log Book Entry Print" ToolTip="Click to Print Log Book Entry Report"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phPrintManHRS" runat="server" visible='<%# ((mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 7) Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintManHrs" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled="<%# (mnWO.StatusID > 1) %>" Text="Man Hrs Utilization" ToolTip="Click to Print Man Hrs"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="Placeholder8" runat="server" visible='<%# ((mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 7) Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) And AppSettings("ShowCAMOOnlyForNewClients") = "True" And (AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax") %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnSparePartConsumption" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												runat="server" Text="Spare Part Consumption" Width="164px" Enabled="<%# (mnWO.StatusID > 1) %>"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>

																								<placeholder id="phPrintNRC" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintNRC" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth" runat="server"
																												Text="Print NRC" Width="200px" Enabled="<%#Not ((mnWO.IsNew)) %>"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phPrintBlankEO" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
																									<tr>
																										<td>
																											<div id="div4" runat="server">
																												<asp:Button ID="btnPrintBlankEO" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																													Enabled="<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) %>" Text="Print Blank CR"
																													ToolTip="Click to Print Blank Component Replacement Detail Sheet" Visible="False"></asp:Button>
																											</div>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phPrintNC" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
																									<tr>
																										<td>
																											<div id="div3" runat="server">
																												<asp:Button ID="btnPrintNC" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																													Enabled="<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) %>" Text="Print NC" ToolTip="Click to Print Additional Work sheet/Non Confirmity"
																													Visible="False"></asp:Button>
																											</div>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phTaskCardExcel" runat="server" visible='<%#IIf(AppSettings("ClientCode") = "IND" Or AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnTaskCardExcel" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) And (mnWO.WOStatusID = 4 Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>' Text="Export Task Card to Excel"
																												ToolTip="Click to Export"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phJobsExcel" runat="server" visible='<%#IIf((mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102) And AppSettings("ShowCAMOOnlyForNewClients") = "True", True, False) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnJobsExcel" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled='<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) And (mnWO.WOStatusID = 4 Or (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID)) %>' Text="Export Jobs to Excel"
																												ToolTip="Click to Export"></asp:Button>
																										</td>
																									</tr>
																								</placeholder>
																								<placeholder id="phTaskCardTallySheet" runat="server" visible='<%#IIf(AppSettings("ClientCode") = "STR", True, False) %>'>
																									<tr>
																										<td>
																											<asp:Button ID="btnPrintTallySheet" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																												Enabled="<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) %>" Text="Tally Sheet"
																												ToolTip="Click to Print Tally Sheet"></asp:Button>
																											<asp:Button ID="btnlRequestForDigitalSignature" runat="server" Text="Request for Digital Signature" CssClass="clsbtnH clsinfoH1"
																												ClientIDMode="Static" Visible="false"></asp:Button>
																											<asp:Button ID="btnViewDSFile" runat="server" Text="View DS File" CssClass="clsbtnH clsinfoH1"
																												ClientIDMode="Static" Visible="false" />
																										</td>
																									</tr>
																								</placeholder>

																							</table>
																						</div>
																						<asp:Button ID="btnPrintCommon" CssClass="clsbtnH clsinfoH1" ClientIDMode="Static" runat="server"
																							Text="Print &#9650;"></asp:Button>
																					</div>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel4">
																				<ContentTemplate>
																					<div class="dropdown">
																						<div class="dropdownbtn-content">
																							<table id="T1" class="clsbtnH clsinfoH1" border="0">
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintBA" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to print Detail" Text="Print Detail"></asp:Button>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintWithPDFBA" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print with Task Attachments" Text="Print with Task Attachments"
																											CausesValidation="False"></asp:Button>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintWOPackage" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print W.O. Package" Text="Print W.O. Package" CausesValidation="False"></asp:Button>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintAdditionalWOAndSheet" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print Additional W.O. with Sheet" Text="Print Additional W.O. with Sheet"
																											CausesValidation="False"></asp:Button>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintAdditionalWO" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print Only Additional W.O." Text="Print Only Additional W.O."
																											CausesValidation="False"></asp:Button>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="btnPrintToolsSpares" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print Tools & Spares" Text="Print Tools & Spares" CausesValidation="False"></asp:Button>
																									</td>

																								</tr>
																								<tr>
																									<td>
																										<asp:Button ID="BtnPrintProductionPlanningForm" runat="server" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth"
																											ToolTip="Click to Print Production Planning Form" Text="Print Production Planning Form" CausesValidation="False" Visible="false"></asp:Button>
																									</td>
																								</tr>
																							</table>
																						</div>
																						<asp:Button ID="mnuPrintReportBA" CssClass="clsbtnH clsinfoH1" ClientIDMode="Static"
																							runat="server" Enabled="<%#Not ((mnWO.IsNew) Or (mnWO.StatusID = 4)) %>" Text="Print &#9650;"></asp:Button>
																					</div>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<div id="divToHide" runat="server">
																				<asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
																					ToolTip="Click to Cancel " Visible="<%# Not mnWO.IsNew And mnWO.WOStatusID = 3 And mnWO.IsSync = 0 %>"></asp:Button>
																			</div>
																		</td>
																		<td>
																			<div id="div1" runat="server">
																				<asp:Button ID="btnAuthorize" runat="server" CssClass="clsbtnH clsinfoH1" Text="Submit"
																					ToolTip="Click to Submit" Visible="<%# Not mnWO.IsNew And mnWO.WOStatusID = 3 And mnWO.StatusID = 1 %>"></asp:Button>
																			</div>
																		</td>
																		<td>
																			<asp:Button ID="btnReject" runat="server" CssClass="clsbtnH clsinfoH1" Text="Reject"
																				ToolTip="Click to Reject" Visible='<%#IIf(AppSettings("ShowNewWOFlow") = "True", True, False) And (Not mnWO.IsNew) And (mnWO.StatusID > 1) And Not (Session("MiddleFrame") = "wfnWOExecutionList.aspx")  %>'></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnAMECompletion" runat="server" CssClass="clsbtnH clsinfoH1" Text="AME Complete"
																				ToolTip="Click to Complete" Visible="false"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnComplete" runat="server" CssClass="clsbtnH clsinfoH1" Text="Complete"
																				ToolTip="Click to Complete" Visible="<%# Not mnWO.IsNew And mnWO.WOJobs.IsCompleted = True And mnWO.StatusID = 1 %>"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to Save"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnPlan" runat="server" CssClass="clsbtnH clsinfoH1" Text="Plan" ToolTip="Click to Plan"
																				Visible="<%# (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (Not mnWO.WOStatusID = 4)  %>"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnQCApproval" runat="server" CssClass="clsbtnH clsinfoH1" Text="QC Update"
																				CausesValidation="true" ToolTip="Click to QC Update" Visible="<%# (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.IsQCStatusApproved = 0)  %>"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnBilling" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save Billing"
																				ValidationGroup="c" OnClientClick="return Validate()" CausesValidation="true"
																				ToolTip="Click to save Billing Details"></asp:Button>
																			<script type="text/javascript">
                                                                                function Validate() {
                                                                                    var isValid = false;
                                                                                    isValid = Page_ClientValidate('m');
                                                                                    if (isValid) {
                                                                                        isValid = Page_ClientValidate('c');
                                                                                    }

                                                                                    return isValid;
                                                                                }
																			</script>
																		</td>
																		<td>
																			<asp:Button ID="btnComplyJobs" runat="server" CssClass="clsbtnH clsinfoH1" Text="Comply"
																				ToolTip="Click to Comply"></asp:Button>
																		</td>
																		<td>
																			<asp:Button ID="btnSaveAttachment" runat="server" Text="Save Attachment" CausesValidation="False"
																				CssClass="clsbtnH clsinfoH1" Visible="<%# (Not mnWO.IsNew)   %>" ToolTip="Click to Save Work Order Attachments"></asp:Button>
																		</td>


																		<td>
																			<asp:Button ID="btnPrintCAMO" CssClass="clsbtnH clsinfoH1 clsHoverDropDownWidth" runat="server"
																				Visible="false" Text="Print CAMO WO" Width="100px" Enabled="<%#Not ((mnWO.IsNew)) %>"></asp:Button>
																		</td>
																	</asp:Panel>
																	<td>
																		<asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
																			Text="Close" ToolTip="Click to Close"></asp:Button>
																	</td>
																	<td>
																		<div id="div2" runat="server">
																			<asp:Button ID="btnSentToBill" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send To Bill"
																				ToolTip="Click to send the Work Order for billing" Visible="False"></asp:Button>
																		</div>
																	</td>
																</tr>
																<!--Dummy panel to open modelpopup for category/nomenclature-->
																<tr style="height: 0px;">
																	<td colspan="3" style="height: 0px;">
																		<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel3">
																			<ContentTemplate>
																				<asp:Button ID="hdnimgbtnReportAll" ClientIDMode="Static" runat="server" Text="..."
																					CausesValidation="False" Style="display: none;"></asp:Button>
																				<asp:Button ID="hdnBtnLogFuelOil" ClientIDMode="Static" runat="server" Text="Add"
																					CausesValidation="False" Style="display: none;"></asp:Button>
																				<asp:Button ID="hdnBtnIssuedSpares" ClientIDMode="Static" runat="server" Text="Add"
																					CausesValidation="False" Style="display: none;"></asp:Button>
																				<asp:Button ID="hdnBtnIssuedTools" ClientIDMode="Static" runat="server" Text="Add"
																					CausesValidation="False" Style="display: none;"></asp:Button>
																				<asp:Button ID="hdnBtnJobList" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
																					Style="display: none;"></asp:Button>
																			</ContentTemplate>
																		</asp:UpdatePanel>
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
							</td>
						</tr>
					</table>
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
		<div>
			<!-- Report ALL Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyReportAll" Text="Dummy ReportAll" ClientIDMode="Static"></asp:Button>
			</div>
			<asp:Panel runat="server" ID="pnlReportAll" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeReportAll" frameborder="0" height="100%" allowtransparency="true"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:modalpopupextender id="mdlPopupReportAll" runat="server" targetcontrolid="btnDummyReportAll"
				popupcontrolid="pnlReportAll" backgroundcssclass="clsModalPopupBG">
			</cc2:modalpopupextender>
			<script type="text/javascript">
                function IFrameStateComplete() {
                    $("#btnDummyReportAll").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenReportAllWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeReportAll").attr("src", "wfnWOReportForAll_AJAX.aspx?Type=pup");
                        // $("#IframeReportAll").load(function () {
                        //                    var doc = IframeReportAll.window;
                        //                    IframeReportAll.SetPageLayout();

                        if (!$.browser.msie) {
                            $("#btnDummyReportAll").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }


                        //});


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForReportAll() {
                    var ReportAllwindow = $find("<%=mdlPopupReportAll.ClientID %>");
                    //close ReportAll popup window
                    ReportAllwindow.hide();
                    //           release resources
                    $("#IframeReportAll").attr("src", "JavaScript:''");
                    //call ReportAll image button
                    $("#hdnimgbtnReportAll").click();
                }
			</script>
			<!-- End-->
		</div>
		<script type="text/javascript">
            function BetweenDatesValidation(source, args) {
                if (source.controltovalidate == "txtBillingDate") {
                    var fromdate = $("#txtDate").val();
                    var todate = $("#txtBillingDate").val();
                }
                else {
                    return;
                }


                args.IsValid = false;

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
                    // $find(extenderid).set_Text(result);
                    __doPostBack($(elem).id, "TextChanged");
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
		<!-- Log Fuel Oil Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyLogFuelOil" Text="Log Fuel Oil" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlLogFuelOil" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeLogFuelOil" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupLogFuelOil" runat="server" targetcontrolid="btnDummyLogFuelOil"
			popupcontrolid="pnlLogFuelOil" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameLogFuelOilStateComplete() {
                $("#btnDummyLogFuelOil").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenLogFuelOilWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeLogFuelOil").attr("src", "wfLogFuelOil_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyLogFuelOil").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForLogFuelOil() {
                var LogFuelOilwindow = $find("<%=mdlPopupLogFuelOil.ClientID %>");
                //close Log Fuel Oil popup window
                LogFuelOilwindow.hide();
                //           release resources
                $("#IframeLogFuelOil").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnLogFuelOil").click();
            }
		</script>
		<!-- End-->
		<!-- Issued Spares Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyIssuedSpares" Text="Issued Spares" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlIssuedSpares" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeIssuedSpares" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupIssuedSpares" runat="server" targetcontrolid="btnDummyIssuedSpares"
			popupcontrolid="pnlIssuedSpares" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameIssuedSparesStateComplete() {
                $("#btnDummyIssuedSpares").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenIssuedWOSpares() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeIssuedSpares").attr("src", "wfnIssuedWOSpares_AJAX.aspx?Type=pup");

                    //                if (!$.browser.msie) {
                    $("#btnDummyIssuedSpares").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    //                }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForIssuedSpares() {
                var IssuedSpareswindow = $find("<%=mdlPopupIssuedSpares.ClientID %>");
                //close Issued Spares popup window
                IssuedSpareswindow.hide();
                //           release resources
                $("#IframeIssuedSpares").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnIssuedSpares").click();
            }
		</script>
		<!-- End-->
		<!-- Issued Tools Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyIssuedTools" Text="Issued Tools" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlIssuedTools" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeIssuedTools" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupIssuedTools" runat="server" targetcontrolid="btnDummyIssuedTools"
			popupcontrolid="pnlIssuedTools" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameIssuedToolsStateComplete() {
                $("#btnDummyIssuedTools").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenIssuedWOTools() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeIssuedTools").attr("src", "wfnIssuedWOTools_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyIssuedTools").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForIssuedTools() {
                var IssuedToolswindow = $find("<%=mdlPopupIssuedTools.ClientID %>");
                //close Issued Tools popup window
                IssuedToolswindow.hide();
                //           release resources
                $("#IframeIssuedTools").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnIssuedTools").click();
            }
		</script>
		<!-- End-->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div>
			<!-- JobList Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyJobList" Text="Dummy JobList" ClientIDMode="Static"></asp:Button>
			</div>
			<asp:Panel runat="server" ID="pnlJobList" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeJobList" frameborder="0" height="100%" allowtransparency="true"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:modalpopupextender id="mdlPopupJobList" runat="server" targetcontrolid="btnDummyJobList"
				popupcontrolid="pnlJobList" backgroundcssclass="clsModalPopupBG">
			</cc2:modalpopupextender>
			<script type="text/javascript">
                function IFrameJobListStateComplete() {
                    $("#btnDummyJobList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenJobTaskListWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeJobList").attr("src", "wfnWOJobTaskList.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyJobList").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForJobList() {
                    var JobListwindow = $find("<%=mdlPopupJobList.ClientID %>");
                    //close JobList popup window
                    JobListwindow.hide();
                    //           release resources
                    $("#IframeJobList").attr("src", "JavaScript:''");
                    //call JobList image button
                    $("#hdnBtnJobList").click();

                }

			</script>
			<!-- End-->
		</div>
		<!-- JobTaskDetail Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyJobTaskDetail" Text="Dummy JobTaskDetail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupJobTaskDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupJobTaskDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupJobTaskDetail" runat="server" targetcontrolid="btnDummyJobTaskDetail"
			popupcontrolid="pnlPopupJobTaskDetail" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameJobTaskDetailStateComplete() {
                $("#btnDummyJobTaskDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddJobTaskDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobTaskDetail").attr("src", "wfnWOJobTask_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyJobTaskDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForJobTaskDetail() {
                var JobTaskDetailWindow = $find("<%=mdlPopupJobTaskDetail.ClientID %>");
                //close JobTaskDetail popup window
                JobTaskDetailWindow.hide();
                $("#iPopupJobTaskDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobTaskDetail").click();
            }
		</script>
		<!-- End-->
		<!-- SelectTasks Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySelectTasks" Text="Dummy SelectTasks" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupSelectTasks" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupSelectTasks" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupSelectTasks" runat="server" targetcontrolid="btnDummySelectTasks"
			popupcontrolid="pnlPopupSelectTasks" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameSelectTasksStateComplete() {
                $("#btnDummySelectTasks").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddSelectTasks() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupSelectTasks").attr("src", "wfSelectTaskCardList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummySelectTasks").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForSelectTasks() {
                var SelectTasksWindow = $find("<%=mdlPopupSelectTasks.ClientID %>");
                //close SelectTasks popup window
                SelectTasksWindow.hide();
                $("#iPopupSelectTasks").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddSelectTasks").click();
            }
		</script>
		<!-- End-->
		<!-- JobCompDetail Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyJobCompDetail" Text="Dummy JobCompDetail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupJobCompDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupJobCompDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupJobCompDetail" runat="server" targetcontrolid="btnDummyJobCompDetail"
			popupcontrolid="pnlPopupJobCompDetail" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameJobCompDetailStateComplete() {
                $("#btnDummyJobCompDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddJobCompDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobCompDetail").attr("src", "wfnWOJobComp_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyJobCompDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForJobCompDetail() {
                var JobCompDetailWindow = $find("<%=mdlPopupJobCompDetail.ClientID %>");
                //close JobCompDetail popup window
                JobCompDetailWindow.hide();
                $("#iPopupJobCompDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobCompDetail").click();
            }
		</script>
		<!-- End-->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div>
			<!-- JobCompList Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyJobCompList" Text="Dummy JobCompList" ClientIDMode="Static"></asp:Button>
			</div>
			<asp:Panel runat="server" ID="pnlJobCompList" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeJobCompList" frameborder="0" height="100%" allowtransparency="true"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:modalpopupextender id="mdlPopupJobCompList" runat="server" targetcontrolid="btnDummyJobCompList"
				popupcontrolid="pnlJobCompList" backgroundcssclass="clsModalPopupBG">
			</cc2:modalpopupextender>
			<script type="text/javascript">
                function IFrameJobCompListStateComplete() {
                    $("#btnDummyJobCompList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenJobCompListWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeJobCompList").attr("src", "wfnWOJobCompList.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyJobCompList").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForJobCompList() {
                    var JobCompListwindow = $find("<%=mdlPopupJobCompList.ClientID %>");
                    //close JobCompList popup window
                    JobCompListwindow.hide();
                    //           release resources
                    $("#IframeJobCompList").attr("src", "JavaScript:''");
                    //call JobCompList image button
                    $("#hdnBtnJobCompList").click();

                }

			</script>
			<!-- End-->
		</div>
		<!-- DesignaionAllocation Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyDesignaionAllocation" Text="Dummy DesignaionAllocation"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupDesignaionAllocation" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="iPopupDesignaionAllocation" frameborder="0" allowtransparency="true"
				height="100%" width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupDesignaionAllocation" runat="server" targetcontrolid="btnDummyDesignaionAllocation"
			popupcontrolid="pnlPopupDesignaionAllocation" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameDesignaionAllocationStateComplete() {
                $("#btnDummyDesignaionAllocation").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddDesignaionAllocation() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupDesignaionAllocation").attr("src", "wfnWOJobDesignationAllocation_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyDesignaionAllocation").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForDesignaionAllocation() {
                var DesignaionAllocationWindow = $find("<%=mdlPopupDesignaionAllocation.ClientID %>");
                //close DesignaionAllocation popup window
                DesignaionAllocationWindow.hide();
                $("#iPopupDesignaionAllocation").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddDesignaionAllocation").click();
            }
		</script>
		<!-- Job Spare List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyJobSpareList" Text="Dummy JobSpareList" ClientIDMode="Static"></asp:Button>
		</div>
		<asp:Panel runat="server" ID="pnlJobSpareList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeJobSpareList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupJobSpareList" runat="server" targetcontrolid="btnDummyJobSpareList"
			popupcontrolid="pnlJobSpareList" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameJobSpareListStateComplete() {
                $("#btnDummyJobSpareList").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenJobSpareListWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeJobSpareList").attr("src", "wfnWOJobSpareList.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyJobSpareList").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForJobSpareList() {
                var JobSpareListwindow = $find("<%=mdlPopupJobSpareList.ClientID %>");
                //close JobSpareList popup window
                JobSpareListwindow.hide();
                //           release resources
                $("#IframeJobSpareList").attr("src", "JavaScript:''");
                //call JobSpareList image button
                $("#hdnBtnJobSpareList").click();

            }

		</script>
		<!-- End-->
		<!-- JobSpareDetail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyJobSpareDetail" Text="Dummy JobSpareDetail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupJobSpareDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupJobSpareDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupJobSpareDetail" runat="server" targetcontrolid="btnDummyJobSpareDetail"
			popupcontrolid="pnlPopupJobSpareDetail" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameJobSpareDetailStateComplete() {
                $("#btnDummyJobSpareDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddJobSpareDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobSpareDetail").attr("src", "wfnWOJobSpare_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyJobSpareDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForJobSpareDetail() {
                var JobSpareDetailWindow = $find("<%=mdlPopupJobSpareDetail.ClientID %>");
                //close JobSpareDetail popup window
                JobSpareDetailWindow.hide();
                $("#iPopupJobSpareDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobSpareDetail").click();
            }
		</script>
		<!-- End-->
		<!-- NRC Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySelectNRC" Text="Dummy NRC" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupSelectNRC" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupSelectNRC" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupSelectNRC" runat="server" targetcontrolid="btnDummySelectNRC"
			popupcontrolid="pnlPopupSelectNRC" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameSelectNRCStateComplete() {
                $("#btnDummySelectNRC").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddSelectNRC() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupSelectNRC").attr("src", "wfnWOJobNRCList.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummySelectNRC").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }
                    return false;
                } catch (e) {
                    alert(e);
                };
            }
		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForSelectNRC() {
                var SelectNRCWindow = $find("<%=mdlPopupSelectNRC.ClientID %>");
                //close SelectNRC popup window
                SelectNRCWindow.hide();
                $("#iPopupSelectNRC").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddSelectNRC").click();
            }
		</script>
		<!-- End-->
		<!-- Requisition View-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyRequisitionView" Text="Dummy Requisition View"
				CausesValidation="false" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlRequisitionView" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IRequisitionView" allowtransparency="true" frameborder="0" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupRequisitionView" runat="server" targetcontrolid="btnDummyRequisitionView"
			popupcontrolid="pnlRequisitionView" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameRequisitionViewComplete() {
                $("#btnDummyRequisitionView").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }
            function RequisitionView() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IRequisitionView").attr("src", "wfReqItemsViewForWO_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyRequisitionView").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForRequisitionView() {
                var RequisitionViewwindow = $find("<%=mdlPopupRequisitionView.ClientID %>");
                //close Ass Insp Maint Done By Emp popup window
                RequisitionViewwindow.hide();
                //Free resources
                $("#IRequisitionView").attr("src", "JavaScript:''");
                //            $("#hdnBtnRequisitionView").click();

            }
		</script>
		<!-- WOTool Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWOTool" Text="Dummy WOTool" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupWOTool" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupWOTool" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupWOTool" runat="server" targetcontrolid="btnDummyWOTool"
			popupcontrolid="pnlPopupWOTool" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
            function IFrameWOToolStateComplete() {
                $("#btnDummyWOTool").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenWOTool() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupWOTool").attr("src", "wfnWOTool_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyWOTool").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForWOTool() {
                var WOToolWindow = $find("<%=mdlPopupWOTool.ClientID %>");
                //close WOTool popup window
                WOToolWindow.hide();
                $("#iPopupWOTool").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddWOTool").click();
            }
		</script>
		<!-- End-->
		<script type="text/javascript">
            function ddlToolTip(ddlWorkshop) {

                if (ddlWorkshop.value == "00000000-0000-0000-0000-000000000000") {
                    ddlWorkshop.title = "";
                } else {
                    ddlWorkshop.title = "Selected Workshop is : " + ddlWorkshop.options[ddlWorkshop.selectedIndex].text;
                }
            }
            function ddlselectedtext(ddlServiceprovider) {

                var txtIssueTo = $find("<%=txtIssueTo.ClientID %>");
                if (ddlWorkshop.value == "00000000-0000-0000-0000-000000000000") {
                    txtIssueTo.value = "";
                } else {
                    alert(ddlServiceprovider.options[ddlServiceprovider.selectedIndex].text);
                    txtIssueTo.value = ddlServiceprovider.options[ddlServiceprovider.selectedIndex].text;
                }
            }

		</script>
		<%--Added By Prashant 16-Aug-2019 call parent function after completing subroutine..(when WO detail page open as popup)--%>
		<script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForWODetail();
                return false;
            }
		</script>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameWODetailStateComplete();
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
				var tempMargtop = $("body #Table-MaxWidth:eq(0),html #Table-MaxWidth:eq(0)").outerHeight();
				var windowheight = $(window).height();
				if (tempMargtop >= windowheight) {
					$("body #Table-MaxWidth:eq(0),html #Table-MaxWidth:eq(0)").css({ 'margin': 'auto' });
				}
				else {
					var margintop = (windowheight / 2) - (tempMargtop / 2);
					$("body #Table-MaxWidth:eq(0),html #Table-MaxWidth:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
				}

			}
		</script>
		<%--End--%>
		<!-- WO Parameters Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWOParameters" Text="WO Parameters" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlWOParameters" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeWOParameters" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupWOParameters" runat="server" targetcontrolid="btnDummyWOParameters"
			popupcontrolid="pnlWOParameters" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
			function IFrameWOParametersStateComplete() {
				$("#btnDummyWOParameters").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenWOParameters() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeWOParameters").attr("src", "wfnWOParameters.aspx?Type=pup");

					//                if (!$.browser.msie) {
					$("#btnDummyWOParameters").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					//                }

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForWOParameters() {
				var WOParameterswindow = $find("<%=mdlPopupWOParameters.ClientID %>");
				//close WO Parameters popup window
				WOParameterswindow.hide();
				//           release resources
				$("#IframeWOParameters").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnWOParameters").click();
			}
		</script>
		<!-- End-->
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
		<!-- Change Location -->
		<div style="display: none">
			<asp:Button runat="server" ID="btndummyCloseAll" Text="Dummy Location" CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlCloseAllPanel">
			<div>
				<asp:UpdatePanel runat="server" ID="upnlPanelCloseAll" UpdateMode="Conditional">
					<ContentTemplate>
						<table class="clstablelistout" id="Table9">
							<tr>
								<td>
									<table class="clstablelistin" id="Table14">
										<tr>
											<td colspan="2" align="left" class="clsFormHeader1">
												<table>
													<tr>
														<td>
															<span id="Span2" class="clsFormHeader">Work Order Job Closing </span>
														</td>
														<td valign="top" align="right">
															<table id="Table15" cellspacing="1" cellpadding="1">
																<tr>
																	<td>
																		<asp:Button ID="bntCompleteAllJobs" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
																			Text="Complete" ToolTip="Click to Complete Job(s) Or NRC(s)"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
																			ToolTip="Click to close screen" CausesValidation="False"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>

											</td>
										</tr>
										<tr>
											<td colspan="2" align="left">
												<asp:UpdatePanel ID="upnlValidationsummary1" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:ValidationSummary ID="Validationsummary1" CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"
															runat="server"></asp:ValidationSummary>
														<asp:CustomValidator ID="CustomValidatorJob" runat="server" CssClass="clsLabelAuto"
															Display="None"></asp:CustomValidator>
														<asp:CustomValidator ID="cvjobStartDate" runat="server" Display="None" CssClass="clsLabelAuto"
															ControlToValidate="txtJobStartDate">
														</asp:CustomValidator>
														<asp:CustomValidator ID="cvJobEndDate" runat="server" Display="None" CssClass="clsLabelAuto"
															ControlToValidate="txtJobEndDate">
														</asp:CustomValidator>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td>
												<span id="Span3" class="clsLabelAuto">Start Date</span>
											</td>
											<td>
												<asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:TextBox ID="txtJobStartDate" runat="server" AutoPostBack="true"
															CssClass="clsTextBoxTagSearch"
															onchange="ValidateDateText(this,'txtJobStartDate_CalendarExtender');" />
														<cc2:calendarextender id="txtJobStartDate_CalendarExtender" runat="server"
															cssclass="cal_Theme1" enabled="True"
															format="<%$AppSettings:DateFormat%>" targetcontrolid="txtJobStartDate" />
														<cc2:textboxwatermarkextender id="txtJobStartDate_WatermarkExtender" runat="server"
															targetcontrolid="txtJobStartDate" watermarktext="<%$AppSettings:DateFormat%>"
															watermarkcssclass="clsTextBoxDate_Ajax" />
														<asp:TextBox ID="txtJobStartDateTime" runat="server" AutoPostBack="true"
															CssClass="clsTextBoxTagSearchSmall" MaxLength="10"
															Text="0:00" ToolTip="Enter Time" Width="65px" />
														<cc2:maskededitextender id="txtJobStartDateMaskedEditExtender"
															targetcontrolid="txtJobStartDateTime" runat="server"
															autocomplete="true" mask="99:99" masktype="Time"
															culturename="en-us" messagevalidatortip="true" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblEndDate" class="clsLabelAuto">End Date</span>
											</td>
											<td>
												<asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:TextBox ID="txtJobEndDate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
															onchange="ValidateDateText(this,'txtJobEndDate_CalendarExtender');" />
														<cc2:calendarextender id="txtJobEndDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
															enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtJobEndDate" />
														<cc2:textboxwatermarkextender id="TBWEEndDate" runat="server" targetcontrolid="txtJobEndDate"
															watermarktext="<%$AppSettings:DateFormat%>" watermarkcssclass="clsTextBoxDate_Ajax" />
														<asp:TextBox ID="txtJobEndDateTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
															Text="0:00" MaxLength="10" ToolTip="Enter Time" Width="65px" />
														<cc2:maskededitextender id="txtJobEndDateTimeMaskedEditExtender"
															targetcontrolid="txtJobEndDateTime" runat="server"
															autocomplete="true" mask="99:99" masktype="Time"
															culturename="en-us" messagevalidatortip="true" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>

									</table>
								</td>
							</tr>
						</table>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopUpChangeCloseAll" runat="server" targetcontrolid="btndummyCloseAll"
			popupcontrolid="pnlCloseAllPanel" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<!-- End Change Location -->
		<!---End-->
		<!-- Popup For MSPAssemblySelection -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyMSPAssemblySelection" Text="MSPAssemblySelection"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlMSPAssemblySelection" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeMSPAssemblySelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupMSPAssemblySelection" runat="server" targetcontrolid="btnDummyMSPAssemblySelection"
			popupcontrolid="pnlMSPAssemblySelection" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
			function OpenMSPAssemblySelectionWindow() {
				try {
					$("#IframeMSPAssemblySelection").attr("src", "wfMSPAssemblySelection_Ajax.aspx?Type=FromWO");
					$("#btnDummyMSPAssemblySelection").click();

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForMSPAssemblySelection() {
				var MSPAssemblySelectionwindow = $find("<%=mdlPopupMSPAssemblySelection.ClientID %>");
				//close popup window
				MSPAssemblySelectionwindow.hide();
				//           release resources
				$("#IframeMSPAssemblySelection").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnMSPAssemblySelection").click();
			}
		</script>
		<!---End-->

		<!-- Import Jobs for Third Party WO : TranstypeId =88-->
		<%--'Added by Saylee on 22-Jun-2023 , for Third Party job transferring--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnImportJobs" Text="Dummy Rate" CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlImportJobs" Style="display: none">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImportJobs">
				<ContentTemplate>
					<asp:Panel runat="server" ID="pnlJobs" Visible="false">
						<table class="clstablelistout" id="Table25" width="100%">
							<tr>
								<td>
									<table class="clstablelistin" id="Table16" width="100%">
										<tr>
											<td colspan="3" class="clsFormHeader1">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="Label16" CssClass="clsFormHeader" runat="server">Import Excel</asp:Label>
														</td>

														<td align="right">
															<table>
																<tr>
																	<td>
																		<asp:Button runat="server" ID="btnImport" CssClass="clsbtnH clsinfoH" Text="Import" Width="60px" />
																	</td>
																	<td>
																		<asp:Button ID="btnImportClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip="Click to close Import screen" Text="Close" CausesValidation="False"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblFile" runat="server" CssClass="clsLabelAuto">File</asp:Label>
											</td>

											<td>
												<div class="fileUpload1 uploadbtn">
													<span style="margin-top: 10px">Browse...</span>
													<asp:FileUpload runat="server" ID="BrowseYourFile" onchange="showfilepath(this);" EnableViewState="true" ClientIDMode="Static" />
												</div>
											</td>
											<td></td>
										</tr>
										<tr>
											<td colspan="3" align="left">
												<div id="FileContentShow" style="overflow: hidden; width: 300px;">

													<div id="filepath" style="display: inline-block; left: 0; position: relative; font-family: Segoe UI; white-space: nowrap; color: gray; font-style: italic;">
														No file selected
													</div>
												</div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</asp:Panel>

				</ContentTemplate>
				<Triggers>
					<asp:PostBackTrigger ControlID="btnImport" />
				</Triggers>

			</asp:UpdatePanel>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopUpImportJobs" runat="server" targetcontrolid="btnImportJobs" clientidmode="Static"
			popupcontrolid="pnlImportJobs" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>

		<!-- End Import Jobs for Third Party WO : TranstypeId =88-->

		<%--Added by Prashant on 6-Jul-2023--%>
		<!-- Popup For CustomerContractSelection -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyCustomerContractSelection" Text="CustomerContractSelection"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlCustomerContractSelection" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeCustomerContractSelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupCustomerContractSelection" runat="server" targetcontrolid="btnDummyCustomerContractSelection"
			popupcontrolid="pnlCustomerContractSelection" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
			function OpenCustomerContractSelectionWindow() {
				try {
					$("#IframeCustomerContractSelection").attr("src", "wfCustomerContractSelection_Ajax.aspx?Type=FromWO");
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
		<!---End-->
		<!--DigitalSignatureRequest Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyDigitalSignatureRequest" Text="DigitalSignatureRequest" CausesValidation="false"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlDigitalSignatureRequest" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeDigitalSignatureRequest" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupDigitalSignatureRequest" runat="server" targetcontrolid="btnDummyDigitalSignatureRequest"
			popupcontrolid="pnlDigitalSignatureRequest" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
			function IFrameDigitalSignatureRequestStateComplete() {
				$("#btnDummyDigitalSignatureRequest").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenDigitalSignatureRequestWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeDigitalSignatureRequest").attr("src", "wfDigitalSignatureRequest.aspx?Type=pup");


					$("#btnDummyDigitalSignatureRequest").click();
					$get("AjaxLoader").style.visibility = 'hidden';

					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForDigitalSignatureRequest() {
				var DigitalSignatureRequestwindow = $find("<%=mdlPopupDigitalSignatureRequest.ClientID %>");
				//close popup window
				DigitalSignatureRequestwindow.hide();
				//release resources
				$("#IframeDigitalSignatureRequest").attr("src", "JavaScript:''");
				//call button click
				$("#hdnBtnDigitalSignatureRequest").click();
			}
		</script>
		<!-- End-->
		<!--WorkOrder Job Attach Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlAttach" runat="server" targetcontrolid="btnDummyAttach"
			popupcontrolid="pnlAttach" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
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
	</form>
	<script src="js/jquery.js" type="text/javascript"></script>
	<script src="js/jquery-1.8.3.js" type="text/javascript"></script>
	<script type="text/javascript" src="Notification/jQuery/ui.core.js"></script>
	<script type="text/javascript" src="Notification/jQuery/ui.notificationmsg.js"></script>
	<script src="bootstrap/bootstrap-toggle.min.js" type="text/javascript"></script>
	<script src="js/semantic.js" type="text/javascript"></script>

	<%--'Added by Saylee on 22-Jun-2023 , for Third Party job transferring--%>
	<script type="text/javascript">

		$(document).ready(function () {
			$("#<%=btnImport.ClientID %>").live("click", function () {
				var tempval = document.getElementById("BrowseYourFile").value;

				if (tempval) {
					document.getElementById("BrowseYourFile").value = tempval;
					return true;
				}
				else {
					return false;
				}
			});
		});

		var timeout;
		var duration;
		var marginleft;

		function showfilepath(elem) {
			$("#<%=btnImport.ClientID %>").removeAttr('disabled');
			$("#filepath").clearQueue().stop();
			$("div:animated").stop(true, true);
			$("#filepath").html('');
			$("#filepath").html(elem.value);
			$("#filepath").attr("title", elem.value);
			$("#filepath").css({ 'left': '0', 'font-style': 'normal', 'color': '#1C1F24' });
			marginleft = $("#filepath").parent().width() - $("#filepath").width();
			if (marginleft < 0) {
				duration = ((-1 * marginleft) / 100) * 2000;
				Marquee(marginleft, duration);
			}
		}

		function Marquee(margin, dur) {
			$("#filepath").delay(2000).animate({ 'left': margin }, dur, 'linear', function () {
				$("#filepath").delay(2000).animate({ 'left': 0 }, 0, 'linear');
				Marquee(marginleft, duration);
			});

		}
	</script>
</body>
</html>

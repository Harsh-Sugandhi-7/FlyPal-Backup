<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDiscrepancyCorrectiveAction.aspx.vb"
	Inherits="Flypal.DiscrepancyCorrectiveActionDetailPage" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Discrepancy Detail</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

    <script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="AlertMessage.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout Table-MaxWidth">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                            <table id="tblinner" class="clsTablelistin" width="100%">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblSnagCorrectiveActionInfo" runat="server"
                                                                CssClass="clsFormHeader">
																Discrepancy
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlHeaderButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSave" runat="server" 
																CssClass="clsbtnH clsinfoH"
                                                                Text="Save" ValidationGroup="a"
                                                                CausesValidation="true" />

                                                            <asp:PlaceHolder id="phSendEMail" runat="server" 
                                                                visible="<%#Not DiscrepancyCorrectiveAction.IsNew %>">
                                                                <asp:Button ID="btnSendMail" runat="server"
                                                                    CssClass="clsbtnH clsinfoH" Text="Send Mail"
                                                                    ToolTip="Send Mail" />
                                                            </asp:PlaceHolder>

                                                            <asp:Button ID="btnPrint" runat="server" 
                                                                CssClass="clsbtnH clsinfoH" Text="Print"
                                                                ToolTip="Print Detailed Report."
                                                                Enabled="false" Visible="false" />
                                                            <asp:Button ID="btnBack" runat="server" 
                                                                CssClass="clsbtnH clsinfoH" Text="Close"
                                                                CausesValidation="False" />
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
                                                <asp:ValidationSummary ID="vsDiscrepencyDetails" CssClass="clsValidationSummary" runat="server"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Fields" />
                                                <asp:CustomValidator ID="cvDefectList" runat="server" CssClass="clsLabelAuto" 
                                                    ControlToValidate="txtDefect"
                                                    ValidationGroup="a" ErrorMessage="Discrepancy is required." 
                                                    Display="None" OnServerValidate="CustomValidate" />
                                                <asp:RequiredFieldValidator ID="rfvDefect" runat="server"
													CssClass="clsLabelAuto"
													ValidationGroup="a" ControlToValidate="txtDefect"
													ErrorMessage="Discrepancy is Required."
													Display="None" />
                                                <asp:CustomValidator ID="cvLogNo" runat="server"
													CssClass="clsLabelAuto"
													ControlToValidate="cmbLogNo"
													ValidationGroup="a" Display="None"
													ErrorMessage="Please select a Log."
													OnServerValidate="CustomValidate" />
                                                <asp:RequiredFieldValidator ID="rfvText" runat="server" 
                                                    ControlToValidate="txtDefectReportNo"
                                                    CssClass="clsLabelAuto" Display="None" 
                                                    ErrorMessage="Discrepancy is Text Required." 
													ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvNo" runat="server" 
													CssClass="clsLabelAuto" 
                                                    ControlToValidate="txtNo" 
													ValidationGroup="a" 
                                                    ErrorMessage="No. Required" 
													Display="None" />
                                                <asp:CustomValidator ID="cvOccDate" 
													runat="server" 
													CssClass="clsLabelAuto" 
                                                    ControlToValidate="txtDateofoccurrence"
                                                    ValidationGroup="a" Display="None" 
                                                    OnServerValidate="CustomValidate" 
													ErrorMessage="Date is Required.." />
                                                <asp:RequiredFieldValidator ID="rfDateofoccurrence" 
                                                    runat="server" CssClass="clsLabelAuto"
                                                    ControlToValidate="txtDateofoccurrence" 
                                                    ErrorMessage="Date of Occurrence is Required.." 
                                                    Display="None" ValidationGroup="a" />
                                                <asp:CustomValidator ID="cvDiscrepancyDetails" runat="server" 
                                                    CssClass="clsLabelAuto" ValidationGroup="a"
                                                    ControlToValidate="txtDefectReportNo" 
                                                    Display="None" ErrorMessage="">
                                                </asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlMELSnagDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" width="100%">
													<tr>
														<td>
															<fieldset class="clsFieldSetNewStyle">
																<legend id="lgnDiscrepencyDetails" runat="server">
																	<asp:Label runat="server" ID="lblDetailLegend"
																		Font-Bold="true" />
																</legend>
																<table width="100%">
																	<tr>
																		<td>
																			<asp:Label ID="lblDatePlaceofoccuranceStar"
																				runat="server" CssClass="clsLabelStar" Text="*" />
																		</td>
																		<td>
																			<asp:Label ID="lblDatePlaceofoccurance" runat="server"
																				CssClass="clsLabelAuto"
																				Text='<%#IIf(AppSettings("ClientCode") = "7AR", "Date Of Occurrence (UTC)", "Date Of Occurrence") %>' />
																		</td>
																		<td>
																			<asp:UpdatePanel ID="upnlOccurranceDate" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:TextBox ID="txtDateofOccurrence" 
																						runat="server" AutoPostBack="True"
																						Enabled="<%# DiscrepancyCorrectiveAction.IsNew %>"
																						CssClass="clsTextBoxTagSearchDate" 
																						CausesValidation="true" Width="100px"
																						onchange="ValidateDateText(this,'txtDateofOccurrence_CalendarExtender');" />
																					<cc2:CalendarExtender ID="txtDateofOccurrence_CalendarExtender" 
																						runat="server" CssClass="cal_Theme1"
																						Enabled="True" Format="<%$AppSettings:DateFormat%>"
																						TargetControlID="txtDateofOccurrence" />
																					<cc2:TextBoxWatermarkExtender ID="tbwmeDateOfOccurence" runat="server"
																						TargetControlID="txtDateofOccurrence"
																						WatermarkText="<%$AppSettings:DateFormat%>"
																						WatermarkCssClass="clsDateTextBox" />
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td>
																			<asp:Label ID="lblLogNoStar" runat="server"
																				CssClass="clsLabelStar" Text="*" />
																		</td>
																		<td>
																			<asp:Label ID="lblLogNo" runat="server"
																				CssClass="clsLabelAuto" Text="Log No." />
																		</td>
																		<td>
																			<asp:UpdatePanel ID="upnlLogNo" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:DropDownList ID="cmbLogNo" runat="server"
																						CssClass="clsTextBoxTagSearchComboNewstyle"
																						DataValueField="LogID"
																						Enabled="<%# DiscrepancyCorrectiveAction.IsNew %>"
																						DataTextField="LogNoLogPageNo" AutoPostBack="True" />
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td align="left">
																			<asp:UpdatePanel ID="upnlLogNoLink" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:LinkButton ID="lnkCheckStatus" runat="server"
																						CssClass="clsLinkButton"
																						ToolTip="View Log Information."
																						CausesValidation="False"
																						Enabled="False" Text="View" />
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td align ="right">
																			<asp:Label ID="lblDefectReportNoStar" runat="server"
																				CssClass="clsLabelStar" Text="*" />
																		</td>
																		<td>
																			<asp:Label ID="lblDefectReportNo" runat="server"
																				CssClass="clsLabelAuto" />
																		</td>
																		<td>
																			<asp:TextBox ID="txtDefectReportNo" runat="server"
																				Enabled='<%#IIf(AppSettings("ClientCode") = "FIT" And
																							 DiscrepancyCorrectiveAction.IsNew, "True", "False") %>'
																				CssClass="clsTextBoxTagSearch" Width="100px"
																				Text="<%# DiscrepancyCorrectiveAction.DefectReportNo %>"
																				MaxLength="25" />
																			<asp:TextBox ID="txtNo" runat="server"
																				Enabled='<%#IIf(AppSettings("ClientCode") = "FIT" And
																						DiscrepancyCorrectiveAction.IsNew, "True", "False") %>'
																				CssClass="clsTextBoxTagSearchSmall" Width="30px"
																				Text="<%# DiscrepancyCorrectiveAction.No %>" MaxLength="4" />
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<asp:Label ID="lblDefectStar" runat="server"
																				CssClass="clsLabelStar" Text="*" />
																		</td>
																		<td>
																			<asp:Label ID="lblDefect" runat="server"
																				CssClass="clsLabelAuto" />
																		</td>
																		<td colspan="8">
																			<asp:TextBox ID="txtDefect" runat="server"
																				class="clsTextBoxTagSearchLong" Height="46px"
																				Text="<%# DiscrepancyCorrectiveAction.Defect %>"
																				MaxLength="1000" TextMode="MultiLine" />
																		</td>
																	</tr>
																	<tr>
																		<td></td>
																		<td>
																			<asp:Label ID="lblSector" runat="server"
																				CssClass="clsLabelAuto" Text="Sector / Place" />
																		</td>
																		<td>
																			<asp:UpdatePanel ID="upnlSector" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:TextBox ID="txtSector" runat="server"
																						CssClass="clsTextBoxTagSearch" Enabled="false"
																						Text="<%# DiscrepancyCorrectiveAction.Sector %>" MaxLength="50"
																						ToolTip="Enter Sector / Place" />
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td></td>
																		<td>
																			<asp:Label ID="lblAircraftHrsLandingsTSNsincelastmajorcheck"
																				runat="server" CssClass="clsLabelAuto" Text="Aircraft TSN / CSN" />
																		</td>
																		<td>
																			<asp:UpdatePanel ID="upnlLastMajorCheck" runat="server"
																				UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:TextBox ID="txtLastMajorCheck" runat="server"
																						CssClass="clsTextBoxTagSearch" Width="128px" Enabled="false"
																						Text="<%# DiscrepancyCorrectiveAction.LastMajorCheckHour %>"
																						ToolTip="Enter Aircraft Hrs. / Landings">
																					</asp:TextBox>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td></td>
																		<td></td>
																		<td>
																			<asp:Label ID="lblReportedBy" runat="server"
																				CssClass="clsLabelAuto" Text="Reported By" />
																		</td>
																		<td>
																			<asp:TextBox ID="txtReportedBy"
																				runat="server" MaxLength="150"
																				CssClass="clsTextBoxTagSearch"
																				Text="<%# DiscrepancyCorrectiveAction.ReportedBy %>"  />
																		</td>
																	</tr>
																	<asp:PlaceHolder runat="server" ID="phReportedAs">
																		<tr>
																			<td colspan="3">
																				<asp:UpdatePanel runat="server" ID="upnlReportedAs" UpdateMode="Conditional">
																					<ContentTemplate>
																						<fieldset runat="server" class="clsFieldSetNewStyle">
																							<legend>
																								<b>Reported As</b>
																							</legend>
																							<table id="tblDefectTypeRadio" width="100%">
																								<tr>
																									<td>
																										<asp:RadioButton ID="rbPireps" runat="server"
																											CssClass="clsRadioButton" Text="Pireps" Width="60px"
																											Checked="<%# DiscrepancyCorrectiveAction.IsPireps %>"
																											GroupName="b" />
																									</td>
																									<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
																									<td>
																										<asp:RadioButton ID="rbMaintenanceDefect" runat="server"
																											CssClass="clsRadioButton" Width="156px"
																											Text="Maintenance Defect" GroupName="b"
																											Checked="<%# DiscrepancyCorrectiveAction.IsMaintenanceDefect %>" />
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
																		<td colspan="5" align="left" style="padding-left: 10px;">
																			<asp:PlaceHolder ID="phWO" runat="server" Visible="false">
																				<asp:UpdatePanel ID="upnlCreateWO" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<asp:LinkButton ID="lnkbtnCreateWorkOrder" 
																							CssClass="clsLinkButton" runat="server"
																							Visible="<%# Not DiscrepancyCorrectiveAction.InvestigationStatus And
																										Not DiscrepancyCorrectiveAction.IsNew %>"
																							Text="Create Work Order">
																						</asp:LinkButton>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</asp:PlaceHolder>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</td>
													</tr>
                                                    <asp:PlaceHolder ID="phShowVerificationDet" runat="server">
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnShowVerificationDet"
                                                                    value="Verification & Rectification Details &#9660;"
                                                                    class="clsbtnH clsinfoH1"
                                                                    onclick="slidePanel('<%= tabMELLogDetailsContainer.ClientID %>')" />
                                                            </td>
                                                        </tr>
                                                    </asp:PlaceHolder>
                                                    <tr>
                                                        <td>
                                                            <cc2:TabContainer ID="tabMELLogDetailsContainer" runat="server" 
																class="clstablelistin" AutoPostBack="False">
                                                                <cc2:TabPanel ID="pnlVerificationDetails" runat="server" 
																	CssClass="clsPanel1" ClientIDMode="Static">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" Text="Verification & Rectification Details"
																			ID="lblVerficationDeatils" />
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:UpdatePanel ID="upnlMMELDetails" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <fieldset class="clsFieldSetNewStyle">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td colspan="10">
                                                                                                <asp:UpdatePanel runat="server" ID="upnlRectificationDetailsErrors" 
																									UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:ValidationSummary ID="RectificationDetailsErrors"
                                                                                                            CssClass="clsValidationSummary" runat="server" 
																											ValidationGroup="2"
                                                                                                            HeaderText="Fill Up The Following Fields" />
                                                                                                        <asp:CustomValidator ID="cvActionList" runat="server"
                                                                                                            CssClass="clsLabelAuto" ControlToValidate="txtAction"
                                                                                                            ValidationGroup="2" ErrorMessage="Action  required."
                                                                                                            Display="None" OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvRectifiedLogNo" runat="server"
                                                                                                            CssClass="clsLabelAuto" ValidationGroup="2"
                                                                                                            ControlToValidate="cmbRectifiedLogNo" 
																											ErrorMessage="Log required"
                                                                                                            Display="None" OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvRectifiedDate" runat="server"
                                                                                                            CssClass="clsLabelAuto" ValidationGroup="2"
                                                                                                            ControlToValidate="txtRectifiedDate" Display="None"
                                                                                                            OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvRectificationDetails" 
																											runat="server" CssClass="clsLabelAuto" 
																											ValidationGroup="RectificationDetails"
                                                                                                            ControlToValidate="txtRectifiedDate" Display="None" 
																											ErrorMessage="" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="10">
                                                                                                <asp:UpdatePanel ID="upnlVerificationDetailsErrors" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:ValidationSummary ID="vsVerificationDetails" runat="server"
                                                                                                            HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"
                                                                                                            ValidationGroup="1" />
                                                                                                        <asp:CustomValidator ID="cvComponent" runat="server" ControlToValidate="cmbMELCategory"
                                                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="" OnServerValidate="CustomValidate"
                                                                                                            ValidationGroup="1" />
                                                                                                        <asp:CustomValidator ID="cvFrequencyInHours" runat="server" CssClass="clsLabelAuto"
                                                                                                            ValidationGroup="1" ControlToValidate="txtFrequencyInHours"
                                                                                                            ErrorMessage="Please select MEL Category"
                                                                                                            Display="None" OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvFrequencyInDay" runat="server" CssClass="clsLabelAuto"
                                                                                                            ValidationGroup="1" ControlToValidate="txtFrequencyInDay"
                                                                                                            ErrorMessage="Please select the Due Date"
                                                                                                            Display="None" OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvDueDate" runat="server" CssClass="clsLabelAuto"
                                                                                                            ControlToValidate="txtDueDate" ValidationGroup="1" Display="None"
                                                                                                            OnServerValidate="CustomValidate" />
                                                                                                        <asp:CustomValidator ID="cvEx" runat="server" ControlToValidate="txtExtensionInDays"
                                                                                                            ErrorMessage="Extension days should be greater than zero"
                                                                                                            OnServerValidate="CustomValidate"
                                                                                                            Display="None" CssClass="clslabelauto" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="10" align="top">
                                                                                                <asp:PlaceHolder runat="server" ID="phDiscrepancyCategoryAndReliability">
                                                                                                    <asp:UpdatePanel ID="upnlSnagType" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table id="tblSnagType" width="100%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <fieldset class="clsFieldSetNewStyle">
                                                                                                                            <legend>
                                                                                                                                <b>
                                                                                                                                    <asp:Label Text="Discrepancy Category"
                                                                                                                                        runat="server" ID="lblMELLabType" />
                                                                                                                                </b>
                                                                                                                            </legend>
                                                                                                                            <table id="tblSnagTypeRadio">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:RadioButton ID="rbMajor" runat="server"
                                                                                                                                            CssClass="clsRadioButton" Text="Major" Width="60px"
                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsMajor %>" 
																																			GroupName="a" />
                                                                                                                                    </td>
                                                                                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:RadioButton ID="rbMinor" runat="server"
                                                                                                                                            CssClass="clsRadioButton" Text="Minor" Width="60px"
                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsMinor %>" 
																																			GroupName="a" />
                                                                                                                                    </td>
                                                                                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:RadioButton ID="rbIncident" runat="server"
                                                                                                                                            CssClass="clsRadioButton" Text="Incident" Width="120px"
                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsIncident %>"
                                                                                                                                            GroupName="a" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </fieldset>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <fieldset class="clsFieldSetNewStyle">
                                                                                                                            <legend>
                                                                                                                                <b>Reliability
                                                                                                                                </b>
                                                                                                                            </legend>
                                                                                                                            <table id="tblReliabilityChkbx">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:CheckBox ID="chkIsInReliability" runat="server"
                                                                                                                                            CssClass="clsCheckBox" Width="152px"
                                                                                                                                            Text="Consider In Reliability"
                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsInReliability %>" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </fieldset>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </asp:PlaceHolder>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label Width="100px" ID="lblATAChapter" runat="server" 
																									CssClass="clsLabelAuto" Text="ATA Chapter" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="cmbATAChapter" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                            AutoPostBack="True"
                                                                                                            SelectedValue="<%# DiscrepancyCorrectiveAction.ATAChapterID %>"
                                                                                                            DataValueField="ID"
                                                                                                            DataTextField="ATAChapter" />

                                                                                                        <asp:CustomValidator ID="cvATA" runat="server"
                                                                                                            CssClass="clslabelauto" Display="None"
                                                                                                            OnServerValidate="CustomValidate"
                                                                                                            ControlToValidate="cmbATAChapter" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblSubATAChapter" runat="server" 
                                                                                                    CssClass="clsLabelAuto" 
                                                                                                    Width="100px" Text="Sub-ATA Chapter" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="cmbSubATAList" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                            DataTextField="SubATAChapter" Width="278px">
                                                                                                        </asp:DropDownList>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <asp:UpdatePanel ID="upnlIsRepetitive" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <td colspan="2">
                                                                                                        <asp:CheckBox ID="chkIsRepetitive" runat="server"
																											CssClass="clsCheckBox"
                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsRepetitive %>" />
                                                                                                    </td>
																									<td>
																										<asp:Label ID="lblIsRepetitive" runat="server"
																											CssClass="clsLabelAuto" Text="Is Repetitive" />
																									</td>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
																							<td></td>
                                                                                        </tr>
                                                                                        <tr>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblIncidentType" runat="server"
																									CssClass="clsLabelAuto" Text="Incident Type" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbIncidentType" runat="server"
                                                                                                    CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                    DataValueField="ID" DataTextField="Name" 
                                                                                                    SelectedValue="<%# DiscrepancyCorrectiveAction.IncidentTypeID %>" />
                                                                                            </td>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblDefectOn" runat="server"
                                                                                                    CssClass="clsLabelAuto" Text="On Assembly" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="cmbAssembly" runat="server"
																											CssClass="clsTextBoxTagSearchComboNewstyle"
																											DataTextField="ModelSerialNoPostion" Width="278px"
																											SelectedValue="<%# DiscrepancyCorrectiveAction.AssemblyStatusID %>"
																											DataValueField="AssemblyStatusID" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
																							<td></td>
																							<td></td>
																							<td></td>
																							<td></td>
                                                                                        </tr>
                                                                                        <tr>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblAction" runat="server" 
																									CssClass="clsLabelAuto" Text="Action" />
                                                                                            </td>
                                                                                            <td colspan="8">
                                                                                                <asp:TextBox ID="txtAction" runat="server"
                                                                                                    class="clsTextBoxTagSearchLong" Height="46px"
                                                                                                    Text="<%# DiscrepancyCorrectiveAction.Action %>"
                                                                                                    ToolTip="Enter Action" MaxLength="1000" TextMode="MultiLine">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
																							<td></td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblRectificationMechanic" runat="server" 
																									CssClass="clsLabelAuto" Text="Rectification By" />
                                                                                            </td>
                                                                                            <td colspan="8">
                                                                                                <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtLicenceNo" runat="server" 
																														CssClass="clsTextBoxTagSearch"
                                                                                                                        ToolTip="Enter Rectification By" AutoComplete="off"
																														ClientIDMode="Static"
                                                                                                                        OnTextChanged="LicenseNo_TextChanged" 
																														AutoPostBack="true" MaxLength="200" />
                                                                                                                    <cc2:AutoCompleteExtender ClientIDMode="Static" 
																														ID="txtLicenceNo_Autocomplete"
                                                                                                                        runat="server" DelimiterCharacters="" 
																														Enabled="True" CompletionSetCount="20"
                                                                                                                        MinimumPrefixLength="0" CompletionInterval="1"
                                                                                                                        ServicePath="wfDiscrepancyCorrectiveAction.aspx"
																														ServiceMethod="GetLicenseList"
                                                                                                                        TargetControlID="txtLicenceNo" 
																														UseContextKey="False" ContextKey=""
                                                                                                                        CompletionListCssClass="ac_results_Main"
                                                                                                                        CompletionListItemCssClass="ac_results_li"
                                                                                                                        CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                                        OnClientPopulated="ClientPopulated" 
																														OnClientPopulating="ClientPopulating"
                                                                                                                        OnClientHiding="ClientHiding"
                                                                                                                        OnClientShown="ClientHiding" 
																														OnClientShowing="ClientShowing" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server"
                                                                                                                        ImageUrl="~/images/plus1.png"
                                                                                                                        Height="22px" Width="24px"
                                                                                                                        ToolTip="Add Multiple Mechanics."
                                                                                                                        CausesValidation="true" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="2">
                                                                                                                    <asp:Label ID="lblLicenceCount" runat="server"
                                                                                                                        Visible="<%# DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                                        ToolTip="<%# DiscrepancyCorrectiveAction.AllLicenceNos%>"
                                                                                                                        Text="and More" CssClass="clsLabelHeader clsCursorStyle" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" colspan="10">
                                                                                                <asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Panel ID="pnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
                                                                                                            <div>
                                                                                                                <div id="divCollapsiblePnl">
                                                                                                                    <table width="100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <span id="lblMastersSelection" class="clsLabelHeader">
                                                                                                                                    Component Information
                                                                                                                                </span>
                                                                                                                            </td>
                                                                                                                            <td align="right">
                                                                                                                                <div id="divCollapsiblePnlImg">
                                                                                                                                    <image id="imgMasters" src="images/collapse_blue.jpg"
                                                                                                                                        alternatetext="(Show Details...)" />
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </asp:Panel>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" colspan="10">
                                                                                                <asp:UpdatePanel runat="server" ID="upnlAvanceSearchContent" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Panel ID="pnlAdvancedSearchContent" runat="server">
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td colspan="10">
                                                                                                                        <asp:Label ID="lblNote" runat="server" Font-Bold="true"	
																															CssClass="clsLabelAuto"	Font-Size="Small"
                                                                                                                            Text="Please select part, If not present then type in Part No. field." />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
																													<td></td>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblComponent" runat="server" 
                                                                                                                            CssClass="clsLabelAuto" Text="Component" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:UpdatePanel ID="upnlPartNoCombo" runat="server" 
																															UpdateMode="Conditional">
                                                                                                                            <ContentTemplate>
                                                                                                                                <asp:DropDownList ID="cmbPartNo" runat="server"
                                                                                                                                    CssClass="clsTextBoxTagSearchComboNewstyle" 
																																	AutoPostBack="True"
                                                                                                                                    SelectedValue="<%# DiscrepancyCorrectiveAction.PartID %>"
                                                                                                                                    DataValueField="CompID"
                                                                                                                                    DataTextField="PartNoSerialNo">
                                                                                                                                </asp:DropDownList>
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
																													<td></td>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblPartNo" runat="server" 
																															CssClass="clsLabelAuto" Text="Part No." />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:UpdatePanel ID="upnlPartNo" runat="server" 
																															UpdateMode="Conditional">
                                                                                                                            <ContentTemplate>
                                                                                                                                <asp:TextBox ID="txtPartNo" runat="server" Width="200px"
                                                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                                                    Text="<%# DiscrepancyCorrectiveAction.PartNo %>"
                                                                                                                                    ToolTip="Enter Part No." MaxLength="50" />
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
																													<td></td>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblSerial" runat="server" 
																															CssClass="clsLabelAuto" Text="Serial No" />
                                                                                                                    </td>
                                                                                                                    <td colspan="6">
                                                                                                                        <asp:UpdatePanel ID="upnlSerialNo" runat="server" UpdateMode="Conditional">
                                                                                                                            <ContentTemplate>
                                                                                                                                <asp:TextBox ID="txtSerialNo" runat="server" Width="100px"
                                                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                                                    Text="<%# DiscrepancyCorrectiveAction.PartSerialNo %>"
                                                                                                                                    ToolTip="Enter Serial No." MaxLength="50" />
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
																													<td></td>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblDescription" runat="server" 
																															CssClass="clsLabelAuto" Text="Description" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:UpdatePanel ID="upnlDesc" runat="server" UpdateMode="Conditional">
                                                                                                                            <ContentTemplate>
                                                                                                                                <asp:TextBox ID="txtDescription" runat="server"
																																	CssClass="clsTextBoxTagSearch"
																																	Text="<%# DiscrepancyCorrectiveAction.Description %>"
																																	ToolTip="Enter Description" MaxLength="50">
                                                                                                                                </asp:TextBox>
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
																													<td></td>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblHrsofComp" Width="100px" runat="server"
                                                                                                                            CssClass="clsLabelAuto" Text="Current Values of Comp" />
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtHrsofComp" runat="server"
                                                                                                                            CssClass="clsTextBoxTagSearch"
                                                                                                                            Width="128px" Text="<%# DiscrepancyCorrectiveAction.ComponentHour %>"
                                                                                                                            ToolTip="Enter Component Current Values"
                                                                                                                            MaxLength="50">
                                                                                                                        </asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </asp:Panel>
                                                                                                        <cc2:CollapsiblePanelExtender BehaviorID="clpBehaviour" ID="clpextAdvancedSearch"
                                                                                                            ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearchContent"
                                                                                                            ExpandControlID="pnlAdvancedSearch" CollapseControlID="pnlAdvancedSearch"
                                                                                                            Collapsed="True" ImageControlID="imgMasters"
                                                                                                            CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
                                                                                                            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                                                            SuppressPostBack="false" />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="10">
                                                                                                <asp:Panel ID="pnlVerify" runat="server">
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td colspan="5" align="right">
                                                                                                                <asp:UpdatePanel ID="upnlLinks" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:LinkButton ID="lnkMELDetail" 
																															runat="server" CssClass="clsHyperlink1"
                                                                                                                            Text="View MEL Detail"
                                                                                                                            ToolTip="Click to view MEL details"
                                                                                                                            Visible="<%# Not DiscrepancyCorrectiveAction.IsNew And
																																	Not DiscrepancyCorrectiveAction.MELID.Equals(Guid.Empty)%>">
                                                                                                                        </asp:LinkButton>
                                                                                                                        <asp:LinkButton ID="lnkDeferredDetail" 
																															runat="server" CssClass="clsHyperlink1"
                                                                                                                            Text="View Deferred Detail"
                                                                                                                            ToolTip="Click to Deferred details"
                                                                                                                            Visible="<%# Not DiscrepancyCorrectiveAction.IsNew And
																																			Not DiscrepancyCorrectiveAction.DeviationListID.Equals(Guid.Empty)%>">
                                                                                                                        </asp:LinkButton>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
																											<td colspan="5" align="right">
                                                                                                                <asp:UpdatePanel ID="upnlTroubleshootCount" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:Label ID="lblTroubleCount" runat="server" 
																															CssClass="clsLabelHeader" Style="z-index: 0"
                                                                                                                            Visible="<%# Not DiscrepancyCorrectiveAction.IsNew And
																																			DiscrepancyCorrectiveAction.TotalTroubleShootCount > 0 %>">
                                                                                                                        </asp:Label>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInvestigationStatus" runat="server" CssClass="clsLabelAuto">Status</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:UpdatePanel ID="upnlInvestigation" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="cmbInvestigation" runat="server"
                                                                                                                            CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
                                                                                                                            Enabled="<%#Not DiscrepancyCorrectiveAction.IsNew And
																																			DiscrepancyCorrectiveAction.InvestigationStatus = False%>">
                                                                                                                            <asp:ListItem Value="0">(SELECT)</asp:ListItem>
                                                                                                                            <asp:ListItem Value="1">Closed</asp:ListItem>
                                                                                                                            <asp:ListItem Value="2">Deferred</asp:ListItem>
                                                                                                                            <asp:ListItem Value="3">AOG</asp:ListItem>
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td colspan="2">
                                                                                                                <asp:PlaceHolder ID="phDeviationMEL" runat="server">
                                                                                                                    <asp:UpdatePanel ID="upnlMELDeviation" runat="server" UpdateMode="Conditional">
                                                                                                                        <ContentTemplate>
                                                                                                                            <table>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:RadioButton ID="rdbMEL" runat="server" 
																																			GroupName="ml" Text="MEL (Minimum Equipment List)" 
																																			AutoPostBack="True" Checked="<%# DiscrepancyCorrectiveAction.IsMEL %>" />
                                                                                                                                        <asp:LinkButton ID="lnkMEL" runat="server" 
																																			CssClass="clsHyperlink1"
                                                                                                                                            Text="(Change)" ToolTip="Click to MEL data"
                                                                                                                                            Visible="<%# Not DiscrepancyCorrectiveAction.IsNew And Not DiscrepancyCorrectiveAction.MELID.Equals(Guid.Empty)  %>">
                                                                                                                                        </asp:LinkButton>
                                                                                                                                        <asp:CheckBox ID="CheckBox1" runat="server" 
                                                                                                                                            Visible="false" CssClass="clsCheckBox" 
                                                                                                                                            Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD (check if ADD)", "MEL (check if MEL)") %>'
                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.IsMEL %>" 
                                                                                                                                            AutoPostBack="True" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:RadioButton ID="rdbCDL" runat="server" 
                                                                                                                                            GroupName="ml" Text="Other Deferred List" 
                                                                                                                                            AutoPostBack="True" 
																																			Checked="<%# DiscrepancyCorrectiveAction.IsDeviationList %>" />
                                                                                                                                        <asp:LinkButton ID="lnkDeferredList" runat="server" 
                                                                                                                                            CssClass="clsHyperlink1"
                                                                                                                                            Text="(Change)" ToolTip="Click to Deferred data"
                                                                                                                                            Visible="<%# Not DiscrepancyCorrectiveAction.IsNew And Not DiscrepancyCorrectiveAction.DeviationListID.Equals(Guid.Empty)  %>" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </ContentTemplate>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </asp:PlaceHolder>
                                                                                                            </td>
                                                                                                        </tr>
																										<asp:PlaceHolder ID="rectDate" runat="server" Visible="false">
																											<tr>
																												<td>
																													<asp:Label ID="lblRectifiedDate" runat="server"
																														CssClass="clsLabelAuto"
																														Text='<%#IIf(AppSettings("ClientCode") = "7AR",
																																		  "Rectification Date (UTC)",
																																		  "Rectification Date") %>' />
																												</td>
																												<td>
																													<asp:UpdatePanel ID="upnlRectifiedDate" runat="server" UpdateMode="Conditional">
																														<ContentTemplate>
																															<asp:TextBox ID="txtRectifiedDate" runat="server"
																																CssClass="clsTextBoxTagSearch" Width="100px"
																																AutoPostBack="false"
																																onchange="ValidateDateText(this,'txtRectifiedDate_watermarkextender');" />
																															<cc2:CalendarExtender ID="CalExt_txtRectifiedDate"
																																runat="server" Enabled="True"
																																TargetControlID="txtRectifiedDate"
																																CssClass="cal_Theme1" Format="<%$AppSettings:DateFormat%>" />
																															<cc2:TextBoxWatermarkExtender TargetControlID="txtRectifiedDate"
																																ID="txtRectifiedDate_watermarkextender"
																																ClientIDMode="Static" runat="server"
																																WatermarkText="<%$AppSettings:DateFormat%>" />
																														</ContentTemplate>
																													</asp:UpdatePanel>
																												</td>
																												<td>
																													<asp:Label ID="lblRectifiedLogNo" runat="server"
																														CssClass="clsLabelAuto"
																														Text="Rectification Log No." />
																												</td>
																												<td>
																													<asp:UpdatePanel ID="upnlRectifiedCombo" runat="server"
																														UpdateMode="Conditional">
																														<ContentTemplate>
																															<asp:DropDownList ID="cmbRectifiedLogNo"
																																runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																																AutoPostBack="False" DataValueField="LogID"
																																DataTextField="LogNoLogPageNo" Enabled="False" />
																														</ContentTemplate>
																													</asp:UpdatePanel>
																												</td>
																											</tr>
																											<asp:PlaceHolder ID="phWatchListDetails" runat="server">
																												<tr>
																													<td>
																														<asp:Label ID="Label16" runat="server"
																															CssClass="clsLabelAuto"
																															Text="Add to Watchlist" />
																													</td>
																													<td>
																														<asp:CheckBox ID="chkAddtoWatchList" runat="server"
																															CssClass="clsCheckBox"
																															Enabled="<%#Not DiscrepancyCorrectiveAction.ConsideredInWatchList %>"
																															TextAlign="Left" Text=""
																															Checked="<%# DiscrepancyCorrectiveAction.AddToWatchList %>" />
																													</td>
																													<td>
																														<asp:Label ID="lblWatchListInstruction" runat="server"
																															CssClass="clsLabelAuto"
																															Text="Watchlist Instructions" />
																													</td>
																													<td>
																														<asp:TextBox ID="txtPreventiveMeasures"
																															runat="server"
																															CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																															Enabled="<%#Not DiscrepancyCorrectiveAction.ConsideredInWatchList %>"
																															TextMode="MultiLine" Width="230px"
																															Text="<%# DiscrepancyCorrectiveAction.PreventionTaken %>"
																															ToolTip="Enter Watchlist Instructions"
																															MaxLength="50" />
																													</td>
																												</tr>
																											</asp:PlaceHolder>
																										</asp:PlaceHolder>
                                                                                                        <asp:PlaceHolder ID="phFreq" runat="server">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="Label14" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtMELDescription" runat="server" 
                                                                                                                        CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                                                                        TextMode="MultiLine" Width="230px" ReadOnly="true"
                                                                                                                        ToolTip="Description" MaxLength="1000" />
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                                <td>
                                                                                                                    <table width="70%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <asp:Label ID="lblMELCategory" runat="server" 
                                                                                                                                    CssClass="clsLabelAuto" Visible="false" Text="Category" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <asp:UpdatePanel ID="upnlMELCategory" runat="server" UpdateMode="Conditional">
                                                                                                                                    <ContentTemplate>
                                                                                                                                        <asp:DropDownList ID="cmbMELCategory" runat="server" 
                                                                                                                                            CssClass="clsTextBoxTagSearchComboSmall1" Visible="false"
                                                                                                                                            DataValueField="ID" DataTextField="Name" 
                                                                                                                                            AutoPostBack="True" 
																																			SelectedValue="<%# DiscrepancyCorrectiveAction.MELCategoryID %>">
                                                                                                                                        </asp:DropDownList>
                                                                                                                                    </ContentTemplate>
                                                                                                                                </asp:UpdatePanel>
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <span id="lblItemSequenceNo" class="clsLabelAuto">Item Sequence No.</span>
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <asp:UpdatePanel ID="upnlItemSequenceNo" runat="server" UpdateMode="Conditional">
                                                                                                                                    <ContentTemplate>
                                                                                                                                        <asp:TextBox ID="txtItemSequenceNo" runat="server" 
                                                                                                                                            CssClass="clsTextBoxTagSearch" Enabled="False" 
                                                                                                                                            Text="<%# DiscrepancyCorrectiveAction.ItemSequenceNo %>"
                                                                                                                                            ToolTip="Item Sequence No." />
                                                                                                                                    </ContentTemplate>
                                                                                                                                </asp:UpdatePanel>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="4">
                                                                                                                    <asp:UpdatePanel ID="upnlFreq" runat="server" UpdateMode="Conditional">
                                                                                                                        <ContentTemplate>
                                                                                                                            <table width="100%" align="top">
                                                                                                                                <tr>
                                                                                                                                    <td align="top">
                                                                                                                                        <asp:Label ID="lblFrequency" runat="server" 
                                                                                                                                            Width="70px" CssClass="clsLabelAuto">Frequency</asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtFrequencyInDay" runat="server"
                                                                                                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="100px"
                                                                                                                                            Text="<%# DiscrepancyCorrectiveAction.FrequencyInDays %>"
                                                                                                                                            ToolTip="Enter Frequency In Days"
                                                                                                                                            AutoPostBack="True" MaxLength="4" Enabled="False" />
                                                                                                                                        <asp:Label ID="lblDays" runat="server" 
																																			CssClass="clsLabelAuto">In Days</asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtFrequencyInHours" runat="server"
                                                                                                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="100px"
                                                                                                                                            Text="<%# DiscrepancyCorrectiveAction.FrequencyInHours %>"
                                                                                                                                            ToolTip="Enter Frequency In Hours"
                                                                                                                                            AutoPostBack="True" MaxLength="5" Enabled="False" />
                                                                                                                                        <asp:Label ID="lblHours" runat="server" 
																																			CssClass="clsLabelAuto">Hours</asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtCycles" runat="server"
                                                                                                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                                                            Width="100px" ReadOnly="true"
                                                                                                                                            Text="<%# DiscrepancyCorrectiveAction.FrequencyInCycles %>"
                                                                                                                                            ToolTip="Enter Frequency In Cycles"
                                                                                                                                            AutoPostBack="True" MaxLength="5" Enabled="False" />
                                                                                                                                        <asp:Label ID="lblCycles" runat="server" 
																																			CssClass="clsLabelAuto">Cycles</asp:Label>

                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="4">
                                                                                                                                        <asp:PlaceHolder ID="phextension" runat="server">
                                                                                                                                            <fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
                                                                                                                                                <legend>
                                                                                                                                                    <b>
                                                                                                                                                        <asp:Label
                                                                                                                                                            Text="Extension"
                                                                                                                                                            runat="server" ID="lblExtensionApplied"> 
                                                                                                                                                        </asp:Label>
                                                                                                                                                        <asp:CheckBox ID="chkExtensionApplied" runat="server"
                                                                                                                                                            CssClass="clsCheckBox"
                                                                                                                                                            Checked="<%# DiscrepancyCorrectiveAction.ExtensionApplied %>"
                                                                                                                                                            AutoPostBack="True" Enabled="False" />
                                                                                                                                                    </b>
                                                                                                                                                </legend>
                                                                                                                                                <table width="100%">
                                                                                                                                                    <tr>
                                                                                                                                                        <td align="top">
                                                                                                                                                            <asp:Label ID="Label2" runat="server" Width="50px" 
                                                                                                                                                                CssClass="clsLabelAuto" />
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:UpdatePanel ID="upnlExtension" runat="server" 
                                                                                                                                                                UpdateMode="Conditional">
                                                                                                                                                                <ContentTemplate>

                                                                                                                                                                    <asp:TextBox ID="txtExtensionInDays" runat="server" 
                                                                                                                                                                        CssClass="clsTextBoxTagSearchRightAlign1" Width="100px"
                                                                                                                                                                        Text="<%# DiscrepancyCorrectiveAction.ExtensionInDays %>" 
                                                                                                                                                                        ToolTip="Enter Frequency In Days"
                                                                                                                                                                        AutoPostBack="True" MaxLength="4" Enabled="False" />
                                                                                                                                                                    <span id="lblInDays" class="clsLabelAuto">In Days</span>
                                                                                                                                                                </ContentTemplate>
                                                                                                                                                            </asp:UpdatePanel>
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:TextBox ID="txtExtensionInHours" runat="server" 
                                                                                                                                                                CssClass="clsTextBoxTagSearchRightAlign1" Width="100px"
                                                                                                                                                                Text="<%# DiscrepancyCorrectiveAction.ExtensionInHours %>" 
                                                                                                                                                                ToolTip="Enter Frequency In Hours"
                                                                                                                                                                AutoPostBack="True" MaxLength="6" Enabled="False" />
                                                                                                                                                            <span id="lblInHours" class="clsLabelAuto">Hours</span>
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:TextBox ID="txtExtensionInCycles" runat="server"
                                                                                                                                                                CssClass="clsTextBoxTagSearchRightAlign1" Width="100px"
                                                                                                                                                                Text="<%# DiscrepancyCorrectiveAction.ExtensionInCycles %>" 
                                                                                                                                                                ToolTip="Enter Frequency In Cycles"
                                                                                                                                                                AutoPostBack="True" MaxLength="4" Enabled="False" />
                                                                                                                                                            <span id="lblInCycles" class="clsLabelAuto">Cycles</span>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td colspan="4">
                                                                                                                                                            <table>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:Label ID="lblExtensionApprovalReq" 
                                                                                                                                                                            runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <span id="lblExtensionApprovalNo" runat="server" 
                                                                                                                                                                            cssclass="clsLabelAuto" align="right">
                                                                                                                                                                            Approval Details
                                                                                                                                                                            </span>
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:TextBox ID="txtExtensionApprovalNo" 
                                                                                                                                                                            runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" 
                                                                                                                                                                            TextMode="MultiLine" Width="230px"
                                                                                                                                                                            Text="<%# DiscrepancyCorrectiveAction.ExtensionApprovalNo %>" 
                                                                                                                                                                            ToolTip="Enter Extension Approval No"
                                                                                                                                                                            MaxLength="100" Enabled="False" />
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                            </table>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </fieldset>
                                                                                                                                        </asp:PlaceHolder>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="lblDueDate" runat="server"
                                                                                                                                            CssClass="clsLabelAuto" Width="168px" 
                                                                                                                                            Style="text-align: center"
                                                                                                                                            Text='<%#IIf(AppSettings("ClientCode") = "7AR", "Due Date (UTC)", "Due Date") %>'>
                                                                                                                                            Due Date
                                                                                                                                        </asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelAuto" Width="168px"
                                                                                                                                            Style="text-align: center">Due Hours</asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto" Width="168px"
                                                                                                                                            Style="text-align: center">Due Cycles</asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                    <td>
                                                                                                                                        <asp:UpdatePanel ID="upnlDueDate" runat="server" UpdateMode="Conditional">
                                                                                                                                            <ContentTemplate>
                                                                                                                                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                                                                                                    ToolTip="Due Date" Width="168px" Height="35px" BackColor="YellowGreen"
                                                                                                                                                    AutoPostBack="true" ForeColor="White"
                                                                                                                                                    Style="font-size: large; text-align: center; 
                                                                                                                                                        border-top-left-radius: 12px; 
                                                                                                                                                        border-top-right-radius: 12px; 
                                                                                                                                                        border-bottom-left-radius: 12px; 
                                                                                                                                                        border-bottom-right-radius: 12px;"
                                                                                                                                                    onchange="ValidateDateText(this,'txtDueDate_watermarkextender');" />
                                                                                                                                                <cc2:CalendarExtender ID="CalExt_txtDueDate" runat="server" Enabled="True"
                                                                                                                                                    TargetControlID="txtDueDate" CssClass="cal_Theme1"
                                                                                                                                                    Format="<%$AppSettings:DateFormat%>" />
                                                                                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDueDate"
                                                                                                                                                    ID="txtDueDate_watermarkextender"
                                                                                                                                                    ClientIDMode="Static" runat="server"
                                                                                                                                                    WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                                                                                    WatermarkCssClass="clsDateTextBox" />
                                                                                                                                            </ContentTemplate>
                                                                                                                                        </asp:UpdatePanel>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:UpdatePanel ID="upnlDueHrs" runat="server" UpdateMode="Conditional">
                                                                                                                                            <ContentTemplate>
                                                                                                                                                <asp:TextBox ID="txtDueHrs" runat="server"
                                                                                                                                                    CssClass="clsTextBoxTagSearchRightAlign1"
                                                                                                                                                    Width="168px" Height="35px" BackColor="#ff6699"
                                                                                                                                                    ForeColor="White" Style="font-size: large; text-align: center; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-left-radius: 12px; border-bottom-right-radius: 12px;"
                                                                                                                                                    Text="<%# DiscrepancyCorrectiveAction.DueInHrs %>" ToolTip="Due Hours"
                                                                                                                                                    MaxLength="10" Enabled="False" />
                                                                                                                                            </ContentTemplate>
                                                                                                                                        </asp:UpdatePanel>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:UpdatePanel ID="upnlDueCycles" runat="server" UpdateMode="Conditional">
                                                                                                                                            <ContentTemplate>
                                                                                                                                                <asp:TextBox ID="txtDueCycles" runat="server"
                                                                                                                                                    CssClass="clsTextBoxTagSearchRightAlign1" Width="168px" Height="35px"
                                                                                                                                                    BackColor="#3399ff" ForeColor="White" 
																																					Style="font-size: large; text-align: center; 
																																						   border-top-left-radius: 12px; 
																																						   border-top-right-radius: 12px; 
																																						   border-bottom-left-radius: 12px; 
																																						   border-bottom-right-radius: 12px;"
                                                                                                                                                    Text="<%# DiscrepancyCorrectiveAction.DueInCycles %>" ToolTip="Due Cycles"
                                                                                                                                                    MaxLength="10" Enabled="False" />
                                                                                                                                            </ContentTemplate>
                                                                                                                                        </asp:UpdatePanel>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </ContentTemplate>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </asp:PlaceHolder>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ContentTemplate>
                                                                </cc2:TabPanel>
																<cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1"
																	ClientIDMode="Static" Visible="false">
																	<HeaderTemplate>
																		<asp:Label runat="server" Text="Rectification Details" ID="Label11" />
																	</HeaderTemplate>
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle">
																			<table width="40%">
																				<tr>
																					<asp:PlaceHolder ID="hide3" runat="server" Visible="false">
																						<td>
																							<asp:UpdatePanel ID="upnlClose" runat="server"
																								UpdateMode="Conditional">
																								<ContentTemplate>
																									<asp:CheckBox ID="chkClose" runat="server"
																										CssClass="clsCheckBox" Text=""
																										Checked="<%# DiscrepancyCorrectiveAction.InvestigationStatus %>"
																										AutoPostBack="True" />
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</td>
																						<td>
																							<asp:Label ID="lblStatus" runat="server"
																								CssClass="clsLabelAuto"
																								Text="Investigation Status (Closed)" />
																						</td>
																					</asp:PlaceHolder>
																				</tr>
																			</table>
																			<asp:PlaceHolder runat="server" ID="plhRectification">
																				<table style="margin-top: -5px">
																					<tr>
																						<td></td>
																						<td></td>
																						<td>
																							<asp:Label Width="100px" ID="Label1" runat="server"
																								CssClass="clsLabelAuto" Text="Cause of defect" />
																						</td>
																						<td>
																							<asp:TextBox ID="txtCauseofDefect" runat="server"
																								CssClass="clsTextBoxTagSearchLong1"
																								Width="275px"
																								Text="<%# DiscrepancyCorrectiveAction.CauseOfDefect %>"
																								ToolTip="Enter causes of Defect"
																								MaxLength="1000" TextMode="MultiLine" />
																						</td>
																						<td></td>
																						<td>
																							<asp:Label ID="lblPreventiveMeasuresTaken"
																								runat="server" CssClass="clsLabelAuto"
																								DESIGNTIMEDRAGDROP="872" Width="120px"
																								Text="Preventive Measures Taken" />
																						</td>
																						<td>
																							<asp:TextBox ID="txtPreventiveMeasuresTaken"
																								runat="server" CssClass="clsTextBoxTagSearchLong1"
																								Width="275px"
																								Text="<%# DiscrepancyCorrectiveAction.PreventionTaken %>"
																								ToolTip="Enter preventive measures to be taken"
																								MaxLength="1000" TextMode="MultiLine" />
																						</td>
																					</tr>
																					<tr>

																						<td></td>
																						<td colspan="2">
																							<asp:Label Width="145px" ID="Label3" runat="server"
																								CssClass="clsLabelAuto"
																								Text="Action taken against eng. staff" />
																						</td>
																						<td>
																							<asp:TextBox ID="txtActionTakenAganistEngStaff"
																								runat="server" CssClass="clsTextBoxTagSearch1"
																								Text="<%# DiscrepancyCorrectiveAction.ActionAgainstStaff %>"
																								ToolTip="Enter actions against Staff"
																								MaxLength="50" />
																						</td>
																						<td></td>
																						<td>
																							<asp:Label ID="lblRectificationSector" runat="server"
																								CssClass="clsLabelAuto" Text="Sector / Place" />
																						</td>
																						<td>
																							<asp:TextBox ID="txtRectificationSector" runat="server"
																								CssClass="clsTextBoxTagSearch1" Enabled="false"
																								Text="<%# DiscrepancyCorrectiveAction.RectifiedStation %>"
																								ToolTip="Enter Sector/Place"
																								MaxLength="50" />
																						</td>
																					</tr>
																					<tr>
																						<td></td>
																						<td colspan="2">
																							<asp:Label ID="lblRemark" runat="server"
																								CssClass="clsLabelAuto" Text="Remark" />
																						</td>
																						<td colspan="4">
																							<asp:TextBox ID="txtRemark" runat="server"
																								CssClass="clsTextBoxTagSearchLong"
																								Text="<%# DiscrepancyCorrectiveAction.Remark %>"
																								ToolTip="Enter Remark" MaxLength="500"
																								Height="46px" Width="40%" />
																						</td>
																					</tr>
																				</table>
																			</asp:PlaceHolder>
																		</fieldset>
																	</ContentTemplate>
																</cc2:TabPanel>
                                                                <cc2:TabPanel ID="tabFileAttachment" runat="server" 
																	ClientIDMode="Static" CssClass="clsPanel1">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" Text="File Attachment" ID="lblFileAttachment" />
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <fieldset class="clsFieldSetNewStyle">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label runat="server" ID="lblAttachFile" 
																												class="clsLabelAuto" Text="Attach File" />
                                                                                                        </td>
																										<td style="padding-left: 50px;">
                                                                                                            <input type="button" id="btnSelectFile" 
																												value="Select File" style="width: 115px;"
                                                                                                                class="clsbtnH clsinfoH1" />
                                                                                                        </td>
																										<td style="padding-left: 10px;">
                                                                                                            <asp:Button ID="btnDelAttach" 
																												runat="server" CssClass="clsbtnH clsinfoH1" 
																												ToolTip="Remove the Attachment added."
                                                                                                                Text="Remove Attachment"
																												Enabled="False" Width="140px" />
                                                                                                        </td>
																										<td style="padding-left: 2px;">
                                                                                                            <asp:ImageButton ID="attachmentICN" runat="server"
																												CausesValidation="False" CssClass="FileAttachmentICN"
																												ImageUrl="icons/CLIP01.ICO" Visible="false" 
																												ToolTip="View / Download the Attachment. "/>
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
                                                                </cc2:TabPanel>
                                                            </cc2:TabContainer>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="tblButton">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" 
																					ID="UpdatePanel1">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="hdnBtnFileUpload" 
																							ClientIDMode="Static" runat="server" Text="Add"
                                                                                            CausesValidation="False" Style="display: none;" />
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" 
																					ID="UpdatePanel2">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="hdnBtnSelectLog" 
																							ClientIDMode="Static"
																							runat="server" Text="Add"
                                                                                            CausesValidation="False" 
																							Style="display: none;" />
                                                                                        <asp:Button ID="hdnImgBtnSendMail" 
																							ClientIDMode="Static" 
																							runat="server" Text="----"
                                                                                            CausesValidation="False" 
																							Style="display: none;" />
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel3">
                                                                <ContentTemplate>
                                                                    <td>
                                                                        <asp:Button ID="hdnBtnMaintDoneBy" 
																			ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;" />
                                                                        <asp:Button ID="hdnimgBtnMELMasterChapter" 
																			ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;" />
                                                                        <asp:Button ID="hdnBtnMELDetail" ClientIDMode="Static" 
																			runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;" />
                                                                        <asp:Button ID="hdnimgBtnCDLMasterChapter" 
																			ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;" />
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

            <script type="text/javascript">

                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

					$("#<%=txtReportedBy.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
                        width: 278,
                        autoFill: false,
                        matchContains: true,
                        max: 30,
                        delay: 0
                    });

                    $("#<%=txtActionTakenAganistEngStaff.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
                        width: 278,
                        autoFill: false,
                        mustMatch: false,
                        matchContains: true,
                        max: 30,
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
                        $(elem).removeClass('ac_loading');
                        $(elem).val(result);
                    }

                    function onError(result) {
                        $(elem).removeClass('ac_loading');
                        $(elem).val('');
					}

                    function OnBeforeSend() {
                        $(elem).addClass('ac_loading');
					}

				}

            </script>

        </div>

        <div>

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

            <!-- Select LogInfo popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummySelectLog" Text="Maintenance Activity" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupSelectLog" runat="server" TargetControlID="btnDummySelectLog"
                PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameSelectLogStateComplete() {
                    $("#btnDummySelectLog").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenSelectLogWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeSelectLog").attr("src", "wfMELSnagCorrectiveActionLogInfo_AJAX.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummySelectLog").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

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

            <!-- Deferred Detail Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyDeferredDetail" Text="Deferred Detail" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlDeferredDetail" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeDeferredDetail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupDeferredDetail" runat="server" TargetControlID="btnDummyDeferredDetail"
                PopupControlID="pnlDeferredDetail" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameDeferredDetailStateComplete() {
                    $("#btnDummyDeferredDetail").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenDeferredDetail() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeDeferredDetail").attr("src", "wfDeferredDetail_Ajax.aspx?Type=pup&OpenFrom=Discrepancy");

                        if (!$.browser.msie) {
                            $("#btnDummyDeferredDetail").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForDeferredDetail() {
                    var DeferredDetailwindow = $find("<%=mdlPopupDeferredDetail.ClientID %>");
                    //close Deferred Detail popup window
                    DeferredDetailwindow.hide();
                    //           release resources
                    $("#IframeDeferredDetail").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnDeferredDetail").click();
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

            <!-- CDLMaster Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyCDLMaster" Text="Dummy CDLMaster" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlPopupCDLMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="iPopupCDLMaster" frameborder="0" allowtransparency="true" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupCDLMaster" runat="server" TargetControlID="btnDummyCDLMaster"
                PopupControlID="pnlPopupCDLMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameCDLMasterStateComplete() {
                    $("#btnDummyCDLMaster").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                function OpenCDLMasterWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#iPopupCDLMaster").attr("src", "wfDeferredListForSelection_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyCDLMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
            </script>
            <script type="text/javascript">
                function ParentCallBackCDLFunction() {
                    var CDLMasterwindow = $find("<%=mdlPopupCDLMaster.ClientID %>");
                    //close CDLMaster popup window
                    CDLMasterwindow.hide();
                    $("#iPopupCDLMaster").attr("src", "JavaScript:''");
                    //call CDLMaster image button
                    $("#hdnimgBtnCDLMasterChapter").click();
                }
            </script>

            <!-- Send Email Modal PopUp-->
            <%--Added By Harsh on 20th Feb 2024--%>
            <div style="display: none">
                <asp:HiddenField runat="server" ID="hdnBtnSendMail" />
            </div>
            <asp:Panel runat="server" ID="pnlSendMail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="ISendMail" allowtransparency="true" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupSendMail" runat="server" TargetControlID="hdnBtnSendMail"
                PopupControlID="pnlSendMail" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameSendMailComplete() {
                    $("#hdnBtnSendMail").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                $(document).ready(function () {
                    $("#btnSendMail").live("click", function () {
                        try {
                            $get("AjaxLoader").style.visibility = 'visible';
                            $("#ISendMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                            if (!$.browser.msie) {
                                $("#hdnBtnSendMail").click();
                                $get("AjaxLoader").style.visibility = 'hidden';
                            }
                            return false;
                        } catch (e) {
                            alert(e);
                        }
                    });
                });

                function ParentCallBackFunctionForSendMail() {
                    var sendEmailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
                    sendEmailWindow.hide();
                    $("#ISendMail").attr("src", "JavaScript:''");
                }

                function ParentCallBackFunctionToSendMail() {
                    var sendEmailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
                    sendEmailWindow.hide();
                    $("#IframeReceipt").attr("src", "JavaScript:''");
                    $("#hdnImgBtnSendMail").click();
                }
            </script>
            <!-- End -->

        </div>

		<script type="text/javascript">

            <% Dim Open As String = Request.QueryString("Type") %>
            <% If Open IsNot Nothing AndAlso Open = "pup" Then %>  

                $(document).ready(function () {

					console.log('calling SetPageLayout from document ready of Discrepancy Detail');

                    SetPageLayout();

					if ($.browser.msie) {

						try {

							<% Dim TransTypeID As Integer = Request.QueryString("TransTypeID") %>

							<% IF TransTypeID = 116 Then %>
								parent.IFrameCabinDefectDetailComplete();
							<% Else %>
								parent.IFrameDiscrepancyDetailComplete();
							<% End if %>

						} catch (e) {
							console.error("Error ocuured while calling IFrame complete function of parent from Deiscrepancy Detail page. Refer the Error " + e);
							alert(e);
						}

                    }

                });

            <% End if %>

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

			function endRequestHandler() {

					try {

						console.log('calling SetPageLayout from Page Load of Discrepancy Detail');
						SetPageLayout();

					} catch (e) {
						console.error("Error ocuured in Page Load of Discrepancy Detail Window. Refer the Error " + e);
						alert(e);
					}

				}

			function SetPageLayout() {

					console.log('SetPageLayout() started');

					try {

						<% Dim OpenAs As String = Request.QueryString("Type") %>

						<% If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then %>  

							console.log('calling ReSetPageLayout() from SetPageLayout() of Discrepancy Detail');
							ReSetPageLayout();
							onResize();

						<% End if %>

						console.log('SetPageLayout() ended');

					} catch (ex) {
						console.error('SetPageLayout() exception' + ex);
					}


				}

			function ReSetPageLayout() {

					console.log('ReSetPageLayout() started');

					try {

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

						console.log('ReSetPageLayout() ended');

					} catch (ex) {
						console.error('ReSetPageLayout() exception' + ex);
					}

				}

		</script>

        <script type="text/javascript">

			function CallParentCallback() {

				try {

					console.log("CallParentCallback() called");

					parent.ParentCallBackFunctionForDiscrepancyDetail();
					return false;

				} catch (e) {
					console.error("Error ocuured in CallParentCallback(). Refer the Error " + e);
					alert(e);
				}

			}

			function CallParentCallbackForCabinDefect() {

				try {

					console.log("CallParentCallbackForCabinDefect() called");

					parent.ParentCallBackForCabinDefectDetail();
					return false;

				} catch (e) {
					console.error("Error ocuured in CallParentCallbackForCabinDefect(). Refer the Error " + e);
					alert(e);
				}

			}

		</script>

        <script type="text/javascript">

			function slidePanel(div) {

				if ($('#' + div).css('display') == 'none') {
                    $('#' + div).slideDown('medium', function () { });
                } else {
                    $('#' + div).slideUp('medium', function () { });
				}

			}

        </script>

        <script type="text/javascript">

            <% Dim IsOpenFrom As String = Request.QueryString("OpenFromWatchDiscrepanciesLink") %>

            <% If (Not IsOpenFrom Is Nothing AndAlso IsOpenFrom = "WatchDiscrepanciesLink") Then %>

                    $(document).ready(function () {
                        $(':input').not('#btnBack').attr('disabled', true);
                        _href = $('#lnkDeferredDetail').attr('href');
                        $('#lnkDeferredDetail').attr('disabled', 'disabled');
                        $('#lnkDeferredDetail').removeAttr('href');
                        $("#lnkMELDetail").css("visibility", "hidden");
                        $("#lnkDeferredDetail").css("visibility", "hidden");
                        $("#lnkCheckStatus").css("visibility", "hidden");
                        $("#btnSave").css("visibility", "hidden");
                        $("#btnSendMail").css("visibility", "hidden");
                        $("#btnPrint").css("visibility", "hidden");
                        $("#lnkMEL").css("visibility", "hidden");
                        $("#lnkDeferredList").css("visibility", "hidden");
                        $('#attachmentICN').attr('disabled', false);
                        $('#btnDelAttach').attr('disabled', true);
                    });

                <% End If %>

            $(document).ready(function () {
                disableControlsOnClose();
            });

			function disableControlsOnClose() {

				try {

					console.log("disableControlsOnClose() called");

					var selectedValue = $('#cmbInvestigation option:selected').val();
					var selectedText = $('#cmbInvestigation option:selected').text();

					if (selectedText == "Closed") {

						$(':input').not('#btnBack').attr('disabled', true);
						_href = $('#lnkDeferredDetail').attr('href');
						$('#lnkDeferredDetail').attr('disabled', 'disabled');
						$('#lnkDeferredDetail').removeAttr('href');
						$("#lnkMELDetail").css("visibility", "hidden");
						$("#lnkDeferredDetail").css("visibility", "hidden");
						$("#lnkCheckStatus").css("visibility", "hidden");
						$("#btnSave").css("visibility", "hidden");
						$("#btnSendMail").css("visibility", "hidden");
						$("#btnPrint").css("visibility", "hidden");
						$("#lnkMEL").css("visibility", "hidden");
						$("#lnkDeferredList").css("visibility", "hidden");
						$('#attachmentICN').attr('disabled', false);
						$('#btnDelAttach').attr('disabled', true);

					}

				} catch (e) {
					console.error("Error ocuured in disableControlsOnClose(). Refer the Error " + e);
					alert(e);
				}

			};

		</script>

    </form>
</body>
</html>

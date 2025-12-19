<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskFinding_Ajax.aspx.vb"
	Inherits="Flypal.wfTaskFinding_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Audit Finding Detail</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" type="text/javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<table class="clstablelistin" id="tblInner">
						<tr>
							<td class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server"> Finding Details</asp:Label>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td align="right">
											<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td>
																<asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Findings"
																	ValidationGroup="1"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" ToolTip="Click to Print Finding"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close the Finding Details Screen"
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
								<asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
											HeaderText="Fill Up The Following Information" ValidationGroup="1"></asp:ValidationSummary>
										<asp:RequiredFieldValidator ID="rfvFindingNo" runat="server" CssClass="clslabelauto"
											Display="None" ErrorMessage="Enter Finding No." ControlToValidate="txtFindingNo"
											ValidationGroup="1"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator ID="rfvFinding" runat="server" CssClass="clslabelauto"
											Display="None" ErrorMessage="Enter Finding" ControlToValidate="txtFinding" ValidationGroup="1"></asp:RequiredFieldValidator>										
										<asp:CustomValidator ID="cvRemark" runat="server" CssClass="clslabelauto" ControlToValidate="txtRemark"
											ErrorMessage="Remark should not be greater than 1000 characters." Display="None"
											ClientValidationFunction="validateNameLength" ValidationGroup="1"></asp:CustomValidator>
										<asp:CustomValidator ID="cvToMailID" runat="server" Display="None" ErrorMessage="Please Enter Valid To Email-ID"
											ControlToValidate="txtToMailID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"
											ValidationGroup="1"></asp:CustomValidator>
										<asp:CustomValidator ID="cvCCMailID" runat="server" Display="None" ErrorMessage="Please Enter Valid CC Email-ID"
											ControlToValidate="txtCCMailID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"
											ValidationGroup="1"></asp:CustomValidator>
										<asp:CustomValidator ID="cvER" runat="server" CssClass="clslabelauto" ControlToValidate="txtExtensionRemark"
											ErrorMessage="Extension Remark should not be greater than 1000 characters." Display="None"
											ClientValidationFunction="validateNameLength" ValidationGroup="1"></asp:CustomValidator>
										<asp:CustomValidator ID="cvEx" runat="server" ControlToValidate="txtExtensionInDays"
											ErrorMessage="Extension days should be greater than zero" OnServerValidate="customvalidate"
											Display="None" CssClass="clslabelauto" ValidationGroup="1"></asp:CustomValidator>
										<script type="text/javascript">

											function validateEmail(field) {
												var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
												return (regex.test(field)) ? true : false;
											}
											function validateMultipleEmailsCommaSeparated(source, args) {
												var text = $get(source.controltovalidate).value;
												var seperator = ',';
												if (text != '') {
													var result = text.split(seperator);
													for (var i = 0; i < result.length; i++) {
														if (result[i] != '') {
															if (!validateEmail(result[i].trim())) {
																args.IsValid = false;
																return;
															}
														}
													}
												}
											}

											function validateNameLength(source, args) {
												var control = source.controltovalidate;
												switch (control) {
													case 'txtExtensionRemark':
														var nameLength = $get(source.controltovalidate).value.length;
														if (nameLength > 1000) {
															args.isvalid = false;
															return;
														}
														break;
													case 'txtRemark':
														var nameLength = $get(source.controltovalidate).value.length;
														if (nameLength > 1000) {
															args.isvalid = false;
															return;
														}
														break;
												}
											}
										</script>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlFindingDetails" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<fieldset id="fdsFindingDetails" class="clsFieldSet" style="border-width: 1px">
											<legend id="ldFindingDetails"><b>Finding Details</b></legend>
											<table>
												<tr>
													<td>
														<span id="lblChargeNameStar1" class="clsLabelStar">*</span>
													</td>
													<td>
														<span id="lblFindingNo" class="clsLabelAuto">Finding No.</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtFindingNo" runat="server" ToolTip="Enter Finding No."
															MaxLength="100" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo %>">
														</asp:TextBox>
													</td>
													<td>
														<span id="lblReference" class="clsLabelAuto">Reference No.</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReference" runat="server" ToolTip="Enter Reference No."
															MaxLength="200" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Reference %>"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>
														<span id="Label4" class="clsLabelStar">*</span>
													</td>
													<td>
														<span id="lblFinding" class="clsLabelAuto">Finding</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtFinding" runat="server"
															ToolTip="Enter Finding" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Finding %>"
															TextMode="MultiLine" Height="34px"></asp:TextBox>
													</td>
													<td>
														<span id="lblRootCause" class="clsLabelAuto">Root Cause</span>
													</td>
													<td>
														<table>
															<tr>
																<td>
																	<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbRootCause" runat="server" DataTextField="RootCause"
																		onchange="chkind()" DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.RootCauseID %>">
																	</asp:DropDownList>
																</td>
															</tr>
															<tr>
																<td>
																	<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtRootCause" runat="server"
																		ToolTip="Enter Root Cause" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.RootCause %>"
																		TextMode="MultiLine"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblPriority" runat="server" class="clsLabelAuto">Priority</span>
													</td>
													<td>
														<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPriority" runat="server" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.PriorityID %>"
															DataValueField="ID" DataTextField="NameWithDays" AutoPostBack="false">
														</asp:DropDownList>
													</td>
													<td>
														<span id="lblFindingStatus" class="clsLabelAuto">Finding Status</span>
													</td>
													<td>
														<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFindingStatus" runat="server"
															DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingStatusID %>">
														</asp:DropDownList>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblDeadlineDate" class="clsLabelAuto">Deadline Date</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagDateSearch" ID="txtDeadlineDate" runat="server" ClientIDMode="Static"
															onchange="ValidateDateText(this,'DeadlineDate_watermarkextender','false');" Width="100px"></asp:TextBox>
														<cc2:CalendarExtender ID="DeadlineDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDeadlineDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender ID="DeadlineDate_watermarkextender" runat="server"
															TargetControlID="txtDeadlineDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
													</td>
													<td>
														<span id="lblLocation" runat="server" class="clsLabelAuto">Location</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtLocation" runat="server" MaxLength="200"
															Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Location %>"
															ToolTip="Enter Location"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<span id="lblExtensionApplied" class="clsLabelAuto">Extension</span>
													</td>
													<td>
														<asp:UpdatePanel ID="upnlExtension" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:CheckBox ID="chkExtensionApplied" runat="server" CssClass="clsCheckBox" Checked="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ExtensionApplied %>"
																	AutoPostBack="True"></asp:CheckBox>
																<asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtExtensionInDays" runat="server"
																	Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ExtensioninDays %>"
																	ToolTip="Enter Frequency In Days" AutoPostBack="True" MaxLength="4"></asp:TextBox>
																<span id="lblInDays" class="clsLabelAuto">In Days</span>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td>
														<span id="Span1" class="clsLabelAuto">Extension Remark</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtExtensionRemark" runat="server"
															Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ExtensionRemark %>"
															ToolTip="Enter Extension Remark" MaxLength="1000" BackColor="White" TextMode="MultiLine"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<span id="lblAuditCategory" runat="server" class="clsLabelAuto">Evidence</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCategory" runat="server" BackColor="White"
															MaxLength="100" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Category %>"
															ToolTip="Enter Evidence"></asp:TextBox>
													</td>
													<td>
														<span id="lblKindAttention" class="clsLabelAuto">Responsible Person</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtKindAttention" runat="server" BackColor="White"
															MaxLength="200" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.KindAttention %>"
															ToolTip="Enter Responsible Person"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblCorrectiveAction" class="clsLabelAuto">Corrective Action</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtCorrectiveAction" runat="server"
															Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CAPA %>"
															TextMode="MultiLine" ToolTip="Enter Corrective action"></asp:TextBox>
													</td>
													<td>
														<span id="Preventive" class="clsLabelAuto">Preventive Action</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtPreventiveAction" runat="server"
															Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Preventive %>"
															TextMode="MultiLine" ToolTip="Enter Preventive action"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td></td>
													<td></td>
													<td colspan="3">
														<span id="lblInfo" class="clsLabelHeader">Please enter valid comma separated Email ID's
                                                        in To And CC.</span>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblTo" class="clsLabelAuto">To</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtToMailID" runat="server"
															ClientIDMode="Static" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ToMailID %>"
															ToolTip="Enter To Email ID's" MaxLength="500" TextMode="MultiLine">
														</asp:TextBox>
													</td>
													<td>
														<span id="lblCC" class="clsLabelAuto">CC</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtCCMailID" runat="server"
															ClientIDMode="Static" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CCMailID %>"
															ToolTip="Enter CC Email ID's" MaxLength="500" TextMode="MultiLine">
														</asp:TextBox>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblCorrectionDate" class="clsLabelAuto">Correction Date</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagDateSearch" ID="txtCorrectionDate" runat="server" ClientIDMode="Static"
															onchange="ValidateDateText(this,'CorrectionDate_watermarkextender','false');"
															Text="" Width="100px"></asp:TextBox>
														<cc2:CalendarExtender ID="CorrectionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCorrectionDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender ID="CorrectionDate_watermarkextender" runat="server"
															TargetControlID="txtCorrectionDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
													</td>
													<td>
													</td>
													<td>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span id="lblRemark" class="clsLabelAuto">Auditors Remark</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtRemark" runat="server" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Remark %>"
															ToolTip="Enter Remark" MaxLength="1000" BackColor="White" TextMode="MultiLine"></asp:TextBox>
													</td>
													<td>
														<span id="lblHeadRemark" runat="server" class="clsLabelAuto">Head of Quality/ Head of Safety Remarks</span>
													</td>
													<td>
														<asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="TxtHeadRemark" runat="server" Text="<%# mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.HeadOfQualityRemark %>"
															ToolTip="Enter Remark" MaxLength="1000" BackColor="White" TextMode="MultiLine"></asp:TextBox>
													</td>
												</tr>
												<%--Added By Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page--%>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td colspan="5" valign="top">
														<fieldset class="clsFieldSetNewStyle">
															<legend class="clsFieldSet1">
																<b>File Attachments</b>
															</legend>
															<asp:UpdatePanel ID="upnlMultipleAttachment" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table width="100%">
																		<tr>
																			<td>
																				<br />
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:UpdatePanel ID="upnlGVMultipleAttachment" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<asp:GridView ID="MultipleAttachment" ToolTip="List of File Attached" runat="server"
																							CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
																							AllowSorting="True" GridLines="Horizontal" CellPadding="5" EnableViewState="true"
																							AllowPaging="False" AutoGenerateColumns="false" ClientIDMode="Static">
																							<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																							<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																							<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																							<Columns>
																								<%--0--%>
																								<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																								<%--1--%>
																								<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																									<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																								</asp:BoundField>
																								<%--2--%>
																								<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																									<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																								</asp:BoundField>
																								<%--3--%>
																								<asp:TemplateField HeaderText="File Name">
																									<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																									<ItemTemplate>
																										<asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBoxTagSearch"
																											MaxLength="100" placeholder="Enter Filename" ClientIDMode="Static" Width="350px"
																											ToolTip="Enter filename to be attached."
																											Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'>
																										</asp:TextBox>
																									</ItemTemplate>
																								</asp:TemplateField>
																								<%--4--%>
																								<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
																									ItemStyle-HorizontalAlign="Center">
																									<ItemTemplate>
																										<div id="dropDownImg" class="dropdown">
																											<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png"
																												runat="server" CssClass="clsActionbtn" />
																											<div id="dropdownICN-content" class="dropdownbtn-content">
																												<table id="dropdown-content" class="clsGridNew_Ajax">
																													<tr>
																														<td>
																															<asp:ImageButton ID="viewICN" class="attachmentICNS" runat="server"
																																CommandArgument='<%# Eval("SrNo") %>' CausesValidation="false"
																																ToolTip="Click to View Attachment."
																																CommandName="View" ImageUrl="icons/CLIP01.ICO" />
																														</td>
																													</tr>
																												</table>
																											</div>
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
																			<td valign="top" style="width: 10px;">
																				<asp:ImageButton ID="AddAttachment" runat="server" ImageUrl="~/images/plus1.png"
																					Height="22px" Width="24px" ToolTip="Click to Add New Attachment"
																					CausesValidation="false"></asp:ImageButton>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</fieldset>
													</td>
												</tr>
												<%--End--%>
												<tr style="height: 0px;">
													<td style="height: 0px;">
														<asp:UpdatePanel runat="server" ID="upnlHdnBtn" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
																	CausesValidation="False" Style="display: none;"></asp:Button>
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
		<!-- End -->

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

			function chkind() {
				var dropdown1 = document.getElementById('cmbRootCause');
				var textbox = document.getElementById('txtRootCause');
				if (dropdown1.selectedIndex == 0) {
					textbox.value = "";
				} else if (dropdown1.selectedIndex >= 1) {
					textbox.value = dropdown1.options[dropdown1.selectedIndex].text;
				}
			}
		</script>
		<!-- End -->
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
		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForFindings();
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
					parent.IFrameFindingsStateComplete();
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
	</form>
</body>
</html>

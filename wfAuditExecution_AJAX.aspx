<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAuditExecution_AJAX.aspx.vb"
	Inherits="Flypal.wfAuditExecution_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Audit Compliance Details</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>

	<script type="text/javascript">
		function showNestedGridView(obj) {
			var nestedGridView = document.getElementById(obj);
			var imageID = document.getElementById('image' + obj);

			if (nestedGridView.style.display == "none") {
				nestedGridView.style.display = "inline";
				imageID.src = "images/close.gif";
			} else {
				nestedGridView.style.display = "none";
				imageID.src = "images/detail.gif";
			}
		}
	</script>

</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
				EnablePageMethods="true">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<table class="clstablelistout" id="tblMain">
			<tr>
				<td>
					<table class="clsTablelistin" id="tblinner">
						<tr>

							<td colspan="4" class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Audit Compliance [New]</asp:Label>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td align="right">
											<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="Table1" cellspacing="1" cellpadding="1" border="0">
														<tr>
															<td>
																<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Send Mail of all Task Finding(s)"
																	ValidationGroup="1" CausesValidation="true" Width="150px" Text="Send Finding(s) Mail"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
																	Text="Print" Visible="false"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Audit Compliance"
																	Text="Save"></asp:Button>
															</td>
															<td>
																<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close the Audit Compliance Screen"
																	Text="Close"></asp:Button>
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
							<td colspan="4">
								<asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
											HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
										<asp:ValidationSummary ID="Validationsummary1" CssClass="clsValidationSummary" runat="server"
											ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
										<asp:CustomValidator ID="cvStartDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtStartDate"
											ErrorMessage="Enter Start Date" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" CssClass="clsLabelAuto"
											ControlToValidate="txtStartDate" ErrorMessage="Enter Start Date" Display="None"></asp:RequiredFieldValidator>
										<asp:CustomValidator ID="cvAuditor" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAuditorList"
											ErrorMessage="Select Auditor" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:CustomValidator ID="cvEndDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtEndDate"
											Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:CustomValidator ID="cvAuditIncharge" runat="server" CssClass="clsLabelAuto"
											ControlToValidate="txtAuditIncharge" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:CustomValidator ID="cvAuditStatus" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbAuditStatusList"
											Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
											Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="clsLabelAuto"
											ValidationGroup="1" ControlToValidate="txtToMailID" ErrorMessage="Please Enter To Mail-ID"
											Display="None"></asp:RequiredFieldValidator>
										<asp:CustomValidator ID="cvMailIDs" runat="server" ValidationGroup="1" Display="None"
											ControlToValidate="txtToMailID" ErrorMessage="Please Enter Valid To Mail-IDs"
											CssClass="clsLabelAuto" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
										<asp:CustomValidator ID="cvCc" runat="server" ValidationGroup="1" Display="None"
											ControlToValidate="txtCCMailID" ErrorMessage="Please Enter Valid Cc Email-ID"
											CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
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

										</script>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlAuditDet" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<fieldset id="fdswodetail" class="clsFieldSetNewStyle" style="border-width: 1px">
											<legend id="ldwodetail" runat="server"><b>Audit Compliance Detail</b></legend>
											<table class="clsTablelistin" id="Table2" cellspacing="1" cellpadding="1" border="0">
												<tr>
													<td style="height: 26px"></td>
													<td style="height: 26px">
														<asp:Label ID="lblAuditStandard" runat="server" CssClass="clsLabelAuto">Audit Standard</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtAuditStandard" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
															ToolTip="Audit Standard" Text="<%# mAuditExecution.AuditStandardName %>" BackColor="#E0E0E0"
															MaxLength="100"></asp:TextBox>
													</td>
													<td style="height: 26px"></td>
													<td>
														<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Audit On</asp:Label>
													</td>
													<td colspan="1">
														<table>
															<tr>
																<td>
																	<asp:TextBox ID="txtAuditOnName" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		MaxLength="100" ReadOnly="True" Text="<%# mAuditExecution.AuditOnName %>" ToolTip="Audit On"
																		Width="90px"></asp:TextBox>
																</td>
																<td>
																	<asp:TextBox ID="txtAuditOnNameDetail" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		MaxLength="100" ReadOnly="True" Text="<%# mAuditExecution.AuditOnNameDetail %>"
																		ToolTip="Audit On" Width="175px"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td style="height: 26px">
														<asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
													</td>
													<td style="height: 26px">
														<asp:Label ID="lblStartDate" runat="server" CssClass="clsLabelAuto">Start Date</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="True" BackColor="#E0E0E0"
															ClientIDMode="Static" CssClass="clsTextBoxTagDateSearch" onchange="ValidateDateText(this,'txtStartDate_watermarkextender');"
															Enabled="False" ForeColor="Black" ReadOnly="True"></asp:TextBox>
														<cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender ID="txtStartDate_watermarkextender" runat="server"
															ClientIDMode="Static" TargetControlID="txtStartDate" WatermarkCssClass="clsDateTextBox"
															WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
													</td>
													<td style="height: 26px">&nbsp;
													</td>
													<td>
														<asp:Label ID="lblAuditNo" runat="server" CssClass="clsLabelAuto">Audit No.</asp:Label>
													</td>
													<td colspan="1">
														<table id="Table5">
															<tr>
																<td>
																	<asp:TextBox ID="txtAuditNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		MaxLength="100" ReadOnly="True" Text="<%# mAuditExecution.AuditNo %>" ToolTip="Audit No."
																		Width="275px"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td style="height: 26px">&nbsp;
													</td>
													<td style="height: 26px">
														<asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
													</td>
													<td>
														<asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
															MaxLength="5000" ReadOnly="True" Text="<%# mAuditExecution.Description %>" TextMode="MultiLine"
															ToolTip="Description"></asp:TextBox>
													</td>
													<td style="height: 26px">&nbsp;
													</td>
													<td>
														<asp:Label ID="lblReferenceNo" runat="server" CssClass="clsLabel">Reference No.</asp:Label>
													</td>
													<td colspan="1">
														<table id="Table6">
															<tr>
																<td>
																	<asp:TextBox ID="txtReferenceNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																		MaxLength="500" ReadOnly="True" Text="<%# mAuditExecution.Reference %>" ToolTip="Reference No."
																		Width="275px"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<asp:Label ID="lblAuditType" runat="server" CssClass="clsLabelAuto">Audit Type</asp:Label>
													</td>
													<td>
														<table id="Table16" cellspacing="1" cellpadding="1" border="0">
															<tr>
																<td>
																	<asp:DropDownList ID="cmbAuditTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
																		Enabled="False" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditTypeID %>">
																	</asp:DropDownList>
																</td>
															</tr>
														</table>
													</td>
													<td></td>
													<td>
														<asp:Label ID="lblAuditIncharge" runat="server" CssClass="clsLabelAuto">Audit Incharge</asp:Label>
													</td>
													<td>
														<table id="Table13">
															<tr>
																<td>
																	<asp:TextBox ID="txtAuditIncharge" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Audit Incharge"
																		Text="<%# mAuditExecution.AuditIncharge %>" BackColor="White" MaxLength="500"
																		Width="275px"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="Label9" runat="server" CssClass="clsLabelStar">*</asp:Label>
													</td>
													<td>
														<asp:Label ID="lblAuditor" runat="server" CssClass="clsLabelAuto">Lead Auditor</asp:Label>
													</td>
													<td>
														<table id="Table11" border="0" cellpadding="1" cellspacing="1">
															<tr>
																<td>
																	<asp:DropDownList ID="cmbAuditorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="NameWithWorkingStatus"
																		AutoPostBack="true" DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditorID %>"
																		Width="277px" OnDataBound="cmbAuditorList_DataBound">
																	</asp:DropDownList>
																</td>
																<td>
																	<asp:ImageButton ID="imgbtnAuditor" runat="server" CausesValidation="False" Height="22px"
																		ImageUrl="~/images/plus1.png" ToolTip="Click to Add New Lead Auditor" Width="24px" />
																</td>
															</tr>
														</table>
													</td>
													<td></td>
													<td>
														<asp:Label ID="lblAuditors" runat="server" CssClass="clsLabelAuto">Auditors</asp:Label>
													</td>
													<td>
														<table id="Table19" width="100%">
															<tr>
																<td>
																	<asp:TextBox ID="txtAuditors" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																		MaxLength="500" Text="<%# mAuditExecution.Auditors %>" TextMode="MultiLine" ToolTip="Enter Auditors" Height="18px"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
													</td>
													<td>
														<asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
													</td>
													<td>
														<table id="Table15" cellspacing="1" cellpadding="1" border="0">
															<tr>
																<td>
																	<asp:DropDownList ID="cmbDesignationList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																		DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditExecution.DesignationID %>"
																		Width="277px">
																	</asp:DropDownList>
																</td>
																<td>
																	<asp:ImageButton ID="imgbtnDesignation" runat="server" ImageUrl="~/images/plus1.png"
																		Height="22px" Width="24px" ToolTip="Click to Add New Designation" CausesValidation="False"></asp:ImageButton>
																</td>
															</tr>
														</table>

													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<asp:Label ID="lblEntityManager" runat="server" CssClass="clsLabelAuto" Width="88px">Entity Manager</asp:Label>
													</td>
													<td colspan="2">
														<table id="Table20" width="97%">
															<tr>
																<td>
																	<asp:TextBox ID="txtlEntityManager" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" ToolTip="Enter Entity Manager"
																		Text="<%# mAuditExecution.EntityManager %>" BackColor="White" MaxLength="500" Height="18px"
																		TextMode="MultiLine"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>

													<td>
														<asp:Label ID="lblOtherParticipants" runat="server" CssClass="clsLabel" Width="108px">Other Participants</asp:Label>
													</td>
													<td>
														<table id="Table21" width="100%">
															<tr>
																<td>
																	<asp:TextBox ID="txtOtherParticipants" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																		ToolTip="Enter Other Participants" Text="<%# mAuditExecution.OtherParticipants %>" Height="18px"
																		BackColor="White" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<asp:Label ID="lblOtherInformation" runat="server" CssClass="clsLabel" Width="108px">Other Information</asp:Label>
													</td>
													<td colspan="2">
														<table id="Table22" width="97%">
															<tr>
																<td>
																	<asp:TextBox ID="txtOtherInformation" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																		ToolTip="Enter Other Information" Text="<%# mAuditExecution.OtherInformation %>"
																		BackColor="White" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>

													<td>
														<asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note</asp:Label>
													</td>
													<td>
														<table id="Table18" cellspacing="1" cellpadding="1" border="0" width="100%">
															<tr>
																<td>
																	<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" ToolTip="Enter Note"
																		Text="<%# mAuditExecution.Note %>" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td></td>
													<td>
														<span class="clsLabelAuto">To Mail</span>
													</td>
													<td>
														<table id="Table19" cellspacing="1" cellpadding="1" border="0" width="100%">
															<tr>
																<td>
																	<asp:TextBox ID="txtToMailID" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
																		Text="<%# mAuditExecution.ToMailIDs %>" TextMode="MultiLine" ClientIDMode="Static"
																		ToolTip="Enter comma separated To Mail ID's to send all Task Findings"></asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
													<td></td>
													<td>
														<span class="clsLabelAuto">CC Mail</span>
													</td>
													<td>
														<asp:TextBox ID="txtCCMailID" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
															Text="<%# mAuditExecution.CCMailIDs %>" TextMode="MultiLine" ClientIDMode="Static"
															ToolTip="Enter comma separated CC Mail ID's to send all Task Findings"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:UpdatePanel ID="upnlEndDetail" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
																	<legend id="Legend1" runat="server"><b>Audit Closing Detail</b></legend>
																	<table id="Table9" cellspacing="1" cellpadding="1" border="0">
																		<tr>
																			<td></td>
																			<td>
																				<asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelAuto" Width="108px">End Date</asp:Label>
																			</td>
																			<td>
																				<asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="True" ClientIDMode="Static"
																					CssClass="clsTextBoxTagDateSearch" onchange="ValidateDateText(this,'txtEndDate_watermarkextender');"></asp:TextBox>
																				<cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																					Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate"></cc2:CalendarExtender>
																				<cc2:TextBoxWatermarkExtender ID="txtEndDate_watermarkextender" runat="server" ClientIDMode="Static"
																					TargetControlID="txtEndDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="Label5" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
																			</td>
																			<td>
																				<asp:Label ID="lblAuditStatus" runat="server" CssClass="clsLabelAuto">Audit Status</asp:Label>
																			</td>
																			<td>
																				<table id="Table12" cellspacing="1" cellpadding="1" border="0">
																					<tr>
																						<td>
																							<asp:DropDownList ID="cmbAuditStatusList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
																								DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditExecution.AuditStatusID %>">
																							</asp:DropDownList>
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
													<td></td>
													<td>
														<span id="lblAttachFile" class="clsLabel">Attach File</span>
													</td>
													<td>
														<asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table border="0" cellpadding="0" cellspacing="0">
																	<tr>
																		<td>
																			<input type="button" id="btnSelectFile" runat="server" value="Select File" style="width: 100px;"
																				clientidmode="Static" class="clsbtnH" />
																		</td>
																		<td style="padding-left: 3px;">
																			<asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH" ToolTip="Click to Remove Attachment"
																				Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
																		</td>
																		<td style="padding-left: 2px;">
																			<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
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
							<td>
								<br />
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlExecutionTask" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table3">
											<tr>
												<td>
													<asp:Label ID="lblAuditScheduleTask" runat="server" CssClass="clsLabelHeaderItem"
														Width="170px">Audit Compliance Task(s)</asp:Label>
												</td>
												<td align="right">
													<asp:Button ID="btnAddExecutionTask" runat="server" CssClass="clsbtnH" ToolTip="Click to add Audit Compliance Task"
														Text="Add" CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:GridView ID="dgAuditExecutionTask" runat="server" AutoGenerateColumns="False"
											CellPadding="10" GridLines="Horizontal" CssClass="clsGridNewStyle" PageSize="3" Width="100%" ShowHeaderWhenEmpty="true">
											<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
											<RowStyle CssClass="clsdgItem"></RowStyle>
											<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
											<Columns>
												<asp:TemplateField HeaderText="Select">
													<HeaderTemplate>
													</HeaderTemplate>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
													<ItemTemplate>
														<div>
															<a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
																<img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
															</a>
														</div>
													</ItemTemplate>
												</asp:TemplateField>
												<%--0--%>
												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
												<%--1--%>
												<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
													<HeaderStyle Width="10px" HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<%--3--%>
												<asp:BoundField DataField="AuditCategoryName" HeaderText="Task Category">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<%--4--%>
												<asp:BoundField DataField="Code" HeaderText="Code">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<%--5--%>
												<asp:TemplateField HeaderText="Kind Attention" Visible="false">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													<ItemTemplate>
														<asp:TextBox ID="txtKindAttention" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
															BackColor="White" Text='<%# DataBinder.Eval(Container.DataItem, "KindAttention") %>'
															ToolTip="Enter Kind Attention"></asp:TextBox>
													</ItemTemplate>
												</asp:TemplateField>
												<%--6--%>
												<asp:BoundField DataField="Description" HeaderText="Description">
													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<%--7--%>
												<asp:TemplateField HeaderText="Task Status">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													<ItemTemplate>
														<asp:DropDownList ID="cmbTaskStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "TaskStatusID") %>'
															DataValueField="ID" DataTextField="Name" Width="106px" DataSource="<%# mTaskStatusList %>">
														</asp:DropDownList>
													</ItemTemplate>
												</asp:TemplateField>
												<%--8--%>
												<asp:TemplateField HeaderText="Compliance Remark">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													<ItemTemplate>
														<asp:TextBox ID="txtComplianceDetails" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
															MaxLength="3000" Text='<%# DataBinder.Eval(Container.DataItem, "ComplianceDetails") %>'
															TextMode="MultiLine" ToolTip="Enter Compliance Remark"></asp:TextBox>
													</ItemTemplate>
												</asp:TemplateField>
												<%--9--%>
												<asp:BoundField Visible="False" DataField="Note" HeaderText="Note">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<%--10--%>
												<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec" Visible="false"
													HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
												<%--11--%>
												<asp:ButtonField DataTextField="FindingsCountDisp"
													HeaderText="Add Findings" CommandName="Findings"
													HeaderStyle-HorizontalAlign="Left">
													<ItemStyle Wrap="false" />
												</asp:ButtonField>
												<%--12--%>
												<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
													<ItemTemplate>
														<asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="RemoveRec" Style="height: 20px; width: 20px"
															ImageUrl="~/images/delete.png" />
													</ItemTemplate>
													<HeaderStyle HorizontalAlign="Center" />
													<ItemStyle HorizontalAlign="Center" />
												</asp:TemplateField>
												<%--13--%>
												<asp:TemplateField>
													<ItemTemplate>
														<tr>
															<td colspan="100%" bgcolor="White" width="0px">
																<div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 17px">
																	<panel>


																		<table>
																			<tr>
																				<asp:Label ID="lblAuditFindings" runat="server" CssClass="clsLabelHeaderItem">Findings(s) </asp:Label>
																			</tr>
																			<tr>
																				<td colspan="100%" bgcolor="White" width="0px">
																					<asp:GridView ID="grdTaskFindings" runat="server" AutoGenerateColumns="False" Width="95%"
																						OnRowCommand="GV_TaskFindings_RowCommand" DataKeyNames="ID,AuditExecutionTaskID"
																						CellPadding="5" GridLines="Horizontal" BorderStyle="Groove" ForeColor="#333333" CssClass="clsGridNewStyle"
																						AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
																						SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
																						<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																						<Columns>
																							<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																							<%--0--%>
																							<asp:BoundField Visible="False" DataField="AuditExecutionTaskID" HeaderText="AuditExecutionTaskID"></asp:BoundField>
																							<%--1--%>
																							<asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Left">
																								<HeaderStyle Width="10px" Wrap="true" />
																							</asp:BoundField>
																							<%--2--%>
																							<asp:BoundField DataField="FindingNo" HeaderText="Finding No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--3--%>
																							<asp:BoundField DataField="Reference" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--4--%>
																							<asp:BoundField DataField="Finding" HeaderText="Finding" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--5--%>
																							<asp:BoundField Visible="False" DataField="RootCause" HeaderStyle-HorizontalAlign="Left"
																								HeaderText="Root Cause"></asp:BoundField>
																							<%--6--%>
																							<asp:BoundField DataField="PriorityName" HeaderText="Priority" HeaderStyle-HorizontalAlign="Left"
																								Visible="false">
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--7--%>
																							<asp:BoundField DataField="FindingStatusName" HeaderText="Finding Status" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--8--%>
																							<asp:BoundField DataField="DeadlineDateFormatted" HeaderText="Deadline Date" Visible="false">
																								<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--9--%>
																							<asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="Left"
																								Visible="false"></asp:BoundField>
																							<%--10--%>
																							<asp:BoundField DataField="Category" HeaderText="Evidence" HeaderStyle-HorizontalAlign="Left"
																								Visible="false"></asp:BoundField>
																							<%--11--%>
																							<asp:BoundField DataField="KindAttention" HeaderText="Responsible Person" HeaderStyle-HorizontalAlign="Left"
																								Visible="false"></asp:BoundField>
																							<%--12--%>
																							<asp:BoundField Visible="False" DataField="CAPA" HeaderText="C/P Action" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--13--%>
																							<asp:BoundField DataField="CorrectionDateFormatted" HeaderText="Correction Date"
																								Visible="false" HeaderStyle-HorizontalAlign="Left">
																								<HeaderStyle Wrap="False"></HeaderStyle>
																								<ItemStyle Wrap="False"></ItemStyle>
																							</asp:BoundField>
																							<%--14--%>
																							<asp:BoundField DataField="ToMailID" HeaderText="ToMailID" HeaderStyle-HorizontalAlign="Left"
																								Visible="false"></asp:BoundField>
																							<%--15--%>
																							<asp:BoundField DataField="CCMailID" HeaderText="CCMailID" HeaderStyle-HorizontalAlign="Left"
																								Visible="false"></asp:BoundField>
																							<%--16--%>
																							<asp:BoundField Visible="False" DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
																							<%--17--%><asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																								<ItemTemplate>
																									<div class="dropdown">
																										<div class="dropdownbtn-content">
																											<table id="dropdown-content" class="clsGridNew_Ajax">
																												<tr>
																													<td>
																														<asp:ImageButton ID="EditView" runat="server"
																															CommandName="EditRecFinding" class="actionICNS"
																															ToolTip="Click to Edit Finding."
																															ImageUrl="~/images/edit.png" CausesValidation="false" />
																													</td>
																													<td>
																														<asp:ImageButton ID="DeleteRecord" runat="server"
																															CausesValidation="false" CommandName="RemoveRecFinding"
																															ToolTip="Click to Delete Finding."
																															class="actionICNS  largerActionICNS" ImageUrl="~/images/delete.png" />
																													</td>

																													<td>
																														<asp:ImageButton ID="View" runat="server" CausesValidation="false"
																															CommandName="ViewRecFinding" class="attachmentICNS"
																															ToolTip="Click to View Attachment(s)."
																															ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
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
																							<%--18--%>
																							<asp:ButtonField Text="Send Mail" HeaderText="Send Mail" CommandName="SendMail" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
																							<%--21--%> <%--19--%>
																							<asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																							<%--22--%> <%--20--%>
																							<asp:BoundField DataField="IsMailExist" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsMailExist" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
																							<%--23--%> <%--21--%>
																						</Columns>
																					</asp:GridView>
																				</td>
																			</tr>
																		</table>
																	</panel>
																</div>
															</td>
														</tr>
													</ItemTemplate>
												</asp:TemplateField>
											</Columns>
											<SelectedRowStyle BackColor="ControlDark" />
											<AlternatingRowStyle CssClass="clsdgAltItem" />
										</asp:GridView>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td align="right" colspan="4">
								<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Button ID="hdnimgBtnDesignation" runat="server" CausesValidation="false" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
										<asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
											Style="display: none;" Text="----" />
										<asp:Button ID="hdnimgBtnAuditor" runat="server" CausesValidation="False" ClientIDMode="Static"
											Style="display: none;" Text="----" />
										<asp:Button ID="hdnimgBtnTaskMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
										<asp:Button ID="hdnimgBtnExecutionTask" runat="server" CausesValidation="false" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
										<asp:Button ID="hdnimgBtnFindings" runat="server" CausesValidation="false" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
										<asp:Button ID="hdnimgBtnReSchedule1" runat="server" CausesValidation="false" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
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
		<!-- TaskMaster Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyTaskMaster" Text="Dummy TaskMaster" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupTaskMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupTaskMaster" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupTaskMaster" runat="server" TargetControlID="btnDummyTaskMaster"
			PopupControlID="pnlPopupTaskMaster" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameTaskMasterStateComplete() {
				$("#btnDummyTaskMaster").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenTaskWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupTaskMaster").attr("src", "wfTaskListForAuditSchedule_AJAX.aspx?Type=pup&AType=2");

					if (!$.browser.msie) {
						$("#btnDummyTaskMaster").click();
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
				var TaskMasterwindow = $find("<%=mdlPopupTaskMaster.ClientID %>");
				//close TaskMaster popup window
				TaskMasterwindow.hide();
				$("#iPopupTaskMaster").attr("src", "JavaScript:''");
				//call TaskMaster image button
				$("#hdnimgBtnTaskMaster").click();
			}
		</script>
		<!-- End-->
		<!-- Designation Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupDesignation" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupDesignation" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
			PopupControlID="pnlPopupDesignation" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameDesignationStateComplete() {
				$("#btnDummyDesignation").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenAuditDesignationWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupDesignation").attr("src", "wfAuditDesignation_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyDesignation").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForDesignation() {
				var Designationwindow = $find("<%=mdlPopupDesignation.ClientID %>");
				//close Designation popup window
				Designationwindow.hide();
				$("#iPopupDesignation").attr("src", "JavaScript:''");
				//call Designation image button
				$("#hdnimgBtnDesignation").click();
			}
		</script>
		<!-- End-->
		<!-- Auditor Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyAuditor" Text="Dummy Auditor" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupAuditor" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupAuditor" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupAuditor" runat="server" TargetControlID="btnDummyAuditor"
			PopupControlID="pnlPopupAuditor" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameAuditorStateComplete() {
				$("#btnDummyAuditor").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenAuditorWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupAuditor").attr("src", "wfAuditor_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyAuditor").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForAuditor() {
				var Auditorwindow = $find("<%=mdlPopupAuditor.ClientID %>");
				//close Auditor popup window
				Auditorwindow.hide();
				$("#iPopupAuditor").attr("src", "JavaScript:''");
				//call Auditor image button
				$("#hdnimgBtnAuditor").click();
			}
		</script>
		<!-- End-->
		<!-- Findings Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyFindings" Text="Dummy Findings" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupFindings" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupFindings" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFindings" runat="server" TargetControlID="btnDummyFindings"
			PopupControlID="pnlPopupFindings" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFindingsStateComplete() {
				$("#btnDummyFindings").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenFindingsWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupFindings").attr("src", "wfAuditExecutionTaskFinding_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyFindings").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFindings() {
				var Findingswindow = $find("<%=mdlPopupFindings.ClientID %>");
				//close Findings popup window
				Findingswindow.hide();
				$("#iPopupFindings").attr("src", "JavaScript:''");
				//call Findings image button
				$("#hdnimgBtnFindings").click();
			}
		</script>
		<!-- End-->
		<!-- ExecutionTask Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyExecutionTask" Text="Dummy ExecutionTask"
				ClientIDMode="Static" CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupExecutionTask" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupExecutionTask" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupExecutionTask" runat="server" TargetControlID="btnDummyExecutionTask"
			PopupControlID="pnlPopupExecutionTask" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameExecutionTaskStateComplete() {
				$("#btnDummyExecutionTask").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenAuditExecutionTaskWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupExecutionTask").attr("src", "wfAuditExecutionTask_AJAX.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyExecutionTask").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForExecutionTask() {
				var ExecutionTaskwindow = $find("<%=mdlPopupExecutionTask.ClientID %>");
				//close ExecutionTask  popup window
				ExecutionTaskwindow.hide();
				$("#iPopupExecutionTask").attr("src", "JavaScript:''");
				//call ExecutionTask  image button
				$("#hdnimgBtnExecutionTask").click();
			}
		</script>
		<!-- End-->
		<!-- ReSchedule Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyReSchedule" Text="Dummy ReSchedule" ClientIDMode="Static"
				CausesValidation="false" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupReSchedule" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupReSchedule" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupReSchedule" runat="server" TargetControlID="btnDummyReSchedule"
			PopupControlID="pnlPopupReSchedule" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameReScheduleStateComplete() {
				$("#btnDummyReSchedule").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenAuditorReScheduleWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupReSchedule").attr("src", "wfAuditReSchedule_AJAX.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyReSchedule").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForReSchedule() {
				var ReSchedulewindow = $find("<%=mdlPopupReSchedule.ClientID %>");
				//close ReSchedule  popup window
				ReSchedulewindow.hide();
				$("#iPopupReSchedule").attr("src", "JavaScript:''");
				//call ReSchedule  image button
				$("#hdnimgBtnReSchedule").click();
			}
		</script>
		<!-- End-->

		<%--Added By Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page--%>
		<!--Multiple Attachment Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="dummyBtnViewMultipleAttachments" Text="Attach" CausesValidation="false"
				ClientIDMode="Static" />
		</div>

		<asp:Panel runat="server" ID="pnlMultipleAttachments" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeMultipleAttachments" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>

		<cc2:ModalPopupExtender ID="mdlMultipleAttachments" runat="server" TargetControlID="dummyBtnViewMultipleAttachments"
			PopupControlID="pnlMultipleAttachments" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">
			function IFrameMultipleAttachmentsStateComplete() {
				$("#dummyBtnViewMultipleAttachments").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenMultipleAttachmentWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeMultipleAttachments").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#dummyBtnViewMultipleAttachments").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForAttach() {
				var Attachwindow = $find("<%=mdlMultipleAttachments.ClientID %>");
				//close popup window
				Attachwindow.hide();
				//release resources
				$("#IframeMultipleAttachments").attr("src", "JavaScript:''");
				//call button click
				$("#hdnBtnAttach").click();
			}
		</script>
		<!-- End-->

		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var resetTodaysDate = 'False';
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
				parent.ParentCallBackFunctionForAuditExecution();
				return false;
			}
		</script>
		<%--End--%>

		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			<% Dim mopen As String = Request.QueryString("Type") %>

			<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

				$(document).ready(function () {
					SetPageLayout();
					if ($.browser.msie) {
						parent.IFrameAuditExecutionStateComplete();
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

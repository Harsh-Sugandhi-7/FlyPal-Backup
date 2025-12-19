<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateMailID_Ajax.aspx.vb"
	Inherits="Flypal.wfUpdateMailID_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<title>Update Due Limit For FAS</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
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
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin" border="0">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">
															Update Mail IDs For Reports
														</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table cellspacing="0">
															<tr>
																<td>
																	<asp:Button ID="btnUpdate" runat="server" 
																		CssClass="clsbtnH clsinfoH" TabIndex="0" ToolTip="Click to Update Record."
																		Text="Update" CausesValidation="true" />
																</td>
																<td>
																	<asp:Button ID="btnClose" runat="server" CausesValidation="False" 
																		CssClass="clsbtnH clsinfoH" ToolTip="Click to close Update Mail IDs For Reports screen."
																		TabIndex="0" Text="Close" />
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
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvMailIDs" runat="server" Display="None" ControlToValidate="txtEmailIDs"
												ErrorMessage="Please Enter Valid Email-ID"
												ClientValidationFunction="validateMultipleEmailsCommaSeparated">
											</asp:CustomValidator>
											<asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ControlToValidate="txtCC"
												ErrorMessage="Please Enter Valid CC Email-ID"
												ClientValidationFunction="validateMultipleCCEmailsCommaSeparated">
											</asp:CustomValidator>

											<script type="text/javascript">
												function validateDayofMonth(source, args) {
													var text = $("#txtDayofMonth").val();
													var seperator = ',';
													if (text != '' && text > 31) {
														args.IsValid = false;
														return;
													}
												}

												function validateEmail(field) {
													var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
													return (regex.test(field)) ? true : false;
												}

												function validateMultipleEmailsCommaSeparated(source, args) {
													var text = $("#txtEmailIDs").val();
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

												function validateMultipleCCEmailsCommaSeparated(source, args) {
													var text = $("#txtCC").val();

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

												function validateMultipleBCCEmailsCommaSeparated(source, args) {
													var text = $("#txtBCC").val();

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
								<td>
									<asp:UpdatePanel ID="upnlMailIDs" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<asp:RadioButton Text="Report List" ID="rdbReportlist" CssClass="clsLabelAuto"
															Checked="true" GroupName="a" runat="server" AutoPostBack="true" />
													</td>
													<td>
														<asp:RadioButton Text="Transaction List" ID="rdbTranslist" GroupName="a"
															runat="server" AutoPostBack="true" CssClass="clsLabelAuto" />
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<asp:PlaceHolder ID="placeholder1" runat="server">
													<tr>
														<td>
															<span id="Span3" class="clsLabelAuto">Report List</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbReportNameList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																DataValueField="ModuleID" DataTextField="Name" Width="355px" AutoPostBack="true">
															</asp:DropDownList>
														</td>
													</tr>
												</asp:PlaceHolder>
												<asp:PlaceHolder ID="placeholder2" runat="server">
													<tr>
														<td>
															<span id="lblReportName" class="clsLabelAuto">Transaction List</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbTransactionList" runat="server"
																CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID" DataTextField="Name" Width="350px" AutoPostBack="true">
															</asp:DropDownList>
														</td>

													</tr>
												</asp:PlaceHolder>
												<tr>
													<td>
														<span id="Span1" class="clsLabelAuto">Email IDs</span>
													</td>
													<td>
														<asp:TextBox ID="txtEmailIDs" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="350px" runat="server"></asp:TextBox>
													</td>

												</tr>
												<tr>
													<td>
														<span id="Span2" class="clsLabelAuto">CC</span>
													</td>
													<td>
														<asp:TextBox ID="txtCC" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="350px" runat="server"></asp:TextBox>
													</td>
													<td>
												</tr>
												<tr>
													<td>&nbsp;</td>
													<td colspan="2">
														<span id="Span12" class="clsLabelAuto"><b>Please add Comma separated mail IDs</b></span>
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
	</form>
</body>
</html>

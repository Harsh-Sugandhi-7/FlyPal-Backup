<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfByMail_Ajax.aspx.vb"
	Inherits="Flypal.wfByMail_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>By Mail</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table class="clstablelistin" id="tblInner">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<span id="lblTitle" class="clsFormHeader">Report Mail Service</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
										HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
									<asp:CustomValidator ID="cvReqMailID" runat="server" ValidationGroup="1" Display="None"
										ErrorMessage="Please Enter at least one Valid Email-ID" ControlToValidate="txtMailIDs"
										CssClass="clsLabel" ClientValidationFunction="validateEmailID" ValidateEmptyText="true"></asp:CustomValidator>
									<asp:CustomValidator ID="cvCc" runat="server" ValidationGroup="1" Display="None"
										ControlToValidate="txtCCIDs" ErrorMessage="Please Enter Valid Cc Email-ID" CssClass="clsLabel"
										ClientValidationFunction="validateMultipleCcEmailsCommaSeparated"></asp:CustomValidator>
									<asp:CustomValidator ID="cvMailIDs" runat="server" ValidationGroup="1" Display="None"
										ErrorMessage="Please Enter Valid Email-ID" CssClass="clsLabel" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
									<script type="text/javascript">
										function validateEmailID(source, args) {
											var MandatoryEmailID = $("#lblToMailID").text();
											var optionalEmailID = $("#txtMailIDs").val();
											if (MandatoryEmailID == '' && optionalEmailID == '') {
												args.IsValid = false;
												return;
											}
										}
										function validateEmail(field) {
											var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
											return (regex.test(field)) ? true : false;
										}
										function validateMultipleEmailsCommaSeparated(source, args) {
											var text = $("#txtMailIDs").val();
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
										function validateMultipleCcEmailsCommaSeparated(source, args) {
											var text = $("#txtCCIDs").val();
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
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSendMailDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px">
												<legend id="Legend1" runat="server"><b>Receive report print on following Email-ID’s
												</b></legend>
												<table>
													<tr>
														<td colspan="2">
															<span class="clsLabel" style="color: #FF0000">Please enter comma separated Email-ID&#39;s
                                                            to receive report print</span>
														</td>
													</tr>
													<tr>
														<td colspan="2">&nbsp;
														</td>
													</tr>
													<tr>
														<td>
															<span class="clsLabelAuto">To...</span>
														</td>
														<td>
															<asp:TextBox ID="txtMailIDs" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ClientIDMode="Static"
																TextMode="MultiLine" Width="350px"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span class="clsLabelAuto">Cc...</span>
														</td>
														<td>
															<asp:TextBox ID="txtCCIDs" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ClientIDMode="Static"
																TextMode="MultiLine" Width="350px"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span class="clsLabelAuto" runat="server" id="lblRemark" visible='<%# IIf(AppSettings("ClientCode") = "APFT" Or
																						   AppSettings("ClientCode") = "HSC" Or
																						   AppSettings("ClientCode") = "CMX" Or
																						   AppSettings("ClientCode") = "AAP",
																						   True,
																						   False) %>'>
																Remark
															</span>
														</td>
														<td>
															<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ClientIDMode="Static"
																TextMode="MultiLine" Width="350px" Visible='<%# IIf(AppSettings("ClientCode") = "APFT" Or
																								   AppSettings("ClientCode") = "HSC" Or
																								   AppSettings("ClientCode") = "CMX" Or
																								   AppSettings("ClientCode") = "AAP", True, False) %>'></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span class="clsLabelAuto" runat="server" id="lblReportGenratedBy" visible='<%# IIf(AppSettings("ClientCode") = "APFT" Or
																											   AppSettings("ClientCode") = "HSC" Or
																											   AppSettings("ClientCode") = "CMX" Or
																											   AppSettings("ClientCode") = "AAP", True, False) %>'>Report Genrated By</span>
														</td>
														<td>
															<asp:TextBox ID="txtReportGenratedBy" runat="server" AutoPostBack="False" CssClass="clsTextBoxTagSearch"
																Width="275px" MaxLength="100" Visible='<%# IIf(AppSettings("ClientCode") = "APFT" Or
																								AppSettings("ClientCode") = "HSC" Or
																								AppSettings("ClientCode") = "CMX" Or
																								AppSettings("ClientCode") = "AAP", True, False) %>'></asp:TextBox>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table border="0">
												<tr>
													<td>
														<asp:Button ID="btnSendMail" runat="server"
															CssClass="clsbtnH clsinfoH1" Text="Send"
															ToolTip="Click to send E-Mail to respective people."
															ValidationGroup="1" CausesValidation="true" />
													</td>
													<td>
														<asp:Button ID="btnBack" runat="server"
															CssClass="clsbtnH clsinfoH1" Text="Close"
															ToolTip="Click to go back to the previous page" />
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
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForSendMail();
				return false;
			}
			function CallParentToSendMail() {
				parent.ParentCallBackFunctionToSendMail();
				return false;
			}
		</script>
		<%--End--%>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
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
		<%--End--%>
	</form>
</body>
</html>

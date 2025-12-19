<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateMailIDsForFAS_Ajax.aspx.vb"
	Inherits="Flypal.wfUpdateMailIDsForFAS_Ajax" %>

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
	<style type="text/css">
		#arrowICN {
			cursor: pointer;
		}

		#dropdown-content {
			z-index: 7;
			position: relative;
		}

		.actionICNS {
			height: 15px;
			width: 15px;
		}
	</style>
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
															Update Mail IDs For FAS
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
																		CssClass="clsbtnH clsinfoH" TabIndex="0"
																		Text="Update" CausesValidation="true" 
																		ToolTip="Click to Update Record."/>
																</td>
																<td>
																	<asp:Button ID="btnClose" runat="server" 
																		CausesValidation="False" CssClass="clsbtnH clsinfoH"
																		TabIndex="0" Text="Close" 
																		ToolTip="Click to close Update Mail IDs For FAS screen."/>
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
												ErrorMessage="Please Enter Valid Email-ID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
											<asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ControlToValidate="txtCC"
												ErrorMessage="Please Enter Valid CC Email-ID" CssClass="" ClientValidationFunction="validateMultipleCCEmailsCommaSeparated"></asp:CustomValidator>
											<asp:CustomValidator ID="CustomValidator2" runat="server" Display="None" ControlToValidate="txtBCC"
												ErrorMessage="Please Enter Valid BCC Email-ID" CssClass="" ClientValidationFunction="validateMultipleBCCEmailsCommaSeparated"></asp:CustomValidator>
											<asp:CustomValidator ID="CustomValidator3" runat="server" Display="None" ControlToValidate="txtDayofMonth"
												ErrorMessage="Please Enter Valid Day of month" CssClass="" ClientValidationFunction="validateDayofMonth"></asp:CustomValidator>
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
														<span id="lblReportName" class="clsLabelAuto">Report Name</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbReportNameList" runat="server"
															CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID" DataTextField="ReportName" Width="355px" AutoPostBack="true">
														</asp:DropDownList>
													</td>
													<td>
														<span id="Span4" class="clsLabelAuto">Monday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbMonday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="1">1</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<span id="Span8" class="clsLabelAuto">Friday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbFriday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="5">5</asp:ListItem>
														</asp:DropDownList>
													</td>
												</tr>
												<tr>
													<td>
														<span id="Span1" class="clsLabelAuto">Email IDs</span>
													</td>
													<td>
														<asp:TextBox ID="txtEmailIDs" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="350px" runat="server"></asp:TextBox>
													</td>
													<td>
														<span id="Span5" class="clsLabelAuto">Tuesday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbTuesday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="2">2</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<span id="Span9" class="clsLabelAuto">Saturday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbSaturday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="6">6</asp:ListItem>
														</asp:DropDownList>
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
														<span id="Span6" class="clsLabelAuto">Wednesday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbWednesday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="3">3</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<span id="Span10" class="clsLabelAuto">Sunday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbSunday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="0">0</asp:ListItem>
														</asp:DropDownList>
													</td>
												</tr>

												<tr>
													<td>
														<span id="Span3" class="clsLabelAuto">BCC</span>
													</td>
													<td>
														<asp:TextBox ID="txtBCC" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="350px"></asp:TextBox>
													</td>
													<td>
														<span id="Span7" class="clsLabelAuto">Thursday</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbThursday" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="50px">
															<asp:ListItem Value="-1">-1</asp:ListItem>
															<asp:ListItem Value="4">4</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td>
														<span id="Span11" class="clsLabelAuto">Day of Month</span>
													</td>
													<td>
														<asp:TextBox ID="txtDayofMonth" runat="server" CssClass="clsTextBoxTagSearchSmall" Width="30px"></asp:TextBox>
													</td>
													<td>
														<asp:CheckBox ID="IsDaily" runat="server" Text="IsDaily" CssClass="clsCheckBox" TextAlign="Left" />
													</td>
												</tr>
												<tr>
													<td>&nbsp;</td>
													<td colspan="5">
														<span id="Span12" class="clsLabelAuto"><b>Please add Comma separated mail IDs</b></span>
													</td>
												</tr>
											</table>
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
								<td>
									<asp:UpdatePanel ID="upnlReportNameListGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:GridView ID="dgReportNameList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
												AllowPaging="True" PageSize="10" ShowHeaderWhenEmpty="true"
												PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ID" HeaderText="Id" Visible="False"></asp:BoundField>
													<asp:BoundField DataField="ReportName" HeaderText="Report Name">
														<HeaderStyle HorizontalAlign="Left" />
														<ItemStyle HorizontalAlign="Left" Wrap="true" Width="150px" />
													</asp:BoundField>
													<asp:BoundField DataField="Emails" HeaderText="Emails">
														<HeaderStyle HorizontalAlign="Left" />
														<ItemStyle HorizontalAlign="Left" Wrap="true" CssClass="TextBreak" Width="500px" />
													</asp:BoundField>
													<asp:BoundField DataField="cc" HeaderText="cc">
														<HeaderStyle HorizontalAlign="Left" />
														<ItemStyle Wrap="True" CssClass="TextBreak" Width="500px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Bcc" HeaderText="Bcc">
														<HeaderStyle HorizontalAlign="Left" Wrap="true" />
														<ItemStyle Wrap="True" CssClass="TextBreak" Width="500px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="DayOfMonth" HeaderText="DayOfMonth">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Monday" HeaderText="Monday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Tuesday" HeaderText="Tuesday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Wednesday" HeaderText="Wednesday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Thursday" HeaderText="Thursday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Friday" HeaderText="Friday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Saturday" HeaderText="Saturday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:BoundField DataField="Sunday" HeaderText="Sunday">
														<HeaderStyle HorizontalAlign="right" />
														<ItemStyle HorizontalAlign="right" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
														<HeaderStyle HorizontalAlign="Center" />
														<ItemStyle HorizontalAlign="Center" />
														<ItemTemplate>
															<div id="dropDownImg" class="dropdown">
																<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																<div id="dropdownICN-content" class="dropdownbtn-content">
																	<table id="dropdown-content" class="clsGridNew_Ajax">
																		<tr>
																			<td>
																				<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																					CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																					ToolTip="Click to Edit record"
																					CommandName="EditRec" ImageUrl="~/images/edit.png" />
																			</td>
																		</tr>
																	</table>
																</div>
															</div>
														</ItemTemplate>
													</asp:TemplateField>
												</Columns>
											</asp:GridView>
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

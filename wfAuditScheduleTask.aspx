<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAuditScheduleTask.aspx.vb" Inherits="Flypal.wfAuditScheduleTask" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Audit Schedule Task</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		<%--<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->--%>
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" title="Charge Informaton" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<TABLE class="clstablelistin" id="tblInner">
							<TR>
								<TD colSpan="3"><asp:label id="lblTitle" CssClass="clstitle1" Runat="server">Audit Schedule Task</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="3"><asp:validationsummary id="Validationsummary1" Runat="server" Cssclass="clsValidationSummary" HeaderText="Fill Up The Following Information"></asp:validationsummary><asp:customvalidator id="cvAuditCategory" runat="server" CssClass="clslabelauto" OnServerValidate="CustomValidate"
										Display="None" ErrorMessage="Select Audit Category" ControlToValidate="cmbAuditCategory"></asp:customvalidator><asp:requiredfieldvalidator id="rfvCode" runat="server" CssClass="clslabelauto" Display="None" ErrorMessage="Enter Code"
										ControlToValidate="txtCode"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvDescription" runat="server" CssClass="clslabelauto" Display="None" ErrorMessage="Enter Description"
										ControlToValidate="txtDescription"></asp:requiredfieldvalidator>
									<asp:customvalidator id="cvNote" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
										Display="None" ErrorMessage="Note should not be greater than1000 characters." ControlToValidate="txtNote"></asp:customvalidator>
									<asp:customvalidator id="cvDescription" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDescription"
										ErrorMessage="Description should not be greater than 1000 characters." Display="None" OnServerValidate="customvalidate"></asp:customvalidator></TD>
							</TR>
							<TR>
								<TD colSpan="3"><asp:label id="lblOtherChargeDetails" runat="server" CssClass="clsLabelHeader">Audit Schedule Task Details</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="3">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" border="0">
										<TR>
											<TD><asp:label id="lblChargeNameStar1" runat="server" CssClass="clsLabelStar">*</asp:label></TD>
											<TD><asp:label id="lblAuditCategory" runat="server" CssClass="clsLabelAuto"> Task Category</asp:label></TD>
											<TD>
												<TABLE id="Table3" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD>
															<asp:dropdownlist id=cmbAuditCategory runat="server" CssClass="clsComboBox" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mAuditSchedule.AuditScheduleTasks.CurrentItem.AuditCategoryID %>">
															</asp:dropdownlist></TD>
														<TD>
															<asp:button id="imgbtnAuditCategory" runat="server" CssClass="clsButtonGrid" Text="..." CausesValidation="False"
																ToolTip="Click to Add New Charge"></asp:button></TD>
													</TR>
												</TABLE>
											</TD>
											<TD>
												<asp:label id="Label3" runat="server" CssClass="clsLabelStar">*</asp:label></TD>
											<TD><asp:label id="lblCode" runat="server" CssClass="clsLabel">Code </asp:label></TD>
											<TD>
												<TABLE id="Table4" cellSpacing="1" cellPadding="1" border="0">
													<TR>
														<TD><asp:textbox id=txtCode runat="server" CssClass="clsTextBox1" Text="<%# mAuditSchedule.AuditScheduleTasks.CurrentItem.Code %>" MaxLength="100" BackColor="White">
															</asp:textbox></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD><asp:label id="Label5" runat="server" CssClass="clsLabelStar">*</asp:label></TD>
											<TD><asp:label id="lblDescription" runat="server" CssClass="clsLabelAuto"> Description </asp:label></TD>
											<TD colSpan="4"><asp:textbox id=txtDescription runat="server" CssClass="clsTextBoxMultilineDefectAction" Text="<%# mAuditSchedule.AuditScheduleTasks.CurrentItem.Description %>" MaxLength="5000" BackColor="White" TextMode="MultiLine">
												</asp:textbox></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD><asp:label id="lblNote" runat="server" CssClass="clsLabelAuto">Note</asp:label></TD>
											<TD colSpan="4"><asp:textbox id=txtNote runat="server" CssClass="clsTextBoxMultilineDefectAction" ToolTip="Enter Note" Text="<%# mAuditSchedule.AuditScheduleTasks.CurrentItem.Note %>" MaxLength="1000" TextMode="MultiLine"></asp:textbox></TD>
										</TR>
									</TABLE>
									.</TD>
							</TR>
							<TR>
								<TD align="right"></TD>
								<TD align="right" colSpan="3">
									<TABLE id="Table1">
										<TR>
											<TD><asp:button id="btnOk" runat="server" CssClass="clsButton" ToolTip="Click to Save Audit Schedule Task "
													Text="Save"></asp:button></TD>
											<TD><asp:button id="btnBack" runat="server" CssClass="clsButton" ToolTip="Click to go back to the previous page"
													CausesValidation="False" Text="Close"></asp:button></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

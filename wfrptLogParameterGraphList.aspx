<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptLogParameterGraphList.aspx.vb" Inherits="Flypal.wfrptLogParameterGraphList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Log Parameter Graph</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
		<script language="javascript" id="clientEventHandlersJS">
			function openTranDetail()
			{
				str = "wfReports.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openTranDetail1()
			{
				str = "webform1.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openDetail()
			{
				str = "wfDetail.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<p></p>
			<table class="clstablelistout" id="tblmain" border="0">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE class="clstablelistin" id="tblInner" border="0">
								<TR>
									<TD colSpan="5">
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Log Parameter Graph Report</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvFromDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFromDate"
											Display="None" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator id="rfvToDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtToDate"
											Display="None" ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
										<asp:CustomValidator id="cvaircraft" runat="server" CssClass="clslabelauto" ControlToValidate="cmbAircraft"
											Display="None" ErrorMessage="CustomValidator" OnServerValidate="customvalidate"></asp:CustomValidator>
										<asp:CustomValidator id="cvAssembly" runat="server" CssClass="clslabelauto" ControlToValidate="cmbAssembly"
											Display="None" ErrorMessage="CustomValidator" OnServerValidate="customvalidate"></asp:CustomValidator></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left">
										<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label></TD>
									<TD align="left">
										<uc1:sicalendar id="txtFromDate" runat="server"></uc1:sicalendar></TD>
									<TD align="left">
										<asp:Label id="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label></TD>
									<TD align="left">
										<uc1:sicalendar id="txtToDate" runat="server"></uc1:sicalendar></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblCurrencyStar1" runat="server" CssClass="clsLabelStar">*</asp:label></TD>
									<TD align="left">
										<asp:Label id="lblDocTypeNo" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label></TD>
									<TD align="left" colSpan="3">
										<asp:DropDownList id="cmbAircraft" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="ID"
											DataTextField="RegNo"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="Label2" runat="server" CssClass="clsLabelStar">*</asp:label></TD>
									<TD align="left">
										<asp:Label id="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label></TD>
									<TD align="left" colSpan="3">
										<asp:DropDownList id="cmbAssembly" runat="server" CssClass="clsComboBoxLong" AutoPostBack="True" DataValueField="ID"
											DataTextField="ModelSerialNo" Enabled="False"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblSelectParameter" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Parameter</asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left">
										<asp:Label id="lblParamater1" runat="server" CssClass="clsLabelAuto" Visible="False">Parameter</asp:Label></TD>
									<TD align="left" colSpan="3">
										<asp:CheckBoxList id="cmbParameter" runat="server" CssClass="clsCheckBox" DataValueField="ParameterId"
											DataTextField="ParameterName"></asp:CheckBoxList></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left"></TD>
									<TD align="left" colSpan="3">
										<TABLE id="Table5" border="0">
											<TR>
												<TD>
													<asp:DropDownList id="cmbParameter1" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="ParameterId"
														DataTextField="ParameterName" Enabled="False" Visible="False"></asp:DropDownList></TD>
												<TD>
													<asp:TextBox id="txtDescription" runat="server" CssClass="clsTextBox" Visible="False" ReadOnly="True"
														BackColor="#E0E0E0" ToolTip="Description"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left">
										<asp:Label id="lblMin" runat="server" CssClass="clsLabelAuto" Visible="False">Min</asp:Label></TD>
									<TD align="left">
										<asp:textbox id="txtMin" runat="server" CssClass="clsTextBoxRightAlignSmall1" Visible="False"
											MaxLength="4"></asp:textbox>
										<asp:customvalidator id="cvMin" runat="server" CssClass="clslabelauto" ControlToValidate="txtMin" Display="None"
											OnServerValidate="customvalidate" Visible="False"></asp:customvalidator></TD>
									<TD align="left">
										<asp:Label id="lblMax" runat="server" CssClass="clsLabelAuto" Visible="False">Max</asp:Label></TD>
									<TD align="left">
										<asp:textbox id="txtMax" runat="server" CssClass="clsTextBoxRightAlignSmall1" Visible="False"
											MaxLength="4"></asp:textbox>
										<asp:customvalidator id="cvMax" runat="server" CssClass="clslabelauto" ControlToValidate="txtMax" Display="None"
											OnServerValidate="customvalidate" Visible="False"></asp:customvalidator></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStepIV" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows :</asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblAircraft" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left">
										<asp:label id="lblParameter" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left"></TD>
									<TD align="left" colSpan="2">
										<asp:label id="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD align="left">
										<asp:label id="lblMinValue" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left"></TD>
									<TD align="left" colSpan="2">
										<asp:label id="lblMaxValue" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="5">
										<asp:Panel id="pnlButton" Runat="server" CssClass="clspanel1">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button id="btnCurrentSearchCriteria" tabIndex="0" runat="server" CssClass="clsButtonlong"
															ToolTip="Click to display Current Searching criterias." Text="Current Criteria"></asp:button></TD>
													<TD>
														<asp:button id="btnDisplay" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Display Report"
															Text="Display"></asp:button></TD>
													<TD>
														<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to close Log Parameter Graph screen"
															Text="Close" CausesValidation="False"></asp:button></TD>
												</TR>
											</TABLE>
										</asp:Panel></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

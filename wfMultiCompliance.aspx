<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfMultiCompliance.aspx.vb" Inherits="Flypal.wfMultiCompliance" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Due Periodwise Report</title>
		<SCRIPT language="javascript">
			function openledgersame(FileName)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
		</SCRIPT>
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
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="4">
										<asp:label id="lbltitle" Runat="server" CssClass="clstitle1">Multi Compliance</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:ValidationSummary id="Validationsummary2" Runat="server" HeaderText="Fill Up The Following Fields"
											Cssclass="clsValidationSummary"></asp:ValidationSummary>
										<asp:customvalidator id="cvCustomer" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Aircraft from the list."
											Display="None" ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate"></asp:customvalidator>
										<asp:CustomValidator id="cvType" runat="server" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<TABLE class="clsTable1" id="Table7" cellSpacing="1" cellPadding="1" border="0">
											<TR>
												<TD vAlign="top" colSpan="2">
													<TABLE id="Table10" cellSpacing="1" cellPadding="1" border="0">
														<TR>
															<TD colSpan="3">
																<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Compliance Date</asp:label></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="lblFromDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:label></TD>
															<TD>
																<TABLE id="Table9" cellPadding="0">
																	<TR>
																		<TD></TD>
																		<TD style="HEIGHT: 24px" colSpan="1">
																			<uc1:SICalendar id="txtAsOnDate" runat="server"></uc1:SICalendar></TD>
																	</TR>
																</TABLE>
															</TD>
														</TR>
														<TR>
															<TD colSpan="2">
																<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:label></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:label></TD>
															<TD>
																<asp:dropdownlist id="cmbAircraft" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataTextField="RegNo"
																	DataValueField="MachineID"></asp:dropdownlist></TD>
														</TR>
														<TR>
															<TD colSpan="2">
																<asp:label id="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:label></TD>
														</TR>
														<TR>
															<TD>
																<asp:Label id="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label></TD>
															<TD>
																<asp:DropDownList id="cmbAssembly" runat="server" CssClass="clsComboBox3" AutoPostBack="True" DataTextField="Description"
																	DataValueField="AssemblyID"></asp:DropDownList></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<TABLE class="clsTable1" id="Table6" cellPadding="0" DESIGNTIMEDRAGDROP="427">
											<TR>
												<TD colSpan="2">
													<asp:LinkButton id="lbtnAdvancedSearch" runat="server" CssClass="clsLabelAuto" CausesValidation="False">Advanced Search</asp:LinkButton></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:label id="lblStep4" runat="server" CssClass="clsLabelHeader" Width="192px" Visible="False">Step IV. Selection of Type</asp:label></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:Panel id="pnlAdvancedSearch" runat="server">
														<TABLE class="clstablelistin" id="Table1" width="300" border="0">
															<TR>
																<TD>
																	<asp:Label id="lblTypeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
																<TD>
																	<asp:Label id="lblType" runat="server" CssClass="clsLabelAuto">Type</asp:Label>&nbsp;</TD>
																<TD>
																	<asp:Panel id="pnlcmbType" runat="server" CssClass="clsPanel1">
																		<asp:CheckBoxList id="cmbType" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataTextField="Name"
																			DataValueField="ID"></asp:CheckBoxList>
																	</asp:Panel>&nbsp;</TD>
																<TD>
																	<asp:Panel id="pnlServiceType" runat="server" CssClass="clsPanel1">&nbsp; 
                        <TABLE class="clstablelistin" id="Table3" width="300" border="0">
																			<TR>
																				<TD style="WIDTH: 105px">
																					<P>
																						<asp:Label id="lblServiceType" runat="server" CssClass="clsLabel">Service Type</asp:Label></P>
																				</TD>
																				<TD>
																					<asp:CheckBoxList id="cmbServiceType" runat="server" CssClass="clsComboBox" DataTextField="CodeType"
																						DataValueField="ID"></asp:CheckBoxList></TD>
																			</TR>
																		</TABLE></asp:Panel>
																	<asp:Panel id="pnlModificationType" runat="server" CssClass="clsPanel1">
																		<TABLE class="clstablelistin" id="Table4" width="300" border="0">
																			<TR>
																				<TD style="WIDTH: 107px">
																					<P>
																						<asp:Label id="lblInspectionType" runat="server" CssClass="clsLabel">Inspection Type</asp:Label></P>
																				</TD>
																				<TD>
																					<asp:CheckBoxList id="cmbInspectionType" runat="server" CssClass="clsComboBox" DataTextField="CodeType"
																						DataValueField="ID"></asp:CheckBoxList></TD>
																			</TR>
																		</TABLE>
																	</asp:Panel>
																	<asp:Panel id="pnlInspectionType" runat="server" CssClass="clsPanel1">
																		<TABLE class="clstablelistin" id="Table5" width="300" border="0">
																			<TR>
																				<TD style="WIDTH: 107px">
																					<P>
																						<asp:Label id="lblModificationType" runat="server" CssClass="clsLabel" Width="104px">Directive Type</asp:Label></P>
																				</TD>
																				<TD>
																					<asp:CheckBoxList id="cmbModificationType" runat="server" CssClass="clsComboBox" DataTextField="CodeType"
																						DataValueField="ID"></asp:CheckBoxList></TD>
																			</TR>
																		</TABLE>
																	</asp:Panel></TD>
															</TR>
														</TABLE>
													</asp:Panel></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:label id="lblStep5" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Due Limits</asp:label></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:panel id="Panel1" Runat="server" CssClass="clsPanelWidth">
														<asp:datagrid id="dgDuePeriodLimits" runat="server" Cssclass="clsGrid" AutoGenerateColumns="False">
															<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
															<ItemStyle CssClass="clsdgItem"></ItemStyle>
															<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
															<Columns>
																<asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
																<asp:TemplateColumn HeaderText="Limit">
																	<ItemTemplate>
																		<asp:TextBox id=txtLimit runat="server" CssClass="clsTextBoxRightAlign" Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value." BackColor="White">
																		</asp:TextBox>
																		<asp:CustomValidator id="cvPeriodLimitsValue" runat="server" Display="None" ControlToValidate="txtLimit"
																			ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:datagrid>
													</asp:panel></TD>
											</TR>
											<TR>
												<TD vAlign="top">
													<TABLE id="Table8" cellSpacing="0">
														<TR>
															<TD vAlign="top">
																<asp:label id="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On Values</asp:label></TD>
														</TR>
														<TR>
															<TD vAlign="top">
																<asp:datagrid id="dgDoneOnValue" runat="server" Cssclass="clsGrid" AutoGenerateColumns="False"
																	PageSize="3">
																	<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
																	<ItemStyle CssClass="clsdgItem"></ItemStyle>
																	<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
																	<Columns>
																		<asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
																		<asp:BoundColumn DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundColumn>
																	</Columns>
																	<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
																</asp:datagrid></TD>
														</TR>
													</TABLE>
												</TD>
												<TD vAlign="top" align="right">
													<TABLE id="Table2" cellSpacing="0">
														<TR>
															<TD>
																<asp:button id="btnSelectLog" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to select the log"
																	Text="Select Log"></asp:button></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD align="right" colSpan="2">
													<TABLE cellSpacing="0">
														<TR>
															<TD>
																<asp:button id="btnFindNow" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to find"
																	Text="Find Now"></asp:button></TD>
															<TD>
																<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
																	ToolTip="Back to Previous Page" Text="Close"></asp:button></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
			</TABLE></form>
	</body>
</HTML>

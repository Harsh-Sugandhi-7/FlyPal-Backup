<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfMultiComplanceList.aspx.vb" Inherits="Flypal.wfMultiComplanceList" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Multi Compliance List</title>
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
		<LINK href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
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
										<TABLE class="clsTable1" id="Table6" cellPadding="0" DESIGNTIMEDRAGDROP="427">
											<TR>
												<TD vAlign="top">
													<TABLE id="Table2" cellSpacing="0">
														<TR>
															<TD>
																<asp:label id="lblComplianceDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:label></TD>
															<TD vAlign="top">
																<uc1:SICalendar id="txtAsOnDate" runat="server"></uc1:SICalendar></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:label></TD>
															<TD vAlign="top">
																<asp:TextBox id="txtAircraft" runat="server" CssClass="clsTextBox" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD>
																<asp:Label id="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label></TD>
															<TD>
																<asp:TextBox id="txtAssembly" runat="server" CssClass="clsTextBox" BackColor="#E0E0E0" ReadOnly="True"
																	Width="184px" Height="24px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD>
																<asp:Label id="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label></TD>
															<TD>
																<asp:TextBox id="txtWorkOrderNo" runat="server" CssClass="clsTextBox"></asp:TextBox></TD>
														</TR>
													</TABLE>
												</TD>
												<TD vAlign="top" align="right">
													<TABLE id="Table1" cellSpacing="0">
														<TR>
															<TD vAlign="top">
																<asp:label id="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On Values</asp:label></TD>
														</TR>
														<TR>
															<TD vAlign="top">
																<asp:datagrid id="dgDoneOnValue" runat="server" PageSize="3" AutoGenerateColumns="False" Cssclass="clsGrid">
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
											</TR>
											<TR>
												<TD>
													<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
												<TD align="right">
													<TABLE id="Table11" cellSpacing="0">
														<TR>
															<TD>
																<asp:button id="btnSaveTop" runat="server" CssClass="clsButton" ToolTip="Click to save" Text="Comply"></asp:button></TD>
															<TD>
																<asp:button id="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
																	Text="Close" CausesValidation="False"></asp:button></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<asp:datagrid id="dgDueJob" runat="server" CssClass="clsGrid" AutoGenerateColumns="False" ToolTip="Due Jobs">
														<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
														<ItemStyle CssClass="clsdgItem"></ItemStyle>
														<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Select">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
															<asp:BoundColumn DataField="MaintenanceOn" HeaderText="Maintenance On"></asp:BoundColumn>
															<asp:BoundColumn DataField="MaintenanceInfo" HeaderText="Maintenance Information"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" HeaderText="Things to do"></asp:BoundColumn>
															<asp:BoundColumn DataField="Frequency" HeaderText="Frequency">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="SinceNewAll" HeaderText="Since New">
																<HeaderStyle Wrap="False"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="ElapsedAll" HeaderText="Elapsed">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="DoneAtAll" HeaderText="Done At">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="ExtensionAll" HeaderText="Extension">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="DueAsofAll" HeaderText="Due At">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="AssDueAsofAll" HeaderText="Due At Assembly">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="RemainingTimeAll" HeaderText="Remaining">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="EstimatedDate" HeaderText="Estimated Date">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Comply Remark">
																<ItemTemplate>
																	<asp:textbox id="txtRemark" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="200" TextMode="MultiLine"></asp:textbox>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid></TD>
											</TR>
											<TR>
												<TD align="right" colSpan="2">
													<TABLE cellSpacing="0">
														<TR>
															<TD>
																<asp:button id="btnSave" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to save"
																	Text="Comply"></asp:button></TD>
															<TD>
																<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
																	Text="Close" CausesValidation="False"></asp:button></TD>
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

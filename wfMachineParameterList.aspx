<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfMachineParameterList.aspx.vb" Inherits="Flypal.wfMachineParameterList" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Aircraft Parameter List</title>
		<meta content="True" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="clstablelistout" id="tblMain">
				<tr>
					<td class="clstablecell"><asp:panel id="pnlMain" Runat="server" Cssclass="clspnl1">
							<TABLE class="clsTablelistin" id="tblinner">
								<TBODY>
									<TR>
										<TD colSpan="4"><asp:label id="lblTitle" runat="server" Cssclass="clstitle1">List of Parameters</asp:label></TD>
									</TR>
									<TR>
										<TD colSpan="4"><asp:validationsummary id="Validationsummary2" Runat="server" Cssclass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:validationsummary>
											<asp:CustomValidator id="cvParameterList" runat="server" OnServerValidate="customvalidate" Display="None"
												ControlToValidate="cmbParameterList" ErrorMessage="Select Parameters form List."></asp:CustomValidator></TD>
									</TR>
									<TR>
										<TD colSpan="4">
											<TABLE id="Table2" cellSpacing="0" border="0">
												<TR>
													<TD><asp:button id="btnAirCraftStatus" tabIndex="0" runat="server" Text="Aircraft Status" CausesValidation="False"
															ToolTip="Click to Add the Status" CssClass="clsButtonLong1"></asp:button></TD>
													<TD><asp:button id="btnAssemblyList" tabIndex="0" runat="server" Text="Assembly List" CausesValidation="False"
															ToolTip="Click to Add Assembly" CssClass="clsButtonLong1"></asp:button></TD>
													<TD><asp:label id="lblParameterList" runat="server" ToolTip="Current page of Parameter Status List "
															CssClass="clsLabelButton1">Parameter List</asp:label></TD>
													<TD>
														<asp:button id="btnTankList" tabIndex="0" runat="server" CssClass="clsButtonLong1" ToolTip="Click to open Tank List page"
															CausesValidation="False" Text="Tank List"></asp:button></TD>
													<TD>
														<asp:Button id="btnFeatureList" Runat="server" ToolTip="Click to open the Feature List page"
															CssClass="clsButtonLong1" Text="Feature List"></asp:Button></TD>
													<TD>
														<asp:Button id="btnCertificateList" Runat="server" ToolTip="Click to open Certificate List page"
															CssClass="clsButtonLong1" Text="Certificate List"></asp:Button></TD>
													<TD>
														<asp:Button id="btnMELList" Runat="server" CssClass="clsButtonLong1" ToolTip="Click to open the MEL List page"
															Text="MEL List" EnableViewState="False"></asp:Button></TD>
													<TD>
														<asp:Button id="btnBoardInfo" Runat="server" CssClass="clsButtonLong1" ToolTip="Click to open the Board Info List page"
															Text="Board Info List" EnableViewState="False" CausesValidation="False"></asp:Button></TD>
												</TR>
											</TABLE>
										</TD>
									</TR>
									<TR>
										<TD colSpan="4"><asp:label id="lblParameterListInfo" runat="server" CssClass="clsLabelHeader">Aircraft Parameter Details</asp:label></TD>
									</TR>
									<tr>
										<td><table>
												<tr>
													<TD><asp:label id="Label1" runat="server" CssClass="clsLabelAuto">Parameter</asp:label></TD>
													<TD><asp:dropdownlist id="cmbParameterList" runat="server" CssClass="clsComboBox" DataValueField="Id"
															DataTextField="Name"></asp:dropdownlist></TD>
													<TD>
														<asp:Button id="imgbtnParameter" runat="server" CssClass="clsButtonGrid" ToolTip="Click to add new Parameter"
															CausesValidation="False" Text="..."></asp:Button></TD>
												</tr>
											</table>
										</td>
										<TD align="right"><asp:button id="btnAdd" tabIndex="0" runat="server" Text="Add" ToolTip="Click to add parameter in the List"
												CssClass="clsButton"></asp:button></TD>
									</tr>
					</td>
				</tr>
				<TR>
					<TD colSpan="4"><asp:label id="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Parameter Details</asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="4"><asp:datagrid id="dgParameterList" runat="server" Cssclass="clsGrid" ToolTip="Aircraft Parameter List."
							PageSize="3" AutoGenerateColumns="False" AllowSorting="True">
							<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
							<ItemStyle CssClass="clsdgItem"></ItemStyle>
							<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="ParameterName" SortExpression="ParameterName" HeaderText="Parameter Name">
									<HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ParameterDescription" SortExpression="ParameterDescription" HeaderText="Parameter Description ">
									<HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
							</Columns>
							<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="4">
						<TABLE id="Table1" cellSpacing="0">
							<TR>
								<TD><asp:button id="btnPrint" tabIndex="0" runat="server" Text="Print" CausesValidation="False"
										ToolTip="Click to Print the list of Parameters" CssClass="clsButton" Visible="False"></asp:button></TD>
								<TD>
									<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to go Previous page"
										CausesValidation="False" Text="Back"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			</asp:panel></TD></TR></TBODY></TABLE></form>
	</body>
</HTML>

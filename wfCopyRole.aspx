<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfCopyRole.aspx.vb" Inherits="Flypal.wfCopyRole" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Role</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain" style="WIDTH: 535px; HEIGHT: 744px">
				<tr>
					<td colSpan="3"><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server" Height="722px">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="3">
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Role</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvr" runat="server" CssClass="clsLabel" Width="40px" ControlToValidate="txtRoleName"
											Display="None" ErrorMessage="Role Name Required"></asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblRole" runat="server" CssClass="clsLabel">Role</asp:label></TD>
									<TD>
										<asp:TextBox id=txtRoleName runat="server" CssClass="clsTextBox" MaxLength="50" ToolTip="Enter Role" Text="<%# mRole.Name %>">
										</asp:TextBox></TD>
									<TD align="right">
										<TABLE id="Table3" style="WIDTH: 90px; HEIGHT: 24px" cellPadding="1" width="90" border="0">
											<TR>
												<TD>
													<asp:Button id="btnNew" runat="server" CssClass="clsButton" ToolTip="Click to create new Role"
														Text="New" CausesValidation="False"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblNote" runat="server" CssClass="clsLabelHeader">Select the permission below for this Role</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lbllistPermission" runat="server" CssClass="clsLabelHeader">List of Permission</asp:label></TD>
									<TD align="right">
										<TABLE id="Table2" style="WIDTH: 188px; HEIGHT: 24px" cellPadding="1" border="0">
											<TR>
												<TD>
													<asp:Button id="btnSav" runat="server" CssClass="clsButton" ToolTip="Click to save the current record"
														Text="Save"></asp:Button></TD>
												<TD>
													<asp:Button id="btnClose" Runat="server" CssClass="clsButton" ToolTip="Click to close Role Information screen"
														Text="Close" CausesValidation="False"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblMaster" runat="server" CssClass="clsLabelHeader">Master/Entry Permission </asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgEntry" runat="server" CssClass="clsGrid" ToolTip="Select Rights from Master Permission"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ModuleName" HeaderText="Master-Entry Modules"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="View">
													<ItemTemplate>
														<asp:CheckBox id=ChkView runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Print">
													<ItemTemplate>
														<asp:CheckBox id=ChkPrint runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Add">
													<ItemTemplate>
														<asp:CheckBox id=ChkAdd runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Edit">
													<ItemTemplate>
														<asp:CheckBox id=ChkEdit runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Delete">
													<ItemTemplate>
														<asp:CheckBox id=ChkDelete runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblRoleModule" runat="server" CssClass="clsLabelHeader">Reports/Other Module Permissions </asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgReport" runat="server" CssClass="clsGrid" ToolTip="Select Rights from Reports Permisssions"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ModuleName" HeaderText="Report &amp; Other Modules"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="View">
													<ItemTemplate>
														<asp:CheckBox id=ChkView runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="3">
										<TABLE id="Table1" border="0">
											<TR>
												<TD>
													<asp:Button id="btnSave1" runat="server" CssClass="clsButton" ToolTip="Click to save the current record"
														Text="Save"></asp:Button></TD>
												<TD>
													<asp:Button id="btnClose1" runat="server" CssClass="clsButton" ToolTip="Click to close Role Information screen"
														Text="Close" CausesValidation="False"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

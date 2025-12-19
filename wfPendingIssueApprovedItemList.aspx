<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfPendingIssueApprovedItemList.aspx.vb" Inherits="Flypal.wfPendingIssueApprovedItemList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Existing Files</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="3">
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Pending Issue Approved Item List</asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblPartNumber" runat="server" CssClass="clsLabel">Part No.</asp:label></TD>
									<TD>
										<asp:TextBox id="txtPartNumber" runat="server" CssClass="clsTextBox1" MaxLength="50"></asp:TextBox></TD>
									<TD align="right" colSpan="1">
										<asp:Button id="btnFindNow" Runat="server" CssClass="clsButton" ToolTip="Click to Find the Part"
											Text="Find Now" CausesValidation="False"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Click on Check box to Select Part Information or Click on Close button to close the screen.</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgPendingIssueApprovedItemList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Select">
													<ItemTemplate>
														<asp:CheckBox id=chkSelect Runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="SrNo" HeaderText="Sr.No."></asp:BoundColumn>
												<asp:BoundColumn DataField="ItemName" HeaderText="Part No.">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ItemDescription" HeaderText="Description"></asp:BoundColumn>
												<asp:BoundColumn DataField="DateFormatted" HeaderText="Requisition Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="RequisitionNo" HeaderText="Requisition No.">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="IssueBalQty" HeaderText="Issue Bal. Qty.">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="2">
										<asp:button id="btnOk" runat="server" ToolTip="Click to Add Selected Parts In the List" Text="Ok"
											Cssclass="clsButton"></asp:button></TD>
									<TD align="right">
										<asp:Button id="btnClose" tabIndex="0" Runat="server" CssClass="clsButton" ToolTip="Click to go back to the Previous page"
											Text="Close" CausesValidation="False"></asp:Button></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfMgtApprovedRequisitionItems.aspx.vb" Inherits="Flypal.wfMgtApprovedRequisitionItems" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Approve Quotation Item</title>
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
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Finance aprroved requisition part list</asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblPartNumber" runat="server" CssClass="clsLabel">Part Number</asp:label></TD>
									<TD>
										<asp:TextBox id="txtPartNumber" runat="server" CssClass="clsTextBox1" MaxLength="50"></asp:TextBox></TD>
									<TD colSpan="1">
										<asp:Button id="btnFindNow" Runat="server" CssClass="clsButton" ToolTip="Click to find the list of records as per searching criteria."
											Text="Find Now" CausesValidation="False"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Click on Select link to Select Part Information or on Close to Close the application respectively.</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgOrderItemList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Select">
													<ItemTemplate>
														<asp:CheckBox id=chkSelect Runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>' AutoPostBack="True">
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="ItemName" HeaderText="Part No.">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ItemDescription" HeaderText="Description"></asp:BoundColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="2">
										<asp:button id="btnOk" runat="server" ToolTip="Click to go back to the previous page" Text="Ok"
											Cssclass="clsButton"></asp:button></TD>
									<TD>
										<asp:Button id="btnClose" tabIndex="0" Runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
											Text="Close" CausesValidation="False"></asp:Button></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

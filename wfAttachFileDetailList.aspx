<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAttachFileDetailList.aspx.vb" Inherits="Flypal.wfAttachFileDetailList" %>
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
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="3">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">Existing Files</asp:Label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 73px">
										<asp:label id="lblName" runat="server" CssClass="clsLabelAuto">Name </asp:label></TD>
									<TD>
										<TABLE id="Table4">
											<TR>
												<TD>
													<asp:TextBox id="txtSearch" runat="server" CssClass="clsTextBox" MaxLength="50" ToolTip="Enter File Name"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="right" colSpan="1">
										<asp:Button id="btnFindNow" CssClass="clsButton" Runat="server" ToolTip="Click to find the list of records as per searching criteria."
											CausesValidation="False" Text="Find Now"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:datagrid id="dgAttachFileList" runat="server" CssClass="clsGrid" ToolTip="Existing Files List"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
												<asp:BoundColumn DataField="DocumentTypeName" HeaderText="Document Type"></asp:BoundColumn>
												<asp:BoundColumn DataField="Remark" HeaderText="Remark"></asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
									<TD align="right" colSpan="1">
										<TABLE id="Table1" height="100%" cellSpacing="0" cellPadding="0" align="right" border="0">
											<TR>
												<TD vAlign="top" align="right"></TD>
											</TR>
											<TR>
												<TD></TD>
											</TR>
											<TR>
												<TD vAlign="bottom" align="right">
													<asp:Button id="btnClose" tabIndex="0" CssClass="clsButton" Runat="server" ToolTip="Back to Previous Page"
														CausesValidation="False" Text="Close"></asp:Button></TD>
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

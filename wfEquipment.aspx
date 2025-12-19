<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfEquipment.aspx.vb" EnableViewState="false" Inherits="Flypal.wfEquipment" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Equipment</title>
		<script language="javascript">
                function OpenLocation(FileName)
                   {
                     window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 
                   }
		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain" style="Z-INDEX: 102; POSITION: absolute; TOP: 0px; LEFT: 2px">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="4">
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Equipment [New]</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:RequiredFieldValidator id="rfvEquipment" runat="server" CssClass="clsLabelAuto" ErrorMessage="Equipment required"
											ControlToValidate="txtEquipment" Display="Dynamic"></asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:label></TD>
									<TD align="right">
										<asp:button id="btnNew" tabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
											ToolTip="Click to add the new Equipment" Text="New"></asp:button></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblEquipment" runat="server" CssClass="clsLabel">Equipment</asp:label></TD>
									<TD>
										<asp:TextBox id=txtEquipment runat="server" CssClass="clstextBox" ToolTip="Enter equipment name" Text="<%# mEquipment.Name %>" MaxLength="50">
										</asp:TextBox></TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:label></TD>
									<TD align="right">
										<asp:Button id="btnSave" Runat="server" CssClass="clsButton" ToolTip="Click to save the Equipment"
											Text="Save"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblSearch" runat="server" CssClass="clsLabelHeader">Equipment List</asp:label></TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgEquipmentList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
											EnableViewState="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="DepartmentID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Equipment"></asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
									<TD>
										<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" align="right" height="100%">
											<TR>
												<TD vAlign="top" align="right">
													<asp:button id="btnBackTop" runat="server" CssClass="clsButton" CausesValidation="False" ToolTip="Click to close Equipment"
														Text="Close"></asp:button></TD>
											</TR>
											<TR>
												<TD align="right"></TD>
											</TR>
											<TR>
												<TD vAlign="bottom" align="right">
													<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
														ToolTip="Click to close Equipment" Text="Close"></asp:button></TD>
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

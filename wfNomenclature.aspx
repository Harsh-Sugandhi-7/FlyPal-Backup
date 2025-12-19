<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfNomenclature.aspx.vb" Inherits="Flypal.wfNomenclature" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Nomenclature</title>
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
	<body bottomMargin="5" MS_POSITIONING="GridLayout" leftMargin="5" topMargin="5" rightMargin="5">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="4">
										<asp:Label id="lblTitle" CssClass="clstitle1" Runat="server">Nomenclature [New]</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:Panel id="pnlError" CssClass="clspanel1" Runat="server"></asp:Panel>
										<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="clsValidationSummary" Height="40px"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required ."
											ControlToValidate="txtName" Display="None">Name Required.</asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:label></TD>
									<TD align="right">
										<asp:button id="btnAdd" runat="server" CssClass="clsButton" CausesValidation="False" ToolTip="Click to add new Place"
											Text="New"></asp:button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblNomenclatureDetails" runat="server" CssClass="clsLabelHeader">Nomenclature Details</asp:label></TD>
								</TR>
								<TR>
									<TD align="right">
										<asp:Label id="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblName" runat="server" CssClass="clsLabel">Name </asp:label></TD>
									<TD>
										<asp:TextBox id=txtName runat="server" CssClass="clstextBox" ToolTip="Enter Name" Text="<%# mNomenclature.Name %>" MaxLength="50">
										</asp:TextBox></TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:label></TD>
									<TD align="right">
										<asp:Button id="btnSave" CssClass="clsButton" Runat="server" ToolTip="Click to Save Nomenclature Information"
											Text="Save"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgNomenclature" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="Nomenclature ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Nomenclature"></asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
									<TD>
										<TABLE id="Table2" height="100%" cellSpacing="0" cellPadding="0" align="right" border="0">
											<TR>
												<TD vAlign="top" align="right">
													<asp:button id=btnBacktop runat="server" CssClass="clsButton" CausesValidation="False" ToolTip="Click to close Nomenclature screen" Text="Close" Visible="<%# mNomenclatureList.Count >25 %>">
													</asp:button></TD>
											</TR>
											<TR>
												<TD></TD>
											</TR>
											<TR>
												<TD vAlign="bottom" align="right" colSpan="3">
													<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" CausesValidation="False"
														ToolTip="Click to close Nomenclature screen" Text="Close"></asp:button></TD>
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

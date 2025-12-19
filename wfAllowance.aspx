<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAllowance.aspx.vb" Inherits="Flypal.wfAllowance" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Allowance</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
	</HEAD>
	<body bottomMargin="5" leftMargin="5" rightMargin="5" topMargin="5" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="clstablelistout" id="tblmain">
				<TR>
					<TD><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="5">
										<asp:Label id="lblTitle" tabIndex="1" CssClass="clstitle1" Runat="server">Allowance Information [New]</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Allowance Required"
											Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
										<asp:CustomValidator id="cvAllowance" runat="server" CssClass="clsLabelAuto" ErrorMessage="Allowance Name too Long."
											Display="None" ControlToValidate="txtName" OnServerValidate="customvalidate"></asp:CustomValidator>
										<asp:RequiredFieldValidator  id="rfvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Allowance Code Required"
											Display="None" ControlToValidate="txtCode"></asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:label></TD>
									<TD align="right">
										<asp:Button id="btnNew" CssClass="clsButton" Runat="server" Text="New" ToolTip="Click to Add the Allowance"
											CausesValidation="False"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblAllowanceDetails" runat="server" CssClass="clsLabelHeader">Allowance Details</asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="middle" align="center">
										<asp:Label  id="Label1" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label></TD>
									<TD vAlign="middle"></TD>
									<TD>
										<asp:label  id="Label3" runat="server" CssClass="clslabelAuto">Code</asp:label>&nbsp;
										<asp:TextBox  id=txtCode runat="server" CssClass="clsTextBoxsmall" Text="<%# mAllowance.Code %>" ToolTip="Enter Allowance Code" MaxLength="5">
										</asp:TextBox></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD vAlign="middle" align="center">
										<asp:Label  id="Label2" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label></TD>
									<TD vAlign="middle"></TD>
									<TD>
										<asp:label  id="lblName" runat="server" CssClass="clslabelAuto">Name</asp:label>&nbsp;
										<asp:TextBox  id=txtName runat="server" CssClass="clsTextBox" Text="<%# mAllowance.Name %>" ToolTip="Enter Allowance Name" MaxLength="50">
										</asp:TextBox></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:label></TD>
									<TD align="right">
										<asp:Button id="btnSave" CssClass="clsButton" Runat="server" Text="Save" ToolTip="Click to Save Allowance Information"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblSearch" runat="server" CssClass="clsLabelHeader">Allowance List</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgAllowance" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Code" HeaderText="Code"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
									<TD align="left">
										<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" align="right" height="100%">
											<TR>
												<TD vAlign="top" align="right">
													<asp:button id="btnBackTop" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Allowance Information screen"
														CausesValidation="False"></asp:button></TD>
											</TR>
											<TR>
												<TD></TD>
											</TR>
											<TR>
												<TD vAlign="bottom" align="right">
													<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Allowance Information screen"
														CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></TD>
				</TR>
			</TABLE>
			&nbsp;
		</form>
	</body>
</HTML>

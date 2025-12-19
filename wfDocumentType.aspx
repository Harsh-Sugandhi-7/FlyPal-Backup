<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfDocumentType.aspx.vb" Inherits="Flypal.wfDocumentType" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Document Type</title>
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
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="4">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">Document Type [New]</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>&nbsp;
									</TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:label></TD>
									<TD align="right">
										<asp:button id="btnAdd" runat="server" CssClass="clsButton" Text="New" ToolTip="Click to add the new Document"
											CausesValidation="False"></asp:button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblCityDetails" runat="server" CssClass="clsLabelHeader">Document Type Details</asp:label></TD>
								</TR>
								<TR>
									<TD align="right">
										<asp:Label id="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label></TD>
									<TD>
										<asp:label id="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:label></TD>
									<TD>
										<TABLE id="Table2">
											<TR>
												<TD>
													<asp:TextBox id=txtName runat="server" CssClass="clsTextBox" Text="<%# mDocumentType.Name %>" ToolTip="Enter Document Type Name" MaxLength="50">
													</asp:TextBox></TD>
											</TR>
										</TABLE>
										<asp:RequiredFieldValidator id="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
											Display="None" ErrorMessage="Name Required"></asp:RequiredFieldValidator>
										<asp:customvalidator id="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName" Display="None"
											ErrorMessage="Name too Long." OnServerValidate="customvalidate"></asp:customvalidator></TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblDocumentTypeFor" runat="server" CssClass="clsLabelAuto" Width="144px">Document Type For</asp:label></TD>
									<TD>
										<TABLE id="Table5">
											<TR>
												<TD>
													<asp:DropDownList id=cmbDocumentTypeFor runat="server" CssClass="clsComboBox" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mDocumentType.DocumentTypeForID %>" Enabled="False">
													</asp:DropDownList></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="right"></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblGMT" runat="server" CssClass="clsLabelAuto">Code</asp:label></TD>
									<TD>
										<TABLE id="Table3">
											<TR>
												<TD>
													<asp:TextBox id=txtCode runat="server" CssClass="clsTextBoxSmall" Text="<%# mDocumentType.Code %>" ToolTip="Enter the Code" MaxLength="50">
													</asp:TextBox></TD>
											</TR>
										</TABLE>
										<asp:customvalidator id="cvCode" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtCode" Display="None"
											ErrorMessage="Code should not be greater than 4 characters." OnServerValidate="customvalidate"></asp:customvalidator></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:label id="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:label></TD>
									<TD align="right">
										<asp:Button id="btnSave" CssClass="clsButton" Runat="server" Text="Save" ToolTip="Click to save the Document Information"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblSearchByCity" runat="server" CssClass="clsLabelHeader">Search by Document Type</asp:label></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:label id="lblCityName" runat="server" CssClass="clsLabelAuto">Name </asp:label></TD>
									<TD>
										<TABLE id="Table4">
											<TR>
												<TD>
													<asp:TextBox id="txtSearch" runat="server" CssClass="clsTextBox" ToolTip="Enter Document Name"
														MaxLength="50"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="right">
										<asp:Button id="btnFindNow" CssClass="clsButton" Runat="server" Text="Find Now" ToolTip="Click to find the list of records as per searching criteria"
											CausesValidation="False"></asp:Button></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<asp:datagrid id="dgDocumentTypeList" runat="server" CssClass="clsGrid" ToolTip="Document Type List"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
												<asp:BoundColumn DataField="Code" HeaderText="Code"></asp:BoundColumn>
												<asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="View"></asp:ButtonColumn>
												<asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
											</Columns>
										</asp:datagrid></TD>
									<TD align="right">
										<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" align="right" height="100%">
											<TR>
												<TD vAlign="top" align="right">
													<asp:Button id=btnCloseTop CssClass="clsButton" Runat="server" Text="Close" ToolTip="Back to Previous Page" CausesValidation="False" Visible="<%# mDocumentTypeList.Count>10 %>">
													</asp:Button></TD>
											</TR>
											<TR>
												<TD></TD>
											</TR>
											<TR>
												<TD vAlign="bottom" align="right">
													<asp:Button id="btnClose" tabIndex="0" CssClass="clsButton" Runat="server" Text="Close" ToolTip="Back to Previous Page"
														CausesValidation="False"></asp:Button></TD>
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

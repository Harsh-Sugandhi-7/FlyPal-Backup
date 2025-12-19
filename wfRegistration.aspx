<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfRegistration.aspx.vb" Inherits="Flypal.wfRegistration" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>wfRegistration</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="clstablelistout" id="tblMain" cellSpacing="1" cellPadding="1" width="472"
				border="0">
				<TR>
					<TD colSpan="1" rowSpan="1">
						<TABLE id="tblInner" cellPadding="1" width="560" border="0">
							<TR>
								<TD colSpan="5"><asp:label id="lblHeading" runat="server" CssClass="clstitle1">Company Registration</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="5"><asp:label id="Label3" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Comapny Details </asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="1"><asp:label id="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:label></TD>
								<TD width="100%" colSpan="4">
									<P><asp:textbox id=txtCompName runat="server" CssClass="clsTextBox" Width="100%" Text="<%# mRegistration.CompanyName %>" MaxLength="150" ReadOnly="<%# mRegistration.IsEntryFound %>"></asp:textbox>
										<asp:customvalidator id="cvCompName" runat="server" OnServerValidate="CustomValidate" Display="None"
											ControlToValidate="txtCompName" ErrorMessage="Please enter Company Name."></asp:customvalidator></P>
								</TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label1" runat="server" CssClass="clsLabelAuto">Short Name</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtShortName" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.ShortName %>" MaxLength="25" ReadOnly="<%# mRegistration.IsEntryFound %>">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label2" runat="server" CssClass="clsLabelAuto">Department</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtDeptName" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.DeptName %>" MaxLength="30" ReadOnly="<%# mRegistration.IsEntryFound %>">
									</asp:TextBox>
									<asp:customvalidator id="cvDeptName" runat="server" OnServerValidate="CustomValidate" Display="None"
										ControlToValidate="txtCompName" ErrorMessage="Please enter Company Name."></asp:customvalidator></TD>
							</TR>
							<TR>
								<TD colSpan="5">
									<asp:label id="Label4" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Contact Details </asp:label></TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label5" runat="server" CssClass="clsLabelAuto">Address</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtAddress1" runat="server" CssClass="clsTextBox" Width="100%" Text="<%# mRegistration.Address1 %>" MaxLength="150">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtAddress2" runat="server" CssClass="clsTextBox" Width="100%" Text="<%# mRegistration.Address2 %>" MaxLength="150">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD colSpan="1">
									<asp:label id="Label6" runat="server" CssClass="clsLabelAuto">Phones</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtTel1" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.Tel1 %>" MaxLength="15">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtTel2" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.Tel2 %>" MaxLength="15">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtTel3" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.Tel3 %>" MaxLength="15">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label7" runat="server" CssClass="clsLabelAuto">Fax</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtFax" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.Fax %>" MaxLength="15">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label8" runat="server" CssClass="clsLabelAuto">Email</asp:label></TD>
								<TD colSpan="4">
									<asp:TextBox id="txtEmail" runat="server" CssClass="clsTextBox" Width="100%" Text="<%# mRegistration.Email %>" MaxLength="100">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:label id="Label9" runat="server" CssClass="clsLabelAuto">Base Currency</asp:label></TD>
								<TD>
									<asp:TextBox id="txtBaseCurrency" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.BaseCurrencyName %>" MaxLength="50">
									</asp:TextBox></TD>
								<TD>
									<asp:label id="Label10" runat="server" CssClass="clsLabelAuto">Symbol</asp:label></TD>
								<TD colSpan="2">
									<asp:TextBox id="txtSymbol" runat="server" CssClass="clsTextBox" Text="<%# mRegistration.BaseCurrencySymboll %>" MaxLength="5">
									</asp:TextBox></TD>
							</TR>
							<TR>
								<TD colSpan="3"></TD>
								<TD colSpan="5">
									<asp:button id="btnApply" runat="server" ToolTip="Click to Add New Part" Text="Apply" Cssclass="clsButton"></asp:button>
									<asp:button id="btnClose" runat="server" ToolTip="Click to go back to the previous page" Text="Close"
										Cssclass="clsButton"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

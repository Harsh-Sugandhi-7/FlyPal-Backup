<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptReleaseNoteNo.aspx.vb" Inherits="Flypal.wfrptReleaseNoteNo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Release Note No/Date</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain" border="0">
				<tr>
					<td align="right" colSpan="2">
						<TABLE class="clstablelistin" id="tblInner" border="0">
							<TR>
								<TD colSpan="5">
									<asp:label id="lblTitle" CssClass="clstitle1" Runat="server">Change Release Note No./Date</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="5">
									<asp:validationsummary id="ValidationSummary2" runat="server" CssClass="clsValidationSummary"></asp:validationsummary>
									<asp:CustomValidator id="cvChangeReleaseNoteDate" runat="server" CssClass="clslabelauto" ControlToValidate="txtChangeReleaseNoteDate"
										Display="None" ErrorMessage="Defect  required." OnServerValidate="customvalidate"></asp:CustomValidator></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 14px"></TD>
								<TD><asp:label id="lblCurrentReleaseNoteNo" runat="server" CssClass="clsLabelAuto"> Old Release Note No. </asp:label></TD>
								<TD><asp:textbox id="txtCurrentReleaseNoteNo" runat="server" CssClass="clsTextBoxTextSearch" ReadOnly="True"
										MaxLength="50" BackColor="#E0E0E0"></asp:textbox></TD>
								<TD>
									<asp:label id="lblCurrentReleaseNoteDate" runat="server" CssClass="clsLabelAuto">Old Release Note Date</asp:label></TD>
								<TD>
									<uc1:sicalendar id="txtCurrentReleaseNoteDate" runat="server"></uc1:sicalendar></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 14px">
									<asp:Label id="lblChangeLocation1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label></TD>
								<TD><asp:label id="lblChangeLocation" runat="server" CssClass="clsLabelAuto">New Release Note No.</asp:label></TD>
								<TD><asp:textbox id="txtChangedReleaseNoteNo" runat="server" CssClass="clsTextBoxTextSearch" MaxLength="200"></asp:textbox></TD>
								<TD>
									<asp:label id="lblChangeReleaseNoteDate" runat="server" CssClass="clsLabelAuto">New Release Note Date</asp:label></TD>
								<TD>
									<uc1:sicalendar id="txtChangeReleaseNoteDate" runat="server"></uc1:sicalendar></TD>
							</TR>
							<TR>
								<TD colSpan="5" align="right">
									<TABLE id="Table1" border="0" cellSpacing="1" cellPadding="1">
										<TR>
											<TD><asp:button id="btnOk" Runat="server" CssClass="clsButton" Text="Save" ToolTip="Click to save new Release Note No./Date"></asp:button></TD>
											<TD>
												<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to close Change Release Note No./Date screen"
													CausesValidation="False"></asp:button></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

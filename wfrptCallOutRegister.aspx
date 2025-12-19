<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptCallOutRegister.aspx.vb" Inherits="Flypal.wfrptCallOutRegister" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>CallOut Register</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
		
		<script language="javascript" id="clientEventHandlersJS">
			function openTranDetail()
			{
				str = "wfReports.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openTranDetail1()
			{
				str = "webform1.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openDetail()
			{
				str = "wfDetail.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="5">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">CallOut Register</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblDateRange" runat="server" CssClass="clsLabel">Date Range</asp:label></TD>
									<TD colSpan="4">
										<TABLE cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:DropDownList id="cmbDateRange" runat="server" CssClass="clsComboBox" AutoPostBack="True">
														<asp:ListItem Value="(All)">(All)</asp:ListItem>
														<asp:ListItem Value="Last Week">Last 1 Week</asp:ListItem>
														<asp:ListItem Value="Last Month">Last 1 Month</asp:ListItem>
														<asp:ListItem Value="Last Quarter">Last 1 Quarter</asp:ListItem>
														<asp:ListItem Value="Last Year">Last 1 Year</asp:ListItem>
														<asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
														<asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
													</asp:DropDownList></TD>
												<TD>&nbsp;
													<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>&nbsp;</TD>
												<TD>
													<TABLE id="Table2" cellSpacing="0" border="0">
														<TR>
															<TD></TD>
															<TD>
																<uc1:sicalendar id="txtFromDate" runat="server" Visible="False"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
												<TD>&nbsp;
													<asp:Label id="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>&nbsp;</TD>
												<TD>
													<TABLE id="Table3" cellSpacing="0" border="0">
														<TR>
															<TD></TD>
															<TD>
																<uc1:sicalendar id="txtToDate" runat="server" Visible="False"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Customer</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:Label id="lblSupplier" runat="server" CssClass="clsLabelAuto">Customer</asp:Label></TD>
									<TD colSpan="4">
										<asp:TextBox id="txtVendor" runat="server" CssClass="clstextBoxAuto" ToolTip="Enter Customer"
											MaxLength="50"></asp:TextBox>
										<asp:DropDownList id="cmbCustomer" runat="server" CssClass="clsComboBox3" Visible="False" DataTextField="Name"
											DataValueField="ID"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep4" runat="server" CssClass="clsLabelHeader">Step III. Selection of Call Out No. Report Type</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblCllOutNo" runat="server" CssClass="clsLabelAuto">CallOut No.</asp:label></TD>
									<TD align="left" colSpan="4">
										<TABLE id="Table4" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:DropDownList id="cmbCallOutText" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataTextField="Text"
														DataValueField="Text"></asp:DropDownList></TD>
												<TD>
													<asp:TextBox id="txtCallOutNo" runat="server" CssClass="clsTextBoxMedium" Visible="False"></asp:TextBox>&nbsp;</TD>
												<TD>
													<asp:label id="lblReportType" runat="server" CssClass="clsLabel">Report Type</asp:label></TD>
												<TD>
													<asp:DropDownList id="cmbReportType" runat="server" CssClass="clsComboBox">
														<asp:ListItem Value="0">Portrait Detail</asp:ListItem>
														<asp:ListItem Value="1">Portrait  Summary</asp:ListItem>
														<asp:ListItem Value="2">Landscape Detail</asp:ListItem>
														<asp:ListItem Value="3">Landscape Summary</asp:ListItem>
													</asp:DropDownList></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<asp:label id="lblStep5" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Reg. No.</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblRegNo" runat="server" CssClass="clsLabel">Reg. No.</asp:label></TD>
									<TD align="left" colSpan="1">
										<TABLE id="Table6" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:TextBox id="txtRegNo" runat="server" CssClass="clstextBox" ToolTip="Enter Registration No"
														MaxLength="50"></asp:TextBox></TD>
												<TD>&nbsp;
													<asp:label id="lblModel" runat="server" CssClass="clsLabel" Visible="False">Model</asp:label></TD>
												<TD>
													<asp:TextBox id="txtModel" runat="server" CssClass="clstextBox" Visible="False" ToolTip="Enter Model"
														MaxLength="50"></asp:TextBox></TD>
												<TD>
													<asp:label id="lblSerialNo" runat="server" CssClass="clsLabel" Visible="False">Serial No</asp:label>&nbsp;</TD>
												<TD>
													<asp:TextBox id="txtSerialNo" runat="server" CssClass="clstextBoxDate" Visible="False" ToolTip="Enter Serial No"
														MaxLength="50"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="left"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="3">
										<asp:label id="lblStep6" runat="server" CssClass="clsLabelHeader">Step V. Selection of Job type or Staus</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblStatus" runat="server" CssClass="clsLabel">Status</asp:label></TD>
									<TD colSpan="4">
										<TABLE id="Table1" cellSpacing="0" cellPadding="0">
											<TR>
												<TD align="left">
													<asp:DropDownList id="cmbStatus" runat="server" CssClass="clsComboBox" DataTextField="Name" DataValueField="ID">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Opened</asp:ListItem>
														<asp:ListItem Value="2">Authorized</asp:ListItem>
														<asp:ListItem Value="3">Canceled</asp:ListItem>
													</asp:DropDownList></TD>
												<TD align="left">&nbsp;
													<asp:label id="lblJobType" runat="server" CssClass="clsLabel">Job Type</asp:label></TD>
												<TD align="left">
													<asp:DropDownList id="cmbJobType" runat="server" CssClass="clsComboBox" DataTextField="Name" DataValueField="ID">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Opened</asp:ListItem>
														<asp:ListItem Value="2">Authorized</asp:ListItem>
														<asp:ListItem Value="3">Canceled</asp:ListItem>
													</asp:DropDownList></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblCompPartNo" runat="server" CssClass="clsLabel" Visible="False">Comp Part No</asp:label></TD>
									<TD colSpan="4">
										<TABLE id="Table7" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:TextBox id="txtComppartNo" runat="server" CssClass="clstextBox" Visible="False" ToolTip="Enter Comp Part No"
														MaxLength="50"></asp:TextBox></TD>
												<TD>&nbsp;
													<asp:label id="lblCompSerialNo" runat="server" CssClass="clsLabel" Visible="False">Comp Serial No</asp:label></TD>
												<TD>
													<asp:TextBox id="txtCompSerialNo" runat="server" CssClass="clstextBox" Visible="False" ToolTip="Enter Comp Serial No"
														MaxLength="50"></asp:TextBox></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left" colSpan="3">
										<asp:label id="lblStep7" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="3">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="3">
										<TABLE class="clsTable1" id="Table5">
											<TR>
												<TD colSpan="2">
													<asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:label id="lblCallOutNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
												<TD>
													<asp:label id="lblReportType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:label id="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
												<TD>
													<asp:label id="lblRegNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:label id="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
												<TD>
													<asp:label id="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:label id="lblStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
												<TD>
													<asp:label id="lblJobType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:label id="lblCompPartNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
												<TD>
													<asp:label id="lblCompSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
								<TR>
									<TD align="right" colSpan="2">
										<asp:Panel id="pnlButton" CssClass="clspanel1" Runat="server">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button id="btnCurrentSearchCriteria" tabIndex="0" runat="server" CssClass="clsButtonlong"
															ToolTip="Click to display Current Searching criterias." Text="Current Criteria"></asp:button></TD>
													<TD>
														<asp:button id="btnDisplay" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Display Report"
															Text="Display"></asp:button></TD>
													<TD>
														<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
															Text="Close" CausesValidation="False"></asp:button></TD>
												</TR>
											</TABLE>
										</asp:Panel></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

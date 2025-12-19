<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptQCCallout.aspx.vb" Inherits="Flypal.wfrptQCCallout" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>QCCall Out Reports</title>
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
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<p></p>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="6">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">QCCall Out Register</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblDateRange" runat="server" CssClass="clsLabel">Date Range</asp:label></TD>
									<TD colSpan="5">
										<TABLE cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:DropDownList id="cmbDateRange" runat="server" CssClass="clsComboBox" AutoPostBack="True">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Last Week</asp:ListItem>
														<asp:ListItem Value="2">Last Month</asp:ListItem>
														<asp:ListItem Value="3">Last Quarter</asp:ListItem>
														<asp:ListItem Value="4">Last Year</asp:ListItem>
														<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
														<asp:ListItem Value="6">Between Dates</asp:ListItem>
													</asp:DropDownList></TD>
												<TD>&nbsp;
													<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>&nbsp;</TD>
												<TD>
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
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
													<TABLE id="Table3" cellSpacing="0" cellPadding="0" border="0">
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
									<TD align="left" colSpan="6">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Vendor or Report Type</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblVendor" runat="server" CssClass="clsLabelAuto">Vendor</asp:label></TD>
									<TD align="left">
										<asp:DropDownList id="cmbVendor" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="ID"
											DataTextField="Name"></asp:DropDownList></TD>
									<TD align="left">
										<asp:Label id="lblReportType" runat="server" CssClass="clsLabel" Visible="False">Report Type</asp:Label></TD>
									<TD align="left">
										<asp:DropDownList id="cmbReportType" runat="server" CssClass="clsComboBox" AutoPostBack="True">
											<asp:ListItem Value="0">All</asp:ListItem>
											<asp:ListItem Value="1">Supplier Wise</asp:ListItem>
											<asp:ListItem Value="3">AirCraft Wise</asp:ListItem>
											<asp:ListItem Value="4">Status Wise</asp:ListItem>
										</asp:DropDownList></TD>
									<TD align="left" colSpan="2"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblStep3" runat="server" CssClass="clsLabelHeader">Step III.  Selection of QcCall Out No. or Detail Report</asp:label></TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 27px" align="left">
										<asp:label id="lblType" runat="server" CssClass="clsLabel">Type</asp:label></TD>
									<TD style="HEIGHT: 27px" align="left">
										<asp:DropDownList id="cmbQcCalloutText" runat="server" CssClass="clsComboBox" AutoPostBack="True"
											DataValueField="Text" DataTextField="Text"></asp:DropDownList></TD>
									<TD style="HEIGHT: 27px" align="left">
										<asp:TextBox id="txtQcCalloutNo" runat="server" CssClass="clstextBoxSmall" Visible="False"></asp:TextBox></TD>
									<TD style="HEIGHT: 27px" align="left">
										<asp:CheckBox id="chkDetail" runat="server" CssClass="clsCheckBox" Text="Detailed Report" Checked="True"></asp:CheckBox></TD>
									<TD style="HEIGHT: 27px" align="left" colSpan="2"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV.  Selection of Reg No. or Model</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblRegNo" runat="server" CssClass="clsLabel">Reg No.</asp:label></TD>
									<TD align="left">
										<asp:DropDownList id="cmbRegNo" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="ID"
											DataTextField="RegNo"></asp:DropDownList></TD>
									<TD align="left">
										<asp:label id="lblModel" runat="server" CssClass="clsLabel">Model</asp:label></TD>
									<TD align="left">
										<asp:TextBox id="txtModel" runat="server" CssClass="clsTextBox"></asp:TextBox></TD>
									<TD align="left" colSpan="2"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblStep5" runat="server" CssClass="clsLabelHeader">Step V.  Selection of Place</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblArrivalAt" runat="server" CssClass="clsLabelAuto">Arrival At</asp:label></TD>
									<TD>
										<asp:DropDownList id="cmbArrivalAt" runat="server" CssClass="clsComboBox" DataValueField="mArrivalPlaceList"
											DataTextField="Name"></asp:DropDownList></TD>
									<TD>
										<asp:label id="lblDepartureFrom" runat="server" CssClass="clsLabel">Departure From</asp:label></TD>
									<TD>
										<asp:DropDownList id="cmbDepartureFrom" runat="server" CssClass="clsComboBox" DataValueField="mDepartPlaceList"
											DataTextField="Name"></asp:DropDownList></TD>
									<TD colSpan="2"></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label id="lblStep6" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Job Type</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblJobType" runat="server" CssClass="clsLabelAuto">Job Type</asp:label></TD>
									<TD>
										<asp:DropDownList id="cmbJobType" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="ID"
											DataTextField="Name"></asp:DropDownList></TD>
									<TD></TD>
									<TD colSpan="3">
										<asp:CheckBox id="chkBillable" runat="server" CssClass="clsCheckBox" Text="Billable"></asp:CheckBox></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblStep7" runat="server" CssClass="clsLabelHeader">Step VII. Selection of Status</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblStatus" runat="server" CssClass="clsLabelAuto">Staus</asp:label></TD>
									<TD align="left">
										<asp:DropDownList id="cmbStatus" runat="server" CssClass="clsComboBox" AutoPostBack="True" DataValueField="Name"
											DataTextField="ID"></asp:DropDownList></TD>
									<TD align="left"></TD>
									<TD align="left" colSpan="3"></TD>
								</TR>
								<TR>
									<TD colSpan="6"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblStep8" runat="server" CssClass="clsLabelHeader">Step VIII. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="6">
										<asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="2">
										<asp:label id="lblVendor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblReportType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="2"></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lblRegNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="2"></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="2">
										<asp:label id="lblArrival1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblDeparture1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="2">
										<asp:label id="lblJobType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="4">
										<asp:label id="lblStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
								<TR>
									<TD align="right" colSpan="6">
										<asp:Panel id="pnlButton" CssClass="clspanel1" Runat="server">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button id="btnCurrentSearchCriteria" tabIndex="0" runat="server" CssClass="clsButtonlong"
															Text="Current Criteria" ToolTip="Click to display Current Searching criterias."></asp:button></TD>
													<TD>
														<asp:button id="btnDisplay" tabIndex="0" runat="server" CssClass="clsButton" Text="Display"
															ToolTip="Display Report"></asp:button></TD>
													<TD>
														<asp:button id="btnClose" tabIndex="0" runat="server" CssClass="clsButton" Text="Close" ToolTip="Back to Previous Page"
															CausesValidation="False"></asp:button></TD>
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

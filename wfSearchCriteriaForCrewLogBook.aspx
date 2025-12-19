<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForCrewLogBook.aspx.vb" Inherits="Flypal.wfSearchCriteriaForCrewLogBook" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<HEAD>
		<title>Graph Report</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK    id="MainStyle" type="text/css" rel="stylesheet"> <!-- #include file= "LocalFunction.htm" -->
		<script id="clientEventHandlersJS" language="javascript">
		    function openTranDetail() {
		        str = "wfReports.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		    function openTranDetail1() {
		        str = "webform1.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		    function openDetail() {
		        str = "wfDetail.aspx"
		        window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		    }
		</script>
		<LINK rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
		<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
		<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
		<script type="text/javascript" src="jquery.textchange.min.js"></script>
		<script type="text/javascript">
		    $(document).ready(function () {
		        $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoEmpNoName.aspx?', {
		            width: 275,
		            autoFill: false,
		            matchContains: true,
		            delay: 0
		        });
		        $("#<%=txtCoPilot.ClientID %>").autocomplete('wfAutoEmpNoName.aspx?', {
		            width: 275,
		            autoFill: false,
		            matchContains: true,
		            delay: 0
		        });
		    });
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" rightMargin="0" topMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE id="tblInner" class="clstablelistin">
								<TR>
									<TD colSpan="6">
										<asp:Label id="lbltitle" Runat="server" CssClass="clstitle1">Search Criteria For Crew Log Book</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:ValidationSummary id="Validationsummary2" Runat="server" HeaderText="Fill Up The Following Fields"
											Cssclass="clsValidationSummary"></asp:ValidationSummary>
										<asp:RequiredFieldValidator id="rfvFromDate" runat="server" CssClass="clslabelauto" Display="None" ControlToValidate="txtFromDate"
											ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
										<asp:RequiredFieldValidator id="rfvToDate" runat="server" CssClass="clslabelauto" Display="None" ControlToValidate="txtToDate"
											ErrorMessage="To Date Required"></asp:RequiredFieldValidator></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:label></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD>
										<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label></TD>
									<TD></TD>
									<TD>
										<uc1:sicalendar id="txtFromDate" runat="server"></uc1:sicalendar></TD>
									<TD>
										<asp:Label id="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label></TD>
									<TD>
										<uc1:sicalendar id="txtToDate" runat="server"></uc1:sicalendar></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:label></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="left">
										<asp:Label id="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label></TD>
									<TD align="left"></TD>
									<TD colSpan="3" align="left">
										<asp:DropDownList id="cmbAircraft" runat="server" CssClass="clsComboBox3" DataValueField="ID" DataTextField="RegNo"></asp:DropDownList></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="Label1" runat="server" CssClass="clsLabelHeader">Step III. Selection of  Pilot in Command /Co-Pilot</asp:label></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="left">
										<asp:Label id="lblCrew" runat="server" CssClass="clsLabelAuto">Pilot In Command</asp:Label></TD>
									<TD align="left"></TD>
									<TD colSpan="3" align="left">
										<asp:TextBox  id="txtSearch" runat="server" CssClass="clsComboBox3"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="left">
										<asp:Label  id="Label4" runat="server" CssClass="clsLabelAuto">Co-Pilot</asp:Label></TD>
									<TD align="left"></TD>
									<TD colSpan="3" align="left">
										<asp:TextBox  id="txtCoPilot" runat="server" CssClass="clsComboBox3"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="Label3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Duty Type</asp:label></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="left">
										<asp:label id="lblDutyAs" Runat="server" CssClass="clsLabelAuto">Duty As</asp:label></TD>
									<TD align="left"></TD>
									<TD colSpan="3" align="left">
										<asp:dropdownlist id="cmbDutyAs" runat="server" CssClass="clsComboBox3" DataValueField="ID" DataTextField="DutyType"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label id="Label2" runat="server" CssClass="clsLabelHeader">Step V. Selection of Reference Document </asp:label></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="right"></TD>
									<TD colSpan="4" align="left">
										<asp:CheckBox id="chkLogNo" runat="server" CssClass="clsCheckBox" Checked="True" Text="Log No."></asp:CheckBox>
										<asp:CheckBox id="chkLogPageNo" runat="server" CssClass="clsCheckBox" Text="Log Page No."></asp:CheckBox>
										<asp:CheckBox id="chkFlightNo" runat="server" CssClass="clsCheckBox" Text="Flight No"></asp:CheckBox></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label  id="lblFormat" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Report Format </asp:label></TD>
								</TR>
								<TR>
									<TD align="right"></TD>
									<TD align="right"></TD>
									<TD colSpan="4" align="left">
										<TABLE  id="Table6" border="0" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:radiobutton id="optDetail" runat="server" CssClass="clsRadioButton" Text="Detail" GroupName="grOrientation"></asp:radiobutton></TD>
												<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:radiobutton id="optSummary" runat="server" CssClass="clsRadioButton" Text="Summary" GroupName="grOrientation"></asp:radiobutton></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 22px" colSpan="6" align="left">
										<asp:label id="lblStep4" runat="server" CssClass="clsLabelHeader">Step VII. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label>
										<asp:label id="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label id="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label id="lblPilot1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label  id="lblCopilot" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left"></TD>
									<TD colSpan="5" align="left">
										<asp:label id="lblDutyAs1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="right">
										<asp:Panel id="pnlButton" Runat="server" CssClass="clspanel1">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button id="btnCurrentSearchCriteria" tabIndex="0" runat="server" CssClass="clsButtonlong"
															Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias"></asp:button></TD>
													<TD>
														<asp:button id="btnDisplay" tabIndex="0" runat="server" CssClass="clsButton" Text="Display"
															CausesValidation="False" ToolTip="Click to Display Report"></asp:button></TD>
													<TD>
														<asp:button id="btnClose" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False"
															ToolTip="Click to Close Search Criteria For Crew Log Book screen"></asp:button></TD>
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


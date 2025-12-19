<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptMonthlyTrend.aspx.vb" Inherits="Flypal.wfrptMonthlyTrend" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<head runat ="server" >
		<title>Monthly Trend Search Criteria</title>
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
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="5">
										<asp:Label id="lbltitle" CssClass="clstitle1" Runat="server">Monthly Trend Search Criteria</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="5"></TD>
								</TR>
								<TR>
									<TD colSpan="5">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Year</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblYear" runat="server" CssClass="clsLabelAuto">Year</asp:Label></TD>
									<TD>
										<asp:DropDownList id="cmbYear" runat="server" CssClass="clsComboBox"></asp:DropDownList></TD>
									<TD>
										<asp:Label id="lblTrendType" runat="server" CssClass="clslabelAuto">Trend Type</asp:Label></TD>
									<TD>
										<asp:TextBox id="txtTrendType" runat="server" CssClass="clsTextBoxSmall" MaxLength="3"></asp:TextBox></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Part Number</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:label></TD>
									<TD align="left">
										<asp:DropDownList id="cmbSearch" runat="server" CssClass="clsComboBox" AutoPostBack="True">
											<asp:ListItem Value="0">All</asp:ListItem>
											<asp:ListItem Value="1">Part No.</asp:ListItem>
											<asp:ListItem Value="2">Description</asp:ListItem>
										</asp:DropDownList></TD>
									<TD align="left">
										<asp:Label id="lblFor" runat="server" CssClass="clslabelAuto" Visible="False">For</asp:Label></TD>
									<TD align="left">
										<asp:TextBox id="txtSearchFor" runat="server" CssClass="clstextBox" Visible="False"></asp:TextBox></TD>
									<TD align="right" colSpan="1">
										<asp:button id="btnFindNow" tabIndex="0" runat="server" CssClass="clsButton" Text="Find Now"></asp:button></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:datagrid id="dgPartSearch" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
											Width="100%" Cssclass="clsGrid" OnPageIndexChanged="NewPage" PageSize="25">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Part Number">
													<HeaderStyle Wrap="False" ForeColor="#FFFFFF"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
													<HeaderStyle ForeColor="#FFFFFF"></HeaderStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="5">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:label></TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 17px" align="left" colSpan="2">
										<asp:label id="lblDispYear" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD style="HEIGHT: 17px" align="left" colSpan="3">
										<asp:label id="lblDispTrendType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="2">
										<asp:label id="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD align="left" colSpan="3">
										<asp:label id="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
								<TR>
									<TD align="right" colSpan="5">
										<asp:Panel id="pnlButton" CssClass="clspanel1" Runat="server">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button id="btnCurrentSearchCriteria" tabIndex="0" runat="server" CssClass="clsButtonlong"
															Text="Current Criteria" ToolTip="Click to display Current Searching criterias."></asp:button></TD>
													<TD>
														<asp:button id="btnDisplay" tabIndex="0" runat="server" CssClass="clsButton" Text="Display"
															ToolTip="Click to Display Report"></asp:button></TD>
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

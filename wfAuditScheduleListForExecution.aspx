<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfAuditScheduleListForExecution.aspx.vb" Inherits="Flypal.wfAuditScheduleListForExecution" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Audit Schedule List For Compliance</title>
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
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td><asp:panel id="pnlmain" Runat="server" CssClass="clspanel1">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="4">
										<asp:Label id="lblTitle" CssClass="clstitle1" Runat="server">Audit Schedule List For Compliance</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<TABLE id="Table4">
											<TR>
												<TD align="right">
													<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto">Compliance Date </asp:Label></TD>
												<TD align="right">
													<uc1:sicalendar id="txtAuditDate" runat="server"></uc1:sicalendar></TD>
											</TR>
										</TABLE>
									</TD>
									<TD align="right">
										<TABLE id="Table1" cellSpacing="0">
											<TR>
											</TR>
										</TABLE>
										<TABLE id="Table3">
											<TR>
												<TD align="right">
													<asp:button id="btnFindNow" runat="server" Text="Find Now" ToolTip="Click to find Audit Schedule List as per searching criteria"
														Cssclass="clsButton"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="2">
										<asp:label id="lblResult" runat="server" Cssclass="clsLabelHeader"></asp:label></TD>
									<TD align="right" colSpan="2">
										<TABLE id="Table5" cellSpacing="1" cellPadding="1" border="0">
											<TR>
												<TD>
													<asp:button id="btnBackTop" tabIndex="0" runat="server" CssClass="clsButton" Text="Back" ToolTip="Click to go back to the previous page"
														CausesValidation="False"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:datagrid id="dgAuditScheduleList" runat="server" Cssclass="clsGrid" AllowSorting="True" AutoGenerateColumns="False"
											PageSize="25" AllowPaging="True">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="ScheduleDateFormatted" HeaderText="Schedule Date">
													<HeaderStyle ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
													<FooterStyle Wrap="False"></FooterStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="AuditText" SortExpression="AuditText" HeaderText="Audit No.">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Audit Type">
													<HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Reference No.">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="NextAuditDateFormatted" HeaderText="Next Audit Date">
													<ItemStyle Wrap="False"></ItemStyle>
													<FooterStyle Wrap="False"></FooterStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="Note" SortExpression="Note" HeaderText="Note">
													<HeaderStyle ForeColor="White"></HeaderStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle VerticalAlign="Middle" NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="4">
										<TABLE id="Table2" cellSpacing="1" cellPadding="1" border="0">
											<TR>
												<TD>
													<asp:button id="btnBack" tabIndex="0" runat="server" CssClass="clsButton" Text="Back" ToolTip="Click to go back to the previous page"
														CausesValidation="False"></asp:button></TD>
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

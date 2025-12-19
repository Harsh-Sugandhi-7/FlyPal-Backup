<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfnWOCallOutJobList.aspx.vb" Inherits="Flypal.wfnWOCallOutJobList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>Call Out Job List</title>
		<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
		<script language="javascript">
		
		function openledgersame(FileName)
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
	<body bottomMargin="5" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="clstablelistout" id="tblMain" border="0">
				<TR>
					<TD><asp:panel id="pnlMain" Runat="server" CssClass="clsPanel1">
							<TABLE class="clstablelistin" id="tblInner" border="0">
								<TR>
									<TD colSpan="4">
										<asp:label id="lblList" runat="server" CssClass="clstitle1">List Of CallOut </asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:ValidationSummary id="Validationsummary" Runat="server" HeaderText="Fill Up The Following Information"
											Cssclass="clsValidationSummary"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<TABLE id="Table3">
											<TR>
												<TD>
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label></TD>
												<TD>
													<P>
														<asp:DropDownList id="cmbSearch" runat="server" CssClass="clsComboBox" AutoPostBack="True">
															<asp:ListItem Value="0">All</asp:ListItem>
															<asp:ListItem Value="1">Date</asp:ListItem>
															<asp:ListItem Value="2">CallOut</asp:ListItem>
															<asp:ListItem Value="3">Customer</asp:ListItem>
															<asp:ListItem Value="5">Status</asp:ListItem>
														</asp:DropDownList></P>
												</TD>
												<TD>
													<asp:DropDownList id="cmbDate" runat="server" CssClass="clsComboBox" AutoPostBack="True" Visible="False">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Last Week</asp:ListItem>
														<asp:ListItem Value="2">Last Month</asp:ListItem>
														<asp:ListItem Value="3">Last Quarter</asp:ListItem>
														<asp:ListItem Value="4">Last Year</asp:ListItem>
														<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
														<asp:ListItem Value="6">Between Dates</asp:ListItem>
													</asp:DropDownList>
													<asp:DropDownList id="cmbCallOutText" runat="server" CssClass="clsComboBox" AutoPostBack="True" Visible="False"
														DataTextField="Text" DataValueField="Text"></asp:DropDownList>
													<asp:TextBox id="txtName" runat="server" CssClass="clsTextBox" Visible="False"></asp:TextBox>
													<asp:DropDownList id="cmbStatus" runat="server" CssClass="clsComboBox" Visible="False">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Opened</asp:ListItem>
														<asp:ListItem Value="2">In-Process</asp:ListItem>
														<asp:ListItem Value="3">Complete</asp:ListItem>
														<asp:ListItem Value="4">Canceled</asp:ListItem>
													</asp:DropDownList></TD>
												<TD>
													<asp:Label id="lblNo" runat="server" CssClass="clsLabelAuto" Visible="False">No.</asp:Label></TD>
												<TD>
													<asp:TextBox id="txtNo" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="4"></asp:TextBox></TD>
												<TD>
													<asp:Label id="lblFromDate" CssClass="clsLabel" Runat="server" Visible="False">From Date </asp:Label></TD>
												<TD>
													<uc1:sicalendar id="txtFromDate" runat="server"></uc1:sicalendar></TD>
												<TD>
													<asp:Label id="lblToDate" CssClass="clsLabel" Runat="server" Visible="False">To Date </asp:Label></TD>
												<TD></TD>
												<TD>
													<uc1:sicalendar id="txtToDate" runat="server"></uc1:sicalendar></TD>
											</TR>
										</TABLE>
										&nbsp;
									</TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 15px" colSpan="4">
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Select CallOut from the List to see its jobs below</asp:Label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
									<TD colSpan="2">
										<P align="right">
											<TABLE id="Table1">
												<TR>
													<TD align="right">
														<asp:Button id="btnFindNow" runat="server" CssClass="clsButton" Text="Find Now" ToolTip="Click to Find"></asp:Button></TD>
												</TR>
											</TABLE>
										</P>
									</TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<asp:datagrid id="dgCallOut" runat="server" CssClass="clsGrid" ToolTip="QCCallOut List" AutoGenerateColumns="False"
											AllowSorting="True">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Date" HeaderText="Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="CalloutNumber" HeaderText="Number">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="CustomerName" HeaderText="Customer"></asp:BoundColumn>
												<asp:BoundColumn DataField="MachineModelNo" HeaderText="Model "></asp:BoundColumn>
												<asp:BoundColumn DataField="MachineSerialNo" HeaderText="Serial No"></asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<TABLE id="Table6" cellSpacing="1" cellPadding="1" border="0">
											<TR>
												<TD align="right">
													<asp:label id="lblCallOutJobs" runat="server" CssClass="clsLabelHeader" DESIGNTIMEDRAGDROP="116">Call Out Job(s)</asp:label></TD>
												<TD align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:CheckBox id="chkAll" runat="server" CssClass="clsLabel" AutoPostBack="True" Visible="False"
														Text="   All" ToolTip="Check to select all the records"></asp:CheckBox></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<asp:datagrid id="dgCallOutJobs" runat="server" CssClass="clsGrid" ToolTip="QCCallOut List" AutoGenerateColumns="False"
											AllowSorting="True">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Select">
													<ItemTemplate>
														<asp:CheckBox id=chkSelect runat="server" CssClass="clscheckbox" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'>
														</asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="JobTypeName" HeaderText="Type"></asp:BoundColumn>
												<asp:BoundColumn DataField="JobDescription" HeaderText="Name"></asp:BoundColumn>
												<asp:BoundColumn DataField="StartDateFormatted" HeaderText="Startdate"></asp:BoundColumn>
												<asp:BoundColumn DataField="PercentComplete" HeaderText="% Complete">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="EstimatedDateFormatted" HeaderText="Estd. Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="EstimatedHours" HeaderText="Estd. Hours"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="4">
										<TABLE id="Table3" border="0">
											<TR>
												<TD>
													<asp:button id="btnDone" runat="server" CssClass="clsButton" Visible="False" Text="Done" ToolTip="Click to go to previous page"
														CausesValidation="False"></asp:button></TD>
												<TD>
													<asp:button id="btnBack" runat="server" CssClass="clsButton" Text="Back"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

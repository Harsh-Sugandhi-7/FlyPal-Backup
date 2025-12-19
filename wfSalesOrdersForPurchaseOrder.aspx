<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfSalesOrdersForPurchaseOrder.aspx.vb" Inherits="Flypal.wfSalesOrdersForPurchaseOrder" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD runat ="server" >
		<title>List Of Sales Order</title>
        <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
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
			<TABLE class="clstablelistout" id="tblMain">
				<TR>
					<TD><asp:panel id="pnlMain" CssClass="clsPanel1" Runat="server">
							<TABLE class="clstablelistin" id="tblInner">
								<TR>
									<TD colSpan="4">
										<asp:label id="lblList" runat="server" CssClass="clstitle1">List Of Sales Order</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:ValidationSummary id="Validationsummary" Runat="server" HeaderText="Fill Up The Following Information"
											Cssclass="clsValidationSummary"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD colSpan="3">
										<TABLE id="Table2">
											<TR>
												<TD>
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabel" Width="48px" Height="10px">Search</asp:Label></TD>
												<TD>
													<TABLE id="Table4">
														<TR>
															<TD>
																<asp:dropdownlist id="cmbSearch" runat="server" CssClass="clsComboBox" Width="170px" AutoPostBack="True">
																	<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
																	<asp:ListItem Value="1">Date</asp:ListItem>
																	<asp:ListItem Value="2">Sales Order</asp:ListItem>
																	<asp:ListItem Value="3">Part No</asp:ListItem>
																	<asp:ListItem Value="4">Vendor</asp:ListItem>
																	<asp:ListItem Value="5">Quotation</asp:ListItem>
																</asp:dropdownlist></TD>
															<TD>
																<asp:Label id="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label></TD>
															<TD>
																<P>
																	<asp:DropDownList id="cmbDate" runat="server" CssClass="clsComboBox1" AutoPostBack="True" Visible="False">
																		<asp:ListItem Value="0">(All)</asp:ListItem>
																		<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																		<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																		<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																		<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																		<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																		<asp:ListItem Value="6">Between Dates</asp:ListItem>
																	</asp:DropDownList>
																	<asp:DropDownList id="cmbSalesOrderText" runat="server" CssClass="clsComboBox1" AutoPostBack="True"
																		Visible="False" DataTextField="Text" DataValueField="Text"></asp:DropDownList>
																	<asp:DropDownList id="cmbQuotationText" runat="server" CssClass="clsComboBox1" AutoPostBack="True"
																		Visible="False" DataTextField="Text" DataValueField="Text"></asp:DropDownList>
																	<asp:TextBox id="txtName" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="100"></asp:TextBox></P>
															</TD>
															<TD>
																<asp:Label id="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label></TD>
															<TD align="left">
																<asp:TextBox id="txtNo" runat="server" CssClass="clsTextBox" Visible="False" MaxLength="4"></asp:TextBox></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">
													<asp:Label id="lblFromDate" Runat="server" CssClass="clsLabel" Visible="False">From Date </asp:Label></TD>
												<TD>
													<TABLE id="Table5">
														<TR>
															<TD align="left">
																<uc1:sicalendar id="txtFromDate" runat="server" Visible="False"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
												<TD align="right">&nbsp;&nbsp;
													<asp:Label id="lblToDate" Runat="server" CssClass="clsLabel" Width="78px" Visible="False" DESIGNTIMEDRAGDROP="19">To Date </asp:Label></TD>
												<TD>
													<TABLE id="Table6">
														<TR>
															<TD align="left">
																<uc1:sicalendar id="txtToDate" runat="server" Visible="False"></uc1:sicalendar></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
										&nbsp;
									</TD>
								</TR>
								<TR>
									<TD colSpan="4">
										<asp:Label id="lblInfo" runat="server" CssClass="clsLabelAuto">Select Sales Order from the List to see its Items below</asp:Label></TD>
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
										<asp:datagrid id="dgSalesOrderList" runat="server" CssClass="clsGrid" DESIGNTIMEDRAGDROP="139"
											AutoGenerateColumns="False">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="FromItemParentID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="FromDateFormatted" HeaderText="Date"></asp:BoundColumn>
												<asp:BoundColumn DataField="FromTextNo" HeaderText="Number"></asp:BoundColumn>
												<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
												<asp:BoundColumn DataField="UserName" HeaderText="Created By"></asp:BoundColumn>
												<asp:BoundColumn DataField="AuthorizedBy" HeaderText="Authorized By"></asp:BoundColumn>
												<asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<asp:label id="lblCallOutJobs" runat="server" CssClass="clsLabelHeader">Sales Order Item(s)</asp:label></TD>
								</TR>
								<TR>
									<TD align="left" colSpan="4">
										<asp:datagrid id="dgSalesOrderItems" runat="server" CssClass="clsGrid" ToolTip="QCCallOut List"
											AutoGenerateColumns="False" AllowSorting="True">
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
												<asp:BoundColumn DataField="ItemName" HeaderText="Part No">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ItemDescription" HeaderText="Description"></asp:BoundColumn>
												<asp:BoundColumn DataField="Qty" HeaderText="Qty">
													<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="right" colSpan="4">
										<TABLE id="Table3">
											<TR>
												<TD>
													<asp:button id="btnDone" runat="server" CssClass="clsButton" Text="Done" ToolTip="Click to go to previous page"
														CausesValidation="False"></asp:button></TD>
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
